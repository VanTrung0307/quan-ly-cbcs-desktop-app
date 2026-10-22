namespace QuanLyCBCS.Forms.PersonnelForms
{
    partial class FrmAdminDashboard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAdminDashboard));
            grpPersonnel = new GroupBox();
            lblOfficerCount = new Label();
            lblSoldierCount = new Label();
            lblTotalPersonnel = new Label();
            grpUsers = new GroupBox();
            lblLockedUsers = new Label();
            lblUserCount = new Label();
            lblAdminCount = new Label();
            lblTotalUsers = new Label();
            grpMission = new GroupBox();
            lblFinishedMission = new Label();
            lblActiveMission = new Label();
            lblEndingMission = new Label();
            lblTotalMission = new Label();
            grpSecurity = new GroupBox();
            lblChangePasswordToday = new Label();
            lblUnlockToday = new Label();
            lblResetToday = new Label();
            lblFailedLoginToday = new Label();
            lblLoginToday = new Label();
            lblLockToday = new Label();
            btnRefresh = new Button();
            dgvRecentLogs = new DataGridView();
            grpPersonnel.SuspendLayout();
            grpUsers.SuspendLayout();
            grpMission.SuspendLayout();
            grpSecurity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRecentLogs).BeginInit();
            SuspendLayout();
            // 
            // grpPersonnel
            // 
            grpPersonnel.Controls.Add(lblOfficerCount);
            grpPersonnel.Controls.Add(lblSoldierCount);
            grpPersonnel.Controls.Add(lblTotalPersonnel);
            grpPersonnel.Location = new Point(84, 30);
            grpPersonnel.Name = "grpPersonnel";
            grpPersonnel.Size = new Size(250, 160);
            grpPersonnel.TabIndex = 0;
            grpPersonnel.TabStop = false;
            grpPersonnel.Text = "CBCS";
            // 
            // lblOfficerCount
            // 
            lblOfficerCount.AutoSize = true;
            lblOfficerCount.Location = new Point(25, 119);
            lblOfficerCount.Name = "lblOfficerCount";
            lblOfficerCount.Size = new Size(110, 20);
            lblOfficerCount.TabIndex = 2;
            lblOfficerCount.Text = "lblOfficerCount";
            // 
            // lblSoldierCount
            // 
            lblSoldierCount.AutoSize = true;
            lblSoldierCount.Location = new Point(25, 80);
            lblSoldierCount.Name = "lblSoldierCount";
            lblSoldierCount.Size = new Size(112, 20);
            lblSoldierCount.TabIndex = 1;
            lblSoldierCount.Text = "lblSoldierCount";
            // 
            // lblTotalPersonnel
            // 
            lblTotalPersonnel.AutoSize = true;
            lblTotalPersonnel.Location = new Point(25, 39);
            lblTotalPersonnel.Name = "lblTotalPersonnel";
            lblTotalPersonnel.Size = new Size(122, 20);
            lblTotalPersonnel.TabIndex = 0;
            lblTotalPersonnel.Text = "lblTotalPersonnel";
            // 
            // grpUsers
            // 
            grpUsers.Controls.Add(lblLockedUsers);
            grpUsers.Controls.Add(lblUserCount);
            grpUsers.Controls.Add(lblAdminCount);
            grpUsers.Controls.Add(lblTotalUsers);
            grpUsers.Location = new Point(387, 30);
            grpUsers.Name = "grpUsers";
            grpUsers.Size = new Size(250, 204);
            grpUsers.TabIndex = 1;
            grpUsers.TabStop = false;
            grpUsers.Text = "Người dùng";
            // 
            // lblLockedUsers
            // 
            lblLockedUsers.AutoSize = true;
            lblLockedUsers.Location = new Point(37, 161);
            lblLockedUsers.Name = "lblLockedUsers";
            lblLockedUsers.Size = new Size(108, 20);
            lblLockedUsers.TabIndex = 4;
            lblLockedUsers.Text = "lblLockedUsers";
            // 
            // lblUserCount
            // 
            lblUserCount.AutoSize = true;
            lblUserCount.Location = new Point(37, 119);
            lblUserCount.Name = "lblUserCount";
            lblUserCount.Size = new Size(94, 20);
            lblUserCount.TabIndex = 3;
            lblUserCount.Text = "lblUserCount";
            // 
            // lblAdminCount
            // 
            lblAdminCount.AutoSize = true;
            lblAdminCount.Location = new Point(37, 80);
            lblAdminCount.Name = "lblAdminCount";
            lblAdminCount.Size = new Size(109, 20);
            lblAdminCount.TabIndex = 2;
            lblAdminCount.Text = "lblAdminCount";
            // 
            // lblTotalUsers
            // 
            lblTotalUsers.AutoSize = true;
            lblTotalUsers.Location = new Point(37, 39);
            lblTotalUsers.Name = "lblTotalUsers";
            lblTotalUsers.Size = new Size(94, 20);
            lblTotalUsers.TabIndex = 1;
            lblTotalUsers.Text = "lblTotalUsers";
            // 
            // grpMission
            // 
            grpMission.Controls.Add(lblFinishedMission);
            grpMission.Controls.Add(lblActiveMission);
            grpMission.Controls.Add(lblEndingMission);
            grpMission.Controls.Add(lblTotalMission);
            grpMission.Location = new Point(695, 30);
            grpMission.Name = "grpMission";
            grpMission.Size = new Size(250, 204);
            grpMission.TabIndex = 1;
            grpMission.TabStop = false;
            grpMission.Text = "Công tác";
            // 
            // lblFinishedMission
            // 
            lblFinishedMission.AutoSize = true;
            lblFinishedMission.Location = new Point(25, 161);
            lblFinishedMission.Name = "lblFinishedMission";
            lblFinishedMission.Size = new Size(130, 20);
            lblFinishedMission.TabIndex = 8;
            lblFinishedMission.Text = "lblFinishedMission";
            // 
            // lblActiveMission
            // 
            lblActiveMission.AutoSize = true;
            lblActiveMission.Location = new Point(25, 80);
            lblActiveMission.Name = "lblActiveMission";
            lblActiveMission.Size = new Size(117, 20);
            lblActiveMission.TabIndex = 6;
            lblActiveMission.Text = "lblActiveMission";
            // 
            // lblEndingMission
            // 
            lblEndingMission.AutoSize = true;
            lblEndingMission.Location = new Point(25, 119);
            lblEndingMission.Name = "lblEndingMission";
            lblEndingMission.Size = new Size(122, 20);
            lblEndingMission.TabIndex = 7;
            lblEndingMission.Text = "lblEndingMission";
            // 
            // lblTotalMission
            // 
            lblTotalMission.AutoSize = true;
            lblTotalMission.Location = new Point(25, 39);
            lblTotalMission.Name = "lblTotalMission";
            lblTotalMission.Size = new Size(109, 20);
            lblTotalMission.TabIndex = 5;
            lblTotalMission.Text = "lblTotalMission";
            // 
            // grpSecurity
            // 
            grpSecurity.Controls.Add(lblChangePasswordToday);
            grpSecurity.Controls.Add(lblUnlockToday);
            grpSecurity.Controls.Add(lblResetToday);
            grpSecurity.Controls.Add(lblFailedLoginToday);
            grpSecurity.Controls.Add(lblLoginToday);
            grpSecurity.Controls.Add(lblLockToday);
            grpSecurity.Location = new Point(1018, 30);
            grpSecurity.Name = "grpSecurity";
            grpSecurity.Size = new Size(250, 283);
            grpSecurity.TabIndex = 1;
            grpSecurity.TabStop = false;
            grpSecurity.Text = "Bảo mật";
            // 
            // lblChangePasswordToday
            // 
            lblChangePasswordToday.AutoSize = true;
            lblChangePasswordToday.Location = new Point(37, 242);
            lblChangePasswordToday.Name = "lblChangePasswordToday";
            lblChangePasswordToday.Size = new Size(177, 20);
            lblChangePasswordToday.TabIndex = 14;
            lblChangePasswordToday.Text = "lblChangePasswordToday";
            // 
            // lblUnlockToday
            // 
            lblUnlockToday.AutoSize = true;
            lblUnlockToday.Location = new Point(37, 200);
            lblUnlockToday.Name = "lblUnlockToday";
            lblUnlockToday.Size = new Size(111, 20);
            lblUnlockToday.TabIndex = 13;
            lblUnlockToday.Text = "lblUnlockToday";
            // 
            // lblResetToday
            // 
            lblResetToday.AutoSize = true;
            lblResetToday.Location = new Point(37, 161);
            lblResetToday.Name = "lblResetToday";
            lblResetToday.Size = new Size(102, 20);
            lblResetToday.TabIndex = 12;
            lblResetToday.Text = "lblResetToday";
            // 
            // lblFailedLoginToday
            // 
            lblFailedLoginToday.AutoSize = true;
            lblFailedLoginToday.Location = new Point(37, 80);
            lblFailedLoginToday.Name = "lblFailedLoginToday";
            lblFailedLoginToday.Size = new Size(142, 20);
            lblFailedLoginToday.TabIndex = 10;
            lblFailedLoginToday.Text = "lblFailedLoginToday";
            // 
            // lblLoginToday
            // 
            lblLoginToday.AutoSize = true;
            lblLoginToday.Location = new Point(37, 39);
            lblLoginToday.Name = "lblLoginToday";
            lblLoginToday.Size = new Size(103, 20);
            lblLoginToday.TabIndex = 9;
            lblLoginToday.Text = "lblLoginToday";
            // 
            // lblLockToday
            // 
            lblLockToday.AutoSize = true;
            lblLockToday.Location = new Point(37, 119);
            lblLockToday.Name = "lblLockToday";
            lblLockToday.Size = new Size(96, 20);
            lblLockToday.TabIndex = 11;
            lblLockToday.Text = "lblLockToday";
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.FromArgb(52, 73, 94);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(858, 272);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(120, 36);
            btnRefresh.TabIndex = 2;
            btnRefresh.Text = "⟳ Làm mới";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // dgvRecentLogs
            // 
            dgvRecentLogs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRecentLogs.Location = new Point(43, 319);
            dgvRecentLogs.Name = "dgvRecentLogs";
            dgvRecentLogs.RowHeadersWidth = 51;
            dgvRecentLogs.Size = new Size(935, 365);
            dgvRecentLogs.TabIndex = 3;
            // 
            // FrmAdminDashboard
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1335, 696);
            Controls.Add(dgvRecentLogs);
            Controls.Add(btnRefresh);
            Controls.Add(grpSecurity);
            Controls.Add(grpMission);
            Controls.Add(grpUsers);
            Controls.Add(grpPersonnel);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FrmAdminDashboard";
            Text = "Bảng điều khiển quản trị";
            Load += FrmAdminDashboard_Load;
            grpPersonnel.ResumeLayout(false);
            grpPersonnel.PerformLayout();
            grpUsers.ResumeLayout(false);
            grpUsers.PerformLayout();
            grpMission.ResumeLayout(false);
            grpMission.PerformLayout();
            grpSecurity.ResumeLayout(false);
            grpSecurity.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRecentLogs).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox grpPersonnel;
        private Label lblOfficerCount;
        private Label lblSoldierCount;
        private Label lblTotalPersonnel;
        private GroupBox grpUsers;
        private GroupBox grpMission;
        private GroupBox grpSecurity;
        private Label lblLockedUsers;
        private Label lblUserCount;
        private Label lblAdminCount;
        private Label lblTotalUsers;
        private Label lblFinishedMission;
        private Label lblActiveMission;
        private Label lblEndingMission;
        private Label lblTotalMission;
        private Label lblResetToday;
        private Label lblFailedLoginToday;
        private Label lblLoginToday;
        private Label lblLockToday;
        private Button btnRefresh;
        private DataGridView dgvRecentLogs;
        private Label lblUnlockToday;
        private Label lblChangePasswordToday;
    }
}