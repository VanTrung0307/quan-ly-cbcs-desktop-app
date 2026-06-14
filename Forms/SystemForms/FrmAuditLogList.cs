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

        private ToolTip _toolTip = new();
        public FrmAuditLogList()
        {
            InitializeComponent();

            PermissionHelper.CheckAdmin();

            SetupTooltips();
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

        private async void FrmAuditLogList_Load(
            object sender,
            EventArgs e)
        {
            if (CurrentUser.Role != "Admin")
            {
                MessageBox.Show(
                    "Bạn không có quyền truy cập chức năng này.");

                this.BeginInvoke(() => Close());
                return;
            }

            SetupDataGridView();

            await LoadAuditLogsAsync();
        }

        private async void btnRefresh_Click(
            object sender,
            EventArgs e)
        {
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
            string keyword =
                txtSearch.Text.Trim();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                await LoadAuditLogsAsync();
                return;
            }

            var data =
                await _auditService
                    .SearchAsync(keyword);

            dgvAuditLogs.DataSource = data;
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
            object? sender,
            DataGridViewCellFormattingEventArgs e)
        {
            var style = e.CellStyle;

            if (e.RowIndex < 0)
                return;

            if (style == null)
                return;

            string columnName =
                dgvAuditLogs.Columns[e.ColumnIndex]
                .Name;

            if (columnName != "Action")
                return;

            string action =
                e.Value?.ToString() ?? "";

            if (action == "LOGIN")
            {
                style.BackColor =
                    Color.FromArgb(
                        39,
                        174,
                        96);

                style.ForeColor =
                    Color.White;

                style.SelectionBackColor =
                    Color.FromArgb(
                        39,
                        174,
                        96);

                style.SelectionForeColor =
                    Color.White;
            }
            else if (action == "FAILED_LOGIN")
            {
                style.BackColor =
                    Color.FromArgb(
                        231,
                        76,
                        60);

                style.ForeColor =
                    Color.White;

                style.SelectionBackColor =
                    Color.FromArgb(
                        231,
                        76,
                        60);

                style.SelectionForeColor =
                    Color.White;
            }
            else if (action == "LOGOUT")
            {
                style.BackColor =
                    Color.FromArgb(
                        243,
                        156,
                        18);

                style.ForeColor =
                    Color.White;

                style.SelectionBackColor =
                    Color.FromArgb(
                        243,
                        156,
                        18);

                style.SelectionForeColor =
                    Color.White;
            }
            else if (action == "RESET_PASSWORD")
            {
                style.BackColor =
                    Color.FromArgb(
                        52,
                        152,
                        219);

                style.ForeColor =
                    Color.White;

                style.SelectionBackColor =
                    Color.FromArgb(
                        52,
                        152,
                        219);

                style.SelectionForeColor =
                    Color.White;
            }
        }

        private void SetupTooltips()
        {
            _toolTip.SetToolTip(
                btnSearch,
                "Tìm kiếm nhật ký");

            _toolTip.SetToolTip(
                btnRefresh,
                "Tải lại dữ liệu");
        }
    }
}
