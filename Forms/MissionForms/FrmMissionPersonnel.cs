using QuanLyCBCS.DTOs;
using QuanLyCBCS.Helpers;
using QuanLyCBCS.Models;
using QuanLyCBCS.Services;

namespace QuanLyCBCS.Forms.MissionForms
{
    public partial class FrmMissionPersonnel : Form
    {
        private readonly int _missionId;

        private readonly MissionPersonnelService
            _missionPersonnelService =
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

            CustomizeGrid();

            _missionId = missionId;

            lblMissionName.Text =
                $"Công tác: {missionName}";
        }

        private async Task LoadPersonnelAsync()
        {
            _availablePersonnel =
                await _missionPersonnelService
                    .GetAvailablePersonnelDtoAsync(
                        _missionId);

            _assignedPersonnel =
                await _missionPersonnelService
                    .GetAssignedPersonnelDtoAsync(
                        _missionId);

            dgvAvailable.DataSource = _availablePersonnel;
            dgvAssigned.DataSource = _assignedPersonnel;

            dgvAssigned.Columns[
                "colNgayPhanCongAssigned"]
                .DefaultCellStyle.Format =
                    "dd/MM/yyyy HH:mm";

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
                VietnameseHelper
                    .RemoveVietnameseSigns(
                        txtSearch.Text.Trim())
                    .ToLower();

            dgvAvailable.DataSource =
                _availablePersonnel
                .Where(x =>

                    VietnameseHelper.RemoveVietnameseSigns(x.HoTen)
                        .ToLower()
                        .Contains(keyword)

                    ||

                    VietnameseHelper.RemoveVietnameseSigns(x.SoHieu)
                        .ToLower()
                        .Contains(keyword)

                    ||

                    VietnameseHelper.RemoveVietnameseSigns(x.DonVi)
                        .ToLower()
                        .Contains(keyword)

                )
                .ToList();

            for (int i = 0; i < dgvAvailable.Rows.Count; i++)
            {
                dgvAvailable.Rows[i]
                    .Cells["colSttAvailable"]
                    .Value = i + 1;
            }
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
            if (dgvAvailable.CurrentRow?.DataBoundItem
                is not MissionPersonnelDto selected)
                        {
                            return;
                        }

            try
            {
                await _missionPersonnelService
                    .AddAsync(
                        _missionId,
                        selected.PersonnelId);

                await AuditHelper.LogAsync(
                    AuditAction.AssignMission,
                    $"Phân công CBCS '{selected.HoTen}' ({selected.SoHieu}) vào {lblMissionName.Text}.");

                await LoadPersonnelAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private async Task
        RemoveSelectedPersonnelAsync()
        {
            if (dgvAssigned.CurrentRow?
                    .DataBoundItem
                is not MissionPersonnelDto selected)
            {
                return;
            }

            await _missionPersonnelService
                .RemoveAsync(
                    _missionId,
                    selected.PersonnelId);

            await AuditHelper.LogAsync(
                AuditAction.RemoveMissionAssignment,
                $"Hủy phân công CBCS '{selected.HoTen}' ({selected.SoHieu}) khỏi {lblMissionName.Text}.");

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
