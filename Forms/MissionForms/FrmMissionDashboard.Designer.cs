namespace QuanLyCBCS.Forms.MissionForms
{
    partial class FrmMissionDashboard
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
            lblTongCongTac = new Label();
            lblDangHoatDong = new Label();
            lblDaKetThuc = new Label();
            lblTopPersonnel = new Label();
            lblTopPersonnelCount = new Label();
            dgvRecentMission = new DataGridView();
            btnRefresh = new Button();
            btnClose = new Button();
            pnlHeader = new Panel();
            lblTitle = new Label();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            dgvTopPersonnel = new DataGridView();
            lblTongCongTacTitle = new Label();
            lblDangHoatDongTitle = new Label();
            lblDaKetThucTitle = new Label();
            lblTopPersonnelTitle = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvRecentMission).BeginInit();
            pnlHeader.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTopPersonnel).BeginInit();
            SuspendLayout();
            // 
            // lblTongCongTac
            // 
            lblTongCongTac.AutoSize = true;
            lblTongCongTac.Location = new Point(374, 94);
            lblTongCongTac.Name = "lblTongCongTac";
            lblTongCongTac.Size = new Size(17, 20);
            lblTongCongTac.TabIndex = 0;
            lblTongCongTac.Text = "0";
            // 
            // lblDangHoatDong
            // 
            lblDangHoatDong.AutoSize = true;
            lblDangHoatDong.Location = new Point(374, 134);
            lblDangHoatDong.Name = "lblDangHoatDong";
            lblDangHoatDong.Size = new Size(17, 20);
            lblDangHoatDong.TabIndex = 1;
            lblDangHoatDong.Text = "0";
            // 
            // lblDaKetThuc
            // 
            lblDaKetThuc.AutoSize = true;
            lblDaKetThuc.Location = new Point(374, 175);
            lblDaKetThuc.Name = "lblDaKetThuc";
            lblDaKetThuc.Size = new Size(17, 20);
            lblDaKetThuc.TabIndex = 2;
            lblDaKetThuc.Text = "0";
            // 
            // lblTopPersonnel
            // 
            lblTopPersonnel.AutoSize = true;
            lblTopPersonnel.Location = new Point(770, 94);
            lblTopPersonnel.Name = "lblTopPersonnel";
            lblTopPersonnel.Size = new Size(93, 20);
            lblTopPersonnel.TabIndex = 3;
            lblTopPersonnel.Text = "Lê Văn Trung";
            // 
            // lblTopPersonnelCount
            // 
            lblTopPersonnelCount.AutoSize = true;
            lblTopPersonnelCount.Location = new Point(770, 125);
            lblTopPersonnelCount.Name = "lblTopPersonnelCount";
            lblTopPersonnelCount.Size = new Size(113, 40);
            lblTopPersonnelCount.TabIndex = 4;
            lblTopPersonnelCount.Text = "Chưa có dữ liệu\n0 lần";
            // 
            // dgvRecentMission
            // 
            dgvRecentMission.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRecentMission.Dock = DockStyle.Fill;
            dgvRecentMission.Location = new Point(3, 23);
            dgvRecentMission.Name = "dgvRecentMission";
            dgvRecentMission.RowHeadersWidth = 51;
            dgvRecentMission.Size = new Size(559, 485);
            dgvRecentMission.TabIndex = 5;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(1152, 85);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(94, 29);
            btnRefresh.TabIndex = 6;
            btnRefresh.Text = "Làm mới";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnClose
            // 
            btnClose.Location = new Point(1252, 85);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(94, 29);
            btnClose.TabIndex = 7;
            btnClose.Text = "Đóng";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(44, 62, 80);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1358, 73);
            pnlHeader.TabIndex = 12;
            // 
            // lblTitle
            // 
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(1358, 73);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "DASHBOARD CÔNG TÁC";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(dgvRecentMission);
            groupBox1.Location = new Point(52, 215);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(565, 511);
            groupBox1.TabIndex = 13;
            groupBox1.TabStop = false;
            groupBox1.Text = "5 công tác gần nhất";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(dgvTopPersonnel);
            groupBox2.Location = new Point(742, 212);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(552, 511);
            groupBox2.TabIndex = 14;
            groupBox2.TabStop = false;
            groupBox2.Text = "Top 5 CBCS công tác nhiều nhất";
            // 
            // dgvTopPersonnel
            // 
            dgvTopPersonnel.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTopPersonnel.Dock = DockStyle.Fill;
            dgvTopPersonnel.Location = new Point(3, 23);
            dgvTopPersonnel.Name = "dgvTopPersonnel";
            dgvTopPersonnel.RowHeadersWidth = 51;
            dgvTopPersonnel.Size = new Size(546, 485);
            dgvTopPersonnel.TabIndex = 0;
            // 
            // lblTongCongTacTitle
            // 
            lblTongCongTacTitle.AutoSize = true;
            lblTongCongTacTitle.Location = new Point(247, 94);
            lblTongCongTacTitle.Name = "lblTongCongTacTitle";
            lblTongCongTacTitle.Size = new Size(107, 20);
            lblTongCongTacTitle.TabIndex = 15;
            lblTongCongTacTitle.Text = "Tổng công tác:";
            // 
            // lblDangHoatDongTitle
            // 
            lblDangHoatDongTitle.AutoSize = true;
            lblDangHoatDongTitle.Location = new Point(247, 134);
            lblDangHoatDongTitle.Name = "lblDangHoatDongTitle";
            lblDangHoatDongTitle.Size = new Size(121, 20);
            lblDangHoatDongTitle.TabIndex = 16;
            lblDangHoatDongTitle.Text = "Đang hoạt động:";
            // 
            // lblDaKetThucTitle
            // 
            lblDaKetThucTitle.AutoSize = true;
            lblDaKetThucTitle.Location = new Point(247, 175);
            lblDaKetThucTitle.Name = "lblDaKetThucTitle";
            lblDaKetThucTitle.Size = new Size(87, 20);
            lblDaKetThucTitle.TabIndex = 17;
            lblDaKetThucTitle.Text = "Đã kết thúc:";
            // 
            // lblTopPersonnelTitle
            // 
            lblTopPersonnelTitle.AutoSize = true;
            lblTopPersonnelTitle.Location = new Point(629, 94);
            lblTopPersonnelTitle.Name = "lblTopPersonnelTitle";
            lblTopPersonnelTitle.Size = new Size(135, 20);
            lblTopPersonnelTitle.TabIndex = 18;
            lblTopPersonnelTitle.Text = "CBCS tích cực nhất:";
            // 
            // FrmMissionDashboard
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1358, 760);
            Controls.Add(lblTopPersonnelTitle);
            Controls.Add(lblDaKetThucTitle);
            Controls.Add(lblDangHoatDongTitle);
            Controls.Add(lblTongCongTacTitle);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(pnlHeader);
            Controls.Add(btnClose);
            Controls.Add(btnRefresh);
            Controls.Add(lblTopPersonnelCount);
            Controls.Add(lblTopPersonnel);
            Controls.Add(lblDaKetThuc);
            Controls.Add(lblDangHoatDong);
            Controls.Add(lblTongCongTac);
            Name = "FrmMissionDashboard";
            Text = "FrmMissionDashboard";
            Load += FrmMissionDashboard_Load;
            ((System.ComponentModel.ISupportInitialize)dgvRecentMission).EndInit();
            pnlHeader.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvTopPersonnel).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTongCongTac;
        private Label lblDangHoatDong;
        private Label lblDaKetThuc;
        private Label lblTopPersonnel;
        private Label lblTopPersonnelCount;
        private DataGridView dgvRecentMission;
        private Button btnRefresh;
        private Button btnClose;
        private Panel pnlHeader;
        private Label lblTitle;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private DataGridView dgvTopPersonnel;
        private Label lblTongCongTacTitle;
        private Label lblDangHoatDongTitle;
        private Label lblDaKetThucTitle;
        private Label lblTopPersonnelTitle;
    }
}