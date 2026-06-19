using QuanLyCBCS.DTOs;
using QuanLyCBCS.Services;

namespace QuanLyCBCS.Forms.MissionForms;

public partial class FrmPersonnelMissionStatistics : Form
{
    private readonly MissionPersonnelService
        _missionPersonnelService = new();

    private List<PersonnelMissionStatisticDto>
        _statistics = new();

    public FrmPersonnelMissionStatistics()
    {
        InitializeComponent();
    }

    private async Task LoadStatisticsAsync()
    {
        _statistics =
            await _missionPersonnelService
                .GetPersonnelMissionStatisticsAsync();

        dgvStatistics.DataSource = null;
        dgvStatistics.DataSource = _statistics;

        for (int i = 0; i < dgvStatistics.Rows.Count; i++)
        {
            dgvStatistics.Rows[i]
                .Cells["colSTT"]
                .Value = i + 1;
        }
    }

    private void CustomizeGrid()
    {
        dgvStatistics.AutoGenerateColumns = false;

        dgvStatistics.AllowUserToAddRows = false;
        dgvStatistics.AllowUserToDeleteRows = false;

        dgvStatistics.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect;

        dgvStatistics.MultiSelect = false;

        dgvStatistics.ReadOnly = true;

        dgvStatistics.RowHeadersVisible = false;

        dgvStatistics.AutoSizeColumnsMode =
            DataGridViewAutoSizeColumnsMode.Fill;

        dgvStatistics.Columns["colSTT"].FillWeight = 50;

        dgvStatistics.Columns["colSoLanCongTac"]
            .DefaultCellStyle.Alignment =
            DataGridViewContentAlignment.MiddleCenter;

        dgvStatistics.RowHeadersVisible = false;

        dgvStatistics.AllowUserToAddRows = false;

        dgvStatistics.AllowUserToDeleteRows = false;

        dgvStatistics.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect;

        dgvStatistics.MultiSelect = false;

        dgvStatistics.ReadOnly = true;

        dgvStatistics.AutoSizeColumnsMode =
            DataGridViewAutoSizeColumnsMode.Fill;

        dgvStatistics.ColumnHeadersHeight = 40;

        dgvStatistics.Columns["colSTT"]
            .FillWeight = 40;

        dgvStatistics.Columns["colSoLanCongTac"]
            .DefaultCellStyle.Alignment =
            DataGridViewContentAlignment.MiddleCenter;
    }

    private async void
    FrmPersonnelMissionStatistics_Load(
        object sender,
        EventArgs e)
    {
        CustomizeGrid();

        await LoadStatisticsAsync();
    }

    private async void
    btnRefresh_Click(
        object sender,
        EventArgs e)
    {
        await LoadStatisticsAsync();
    }

    private void
    btnClose_Click(
        object sender,
        EventArgs e)
    {
        Close();
    }
}