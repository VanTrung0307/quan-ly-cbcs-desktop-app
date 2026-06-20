using QuanLyCBCS.Helpers;
using QuanLyCBCS.Models;
using QuanLyCBCS.Services;

namespace QuanLyCBCS.Forms.MissionForms
{
    public partial class FrmMissionDetail : Form
    {
        private readonly MissionService
            _missionService =
                new();

        private Mission? _mission;

        private readonly AuditLogService
            _auditLogService =
                new();

        public FrmMissionDetail()
        {
            InitializeComponent();

            SetupMissionType();

            chkIsActive.Checked = true;
        }

        public FrmMissionDetail(
            Mission mission)
        {
            InitializeComponent();

            SetupMissionType();

            _mission = mission;

            LoadMissionData();
        }

        private void SetupMissionType()
        {
            cboMissionType.Items.Clear();

            cboMissionType.Items.AddRange(
            [
                "Bảo vệ mục tiêu",
                "Bảo vệ ngân hàng",
                "Trưng dụng",
                "Tăng cường",
                "Ứng trực",
                "Huấn luyện",
                "Học tập",
                "Thi đấu",
                "Diễn tập",
                "Khác"
            ]);

            cboMissionType.SelectedIndex = 0;
        }

        private void LoadMissionData()
        {
            if (_mission == null)
                return;

            txtMissionName.Text =
                _mission.MissionName;

            cboMissionType.Text =
                _mission.MissionType;

            txtLocation.Text =
                _mission.Location;

            rtbDescription.Text =
                _mission.Description;

            dtpStartTime.Value =
                _mission.StartTime;

            dtpEndTime.Value =
                _mission.EndTime;

            chkIsActive.Checked =
                _mission.IsActive;
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(
                txtMissionName.Text))
            {
                MessageBox.Show(
                    "Vui lòng nhập tên công tác.");

                txtMissionName.Focus();

                return false;
            }

            if (dtpEndTime.Value
                <= dtpStartTime.Value)
            {
                MessageBox.Show(
                    "Thời gian kết thúc phải lớn hơn thời gian bắt đầu.");

                dtpEndTime.Focus();

                return false;
            }

            return true;
        }

        private async Task SaveAsync()
        {
            if (!ValidateInput())
                return;

            if (_mission == null)
            {
                Mission mission =
                    new Mission();

                mission.MissionName =
                    txtMissionName.Text.Trim();

                mission.MissionType =
                    cboMissionType.Text;

                mission.Location =
                    txtLocation.Text.Trim();

                mission.Description =
                    rtbDescription.Text.Trim();

                mission.StartTime =
                    dtpStartTime.Value;

                mission.EndTime =
                    dtpEndTime.Value;

                mission.IsActive =
                    chkIsActive.Checked;

                await _missionService
                    .AddAsync(mission);

                await _auditLogService
                    .AddLogAsync(
                        "THÊM CÔNG TÁC",
                        mission.MissionName);
            }
            else
            {
                _mission.MissionName =
                    txtMissionName.Text.Trim();

                _mission.MissionType =
                    cboMissionType.Text;

                _mission.Location =
                    txtLocation.Text.Trim();

                _mission.Description =
                    rtbDescription.Text.Trim();

                _mission.StartTime =
                    dtpStartTime.Value;

                _mission.EndTime =
                    dtpEndTime.Value;

                _mission.IsActive =
                    chkIsActive.Checked;

                await _missionService
                    .UpdateAsync(_mission);

                await _auditLogService
                    .AddLogAsync(
                        "CẬP NHẬT CÔNG TÁC",
                        _mission.MissionName);
            }

            DialogResult =
                DialogResult.OK;
        }

        private async void btnSave_Click(
            object sender,
            EventArgs e)
        {
            await SaveAsync();
        }

        private void btnCancel_Click(
            object sender,
            EventArgs e)
        {
            Close();
        }

    }
}
