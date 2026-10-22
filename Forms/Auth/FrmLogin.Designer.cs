namespace QuanLyCBCS.Forms.Auth
{
    partial class FrmLogin
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
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(FrmLogin));

            pnlBrand = new Panel();
            pnlLoginArea = new Panel();
            pnlLoginCard = new Panel();

            label1 = new Label();
            label2 = new Label();
            label3 = new Label();

            txtUsername = new TextBox();
            txtPassword = new TextBox();

            btnLogin = new Button();
            btnExit = new Button();

            pictureBox1 = new PictureBox();
            lblSubTitle = new Label();
            lblVersion = new Label();

            lblBrandTitle = new Label();
            lblBrandSubtitle = new Label();
            lblBrandSlogan = new Label();
            lblDivider = new Label();

            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();

            // ==========================================
            // FORM
            // ==========================================
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 248, 252);
            ClientSize = new Size(1500, 940);
            MinimumSize = new Size(1200, 760);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Sizable;
            MaximizeBox = true;
            MinimizeBox = true;
            WindowState = FormWindowState.Maximized;
            Text = "Hệ thống Quản lý CBCS";

            if (resources.GetObject("$this.Icon") is Icon appIcon)
                Icon = appIcon;

            // ==========================================
            // PANEL TRÁI - THƯƠNG HIỆU CSCĐ
            // ==========================================
            pnlBrand.Dock = DockStyle.Left;
            pnlBrand.Width = 750;
            pnlBrand.MinimumSize = new Size(400, 0);
            pnlBrand.BackColor = Color.FromArgb(12, 54, 105);
            pnlBrand.BackgroundImageLayout = ImageLayout.Stretch;
            pnlBrand.Name = "pnlBrand";

            // Ảnh nền đã thêm vào Resources.
            // Đổi "TEN_RESOURCE_ANH_NEN" thành tên Resource thực tế.
            pnlBrand.BackgroundImage =
                Properties.Resources.ResourceManager.GetObject(
                    "LoginBackground"
                ) as Image;

            // Không tạo PictureBox logo riêng vì ảnh nền thiết kế
            // đã chứa logo, tiêu đề và khẩu hiệu.
            lblBrandTitle.AutoSize = false;
            lblBrandTitle.Dock = DockStyle.Top;
            lblBrandTitle.Height = 0;
            lblBrandTitle.Visible = false;

            lblBrandSubtitle.Visible = false;
            lblBrandSlogan.Visible = false;
            lblDivider.Visible = false;

            pnlBrand.Controls.Add(lblBrandTitle);
            pnlBrand.Controls.Add(lblBrandSubtitle);
            pnlBrand.Controls.Add(lblBrandSlogan);
            pnlBrand.Controls.Add(lblDivider);

            // ==========================================
            // PANEL PHẢI - KHU VỰC ĐĂNG NHẬP
            // ==========================================
            pnlLoginArea.Dock = DockStyle.Fill;
            pnlLoginArea.BackColor = Color.FromArgb(247, 249, 252);
            pnlLoginArea.Padding = new Padding(35);
            pnlLoginArea.Name = "pnlLoginArea";

            // ==========================================
            // THẺ ĐĂNG NHẬP
            // ==========================================
            pnlLoginCard.Size = new Size(650, 800);
            pnlLoginCard.MaximumSize = new Size(700, 850);
            pnlLoginCard.MinimumSize = new Size(450, 650);
            pnlLoginCard.BackColor = Color.White;
            pnlLoginCard.Anchor = AnchorStyles.None;
            pnlLoginCard.Name = "pnlLoginCard";

            // ==========================================
            // TIÊU ĐỀ ĐĂNG NHẬP
            // ==========================================
            label1.AutoSize = false;
            label1.Font = new Font("Segoe UI", 25F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(22, 56, 99);
            label1.Location = new Point(30, 48);
            label1.Size = new Size(590, 58);
            label1.Text = "ĐĂNG NHẬP HỆ THỐNG";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            label1.Name = "label1";

            lblSubTitle.AutoSize = false;
            lblSubTitle.Font = new Font("Segoe UI", 14F);
            lblSubTitle.ForeColor = Color.FromArgb(105, 121, 145);
            lblSubTitle.Location = new Point(35, 112);
            lblSubTitle.Size = new Size(580, 36);
            lblSubTitle.Text = "Quản lý cán bộ chiến sĩ Cảnh sát cơ động";
            lblSubTitle.TextAlign = ContentAlignment.MiddleCenter;

            // Gạch nhấn màu xanh
            lblDivider.AutoSize = false;
            lblDivider.BackColor = Color.FromArgb(0, 103, 230);
            lblDivider.Location = new Point(275, 177);
            lblDivider.Size = new Size(100, 5);
            lblDivider.Visible = true;

            // ==========================================
            // TÊN ĐĂNG NHẬP
            // ==========================================
            label2.AutoSize = false;
            label2.Font = new Font("Segoe UI", 12F);
            label2.ForeColor = Color.FromArgb(22, 56, 99);
            label2.Location = new Point(75, 220);
            label2.Size = new Size(500, 30);
            label2.Text = "Tên đăng nhập";

            txtUsername.BorderStyle = BorderStyle.FixedSingle;
            txtUsername.Font = new Font("Segoe UI", 13F);
            txtUsername.Location = new Point(75, 255);
            txtUsername.Size = new Size(500, 42);
            txtUsername.PlaceholderText = "Nhập tên đăng nhập...";
            txtUsername.TabIndex = 0;
            txtUsername.Name = "txtUsername";

            // ==========================================
            // MẬT KHẨU
            // ==========================================
            label3.AutoSize = false;
            label3.Font = new Font("Segoe UI", 12F);
            label3.ForeColor = Color.FromArgb(22, 56, 99);
            label3.Location = new Point(75, 330);
            label3.Size = new Size(500, 30);
            label3.Text = "Mật khẩu";

            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.Font = new Font("Segoe UI", 13F);
            txtPassword.Location = new Point(75, 365);
            txtPassword.Size = new Size(500, 42);
            txtPassword.PlaceholderText = "Nhập mật khẩu...";
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.TabIndex = 1;
            txtPassword.Name = "txtPassword";

            // ==========================================
            // NÚT ĐĂNG NHẬP
            // ==========================================
            btnLogin.BackColor = Color.FromArgb(0, 101, 230);
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(75, 475);
            btnLogin.Size = new Size(500, 65);
            btnLogin.Text = "Đăng nhập";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.TabIndex = 2;
            btnLogin.Name = "btnLogin";
            btnLogin.Click += btnLogin_Click;

            // ==========================================
            // NÚT THOÁT
            // ==========================================
            btnExit.BackColor = Color.FromArgb(231, 237, 246);
            btnExit.Cursor = Cursors.Hand;
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            btnExit.ForeColor = Color.FromArgb(190, 45, 35);
            btnExit.Location = new Point(75, 565);
            btnExit.Size = new Size(500, 60);
            btnExit.Text = "Thoát";
            btnExit.UseVisualStyleBackColor = false;
            btnExit.TabIndex = 3;
            btnExit.Name = "btnExit";
            btnExit.Click += btnExit_Click;

            // ==========================================
            // PHIÊN BẢN
            // ==========================================
            lblVersion.AutoSize = false;
            lblVersion.Font = new Font("Segoe UI", 11F);
            lblVersion.ForeColor = Color.FromArgb(105, 121, 145);
            lblVersion.Location = new Point(75, 690);
            lblVersion.Size = new Size(500, 30);
            lblVersion.Text = "Quản lý CBCS v2.0";
            lblVersion.TextAlign = ContentAlignment.MiddleCenter;

            // PictureBox cũ không còn cần thiết:
            // ảnh nền bên trái đã chứa logo CSCĐ.
            pictureBox1.Visible = false;
            pictureBox1.Size = Size.Empty;

            // Đưa các control đăng nhập vào thẻ trắng.
            pnlLoginCard.Controls.Add(label1);
            pnlLoginCard.Controls.Add(lblSubTitle);
            pnlLoginCard.Controls.Add(lblDivider);
            pnlLoginCard.Controls.Add(label2);
            pnlLoginCard.Controls.Add(txtUsername);
            pnlLoginCard.Controls.Add(label3);
            pnlLoginCard.Controls.Add(txtPassword);
            pnlLoginCard.Controls.Add(btnLogin);
            pnlLoginCard.Controls.Add(btnExit);
            pnlLoginCard.Controls.Add(lblVersion);

            // Đặt thẻ đăng nhập ở giữa vùng bên phải.
            pnlLoginArea.Controls.Add(pnlLoginCard);

            // Bố cục tổng thể.
            Controls.Add(pnlLoginArea);
            Controls.Add(pnlBrand);

            AcceptButton = btnLogin;
            CancelButton = btnExit;

            // Căn thẻ đăng nhập vào giữa panel bên phải.
            pnlLoginArea.Resize += (sender, e) =>
            {
                pnlLoginCard.Left =
                    Math.Max(0, (pnlLoginArea.ClientSize.Width
                                - pnlLoginCard.Width) / 2);

                pnlLoginCard.Top =
                    Math.Max(0, (pnlLoginArea.ClientSize.Height
                                - pnlLoginCard.Height) / 2);
            };

            pnlLoginArea.PerformLayout();

            pnlLoginCard.Left =
                Math.Max(0, (pnlLoginArea.ClientSize.Width
                            - pnlLoginCard.Width) / 2);

            pnlLoginCard.Top =
                Math.Max(0, (pnlLoginArea.ClientSize.Height
                            - pnlLoginCard.Height) / 2);

            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }


        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnExit;
        private PictureBox pictureBox1;
        private Label lblSubTitle;
        private Label lblVersion;
        private Panel pnlBrand;
        private Panel pnlLoginArea;
        private Panel pnlLoginCard;
        private Label lblBrandTitle;
        private Label lblBrandSubtitle;
        private Label lblBrandSlogan;
        private Label lblDivider;
    }
}