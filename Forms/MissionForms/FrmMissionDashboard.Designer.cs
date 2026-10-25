namespace QuanLyCBCS.Forms.MissionForms
{
    partial class FrmMissionDashboard
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMissionDashboard));
            pnlHeader = new Panel();
            lblSubtitle = new Label();
            lblTitle = new Label();
            lblHeaderIcon = new Label();
            btnRefresh = new Button();
            btnClose = new Button();
            pnlDashboard = new Panel();
            pnlTopPersonnel = new Panel();
            dgvTopPersonnel = new DataGridView();
            lblPersonnelTableTitle = new Label();
            pnlRecent = new Panel();
            dgvRecentMission = new DataGridView();
            lblRecentEmpty = new Label();
            lblRecentTitle = new Label();
            pnlCardTop = new Panel();
            lblTopPersonnelCount = new Label();
            lblTopPersonnel = new Label();
            lblTopPersonnelTitle = new Label();
            pnlCardEnded = new Panel();
            lblEndedUnit = new Label();
            lblDaKetThuc = new Label();
            lblDaKetThucTitle = new Label();
            pnlCardActive = new Panel();
            lblActiveUnit = new Label();
            lblDangHoatDong = new Label();
            lblDangHoatDongTitle = new Label();
            pnlCardTotal = new Panel();
            lblTotalUnit = new Label();
            lblTongCongTac = new Label();
            lblTongCongTacTitle = new Label();
            pnlHeader.SuspendLayout();
            pnlDashboard.SuspendLayout();
            pnlTopPersonnel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTopPersonnel).BeginInit();
            pnlRecent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRecentMission).BeginInit();
            pnlCardTop.SuspendLayout();
            pnlCardEnded.SuspendLayout();
            pnlCardActive.SuspendLayout();
            pnlCardTotal.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(18, 64, 115);
            pnlHeader.Controls.Add(lblSubtitle);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblHeaderIcon);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1280, 140);
            pnlHeader.TabIndex = 0;
            // 
            // lblSubtitle
            // 
            lblSubtitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblSubtitle.BackColor = Color.Transparent;
            lblSubtitle.Font = new Font("Segoe UI", 13F);
            lblSubtitle.ForeColor = Color.FromArgb(226, 239, 255);
            lblSubtitle.Location = new Point(145, 82);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(1280, 38);
            lblSubtitle.TabIndex = 2;
            lblSubtitle.Text = "Theo dõi và thống kê tình hình công tác của cán bộ chiến sĩ";
            // 
            // lblTitle
            // 
            lblTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Font = new Font("Segoe UI", 27F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(142, 18);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(1340, 62);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "DASHBOARD CÔNG TÁC";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblHeaderIcon
            // 
            lblHeaderIcon.BackColor = Color.Transparent;
            lblHeaderIcon.Font = new Font("Segoe UI Symbol", 38F, FontStyle.Bold);
            lblHeaderIcon.ForeColor = Color.White;
            lblHeaderIcon.Location = new Point(34, 27);
            lblHeaderIcon.Name = "lblHeaderIcon";
            lblHeaderIcon.Size = new Size(90, 100);
            lblHeaderIcon.TabIndex = 0;
            lblHeaderIcon.Text = "▤";
            lblHeaderIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnRefresh
            // 
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.BackColor = Color.White;
            btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(28, 100, 190);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnRefresh.ForeColor = Color.FromArgb(20, 61, 112);
            btnRefresh.Location = new Point(1215, 179);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(150, 46);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "↻  Làm mới";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.BackColor = Color.FromArgb(19, 62, 111);
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(1377, 179);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(130, 46);
            btnClose.TabIndex = 2;
            btnClose.Text = "×  Đóng";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // pnlDashboard
            // 
            pnlDashboard.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlDashboard.Controls.Add(pnlTopPersonnel);
            pnlDashboard.Controls.Add(pnlRecent);
            pnlDashboard.Controls.Add(pnlCardTop);
            pnlDashboard.Controls.Add(pnlCardEnded);
            pnlDashboard.Controls.Add(pnlCardActive);
            pnlDashboard.Controls.Add(pnlCardTotal);
            pnlDashboard.Location = new Point(22, 210);
            pnlDashboard.Name = "pnlDashboard";
            pnlDashboard.Size = new Size(1236, 590);
            pnlDashboard.TabIndex = 3;
            // 
            // pnlTopPersonnel
            // 
            pnlTopPersonnel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlTopPersonnel.BackColor = Color.White;
            pnlTopPersonnel.BorderStyle = BorderStyle.FixedSingle;
            pnlTopPersonnel.Controls.Add(dgvTopPersonnel);
            pnlTopPersonnel.Controls.Add(lblPersonnelTableTitle);
            pnlTopPersonnel.Location = new Point(633, 145);
            pnlTopPersonnel.Name = "pnlTopPersonnel";
            pnlTopPersonnel.Size = new Size(603, 433);
            pnlTopPersonnel.TabIndex = 5;
            // 
            // dgvTopPersonnel
            // 
            dgvTopPersonnel.AllowUserToAddRows = false;
            dgvTopPersonnel.AllowUserToDeleteRows = false;
            dgvTopPersonnel.AllowUserToResizeRows = false;
            dgvTopPersonnel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvTopPersonnel.BackgroundColor = Color.White;
            dgvTopPersonnel.BorderStyle = BorderStyle.None;
            dgvTopPersonnel.ColumnHeadersHeight = 52;
            dgvTopPersonnel.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvTopPersonnel.Location = new Point(13, 65);
            dgvTopPersonnel.MultiSelect = false;
            dgvTopPersonnel.Name = "dgvTopPersonnel";
            dgvTopPersonnel.ReadOnly = true;
            dgvTopPersonnel.RowHeadersVisible = false;
            dgvTopPersonnel.RowHeadersWidth = 51;
            dgvTopPersonnel.RowTemplate.Height = 42;
            dgvTopPersonnel.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTopPersonnel.Size = new Size(692, 462);
            dgvTopPersonnel.TabIndex = 1;
            // 
            // lblPersonnelTableTitle
            // 
            lblPersonnelTableTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblPersonnelTableTitle.ForeColor = Color.FromArgb(12, 37, 122);
            lblPersonnelTableTitle.Location = new Point(16, 12);
            lblPersonnelTableTitle.Name = "lblPersonnelTableTitle";
            lblPersonnelTableTitle.Size = new Size(680, 42);
            lblPersonnelTableTitle.TabIndex = 0;
            lblPersonnelTableTitle.Text = "♛  Top 5 CBCS công tác nhiều nhất";
            // 
            // pnlRecent
            // 
            pnlRecent.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            pnlRecent.BackColor = Color.White;
            pnlRecent.BorderStyle = BorderStyle.FixedSingle;
            pnlRecent.Controls.Add(dgvRecentMission);
            pnlRecent.Controls.Add(lblRecentEmpty);
            pnlRecent.Controls.Add(lblRecentTitle);
            pnlRecent.Location = new Point(0, 145);
            pnlRecent.Name = "pnlRecent";
            pnlRecent.Size = new Size(619, 433);
            pnlRecent.TabIndex = 4;
            // 
            // dgvRecentMission
            // 
            dgvRecentMission.AllowUserToAddRows = false;
            dgvRecentMission.AllowUserToDeleteRows = false;
            dgvRecentMission.AllowUserToResizeRows = false;
            dgvRecentMission.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvRecentMission.BackgroundColor = Color.White;
            dgvRecentMission.BorderStyle = BorderStyle.None;
            dgvRecentMission.ColumnHeadersHeight = 52;
            dgvRecentMission.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvRecentMission.Location = new Point(13, 65);
            dgvRecentMission.MultiSelect = false;
            dgvRecentMission.Name = "dgvRecentMission";
            dgvRecentMission.ReadOnly = true;
            dgvRecentMission.RowHeadersVisible = false;
            dgvRecentMission.RowHeadersWidth = 51;
            dgvRecentMission.RowTemplate.Height = 42;
            dgvRecentMission.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRecentMission.Size = new Size(724, 462);
            dgvRecentMission.TabIndex = 1;
            // 
            // lblRecentEmpty
            // 
            lblRecentEmpty.Anchor = AnchorStyles.None;
            lblRecentEmpty.BackColor = Color.White;
            lblRecentEmpty.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblRecentEmpty.ForeColor = Color.FromArgb(47, 79, 130);
            lblRecentEmpty.Location = new Point(90, 250);
            lblRecentEmpty.Name = "lblRecentEmpty";
            lblRecentEmpty.Size = new Size(570, 40);
            lblRecentEmpty.TabIndex = 2;
            lblRecentEmpty.Text = "Chưa có dữ liệu công tác";
            lblRecentEmpty.TextAlign = ContentAlignment.MiddleCenter;
            lblRecentEmpty.Visible = false;
            // 
            // lblRecentTitle
            // 
            lblRecentTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblRecentTitle.ForeColor = Color.FromArgb(12, 37, 122);
            lblRecentTitle.Location = new Point(16, 12);
            lblRecentTitle.Name = "lblRecentTitle";
            lblRecentTitle.Size = new Size(710, 42);
            lblRecentTitle.TabIndex = 0;
            lblRecentTitle.Text = "▤  5 công tác gần nhất";
            // 
            // pnlCardTop
            // 
            pnlCardTop.BackColor = Color.FromArgb(241, 239, 255);
            pnlCardTop.BorderStyle = BorderStyle.None;
            pnlCardTop.Controls.Add(lblTopPersonnelCount);
            pnlCardTop.Controls.Add(lblTopPersonnel);
            pnlCardTop.Controls.Add(lblTopPersonnelTitle);
            pnlCardTop.Location = new Point(939, 0);
            pnlCardTop.Name = "pnlCardTop";
            pnlCardTop.Size = new Size(297, 125);
            pnlCardTop.TabIndex = 3;
            // 
            // lblTopPersonnelCount
            // 
            lblTopPersonnelCount.Font = new Font("Segoe UI", 13F);
            lblTopPersonnelCount.ForeColor = Color.FromArgb(87, 99, 139);
            lblTopPersonnelCount.Location = new Point(22, 91);
            lblTopPersonnelCount.Name = "lblTopPersonnelCount";
            lblTopPersonnelCount.Size = new Size(300, 30);
            lblTopPersonnelCount.TabIndex = 2;
            lblTopPersonnelCount.Text = "0 lần";
            // 
            // lblTopPersonnel
            // 
            lblTopPersonnel.AutoEllipsis = true;
            lblTopPersonnel.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTopPersonnel.ForeColor = Color.FromArgb(35, 38, 160);
            lblTopPersonnel.Location = new Point(22, 51);
            lblTopPersonnel.Name = "lblTopPersonnel";
            lblTopPersonnel.Size = new Size(320, 38);
            lblTopPersonnel.TabIndex = 1;
            lblTopPersonnel.Text = "Chưa có dữ liệu";
            // 
            // lblTopPersonnelTitle
            // 
            lblTopPersonnelTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTopPersonnelTitle.ForeColor = Color.FromArgb(39, 38, 174);
            lblTopPersonnelTitle.Location = new Point(22, 18);
            lblTopPersonnelTitle.Name = "lblTopPersonnelTitle";
            lblTopPersonnelTitle.Size = new Size(320, 30);
            lblTopPersonnelTitle.TabIndex = 0;
            lblTopPersonnelTitle.Text = "CBCS tích cực nhất";
            // 
            // pnlCardEnded
            // 
            pnlCardEnded.BackColor = Color.FromArgb(255, 238, 239);
            pnlCardEnded.BorderStyle = BorderStyle.None;
            pnlCardEnded.Controls.Add(lblEndedUnit);
            pnlCardEnded.Controls.Add(lblDaKetThuc);
            pnlCardEnded.Controls.Add(lblDaKetThucTitle);
            pnlCardEnded.Location = new Point(628, 0);
            pnlCardEnded.Name = "pnlCardEnded";
            pnlCardEnded.Size = new Size(299, 125);
            pnlCardEnded.TabIndex = 2;
            // 
            // lblEndedUnit
            // 
            lblEndedUnit.Font = new Font("Segoe UI", 12F);
            lblEndedUnit.ForeColor = Color.FromArgb(165, 28, 32);
            lblEndedUnit.Location = new Point(205, 79);
            lblEndedUnit.Name = "lblEndedUnit";
            lblEndedUnit.Size = new Size(90, 32);
            lblEndedUnit.TabIndex = 2;
            lblEndedUnit.Text = "lượt";
            // 
            // lblDaKetThuc
            // 
            lblDaKetThuc.Font = new Font("Segoe UI", 30F, FontStyle.Bold);
            lblDaKetThuc.ForeColor = Color.FromArgb(160, 24, 31);
            lblDaKetThuc.Location = new Point(118, 55);
            lblDaKetThuc.Name = "lblDaKetThuc";
            lblDaKetThuc.Size = new Size(100, 67);
            lblDaKetThuc.TabIndex = 1;
            lblDaKetThuc.Text = "0";
            // 
            // lblDaKetThucTitle
            // 
            lblDaKetThucTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblDaKetThucTitle.ForeColor = Color.FromArgb(165, 28, 32);
            lblDaKetThucTitle.Location = new Point(118, 24);
            lblDaKetThucTitle.Name = "lblDaKetThucTitle";
            lblDaKetThucTitle.Size = new Size(220, 30);
            lblDaKetThucTitle.TabIndex = 0;
            lblDaKetThucTitle.Text = "Đã kết thúc";
            // 
            // pnlCardActive
            // 
            pnlCardActive.BackColor = Color.FromArgb(231, 249, 240);
            pnlCardActive.BorderStyle = BorderStyle.None;
            pnlCardActive.Controls.Add(lblActiveUnit);
            pnlCardActive.Controls.Add(lblDangHoatDong);
            pnlCardActive.Controls.Add(lblDangHoatDongTitle);
            pnlCardActive.Location = new Point(316, 0);
            pnlCardActive.Name = "pnlCardActive";
            pnlCardActive.Size = new Size(299, 125);
            pnlCardActive.TabIndex = 1;
            // 
            // lblActiveUnit
            // 
            lblActiveUnit.Font = new Font("Segoe UI", 12F);
            lblActiveUnit.ForeColor = Color.FromArgb(16, 105, 58);
            lblActiveUnit.Location = new Point(205, 79);
            lblActiveUnit.Name = "lblActiveUnit";
            lblActiveUnit.Size = new Size(90, 32);
            lblActiveUnit.TabIndex = 2;
            lblActiveUnit.Text = "lượt";
            // 
            // lblDangHoatDong
            // 
            lblDangHoatDong.Font = new Font("Segoe UI", 30F, FontStyle.Bold);
            lblDangHoatDong.ForeColor = Color.FromArgb(12, 107, 57);
            lblDangHoatDong.Location = new Point(118, 55);
            lblDangHoatDong.Name = "lblDangHoatDong";
            lblDangHoatDong.Size = new Size(100, 67);
            lblDangHoatDong.TabIndex = 1;
            lblDangHoatDong.Text = "0";
            // 
            // lblDangHoatDongTitle
            // 
            lblDangHoatDongTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblDangHoatDongTitle.ForeColor = Color.FromArgb(16, 105, 58);
            lblDangHoatDongTitle.Location = new Point(118, 24);
            lblDangHoatDongTitle.Name = "lblDangHoatDongTitle";
            lblDangHoatDongTitle.Size = new Size(230, 30);
            lblDangHoatDongTitle.TabIndex = 0;
            lblDangHoatDongTitle.Text = "Đang hoạt động";
            // 
            // pnlCardTotal
            // 
            pnlCardTotal.BackColor = Color.FromArgb(232, 244, 255);
            pnlCardTotal.BorderStyle = BorderStyle.None;
            pnlCardTotal.Controls.Add(lblTotalUnit);
            pnlCardTotal.Controls.Add(lblTongCongTac);
            pnlCardTotal.Controls.Add(lblTongCongTacTitle);
            pnlCardTotal.Location = new Point(0, 0);
            pnlCardTotal.Name = "pnlCardTotal";
            pnlCardTotal.Size = new Size(299, 125);
            pnlCardTotal.TabIndex = 0;
            // 
            // lblTotalUnit
            // 
            lblTotalUnit.Font = new Font("Segoe UI", 12F);
            lblTotalUnit.ForeColor = Color.FromArgb(30, 90, 170);
            lblTotalUnit.Location = new Point(205, 79);
            lblTotalUnit.Name = "lblTotalUnit";
            lblTotalUnit.Size = new Size(90, 32);
            lblTotalUnit.TabIndex = 2;
            lblTotalUnit.Text = "lượt";
            // 
            // lblTongCongTac
            // 
            lblTongCongTac.Font = new Font("Segoe UI", 30F, FontStyle.Bold);
            lblTongCongTac.ForeColor = Color.FromArgb(13, 76, 165);
            lblTongCongTac.Location = new Point(118, 55);
            lblTongCongTac.Name = "lblTongCongTac";
            lblTongCongTac.Size = new Size(100, 67);
            lblTongCongTac.TabIndex = 1;
            lblTongCongTac.Text = "0";
            // 
            // lblTongCongTacTitle
            // 
            lblTongCongTacTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTongCongTacTitle.ForeColor = Color.FromArgb(20, 70, 145);
            lblTongCongTacTitle.Location = new Point(118, 24);
            lblTongCongTacTitle.Name = "lblTongCongTacTitle";
            lblTongCongTacTitle.Size = new Size(220, 30);
            lblTongCongTacTitle.TabIndex = 0;
            lblTongCongTacTitle.Text = "Tổng công tác";
            // 
            // FrmMissionDashboard
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(242, 248, 255);
            ClientSize = new Size(1280, 820);
            Controls.Add(pnlDashboard);
            Controls.Add(btnRefresh);
            Controls.Add(btnClose);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 10F);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(1080, 700);
            Name = "FrmMissionDashboard";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Bảng điều khiển công tác";
            Load += FrmMissionDashboard_Load;
            pnlHeader.ResumeLayout(false);
            pnlDashboard.ResumeLayout(false);
            pnlTopPersonnel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvTopPersonnel).EndInit();
            pnlRecent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvRecentMission).EndInit();
            pnlCardTop.ResumeLayout(false);
            pnlCardEnded.ResumeLayout(false);
            pnlCardActive.ResumeLayout(false);
            pnlCardTotal.ResumeLayout(false);
            ResumeLayout(false);
        }

        private Panel pnlHeader;
        private Label lblHeaderIcon;
        private Label lblTitle;
        private Label lblSubtitle;
        private Button btnRefresh;
        private Button btnClose;
        private Panel pnlDashboard;
        private Panel pnlCardTotal;
        private Label lblTongCongTacTitle;
        private Label lblTongCongTac;
        private Label lblTotalUnit;
        private Panel pnlCardActive;
        private Label lblDangHoatDongTitle;
        private Label lblDangHoatDong;
        private Label lblActiveUnit;
        private Panel pnlCardEnded;
        private Label lblDaKetThucTitle;
        private Label lblDaKetThuc;
        private Label lblEndedUnit;
        private Panel pnlCardTop;
        private Label lblTopPersonnelTitle;
        private Label lblTopPersonnel;
        private Label lblTopPersonnelCount;
        private Panel pnlRecent;
        private Label lblRecentTitle;
        private DataGridView dgvRecentMission;
        private Label lblRecentEmpty;
        private Panel pnlTopPersonnel;
        private Label lblPersonnelTableTitle;
        private DataGridView dgvTopPersonnel;
    }
}