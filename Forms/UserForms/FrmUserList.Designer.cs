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
            label1 = new Label();
            btnAddUser = new Button();
            dgvUsers = new DataGridView();
            btnDeleteUser = new Button();
            btnResetPassword = new Button();
            btnRefresh = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvUsers).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(50, 34);
            label1.Name = "label1";
            label1.Size = new Size(369, 41);
            label1.TabIndex = 0;
            label1.Text = "DANH SÁCH TÀI KHOẢN";
            // 
            // btnAddUser
            // 
            btnAddUser.Location = new Point(12, 93);
            btnAddUser.Name = "btnAddUser";
            btnAddUser.Size = new Size(94, 29);
            btnAddUser.TabIndex = 1;
            btnAddUser.Text = "➕ Thêm";
            btnAddUser.UseVisualStyleBackColor = true;
            btnAddUser.Click += btnAddUser_Click;
            // 
            // dgvUsers
            // 
            dgvUsers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsers.Location = new Point(2, 128);
            dgvUsers.Name = "dgvUsers";
            dgvUsers.RowHeadersWidth = 51;
            dgvUsers.Size = new Size(507, 319);
            dgvUsers.TabIndex = 2;
            // 
            // btnDeleteUser
            // 
            btnDeleteUser.Location = new Point(112, 93);
            btnDeleteUser.Name = "btnDeleteUser";
            btnDeleteUser.Size = new Size(94, 29);
            btnDeleteUser.TabIndex = 3;
            btnDeleteUser.Text = "🗑️ Xoá";
            btnDeleteUser.UseVisualStyleBackColor = true;
            btnDeleteUser.Click += btnDeleteUser_Click;
            // 
            // btnResetPassword
            // 
            btnResetPassword.Location = new Point(212, 93);
            btnResetPassword.Name = "btnResetPassword";
            btnResetPassword.Size = new Size(157, 29);
            btnResetPassword.TabIndex = 4;
            btnResetPassword.Text = "🔄️ Reset Password";
            btnResetPassword.UseVisualStyleBackColor = true;
            btnResetPassword.Click += btnResetPassword_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(375, 93);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(111, 29);
            btnRefresh.TabIndex = 5;
            btnRefresh.Text = "🔁 Làm mới";
            btnRefresh.UseVisualStyleBackColor = true;
            // 
            // FrmUserList
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(511, 450);
            Controls.Add(btnRefresh);
            Controls.Add(btnResetPassword);
            Controls.Add(btnDeleteUser);
            Controls.Add(dgvUsers);
            Controls.Add(btnAddUser);
            Controls.Add(label1);
            Name = "FrmUserList";
            Text = "FrmUserList";
            Load += FrmUserList_Load;
            ((System.ComponentModel.ISupportInitialize)dgvUsers).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button btnAddUser;
        private DataGridView dgvUsers;
        private Button btnDeleteUser;
        private Button btnResetPassword;
        private Button btnRefresh;
    }
}