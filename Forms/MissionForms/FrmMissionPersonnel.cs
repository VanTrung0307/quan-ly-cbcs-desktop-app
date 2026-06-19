using QuanLyCBCS.Models;
using QuanLyCBCS.Services;
using QuanLyCBCS.DTOs;

namespace QuanLyCBCS.Forms.MissionForms
{
    public partial class FrmMissionPersonnel : Form
    {
        private readonly int _missionId;

        private readonly MissionPersonnelService
            _missionPersonnelService =
                new();

        private readonly PersonnelService
            _personnelService =
                new();

        private List<MissionPersonnelDto>
            _availablePersonnel =
                new();

        private List<MissionPersonnelDto>
            _assignedPersonnel =
                new();

        public FrmMissionPersonnel(
            int missionId,
            string missionName)
        {
            InitializeComponent();

            _missionId = missionId;

            lblMissionName.Text =
                $"Công tác: {missionName}";

            this.Load += FrmMissionPersonnel_Load;
        }

        private async Task LoadPersonnelAsync()
        {
            CustomizeGrid();

            _availablePersonnel =
                await _missionPersonnelService
                    .GetAvailablePersonnelDtoAsync(
                        _missionId);

            _assignedPersonnel =
                await _missionPersonnelService
                    .GetAssignedPersonnelDtoAsync(
                        _missionId);

            dgvAvailable.DataSource = null;
            dgvAvailable.DataSource = _availablePersonnel;

            dgvAssigned.DataSource = null;
            dgvAssigned.DataSource = _assignedPersonnel;

            for (int i = 0; i < dgvAvailable.Rows.Count; i++)
            {
                dgvAvailable.Rows[i]
                    .Cells["colSttAvailable"]
                    .Value = i + 1;
            }

            for (int i = 0; i < dgvAssigned.Rows.Count; i++)
            {
                dgvAssigned.Rows[i]
                    .Cells["colSttAssigned"]
                    .Value = i + 1;
            }

            UpdateCounts();
        }

        private void UpdateCounts()
        {
            lblAvailableCount.Text =
                $"Chưa phân công: {_availablePersonnel.Count} CBCS";

            lblAssignedCount.Text =
                $"Đã phân công: {_assignedPersonnel.Count} CBCS";
        }

        private async void
        FrmMissionPersonnel_Load(
            object? sender,
            EventArgs e)
        {
            await LoadPersonnelAsync();
        }

        private void CustomizeGrid()
        {
            dgvAvailable.AutoGenerateColumns = false;
            dgvAssigned.AutoGenerateColumns = false;

            dgvAvailable.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvAssigned.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvAvailable.MultiSelect = false;
            dgvAssigned.MultiSelect = false;

            dgvAvailable.ReadOnly = true;
            dgvAssigned.ReadOnly = true;

            dgvAvailable.AllowUserToAddRows = false;
            dgvAssigned.AllowUserToAddRows = false;

            dgvAvailable.RowHeadersVisible = false;
            dgvAssigned.RowHeadersVisible = false;
        }

        private async void btnAssign_Click(
            object sender,
            EventArgs e)
        {
            await AssignSelectedPersonnelAsync();
        }

        private async void btnRemove_Click(
            object sender,
            EventArgs e)
        {
            await RemoveSelectedPersonnelAsync();
        }

        private void txtSearch_TextChanged(
            object sender,
            EventArgs e)
        {
            string keyword =
                txtSearch.Text
                .Trim()
                .ToLower();

            dgvAvailable.DataSource =
                _availablePersonnel
                .Where(x =>
                    x.HoTen.ToLower()
                        .Contains(keyword)

                    ||

                    x.SoHieu.ToLower()
                        .Contains(keyword))
                .ToList();
        }

        private async void
        dgvAvailable_CellDoubleClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            await AssignSelectedPersonnelAsync();
        }

        private async Task
        AssignSelectedPersonnelAsync()
        {
            if (dgvAvailable.CurrentRow == null)
                return;

            var selected =
                (MissionPersonnelDto)
                dgvAvailable.CurrentRow
                    .DataBoundItem;

            await _missionPersonnelService
                .AddAsync(
                    _missionId,
                    selected.PersonnelId);

            await LoadPersonnelAsync();
        }

        private async Task
        RemoveSelectedPersonnelAsync()
        {
            if (dgvAssigned.CurrentRow == null)
                return;

            var selected =
                (MissionPersonnelDto)
                dgvAssigned.CurrentRow
                    .DataBoundItem;

            await _missionPersonnelService
                .RemoveAsync(
                    _missionId,
                    selected.PersonnelId);

            await LoadPersonnelAsync();
        }

        private async void
        dgvAssigned_CellDoubleClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            await RemoveSelectedPersonnelAsync();
        }

        private void
        btnClose_Click(
            object sender,
            EventArgs e)
        {
            Close();
        }
    }
}
