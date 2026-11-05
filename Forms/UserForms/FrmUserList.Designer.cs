namespace QuanLyCBCS.Forms.UserForms
{
    partial class FrmUserList
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            pnlMain = new Panel();
            pnlBody = new Panel();
            pnlGridSurface = new Panel();
            pnlToolbarSpacer = new Panel();
            pnlStatusSpacer = new Panel();
            dgvUsers = new DataGridView();
            pnlHeader = new Panel();
            pnlHeaderAccent = new Panel();
            lblHeaderIcon = new Label();
            lblHeaderTitle = new Label();
            lblHeaderSubtitle = new Label();
            pnlToolbar = new Panel();
            btnUnlock = new Button();
            btnRefresh = new Button();
            btnResetPassword = new Button();
            btnDeleteUser = new Button();
            btnAddUser = new Button();
            pnlStatus = new Panel();
            lblStatusIcon = new Label();
            lblUserCount = new Label();
            toolTipMain = new ToolTip(components);

            ((System.ComponentModel.ISupportInitialize)dgvUsers).BeginInit();
            pnlMain.SuspendLayout();
            pnlGridSurface.SuspendLayout();
            pnlHeader.SuspendLayout();
            pnlToolbar.SuspendLayout();
            pnlStatus.SuspendLayout();
            SuspendLayout();

            // pnlMain
            pnlMain.BackColor = Color.FromArgb(242, 247, 253);
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Padding = new Padding(0);
            pnlMain.Name = "pnlMain";
            pnlMain.Controls.Add(pnlBody);
            pnlMain.Controls.Add(pnlHeader);

            // pnlBody: tạo khoảng đệm riêng cho thanh công cụ, bảng và trạng thái.
            pnlBody.BackColor = Color.FromArgb(242, 247, 253);
            pnlBody.Dock = DockStyle.Fill;
            pnlBody.Padding = new Padding(10, 10, 10, 10);
            pnlBody.Name = "pnlBody";
            pnlBody.Controls.Add(pnlGridSurface);
            // DockStyle.Bottom được xử lý theo thứ tự z-order ngược:
            // thêm spacer trước, status sau để spacer nằm phía trên thẻ tổng.
            pnlBody.Controls.Add(pnlStatusSpacer);
            pnlBody.Controls.Add(pnlStatus);
            pnlBody.Controls.Add(pnlToolbarSpacer);
            pnlBody.Controls.Add(pnlToolbar);

            // Khoảng cách độc lập giữa thanh công cụ - bảng - thẻ tổng.
            pnlToolbarSpacer.BackColor = pnlBody.BackColor;
            pnlToolbarSpacer.Dock = DockStyle.Top;
            pnlToolbarSpacer.Height = 12;
            pnlToolbarSpacer.Name = "pnlToolbarSpacer";

            pnlStatusSpacer.BackColor = pnlBody.BackColor;
            pnlStatusSpacer.Dock = DockStyle.Bottom;
            pnlStatusSpacer.Height = 14;
            pnlStatusSpacer.Name = "pnlStatusSpacer";

            // pnlGridSurface: nền trắng của khu vực danh sách, không tạo viền đen.
            pnlGridSurface.BackColor = Color.White;
            pnlGridSurface.Dock = DockStyle.Fill;
            pnlGridSurface.Padding = new Padding(1);
            pnlGridSurface.Name = "pnlGridSurface";
            pnlGridSurface.Margin = new Padding(0, 13, 0, 16);
            pnlGridSurface.Controls.Add(dgvUsers);
            pnlGridSurface.Paint += pnlGridSurface_Paint;

            // pnlHeader
            pnlHeader.BackColor = Color.FromArgb(0, 83, 171);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 100;
            pnlHeader.Margin = new Padding(0, 0, 0, 8);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new Padding(18, 0, 18, 0);
            pnlHeader.Paint += pnlHeader_Paint;
            pnlHeader.Controls.Add(lblHeaderSubtitle);
            pnlHeader.Controls.Add(lblHeaderTitle);
            pnlHeader.Controls.Add(pnlHeaderAccent);
            pnlHeader.Controls.Add(lblHeaderIcon);

            // lblHeaderIcon
            lblHeaderIcon.Dock = DockStyle.Left;
            lblHeaderIcon.Width = 88;
            lblHeaderIcon.Text = "\uE716";
            lblHeaderIcon.Font = new Font("Segoe MDL2 Assets", 39F, FontStyle.Regular);
            lblHeaderIcon.ForeColor = Color.White;
            lblHeaderIcon.TextAlign = ContentAlignment.MiddleCenter;
            lblHeaderIcon.BackColor = Color.Transparent;

            // pnlHeaderAccent
            pnlHeaderAccent.Dock = DockStyle.Left;
            pnlHeaderAccent.Width = 1;
            pnlHeaderAccent.Margin = new Padding(10, 16, 0, 16);
            pnlHeaderAccent.BackColor = Color.FromArgb(100, 220, 240, 255);

            // lblHeaderTitle
            lblHeaderTitle.AutoSize = true;
            lblHeaderTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblHeaderTitle.ForeColor = Color.White;
            lblHeaderTitle.BackColor = Color.Transparent;
            lblHeaderTitle.Location = new Point(126, 12);
            lblHeaderTitle.Name = "lblHeaderTitle";
            lblHeaderTitle.Text = "QUẢN LÝ TÀI KHOẢN";

            // lblHeaderSubtitle
            lblHeaderSubtitle.AutoSize = true;
            lblHeaderSubtitle.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            lblHeaderSubtitle.ForeColor = Color.White;
            lblHeaderSubtitle.BackColor = Color.Transparent;
            lblHeaderSubtitle.Location = new Point(127, 60);
            lblHeaderSubtitle.Name = "lblHeaderSubtitle";
            lblHeaderSubtitle.Text = "Quản trị người dùng hệ thống";

            // pnlToolbar
            pnlToolbar.BackColor = Color.FromArgb(247, 250, 255);
            pnlToolbar.Dock = DockStyle.Top;
            pnlToolbar.Height = 76;
            pnlToolbar.Padding = new Padding(12, 8, 12, 8);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Controls.Add(btnUnlock);
            pnlToolbar.Controls.Add(btnRefresh);
            pnlToolbar.Controls.Add(btnResetPassword);
            pnlToolbar.Controls.Add(btnDeleteUser);
            pnlToolbar.Controls.Add(btnAddUser);
            pnlToolbar.Paint += pnlToolbar_Paint;

            // btnAddUser
            ConfigureToolbarButton(btnAddUser, "＋  Thêm tài", new Point(14, 13), new Size(168, 46), Color.FromArgb(28, 181, 99));
            btnAddUser.Click += btnAddUser_Click;

            // btnDeleteUser
            ConfigureToolbarButton(btnDeleteUser, "▤  Xóa", new Point(196, 13), new Size(145, 46), Color.FromArgb(239, 67, 59));
            btnDeleteUser.Click += btnDeleteUser_Click;

            // btnResetPassword
            ConfigureToolbarButton(btnResetPassword, "↻  Reset", new Point(356, 13), new Size(153, 46), Color.FromArgb(247, 158, 17));
            btnResetPassword.Click += btnResetPassword_Click;

            // btnRefresh
            ConfigureToolbarButton(btnRefresh, "▣  Làm mới", new Point(523, 13), new Size(163, 46), Color.FromArgb(42, 153, 239));
            btnRefresh.Click += btnRefresh_Click;

            // btnUnlock
            ConfigureToolbarButton(btnUnlock, "🔓  Mở khóa", new Point(700, 13), new Size(156, 46), Color.FromArgb(190, 27, 157));
            btnUnlock.Click += btnUnlock_Click;

            // dgvUsers
            dgvUsers.Dock = DockStyle.Fill;
            dgvUsers.Name = "dgvUsers";
            dgvUsers.Margin = new Padding(2, 10, 2, 0);
            dgvUsers.BackgroundColor = Color.White;
            dgvUsers.BorderStyle = BorderStyle.None;
            dgvUsers.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvUsers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvUsers.ColumnHeadersHeight = 51;
            dgvUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvUsers.RowHeadersWidth = 48;
            dgvUsers.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsers.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
            dgvUsers.AllowUserToResizeRows = false;
            dgvUsers.MultiSelect = false;
            dgvUsers.ReadOnly = true;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.RowTemplate.Height = 52;
            dgvUsers.EnableHeadersVisualStyles = false;
            dgvUsers.CellFormatting += dgvUsers_CellFormatting;
            dgvUsers.CellPainting += dgvUsers_CellPainting;
            dgvUsers.CellClick += dgvUsers_CellClick;

            // pnlStatus
            pnlStatus.BackColor = Color.FromArgb(211, 232, 252);
            pnlStatus.Dock = DockStyle.Bottom;
            pnlStatus.Height = 53;
            pnlStatus.Padding = new Padding(14, 0, 14, 0);
            pnlStatus.Name = "pnlStatus";
            pnlStatus.Controls.Add(lblUserCount);
            pnlStatus.Controls.Add(lblStatusIcon);

            // lblStatusIcon
            lblStatusIcon.Dock = DockStyle.Left;
            lblStatusIcon.Width = 28;
            lblStatusIcon.Text = "\uE946";
            lblStatusIcon.Font = new Font("Segoe MDL2 Assets", 15F);
            lblStatusIcon.ForeColor = Color.FromArgb(0, 116, 220);
            lblStatusIcon.TextAlign = ContentAlignment.MiddleCenter;

            // lblUserCount
            lblUserCount.Dock = DockStyle.Fill;
            lblUserCount.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblUserCount.ForeColor = Color.FromArgb(0, 55, 110);
            lblUserCount.TextAlign = ContentAlignment.MiddleLeft;
            lblUserCount.Name = "lblUserCount";
            lblUserCount.Text = "Đang hiển thị: 0 tài khoản";

            // toolTipMain
            toolTipMain.AutoPopDelay = 5000;
            toolTipMain.InitialDelay = 300;
            toolTipMain.ReshowDelay = 100;
            toolTipMain.ShowAlways = true;

            // FrmUserList
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(242, 247, 253);
            ClientSize = new Size(1380, 725);
            Controls.Add(pnlMain);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmUserList";
            StartPosition = FormStartPosition.Manual;
            Text = "Quản lý tài khoản";
            Load += FrmUserList_Load;

            pnlGridSurface.ResumeLayout(false);
            pnlStatus.ResumeLayout(false);
            pnlToolbar.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvUsers).EndInit();
            ResumeLayout(false);
        }

        private void ConfigureToolbarButton(Button button, string text, Point location, Size size, Color backColor)
        {
            button.BackColor = backColor;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = ControlPaint.Light(backColor, 0.12f);
            button.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(backColor, 0.08f);
            button.ForeColor = Color.White;
            button.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            button.Location = location;
            button.Size = size;
            button.Text = text;
            button.TextAlign = ContentAlignment.MiddleCenter;
            button.UseVisualStyleBackColor = false;
            button.Cursor = Cursors.Hand;
            button.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        }

        #endregion

        private Panel pnlMain;
        private Panel pnlBody;
        private Panel pnlGridSurface;
        private Panel pnlToolbarSpacer;
        private Panel pnlStatusSpacer;
        private DataGridView dgvUsers;
        private Panel pnlHeader;
        private Panel pnlHeaderAccent;
        private Label lblHeaderIcon;
        private Label lblHeaderTitle;
        private Label lblHeaderSubtitle;
        private Panel pnlToolbar;
        private Button btnAddUser;
        private Button btnRefresh;
        private Button btnResetPassword;
        private Button btnDeleteUser;
        private Button btnUnlock;
        private Panel pnlStatus;
        private Label lblStatusIcon;
        private Label lblUserCount;
        private ToolTip toolTipMain;
    }
}
