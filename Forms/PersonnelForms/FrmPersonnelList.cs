using QuanLyCBCS.Services;
using QuanLyCBCS.Models;
using QuanLyCBCS.Enums;
using QuanLyCBCS.Forms.PersonnelForms;

namespace QuanLyCBCS.Forms.PersonnelForms
{
    public partial class FrmPersonnelList : Form
    {
        private readonly PersonnelService
            _service;


        private int SelectedPersonnelId = 0;
        public FrmPersonnelList()
        {
            InitializeComponent();

            _service = new PersonnelService();
        }

        private async Task LoadPersonnelAsync()
        {

            dgvPersonnel.DataSource = null;

            dgvPersonnel.DataSource =
                await _service.GetAllAsync();

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
                dgvPersonnel.Columns["PersonnelID"].HeaderText = "ID";

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
        "PersonnelID",
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
            }
        }

        private async void FrmPersonnelList_Load(
            object sender,
            EventArgs e)
        {
            await LoadPersonnelAsync();
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
            dgvPersonnel.DataSource =
                await _service.SearchAsync(
                    txtSearch.Text);

            CustomizeDataGridView();
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

            await LoadPersonnelAsync();

            MessageBox.Show(
                "Xóa thành công");
        }

        private async void btnAdd_Click(
            object sender,
            EventArgs e)
        {
            FrmPersonnelDetail frm =
                new FrmPersonnelDetail(
                    FormMode.Add);

            if (frm.ShowDialog() ==
                DialogResult.OK)
            {
                await LoadPersonnelAsync();
            }
        }

        private async void btnEdit_Click(
            object sender,
            EventArgs e)
        {
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
            }
        }

        private async void dgvPersonnel_CellDoubleClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
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
    }
}
