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
            btnRefresh.Location = new Point(672, 96);
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
            dgvAuditLogs.Location = new Point(0, 137);
            dgvAuditLogs.Name = "dgvAuditLogs";
            dgvAuditLogs.ReadOnly = true;
            dgvAuditLogs.RowHeadersWidth = 51;
            dgvAuditLogs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAuditLogs.Size = new Size(788, 409);
            dgvAuditLogs.TabIndex = 2;
            dgvAuditLogs.CellFormatting += dgvAuditLogs_CellFormatting;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(12, 96);
            txtSearch.Multiline = true;
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = " Tìm tài khoản...";
            txtSearch.Size = new Size(250, 35);
            txtSearch.TabIndex = 3;
            // 
            // btnSearch
            // 
            btnSearch.BackColor = Color.FromArgb(52, 152, 219);
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.ForeColor = Color.White;
            btnSearch.Location = new Point(268, 96);
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
            statusStrip1.Location = new Point(0, 520);
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
            // FrmAuditLogList
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(788, 546);
            Controls.Add(statusStrip1);
            Controls.Add(btnSearch);
            Controls.Add(txtSearch);
            Controls.Add(btnRefresh);
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
    }
}