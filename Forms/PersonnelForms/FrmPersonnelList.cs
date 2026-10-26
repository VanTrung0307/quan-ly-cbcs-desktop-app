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
using System.Runtime.InteropServices;

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

        // Thành phần giao diện mới theo bản thiết kế FrmPersonnelList.
        private Panel? _designHeader;
        private Panel? _designFilterPanel;
        private Panel? _designGridFrame;
        private Panel? _designFooter;
        private Label? _designTitle;
        private Label? _designSubtitle;
        private Label? _designSearchIcon;
        private Label? _designFilterUnitIcon;
        private Label? _designFilterRankIcon;
        private Label? _designFilterPositionIcon;
        private Label? _designUnitLabel;
        private Label? _designRankLabel;
        private Label? _designPositionLabel;
        private Label? _designTotalLabel;
        private Label? _designShowingLabel;
        private Label? _designSelectedLabel;
        private Button? _designFirstPage;
        private Button? _designPrevPage;
        private Button? _designPageNumber;
        private Button? _designNextPage;
        private Button? _designLastPage;
        private ComboBox? _designPageSize;

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
            UpdateDesignedFooter();
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
        private void EnsureDesignedLayoutControls()
        {
            if (_designHeader != null)
                return;

            BackColor = Color.FromArgb(242, 248, 253);
            Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            MinimumSize = new Size(1100, 700);

            _designHeader = new Panel
            {
                Name = "pnlDesignHeader",
                BackColor = Color.FromArgb(20, 61, 105),
                Height = 110,
                Dock = DockStyle.None
            };
            _designHeader.Paint += (_, e) =>
            {
                var rect = _designHeader.ClientRectangle;
                if (rect.Width <= 0 || rect.Height <= 0) return;
                using var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                    rect, Color.FromArgb(15, 43, 82), Color.FromArgb(37, 111, 183), 0f);
                e.Graphics.FillRectangle(brush, rect);
                using var overlay = new SolidBrush(Color.FromArgb(28, 255, 255, 255));
                e.Graphics.FillEllipse(overlay, rect.Width - 290, -100, 300, 300);
                using var linePen = new Pen(Color.FromArgb(45, 180, 220, 255), 2);
                e.Graphics.DrawEllipse(linePen, rect.Width - 260, -75, 230, 230);
                e.Graphics.DrawEllipse(linePen, rect.Width - 245, -60, 200, 200);
                using var starFont = new Font("Segoe UI Symbol", 74F, FontStyle.Regular);
                using var starBrush = new SolidBrush(Color.FromArgb(45, 160, 205, 255));
                e.Graphics.DrawString("★", starFont, starBrush, rect.Width - 205, -22);
            };

            var emblem = new Panel
            {
                Name = "pnlDesignEmblem",
                BackColor = Color.Transparent
            };
            emblem.Paint += (_, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using var white = new SolidBrush(Color.White);
                // Ba khuôn mặt và phần vai tạo thành biểu tượng nhóm người màu trắng.
                e.Graphics.FillEllipse(white, 39, 7, 22, 22);
                e.Graphics.FillEllipse(white, 15, 15, 18, 18);
                e.Graphics.FillEllipse(white, 66, 15, 18, 18);
                using var bodyCenter = CreateDesignRoundedPath(new Rectangle(29, 31, 42, 36), 10);
                using var bodyLeft = CreateDesignRoundedPath(new Rectangle(5, 36, 29, 29), 9);
                using var bodyRight = CreateDesignRoundedPath(new Rectangle(63, 36, 29, 29), 9);
                e.Graphics.FillPath(white, bodyCenter);
                e.Graphics.FillPath(white, bodyLeft);
                e.Graphics.FillPath(white, bodyRight);
            };
            _designTitle = new Label
            {
                Name = "lblDesignTitle",
                Text = "QUẢN LÝ CBCS",
                Font = new Font("Segoe UI", 25F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                BackColor = Color.Transparent
            };
            _designSubtitle = new Label
            {
                Name = "lblDesignSubtitle",
                Text = "Quản lý thông tin cán bộ, chiến sĩ trong đơn vị",
                Font = new Font("Segoe UI", 14F, FontStyle.Regular),
                ForeColor = Color.FromArgb(224, 237, 251),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            _designHeader.Controls.Add(emblem);
            _designHeader.Controls.Add(_designTitle);
            _designHeader.Controls.Add(_designSubtitle);
            Controls.Add(_designHeader);

            _designFilterPanel = new Panel
            {
                Name = "pnlDesignFilters",
                BackColor = Color.FromArgb(235, 246, 255),
                BorderStyle = BorderStyle.None
            };
            _designFilterPanel.Paint += (_, e) =>
            {
                var r = _designFilterPanel.ClientRectangle;
                r.Width -= 1; r.Height -= 1;
                using var pen = new Pen(Color.FromArgb(195, 224, 250));
                using var path = CreateDesignRoundedPath(r, 9);
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(pen, path);
            };
            Controls.Add(_designFilterPanel);

            _designGridFrame = new Panel
            {
                Name = "pnlDesignGridFrame",
                BackColor = Color.FromArgb(242, 248, 253),
                Padding = new Padding(9),
                BorderStyle = BorderStyle.None
            };
            _designGridFrame.Paint += (_, e) =>
            {
                var r = _designGridFrame.ClientRectangle;
                r.Width -= 1; r.Height -= 1;
                using var pen = new Pen(Color.FromArgb(211, 232, 250));
                using var path = CreateDesignRoundedPath(r, 10);
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(pen, path);
            };
            Controls.Remove(dgvPersonnel);
            _designGridFrame.Controls.Add(dgvPersonnel);
            Controls.Add(_designGridFrame);

            _designFooter = new Panel
            {
                Name = "pnlDesignFooter",
                BackColor = Color.FromArgb(242, 248, 253)
            };
            Controls.Add(_designFooter);

            _designSearchIcon = new Label
            {
                Text = "\uE721",
                Font = new Font("Segoe MDL2 Assets", 17F, FontStyle.Regular),
                ForeColor = Color.FromArgb(20, 66, 126),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            // Vẽ viền bo bằng GDI+ để không bị mất nét ở bốn góc.
            pnlSearch.BorderStyle = BorderStyle.None;
            pnlSearch.Paint += (_, e) =>
            {
                var r = pnlSearch.ClientRectangle;
                r.Width -= 1; r.Height -= 1;
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using var pen = new Pen(Color.FromArgb(177, 204, 238), 1F);
                using var path = CreateDesignRoundedPath(r, 8);
                e.Graphics.DrawPath(pen, path);
            };
            pnlSearch.Controls.Add(_designSearchIcon);
            _designSearchIcon.BringToFront();

            _designUnitLabel = MakeFilterLabel("Đơn vị");
            _designRankLabel = MakeFilterLabel("Cấp bậc:");
            _designPositionLabel = MakeFilterLabel("Chức vụ:");
            _designFilterUnitIcon = MakeFilterIcon("\uE80F");
            _designFilterRankIcon = MakeFilterIcon("\uE735");
            _designFilterPositionIcon = MakeFilterIcon("\uE821");
            _designFilterPanel.Controls.AddRange(new Control[]
            {
                _designUnitLabel, _designRankLabel, _designPositionLabel,
                _designFilterUnitIcon, _designFilterRankIcon, _designFilterPositionIcon,
                cboDonVi, cboCapBac, cboChucVu
            });

            _designTotalLabel = MakeFooterLabel("Tổng số: 0", 11F, FontStyle.Bold);
            _designShowingLabel = MakeFooterLabel("Đang hiển thị: 0", 11F, FontStyle.Bold);
            _designSelectedLabel = MakeFooterLabel("Đã chọn: 0", 11F, FontStyle.Bold);
            _designFirstPage = MakePagerButton("«");
            _designPrevPage = MakePagerButton("‹");
            _designPageNumber = MakePagerButton("1", true);
            _designNextPage = MakePagerButton("›");
            _designLastPage = MakePagerButton("»");
            _designPageSize = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(30, 55, 90)
            };
            _designPageSize.Items.AddRange(new object[] { "50 dòng/trang", "100 dòng/trang", "200 dòng/trang" });
            _designPageSize.AccessibleName = "Số dòng mỗi trang";
            _designPageSize.IntegralHeight = true;
            _designPageSize.SelectedIndex = 0;
            var footerSeparator1 = new Label { Name = "lblFooterSeparator1", Text = "│", AutoSize = true, Font = new Font("Segoe UI", 12F), ForeColor = Color.FromArgb(166, 190, 219) };
            var footerSeparator2 = new Label { Name = "lblFooterSeparator2", Text = "│", AutoSize = true, Font = new Font("Segoe UI", 12F), ForeColor = Color.FromArgb(166, 190, 219) };
            _designFooter.Controls.AddRange(new Control[]
            {
                _designTotalLabel, footerSeparator1, _designShowingLabel, footerSeparator2, _designSelectedLabel,
                _designFirstPage, _designPrevPage, _designPageNumber,
                _designNextPage, _designLastPage, _designPageSize
            });

            dgvPersonnel.SelectionChanged += (_, _) => UpdateDesignedFooter();
            _designPageSize.SelectedIndexChanged += (_, _) => UpdateDesignedFooter();
        }

        private static System.Drawing.Drawing2D.GraphicsPath CreateDesignRoundedPath(Rectangle rect, int radius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            int d = Math.Max(2, radius * 2);
            if (rect.Width <= d || rect.Height <= d)
            {
                path.AddRectangle(rect);
                return path;
            }
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static Label MakeFilterLabel(string text) => new Label
        {
            Text = text,
            AutoSize = false,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = Color.FromArgb(15, 39, 91),
            TextAlign = ContentAlignment.MiddleLeft,
            BackColor = Color.Transparent
        };

        private static Label MakeFilterIcon(string text) => new Label
        {
            Text = text,
            Font = new Font("Segoe MDL2 Assets", 17F, FontStyle.Regular),
            ForeColor = Color.FromArgb(22, 73, 139),
            BackColor = Color.FromArgb(215, 236, 255),
            TextAlign = ContentAlignment.MiddleCenter
        };

        private static Label MakeFooterLabel(string text, float size, FontStyle style) => new Label
        {
            Text = text,
            AutoSize = true,
            Font = new Font("Segoe UI", size, style),
            ForeColor = Color.FromArgb(20, 48, 104),
            TextAlign = ContentAlignment.MiddleLeft
        };

        private void DgvPersonnel_CellPaintingHeaderWithoutOuterBorder(
            object? sender,
            DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex != -1 || e.ColumnIndex < 0)
                return;

            // Vẽ nền, chữ và biểu tượng header nhưng bỏ border mặc định,
            // để không xuất hiện đường viền bao quanh mép ngoài của header.
            e.Paint(e.ClipBounds, e.PaintParts & ~DataGridViewPaintParts.Border);

            using (var separatorPen = new Pen(Color.White, 1F))
            {
                var currentColumn = dgvPersonnel.Columns[e.ColumnIndex];
                bool hasColumnAfter = dgvPersonnel.Columns
                    .Cast<DataGridViewColumn>()
                    .Any(c => c.Visible && c.DisplayIndex > currentColumn.DisplayIndex);

                // Chỉ vẽ đường dọc giữa các cột, không vẽ ở cạnh phải ngoài cùng.
                if (hasColumnAfter)
                {
                    int x = e.CellBounds.Right - 1;
                    if (e.Graphics != null)
                    {
                        e.Graphics.DrawLine(
                            separatorPen,
                            x,
                            e.CellBounds.Top,
                            x,
                            e.CellBounds.Bottom - 1);
                    }
                }
            }

            // Giữ đường phân cách dưới header, không vẽ đường trên hoặc hai cạnh ngoài.
            using (var bottomPen = new Pen(Color.FromArgb(190, 211, 231), 1F))
            {
                int y = e.CellBounds.Bottom - 1;
                if (e.Graphics != null)
                {
                    e.Graphics.DrawLine(
                        bottomPen,
                        e.CellBounds.Left,
                        y,
                        e.CellBounds.Right - 1,
                        y);
                }
            }

            e.Handled = true;
        }

        private static Button MakePagerButton(string text, bool selected = false)
        {
            var button = new Button
            {
                Text = text,
                FlatStyle = FlatStyle.Flat,
                BackColor = selected ? Color.FromArgb(0, 92, 204) : Color.White,
                ForeColor = selected ? Color.White : Color.FromArgb(30, 70, 130),
                Font = new Font("Segoe UI", 11F, FontStyle.Regular),
                Size = new Size(40, 40),
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            button.FlatAppearance.BorderSize = 0;
            button.Paint += (_, e) =>
            {
                var rect = button.ClientRectangle;
                rect.Width -= 1;
                rect.Height -= 1;
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using var path = CreateDesignRoundedPath(rect, 7);
                using var pen = new Pen(
                    selected ? Color.FromArgb(0, 92, 204) : Color.FromArgb(190, 216, 243),
                    1.2F);
                e.Graphics.DrawPath(pen, path);
            };
            button.Resize += (_, _) => ApplyRoundedRegion(button, 7);
            ApplyRoundedRegion(button, 7);
            return button;
        }

        private void UpdateDesignedFooter()
        {
            if (_designTotalLabel == null || _designShowingLabel == null || _designSelectedLabel == null)
                return;
            int total = _allPersonnel.Count;
            int shown = dgvPersonnel.Rows.Cast<DataGridViewRow>().Count(r => !r.IsNewRow);
            int selected = dgvPersonnel.SelectedRows.Count;
            _designTotalLabel.Text = $"Tổng số: {total}";
            _designShowingLabel.Text = $"Đang hiển thị: {shown}";
            _designSelectedLabel.Text = $"Đã chọn: {selected}";
            _designTotalLabel.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            _designShowingLabel.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            _designSelectedLabel.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            _designTotalLabel.ForeColor = _designShowingLabel.ForeColor = _designSelectedLabel.ForeColor = Color.FromArgb(0, 70, 160);
            if (_designPageNumber != null)
                _designPageNumber.Text = "1";
        }

        /// <summary>
        /// Bố cục theo bản thiết kế mới: header xanh, thanh thao tác,
        /// vùng lọc, bảng có khung và thanh thông tin/phân trang phía dưới.
        /// </summary>
        private void ApplyCompactPersonnelLayout()
        {
            if (IsDisposed || ClientSize.Width < 700 || ClientSize.Height < 400)
                return;

            EnsureDesignedLayoutControls();

            btnUsers.Visible = false;
            btnAuditLog.Visible = false;
            btnChangePassword.Visible = false;
            btnMission.Visible = false;
            btnStatistics.Visible = false;
            btnNotification.Visible = false;
            btnAdminDashboard.Visible = false;
            btnPermission.Visible = false;
            btnReport.Visible = false;
            pnlThongKe.Visible = false;
            pnlTongCBCS.Visible = false;
            pnlChienSi.Visible = false;
            pnlCanBo.Visible = false;
            pnlDangVien.Visible = false;
            lblCurrentUser.Visible = false;
            btnLogout.Visible = false;
            statusStrip1.Visible = false;

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
            cboDonVi.Visible = cboCapBac.Visible = cboChucVu.Visible = true;
            dgvPersonnel.Visible = true;

            int w = ClientSize.Width;
            int h = ClientSize.Height;
            const int outer = 22;
            const int gap = 10;
            const int headerH = 110;
            const int toolbarY = 120;
            const int toolbarH = 50;
            const int filterY = 192;
            const int filterH = 72;
            const int footerH = 70;
            int toolbarW = w - outer * 2;

            _designHeader!.SetBounds(0, 0, w, headerH);
            var emblem = _designHeader.Controls[0];
            emblem.SetBounds(26, 16, 100, 78);
            _designTitle!.Location = new Point(142, 18);
            _designSubtitle!.Location = new Point(143, 67);

            // Tìm kiếm và ba nút CRUD ở nửa trái.
            int searchW = Math.Min(502, Math.Max(300, (int)(toolbarW * 0.315)));
            pnlSearch.SetBounds(outer, toolbarY, searchW, toolbarH);
            pnlSearch.BackColor = Color.White;
            pnlSearch.Padding = new Padding(8, 5, 8, 5);
            pnlSearch.BorderStyle = BorderStyle.None;
            _designSearchIcon!.SetBounds(8, 4, 34, toolbarH - 8);
            // TextBox đơn dòng có chiều cao cố định theo font; đặt Top thủ công để placeholder nằm giữa.
            int searchTextH = 26;
            txtSearch.SetBounds(45, (toolbarH - searchTextH) / 2, Math.Max(100, searchW - 55), searchTextH);
            txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSearch.Font = new Font("Segoe UI", 11F);
            txtSearch.BorderStyle = BorderStyle.None;
            txtSearch.BackColor = Color.White;
            txtSearch.ForeColor = Color.FromArgb(55, 76, 110);

            int crudX = outer + searchW + gap;
            int crudGap = 10;
            int crudW = Math.Min(126, Math.Max(92, (int)(toolbarW * 0.078)));
            int crudH = toolbarH;
            btnAdd.SetBounds(crudX, toolbarY, crudW, crudH);
            btnEdit.SetBounds(crudX + crudW + crudGap, toolbarY, crudW, crudH);
            btnDelete.SetBounds(crudX + (crudW + crudGap) * 2, toolbarY, crudW, crudH);

            // Năm nút dữ liệu nằm bên phải, kích thước cân đối.
            int actionW = Math.Min(134, Math.Max(100, (toolbarW - 3 * (crudW + crudGap) - searchW - 6 * gap) / 5));
            int actionsTotal = actionW * 5 + gap * 4;
            int actionX = w - outer - actionsTotal;
            // Nếu không đủ chỗ, thu gọn tìm kiếm thay vì để các nút tràn khỏi form.
            int minCrudEnd = crudX + (crudW + crudGap) * 2 + crudW;
            if (actionX < minCrudEnd + gap)
            {
                actionW = Math.Max(78, (w - outer * 2 - minCrudEnd - gap * 5) / 5);
                actionsTotal = actionW * 5 + gap * 4;
                actionX = w - outer - actionsTotal;
                int adjustedSearch = Math.Max(250, actionX - gap - (crudX + (crudW + crudGap) * 2 + crudW));
                if (adjustedSearch < searchW)
                {
                    searchW = adjustedSearch;
                    pnlSearch.Width = searchW;
                    txtSearch.Width = Math.Max(100, searchW - 55);
                    crudX = outer + searchW + gap;
                    btnAdd.Left = crudX;
                    btnEdit.Left = crudX + crudW + crudGap;
                    btnDelete.Left = crudX + (crudW + crudGap) * 2;
                    minCrudEnd = btnDelete.Right;
                    actionX = Math.Max(minCrudEnd + gap, w - outer - actionsTotal);
                }
            }
            btnBackup.SetBounds(actionX, toolbarY, actionW, toolbarH);
            btnImportExcel.SetBounds(actionX + actionW + gap, toolbarY, actionW, toolbarH);
            btnExport.SetBounds(actionX + (actionW + gap) * 2, toolbarY, actionW, toolbarH);
            btnRestore.SetBounds(actionX + (actionW + gap) * 3, toolbarY, actionW, toolbarH);
            btnRefresh.SetBounds(actionX + (actionW + gap) * 4, toolbarY, actionW, toolbarH);

            // Khu vực lọc với biểu tượng, nhãn và combobox theo mẫu.
            _designFilterPanel!.SetBounds(outer, filterY, w - outer * 2, filterH);
            int innerX = 20;
            int groupW = (w - outer * 2 - 40) / 3;
            int iconW = 42;
            int labelW = 82;
            int comboW = Math.Min(270, Math.Max(170, groupW - iconW - labelW - 58));
            ConfigureFilterGroup(_designFilterUnitIcon!, _designUnitLabel!, cboDonVi, innerX, iconW, labelW, comboW, filterH);
            ConfigureFilterGroup(_designFilterRankIcon!, _designRankLabel!, cboCapBac, innerX + groupW, iconW, labelW, comboW, filterH);
            ConfigureFilterGroup(_designFilterPositionIcon!, _designPositionLabel!, cboChucVu, innerX + groupW * 2, iconW, labelW, comboW, filterH);

            // Lưới có lề bên trong khung, tránh chạm vào viền cửa sổ.
            int gridTop = filterY + filterH + 12;
            int gridBottom = h - footerH - 8;
            int gridH = Math.Max(150, gridBottom - gridTop);
            _designGridFrame!.SetBounds(outer - 2, gridTop, w - (outer - 2) * 2, gridH);
            dgvPersonnel.SetBounds(8, 5, _designGridFrame.ClientSize.Width - 18, _designGridFrame.ClientSize.Height - 12);
            dgvPersonnel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvPersonnel.BackgroundColor = Color.White;
            dgvPersonnel.BorderStyle = BorderStyle.None;
            dgvPersonnel.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            // Bỏ đường viền bao quanh header; vẽ lại các đường phân cách bên trong.
            dgvPersonnel.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.None;
            dgvPersonnel.CellPainting -= DgvPersonnel_CellPaintingHeaderWithoutOuterBorder;
            dgvPersonnel.CellPainting += DgvPersonnel_CellPaintingHeaderWithoutOuterBorder;
            dgvPersonnel.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            dgvPersonnel.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
            dgvPersonnel.RowHeadersVisible = false;
            dgvPersonnel.ColumnHeadersHeight = 58;
            dgvPersonnel.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            ApplyRoundedRegion(dgvPersonnel, 8);
            dgvPersonnel.Resize -= DgvPersonnel_ApplyRoundedRegion;
            dgvPersonnel.Resize += DgvPersonnel_ApplyRoundedRegion;

            _designFooter!.SetBounds(outer, h - footerH, w - outer * 2, footerH - 4);
            _designTotalLabel!.Location = new Point(14, 23);
            _designShowingLabel!.Location = new Point(164, 23);
            _designSelectedLabel!.Location = new Point(335, 23);
            if (_designFooter.Controls[1] is Label separator1) separator1.Location = new Point(145, 21);
            if (_designFooter.Controls[3] is Label separator2) separator2.Location = new Point(316, 21);
            int pagerY = 8;
            int pagerButtonW = 40;
            int pagerGap = 10;
            // Ô chọn số dòng có cùng chiều cao với nút phân trang nhưng giữ
            // chiều rộng đủ để hiển thị đầy đủ nội dung "50 dòng/trang".
            int pageSizeW = 150;
            int pagerTotalW = pagerButtonW * 5 + pagerGap * 5 + pageSizeW;
            int pagerX = _designFooter.Width - pagerTotalW;
            _designFirstPage!.SetBounds(pagerX, pagerY, pagerButtonW, 40);
            _designPrevPage!.SetBounds(pagerX + (pagerButtonW + pagerGap), pagerY, pagerButtonW, 40);
            _designPageNumber!.SetBounds(pagerX + (pagerButtonW + pagerGap) * 2, pagerY, pagerButtonW, 40);
            _designNextPage!.SetBounds(pagerX + (pagerButtonW + pagerGap) * 3, pagerY, pagerButtonW, 40);
            _designLastPage!.SetBounds(pagerX + (pagerButtonW + pagerGap) * 4, pagerY, pagerButtonW, 40);
            // ComboBox DropDownList thường chỉ vẽ phần hộp cao khoảng 28 px;
            // dịch xuống 6 px để phần hộp hiển thị nằm giữa các nút cao 40 px.
            _designPageSize!.SetBounds(
                pagerX + (pagerButtonW + pagerGap) * 5,
                pagerY + 6,
                pageSizeW,
                40);
            _designPageSize.DropDownWidth = 150;
            _designPageSize.DropDownHeight = 120;
            _designPageSize.DropDown -= ComboBox_RoundDropDown;
            _designPageSize.DropDown += ComboBox_RoundDropDown;
            _designPageSize.Resize -= ComboBox_ApplyRoundedRegion;
            _designPageSize.Resize += ComboBox_ApplyRoundedRegion;
            ApplyRoundedRegion(_designPageSize, 7);
            foreach (var pagerButton in new[] { _designFirstPage, _designPrevPage, _designPageNumber, _designNextPage, _designLastPage })
                RoundedControlHelper.SetRounded(pagerButton, 6);
            RoundedControlHelper.SetRounded(_designPageSize, 6);

            // Màu sắc/nội dung các nút giữ nguyên luồng xử lý hiện có.
            btnAdd.Text = "\uE710  Thêm";
            btnEdit.Text = "\uE70F  Sửa";
            btnDelete.Text = "\uE74D  Xóa";
            btnBackup.Text = "\uE1D8  Backup";
            btnImportExcel.Text = "\uE898  Upload";
            btnExport.Text = "\uE9F9  Excel";
            btnRestore.Text = "\uE777  Restore";
            btnRefresh.Text = "\uE72C  Làm mới";

            btnAdd.BackColor = Color.FromArgb(0, 75, 190);
            btnEdit.BackColor = Color.FromArgb(35, 112, 239);
            btnDelete.BackColor = Color.FromArgb(245, 52, 43);
            btnBackup.BackColor = Color.FromArgb(34, 117, 237);
            btnImportExcel.BackColor = Color.FromArgb(0, 151, 83);
            btnExport.BackColor = Color.FromArgb(0, 157, 89);
            btnRestore.BackColor = Color.FromArgb(239, 145, 0);
            btnRefresh.BackColor = Color.FromArgb(29, 54, 91);
            foreach (var button in new[] { btnAdd, btnEdit, btnDelete, btnBackup, btnImportExcel, btnExport, btnRestore, btnRefresh })
            {
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 0;
                button.ForeColor = Color.White;
                button.TextAlign = ContentAlignment.MiddleCenter;
                button.Font = new Font("Segoe MDL2 Assets", 10F, FontStyle.Regular);
                button.Cursor = Cursors.Hand;
                RoundedControlHelper.SetRounded(button, 8);
            }
            RoundedControlHelper.SetRounded(pnlSearch, 8);
            RoundedControlHelper.SetRounded(_designFilterPanel, 9);
            UpdateDesignedFooter();

            // Đảm bảo các khung trang trí nằm phía sau nội dung tương ứng.
            _designHeader.SendToBack();
            _designFilterPanel.SendToBack();
            _designGridFrame.SendToBack();
            _designFooter.BringToFront();
            pnlSearch.BringToFront();
            btnAdd.BringToFront(); btnEdit.BringToFront(); btnDelete.BringToFront();
            btnBackup.BringToFront(); btnImportExcel.BringToFront(); btnExport.BringToFront(); btnRestore.BringToFront(); btnRefresh.BringToFront();
        }

        private void DgvPersonnel_ApplyRoundedRegion(object? sender, EventArgs e)
        {
            if (sender is Control control)
                ApplyRoundedRegion(control, 8);
        }

        private static void ApplyRoundedRegion(Control control, int radius)
        {
            if (control.Width < 2 || control.Height < 2) return;
            using var path = CreateDesignRoundedPath(new Rectangle(0, 0, control.Width - 1, control.Height - 1), radius);
            var newRegion = new Region(path);
            var oldRegion = control.Region;
            control.Region = newRegion;
            oldRegion?.Dispose();
        }

        private static void ConfigureFilterGroup(
            Label icon, Label label, ComboBox combo, int x,
            int iconW, int labelW, int comboW, int groupHeight)
        {
            icon.SetBounds(x, 15, iconW, 42);
            RoundedControlHelper.SetRounded(icon, 8);
            label.SetBounds(x + iconW + 10, 14, labelW, groupHeight - 24);
            // Chỉ dịch ComboBox xuống nhẹ để phần hiển thị native nằm giữa thẻ;
            // vị trí icon và label được giữ nguyên theo thiết kế.
            combo.SetBounds(x + iconW + labelW + 18, 23, comboW, 34);
            combo.Font = new Font("Segoe UI", 10F);
            combo.BackColor = Color.White;
            combo.ForeColor = Color.FromArgb(30, 50, 85);
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
            combo.FlatStyle = FlatStyle.Flat;
            combo.DrawMode = DrawMode.OwnerDrawFixed;
            combo.ItemHeight = 22;
            combo.DropDownHeight = 180;
            combo.DropDownWidth = Math.Max(combo.Width, 220);
            ApplyRoundedRegion(combo, 6);
            combo.Resize -= ComboBox_ApplyRoundedRegion;
            combo.Resize += ComboBox_ApplyRoundedRegion;
            combo.DropDown -= ComboBox_RoundDropDown;
            combo.DropDown += ComboBox_RoundDropDown;
            combo.DrawItem -= FilterCombo_DrawItem;
            combo.DrawItem += FilterCombo_DrawItem;
        }

        private static void ComboBox_ApplyRoundedRegion(object? sender, EventArgs e)
        {
            if (sender is Control control)
                ApplyRoundedRegion(control, 6);
        }

        private static void FilterCombo_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (sender is not ComboBox combo || e.Index < 0) return;
            bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            Color back = selected ? Color.FromArgb(0, 105, 210) : Color.White;
            Color fore = selected ? Color.White : Color.FromArgb(30, 50, 85);
            using (var brush = new SolidBrush(back))
                e.Graphics.FillRectangle(brush, e.Bounds);
            TextRenderer.DrawText(
                e.Graphics,
                combo.GetItemText(combo.Items[e.Index]),
                combo.Font,
                Rectangle.Inflate(e.Bounds, -7, 0),
                fore,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }

        private static void ComboBox_RoundDropDown(object? sender, EventArgs e)
        {
            if (sender is not ComboBox combo) return;
            try
            {
                var info = new ComboBoxInfo { cbSize = Marshal.SizeOf<ComboBoxInfo>() };
                if (GetComboBoxInfo(combo.Handle, ref info) && info.hwndList != IntPtr.Zero)
                {
                    // Lấy kích thước thật của popup native thay vì dùng DropDownWidth/
                    // DropDownHeight (các giá trị này không luôn khớp kích thước thực).
                    var popupRect = new RECT();
                    if (!GetWindowRect(info.hwndList, ref popupRect)) return;

                    int width = Math.Max(1, popupRect.Right - popupRect.Left);
                    int height = Math.Max(1, popupRect.Bottom - popupRect.Top);
                    int radius = Math.Min(12, Math.Max(4, height / 8));

                    // Tạo vùng bo góc, sau đó lấp phần góc trên bằng hình chữ nhật:
                    // hai góc trên vuông, chỉ hai góc dưới còn bo tròn.
                    IntPtr roundedRegion = CreateRoundRectRgn(0, 0, width + 1, height + 1, radius * 2, radius * 2);
                    IntPtr topRegion = CreateRectRgn(0, 0, width + 1, Math.Min(height + 1, radius + 2));
                    if (roundedRegion != IntPtr.Zero && topRegion != IntPtr.Zero)
                    {
                        CombineRgn(roundedRegion, roundedRegion, topRegion, RGN_OR);
                        if (SetWindowRgn(info.hwndList, roundedRegion, true) != 0)
                            roundedRegion = IntPtr.Zero; // Windows sở hữu region khi SetWindowRgn thành công.
                    }
                    if (roundedRegion != IntPtr.Zero) DeleteObject(roundedRegion);
                    if (topRegion != IntPtr.Zero) DeleteObject(topRegion);
                }
            }
            catch
            {
                // Nếu Windows không cho áp dụng vùng bo cho popup native,
                // ComboBox vẫn hoạt động bình thường.
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct ComboBoxInfo
        {
            public int cbSize;
            public RECT rcItem;
            public RECT rcButton;
            public int stateButton;
            public IntPtr hwndCombo;
            public IntPtr hwndItem;
            public IntPtr hwndList;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool GetComboBoxInfo(IntPtr hwndCombo, ref ComboBoxInfo info);

        [DllImport("gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int left, int top, int right, int bottom, int width, int height);

        [DllImport("user32.dll", EntryPoint = "SetWindowRgn")]
        private static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool redraw);

        [DllImport("user32.dll", EntryPoint = "GetWindowRect")]
        private static extern bool GetWindowRect(IntPtr hWnd, ref RECT rect);

        [DllImport("gdi32.dll", EntryPoint = "CreateRectRgn")]
        private static extern IntPtr CreateRectRgn(int left, int top, int right, int bottom);

        [DllImport("gdi32.dll", EntryPoint = "CombineRgn")]
        private static extern int CombineRgn(IntPtr destination, IntPtr source1, IntPtr source2, int mode);

        private const int RGN_OR = 2;

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        private static extern bool DeleteObject(IntPtr hObject);

    }
}