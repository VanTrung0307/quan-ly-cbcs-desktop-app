using QuanLyCBCS.Services;
using QuanLyCBCS.DTOs;

namespace QuanLyCBCS.Forms.MissionForms
{
    public partial class FrmMissionDashboard : Form
    {
        private readonly MissionService
            _missionService =
                new();
        public FrmMissionDashboard()
        {
            InitializeComponent();
        }

        private async void
            FrmMissionDashboard_Load(
                object sender,
                EventArgs e)
        {
            await LoadDashboardAsync();
        }

        private async Task
            LoadDashboardAsync()
        {
            var dashboard =
                await _missionService
                    .GetDashboardAsync();

            lblTongCongTac.Text =
                dashboard.TongCongTac
                    .ToString();

            lblDangHoatDong.Text =
                dashboard.DangHoatDong
                    .ToString();

            lblDaKetThuc.Text =
                dashboard.DaKetThuc
                    .ToString();

            lblTopPersonnel.Text =
                dashboard.TopPersonnelName
                ?? "Chưa có dữ liệu";

            lblTopPersonnelCount.Text =
                $"{dashboard.TopPersonnelCount} lần";

            await LoadRecentMissionAsync();

            await LoadTopPersonnelAsync();
        }

        private async void
            btnRefresh_Click(
                object sender,
                EventArgs e)
        {
            await LoadDashboardAsync();
        }

        private void
            btnClose_Click(
                object sender,
                EventArgs e)
        {
            Close();
        }

        private async Task
            LoadRecentMissionAsync()
        {
            var missions =
                await _missionService
                    .GetRecentMissionsAsync();

            dgvRecentMission.DataSource =
                null;

            dgvRecentMission.DataSource =
                missions;

            dgvRecentMission.Columns["MissionId"]
                .Visible = false;
        }

        private async Task
            LoadTopPersonnelAsync()
        {
            dgvTopPersonnel.DataSource =
                await _missionService
                    .GetTopPersonnelAsync();

            dgvTopPersonnel.Columns["PersonnelId"]
                .Visible = false;

            dgvTopPersonnel.Columns["SoLanCongTac"]
                .HeaderText =
                    "Số lần CT";

            dgvTopPersonnel.Columns["HoTen"]
                .HeaderText =
                    "Họ tên";

            dgvTopPersonnel.Columns["DonVi"]
                .HeaderText =
                    "Đơn vị";

            dgvTopPersonnel.Columns["SoHieu"]
                .HeaderText =
                    "Số hiệu";
        }
    }
}
