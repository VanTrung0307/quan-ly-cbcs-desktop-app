namespace QuanLyCBCS.Forms.UserForms
{
    partial class FrmChangePassword
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmChangePassword));
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            txtOldPassword = new TextBox();
            txtNewPassword = new TextBox();
            txtConfirmPassword = new TextBox();
            btnSave = new Button();
            btnCancel = new Button();
            pnlHeader = new Panel();
            label5 = new Label();
            label4 = new Label();
            toolTipMain = new ToolTip(components);
            pnlHeader.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Font = new Font("Segoe UI", 10F);
            label1.ForeColor = Color.FromArgb(44, 62, 80);
            label1.Location = new Point(81, 127);
            label1.Name = "label1";
            label1.Size = new Size(150, 35);
            label1.TabIndex = 0;
            label1.Text = "Mật khẩu cũ:";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            label2.Font = new Font("Segoe UI", 10F);
            label2.ForeColor = Color.FromArgb(44, 62, 80);
            label2.Location = new Point(81, 188);
            label2.Name = "label2";
            label2.Size = new Size(150, 35);
            label2.TabIndex = 1;
            label2.Text = "Mật khẩu mới:";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            label3.Font = new Font("Segoe UI", 10F);
            label3.ForeColor = Color.FromArgb(44, 62, 80);
            label3.Location = new Point(66, 249);
            label3.Name = "label3";
            label3.Size = new Size(165, 35);
            label3.TabIndex = 2;
            label3.Text = "Xác nhận mật khẩu:";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtOldPassword
            // 
            txtOldPassword.Font = new Font("Segoe UI", 10F);
            txtOldPassword.Location = new Point(237, 127);
            txtOldPassword.Name = "txtOldPassword";
            txtOldPassword.PlaceholderText = " Nhập mật khẩu hiện tại";
            txtOldPassword.Size = new Size(260, 30);
            txtOldPassword.TabIndex = 3;
            txtOldPassword.UseSystemPasswordChar = true;
            // 
            // txtNewPassword
            // 
            txtNewPassword.Font = new Font("Segoe UI", 10F);
            txtNewPassword.Location = new Point(237, 188);
            txtNewPassword.Name = "txtNewPassword";
            txtNewPassword.PlaceholderText = " Nhập mật khẩu mới";
            txtNewPassword.Size = new Size(260, 30);
            txtNewPassword.TabIndex = 4;
            txtNewPassword.UseSystemPasswordChar = true;
            // 
            // txtConfirmPassword
            // 
            txtConfirmPassword.Font = new Font("Segoe UI", 10F);
            txtConfirmPassword.Location = new Point(237, 246);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.PlaceholderText = " Nhập lại mật khẩu mới";
            txtConfirmPassword.Size = new Size(260, 30);
            txtConfirmPassword.TabIndex = 5;
            txtConfirmPassword.UseSystemPasswordChar = true;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(39, 174, 96);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(134, 309);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(130, 40);
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
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(315, 309);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(130, 40);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "❌ Huỷ";
            btnCancel.UseVisualStyleBackColor = false;
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(44, 62, 80);
            pnlHeader.Controls.Add(label5);
            pnlHeader.Controls.Add(label4);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(582, 90);
            pnlHeader.TabIndex = 8;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.ForeColor = Color.Gainsboro;
            label5.Location = new Point(191, 50);
            label5.Name = "label5";
            label5.Size = new Size(198, 20);
            label5.TabIndex = 1;
            label5.Text = "Cập nhật mật khẩu tài khoản";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.White;
            label4.Location = new Point(174, 9);
            label4.Name = "label4";
            label4.Size = new Size(242, 41);
            label4.TabIndex = 0;
            label4.Text = "ĐỔI MẬT KHẨU";
            // 
            // toolTipMain
            // 
            toolTipMain.AutoPopDelay = 5000;
            toolTipMain.InitialDelay = 300;
            toolTipMain.ReshowDelay = 100;
            toolTipMain.ShowAlways = true;
            // 
            // FrmChangePassword
            // 
            AcceptButton = btnSave;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(582, 373);
            Controls.Add(pnlHeader);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(txtConfirmPassword);
            Controls.Add(txtNewPassword);
            Controls.Add(txtOldPassword);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmChangePassword";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Cập nhật mật khẩu";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txtOldPassword;
        private TextBox txtNewPassword;
        private TextBox txtConfirmPassword;
        private Button btnSave;
        private Button btnCancel;
        private Panel pnlHeader;
        private Label label5;
        private Label label4;
        private ToolTip toolTipMain;
    }
}