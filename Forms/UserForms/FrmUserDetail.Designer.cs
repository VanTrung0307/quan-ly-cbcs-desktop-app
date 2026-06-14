namespace QuanLyCBCS.Forms.UserForms
{
    partial class FrmUserDetail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmUserDetail));
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            txtPassword = new TextBox();
            cboRole = new ComboBox();
            btnSave = new Button();
            btnCancel = new Button();
            panel1 = new Panel();
            lblSubTitle = new Label();
            lblTitle = new Label();
            txtUsername = new TextBox();
            toolTipMain = new ToolTip(components);
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F);
            label1.ForeColor = Color.FromArgb(44, 62, 80);
            label1.Location = new Point(87, 119);
            label1.Name = "label1";
            label1.Size = new Size(136, 23);
            label1.TabIndex = 0;
            label1.Text = "Tên đăng nhập *";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F);
            label2.ForeColor = Color.FromArgb(44, 62, 80);
            label2.Location = new Point(87, 163);
            label2.Name = "label2";
            label2.Size = new Size(94, 23);
            label2.TabIndex = 1;
            label2.Text = "Mật khẩu *";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F);
            label3.ForeColor = Color.FromArgb(44, 62, 80);
            label3.Location = new Point(87, 210);
            label3.Name = "label3";
            label3.Size = new Size(72, 23);
            label3.TabIndex = 2;
            label3.Text = "Vai trò *";
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Segoe UI", 10F);
            txtPassword.Location = new Point(227, 159);
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = " Nhập mật khẩu";
            txtPassword.Size = new Size(250, 30);
            txtPassword.TabIndex = 4;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // cboRole
            // 
            cboRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cboRole.Font = new Font("Segoe UI", 10F);
            cboRole.FormattingEnabled = true;
            cboRole.Location = new Point(227, 206);
            cboRole.Name = "cboRole";
            cboRole.Size = new Size(250, 31);
            cboRole.TabIndex = 5;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(39, 174, 96);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(132, 270);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(120, 40);
            btnSave.TabIndex = 6;
            btnSave.Text = "💾 Lưu";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(231, 76, 60);
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(302, 270);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(120, 40);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "❌ Huỷ";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(44, 62, 80);
            panel1.Controls.Add(lblSubTitle);
            panel1.Controls.Add(lblTitle);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(565, 90);
            panel1.TabIndex = 8;
            // 
            // lblSubTitle
            // 
            lblSubTitle.AutoSize = true;
            lblSubTitle.ForeColor = Color.Gainsboro;
            lblSubTitle.Location = new Point(170, 50);
            lblSubTitle.Name = "lblSubTitle";
            lblSubTitle.Size = new Size(213, 20);
            lblSubTitle.TabIndex = 1;
            lblSubTitle.Text = "Thêm hoặc chỉnh sửa tài khoản";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(118, 9);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(321, 41);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "QUẢN LÝ TÀI KHOẢN";
            // 
            // txtUsername
            // 
            txtUsername.Font = new Font("Segoe UI", 10F);
            txtUsername.Location = new Point(227, 115);
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderText = " Nhập tên đăng nhập";
            txtUsername.Size = new Size(250, 30);
            txtUsername.TabIndex = 3;
            // 
            // FrmUserDetail
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(565, 367);
            Controls.Add(panel1);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(cboRole);
            Controls.Add(txtPassword);
            Controls.Add(txtUsername);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FrmUserDetail";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Thông tin tài khoản";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txtPassword;
        private ComboBox cboRole;
        private Button btnSave;
        private Button btnCancel;
        private Panel panel1;
        private Label lblTitle;
        private Label lblSubTitle;
        private TextBox txtUsername;
        private ToolTip toolTipMain;
    }
}