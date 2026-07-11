using QuanLyCBCS.Helpers;
using QuanLyCBCS.Models;
using QuanLyCBCS.Services;
using QuanLyCBCS.Common;

namespace QuanLyCBCS.Forms.MissionForms
{
    public partial class FrmMissionDetail : Form
    {
        private readonly MissionService
            _missionService =
                new();

        private Mission? _mission;

        public FrmMissionDetail()
        {
            InitializeComponent();

            SetupMissionType();

            LoadPriority();

            chkIsActive.Checked = true;
        }

        public FrmMissionDetail(
            Mission mission)
        {
            InitializeComponent();

            SetupMissionType();

            LoadPriority();

            _mission = mission;

            LoadMissionData();
        }

        private void SetupMissionType()
        {
            cboMissionType.Items.Clear();

            cboMissionType.Items.AddRange(
            [
                MissionTypes.DutyCommander,
                MissionTypes.DutyOfficer,

                MissionTypes.TargetProtection,

                MissionTypes.Patrol,

                MissionTypes.SSCD,

                MissionTypes.Training,

                MissionTypes.Meeting,

                MissionTypes.Other,

                MissionTypes.Guard,

                MissionTypes.Escort,

                MissionTypes.FireSupport,

                MissionTypes.TrafficSupport
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

            cboPriority.SelectedValue =
                _mission.Priority;
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

                mission.Priority =
                    GetSelectedPriority();

                await _missionService
                    .AddAsync(mission);

                await AuditHelper.LogAsync(
                    AuditAction.AddMission,
                    $"Thêm công tác '{mission.MissionName}' tại '{mission.Location}'.");
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

                _mission.Priority =
                    GetSelectedPriority();

                await _missionService
                    .UpdateAsync(_mission);

                await AuditHelper.LogAsync(
                    AuditAction.UpdateMission,
                    $"Cập nhật công tác '{_mission.MissionName}' tại '{_mission.Location}'.");
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

        public class PriorityItem
        {
            public int Value { get; set; }

            public string Text { get; set; } = "";

            public override string ToString()
            {
                return Text;
            }
        }

        private void LoadPriority()
        {
            cboPriority.DataSource = new List<PriorityItem>
            {
                new PriorityItem
                {
                    Value = 1,
                    Text = "1 - Thấp"
                },

                new PriorityItem
                {
                    Value = 2,
                    Text = "2 - Bình thường"
                },

                new PriorityItem
                {
                    Value = 3,
                    Text = "3 - Cao"
                },

                new PriorityItem
                {
                    Value = 4,
                    Text = "4 - Khẩn"
                }
            };

            cboPriority.DisplayMember = nameof(PriorityItem.Text);
            cboPriority.ValueMember = nameof(PriorityItem.Value);

            cboPriority.SelectedValue = 2;
        }

        private int GetSelectedPriority()
        {
            if (cboPriority.SelectedValue is int priority)
                return priority;

            return 2;
        }

    }
}
