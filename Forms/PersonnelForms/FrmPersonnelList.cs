using Microsoft.EntityFrameworkCore;
using QuanLyCBCS.Data;
using QuanLyCBCS.Enums;
using QuanLyCBCS.Forms.MissionForms;
using QuanLyCBCS.Forms.SystemForms;
using QuanLyCBCS.Forms.UserForms;
using QuanLyCBCS.Helpers;
using QuanLyCBCS.Models;
using QuanLyCBCS.Services;
using System.Diagnostics;
using QuanLyCBCS.Forms.CommonForms;
using System.IO;
using System.Linq;

namespace QuanLyCBCS.Forms.PersonnelForms
{
    public partial class FrmPersonnelList : Form
    {
        private readonly PersonnelService _service = new();
        private List<Personnel> _allPersonnel = new();

        private readonly System.Windows.Forms.Timer _idleTimer =
            new();

        private readonly System.Windows.Forms.Timer _clockTimer =
            new();

        private int _selectedPersonnelId;

        private bool _filterLoaded;
        private bool _isHostedInHome;

        // Được FrmHome gọi trước khi nhúng form vào tab Quản lý CBCS.
        public void SetHostedInHome(bool hosted)
        {
            _isHostedInHome = hosted;
        }
        private ToolTip _toolTip = new();

        private FrmMissionDashboard?
           _missionDashboard;

        private FrmNotificationCenter? _notificationForm;
        private readonly NotificationService _notificationService;

        public FrmPersonnelList()
        {
            InitializeComponent();

            dgvPersonnel.AllowDrop = true;

            dgvPersonnel.DragEnter += dgvPersonnel_DragEnter;
            dgvPersonnel.DragDrop += dgvPersonnel_DragDrop;

            _service = new PersonnelService();

            ApplyPermission();

            RegisterActivityControl(this);

            ConfigureButtons();

            SetupSearchBox();

            ConfigureTimers();

            txtSearch.TextChanged += (_, _) => SearchPersonnel();
            txtSearch.KeyDown += txtSearch_KeyDown;

            dgvPersonnel.RowPrePaint += dgvPersonnel_RowPrePaint;

            this.Shown +=
                FrmPersonnelList_Shown;

            _notificationService =
                new NotificationService();

            ApplyCompactPersonnelLayout();
            this.Resize += (_, _) => ApplyCompactPersonnelLayout();
        }

        private void ApplyPermission()
        {
            btnAdd.Visible =
                CurrentUser.HasPermission("Personnel.Create");

            btnEdit.Visible =
                CurrentUser.HasPermission("Personnel.Update");

            btnDelete.Visible =
                CurrentUser.HasPermission("Personnel.Delete");

            btnUsers.Visible =
                CurrentUser.HasPermission("User.Manage");

            btnAuditLog.Visible =
                CurrentUser.HasPermission("Audit.View");

            btnMission.Visible =
                CurrentUser.HasPermission("Mission.Manage");

            btnBackup.Visible =
                CurrentUser.HasPermission("Database.Backup");

            btnRestore.Visible =
                CurrentUser.HasPermission("Database.Restore");

            btnPermission.Visible =
                PermissionHelper.CanManagePermission;

            btnStatistics.Visible = true;

            btnNotification.Visible = true;

            btnChangePassword.Visible = true;

            btnAdminDashboard.Visible =
                CurrentUser.HasPermission("Dashboard.View");
            btnImportExcel.Visible =
                CurrentUser.HasPermission("Personnel.Create");
        }

        protected override bool ProcessCmdKey(
            ref Message msg,
            Keys keyData)
        {
            IdleTimeoutHelper.ResetTimer();

            return base.ProcessCmdKey(
                ref msg,
                keyData);
        }

        protected override void OnMouseMove(
            MouseEventArgs e)
        {
            IdleTimeoutHelper.ResetTimer();

            base.OnMouseMove(e);
        }

        private void StyleButton(Button btn)
        {
            btn.FlatStyle =
                FlatStyle.Flat;

            btn.FlatAppearance.BorderSize = 0;

            btn.ForeColor =
                Color.White;

            btn.BackColor =
                ThemeHelper.PrimaryColor;

            btn.Font =
                new Font(
                    "Segoe UI",
                    9,
                    FontStyle.Bold);

            RoundedControlHelper.SetRounded(
                btn,
                12);
        }

        private void StyleButtons(
            params Button[] buttons)
        {
            foreach (var btn in buttons)
            {
                StyleButton(btn);
            }
        }

        private void ConfigureButtons()
        {
            StyleButtons(
                btnAdd,
                btnEdit,
                btnDelete,
                btnUsers,
                btnAuditLog,
                btnChangePassword,
                btnMission,
                btnStatistics,
                btnNotification,
                btnAdminDashboard,
                btnPermission,
                btnReport
            );

            RegisterHoverEffect(
                btnAdd,
                btnEdit,
                btnDelete,
                btnUsers,
                btnAuditLog,
                btnChangePassword,
                btnMission,
                btnStatistics,
                btnNotification,
                btnAdminDashboard,
                btnPermission,
                btnReport
            );
        }

        private void ConfigureTimers()
        {
            _idleTimer.Interval = 1000;
            _idleTimer.Tick += IdleTimer_Tick;
            _idleTimer.Start();

            _clockTimer.Interval = 1000;
            _clockTimer.Tick += TimerClock_Tick;
            _clockTimer.Start();
        }

        private void SetupSearchBox()
        {
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.Font = new Font(
                "Segoe UI",
                11F);

            txtSearch.BackColor =
                Color.White;

            txtSearch.ForeColor =
                ThemeHelper.PrimaryColor;
        }

        private void Button_MouseEnter(
            object? sender,
            EventArgs e)
        {
            if (sender is Button btn)
            {
                btn.BackColor =
                    ThemeHelper.SecondaryColor;
            }
        }

        private void RegisterHoverEffect(
            params Button[] buttons)
        {
            foreach (var btn in buttons)
            {
                btn.MouseEnter += Button_MouseEnter;
                btn.MouseLeave += Button_MouseLeave;
            }
        }

        private void Button_MouseLeave(
            object? sender,
            EventArgs e)
        {
            if (sender is Button btn)
            {
                btn.BackColor =
                    ThemeHelper.PrimaryColor;
            }
        }

        private void AddSttColumn()
        {
            if (dgvPersonnel.Columns["STT"] == null)
            {
                DataGridViewTextBoxColumn sttColumn =
                    new DataGridViewTextBoxColumn();

                sttColumn.Name = "STT";
                sttColumn.HeaderText = "STT";
                sttColumn.Width = 50;
                sttColumn.ReadOnly = true;

                dgvPersonnel.Columns.Insert(
                    0,
                    sttColumn);
            }

            dgvPersonnel.Columns["STT"].Visible = true;
            dgvPersonnel.Columns["STT"].Frozen = true;

            for (int i = 0;
                 i < dgvPersonnel.Rows.Count;
                 i++)
            {
                dgvPersonnel.Rows[i]
                    .Cells["STT"]
                    .Value = i + 1;
            }
        }

        private async Task LoadPersonnelAsync()
        {
            try
            {
                _allPersonnel = await _service.GetAllAsync();

                // Nạp lại danh sách lựa chọn sau mỗi lần tải dữ liệu.
                LoadFilterData();

                // Hiển thị theo các điều kiện tìm kiếm và lọc hiện tại.
                ApplyFilter();
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                dgvPersonnel.DataSource = null;

                _allPersonnel.Clear();

                UpdateRecordCount(0);
            }
        }

        private void UpdateRecordCount(
            int currentCount)
        {
            lblRecordCount.Text =
                $"📋 Hiển thị: {currentCount}/{_allPersonnel.Count} CBCS";
        }

        private void SearchPersonnel()
        {
            if (!_filterLoaded)
                return;

            ApplyFilter();
        }

        private void CustomizeDataGridView()
        {
            DataGridViewHelper.ApplyModernStyle(
                dgvPersonnel);

            DataGridViewHelper.ConfigureColumnHeaders(
                dgvPersonnel);

            DataGridViewHelper.ConfigureColumnWidths(
                dgvPersonnel);

            DataGridViewHelper.HideTechnicalColumns(
                dgvPersonnel);

            DataGridViewHelper.ConfigureDateFormats(
                dgvPersonnel);

            DataGridViewHelper.ConfigureVisibleColumns(
                dgvPersonnel);

            dgvPersonnel.Columns["CCCD"]
                .DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            dgvPersonnel.Columns["SoDienThoai"]
                .DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            dgvPersonnel.ColumnHeadersHeight = 50;

            dgvPersonnel.Columns["QueQuanMoi"].Width = 220;
            dgvPersonnel.Columns["HKTTMoi"].Width = 170;
        }

        private void SetupRoundedControls()
        {
            RoundedControlHelper.SetRounded(
                pnlTongCBCS,
                20);

            RoundedControlHelper.SetRounded(
                pnlChienSi,
                20);

            RoundedControlHelper.SetRounded(
                pnlCanBo,
                20);

            RoundedControlHelper.SetRounded(
                pnlDangVien,
                20);

            RoundedControlHelper.SetRounded(
                pnlThongKe,
                20);

            RoundedControlHelper.SetRounded(
                pnlSearch,
                15);

            RoundedControlHelper.SetRounded(
                btnLogout,
                12);
        }

        private void SetupCurrentUser()
        {
            IdleTimeoutHelper.ResetTimer();

            lblCurrentUser.Text =
                $"👤 {CurrentUser.Username} ({CurrentUser.Role})";

            Text =
                $"Quản lý CBCS - {CurrentUser.Role}";
        }

        private void SetupDataGridView()
        {
            DataGridViewHelper
                .ApplyModernStyle(
                    dgvPersonnel);

            DataGridViewHelper
                .EnableDoubleBuffer(
                    dgvPersonnel);
        }

        private void SetupStatusBar()
        {
            statusStrip1.BackColor =
                ThemeHelper.PrimaryColor;

            statusStrip1.ForeColor =
                Color.White;
        }

        private async void FrmPersonnelList_Load(
            object? sender,
            EventArgs e)
        {
            SetupRoundedControls();

            SetupCurrentUser();

            SetupDataGridView();

            SetupStatusBar();

            SetupTooltips();

            // Chỉ tải danh sách CBCS nếu có quyền
            if (CurrentUser.HasPermission("Personnel.View"))
            {
                await LoadPersonnelAsync();
            }
            else
            {
                dgvPersonnel.DataSource = null;

                UpdateRecordCount(0);

                MessageBox.Show(
                    "Bạn không có quyền xem danh sách CBCS.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            // Dashboard chỉ tải nếu có quyền
            if (PermissionHelper.CanViewStatistics)
            {
                await LoadDashboardAsync();
            }

            await LoadNotificationBadgeAsync();

            // Chỉ tự mở dashboard khi FrmPersonnelList chạy độc lập.
            // Khi được nhúng trong FrmHome, không được bật hộp thoại ngoài ý muốn.
            if (!_isHostedInHome && PermissionHelper.CanViewStatistics)
            {
                BeginInvoke(() =>
                {
                    using FrmAdminDashboard frm =
                        new FrmAdminDashboard();

                    frm.ShowDialog();
                });
            }
        }

        private void dgvPersonnel_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            _selectedPersonnelId =
                Convert.ToInt32(
                    dgvPersonnel
                    .Rows[e.RowIndex]
                    .Cells["PersonnelID"]
                    .Value);
        }

        private void txtSearch_TextChanged(
            object? sender,
            EventArgs e)
        {
            SearchPersonnel();
        }

        private async void btnRefresh_Click(
            object sender,
            EventArgs e)
        {
            txtSearch.Clear();

            await LoadPersonnelAsync();
        }

        private async void btnDelete_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                if (_selectedPersonnelId == 0)
                {
                    MessageBox.Show(
                        "Vui lòng chọn CBCS cần xóa.");

                    return;
                }

                Personnel? personnel =
                    await _service.GetByIdAsync(
                        _selectedPersonnelId);

                if (personnel == null)
                {
                    MessageBox.Show(
                        "CBCS không tồn tại.");

                    return;
                }

                DialogResult result =
                    MessageBox.Show(
                        $"Bạn có chắc muốn xóa CBCS\n\n{personnel.HoTen} ?",
                        "Xác nhận",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                {
                    return;
                }

                string hoTen =
                    personnel.HoTen;

                int personnelId =
                    personnel.PersonnelID;

                await _service.DeleteAsync(personnelId);

                await AuditHelper.LogAsync(
                    AuditAction.DeletePersonnel,
                    $"Đã xóa CBCS '{hoTen}' (ID: {personnelId})");

                PersonnelHistoryService historyService =
                    new PersonnelHistoryService();

                await historyService.AddHistoryAsync(
                    personnelId,
                    hoTen,
                    "DELETE_PERSONNEL",
                    $"Đã xóa hồ sơ CBCS '{hoTen}'");

                _selectedPersonnelId = 0;

                await LoadPersonnelAsync();

                await LoadDashboardAsync();

                MessageBox.Show(
                    "Xóa CBCS thành công.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Không có quyền",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async void btnAdd_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                FrmPersonnelDetail frm =
                    new FrmPersonnelDetail(FormMode.Add);

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    await LoadPersonnelAsync();
                    await LoadDashboardAsync();
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Không có quyền",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async void btnEdit_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                if (_selectedPersonnelId == 0)
                {
                    MessageBox.Show(
                        "Vui lòng chọn CBCS cần chỉnh sửa.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return;
                }

                FrmPersonnelDetail frm =
                    new FrmPersonnelDetail(
                        FormMode.Edit,
                        _selectedPersonnelId);

                if (frm.ShowDialog() ==
                    DialogResult.OK)
                {
                    await LoadPersonnelAsync();

                    await LoadDashboardAsync();
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Không có quyền",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async void dgvPersonnel_CellDoubleClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            try
            {
                if (_selectedPersonnelId == 0)
                    return;

                FrmPersonnelDetail frm =
                    new FrmPersonnelDetail(
                        FormMode.Edit,
                        _selectedPersonnelId);

                if (frm.ShowDialog() ==
                    DialogResult.OK)
                {
                    await LoadPersonnelAsync();
                    await LoadDashboardAsync();
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Không có quyền",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private async void btnExport_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                SaveFileDialog saveDialog =
                    new SaveFileDialog();

                saveDialog.Filter =
                    "Excel File (*.xlsx)|*.xlsx";

                saveDialog.FileName =
                    $"DanhSachCBCS_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                if (saveDialog.ShowDialog()
                    != DialogResult.OK)
                {
                    return;
                }

                var list =
                    await _service.GetAllAsync();

                ExcelExportService excel =
                    new ExcelExportService();

                excel.ExportPersonnel(
                    list,
                    saveDialog.FileName);

                await AuditHelper.LogAsync(
                    AuditAction.ExportExcel,
                    $"Xuất danh sách CBCS ra file '{Path.GetFileName(saveDialog.FileName)}'");

                MessageBox.Show(
                    "Xuất Excel thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                Process.Start(
                    new ProcessStartInfo
                    {
                        FileName =
                            saveDialog.FileName,
                        UseShellExecute = true
                    });
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ApplyDashboardLabelColor()
        {
            Control[] controls =
            {
                lblTongValue,
                lblChienSiValue,
                lblCanBoValue,
                lblDangVienValue,
                lblThongke,
                lblDonViDongNhat,
                lblNam,
                lblNu,
                lblTuoiTrungBinh,
                label1,
                label2,
                label3,
                label4
            };

            foreach (var c in controls)
            {
                c.ForeColor = Color.White;
            }
        }

        private async Task LoadDashboardAsync()
        {
            PermissionHelper.Require("Dashboard.View");

            ApplyDashboardLabelColor();

            lblTongValue.Text =
                (await _service
                    .GetTotalCountAsync())
                .ToString();

            lblChienSiValue.Text =
                (await _service
                    .GetChienSiCountAsync())
                .ToString();

            lblCanBoValue.Text =
                (await _service
                    .GetCanBoCountAsync())
                .ToString();

            lblDangVienValue.Text =
                (await _service
                    .GetDangVienCountAsync())
                .ToString();

            var dashboard =
                await _service.GetPersonnelDashboardAsync();

            lblNam.Text =
                dashboard.Nam.ToString();

            lblNu.Text =
                dashboard.Nu.ToString();

            lblTuoiTrungBinh.Text =
                $"{dashboard.TuoiTrungBinh:0.0}";

            lblDonViDongNhat.Text =
                dashboard.DonViDongNhat;

            DashboardHelper.ApplyPanelColor(
                pnlTongCBCS,
                ThemeHelper.AccentColor);

            DashboardHelper.ApplyPanelColor(
                pnlChienSi,
                ThemeHelper.DashboardBlue);

            DashboardHelper.ApplyPanelColor(
                pnlCanBo,
                ThemeHelper.DashboardGreen);

            DashboardHelper.ApplyPanelColor(
                pnlDangVien,
                ThemeHelper.DashboardPurple);

            DashboardHelper.ApplyPanelColor(
                pnlThongKe,
                ThemeHelper.PrimaryColor);
        }

        private async void btnBackup_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                string backupFolder =
                    @"E:\QuanLyCBCS_Backup";

                if (!Directory.Exists(
                    backupFolder))
                {
                    Directory.CreateDirectory(
                        backupFolder);
                }

                string backupFilePath =
                    Path.Combine(
                        backupFolder,
                        $"CBCS_{DateTime.Now:yyyyMMdd_HHmmss}.bak");

                using ApplicationDbContext db =
                    new ApplicationDbContext();

                DatabaseBackupService backupService =
                    new DatabaseBackupService(
                        db.Database
                          .GetDbConnection()
                          .ConnectionString);

                await backupService
                    .BackupDatabaseAsync(
                        backupFilePath);

                MessageBox.Show(
                    $"Sao lưu thành công!\n\n{backupFilePath}",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Không có quyền",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async void btnRestore_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                OpenFileDialog openDialog =
                    new OpenFileDialog();

                openDialog.Filter =
                    "Backup File (*.bak)|*.bak";

                if (openDialog.ShowDialog()
                    != DialogResult.OK)
                {
                    return;
                }

                DialogResult result =
                    MessageBox.Show(
                        "Restore sẽ ghi đè toàn bộ dữ liệu hiện tại.\n\nTiếp tục?",
                        "Xác nhận",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                {
                    return;
                }

                using ApplicationDbContext db =
                    new ApplicationDbContext();

                DatabaseRestoreService
                    restoreService =
                        new DatabaseRestoreService(
                            db.Database
                              .GetDbConnection()
                              .ConnectionString);

                await restoreService
                    .RestoreDatabaseAsync(
                        openDialog.FileName);

                await LoadPersonnelAsync();
                await LoadDashboardAsync();

                MessageBox.Show(
                    "Restore thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        private async void btnLogout_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                // Ghi nhật ký đăng xuất
                await AuditHelper.LogAsync(
                    AuditAction.Logout,
                    "Đăng xuất hệ thống");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Không thể ghi nhật ký đăng xuất: {ex.Message}",
                    "Cảnh báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            finally
            {
                // Đóng Dashboard công tác nếu đang mở
                if (_missionDashboard != null)
                {
                    if (!_missionDashboard.IsDisposed)
                    {
                        _missionDashboard.Close();
                    }

                    _missionDashboard = null;
                }

                // Đóng Trung tâm thông báo nếu đang mở
                if (_notificationForm != null)
                {
                    if (!_notificationForm.IsDisposed)
                    {
                        _notificationForm.Close();
                    }

                    _notificationForm = null;
                }

                // Đóng màn hình quản lý hồ sơ
                this.Close();
            }
        }


        private void btnUsers_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                FrmUserList frm =
                    new FrmUserList();

                frm.ShowDialog();
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Không có quyền",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void btnAuditLog_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                FrmAuditLogList frm =
                new FrmAuditLogList();

                frm.ShowDialog();
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Không có quyền",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void btnChangePassword_Click(
            object sender,
            EventArgs e)
        {
            FrmChangePassword frm =
                new FrmChangePassword();

            frm.ShowDialog();
        }

        private async void IdleTimer_Tick(
            object? sender,
            EventArgs e)
        {
            TimeSpan idleTime =
                DateTime.Now -
                IdleTimeoutHelper.LastActivityTime;

            if (idleTime.TotalMinutes < 15)
            {
                return;
            }

            _idleTimer.Stop();

            await AuditHelper.LogAsync(
                AuditAction.AutoLogout,
                "Tự động đăng xuất do không hoạt động trong 15 phút.");

            MessageBox.Show(
                "Phiên làm việc đã hết hạn.\nVui lòng đăng nhập lại.");

            Close();
        }

        private void RegisterActivityControl(
            Control parent)
        {
            foreach (Control control
                in parent.Controls)
            {
                control.MouseMove +=
                    (_, _) =>
                    IdleTimeoutHelper.ResetTimer();

                control.KeyPress +=
                    (_, _) =>
                    IdleTimeoutHelper.ResetTimer();

                if (control.HasChildren)
                {
                    RegisterActivityControl(
                        control);
                }
            }
        }

        private void TimerClock_Tick(
            object? sender,
            EventArgs e)
        {
            lblTime.Text =
                DateTime.Now.ToString(
                    " dd/MM/yyyy HH:mm:ss |");

            lblUser.Text =
                $"👤 {CurrentUser.Username} |";

            lblRole.Text =
                $" 🛡 {CurrentUser.Role} |";
        }



        private void ApplyFilter()
        {
            PermissionHelper.Require("Personnel.View");

            IEnumerable<Personnel> filtered = _allPersonnel;

            // 1. Lọc theo từ khóa tìm kiếm.
            string keyword = txtSearch.Text.Trim();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                filtered = filtered.Where(x =>
                    PersonnelSearchHelper.IsMatch(x, keyword));
            }

            // 2. Lọc theo đơn vị.
            string donVi = cboDonVi.Text.Trim();

            if (!string.IsNullOrWhiteSpace(donVi) &&
                !string.Equals(
                    donVi, "Tất cả",
                    StringComparison.CurrentCultureIgnoreCase))
            {
                filtered = filtered.Where(x =>
                    string.Equals(
                        x.DonVi?.Trim(),
                        donVi,
                        StringComparison.CurrentCultureIgnoreCase));
            }

            // 3. Lọc theo cấp bậc hàm.
            string capBac = cboCapBac.Text.Trim();

            if (!string.IsNullOrWhiteSpace(capBac) &&
                !string.Equals(
                    capBac, "Tất cả",
                    StringComparison.CurrentCultureIgnoreCase))
            {
                filtered = filtered.Where(x =>
                    string.Equals(
                        x.CapBacHam?.Trim(),
                        capBac,
                        StringComparison.CurrentCultureIgnoreCase));
            }

            // 4. Lọc theo chức vụ.
            string chucVu = cboChucVu.Text.Trim();

            if (!string.IsNullOrWhiteSpace(chucVu) &&
                !string.Equals(
                    chucVu, "Tất cả",
                    StringComparison.CurrentCultureIgnoreCase))
            {
                filtered = filtered.Where(x =>
                    string.Equals(
                        x.ChucVu?.Trim(),
                        chucVu,
                        StringComparison.CurrentCultureIgnoreCase));
            }

            var result = filtered.ToList();

            dgvPersonnel.DataSource = null;
            dgvPersonnel.DataSource = result;

            CustomizeDataGridView();
            AddSttColumn();

            UpdateRecordCount(result.Count);
        }



        private void LoadFilterOptions()
        {
            LoadComboBoxOptions(
                cboDonVi,
                _allPersonnel.Select(x => x.DonVi));

            LoadComboBoxOptions(
                cboCapBac,
                _allPersonnel.Select(x => x.CapBacHam));

            LoadComboBoxOptions(
                cboChucVu,
                _allPersonnel.Select(x => x.ChucVu));
        }

        private static void LoadComboBoxOptions(
            ComboBox comboBox,
            IEnumerable<string?> values)
        {
            comboBox.BeginUpdate();

            try
            {
                comboBox.Items.Clear();
                comboBox.Items.Add("Tất cả");

                foreach (string value in values
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x!.Trim())
                    .Distinct(StringComparer.CurrentCultureIgnoreCase)
                    .OrderBy(x => x))
                {
                    comboBox.Items.Add(value);
                }

                comboBox.SelectedIndex = 0;
            }
            finally
            {
                comboBox.EndUpdate();
            }
        }

        private void LoadFilterData()
        {
            // Tạm ngừng xử lý sự kiện trong lúc nạp các lựa chọn.
            _filterLoaded = false;

            try
            {
                bool canView =
                    CurrentUser.HasPermission("Personnel.View");

                cboDonVi.Enabled = canView;
                cboCapBac.Enabled = canView;
                cboChucVu.Enabled = canView;

                if (!canView)
                {
                    cboDonVi.Items.Clear();
                    cboCapBac.Items.Clear();
                    cboChucVu.Items.Clear();
                    return;
                }

                LoadComboBoxOptions(
                    cboDonVi,
                    _allPersonnel.Select(x => x.DonVi));

                LoadComboBoxOptions(
                    cboCapBac,
                    _allPersonnel.Select(x => x.CapBacHam));

                LoadComboBoxOptions(
                    cboChucVu,
                    _allPersonnel.Select(x => x.ChucVu));
            }
            finally
            {
                _filterLoaded =
                    CurrentUser.HasPermission("Personnel.View");
            }
        }

        private void FilterChanged(
            object? sender,
            EventArgs e)
        {
            if (!_filterLoaded)
                return;

            ApplyFilter();
        }

        private void SetupTooltips()
        {
            _toolTip.SetToolTip(
                btnAdd,
                "Thêm CBCS mới");

            _toolTip.SetToolTip(
                btnEdit,
                "Chỉnh sửa CBCS");

            _toolTip.SetToolTip(
                btnDelete,
                "Xóa CBCS");

            _toolTip.SetToolTip(
                btnUsers,
                "Quản lý tài khoản");

            _toolTip.SetToolTip(
                btnAuditLog,
                "Nhật ký hệ thống");

            _toolTip.SetToolTip(
                btnExport,
                "Xuất Excel");

            _toolTip.SetToolTip(
                btnBackup,
                "Sao lưu dữ liệu");

            _toolTip.SetToolTip(
                btnRestore,
                "Khôi phục dữ liệu");

            _toolTip.SetToolTip(
                btnRefresh,
                "Tải lại dữ liệu");

            _toolTip.SetToolTip(
                btnLogout,
                "Đăng xuất");

            _toolTip.SetToolTip(
                pnlTongCBCS,
                "Tổng số cán bộ chiến sĩ hiện có");

            _toolTip.SetToolTip(
                pnlChienSi,
                "Tổng số chiến sĩ");

            _toolTip.SetToolTip(
                pnlCanBo,
                "Tổng số cán bộ");

            _toolTip.SetToolTip(
                pnlDangVien,
                "Tổng số đảng viên");

            _toolTip.SetToolTip(
                pnlThongKe,
                "Thông tin thống kê nhanh của đơn vị");

            _toolTip.SetToolTip(
                lblTongTitle,
                "Tổng số cán bộ chiến sĩ hiện có");

            _toolTip.SetToolTip(
                lblChienSiTitle,
                "Tổng số chiến sĩ");

            _toolTip.SetToolTip(
                lblCanBoTitle,
                "Tổng số cán bộ");

            _toolTip.SetToolTip(
                lblDangVienTitle,
                "Tổng số đảng viên");

            _toolTip.SetToolTip(
                lblThongke,
                "Thông tin thống kê nhanh của đơn vị");

            _toolTip.SetToolTip(
                btnMission,
                "Quản lý công tác và phân công CBCS");
        }

        private void dgvPersonnel_CellMouseEnter(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex < 0)
                return;

            var value =
                dgvPersonnel.Rows[e.RowIndex]
                    .Cells[e.ColumnIndex]
                    .Value?
                    .ToString();

            dgvPersonnel.Rows[e.RowIndex]
                .Cells[e.ColumnIndex]
                .ToolTipText =
                value ?? "";
        }

        private void txtSearch_KeyDown(
            object? sender,
            KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                txtSearch.Clear();
            }
        }

        private void dgvPersonnel_RowPrePaint(
            object? sender,
            DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row =
                dgvPersonnel.Rows[e.RowIndex];

            if (row.DataBoundItem
                is not Personnel personnel)
            {
                return;
            }

            switch (personnel.PersonnelType)
            {
                case 1: // Chiến sĩ

                    row.DefaultCellStyle.BackColor =
                        Color.FromArgb(
                            220,
                            240,
                            255);

                    break;

                case 2: // Cán bộ

                    row.DefaultCellStyle.BackColor =
                        Color.FromArgb(
                            255,
                            245,
                            210);

                    break;
            }
        }

        private void btnMission_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                FrmMissionList frm =
                    new FrmMissionList();

                frm.ShowDialog();
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Không có quyền",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void btnStatistics_Click(
            object sender,
            EventArgs e)
        {
            if (!PermissionHelper.HasPermission("Mission.View"))
            {
                MessageBox.Show(
                    "Bạn không có quyền xem thống kê công tác.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            using FrmPersonnelMissionStatistics frm =
                new FrmPersonnelMissionStatistics();

            frm.ShowDialog();
        }

        public void ShowMissionDashboard()
        {
            if (_missionDashboard == null
                || _missionDashboard.IsDisposed)
            {
                _missionDashboard =
                    new FrmMissionDashboard();

                _missionDashboard.Show();
            }
            else
            {
                _missionDashboard.Focus();
            }
        }

        private void FrmPersonnelList_Shown(
            object? sender,
            EventArgs e)
        {
            // Khi được nhúng trong tab Quản lý CBCS của FrmHome,
            // không tự mở FrmMissionDashboard thành cửa sổ riêng.
            // Chỉ mở dashboard khi FrmPersonnelList chạy độc lập.
            if (!_isHostedInHome
                && CurrentUser.HasPermission("Mission.View"))
            {
                ShowMissionDashboard();
            }
        }

        private void btnNotification_Click(object sender, EventArgs e)
        {
            if (_notificationForm == null || _notificationForm.IsDisposed)
            {
                _notificationForm = new FrmNotificationCenter();
                _notificationForm.Show();
            }
            else
            {
                _notificationForm.BringToFront();
                _notificationForm.Focus();
            }
        }

        private async Task LoadNotificationBadgeAsync()
        {
            int count =
                await _notificationService
                    .GetNotificationCountAsync();

            btnNotification.Text =
                count > 0
                ? $"🔔 Thông báo ({count})"
                : "🔔 Thông báo";
        }

        private void btnAdminDashboard_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                using FrmAdminDashboard frm =
                    new FrmAdminDashboard();

                frm.ShowDialog();
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Không có quyền",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void btnPermission_Click(
            object sender,
            EventArgs e)
        {
            if (!PermissionHelper.CanManagePermission)
            {
                MessageBox.Show(
                    "Bạn không có quyền quản lý phân quyền.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            using FrmPermissionManagement frm =
                new FrmPermissionManagement();

            frm.ShowDialog();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                using FrmPersonnelReport frm = new FrmPersonnelReport();

                frm.ShowDialog();
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Không có quyền",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        private async void btnImportExcel_Click(
            object? sender,
            EventArgs e)
        {
            if (!CurrentUser.HasPermission("Personnel.Create"))
            {
                MessageBox.Show(
                    "Bạn không có quyền thêm CBCS.",
                    "Không có quyền",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            using OpenFileDialog dialog = new();

            dialog.Title = "Chọn file Excel danh sách CBCS";
            dialog.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
            dialog.Multiselect = false;

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            await ImportExcelFileAsync(dialog.FileName);
        }

        private void dgvPersonnel_DragEnter(
            object? sender,
            DragEventArgs e)
        {
            if (e.Data == null ||
                !e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            string[] files =
                (string[])e.Data.GetData(DataFormats.FileDrop)!;

            e.Effect =
                files.Length == 1 &&
                string.Equals(
                    Path.GetExtension(files[0]),
                    ".xlsx",
                    StringComparison.OrdinalIgnoreCase)
                ? DragDropEffects.Copy
                : DragDropEffects.None;
        }

        private async void dgvPersonnel_DragDrop(
            object? sender,
            DragEventArgs e)
        {
            if (!CurrentUser.HasPermission("Personnel.Create"))
            {
                MessageBox.Show(
                    "Bạn không có quyền thêm CBCS.",
                    "Không có quyền",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (e.Data == null ||
                !e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return;
            }

            string[] files =
                (string[])e.Data.GetData(DataFormats.FileDrop)!;

            if (files.Length != 1)
                return;

            string filePath = files[0];

            if (!string.Equals(
                Path.GetExtension(filePath),
                ".xlsx",
                StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(
                    "Vui lòng kéo thả một file Excel .xlsx.",
                    "File không hợp lệ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            await ImportExcelFileAsync(filePath);
        }

        private async Task ImportExcelFileAsync(string filePath)
        {
            try
            {
                PermissionHelper.Require("Personnel.Create");

                ExcelImportService importService = new();

                List<Personnel> personnelList =
                    importService.ReadPersonnel(filePath);

                DialogResult confirm = MessageBox.Show(
                    $"File: {Path.GetFileName(filePath)}\n\n" +
                    $"Số hồ sơ đọc được: {personnelList.Count}\n\n" +
                    "Hệ thống sẽ bỏ qua hồ sơ có Số hiệu hoặc CCCD " +
                    "đã tồn tại. Hồ sơ hiện có sẽ không bị sửa.\n\n" +
                    "Bạn có muốn tiếp tục nhập không?",
                    "Xác nhận nhập Excel",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes)
                    return;

                Cursor previousCursor = Cursor;
                Cursor = Cursors.WaitCursor;

                try
                {
                    var importResult =
                        await _service.ImportManyAsync(personnelList);

                    await LoadPersonnelAsync();

                    if (PermissionHelper.CanViewStatistics)
                        await LoadDashboardAsync();

                    _selectedPersonnelId = 0;

                    string message =
                        "Nhập Excel hoàn tất!\n\n" +
                        $"Đã thêm: {importResult.ImportedCount} hồ sơ\n" +
                        $"Bỏ qua do trùng: {importResult.Skipped.Count} hồ sơ";

                    if (importResult.Skipped.Count > 0)
                    {
                        message += "\n\nDanh sách bị bỏ qua:";

                        int displayCount =
                            Math.Min(importResult.Skipped.Count, 15);

                        for (int i = 0; i < displayCount; i++)
                        {
                            message += "\n• " + importResult.Skipped[i];
                        }

                        if (importResult.Skipped.Count > displayCount)
                        {
                            message +=
                                $"\n... và " +
                                $"{importResult.Skipped.Count - displayCount} hồ sơ khác.";
                        }
                    }

                    MessageBox.Show(
                        message,
                        "Kết quả nhập Excel",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                finally
                {
                    Cursor = previousCursor;
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Không có quyền",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể nhập file Excel.\n\n" + ex.Message,
                    "Lỗi nhập Excel",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Bố cục gọn cho màn hình Quản lý CBCS: chỉ giữ các thao tác
        /// quản lý hồ sơ, bộ lọc và các chức năng dữ liệu cần thiết.
        /// Có thể chạy độc lập hoặc được nhúng trong FrmHome.
        /// </summary>
        private void ApplyCompactPersonnelLayout()
        {
            if (IsDisposed)
                return;

            // Ẩn các nút chức năng không còn dùng tại màn hình này.
            btnUsers.Visible = false;
            btnAuditLog.Visible = false;
            btnChangePassword.Visible = false;
            btnMission.Visible = false;
            btnStatistics.Visible = false;
            btnNotification.Visible = false;
            btnAdminDashboard.Visible = false;
            btnPermission.Visible = false;
            btnReport.Visible = false;

            // Ẩn thẻ thống kê và thông tin người dùng/đăng xuất ở đầu form.
            pnlThongKe.Visible = false;
            pnlTongCBCS.Visible = false;
            pnlChienSi.Visible = false;
            pnlCanBo.Visible = false;
            pnlDangVien.Visible = false;
            lblCurrentUser.Visible = false;
            btnLogout.Visible = false;
            statusStrip1.Visible = false;

            // Giữ lại các thành phần cần dùng.
            pnlSearch.Visible = true;
            txtSearch.Visible = true;
            btnAdd.Visible = CurrentUser.HasPermission("Personnel.Create");
            btnEdit.Visible = CurrentUser.HasPermission("Personnel.Update");
            btnDelete.Visible = CurrentUser.HasPermission("Personnel.Delete");
            btnBackup.Visible = CurrentUser.HasPermission("Database.Backup");
            btnRestore.Visible = CurrentUser.HasPermission("Database.Restore");
            btnImportExcel.Visible = CurrentUser.HasPermission("Personnel.Create");
            btnExport.Visible = true;
            btnRefresh.Visible = true;
            cboDonVi.Visible = true;
            cboCapBac.Visible = true;
            cboChucVu.Visible = true;
            dgvPersonnel.Visible = true;

            int formWidth = ClientSize.Width;
            int formHeight = ClientSize.Height;
            if (formWidth < 600 || formHeight < 300)
                return;

            const int left = 10;
            const int right = 10;
            const int gap = 8;
            const int rowHeight = 36;
            const int row1Y = 10;
            const int row2Y = 54;

            // Hàng 1: tìm kiếm + Thêm/Sửa/Xóa.
            int availableWidth = Math.Max(500, formWidth - left - right);
            int crudWidth = Math.Max(82, Math.Min(104, (availableWidth - 320 - gap * 3) / 3));
            int searchWidth = Math.Max(260, Math.Min(480, availableWidth - (crudWidth * 3) - (gap * 3)));

            pnlSearch.SetBounds(left, row1Y, searchWidth, rowHeight);
            pnlSearch.Padding = new Padding(8, 5, 8, 4);
            txtSearch.SetBounds(8, 5, Math.Max(80, pnlSearch.ClientSize.Width - 16), 25);
            txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            int x = left + searchWidth + gap;
            btnAdd.SetBounds(x, row1Y, crudWidth, rowHeight);
            x += crudWidth + gap;
            btnEdit.SetBounds(x, row1Y, crudWidth, rowHeight);
            x += crudWidth + gap;
            btnDelete.SetBounds(x, row1Y, crudWidth, rowHeight);

            // Hàng 2: ba bộ lọc bên trái, năm chức năng dữ liệu bên phải.
            int actionWidth = Math.Max(82, Math.Min(112, (availableWidth - 3 * 105 - 7 * gap) / 5));
            int filterGap = gap;
            int filterWidth = Math.Max(105, Math.Min(160,
                (availableWidth - (5 * actionWidth) - (7 * gap)) / 3));

            int filterX = left;
            cboDonVi.SetBounds(filterX, row2Y, filterWidth, rowHeight);
            filterX += filterWidth + filterGap;
            cboCapBac.SetBounds(filterX, row2Y, filterWidth, rowHeight);
            filterX += filterWidth + filterGap;
            cboChucVu.SetBounds(filterX, row2Y, filterWidth, rowHeight);

            int actionX = formWidth - right - actionWidth;
            btnRefresh.SetBounds(actionX, row2Y, actionWidth, rowHeight);
            actionX -= actionWidth + gap;
            btnRestore.SetBounds(actionX, row2Y, actionWidth, rowHeight);
            actionX -= actionWidth + gap;
            btnExport.SetBounds(actionX, row2Y, actionWidth, rowHeight);
            actionX -= actionWidth + gap;
            btnImportExcel.SetBounds(actionX, row2Y, actionWidth, rowHeight);
            actionX -= actionWidth + gap;
            btnBackup.SetBounds(actionX, row2Y, actionWidth, rowHeight);

            // Bảng bắt đầu ngay dưới thanh công cụ, thẳng hàng với lề form.
            int gridTop = row2Y + rowHeight + 10;
            int gridHeight = Math.Max(120, formHeight - gridTop - 8);
            dgvPersonnel.SetBounds(left, gridTop,
                Math.Max(200, formWidth - left - right), gridHeight);
            dgvPersonnel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom |
                                  AnchorStyles.Left | AnchorStyles.Right;

            // Giữ nhãn ngắn để các nút chức năng không bị chật chữ.
            btnImportExcel.Text = "⬆ Upload";
            btnExport.Text = "📊 Excel";
            foreach (var button in new[] { btnBackup, btnImportExcel, btnExport, btnRestore, btnRefresh })
            {
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 0;
                button.ForeColor = Color.White;
                button.TextAlign = ContentAlignment.MiddleCenter;
                if (button.Font.Size != 9F || button.Font.Style != FontStyle.Bold)
                    button.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            }

            RoundedControlHelper.SetRounded(pnlSearch, 10);
            RoundedControlHelper.SetRounded(btnAdd, 8);
            RoundedControlHelper.SetRounded(btnEdit, 8);
            RoundedControlHelper.SetRounded(btnDelete, 8);
            RoundedControlHelper.SetRounded(btnBackup, 8);
            RoundedControlHelper.SetRounded(btnImportExcel, 8);
            RoundedControlHelper.SetRounded(btnExport, 8);
            RoundedControlHelper.SetRounded(btnRestore, 8);
            RoundedControlHelper.SetRounded(btnRefresh, 8);
        }

    }
}