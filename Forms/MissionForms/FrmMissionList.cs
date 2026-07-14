using QuanLyCBCS.Helpers;
using QuanLyCBCS.Models;
using QuanLyCBCS.Services;

namespace QuanLyCBCS.Forms.MissionForms
{
    public partial class FrmMissionList : Form
    {
        private readonly MissionService
            _missionService =
                new();

        private List<Mission>
            _allMissions =
                new();

        private readonly Font _priorityBoldFont;

        public FrmMissionList()
        {
            InitializeComponent();

            dgvMission.KeyDown += dgvMission_KeyDown;

            dgvMission.CellDoubleClick += 
                dgvMission_CellDoubleClick;

            dgvMission.CellFormatting +=
                dgvMission_CellFormatting;

            _priorityBoldFont =
                new Font(
                    dgvMission.Font,
                    FontStyle.Bold);
        }

        private async Task
            LoadMissionAsync()
        {
            _allMissions =
                await _missionService
                    .GetAllAsync();

            dgvMission.DataSource =
                _allMissions;

            CustomizeMissionGrid();

            AddSttColumn();

            UpdateRecordCount(
                _allMissions.Count); ;
        }

        private async void
        FrmMissionList_Load(
            object sender,
            EventArgs e)
        {
            await LoadMissionAsync();

            btnAddMission.Visible =
                PermissionHelper.CanManageMission;

            btnEditMission.Visible =
                PermissionHelper.CanManageMission;

            btnDeleteMission.Visible =
                PermissionHelper.CanManageMission;
        }

        private void
        UpdateRecordCount(
            int count)
        {
            lblRecordCount.Text =
                $"Đang hiển thị: {count} công tác";
        }

        private void
        CustomizeMissionGrid()
        {
            if (dgvMission.Columns.Count == 0)
                return;

            dgvMission.Columns[
                "MissionId"]
                .Visible = false;

            //dgvMission.Columns[
            //    "MissionPersonnel"]
            //    .Visible = false;

            dgvMission.Columns[
                "CreatedAt"]
                .Visible = false;

            dgvMission.Columns[
                "MissionName"]
                .HeaderText =
                    "Tên công tác";

            dgvMission.Columns[
                "MissionType"]
                .HeaderText =
                    "Loại công tác";

            dgvMission.Columns[
                "StartTime"]
                .HeaderText =
                    "Bắt đầu";

            dgvMission.Columns[
                "EndTime"]
                .HeaderText =
                    "Kết thúc";

            dgvMission.Columns[
                "Location"]
                .HeaderText =
                    "Địa điểm";

            dgvMission.Columns[
                "Description"]
                .HeaderText =
                    "Mô tả";

            dgvMission.Columns["Priority"]
                .DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;

            dgvMission.Columns["Priority"]
                .HeaderCell.Style.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;

            dgvMission.Columns["MissionType"].Width = 130;
            dgvMission.Columns["StartTime"].Width = 150;
            dgvMission.Columns["EndTime"].Width = 150;
            dgvMission.Columns["Location"].Width = 180;
        }

        private void AddSttColumn()
        {
            if (dgvMission.Columns["STT"] == null)
            {
                DataGridViewTextBoxColumn sttColumn =
                    new DataGridViewTextBoxColumn();

                sttColumn.Name = "STT";
                sttColumn.HeaderText = "STT";
                sttColumn.Width = 50;
                sttColumn.ReadOnly = true;

                dgvMission.Columns.Insert(
                    0,
                    sttColumn);
            }

            dgvMission.Columns["STT"].Visible = true;
            dgvMission.Columns["STT"].Frozen = true;

            for (int i = 0;
                 i < dgvMission.Rows.Count;
                 i++)
            {
                dgvMission.Rows[i]
                    .Cells["STT"]
                    .Value = i + 1;
            }
        }

        private async void btnAdd_Click(
            object sender,
            EventArgs e)
        {
            FrmMissionDetail frm =
                new();

            if (frm.ShowDialog()
                == DialogResult.OK)
            {
                await LoadMissionAsync();
            }
        }

        private async void btnEdit_Click(
            object sender,
            EventArgs e)
        {
            await EditMissionAsync();
        }

        private async void btnDelete_Click(
            object sender,
            EventArgs e)
        {
            if (dgvMission.CurrentRow == null)
                return;

            var mission =
                dgvMission.CurrentRow
                    .DataBoundItem as Mission;

            if (mission == null)
                return;

            DialogResult result =
                MessageBox.Show(
                    $"Xóa công tác '{mission.MissionName}' ?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            await _missionService
                .DeleteAsync(
                    mission.MissionId);

            await LoadMissionAsync();
        }

        private async void dgvMission_CellDoubleClick(
            object? sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex < 0)
                return;

            await EditMissionAsync();
        }

        private async Task EditMissionAsync()
        {
            if (dgvMission.CurrentRow == null)
            {
                MessageBox.Show(
                    "Vui lòng chọn công tác!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            Mission? mission =
                dgvMission.CurrentRow
                    .DataBoundItem as Mission;

            if (mission == null)
                return;

            FrmMissionDetail frm =
                new(mission);

            if (frm.ShowDialog() == DialogResult.OK)
            {
                await LoadMissionAsync();
            }
        }

        private async void dgvMission_KeyDown(
            object? sender,
            KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                await EditMissionAsync();
            }
        }

        private void btnAssign_Click(
            object sender,
            EventArgs e)
        {
            if (dgvMission.CurrentRow == null)
            {
                MessageBox.Show(
                    "Vui lòng chọn công tác.");

                return;
            }

            Mission mission =
                (Mission)dgvMission
                    .CurrentRow
                    .DataBoundItem;

            FrmMissionPersonnel frm =
                new(
                    mission.MissionId,
                    mission.MissionName);

            frm.ShowDialog();
        }

        private void dgvMission_CellFormatting(        
            object? sender,
            DataGridViewCellFormattingEventArgs e)
    
        {
            if (dgvMission.Columns[e.ColumnIndex].Name != "Priority")
                return;

            if (e.Value == null)
                return;

            if (e.CellStyle is null)
                return;

            var style = e.CellStyle;

            int priority =
                Convert.ToInt32(e.Value);

            switch (priority)
            {
                case 4:

                    e.Value = "Khẩn";

                    style.BackColor =
                        Color.FromArgb(255, 199, 206);

                    style.ForeColor =
                        Color.DarkRed;

                    style.SelectionBackColor =
                        Color.Red;

                    style.SelectionForeColor =
                        Color.White;

                    style.Font =
                        _priorityBoldFont;

                    break;

                case 3:

                    e.Value = "Cao";

                    style.BackColor =
                        Color.FromArgb(255, 235, 156);

                    style.ForeColor =
                        Color.DarkOrange;

                    style.Font =
                        _priorityBoldFont;

                    break;

                case 2:

                    e.Value = "Bình thường";

                    style.BackColor =
                        Color.FromArgb(221, 235, 247);

                    style.ForeColor =
                        Color.DarkBlue;

                    break;

                case 1:

                    e.Value = "Thấp";

                    style.BackColor =
                        Color.Gainsboro;

                    style.ForeColor =
                        Color.DimGray;

                    break;
            }

            e.FormattingApplied = true;
        }

        private async void btnRefresh_Click(
            object? sender,
            EventArgs e)
        {
            await LoadMissionAsync();
        }
    }
}
