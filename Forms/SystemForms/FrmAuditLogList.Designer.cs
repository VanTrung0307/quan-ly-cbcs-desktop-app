namespace QuanLyCBCS.Forms.SystemForms
{
    partial class FrmAuditLogList
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAuditLogList));
            lblTitle = new Label();
            btnRefresh = new Button();
            dgvAuditLogs = new DataGridView();
            txtSearch = new TextBox();
            btnSearch = new Button();
            pnlHeader = new Panel();
            lblSubTitle = new Label();
            statusStrip1 = new StatusStrip();
            lblStatus = new ToolStripStatusLabel();
            toolTip1 = new ToolTip(components);
            btnExport = new Button();
            lblFromDate = new Label();
            lblToDate = new Label();
            dtpFromDate = new DateTimePicker();
            dtpToDate = new DateTimePicker();
            lblFindAccount = new Label();
            chkUseDateFilter = new CheckBox();
            cboQuickFilter = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)dgvAuditLogs).BeginInit();
            pnlHeader.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Anchor = AnchorStyles.Top;
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(226, 9);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(313, 41);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "NHẬT KÝ HỆ THỐNG";
            lblTitle.TextAlign = ContentAlignment.TopCenter;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.FromArgb(52, 73, 94);
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(672, 102);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(104, 35);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "🔁 Làm mới";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // dgvAuditLogs
            // 
            dgvAuditLogs.AllowUserToAddRows = false;
            dgvAuditLogs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAuditLogs.Location = new Point(0, 212);
            dgvAuditLogs.Name = "dgvAuditLogs";
            dgvAuditLogs.ReadOnly = true;
            dgvAuditLogs.RowHeadersWidth = 51;
            dgvAuditLogs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAuditLogs.Size = new Size(788, 460);
            dgvAuditLogs.TabIndex = 2;
            dgvAuditLogs.CellFormatting += dgvAuditLogs_CellFormatting;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(101, 102);
            txtSearch.Multiline = true;
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = " Tìm tài khoản...";
            txtSearch.Size = new Size(250, 35);
            txtSearch.TabIndex = 3;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // btnSearch
            // 
            btnSearch.BackColor = Color.FromArgb(52, 152, 219);
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.ForeColor = Color.White;
            btnSearch.Location = new Point(357, 102);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(69, 35);
            btnSearch.TabIndex = 4;
            btnSearch.Text = "🔍 Tìm";
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.Click += btnSearch_Click;
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(44, 62, 80);
            pnlHeader.Controls.Add(lblSubTitle);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(788, 90);
            pnlHeader.TabIndex = 5;
            // 
            // lblSubTitle
            // 
            lblSubTitle.AutoSize = true;
            lblSubTitle.ForeColor = Color.Gainsboro;
            lblSubTitle.Location = new Point(268, 50);
            lblSubTitle.Name = "lblSubTitle";
            lblSubTitle.Size = new Size(222, 20);
            lblSubTitle.TabIndex = 1;
            lblSubTitle.Text = "Theo dõi hoạt động người dùng";
            lblSubTitle.TextAlign = ContentAlignment.BottomCenter;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { lblStatus });
            statusStrip1.Location = new Point(0, 675);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(788, 26);
            statusStrip1.TabIndex = 6;
            statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(151, 20);
            lblStatus.Text = "toolStripStatusLabel1";
            // 
            // toolTip1
            // 
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 300;
            toolTip1.ReshowDelay = 100;
            toolTip1.ShowAlways = true;
            // 
            // btnExport
            // 
            btnExport.BackColor = Color.Green;
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.ForeColor = Color.White;
            btnExport.Location = new Point(555, 102);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(111, 35);
            btnExport.TabIndex = 7;
            btnExport.Text = "📊 Xuất Excel";
            btnExport.UseVisualStyleBackColor = false;
            btnExport.Click += btnExport_Click;
            // 
            // lblFromDate
            // 
            lblFromDate.AutoSize = true;
            lblFromDate.Location = new Point(13, 178);
            lblFromDate.Name = "lblFromDate";
            lblFromDate.Size = new Size(65, 20);
            lblFromDate.TabIndex = 8;
            lblFromDate.Text = "Từ ngày:";
            // 
            // lblToDate
            // 
            lblToDate.AutoSize = true;
            lblToDate.Location = new Point(227, 178);
            lblToDate.Name = "lblToDate";
            lblToDate.Size = new Size(75, 20);
            lblToDate.TabIndex = 9;
            lblToDate.Text = "Đến ngày:";
            // 
            // dtpFromDate
            // 
            dtpFromDate.Format = DateTimePickerFormat.Short;
            dtpFromDate.Location = new Point(84, 173);
            dtpFromDate.Name = "dtpFromDate";
            dtpFromDate.Size = new Size(120, 27);
            dtpFromDate.TabIndex = 10;
            dtpFromDate.ValueChanged += dtpFromDate_ValueChanged;
            // 
            // dtpToDate
            // 
            dtpToDate.Format = DateTimePickerFormat.Short;
            dtpToDate.Location = new Point(308, 173);
            dtpToDate.Name = "dtpToDate";
            dtpToDate.Size = new Size(120, 27);
            dtpToDate.TabIndex = 11;
            dtpToDate.ValueChanged += dtpToDate_ValueChanged;
            // 
            // lblFindAccount
            // 
            lblFindAccount.AutoSize = true;
            lblFindAccount.Location = new Point(12, 109);
            lblFindAccount.Name = "lblFindAccount";
            lblFindAccount.Size = new Size(74, 20);
            lblFindAccount.TabIndex = 12;
            lblFindAccount.Text = "Tài khoản:\n";
            // 
            // chkUseDateFilter
            // 
            chkUseDateFilter.AutoSize = true;
            chkUseDateFilter.Location = new Point(15, 143);
            chkUseDateFilter.Name = "chkUseDateFilter";
            chkUseDateFilter.Size = new Size(124, 24);
            chkUseDateFilter.TabIndex = 13;
            chkUseDateFilter.Text = "Lọc theo ngày";
            chkUseDateFilter.UseVisualStyleBackColor = true;
            chkUseDateFilter.CheckedChanged += chkUseDateFilter_CheckedChanged;
            // 
            // cboQuickFilter
            // 
            cboQuickFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cboQuickFilter.FormattingEnabled = true;
            cboQuickFilter.Items.AddRange(new object[] { "Tất cả", "Hôm nay", "Hôm qua", "7 ngày gần nhất", "30 ngày gần nhất", "Tháng này", "Tháng trước", "Năm nay" });
            cboQuickFilter.Location = new Point(625, 172);
            cboQuickFilter.Name = "cboQuickFilter";
            cboQuickFilter.Size = new Size(151, 28);
            cboQuickFilter.TabIndex = 14;
            cboQuickFilter.SelectedIndexChanged += cboQuickFilter_SelectedIndexChanged;
            // 
            // FrmAuditLogList
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(788, 701);
            Controls.Add(cboQuickFilter);
            Controls.Add(chkUseDateFilter);
            Controls.Add(lblFindAccount);
            Controls.Add(btnRefresh);
            Controls.Add(btnExport);
            Controls.Add(dtpToDate);
            Controls.Add(dtpFromDate);
            Controls.Add(lblToDate);
            Controls.Add(lblFromDate);
            Controls.Add(statusStrip1);
            Controls.Add(btnSearch);
            Controls.Add(txtSearch);
            Controls.Add(pnlHeader);
            Controls.Add(dgvAuditLogs);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FrmAuditLogList";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Nhật ký hệ thống";
            Load += FrmAuditLogList_Load;
            ((System.ComponentModel.ISupportInitialize)dgvAuditLogs).EndInit();
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Button btnRefresh;
        private DataGridView dgvAuditLogs;
        private TextBox txtSearch;
        private Button btnSearch;
        private Panel pnlHeader;
        private Label lblSubTitle;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lblStatus;
        private ToolTip toolTip1;
        private Button btnExport;
        private Label lblFromDate;
        private Label lblToDate;
        private DateTimePicker dtpFromDate;
        private DateTimePicker dtpToDate;
        private Label lblFindAccount;
        private CheckBox chkUseDateFilter;
        private ComboBox cboQuickFilter;
    }
}