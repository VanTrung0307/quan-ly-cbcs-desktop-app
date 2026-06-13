using Microsoft.EntityFrameworkCore;
using QuanLyCBCS.Data;
using QuanLyCBCS.Enums;
using QuanLyCBCS.Forms.Auth;
using QuanLyCBCS.Forms.SystemForms;
using QuanLyCBCS.Forms.UserForms;
using QuanLyCBCS.Helpers;
using QuanLyCBCS.Services;

namespace QuanLyCBCS.Forms.PersonnelForms
{
    public partial class FrmPersonnelList : Form
    {
        private PersonnelService _service;

        private System.Windows.Forms.Timer _idleTimer =
            new System.Windows.Forms.Timer();

        private int SelectedPersonnelId = 0;
        private CancellationTokenSource?
    _searchCts;
        private readonly AuditLogService _auditService =
    new AuditLogService();

        public FrmPersonnelList()
        {
            InitializeComponent();

            _service =
                new PersonnelService();

            ApplyPermission();

            RegisterActivityControl(this);

            _idleTimer.Interval = 1000;

            _idleTimer.Tick +=
                IdleTimer_Tick;

            _idleTimer.Start();
        }

        private bool IsAdmin =>
            CurrentUser.Role.Equals(
        "Admin",
        StringComparison.OrdinalIgnoreCase);

        private void ApplyPermission()
        {
            btnUsers.Visible = IsAdmin;
            btnAuditLog.Visible = IsAdmin;

            if (IsAdmin)
            {
                return;
            }

            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnBackup.Enabled = false;
            btnRestore.Enabled = false;
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
            dgvPersonnel.DataSource = null;

            dgvPersonnel.DataSource =
                await _service.GetAllAsync();

            AddSttColumn();

            CustomizeDataGridView();
        }

        private void CustomizeDataGridView()
        {
            // ==========================
            // GIAO DIỆN CHUNG
            // ==========================

            dgvPersonnel.BorderStyle =
                BorderStyle.None;

            dgvPersonnel.BackgroundColor =
                Color.White;

            dgvPersonnel.DefaultCellStyle.Font =
                new Font("Segoe UI", 10);

            dgvPersonnel.ColumnHeadersDefaultCellStyle.Font =
                new Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold);

            dgvPersonnel.RowTemplate.Height = 32;

            dgvPersonnel.AllowUserToAddRows = false;

            dgvPersonnel.ReadOnly = true;

            dgvPersonnel.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvPersonnel.MultiSelect = false;

            // ==========================
            // CHO PHÉP CUỘN NGANG
            // ==========================

            dgvPersonnel.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.None;

            dgvPersonnel.ScrollBars =
                ScrollBars.Both;

            // ==========================
            // ĐỔI TÊN CỘT
            // ==========================
            if (dgvPersonnel.Columns["PersonnelID"] != null)
            {
                dgvPersonnel.Columns["PersonnelID"]
                    .Visible = false;
            }

            //if (dgvPersonnel.Columns["PersonnelID"] != null)
            //    dgvPersonnel.Columns["PersonnelID"].HeaderText = "STT";

            if (dgvPersonnel.Columns["SoHieu"] != null)
                dgvPersonnel.Columns["SoHieu"].HeaderText = "Số hiệu";

            if (dgvPersonnel.Columns["HoTen"] != null)
                dgvPersonnel.Columns["HoTen"].HeaderText = "Họ tên";

            if (dgvPersonnel.Columns["DonVi"] != null)
                dgvPersonnel.Columns["DonVi"].HeaderText = "Đơn vị";

            if (dgvPersonnel.Columns["GioiTinh"] != null)
                dgvPersonnel.Columns["GioiTinh"].HeaderText = "Giới tính";

            if (dgvPersonnel.Columns["NgaySinh"] != null)
                dgvPersonnel.Columns["NgaySinh"].HeaderText = "Ngày sinh";

            if (dgvPersonnel.Columns["CapBacHam"] != null)
                dgvPersonnel.Columns["CapBacHam"].HeaderText = "Cấp bậc";

            if (dgvPersonnel.Columns["ChucVu"] != null)
                dgvPersonnel.Columns["ChucVu"].HeaderText = "Chức vụ";

            if (dgvPersonnel.Columns["CCCD"] != null)
                dgvPersonnel.Columns["CCCD"].HeaderText = "CCCD";

            if (dgvPersonnel.Columns["SoDienThoai"] != null)
                dgvPersonnel.Columns["SoDienThoai"].HeaderText = "Số điện thoại";

            // ==========================
            // ĐỘ RỘNG CỘT
            // ==========================

            if (dgvPersonnel.Columns["PersonnelID"] != null)
                dgvPersonnel.Columns["PersonnelID"].Width = 60;

            if (dgvPersonnel.Columns["SoHieu"] != null)
                dgvPersonnel.Columns["SoHieu"].Width = 120;

            if (dgvPersonnel.Columns["HoTen"] != null)
                dgvPersonnel.Columns["HoTen"].Width = 250;

            if (dgvPersonnel.Columns["DonVi"] != null)
                dgvPersonnel.Columns["DonVi"].Width = 220;

            if (dgvPersonnel.Columns["GioiTinh"] != null)
                dgvPersonnel.Columns["GioiTinh"].Width = 90;

            if (dgvPersonnel.Columns["NgaySinh"] != null)
                dgvPersonnel.Columns["NgaySinh"].Width = 120;

            if (dgvPersonnel.Columns["CapBacHam"] != null)
                dgvPersonnel.Columns["CapBacHam"].Width = 140;

            if (dgvPersonnel.Columns["ChucVu"] != null)
                dgvPersonnel.Columns["ChucVu"].Width = 180;

            if (dgvPersonnel.Columns["CCCD"] != null)
                dgvPersonnel.Columns["CCCD"].Width = 140;

            if (dgvPersonnel.Columns["SoDienThoai"] != null)
                dgvPersonnel.Columns["SoDienThoai"].Width = 130;

            // ==========================
            // ẨN CỘT KỸ THUẬT
            // ==========================

            string[] hiddenColumns =
            {
                "PersonnelID",
                "HoTenKhongDau",
                "PersonnelType",
                "CreatedAt",
                "UpdatedAt"
            };

            foreach (string column in hiddenColumns)
            {
                if (dgvPersonnel.Columns[column] != null)
                {
                    dgvPersonnel.Columns[column]
                        .Visible = false;
                }
            }

            // ==========================
            // FORMAT NGÀY THÁNG
            // ==========================

            string[] fullDateColumns =
            {
                "NgaySinh",
                "NgayCapCCCD",
                "NgayNhanCapBac",
                "NgayBoNhiem",
                "NgayVaoDangDuBi",
                "NgayVaoDangChinhThuc"
            };

            foreach (string column in fullDateColumns)
            {
                if (dgvPersonnel.Columns[column] != null)
                {
                    dgvPersonnel.Columns[column]
                        .DefaultCellStyle.Format =
                        "dd/MM/yyyy";
                }
            }

            string[] monthYearColumns =
            {
        "ThangNamVaoNganh",
        "ThangNamNhanCongTac",
        "ThangNamTotNghiep"
    };

            foreach (string column in monthYearColumns)
            {
                if (dgvPersonnel.Columns[column] != null)
                {
                    dgvPersonnel.Columns[column]
                        .DefaultCellStyle.Format =
                        "MM/yyyy";
                }
            }

            // ==========================
            // CHỈ HIỂN THỊ CÁC CỘT CHÍNH
            // ==========================

            foreach (DataGridViewColumn column
                in dgvPersonnel.Columns)
            {
                column.Visible = false;
            }

            string[] visibleColumns =
            {
                "STT",
                "SoHieu",
                "HoTen",
                "DonVi",
                "GioiTinh",
                "NgaySinh",
                "CapBacHam",
                "ChucVu",
                "CCCD",
                "SoDienThoai"
            };

            foreach (string column in visibleColumns)
            {
                if (dgvPersonnel.Columns[column] != null)
                {
                    dgvPersonnel.Columns[column]
                        .Visible = true;
                }
                if (dgvPersonnel.Columns["STT"] != null)
                    dgvPersonnel.Columns["STT"].Width = 60;
            }
        }

        private async void FrmPersonnelList_Load(
            object sender,
            EventArgs e)
        {
            IdleTimeoutHelper.ResetTimer();
            lblCurrentUser.Text =
                $"👤 {CurrentUser.Username} ({CurrentUser.Role})";

            this.Text =
                $"Quản lý CBCS - {CurrentUser.Role}";

            if (CurrentUser.Role != "Admin")
            {
                btnUsers.Visible = false;

                btnAuditLog.Visible = false;

                btnBackup.Visible = false;

                btnRestore.Visible = false;
            }

            await LoadPersonnelAsync();

            await LoadDashboardAsync();
        }

        private void dgvPersonnel_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            SelectedPersonnelId =
                Convert.ToInt32(
                    dgvPersonnel
                    .Rows[e.RowIndex]
                    .Cells["PersonnelID"]
                    .Value);
        }

        private async void txtSearch_TextChanged(
            object sender,
            EventArgs e)
        {
            try
            {
                _searchCts?.Cancel();

                _searchCts =
                    new CancellationTokenSource();

                var token =
                    _searchCts.Token;

                await Task.Delay(
                    300,
                    token);

                string keyword =
                    txtSearch.Text.Trim();

                var result =
                    await _service.SearchAsync(
                        keyword);

                if (token.IsCancellationRequested)
                    return;

                dgvPersonnel.DataSource = null;
                dgvPersonnel.DataSource = result;

                AddSttColumn();

                CustomizeDataGridView();

                this.Text =
                    $"Quản lý CBCS - {result.Count} kết quả";
            }
            catch (TaskCanceledException)
            {
                // bỏ qua
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Lỗi tìm kiếm");
            }
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
            if (CurrentUser.Role != "Admin")
            {
                MessageBox.Show(
                    "Bạn không có quyền thực hiện chức năng này.");

                return;
            }

            if (SelectedPersonnelId == 0)
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
                SelectedPersonnelId);

            AuditLogService auditService =
                new AuditLogService();

            await _auditService.AddLogAsync(
                "DELETE_PERSONNEL",
                $"PersonnelId = {SelectedPersonnelId}");

            await LoadPersonnelAsync();
            await LoadDashboardAsync();

            MessageBox.Show(
                "Xóa thành công");
        }

        private async void btnAdd_Click(
            object sender,
            EventArgs e)
        {
            if (CurrentUser.Role != "Admin")
            {
                MessageBox.Show(
                    "Bạn không có quyền thực hiện chức năng này.");

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
            if (CurrentUser.Role != "Admin")
            {
                MessageBox.Show(
                    "Bạn không có quyền thực hiện chức năng này.");

                return;
            }

            if (SelectedPersonnelId == 0)
            {
                MessageBox.Show(
                    "Vui lòng chọn CBCS");

                return;
            }

            FrmPersonnelDetail frm =
                new FrmPersonnelDetail(
                    FormMode.Edit,
                    SelectedPersonnelId);

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

            if (SelectedPersonnelId == 0)
                return;

            FrmPersonnelDetail frm =
                new FrmPersonnelDetail(
                    FormMode.Edit,
                    SelectedPersonnelId);

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

        private async Task LoadDashboardAsync()
        {
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
            pnlTongCBCS.BackColor =
                Color.SteelBlue;

            pnlChienSi.BackColor =
                Color.SeaGreen;

            pnlCanBo.BackColor =
                Color.DarkOrange;

            pnlDangVien.BackColor =
                Color.MediumPurple;

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
        }

        private async void btnBackup_Click(
            object sender,
            EventArgs e)
        {
            if (CurrentUser.Role != "Admin")
            {
                MessageBox.Show(
                    "Bạn không có quyền thực hiện chức năng này.");

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
            if (CurrentUser.Role != "Admin")
            {
                MessageBox.Show(
                    "Bạn không có quyền thực hiện chức năng này.");

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
            if (CurrentUser.Role != "Admin")
            {
                MessageBox.Show(
                    "Bạn không có quyền.");

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
    }
}
