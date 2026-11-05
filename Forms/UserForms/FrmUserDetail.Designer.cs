using System.Drawing.Drawing2D;

namespace QuanLyCBCS.Forms.UserForms
{
    partial class FrmUserDetail
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
            panelHeader = new Panel();
            lblHeaderIcon = new Label();
            lblHeaderGear = new Label();
            lblHeaderText = new Label();
            btnClose = new Button();
            lblTitle = new Label();
            lblSubTitle = new Label();
            panelTitleLineLeft = new Panel();
            panelTitleLineRight = new Panel();
            rowUsername = new Panel();
            lblUsernameIcon = new Label();
            label1 = new Label();
            txtUsername = new TextBox();
            pnlUsernameInput = new Panel();
            lblUsernameRequired = new Label();
            rowPassword = new Panel();
            lblPasswordIcon = new Label();
            label2 = new Label();
            txtPassword = new TextBox();
            btnTogglePassword = new Button();
            pnlPasswordInput = new Panel();
            lblPasswordRequired = new Label();
            rowRole = new Panel();
            lblRoleIcon = new Label();
            label3 = new Label();
            cboRole = new ComboBox();
            pnlRoleInput = new Panel();
            lblRoleRequired = new Label();
            panelFooter = new Panel();
            btnSave = new Button();
            btnCancel = new Button();
            toolTipMain = new ToolTip(components);
            panelHeader.SuspendLayout();
            rowUsername.SuspendLayout();
            pnlUsernameInput.SuspendLayout();
            rowPassword.SuspendLayout();
            pnlPasswordInput.SuspendLayout();
            rowRole.SuspendLayout();
            pnlRoleInput.SuspendLayout();
            panelFooter.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(0, 78, 164);
            panelHeader.Controls.Add(lblHeaderIcon);
            panelHeader.Controls.Add(lblHeaderGear);
            panelHeader.Controls.Add(lblHeaderText);
            panelHeader.Controls.Add(btnClose);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(620, 64);
            panelHeader.TabIndex = 0;
            panelHeader.Paint += panelHeader_Paint;
            // 
            // lblHeaderIcon
            // 
            lblHeaderIcon.BackColor = Color.Transparent;
            lblHeaderIcon.Font = new Font("Segoe MDL2 Assets", 27F, FontStyle.Regular);
            lblHeaderIcon.ForeColor = Color.White;
            lblHeaderIcon.Location = new Point(14, 4);
            lblHeaderIcon.Name = "lblHeaderIcon";
            lblHeaderIcon.Size = new Size(54, 55);
            lblHeaderIcon.TabIndex = 0;
            lblHeaderIcon.Text = "\uE77B";
            lblHeaderIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblHeaderGear
            // 
            lblHeaderGear.BackColor = Color.Transparent;
            lblHeaderGear.Font = new Font("Segoe MDL2 Assets", 12F, FontStyle.Regular);
            lblHeaderGear.ForeColor = Color.White;
            lblHeaderGear.Location = new Point(48, 36);
            lblHeaderGear.Name = "lblHeaderGear";
            lblHeaderGear.Size = new Size(22, 22);
            lblHeaderGear.TabIndex = 1;
            lblHeaderGear.Text = "\uE713";
            lblHeaderGear.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblHeaderText
            // 
            lblHeaderText.BackColor = Color.Transparent;
            lblHeaderText.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblHeaderText.ForeColor = Color.White;
            lblHeaderText.Location = new Point(82, 7);
            lblHeaderText.Name = "lblHeaderText";
            lblHeaderText.Size = new Size(430, 50);
            lblHeaderText.TabIndex = 2;
            lblHeaderText.Text = "Thông tin tài khoản";
            lblHeaderText.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.BackColor = Color.Transparent;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 62, 130);
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 91, 180);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 22F, FontStyle.Regular);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(566, 4);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(48, 55);
            btnClose.TabIndex = 3;
            btnClose.Text = "×";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnCancel_Click;
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Segoe UI", 21F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(0, 54, 112);
            lblTitle.Location = new Point(155, 72);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(310, 43);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "QUẢN LÝ TÀI KHOẢN";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSubTitle
            // 
            lblSubTitle.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular);
            lblSubTitle.ForeColor = Color.FromArgb(95, 105, 118);
            lblSubTitle.Location = new Point(145, 111);
            lblSubTitle.Name = "lblSubTitle";
            lblSubTitle.Size = new Size(330, 25);
            lblSubTitle.TabIndex = 2;
            lblSubTitle.Text = "Thêm hoặc chỉnh sửa tài khoản";
            lblSubTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelTitleLineLeft
            // 
            panelTitleLineLeft.BackColor = Color.FromArgb(0, 117, 255);
            panelTitleLineLeft.Location = new Point(30, 94);
            panelTitleLineLeft.Name = "panelTitleLineLeft";
            panelTitleLineLeft.Size = new Size(112, 2);
            panelTitleLineLeft.TabIndex = 3;
            // 
            // panelTitleLineRight
            // 
            panelTitleLineRight.BackColor = Color.FromArgb(0, 117, 255);
            panelTitleLineRight.Location = new Point(478, 94);
            panelTitleLineRight.Name = "panelTitleLineRight";
            panelTitleLineRight.Size = new Size(112, 2);
            panelTitleLineRight.TabIndex = 4;
            // 
            // rowUsername
            // 
            rowUsername.BackColor = Color.FromArgb(231, 242, 255);
            rowUsername.Controls.Add(lblUsernameIcon);
            rowUsername.Controls.Add(label1);
            rowUsername.Controls.Add(pnlUsernameInput);
            rowUsername.Controls.Add(lblUsernameRequired);
            rowUsername.Location = new Point(26, 151);
            rowUsername.Name = "rowUsername";
            rowUsername.Size = new Size(568, 46);
            rowUsername.TabIndex = 5;
            // 
            // lblUsernameIcon
            // 
            lblUsernameIcon.Font = new Font("Segoe MDL2 Assets", 20F);
            lblUsernameIcon.ForeColor = Color.FromArgb(0, 91, 190);
            lblUsernameIcon.Location = new Point(10, 0);
            lblUsernameIcon.Name = "lblUsernameIcon";
            lblUsernameIcon.Size = new Size(48, 46);
            lblUsernameIcon.TabIndex = 0;
            lblUsernameIcon.Text = "\uE77B";
            lblUsernameIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(0, 48, 103);
            label1.Location = new Point(67, 2);
            label1.Name = "label1";
            label1.Size = new Size(158, 42);
            label1.TabIndex = 1;
            label1.Text = "Tên đăng nhập";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtUsername
            // 
            txtUsername.BorderStyle = BorderStyle.None;
            txtUsername.Font = new Font("Segoe UI", 12F);
            txtUsername.Location = new Point(5, 1);
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderText = "Nhập tên đăng nhập";
            txtUsername.Size = new Size(311, 29);
            txtUsername.TabIndex = 3;
            //
            // pnlUsernameInput
            //
            pnlUsernameInput.BackColor = Color.White;
            pnlUsernameInput.Controls.Add(txtUsername);
            pnlUsernameInput.Location = new Point(239, 7);
            pnlUsernameInput.Name = "pnlUsernameInput";
            pnlUsernameInput.Size = new Size(321, 32);
            pnlUsernameInput.TabIndex = 2;
            pnlUsernameInput.Paint += inputPanel_Paint;
            //
            // lblUsernameRequired
            //
            lblUsernameRequired.AutoSize = true;
            lblUsernameRequired.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
            lblUsernameRequired.ForeColor = Color.FromArgb(230, 55, 55);
            lblUsernameRequired.Location = new Point(220, 12);
            lblUsernameRequired.Name = "lblUsernameRequired";
            lblUsernameRequired.Size = new Size(14, 21);
            lblUsernameRequired.TabIndex = 3;
            lblUsernameRequired.Text = "*";
            //
            // rowPassword
            // 
            rowPassword.BackColor = Color.FromArgb(231, 242, 255);
            rowPassword.Controls.Add(lblPasswordIcon);
            rowPassword.Controls.Add(label2);
            rowPassword.Controls.Add(pnlPasswordInput);
            rowPassword.Controls.Add(lblPasswordRequired);
            rowPassword.Location = new Point(26, 205);
            rowPassword.Name = "rowPassword";
            rowPassword.Size = new Size(568, 46);
            rowPassword.TabIndex = 6;
            // 
            // lblPasswordIcon
            // 
            lblPasswordIcon.Font = new Font("Segoe MDL2 Assets", 20F);
            lblPasswordIcon.ForeColor = Color.FromArgb(0, 91, 190);
            lblPasswordIcon.Location = new Point(10, 0);
            lblPasswordIcon.Name = "lblPasswordIcon";
            lblPasswordIcon.Size = new Size(48, 46);
            lblPasswordIcon.TabIndex = 0;
            lblPasswordIcon.Text = "\uE72E";
            lblPasswordIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
            label2.ForeColor = Color.FromArgb(0, 48, 103);
            label2.Location = new Point(67, 2);
            label2.Name = "label2";
            label2.Size = new Size(158, 42);
            label2.TabIndex = 1;
            label2.Text = "Mật khẩu";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtPassword
            // 
            txtPassword.BorderStyle = BorderStyle.None;
            txtPassword.Font = new Font("Segoe UI", 12F);
            txtPassword.Location = new Point(5, 1);
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = "Nhập mật khẩu";
            txtPassword.Size = new Size(278, 29);
            txtPassword.TabIndex = 4;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // btnTogglePassword
            // 
            btnTogglePassword.BackColor = Color.White;
            btnTogglePassword.FlatAppearance.BorderSize = 0;
            btnTogglePassword.FlatStyle = FlatStyle.Flat;
            btnTogglePassword.Font = new Font("Segoe MDL2 Assets", 14F);
            btnTogglePassword.ForeColor = Color.FromArgb(70, 78, 88);
            btnTogglePassword.Location = new Point(285, 2);
            btnTogglePassword.Name = "btnTogglePassword";
            btnTogglePassword.Size = new Size(30, 27);
            btnTogglePassword.TabIndex = 5;
            btnTogglePassword.Text = "\uE890";
            btnTogglePassword.UseVisualStyleBackColor = false;
            btnTogglePassword.Click += btnTogglePassword_Click;
            //
            // pnlPasswordInput
            //
            pnlPasswordInput.BackColor = Color.White;
            pnlPasswordInput.Controls.Add(txtPassword);
            pnlPasswordInput.Controls.Add(btnTogglePassword);
            pnlPasswordInput.Location = new Point(239, 7);
            pnlPasswordInput.Name = "pnlPasswordInput";
            pnlPasswordInput.Size = new Size(321, 32);
            pnlPasswordInput.TabIndex = 2;
            pnlPasswordInput.Paint += inputPanel_Paint;
            //
            // lblPasswordRequired
            //
            lblPasswordRequired.AutoSize = true;
            lblPasswordRequired.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
            lblPasswordRequired.ForeColor = Color.FromArgb(230, 55, 55);
            lblPasswordRequired.Location = new Point(220, 12);
            lblPasswordRequired.Name = "lblPasswordRequired";
            lblPasswordRequired.Size = new Size(14, 21);
            lblPasswordRequired.TabIndex = 3;
            lblPasswordRequired.Text = "*";
            //
            // rowRole
            // 
            rowRole.BackColor = Color.FromArgb(231, 242, 255);
            rowRole.Controls.Add(lblRoleIcon);
            rowRole.Controls.Add(label3);
            rowRole.Controls.Add(pnlRoleInput);
            rowRole.Controls.Add(lblRoleRequired);
            rowRole.Location = new Point(26, 259);
            rowRole.Name = "rowRole";
            rowRole.Size = new Size(568, 46);
            rowRole.TabIndex = 7;
            // 
            // lblRoleIcon
            // 
            lblRoleIcon.Font = new Font("Segoe MDL2 Assets", 20F);
            lblRoleIcon.ForeColor = Color.FromArgb(0, 91, 190);
            lblRoleIcon.Location = new Point(10, 0);
            lblRoleIcon.Name = "lblRoleIcon";
            lblRoleIcon.Size = new Size(48, 46);
            lblRoleIcon.TabIndex = 0;
            lblRoleIcon.Text = "\uE8D7";
            lblRoleIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            label3.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
            label3.ForeColor = Color.FromArgb(0, 48, 103);
            label3.Location = new Point(67, 2);
            label3.Name = "label3";
            label3.Size = new Size(158, 42);
            label3.TabIndex = 1;
            label3.Text = "Vai trò";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboRole
            // 
            cboRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cboRole.Font = new Font("Segoe UI", 12F);
            cboRole.FormattingEnabled = true;
            cboRole.Location = new Point(5, 2);
            cboRole.Name = "cboRole";
            cboRole.Size = new Size(311, 28);
            cboRole.TabIndex = 6;
            //
            // pnlRoleInput
            //
            pnlRoleInput.BackColor = Color.White;
            pnlRoleInput.Controls.Add(cboRole);
            pnlRoleInput.Location = new Point(239, 7);
            pnlRoleInput.Name = "pnlRoleInput";
            pnlRoleInput.Size = new Size(321, 32);
            pnlRoleInput.TabIndex = 2;
            pnlRoleInput.Paint += inputPanel_Paint;
            //
            // lblRoleRequired
            //
            lblRoleRequired.AutoSize = true;
            lblRoleRequired.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
            lblRoleRequired.ForeColor = Color.FromArgb(230, 55, 55);
            lblRoleRequired.Location = new Point(220, 12);
            lblRoleRequired.Name = "lblRoleRequired";
            lblRoleRequired.Size = new Size(14, 21);
            lblRoleRequired.TabIndex = 3;
            lblRoleRequired.Text = "*";
            //
            // panelFooter
            // 
            panelFooter.BackColor = Color.FromArgb(235, 244, 255);
            panelFooter.Controls.Add(btnSave);
            panelFooter.Controls.Add(btnCancel);
            panelFooter.Dock = DockStyle.Bottom;
            panelFooter.Location = new Point(0, 327);
            panelFooter.Name = "panelFooter";
            panelFooter.Size = new Size(620, 71);
            panelFooter.TabIndex = 8;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(20, 174, 91);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatAppearance.MouseDownBackColor = Color.FromArgb(12, 145, 73);
            btnSave.FlatAppearance.MouseOverBackColor = Color.FromArgb(15, 158, 82);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(137, 10);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(150, 47);
            btnSave.TabIndex = 7;
            btnSave.Text = "💾  Lưu";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(255, 250, 250);
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(239, 65, 65);
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatAppearance.MouseDownBackColor = Color.FromArgb(255, 230, 230);
            btnCancel.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 242, 242);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnCancel.ForeColor = Color.FromArgb(235, 55, 55);
            btnCancel.Location = new Point(311, 10);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(150, 47);
            btnCancel.TabIndex = 8;
            btnCancel.Text = "✕  Hủy";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            btnCancel.Paint += btnCancel_Paint;
            // 
            // FrmUserDetail
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(620, 398);
            Controls.Add(panelFooter);
            Controls.Add(rowRole);
            Controls.Add(rowPassword);
            Controls.Add(rowUsername);
            Controls.Add(panelTitleLineRight);
            Controls.Add(panelTitleLineLeft);
            Controls.Add(lblSubTitle);
            Controls.Add(lblTitle);
            Controls.Add(panelHeader);
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmUserDetail";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Thông tin tài khoản";
            panelHeader.ResumeLayout(false);
            rowUsername.ResumeLayout(false);
            rowUsername.PerformLayout();
            pnlUsernameInput.ResumeLayout(false);
            pnlUsernameInput.PerformLayout();
            rowPassword.ResumeLayout(false);
            rowPassword.PerformLayout();
            pnlPasswordInput.ResumeLayout(false);
            pnlPasswordInput.PerformLayout();
            rowRole.ResumeLayout(false);
            pnlRoleInput.ResumeLayout(false);
            pnlRoleInput.PerformLayout();
            panelFooter.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelHeader;
        private Label lblHeaderIcon;
        private Label lblHeaderGear;
        private Label lblHeaderText;
        private Button btnClose;
        private Label lblTitle;
        private Label lblSubTitle;
        private Panel panelTitleLineLeft;
        private Panel panelTitleLineRight;
        private Panel rowUsername;
        private Label lblUsernameIcon;
        private Label label1;
        private TextBox txtUsername;
        private Panel pnlUsernameInput;
        private Label lblUsernameRequired;
        private Panel rowPassword;
        private Label lblPasswordIcon;
        private Label label2;
        private TextBox txtPassword;
        private Button btnTogglePassword;
        private Panel pnlPasswordInput;
        private Label lblPasswordRequired;
        private Panel rowRole;
        private Label lblRoleIcon;
        private Label label3;
        private ComboBox cboRole;
        private Panel pnlRoleInput;
        private Label lblRoleRequired;
        private Panel panelFooter;
        private Button btnSave;
        private Button btnCancel;
        private ToolTip toolTipMain;
    }
}
