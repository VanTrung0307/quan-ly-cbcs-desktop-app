namespace QuanLyCBCS.Forms.CommonForms
{
    partial class FrmNotificationCenter
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            headerPanel = new Panel();
            iconPanel = new Panel();
            lblTitle = new Label();
            lblSubtitle = new Label();
            toolbarPanel = new Panel();
            searchBorderPanel = new Panel();
            searchIconPanel = new Panel();
            txtSearch = new TextBox();
            btnRefresh = new Button();
            btnClose = new Button();
            gridContainer = new Panel();
            dgvNotifications = new DataGridView();
            emptyStatePanel = new Panel();
            emptyBellPanel = new Panel();
            lblEmptyTitle = new Label();
            lblEmptyDescription = new Label();
            headerPanel.SuspendLayout();
            toolbarPanel.SuspendLayout();
            searchBorderPanel.SuspendLayout();
            gridContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvNotifications).BeginInit();
            emptyStatePanel.SuspendLayout();
            SuspendLayout();
            // headerPanel
            headerPanel.Controls.Add(lblSubtitle);
            headerPanel.Controls.Add(lblTitle);
            headerPanel.Controls.Add(iconPanel);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 140;
            headerPanel.Name = "headerPanel";
            headerPanel.TabIndex = 0;
            // iconPanel
            iconPanel.Location = new Point(28, 22);
            iconPanel.Name = "iconPanel";
            iconPanel.Size = new Size(98, 96);
            iconPanel.TabIndex = 0;
            // lblTitle
            lblTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Font = new Font("Segoe UI", 30F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(154, 29);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(1300, 59);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "TRUNG TÂM THÔNG BÁO";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            // lblSubtitle
            lblSubtitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblSubtitle.BackColor = Color.Transparent;
            lblSubtitle.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            lblSubtitle.ForeColor = Color.FromArgb(226, 239, 253);
            lblSubtitle.Location = new Point(158, 88);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(1250, 28);
            lblSubtitle.TabIndex = 2;
            lblSubtitle.Text = "Theo dõi và cập nhật các thông báo của hệ thống";
            lblSubtitle.TextAlign = ContentAlignment.MiddleLeft;
            // toolbarPanel
            toolbarPanel.Controls.Add(btnClose);
            toolbarPanel.Controls.Add(btnRefresh);
            toolbarPanel.Controls.Add(searchBorderPanel);
            toolbarPanel.Dock = DockStyle.Top;
            toolbarPanel.Height = 92;
            toolbarPanel.Name = "toolbarPanel";
            toolbarPanel.Padding = new Padding(27, 19, 27, 16);
            toolbarPanel.TabIndex = 1;
            toolbarPanel.BackColor = Color.FromArgb(242, 248, 253);
            // searchBorderPanel
            searchBorderPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            searchBorderPanel.BackColor = Color.White;
            searchBorderPanel.BorderStyle = BorderStyle.FixedSingle;
            searchBorderPanel.Controls.Add(txtSearch);
            searchBorderPanel.Controls.Add(searchIconPanel);
            searchBorderPanel.Location = new Point(27, 19);
            searchBorderPanel.Name = "searchBorderPanel";
            searchBorderPanel.Size = new Size(770, 60);
            searchBorderPanel.TabIndex = 0;
            // searchIconPanel
            searchIconPanel.Location = new Point(10, 9);
            searchIconPanel.Name = "searchIconPanel";
            searchIconPanel.Size = new Size(40, 40);
            searchIconPanel.TabIndex = 0;
            // txtSearch
            txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSearch.BorderStyle = BorderStyle.None;
            txtSearch.Font = new Font("Segoe UI", 13F, FontStyle.Regular, GraphicsUnit.Point);
            txtSearch.ForeColor = Color.FromArgb(37, 67, 103);
            txtSearch.Location = new Point(55, 15);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Tìm tiêu đề hoặc nội dung...";
            txtSearch.Size = new Size(700, 24);
            txtSearch.TabIndex = 1;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // btnRefresh
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.BackColor = Color.FromArgb(0, 112, 219);
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 135, 230);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(1179, 19);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(190, 60);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "⟳  Làm mới";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // btnClose
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.BackColor = Color.FromArgb(231, 64, 53);
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 91, 80);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(1385, 19);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(157, 60);
            btnClose.TabIndex = 2;
            btnClose.Text = "✕  Đóng";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // gridContainer
            gridContainer.BackColor = Color.White;
            gridContainer.BorderStyle = BorderStyle.FixedSingle;
            gridContainer.Controls.Add(emptyStatePanel);
            gridContainer.Controls.Add(dgvNotifications);
            gridContainer.Dock = DockStyle.Fill;
            gridContainer.Margin = new Padding(27, 0, 27, 26);
            gridContainer.Name = "gridContainer";
            gridContainer.Padding = new Padding(1);
            gridContainer.TabIndex = 2;
            // dgvNotifications
            dgvNotifications.AllowUserToAddRows = false;
            dgvNotifications.AllowUserToDeleteRows = false;
            dgvNotifications.AllowUserToResizeRows = false;
            dgvNotifications.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvNotifications.BackgroundColor = Color.White;
            dgvNotifications.BorderStyle = BorderStyle.None;
            dgvNotifications.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvNotifications.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvNotifications.ColumnHeadersHeight = 72;
            dgvNotifications.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvNotifications.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvNotifications.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(35, 75, 119);
            dgvNotifications.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold, GraphicsUnit.Point);
            dgvNotifications.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvNotifications.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(35, 75, 119);
            dgvNotifications.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvNotifications.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvNotifications.DefaultCellStyle.BackColor = Color.White;
            dgvNotifications.DefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            dgvNotifications.DefaultCellStyle.ForeColor = Color.FromArgb(24, 58, 96);
            dgvNotifications.DefaultCellStyle.SelectionBackColor = Color.FromArgb(126, 207, 239);
            dgvNotifications.DefaultCellStyle.SelectionForeColor = Color.FromArgb(18, 51, 86);
            dgvNotifications.Dock = DockStyle.Fill;
            dgvNotifications.EnableHeadersVisualStyles = false;
            dgvNotifications.GridColor = Color.FromArgb(206, 226, 244);
            dgvNotifications.MultiSelect = false;
            dgvNotifications.Name = "dgvNotifications";
            dgvNotifications.ReadOnly = true;
            dgvNotifications.RowHeadersVisible = false;
            dgvNotifications.RowTemplate.Height = 43;
            dgvNotifications.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvNotifications.TabIndex = 0;
            // emptyStatePanel (positioned over grid rows only; header remains visible)
            emptyStatePanel.BackColor = Color.White;
            emptyStatePanel.Controls.Add(lblEmptyDescription);
            emptyStatePanel.Controls.Add(lblEmptyTitle);
            emptyStatePanel.Controls.Add(emptyBellPanel);
            emptyStatePanel.Location = new Point(1, 73);
            emptyStatePanel.Name = "emptyStatePanel";
            emptyStatePanel.Size = new Size(1200, 450);
            emptyStatePanel.TabIndex = 1;
            // emptyBellPanel
            emptyBellPanel.Location = new Point(0, 45);
            emptyBellPanel.Name = "emptyBellPanel";
            emptyBellPanel.Size = new Size(170, 170);
            emptyBellPanel.TabIndex = 0;
            // lblEmptyTitle
            lblEmptyTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblEmptyTitle.Font = new Font("Segoe UI", 19F, FontStyle.Bold, GraphicsUnit.Point);
            lblEmptyTitle.ForeColor = Color.FromArgb(24, 58, 101);
            lblEmptyTitle.Location = new Point(0, 230);
            lblEmptyTitle.Name = "lblEmptyTitle";
            lblEmptyTitle.Size = new Size(1200, 43);
            lblEmptyTitle.TabIndex = 1;
            lblEmptyTitle.Text = "Chưa có thông báo";
            lblEmptyTitle.TextAlign = ContentAlignment.MiddleCenter;
            // lblEmptyDescription
            lblEmptyDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblEmptyDescription.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            lblEmptyDescription.ForeColor = Color.FromArgb(89, 126, 170);
            lblEmptyDescription.Location = new Point(0, 278);
            lblEmptyDescription.Name = "lblEmptyDescription";
            lblEmptyDescription.Size = new Size(1200, 39);
            lblEmptyDescription.TabIndex = 2;
            lblEmptyDescription.Text = "Thông báo mới sẽ hiển thị tại đây";
            lblEmptyDescription.TextAlign = ContentAlignment.MiddleCenter;
            // FrmNotificationCenter
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(242, 248, 253);
            ClientSize = new Size(1568, 930);
            Controls.Add(gridContainer);
            Controls.Add(toolbarPanel);
            Controls.Add(headerPanel);
            DoubleBuffered = true;
            Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            MinimumSize = new Size(950, 620);
            Name = "FrmNotificationCenter";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Trung tâm thông báo";
            Load += FrmNotificationCenter_Load;
            headerPanel.ResumeLayout(false);
            toolbarPanel.ResumeLayout(false);
            searchBorderPanel.ResumeLayout(false);
            searchBorderPanel.PerformLayout();
            gridContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvNotifications).EndInit();
            emptyStatePanel.ResumeLayout(false);
            ResumeLayout(false);
        }
        #endregion

        private Panel headerPanel;
        private Panel iconPanel;
        private Label lblTitle;
        private Label lblSubtitle;
        private Panel toolbarPanel;
        private Panel searchBorderPanel;
        private Panel searchIconPanel;
        private TextBox txtSearch;
        private Button btnRefresh;
        private Button btnClose;
        private Panel gridContainer;
        private DataGridView dgvNotifications;
        private Panel emptyStatePanel;
        private Panel emptyBellPanel;
        private Label lblEmptyTitle;
        private Label lblEmptyDescription;
    }
}