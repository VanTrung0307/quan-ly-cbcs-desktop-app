using Microsoft.VisualBasic.Logging;
using QuanLyCBCS.Helpers;
using QuanLyCBCS.Services;


namespace QuanLyCBCS.Forms.SystemForms
{
    public partial class FrmAuditLogList : Form
    {
        private readonly AuditLogService
            _auditService =
                new AuditLogService();

        private readonly AuditExportService
            _exportService =
                new AuditExportService();

        private ToolTip _toolTip = new();

        private readonly System.Windows.Forms.Timer _searchTimer = new();

        public FrmAuditLogList()
        {
            InitializeComponent();

            if (!PermissionHelper.IsAdmin)
            {
                MessageBox.Show(
                    "Bạn không có quyền truy cập chức năng này.",
                    "Từ chối truy cập",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                BeginInvoke(() => Close());

                return;
            }

            SetupTooltips();

            _searchTimer.Interval = 300;

            _searchTimer.Tick += SearchTimer_Tick;
        }

        private async Task LoadAuditLogsAsync()
        {
            dgvAuditLogs.DataSource = null;

            dgvAuditLogs.DataSource =
                await _auditService
                    .GetAllAsync();

            AddSttColumn();
            CustomizeAuditGrid();

            if (dgvAuditLogs.Columns["AuditLogId"] != null)
            {
                dgvAuditLogs.Columns["AuditLogId"].Visible =
                    false;
            }

            lblStatus.Text =
                $"Đang hiển thị: {dgvAuditLogs.Rows.Count} bản ghi";

        }

        private async Task SearchAuditLogsAsync()
        {
            string keyword = txtSearch.Text.Trim();

            DateTime? fromDate = null;

            DateTime? toDate = null;

            if (chkUseDateFilter.Checked)
            {
                fromDate = dtpFromDate.Value.Date;

                toDate = dtpToDate.Value.Date;
            }

            var data =
                await _auditService.SearchAsync(
                    keyword,
                    fromDate,
                    toDate);

            dgvAuditLogs.DataSource = null;

            dgvAuditLogs.DataSource = data;

            AddSttColumn();

            CustomizeAuditGrid();

            lblStatus.Text =
                $"Đang hiển thị: {data.Count} bản ghi";
        }

        private void ApplyQuickFilter()
        {
            if (cboQuickFilter.SelectedItem == null)
                return;

            string filter =
                cboQuickFilter.SelectedItem.ToString()!;

            switch (filter)
            {
                case "Tất cả":

                    chkUseDateFilter.Checked = false;

                    return;

                case "Hôm nay":

                    chkUseDateFilter.Checked = true;

                    dtpFromDate.Value = DateTime.Today;

                    dtpToDate.Value = DateTime.Today;

                    break;

                case "Hôm qua":

                    chkUseDateFilter.Checked = true;

                    DateTime yesterday =
                        DateTime.Today.AddDays(-1);

                    dtpFromDate.Value = yesterday;

                    dtpToDate.Value = yesterday;

                    break;

                case "7 ngày gần nhất":

                    chkUseDateFilter.Checked = true;

                    dtpFromDate.Value =
                        DateTime.Today.AddDays(-6);

                    dtpToDate.Value =
                        DateTime.Today;

                    break;

                case "30 ngày gần nhất":

                    chkUseDateFilter.Checked = true;

                    dtpFromDate.Value =
                        DateTime.Today.AddDays(-29);

                    dtpToDate.Value =
                        DateTime.Today;

                    break;

                case "Tháng này":

                    chkUseDateFilter.Checked = true;

                    dtpFromDate.Value =
                        new DateTime(
                            DateTime.Today.Year,
                            DateTime.Today.Month,
                            1);

                    dtpToDate.Value =
                        DateTime.Today;

                    break;

                case "Tháng trước":

                    chkUseDateFilter.Checked = true;

                    DateTime firstDayLastMonth =
                        new DateTime(
                            DateTime.Today.Year,
                            DateTime.Today.Month,
                            1).AddMonths(-1);

                    DateTime lastDayLastMonth =
                        new DateTime(
                            DateTime.Today.Year,
                            DateTime.Today.Month,
                            1).AddDays(-1);

                    dtpFromDate.Value =
                        firstDayLastMonth;

                    dtpToDate.Value =
                        lastDayLastMonth;

                    break;

                case "Năm nay":

                    chkUseDateFilter.Checked = true;

                    dtpFromDate.Value =
                        new DateTime(
                            DateTime.Today.Year,
                            1,
                            1);

                    dtpToDate.Value =
                        DateTime.Today;

                    break;
            }
        }

        private async void FrmAuditLogList_Load(
            object sender,
            EventArgs e)
        {
            if (!PermissionHelper.IsAdmin)
            {
                MessageBox.Show(
                    "Bạn không có quyền truy cập chức năng này.");

                this.BeginInvoke(() => Close());
                return;
            }

            SetupDataGridView();

            chkUseDateFilter.Checked = false;

            dtpFromDate.Value = DateTime.Today;

            dtpToDate.Value = DateTime.Today;

            dtpFromDate.Enabled = false;

            dtpToDate.Enabled = false;

            cboQuickFilter.Items.Clear();

            cboQuickFilter.Items.AddRange(new object[]
            {
                "Tất cả",
                "Hôm nay",
                "Hôm qua",
                "7 ngày gần nhất",
                "30 ngày gần nhất",
                "Tháng này",
                "Tháng trước",
                "Năm nay"
            });

            cboQuickFilter.SelectedIndex = 0;

            await LoadAuditLogsAsync();
        }

        private async void btnRefresh_Click(
            object sender,
            EventArgs e)
        {
            txtSearch.Clear();

            await LoadAuditLogsAsync();
        }

        private void AddSttColumn()
        {
            if (dgvAuditLogs.Columns["STT"] != null)
                return;

            DataGridViewTextBoxColumn sttColumn =
                new DataGridViewTextBoxColumn();

            sttColumn.Name = "STT";
            sttColumn.HeaderText = "STT";

            dgvAuditLogs.Columns.Insert(
                0,
                sttColumn);

            foreach (DataGridViewRow row
                in dgvAuditLogs.Rows)
            {
                row.Cells["STT"].Value =
                    row.Index + 1;
            }
        }

        private void CustomizeAuditGrid()
        {
            dgvAuditLogs.RowHeadersVisible = false;

            dgvAuditLogs.Columns["AuditLogId"]
                .Visible = false;

            dgvAuditLogs.Columns["Username"]
                .HeaderText = "Tài khoản";

            dgvAuditLogs.Columns["Action"]
                .HeaderText = "Thao tác";

            dgvAuditLogs.Columns["Description"]
                .HeaderText = "Mô tả";

            dgvAuditLogs.Columns["CreatedAt"]
                .HeaderText = "Thời gian";

            dgvAuditLogs.Columns["CreatedAt"]
                .DefaultCellStyle.Format =
                    "dd/MM/yyyy HH:mm:ss";

            dgvAuditLogs.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvAuditLogs.ReadOnly = true;

            dgvAuditLogs.AllowUserToAddRows =
                false;

            dgvAuditLogs.AllowUserToDeleteRows =
                false;

            foreach (DataGridViewColumn column
                in dgvAuditLogs.Columns)
            {
                column.SortMode =
                    DataGridViewColumnSortMode.NotSortable;
            }

            dgvAuditLogs.Columns["Description"].FillWeight = 180;
            dgvAuditLogs.Columns["CreatedAt"].FillWeight = 120;
        }

        private async void btnSearch_Click(
            object sender,
            EventArgs e)
        {
            await SearchAuditLogsAsync();
        }

        private void SetupDataGridView()
        {
            DataGridViewHelper
                .SetupDefaultStyle(dgvAuditLogs);

            dgvAuditLogs.RowTemplate.Height = 35;

            dgvAuditLogs.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.None;

            dgvAuditLogs.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvAuditLogs.MultiSelect = false;

            dgvAuditLogs.ReadOnly = true;

            dgvAuditLogs.AllowUserToAddRows = false;

            dgvAuditLogs.AllowUserToDeleteRows = false;
        }

        private void dgvAuditLogs_CellFormatting(
            object sender,
            DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (dgvAuditLogs.Columns[e.ColumnIndex].Name != "Action")
                return;

            if (e.Value is not string action)
                return;

            if (string.IsNullOrWhiteSpace(action))
                return;

            AuditStyleHelper.Apply(
                action,
                e.CellStyle);
        }

        private void SetupTooltips()
        {
            _toolTip.SetToolTip(
                btnSearch,
                "Tìm kiếm nhật ký");

            _toolTip.SetToolTip(
                btnRefresh,
                "Tải lại dữ liệu");

            _toolTip.SetToolTip(
                btnExport,
                "Xuất nhật ký ra Excel");
        }

        private void txtSearch_TextChanged(
            object sender,
            EventArgs e)
        {
            _searchTimer.Stop();

            _searchTimer.Start();
        }

        private async void txtSearch_KeyDown(
            object sender,
            KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                _searchTimer.Stop();

                await SearchAuditLogsAsync();
            }
        }

        private async void SearchTimer_Tick(
            object? sender,
            EventArgs e)
        {
            _searchTimer.Stop();

            await SearchAuditLogsAsync();
        }

        private void btnExport_Click(
            object sender,
            EventArgs e)
        {
            if (dgvAuditLogs.Rows.Count == 0)
            {
                MessageBox.Show(
                    "Không có dữ liệu để xuất.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            _exportService.Export(
                dgvAuditLogs);
        }

        private async void dtpFromDate_ValueChanged(
            object sender,
            EventArgs e)
        {
            await SearchAuditLogsAsync();
        }

        private async void dtpToDate_ValueChanged(
            object sender,
            EventArgs e)
        {
            await SearchAuditLogsAsync();
        }

        private async void chkUseDateFilter_CheckedChanged(
            object sender,
            EventArgs e)
        {
            dtpFromDate.Enabled =
                chkUseDateFilter.Checked;

            dtpToDate.Enabled =
                chkUseDateFilter.Checked;

            await SearchAuditLogsAsync();
        }

        private async void cboQuickFilter_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            ApplyQuickFilter();

            await SearchAuditLogsAsync();
        }
    }
}
