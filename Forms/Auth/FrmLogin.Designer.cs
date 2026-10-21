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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLogin));
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
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top;
            label1.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(44, 62, 80);
            label1.Location = new Point(-126, 9);
            label1.Name = "label1";
            label1.Size = new Size(789, 46);
            label1.TabIndex = 0;
            label1.Text = "ĐĂNG NHẬP HỆ THỐNG";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F);
            label2.Location = new Point(55, 129);
            label2.Name = "label2";
            label2.Size = new Size(124, 23);
            label2.TabIndex = 1;
            label2.Text = "Tên đăng nhập";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F);
            label3.Location = new Point(55, 181);
            label3.Name = "label3";
            label3.Size = new Size(82, 23);
            label3.TabIndex = 2;
            label3.Text = "Mật khẩu";
            // 
            // txtUsername
            // 
            txtUsername.BorderStyle = BorderStyle.FixedSingle;
            txtUsername.Font = new Font("Segoe UI", 11F);
            txtUsername.Location = new Point(185, 125);
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderText = " Username...";
            txtUsername.Size = new Size(222, 32);
            txtUsername.TabIndex = 1;
            // 
            // txtPassword
            // 
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.Font = new Font("Segoe UI", 11F);
            txtPassword.Location = new Point(185, 177);
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = " Password...";
            txtPassword.Size = new Size(222, 32);
            txtPassword.TabIndex = 2;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.FromArgb(44, 62, 80);
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(121, 247);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(150, 45);
            btnLogin.TabIndex = 3;
            btnLogin.Text = " 🔐 Đăng nhập";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // btnExit
            // 
            btnExit.BackColor = Color.Firebrick;
            btnExit.Cursor = Cursors.Hand;
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnExit.ForeColor = Color.White;
            btnExit.Location = new Point(299, 247);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(120, 45);
            btnExit.TabIndex = 4;
            btnExit.Text = "🚪 Thoát";
            btnExit.UseVisualStyleBackColor = false;
            btnExit.Click += btnExit_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.logoCSCD;
            pictureBox1.Location = new Point(165, 335);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(220, 220);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 7;
            pictureBox1.TabStop = false;
            // 
            // lblSubTitle
            // 
            lblSubTitle.AutoSize = true;
            lblSubTitle.Font = new Font("Segoe UI", 9F);
            lblSubTitle.ForeColor = Color.Gray;
            lblSubTitle.Location = new Point(155, 55);
            lblSubTitle.Name = "lblSubTitle";
            lblSubTitle.Size = new Size(202, 20);
            lblSubTitle.TabIndex = 8;
            lblSubTitle.Text = "Quản lý cán bộ chiến sĩ CSCĐ";
            // 
            // lblVersion
            // 
            lblVersion.AutoSize = true;
            lblVersion.Font = new Font("Segoe UI", 8F);
            lblVersion.ForeColor = Color.Gray;
            lblVersion.Location = new Point(207, 559);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(124, 19);
            lblVersion.TabIndex = 9;
            lblVersion.Text = "Quản lý CBCS v2.0";
            // 
            // FrmLogin
            // 
            AcceptButton = btnLogin;
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 246, 250);
            CancelButton = btnExit;
            ClientSize = new Size(533, 587);
            Controls.Add(lblVersion);
            Controls.Add(lblSubTitle);
            Controls.Add(pictureBox1);
            Controls.Add(btnExit);
            Controls.Add(btnLogin);
            Controls.Add(txtPassword);
            Controls.Add(txtUsername);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Font = new Font("Segoe UI", 10F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hệ thống Quản lý CBCS";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
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
    }
}