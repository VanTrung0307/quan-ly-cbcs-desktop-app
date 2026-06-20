using QuanLyCBCS.Services;
using QuanLyCBCS.DTOs;

namespace QuanLyCBCS.Forms.MissionForms
{
    public partial class FrmPersonnelMissionHistory : Form
    {

        private readonly int _personnelId;

        private readonly string _hoTen;

        private readonly MissionPersonnelService
            _missionPersonnelService =
                new();

        private List<PersonnelMissionHistoryDto>
            _history =
                new();
        public FrmPersonnelMissionHistory(
            int personnelId,
            string hoTen)
        {
            InitializeComponent();

            _personnelId = personnelId;
            _hoTen = hoTen;

            lblPersonnelInfo.Text =
                $"CBCS: {hoTen}";

            Load += FrmPersonnelMissionHistory_Load;
        }

        private async void
        FrmPersonnelMissionHistory_Load(
            object? sender,
            EventArgs e)
        {
            await LoadHistoryAsync();
        }

        private async Task LoadHistoryAsync()
        {
            _history =
                await _missionPersonnelService
                    .GetPersonnelMissionHistoryAsync(
                        _personnelId);

            dgvHistory.DataSource = null;
            dgvHistory.DataSource = _history;

            for (int i = 0;
                 i < dgvHistory.Rows.Count;
                 i++)
            {
                dgvHistory.Rows[i]
                    .Cells["colStt"]
                    .Value = i + 1;
            }

            CustomizeGrid();
        }

        private void CustomizeGrid()
        {
            if (dgvHistory.Columns["NgayPhanCong"] != null)
            {
                dgvHistory.Columns["NgayPhanCong"]
                    .DefaultCellStyle.Format =
                        "dd/MM/yyyy HH:mm";
            }

            if (dgvHistory.Columns["BatDau"] != null)
            {
                dgvHistory.Columns["BatDau"].Visible = false;
            }

            if (dgvHistory.Columns["KetThuc"] != null)
            {
                dgvHistory.Columns["KetThuc"].Visible = false;
            }

            dgvHistory.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
        }
    }
}
