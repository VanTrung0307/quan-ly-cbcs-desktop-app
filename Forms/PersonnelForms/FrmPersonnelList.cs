using Microsoft.EntityFrameworkCore;
using QuanLyCBCS.Data;
using QuanLyCBCS.Enums;
using QuanLyCBCS.Forms.SystemForms;
using QuanLyCBCS.Forms.UserForms;
using QuanLyCBCS.Helpers;
using QuanLyCBCS.Models;
using QuanLyCBCS.Services;

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

        public FrmPersonnelList()
        {
            InitializeComponent();

            _service = new PersonnelService();

            ApplyPermission();

            RegisterActivityControl(this);

            ConfigureButtons();

            SetupSearchBox();

            ConfigureTimers();
        }

        private bool IsAdmin =>
            CurrentUser.Role.Equals(
        "Admin",
        StringComparison.OrdinalIgnoreCase);

        private void ApplyPermission()
        {
            btnUsers.Visible = IsAdmin;
            btnAuditLog.Visible = IsAdmin;
            btnAdd.Visible = IsAdmin;
            btnEdit.Visible = IsAdmin;
            btnDelete.Visible = IsAdmin;
            btnBackup.Visible = IsAdmin;
            btnRestore.Visible = IsAdmin;
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
                    10,
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
                btnExport,
                btnBackup,
                btnRestore,
                btnRefresh);

            RegisterHoverEffect(
                btnAdd,
                btnEdit,
                btnDelete,
                btnUsers,
                btnAuditLog,
                btnChangePassword,
                btnExport,
                btnBackup,
                btnRestore,
                btnRefresh);
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

        private bool EnsureAdmin()
        {
            if (IsAdmin)
            {
                return true;
            }

            MessageBox.Show(
                "Bạn không có quyền thực hiện chức năng này.");

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
                sttColumn.Width = 60;
                sttColumn.ReadOnly = true;

                dgvPersonnel.Columns.Insert(
                    0,
                    sttColumn);
            }

            dgvPersonnel.Columns["STT"].Visible = true;

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

            string keywordNoSign =
                VietnameseHelper
                    .RemoveVietnameseSigns(
                        keyword)
                    .ToLower();

            var result =
                _allPersonnel
                .Where(x =>
                    (x.HoTen != null &&
                     VietnameseHelper
                        .RemoveVietnameseSigns(
                            x.HoTen)
                        .ToLower()
                        .Contains(keywordNoSign))

                    ||

                    (x.SoHieu != null &&
                     x.SoHieu
                        .ToLower()
                        .Contains(keywordNoSign))

                    ||

                    (x.DonVi != null &&
                     VietnameseHelper
                        .RemoveVietnameseSigns(
                            x.DonVi)
                        .ToLower()
                        .Contains(keywordNoSign))

                    ||

                    (x.ChucVu != null &&
                     VietnameseHelper
                        .RemoveVietnameseSigns(
                            x.ChucVu)
                        .ToLower()
                        .Contains(keywordNoSign))

                    ||

                    (x.CCCD != null &&
                     x.CCCD.Contains(keyword))
                )
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
            if (!EnsureAdmin())
            {
                return;
            }

            if (_selectedPersonnelId == 0)
            {
                MessageBox.Show(
                    "Vui lòng chọn CBCS cần xóa");

                return;
            }

            var confirm =
                MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            await _service.DeleteAsync(
                _selectedPersonnelId);

            await _auditService.AddLogAsync(
                "DELETE_PERSONNEL",
                $"PersonnelId = {_selectedPersonnelId}");

            await LoadPersonnelAsync();
            await LoadDashboardAsync();

            MessageBox.Show(
                "Xóa thành công");
        }

        private async void btnAdd_Click(
            object sender,
            EventArgs e)
        {
            if (!EnsureAdmin())
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
            if (!EnsureAdmin())
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
            if (!IsAdmin)
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
            var list =
                await _service.GetAllAsync();

            ExcelExportService excel =
                new ExcelExportService();

            string filePath =
                Path.Combine(
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.Desktop),
                    "CBCS.xlsx");

            excel.ExportPersonnel(
                list,
                filePath);

            MessageBox.Show(
                "Xuất Excel thành công!");
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
                await _service
                    .GetDashboardAsync();

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
                ThemeHelper.PrimaryColor);

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
            if (!EnsureAdmin())
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
            if (!EnsureAdmin())
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

        private void btnLogout_Click(
            object sender,
            EventArgs e)
        {
            this.Close();
        }

        private void btnUsers_Click(
            object sender,
            EventArgs e)
        {
            if (!EnsureAdmin())
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
            if (!EnsureAdmin())
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
        }
    }
}
