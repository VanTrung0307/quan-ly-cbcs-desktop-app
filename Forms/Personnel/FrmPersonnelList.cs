using QuanLyCBCS.Services;
using QuanLyCBCS.Models;

namespace QuanLyCBCS.Forms.Personnel
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
            dgvPersonnel.DataSource =
                await _service.GetAllAsync();

            CustomizeDataGridView();
        }

        private void CustomizeDataGridView()
        {
            dgvPersonnel.BorderStyle =
                BorderStyle.None;

            dgvPersonnel.BackgroundColor =
                Color.White;

            dgvPersonnel.DefaultCellStyle.Font =
                new Font("Segoe UI", 10);

            dgvPersonnel.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 10,
                FontStyle.Bold);

            dgvPersonnel.Columns["PersonnelID"]
                .HeaderText = "ID";

            dgvPersonnel.Columns["SoHieu"]
                .HeaderText = "Số hiệu";

            dgvPersonnel.Columns["HoTen"]
                .HeaderText = "Họ tên";

            dgvPersonnel.Columns["DonVi"]
                .HeaderText = "Đơn vị";

            dgvPersonnel.Columns["SoDienThoai"]
                .HeaderText = "Số điện thoại";

            dgvPersonnel.Columns["CCCD"]
                .HeaderText = "CCCD";

            // Ẩn cột không cần

            dgvPersonnel.Columns["HoTenKhongDau"]
                .Visible = false;
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
    }
}
