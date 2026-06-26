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

        private readonly AuditLogService _auditService =
            new AuditLogService();

        private bool _filterLoaded;
        private ToolTip _toolTip = new();

        private FrmMissionDashboard?
           _missionDashboard;

        private FrmNotificationCenter? _notificationForm;
        private readonly NotificationService _notificationService;

        public FrmPersonnelList()
        {
            InitializeComponent();

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

            btnStatistics.Visible = true;

            btnNotification.Visible = true;

            btnChangePassword.Visible = true;

            btnAdminDashboard.Visible =
                CurrentUser.HasPermission("Dashboard.View");
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
                btnAdminDashboard
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
                btnAdminDashboard
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

        private bool CheckPermission(
            bool hasPermission)
        {
            if (hasPermission)
            {
                return true;
            }

            MessageBox.Show(
                "Bạn không có quyền thực hiện chức năng này.",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return false;
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
            _allPersonnel =
                await _service.GetAllAsync();

            dgvPersonnel.DataSource =
                _allPersonnel;

            CustomizeDataGridView();

            AddSttColumn();

            UpdateRecordCount(
                _allPersonnel.Count);
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

            await LoadPersonnelAsync();

            LoadFilterData();

            await LoadDashboardAsync();
            await LoadNotificationBadgeAsync();

            if (CurrentUser.HasPermission("Dashboard.View"))
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
            if (!CheckPermission(
                CurrentUser.HasPermission("Personnel.Delete")))
            {
                return;
            }

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

            await _service.DeleteAsync(
                personnelId);

            await _auditService.AddLogAsync(
                "DELETE_PERSONNEL",
                $"Đã xóa CBCS '{hoTen}'");

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

        private async void btnAdd_Click(
            object sender,
            EventArgs e)
        {
            if (!CheckPermission(
                CurrentUser.HasPermission("Personnel.Create")))
            {
                return;
            }

            FrmPersonnelDetail frm =
                new FrmPersonnelDetail(
                    FormMode.Add);

            if (frm.ShowDialog() ==
                DialogResult.OK)
            {
                await LoadPersonnelAsync();
                await LoadDashboardAsync();
            }
        }

        private async void btnEdit_Click(
            object sender,
            EventArgs e)
        {
            if (!CheckPermission(
                CurrentUser.HasPermission("Personnel.Update")))
            {
                return;
            }

            if (_selectedPersonnelId == 0)
            {
                MessageBox.Show(
                    "Vui lòng chọn CBCS");

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

        private async void dgvPersonnel_CellDoubleClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (!CurrentUser.HasPermission("Personnel.Update"))
            {
                MessageBox.Show(
                    "Bạn không có quyền chỉnh sửa dữ liệu.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

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

                await _auditService.AddLogAsync(
                    "EXPORT_EXCEL",
                    saveDialog.FileName);

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
            if (!CurrentUser.HasPermission("Database.Backup"))
            {
                return;
            }

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
            if (!CurrentUser.HasPermission("Database.Restore"))
            {
                return;
            }

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
            AuditLogService auditService =
                new AuditLogService();

            await auditService.AddLogAsync(
                "LOGOUT",
                $"{CurrentUser.Username} đăng xuất");

            this.Close();
        }

        private void btnUsers_Click(
            object sender,
            EventArgs e)
        {
            if (!CurrentUser.HasPermission("User.Manage"))
            {
                return;
            }

            FrmUserList frm =
                new FrmUserList();

            frm.ShowDialog();
        }

        private void btnAuditLog_Click(
            object sender,
            EventArgs e)
        {
            if (!CurrentUser.HasPermission("Audit.View"))
            {
                return;
            }
            FrmAuditLogList frm =
                new FrmAuditLogList();

            frm.ShowDialog();
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

            await _auditService.AddLogAsync(
                "AUTO_LOGOUT",
                "Tự động đăng xuất do không hoạt động");

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
            if (!CurrentUser.HasPermission("Mission.Manage"))
            {
                return;
            }

            FrmMissionList frm =
                new FrmMissionList();

            frm.ShowDialog();
        }

        private void btnStatistics_Click(
            object sender,
            EventArgs e)
        {
            FrmPersonnelMissionStatistics frm =
                new();

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
            using FrmAdminDashboard frm =
                new FrmAdminDashboard();

            frm.ShowDialog();
        }
    }
}
