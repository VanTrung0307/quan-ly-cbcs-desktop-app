using QuanLyCBCS.DTOs;
using QuanLyCBCS.Forms.CommonForms;
using QuanLyCBCS.Services;
using System;
using System.Windows.Forms;

namespace QuanLyCBCS.Forms.PersonnelForms
{
    public partial class FrmPersonnelReport : Form
    {
        private readonly PersonnelReportService _reportService;
        public FrmPersonnelReport()
        {
            InitializeComponent();

            _reportService = new PersonnelReportService();
        }

        private async Task LoadPersonnelReportAsync()
        {
            try
            {
                DateTime reportDate = dtpReportDate.Value;

                List<PersonnelReportDto> data =
                    await _reportService.GetReportAsync(reportDate);

                dgvPersonnelReport.AutoGenerateColumns = true;
                dgvPersonnelReport.DataSource = null;
                dgvPersonnelReport.DataSource = data;

                CustomizeGrid();
                AddSttColumn();

                lblTotalCount.Text = data.Count.ToString();
                lblPresentCount.Text = data.Count(x => x.IsPresent).ToString();
                lblAbsentCount.Text = data.Count(x => !x.IsPresent).ToString();

                lblStatus.Text =
                    $"Đang hiển thị {data.Count} CBCS";
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

        private void CustomizeGrid()
        {
            if (dgvPersonnelReport.Columns.Count == 0)
                return;

            //--------------------------------------------------
            // Ẩn cột
            //--------------------------------------------------

            dgvPersonnelReport.Columns["PersonnelId"].Visible = false;
            dgvPersonnelReport.Columns["MissionStart"].Visible = false;
            dgvPersonnelReport.Columns["MissionEnd"].Visible = false;

            //--------------------------------------------------
            // Đổi tên
            //--------------------------------------------------

            dgvPersonnelReport.Columns["SoHieu"].HeaderText = "Số hiệu";
            dgvPersonnelReport.Columns["HoTen"].HeaderText = "Họ tên";
            dgvPersonnelReport.Columns["DonVi"].HeaderText = "Đơn vị";
            dgvPersonnelReport.Columns["CapBacHam"].HeaderText = "Cấp bậc";
            dgvPersonnelReport.Columns["Status"].HeaderText = "Trạng thái";
            dgvPersonnelReport.Columns["Reason"].HeaderText = "Lý do";
            dgvPersonnelReport.Columns["MissionName"].HeaderText = "Nhiệm vụ";
            dgvPersonnelReport.Columns["Location"].HeaderText = "Địa điểm";

            //--------------------------------------------------
            // Giao diện
            //--------------------------------------------------

            dgvPersonnelReport.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvPersonnelReport.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 10F, FontStyle.Bold);

            dgvPersonnelReport.ColumnHeadersHeight = 38;

            dgvPersonnelReport.EnableHeadersVisualStyles = false;

            dgvPersonnelReport.RowTemplate.Height = 32;

            dgvPersonnelReport.AllowUserToResizeRows = false;
        }

        private async void FrmPersonnelReport_Load(
            object sender,
            EventArgs e)
        {
            await LoadPersonnelReportAsync();
        }

        private void AddSttColumn()
        {
            if (dgvPersonnelReport.Columns.Contains("STT"))
                return;

            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();

            col.Name = "STT";
            col.HeaderText = "STT";
            col.Width = 60;
            col.ReadOnly = true;

            dgvPersonnelReport.Columns.Insert(0, col);

            for (int i = 0; i < dgvPersonnelReport.Rows.Count; i++)
            {
                dgvPersonnelReport.Rows[i].Cells["STT"].Value = i + 1;
            }
        }

        private async void btnCopyReport_Click(
            object sender,
            EventArgs e)
        {
            string text =
                await _reportService
                    .BuildReportTextAsync(
                        dtpReportDate.Value);

            Clipboard.SetText(text);

            MessageBox.Show(
                "Đã copy báo cáo vào Clipboard.",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private async void btnPreview_Click(object sender, EventArgs e)
        {
            string report =
                await _reportService.BuildReportTextAsync(
                    dtpReportDate.Value);

            FrmReportPreview frm =
                new FrmReportPreview(report);

            frm.ShowDialog();
        }
    }
}