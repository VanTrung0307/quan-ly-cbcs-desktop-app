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
                btnReport,
                btnImportExcel
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
                btnReport,
                btnImportExcel
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
                _allPersonnel =
                    await _service.GetAllAsync();

                dgvPersonnel.DataSource =
                    _allPersonnel;

                CustomizeDataGridView();

                AddSttColumn();

                UpdateRecordCount(
                    _allPersonnel.Count);
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
            string keyword =
                txtSearch.Text.Trim();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                dgvPersonnel.DataSource =
                    _allPersonnel;

                AddSttColumn();

                UpdateRecordCount(
                    _allPersonnel.Count);

                return;
            }

            var result =
                _allPersonnel
                .Where(x =>
                    PersonnelSearchHelper
                        .IsMatch(
                            x,
                            keyword))
                .ToList();

            dgvPersonnel.DataSource =
                result;

            CustomizeDataGridView();

            AddSttColumn();

            UpdateRecordCount(
                result.Count);
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

            LoadFilterData();

            await LoadNotificationBadgeAsync();

            if (PermissionHelper.CanViewStatistics)
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
            await AuditHelper.LogAsync(
                AuditAction.Logout,
                "Đăng xuất hệ thống");

            this.Close();
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

            IEnumerable<Personnel> filtered =
                _allPersonnel;

            if (cboDonVi.Text != "Tất cả")
            {
                filtered =
                    filtered.Where(
                        x => x.DonVi ==
                             cboDonVi.Text);
            }

            if (cboCapBac.Text != "Tất cả")
            {
                filtered =
                    filtered.Where(
                        x => x.CapBacHam ==
                             cboCapBac.Text);
            }

            if (cboChucVu.Text != "Tất cả")
            {
                filtered =
                    filtered.Where(
                        x => x.ChucVu ==
                             cboChucVu.Text);
            }

            var result =
                filtered.ToList();

            dgvPersonnel.DataSource =
                result;

            AddSttColumn();

            UpdateRecordCount(
                result.Count);
        }

        private void LoadFilterData()
        {
            if (!CurrentUser.HasPermission("Personnel.View"))
            {
                cboDonVi.Items.Clear();
                cboCapBac.Items.Clear();
                cboChucVu.Items.Clear();

                cboDonVi.Enabled = false;
                cboCapBac.Enabled = false;
                cboChucVu.Enabled = false;

                _filterLoaded = false;
                return;
            }

            cboDonVi.Enabled = true;
            cboCapBac.Enabled = true;
            cboChucVu.Enabled = true;

            cboDonVi.Items.Clear();
            cboCapBac.Items.Clear();
            cboChucVu.Items.Clear();

            cboDonVi.Items.Add("Tất cả");
            cboCapBac.Items.Add("Tất cả");
            cboChucVu.Items.Add("Tất cả");

            cboDonVi.Items.AddRange(
                _allPersonnel
                    .Select(x => x.DonVi)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct()
                    .OrderBy(x => x)
                    .Cast<object>()
                    .ToArray());

            cboCapBac.Items.AddRange(
                _allPersonnel
                    .Select(x => x.CapBacHam)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct()
                    .OrderBy(x => x)
                    .Cast<object>()
                    .ToArray());

            cboChucVu.Items.AddRange(
                _allPersonnel
                    .Select(x => x.ChucVu)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct()
                    .OrderBy(x => x)
                    .Cast<object>()
                    .ToArray());

            cboDonVi.SelectedIndex = 0;
            cboCapBac.SelectedIndex = 0;
            cboChucVu.SelectedIndex = 0;

            _filterLoaded = true;
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
            if (CurrentUser.HasPermission("Mission.View"))
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

    }
}
