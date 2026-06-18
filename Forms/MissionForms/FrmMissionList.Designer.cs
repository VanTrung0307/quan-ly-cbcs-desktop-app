namespace QuanLyCBCS.Forms.MissionForms
{
    partial class FrmMissionList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMissionList));
            pnlHeader = new Panel();
            lblSubTitle = new Label();
            lblTitle = new Label();
            pnlToolbar = new Panel();
            panel4 = new Panel();
            label8 = new Label();
            label4 = new Label();
            panel3 = new Panel();
            label7 = new Label();
            label3 = new Label();
            panel2 = new Panel();
            label6 = new Label();
            label2 = new Label();
            panel1 = new Panel();
            label5 = new Label();
            label1 = new Label();
            txtSearchMission = new TextBox();
            btnRefresh = new Button();
            btnMembers = new Button();
            btnDeleteMission = new Button();
            btnEditMission = new Button();
            btnAddMission = new Button();
            dgvMission = new DataGridView();
            statusStripMain = new StatusStrip();
            lblRecordCount = new ToolStripStatusLabel();
            pnlHeader.SuspendLayout();
            pnlToolbar.SuspendLayout();
            panel4.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMission).BeginInit();
            statusStripMain.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(47, 69, 92);
            pnlHeader.Controls.Add(lblSubTitle);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1282, 80);
            pnlHeader.TabIndex = 0;
            // 
            // lblSubTitle
            // 
            lblSubTitle.AutoSize = true;
            lblSubTitle.ForeColor = SystemColors.ActiveBorder;
            lblSubTitle.Location = new Point(498, 50);
            lblSubTitle.Name = "lblSubTitle";
            lblSubTitle.Size = new Size(252, 20);
            lblSubTitle.TabIndex = 1;
            lblSubTitle.Text = "Quản lý công tác và phân công CBCS";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(470, 9);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(306, 41);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "QUẢN LÝ CÔNG TÁC";
            // 
            // pnlToolbar
            // 
            pnlToolbar.BackColor = SystemColors.Control;
            pnlToolbar.Controls.Add(panel4);
            pnlToolbar.Controls.Add(panel3);
            pnlToolbar.Controls.Add(panel2);
            pnlToolbar.Controls.Add(panel1);
            pnlToolbar.Controls.Add(txtSearchMission);
            pnlToolbar.Controls.Add(btnRefresh);
            pnlToolbar.Controls.Add(btnMembers);
            pnlToolbar.Controls.Add(btnDeleteMission);
            pnlToolbar.Controls.Add(btnEditMission);
            pnlToolbar.Controls.Add(btnAddMission);
            pnlToolbar.Dock = DockStyle.Top;
            pnlToolbar.Location = new Point(0, 80);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Size = new Size(1282, 126);
            pnlToolbar.TabIndex = 1;
            // 
            // panel4
            // 
            panel4.Controls.Add(label8);
            panel4.Controls.Add(label4);
            panel4.Location = new Point(781, 8);
            panel4.Name = "panel4";
            panel4.Size = new Size(149, 99);
            panel4.TabIndex = 7;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(66, 42);
            label8.Name = "label8";
            label8.Size = new Size(25, 20);
            label8.TabIndex = 4;
            label8.Text = "28";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(42, 7);
            label4.Name = "label4";
            label4.Size = new Size(65, 20);
            label4.TabIndex = 3;
            label4.Text = "CBCS TG";
            // 
            // panel3
            // 
            panel3.Controls.Add(label7);
            panel3.Controls.Add(label3);
            panel3.Location = new Point(612, 8);
            panel3.Name = "panel3";
            panel3.Size = new Size(149, 99);
            panel3.TabIndex = 7;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(62, 42);
            label7.Name = "label7";
            label7.Size = new Size(17, 20);
            label7.TabIndex = 3;
            label7.Text = "3";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(39, 7);
            label3.Name = "label3";
            label3.Size = new Size(69, 20);
            label3.TabIndex = 2;
            label3.Text = "Hôm nay";
            // 
            // panel2
            // 
            panel2.Controls.Add(label6);
            panel2.Controls.Add(label2);
            panel2.Location = new Point(446, 8);
            panel2.Name = "panel2";
            panel2.Size = new Size(149, 99);
            panel2.TabIndex = 7;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(63, 42);
            label6.Name = "label6";
            label6.Size = new Size(17, 20);
            label6.TabIndex = 2;
            label6.Text = "5";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(39, 7);
            label2.Name = "label2";
            label2.Size = new Size(66, 20);
            label2.TabIndex = 1;
            label2.Text = "Đang CT";
            // 
            // panel1
            // 
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label1);
            panel1.Location = new Point(277, 8);
            panel1.Name = "panel1";
            panel1.Size = new Size(149, 99);
            panel1.TabIndex = 6;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(62, 42);
            label5.Name = "label5";
            label5.Size = new Size(25, 20);
            label5.TabIndex = 1;
            label5.Text = "12";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(41, 7);
            label1.Name = "label1";
            label1.Size = new Size(64, 20);
            label1.TabIndex = 0;
            label1.Text = "Tổng CT";
            // 
            // txtSearchMission
            // 
            txtSearchMission.Location = new Point(970, 8);
            txtSearchMission.Name = "txtSearchMission";
            txtSearchMission.PlaceholderText = "Tìm theo tên công tác, loại công tác...";
            txtSearchMission.Size = new Size(300, 27);
            txtSearchMission.TabIndex = 5;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(112, 41);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(113, 29);
            btnRefresh.TabIndex = 4;
            btnRefresh.Text = "🔄 Làm mới";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += FrmMissionList_Load;
            // 
            // btnMembers
            // 
            btnMembers.Location = new Point(112, 6);
            btnMembers.Name = "btnMembers";
            btnMembers.Size = new Size(113, 29);
            btnMembers.TabIndex = 3;
            btnMembers.Text = "👥 Phân công";
            btnMembers.UseVisualStyleBackColor = true;
            // 
            // btnDeleteMission
            // 
            btnDeleteMission.Location = new Point(12, 76);
            btnDeleteMission.Name = "btnDeleteMission";
            btnDeleteMission.Size = new Size(94, 29);
            btnDeleteMission.TabIndex = 2;
            btnDeleteMission.Text = "🗑 Xóa";
            btnDeleteMission.UseVisualStyleBackColor = true;
            btnDeleteMission.Click += btnDelete_Click;
            // 
            // btnEditMission
            // 
            btnEditMission.Location = new Point(12, 41);
            btnEditMission.Name = "btnEditMission";
            btnEditMission.Size = new Size(94, 29);
            btnEditMission.TabIndex = 1;
            btnEditMission.Text = "✏ Sửa";
            btnEditMission.UseVisualStyleBackColor = true;
            btnEditMission.Click += btnEdit_Click;
            // 
            // btnAddMission
            // 
            btnAddMission.Location = new Point(12, 6);
            btnAddMission.Name = "btnAddMission";
            btnAddMission.Size = new Size(94, 29);
            btnAddMission.TabIndex = 0;
            btnAddMission.Text = "➕ Thêm";
            btnAddMission.UseVisualStyleBackColor = true;
            btnAddMission.Click += btnAdd_Click;
            // 
            // dgvMission
            // 
            dgvMission.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMission.Dock = DockStyle.Fill;
            dgvMission.Location = new Point(0, 206);
            dgvMission.Name = "dgvMission";
            dgvMission.RowHeadersWidth = 51;
            dgvMission.Size = new Size(1282, 497);
            dgvMission.TabIndex = 2;
            dgvMission.CellContentDoubleClick += dgvMission_CellDoubleClick;
            // 
            // statusStripMain
            // 
            statusStripMain.ImageScalingSize = new Size(20, 20);
            statusStripMain.Items.AddRange(new ToolStripItem[] { lblRecordCount });
            statusStripMain.Location = new Point(0, 677);
            statusStripMain.Name = "statusStripMain";
            statusStripMain.Size = new Size(1282, 26);
            statusStripMain.TabIndex = 3;
            statusStripMain.Text = "statusStrip1";
            // 
            // lblRecordCount
            // 
            lblRecordCount.Name = "lblRecordCount";
            lblRecordCount.Size = new Size(174, 20);
            lblRecordCount.Text = "Đang hiển thị: 0 công tác";
            // 
            // FrmMissionList
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1282, 703);
            Controls.Add(statusStripMain);
            Controls.Add(dgvMission);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlHeader);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(1300, 750);
            Name = "FrmMissionList";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Quản lý công tác";
            Load += FrmMissionList_Load;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlToolbar.ResumeLayout(false);
            pnlToolbar.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMission).EndInit();
            statusStripMain.ResumeLayout(false);
            statusStripMain.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlHeader;
        private Label lblTitle;
        private Label lblSubTitle;
        private Panel pnlToolbar;
        private Button btnAddMission;
        private Button btnDeleteMission;
        private Button btnEditMission;
        private Button btnRefresh;
        private Button btnMembers;
        private TextBox txtSearchMission;
        private DataGridView dgvMission;
        private StatusStrip statusStripMain;
        private ToolStripStatusLabel lblRecordCount;
        private Panel panel4;
        private Label label8;
        private Label label4;
        private Panel panel3;
        private Label label7;
        private Label label3;
        private Panel panel2;
        private Label label6;
        private Label label2;
        private Panel panel1;
        private Label label5;
        private Label label1;
    }
}