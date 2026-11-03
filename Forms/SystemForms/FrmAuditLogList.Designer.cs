namespace QuanLyCBCS.Forms.SystemForms
{
    partial class FrmAuditLogList
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            pnlRoot = new Panel();
            pnlGridHost = new Panel();
            dgvAuditLogs = new DataGridView();
            pnlFilterHost = new Panel();
            pnlFilter = new Panel();
            lblFindAccount = new Label();
            txtSearch = new TextBox();
            btnSearch = new Button();
            chkUseDateFilter = new CheckBox();
            lblFromDate = new Label();
            dtpFromDate = new DateTimePicker();
            lblToDate = new Label();
            dtpToDate = new DateTimePicker();
            lblQuickFilter = new Label();
            cboQuickFilter = new ComboBox();
            pnlActions = new Panel();
            btnExport = new Button();
            btnRefresh = new Button();
            pnlHeader = new Panel();
            lblHeaderIcon = new Label();
            lblTitle = new Label();
            lblSubTitle = new Label();
            pnlFooter = new Panel();
            lblStatusIcon = new Label();
            lblStatus = new Label();
            toolTip1 = new ToolTip(components);
            pnlRoot.SuspendLayout();
            pnlGridHost.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAuditLogs).BeginInit();
            pnlFilterHost.SuspendLayout();
            pnlFilter.SuspendLayout();
            pnlActions.SuspendLayout();
            pnlHeader.SuspendLayout();
            pnlFooter.SuspendLayout();
            SuspendLayout();
            // 
            // pnlRoot
            // 
            pnlRoot.BackColor = Color.FromArgb(244, 248, 253);
            pnlRoot.Controls.Add(pnlGridHost);
            pnlRoot.Controls.Add(pnlFooter);
            pnlRoot.Controls.Add(pnlFilterHost);
            pnlRoot.Controls.Add(pnlHeader);
            pnlRoot.Dock = DockStyle.Fill;
            pnlRoot.Padding = new Padding(8, 8, 8, 6);
            pnlRoot.Name = "pnlRoot";
            pnlRoot.TabIndex = 0;
            // 
            // pnlGridHost
            // 
            pnlGridHost.BackColor = Color.White;
            pnlGridHost.Controls.Add(dgvAuditLogs);
            pnlGridHost.Dock = DockStyle.Fill;
            pnlGridHost.Padding = new Padding(1);
            pnlGridHost.Name = "pnlGridHost";
            pnlGridHost.TabIndex = 3;
            // 
            // dgvAuditLogs
            // 
            dgvAuditLogs.AllowUserToAddRows = false;
            dgvAuditLogs.AllowUserToDeleteRows = false;
            dgvAuditLogs.AllowUserToResizeRows = false;
            dgvAuditLogs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAuditLogs.BackgroundColor = Color.White;
            dgvAuditLogs.BorderStyle = BorderStyle.None;
            dgvAuditLogs.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvAuditLogs.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvAuditLogs.ColumnHeadersHeight = 38;
            dgvAuditLogs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvAuditLogs.Dock = DockStyle.Fill;
            dgvAuditLogs.EnableHeadersVisualStyles = false;
            dgvAuditLogs.GridColor = Color.FromArgb(207, 224, 244);
            dgvAuditLogs.Location = new Point(1, 1);
            dgvAuditLogs.MultiSelect = false;
            dgvAuditLogs.Name = "dgvAuditLogs";
            dgvAuditLogs.ReadOnly = true;
            dgvAuditLogs.RowHeadersVisible = false;
            dgvAuditLogs.RowTemplate.Height = 33;
            dgvAuditLogs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAuditLogs.Size = new Size(1300, 500);
            dgvAuditLogs.TabIndex = 0;
            dgvAuditLogs.CellFormatting += dgvAuditLogs_CellFormatting;
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = Color.FromArgb(226, 240, 255);
            pnlFooter.Controls.Add(lblStatus);
            pnlFooter.Controls.Add(lblStatusIcon);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Location = new Point(8, 650);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Padding = new Padding(10, 0, 10, 0);
            pnlFooter.Size = new Size(1300, 42);
            pnlFooter.TabIndex = 2;
            // 
            // lblStatusIcon
            // 
            lblStatusIcon.Dock = DockStyle.Left;
            lblStatusIcon.Font = new Font("Segoe UI Symbol", 13F, FontStyle.Bold);
            lblStatusIcon.ForeColor = Color.FromArgb(0, 112, 210);
            lblStatusIcon.Location = new Point(10, 0);
            lblStatusIcon.Name = "lblStatusIcon";
            lblStatusIcon.Size = new Size(27, 42);
            lblStatusIcon.TabIndex = 0;
            lblStatusIcon.Text = "ⓘ";
            lblStatusIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblStatus
            // 
            lblStatus.Dock = DockStyle.Left;
            lblStatus.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            lblStatus.ForeColor = Color.FromArgb(20, 54, 94);
            lblStatus.Location = new Point(37, 0);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(500, 42);
            lblStatus.TabIndex = 1;
            lblStatus.Text = "Đang tải nhật ký...";
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlFilterHost
            // 
            pnlFilterHost.BackColor = Color.FromArgb(244, 248, 253);
            pnlFilterHost.Controls.Add(pnlFilter);
            pnlFilterHost.Controls.Add(pnlActions);
            pnlFilterHost.Dock = DockStyle.Top;
            pnlFilterHost.Location = new Point(8, 92);
            pnlFilterHost.Name = "pnlFilterHost";
            pnlFilterHost.Padding = new Padding(4, 8, 4, 8);
            pnlFilterHost.Size = new Size(1300, 120);
            pnlFilterHost.TabIndex = 1;
            // 
            // pnlFilter
            // 
            pnlFilter.BackColor = Color.FromArgb(235, 245, 255);
            pnlFilter.BorderStyle = BorderStyle.None;
            pnlFilter.Controls.Add(lblFindAccount);
            pnlFilter.Controls.Add(txtSearch);
            pnlFilter.Controls.Add(btnSearch);
            pnlFilter.Controls.Add(chkUseDateFilter);
            pnlFilter.Controls.Add(lblFromDate);
            pnlFilter.Controls.Add(dtpFromDate);
            pnlFilter.Controls.Add(lblToDate);
            pnlFilter.Controls.Add(dtpToDate);
            pnlFilter.Controls.Add(lblQuickFilter);
            pnlFilter.Controls.Add(cboQuickFilter);
            pnlFilter.Dock = DockStyle.Fill;
            pnlFilter.Location = new Point(2, 10);
            pnlFilter.Name = "pnlFilter";
            pnlFilter.Padding = new Padding(10, 5, 10, 5);
            pnlFilter.Size = new Size(1050, 80);
            pnlFilter.TabIndex = 0;
            // 
            // lblFindAccount
            // 
            lblFindAccount.AutoSize = true;
            lblFindAccount.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblFindAccount.ForeColor = Color.FromArgb(20, 54, 94);
            lblFindAccount.Location = new Point(14, 10);
            lblFindAccount.Name = "lblFindAccount";
            lblFindAccount.Size = new Size(72, 20);
            lblFindAccount.TabIndex = 0;
            lblFindAccount.Text = "Tài khoản:";
            // 
            // txtSearch
            // 
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.Font = new Font("Segoe UI", 9.5F);
            txtSearch.Location = new Point(91, 7);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Tìm tài khoản...";
            txtSearch.Size = new Size(245, 29);
            txtSearch.TabIndex = 1;
            txtSearch.TextChanged += txtSearch_TextChanged;
            txtSearch.KeyDown += txtSearch_KeyDown;
            // 
            // btnSearch
            // 
            btnSearch.BackColor = Color.FromArgb(0, 105, 205);
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSearch.ForeColor = Color.White;
            btnSearch.Location = new Point(340, 6);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(43, 31);
            btnSearch.TabIndex = 2;
            btnSearch.Text = "";
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.Click += btnSearch_Click;
            // 
            // chkUseDateFilter
            // 
            chkUseDateFilter.AutoSize = true;
            chkUseDateFilter.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            chkUseDateFilter.ForeColor = Color.FromArgb(20, 54, 94);
            chkUseDateFilter.Location = new Point(410, 10);
            chkUseDateFilter.Name = "chkUseDateFilter";
            chkUseDateFilter.Size = new Size(120, 24);
            chkUseDateFilter.TabIndex = 3;
            chkUseDateFilter.Text = "Lọc theo ngày";
            chkUseDateFilter.UseVisualStyleBackColor = true;
            chkUseDateFilter.CheckedChanged += chkUseDateFilter_CheckedChanged;
            // 
            // lblFromDate
            // 
            lblFromDate.AutoSize = true;
            lblFromDate.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblFromDate.ForeColor = Color.FromArgb(20, 54, 94);
            lblFromDate.Location = new Point(14, 48);
            lblFromDate.Name = "lblFromDate";
            lblFromDate.Size = new Size(65, 20);
            lblFromDate.TabIndex = 4;
            lblFromDate.Text = "Từ ngày:";
            // 
            // dtpFromDate
            // 
            dtpFromDate.Font = new Font("Segoe UI", 9F);
            dtpFromDate.Format = DateTimePickerFormat.Custom;
            dtpFromDate.CustomFormat = "dd/MM/yyyy";
            dtpFromDate.Location = new Point(91, 44);
            dtpFromDate.Name = "dtpFromDate";
            dtpFromDate.Size = new Size(145, 27);
            dtpFromDate.TabIndex = 5;
            dtpFromDate.ValueChanged += dtpFromDate_ValueChanged;
            // 
            // lblToDate
            // 
            lblToDate.AutoSize = true;
            lblToDate.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblToDate.ForeColor = Color.FromArgb(20, 54, 94);
            lblToDate.Location = new Point(253, 48);
            lblToDate.Name = "lblToDate";
            lblToDate.Size = new Size(77, 20);
            lblToDate.TabIndex = 6;
            lblToDate.Text = "Đến ngày:";
            // 
            // dtpToDate
            // 
            dtpToDate.Font = new Font("Segoe UI", 9F);
            dtpToDate.Format = DateTimePickerFormat.Custom;
            dtpToDate.CustomFormat = "dd/MM/yyyy";
            dtpToDate.Location = new Point(340, 44);
            dtpToDate.Name = "dtpToDate";
            dtpToDate.Size = new Size(145, 27);
            dtpToDate.TabIndex = 7;
            dtpToDate.ValueChanged += dtpToDate_ValueChanged;
            // 
            // lblQuickFilter
            // 
            lblQuickFilter.AutoSize = true;
            lblQuickFilter.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblQuickFilter.ForeColor = Color.FromArgb(20, 54, 94);
            lblQuickFilter.Location = new Point(510, 48);
            lblQuickFilter.Name = "lblQuickFilter";
            lblQuickFilter.Size = new Size(105, 20);
            lblQuickFilter.TabIndex = 8;
            lblQuickFilter.Text = "Khoảng thời gian:";
            // 
            // cboQuickFilter
            // 
            cboQuickFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cboQuickFilter.FlatStyle = FlatStyle.Flat;
            cboQuickFilter.Font = new Font("Segoe UI", 9F);
            cboQuickFilter.FormattingEnabled = true;
            cboQuickFilter.Location = new Point(621, 44);
            cboQuickFilter.Name = "cboQuickFilter";
            cboQuickFilter.Size = new Size(190, 28);
            cboQuickFilter.TabIndex = 9;
            cboQuickFilter.SelectedIndexChanged += cboQuickFilter_SelectedIndexChanged;
            // 
            // pnlActions
            // 
            pnlActions.BackColor = Color.FromArgb(244, 248, 253);
            pnlActions.Controls.Add(btnExport);
            pnlActions.Controls.Add(btnRefresh);
            pnlActions.Dock = DockStyle.Right;
            pnlActions.Location = new Point(1050, 10);
            pnlActions.Name = "pnlActions";
            pnlActions.Padding = new Padding(12, 14, 2, 0);
            pnlActions.Size = new Size(248, 80);
            pnlActions.TabIndex = 1;
            // 
            // btnExport
            // 
            btnExport.BackColor = Color.FromArgb(8, 163, 62);
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnExport.ForeColor = Color.White;
            btnExport.Location = new Point(8, 17);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(116, 40);
            btnExport.TabIndex = 0;
            btnExport.Text = "";
            btnExport.UseVisualStyleBackColor = false;
            btnExport.Click += btnExport_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.White;
            btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(73, 158, 255);
            btnRefresh.FlatAppearance.BorderSize = 2;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRefresh.ForeColor = Color.FromArgb(0, 73, 145);
            btnRefresh.Location = new Point(132, 17);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(132, 40);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(0, 73, 145);
            pnlHeader.Controls.Add(lblSubTitle);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblHeaderIcon);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(8, 8);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1300, 84);
            pnlHeader.TabIndex = 0;
            pnlHeader.Paint += pnlHeader_Paint;
            // 
            // lblHeaderIcon
            // 
            lblHeaderIcon.Font = new Font("Segoe UI Symbol", 42F, FontStyle.Regular);
            lblHeaderIcon.ForeColor = Color.White;
            lblHeaderIcon.Location = new Point(18, 4);
            lblHeaderIcon.Name = "lblHeaderIcon";
            lblHeaderIcon.Size = new Size(88, 76);
            lblHeaderIcon.TabIndex = 0;
            lblHeaderIcon.Text = "";
            lblHeaderIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Location = new Point(112, 8);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(395, 50);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "NHẬT KÝ HỆ THỐNG";
            // 
            // lblSubTitle
            // 
            lblSubTitle.AutoSize = true;
            lblSubTitle.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lblSubTitle.ForeColor = Color.White;
            lblSubTitle.BackColor = Color.Transparent;
            lblSubTitle.Location = new Point(115, 54);
            lblSubTitle.Name = "lblSubTitle";
            lblSubTitle.Size = new Size(239, 23);
            lblSubTitle.TabIndex = 2;
            lblSubTitle.Text = "Theo dõi hoạt động người dùng";
            // 
            // FrmAuditLogList
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 248, 253);
            ClientSize = new Size(1316, 700);
            Controls.Add(pnlRoot);
            Font = new Font("Segoe UI", 9F);
            MinimumSize = new Size(900, 560);
            Name = "FrmAuditLogList";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Nhật ký hệ thống";
            Load += FrmAuditLogList_Load;
            pnlRoot.ResumeLayout(false);
            pnlGridHost.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvAuditLogs).EndInit();
            pnlFilterHost.ResumeLayout(false);
            pnlFilter.ResumeLayout(false);
            pnlFilter.PerformLayout();
            pnlActions.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlFooter.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlRoot;
        private Panel pnlGridHost;
        private DataGridView dgvAuditLogs;
        private Panel pnlFilterHost;
        private Panel pnlFilter;
        private Label lblFindAccount;
        private TextBox txtSearch;
        private Button btnSearch;
        private CheckBox chkUseDateFilter;
        private Label lblFromDate;
        private DateTimePicker dtpFromDate;
        private Label lblToDate;
        private DateTimePicker dtpToDate;
        private Label lblQuickFilter;
        private ComboBox cboQuickFilter;
        private Panel pnlActions;
        private Button btnExport;
        private Button btnRefresh;
        private Panel pnlHeader;
        private Label lblHeaderIcon;
        private Label lblTitle;
        private Label lblSubTitle;
        private Panel pnlFooter;
        private Label lblStatusIcon;
        private Label lblStatus;
        private ToolTip toolTip1;
    }
}