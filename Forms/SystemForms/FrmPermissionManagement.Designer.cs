namespace QuanLyCBCS.Forms.SystemForms
{
    partial class FrmPermissionManagement
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPermissionManagement));
            lblRole = new Label();
            cboRole = new ComboBox();
            clbPermission = new CheckedListBox();
            btnSave = new Button();
            btnRefresh = new Button();
            btnClose = new Button();
            btnRedo = new Button();
            btnUndo = new Button();
            SuspendLayout();
            // 
            // lblRole
            // 
            lblRole.AutoSize = true;
            lblRole.Location = new Point(49, 39);
            lblRole.Name = "lblRole";
            lblRole.Size = new Size(42, 20);
            lblRole.TabIndex = 0;
            lblRole.Text = "Role:";
            // 
            // cboRole
            // 
            cboRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cboRole.FormattingEnabled = true;
            cboRole.Location = new Point(49, 62);
            cboRole.Name = "cboRole";
            cboRole.Size = new Size(250, 28);
            cboRole.TabIndex = 1;
            // 
            // clbPermission
            // 
            clbPermission.CheckOnClick = true;
            clbPermission.FormattingEnabled = true;
            clbPermission.Location = new Point(49, 119);
            clbPermission.Name = "clbPermission";
            clbPermission.Size = new Size(760, 378);
            clbPermission.TabIndex = 2;
            clbPermission.ItemCheck += clbPermission_ItemCheck;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(515, 512);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(94, 29);
            btnSave.TabIndex = 3;
            btnSave.Text = "Lưu";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(615, 512);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(94, 29);
            btnRefresh.TabIndex = 4;
            btnRefresh.Text = "Làm mới";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnClose
            // 
            btnClose.Location = new Point(715, 512);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(94, 29);
            btnClose.TabIndex = 5;
            btnClose.Text = "Đóng";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // btnRedo
            // 
            btnRedo.Location = new Point(715, 84);
            btnRedo.Name = "btnRedo";
            btnRedo.Size = new Size(94, 29);
            btnRedo.TabIndex = 6;
            btnRedo.Text = "↷ Redo";
            btnRedo.UseVisualStyleBackColor = true;
            btnRedo.Click += btnRedo_Click;
            // 
            // btnUndo
            // 
            btnUndo.Location = new Point(615, 84);
            btnUndo.Name = "btnUndo";
            btnUndo.Size = new Size(94, 29);
            btnUndo.TabIndex = 7;
            btnUndo.Text = "↶ Undo";
            btnUndo.UseVisualStyleBackColor = true;
            btnUndo.Click += btnUndo_Click;
            // 
            // FrmPermissionManagement
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(882, 553);
            Controls.Add(btnUndo);
            Controls.Add(btnRedo);
            Controls.Add(btnClose);
            Controls.Add(btnRefresh);
            Controls.Add(btnSave);
            Controls.Add(clbPermission);
            Controls.Add(cboRole);
            Controls.Add(lblRole);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmPermissionManagement";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Quản lý phân quyền";
            Load += FrmPermissionManagement_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblRole;
        private ComboBox cboRole;
        private CheckedListBox clbPermission;
        private Button btnSave;
        private Button btnRefresh;
        private Button btnClose;
        private Button btnRedo;
        private Button btnUndo;
    }
}