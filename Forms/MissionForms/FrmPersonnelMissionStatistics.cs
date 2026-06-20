using QuanLyCBCS.DTOs;
using QuanLyCBCS.Services;

namespace QuanLyCBCS.Forms.MissionForms;

public partial class FrmPersonnelMissionStatistics : Form
{
    private readonly MissionPersonnelService
        _missionPersonnelService = new();

    private List<PersonnelMissionStatisticDto>
        _statistics = new();

    private readonly
        PersonnelMissionStatisticExportService
            _exportService =
                new();

    public FrmPersonnelMissionStatistics()
    {
        InitializeComponent();
    }

    private async Task LoadStatisticsAsync()
    {
        _statistics =
            await _missionPersonnelService
                .GetPersonnelMissionStatisticsAsync();

        lblTotalPersonnel.Text =
            $"Tổng CBCS: {_statistics.Count}";

        lblTotalMissionCount.Text =
            $"Tổng lượt công tác: {_statistics.Sum(x => x.SoLanCongTac)}";

        dgvStatistics.DataSource = null;
        dgvStatistics.DataSource = _statistics;

        for (int i = 0; i < dgvStatistics.Rows.Count; i++)
        {
            dgvStatistics.Rows[i]
                .Cells["colSTT"]
                .Value = i + 1;
        }
        if (dgvStatistics.Rows.Count > 0)
        {
            dgvStatistics.Rows[0]
                .DefaultCellStyle.BackColor =
                Color.Gold;
        }

        if (dgvStatistics.Rows.Count > 1)
        {
            dgvStatistics.Rows[1]
                .DefaultCellStyle.BackColor =
                Color.Silver;
        }

        if (dgvStatistics.Rows.Count > 2)
        {
            dgvStatistics.Rows[2]
                .DefaultCellStyle.BackColor =
                Color.Peru;
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

    private void btnExportExcel_Click(
        object sender,
        EventArgs e)
    {
        SaveFileDialog saveFileDialog =
            new();

        saveFileDialog.Filter =
            "Excel File|*.xlsx";

        saveFileDialog.FileName =
            $"ThongKeCongTac_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

        if (saveFileDialog.ShowDialog()
            != DialogResult.OK)
            return;

        try
        {
            _exportService.Export(
                _statistics,
                saveFileDialog.FileName);

            MessageBox.Show(
                "Xuất Excel thành công.",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
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

    private void txtSearch_TextChanged(
        object sender,
        EventArgs e)
    {
        string keyword =
            txtSearch.Text
                .Trim()
                .ToLower();

        var filtered =
            _statistics
                .Where(x =>
                    x.HoTen.ToLower()
                        .Contains(keyword)
                    ||
                    x.SoHieu.ToLower()
                        .Contains(keyword))
                .ToList();

        dgvStatistics.DataSource = null;
        dgvStatistics.DataSource = filtered;

        for (int i = 0; i < dgvStatistics.Rows.Count; i++)
        {
            dgvStatistics.Rows[i]
                .Cells["colSTT"]
                .Value = i + 1;
        }
    }

    private void btnTop10_Click(
        object sender,
        EventArgs e)
    {
        dgvStatistics.DataSource = null;

        dgvStatistics.DataSource =
            _statistics
                .OrderByDescending(x =>
                    x.SoLanCongTac)
                .Take(10)
                .ToList();
    }

    private void btnHistory_Click(
        object sender,
        EventArgs e)
    {
        if (dgvStatistics.CurrentRow == null)
        {
            MessageBox.Show(
                "Vui lòng chọn CBCS!",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return;
        }

        var selected =
            dgvStatistics.CurrentRow
            .DataBoundItem
            as PersonnelMissionStatisticDto;

        if (selected == null)
            return;

        FrmPersonnelMissionHistory frm =
            new FrmPersonnelMissionHistory(
                selected.PersonnelId,
                selected.HoTen);

        frm.ShowDialog();
    }
}