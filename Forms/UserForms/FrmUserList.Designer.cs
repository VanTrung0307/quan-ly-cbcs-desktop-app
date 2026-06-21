namespace QuanLyCBCS.Forms.UserForms
{
    partial class FrmUserList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmUserList));
            dgvUsers = new DataGridView();
            pnlHeader = new Panel();
            label2 = new Label();
            label1 = new Label();
            pnlToolbar = new Panel();
            btnUnlock = new Button();
            btnRefresh = new Button();
            btnResetPassword = new Button();
            btnDeleteUser = new Button();
            btnAddUser = new Button();
            statusStrip1 = new StatusStrip();
            lblUserCount = new ToolStripStatusLabel();
            toolTipMain = new ToolTip(components);
            ((System.ComponentModel.ISupportInitialize)dgvUsers).BeginInit();
            pnlHeader.SuspendLayout();
            pnlToolbar.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // dgvUsers
            // 
            dgvUsers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsers.Location = new Point(2, 137);
            dgvUsers.Name = "dgvUsers";
            dgvUsers.RowHeadersWidth = 51;
            dgvUsers.Size = new Size(878, 463);
            dgvUsers.TabIndex = 2;
            dgvUsers.CellFormatting += dgvUsers_CellFormatting;
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(47, 67, 90);
            pnlHeader.Controls.Add(label2);
            pnlHeader.Controls.Add(label1);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(882, 80);
            pnlHeader.TabIndex = 7;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.WhiteSmoke;
            label2.Location = new Point(341, 50);
            label2.Name = "label2";
            label2.Size = new Size(206, 20);
            label2.TabIndex = 1;
            label2.Text = "Quản trị người dùng hệ thống";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(284, 9);
            label1.Name = "label1";
            label1.Size = new Size(321, 41);
            label1.TabIndex = 0;
            label1.Text = "QUẢN LÝ TÀI KHOẢN";
            label1.TextAlign = ContentAlignment.TopCenter;
            // 
            // pnlToolbar
            // 
            pnlToolbar.Controls.Add(btnUnlock);
            pnlToolbar.Controls.Add(btnRefresh);
            pnlToolbar.Controls.Add(btnResetPassword);
            pnlToolbar.Controls.Add(btnDeleteUser);
            pnlToolbar.Controls.Add(btnAddUser);
            pnlToolbar.Dock = DockStyle.Top;
            pnlToolbar.Location = new Point(0, 80);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Size = new Size(882, 55);
            pnlToolbar.TabIndex = 8;
            // 
            // btnUnlock
            // 
            btnUnlock.BackColor = Color.MediumVioletRed;
            btnUnlock.ForeColor = Color.White;
            btnUnlock.Location = new Point(564, 6);
            btnUnlock.Name = "btnUnlock";
            btnUnlock.Size = new Size(101, 36);
            btnUnlock.TabIndex = 13;
            btnUnlock.Text = "🔓 Mở khoá";
            btnUnlock.UseVisualStyleBackColor = false;
            btnUnlock.Click += btnUnlock_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.FromArgb(52, 152, 219);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(438, 6);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(120, 36);
            btnRefresh.TabIndex = 9;
            btnRefresh.Text = "🔁 Làm mới";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnResetPassword
            // 
            btnResetPassword.BackColor = Color.FromArgb(243, 156, 18);
            btnResetPassword.ForeColor = Color.White;
            btnResetPassword.Location = new Point(297, 6);
            btnResetPassword.Name = "btnResetPassword";
            btnResetPassword.Size = new Size(135, 36);
            btnResetPassword.TabIndex = 10;
            btnResetPassword.Text = "↺ Reset Password";
            btnResetPassword.UseVisualStyleBackColor = false;
            btnResetPassword.Click += btnResetPassword_Click;
            // 
            // btnDeleteUser
            // 
            btnDeleteUser.BackColor = Color.FromArgb(231, 76, 60);
            btnDeleteUser.ForeColor = Color.White;
            btnDeleteUser.Location = new Point(171, 6);
            btnDeleteUser.Name = "btnDeleteUser";
            btnDeleteUser.Size = new Size(120, 36);
            btnDeleteUser.TabIndex = 11;
            btnDeleteUser.Text = "🗑️ Xoá";
            btnDeleteUser.UseVisualStyleBackColor = false;
            btnDeleteUser.Click += btnDeleteUser_Click;
            // 
            // btnAddUser
            // 
            btnAddUser.BackColor = Color.FromArgb(39, 174, 96);
            btnAddUser.ForeColor = Color.White;
            btnAddUser.Location = new Point(12, 6);
            btnAddUser.Name = "btnAddUser";
            btnAddUser.Size = new Size(153, 36);
            btnAddUser.TabIndex = 12;
            btnAddUser.Text = "➕ Thêm tài khoản";
            btnAddUser.UseVisualStyleBackColor = false;
            btnAddUser.Click += btnAddUser_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { lblUserCount });
            statusStrip1.Location = new Point(0, 577);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(882, 26);
            statusStrip1.TabIndex = 9;
            statusStrip1.Text = "statusStrip1";
            // 
            // lblUserCount
            // 
            lblUserCount.Name = "lblUserCount";
            lblUserCount.Size = new Size(151, 20);
            lblUserCount.Text = "toolStripStatusLabel1";
            // 
            // toolTipMain
            // 
            toolTipMain.AutoPopDelay = 5000;
            toolTipMain.InitialDelay = 300;
            toolTipMain.ReshowDelay = 100;
            toolTipMain.ShowAlways = true;
            // 
            // FrmUserList
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(882, 603);
            Controls.Add(statusStrip1);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlHeader);
            Controls.Add(dgvUsers);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmUserList";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Quản lý tài khoản";
            Load += FrmUserList_Load;
            ((System.ComponentModel.ISupportInitialize)dgvUsers).EndInit();
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlToolbar.ResumeLayout(false);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private DataGridView dgvUsers;
        private Panel pnlHeader;
        private Label label1;
        private Label label2;
        private Panel pnlToolbar;
        private Button btnAddUser;
        private Button btnRefresh;
        private Button btnResetPassword;
        private Button btnDeleteUser;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lblUserCount;
        private ToolTip toolTipMain;
        private Button btnUnlock;
    }
}