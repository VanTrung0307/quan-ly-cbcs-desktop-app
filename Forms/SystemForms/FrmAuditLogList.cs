using QuanLyCBCS.Helpers;
using QuanLyCBCS.Services;


namespace QuanLyCBCS.Forms.SystemForms
{
    public partial class FrmAuditLogList : Form
    {
        private readonly AuditLogService
            _auditService =
                new AuditLogService();
        public FrmAuditLogList()
        {
            InitializeComponent();

            PermissionHelper.CheckAdmin();
        }

        private async Task LoadAuditLogsAsync()
        {
            dgvAuditLogs.DataSource = null;

            dgvAuditLogs.DataSource =
                await _auditService
                    .GetAllAsync();

            AddSttColumn();
            CustomizeAuditGrid();
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
    }
}
