namespace QuanLyCBCS.Forms.PersonnelForms
{
    partial class FrmPersonnelList
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
            txtSearch = new TextBox();
            btnAdd = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnRefresh = new Button();
            dgvPersonnel = new DataGridView();
            btnExport = new Button();
            lblTongTitle = new Label();
            lblChienSiTitle = new Label();
            lblCanBoTitle = new Label();
            lblDangVienTitle = new Label();
            pnlTongCBCS = new Panel();
            lblTongValue = new Label();
            pnlChienSi = new Panel();
            lblChienSiValue = new Label();
            pnlCanBo = new Panel();
            lblCanBoValue = new Label();
            pnlDangVien = new Panel();
            lblDangVienValue = new Label();
            btnBackup = new Button();
            btnRestore = new Button();
            lblCurrentUser = new Label();
            btnLogout = new Button();
            btnUsers = new Button();
            btnAuditLog = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            lblDonViDongNhat = new Label();
            lblTuoiTrungBinh = new Label();
            lblNu = new Label();
            lblNam = new Label();
            btnChangePassword = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvPersonnel).BeginInit();
            pnlTongCBCS.SuspendLayout();
            pnlChienSi.SuspendLayout();
            pnlCanBo.SuspendLayout();
            pnlDangVien.SuspendLayout();
            SuspendLayout();
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 11F);
            txtSearch.Location = new Point(12, 12);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "🔍 Tìm kiếm CBCS...";
            txtSearch.Size = new Size(273, 32);
            txtSearch.TabIndex = 0;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(12, 50);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(94, 29);
            btnAdd.TabIndex = 1;
            btnAdd.Text = "➕ Thêm";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnEdit
            // 
            btnEdit.Location = new Point(12, 85);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(94, 29);
            btnEdit.TabIndex = 2;
            btnEdit.Text = "✏️ Sửa";
            btnEdit.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(12, 120);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(94, 29);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "🗑️ Xóa";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.Location = new Point(1441, 123);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(101, 29);
            btnRefresh.TabIndex = 4;
            btnRefresh.Text = "⟳ Làm mới";
            btnRefresh.UseVisualStyleBackColor = true;
            // 
            // dgvPersonnel
            // 
            dgvPersonnel.AllowUserToAddRows = false;
            dgvPersonnel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvPersonnel.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPersonnel.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPersonnel.Location = new Point(1, 158);
            dgvPersonnel.MultiSelect = false;
            dgvPersonnel.Name = "dgvPersonnel";
            dgvPersonnel.ReadOnly = true;
            dgvPersonnel.RowHeadersVisible = false;
            dgvPersonnel.RowHeadersWidth = 51;
            dgvPersonnel.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPersonnel.Size = new Size(1552, 292);
            dgvPersonnel.TabIndex = 5;
            dgvPersonnel.CellClick += dgvPersonnel_CellClick;
            dgvPersonnel.CellDoubleClick += dgvPersonnel_CellDoubleClick;
            // 
            // btnExport
            // 
            btnExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExport.Location = new Point(1334, 123);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(101, 29);
            btnExport.TabIndex = 6;
            btnExport.Text = "📊 Excel";
            btnExport.UseVisualStyleBackColor = true;
            btnExport.Click += btnExport_Click;
            // 
            // lblTongTitle
            // 
            lblTongTitle.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            lblTongTitle.AutoSize = true;
            lblTongTitle.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            lblTongTitle.Location = new Point(29, 12);
            lblTongTitle.Name = "lblTongTitle";
            lblTongTitle.Size = new Size(104, 23);
            lblTongTitle.TabIndex = 7;
            lblTongTitle.Text = "Tổng CBCS:";
            // 
            // lblChienSiTitle
            // 
            lblChienSiTitle.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            lblChienSiTitle.AutoSize = true;
            lblChienSiTitle.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            lblChienSiTitle.Location = new Point(38, 12);
            lblChienSiTitle.Name = "lblChienSiTitle";
            lblChienSiTitle.Size = new Size(77, 23);
            lblChienSiTitle.TabIndex = 8;
            lblChienSiTitle.Text = "Chiến sĩ:";
            // 
            // lblCanBoTitle
            // 
            lblCanBoTitle.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            lblCanBoTitle.AutoSize = true;
            lblCanBoTitle.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            lblCanBoTitle.Location = new Point(51, 12);
            lblCanBoTitle.Name = "lblCanBoTitle";
            lblCanBoTitle.Size = new Size(71, 23);
            lblCanBoTitle.TabIndex = 9;
            lblCanBoTitle.Text = "Cán bộ:";
            // 
            // lblDangVienTitle
            // 
            lblDangVienTitle.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            lblDangVienTitle.AutoSize = true;
            lblDangVienTitle.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            lblDangVienTitle.Location = new Point(36, 12);
            lblDangVienTitle.Name = "lblDangVienTitle";
            lblDangVienTitle.Size = new Size(96, 23);
            lblDangVienTitle.TabIndex = 10;
            lblDangVienTitle.Text = "Đảng viên:";
            // 
            // pnlTongCBCS
            // 
            pnlTongCBCS.Anchor = AnchorStyles.Top;
            pnlTongCBCS.Controls.Add(lblTongValue);
            pnlTongCBCS.Controls.Add(lblTongTitle);
            pnlTongCBCS.Location = new Point(542, 12);
            pnlTongCBCS.Name = "pnlTongCBCS";
            pnlTongCBCS.Size = new Size(166, 114);
            pnlTongCBCS.TabIndex = 11;
            // 
            // lblTongValue
            // 
            lblTongValue.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            lblTongValue.AutoSize = true;
            lblTongValue.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTongValue.Location = new Point(65, 47);
            lblTongValue.Name = "lblTongValue";
            lblTongValue.Size = new Size(0, 41);
            lblTongValue.TabIndex = 8;
            // 
            // pnlChienSi
            // 
            pnlChienSi.Anchor = AnchorStyles.Top;
            pnlChienSi.Controls.Add(lblChienSiValue);
            pnlChienSi.Controls.Add(lblChienSiTitle);
            pnlChienSi.Location = new Point(731, 12);
            pnlChienSi.Name = "pnlChienSi";
            pnlChienSi.Size = new Size(166, 114);
            pnlChienSi.TabIndex = 12;
            // 
            // lblChienSiValue
            // 
            lblChienSiValue.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            lblChienSiValue.AutoSize = true;
            lblChienSiValue.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblChienSiValue.Location = new Point(61, 47);
            lblChienSiValue.Name = "lblChienSiValue";
            lblChienSiValue.Size = new Size(0, 41);
            lblChienSiValue.TabIndex = 9;
            // 
            // pnlCanBo
            // 
            pnlCanBo.Anchor = AnchorStyles.Top;
            pnlCanBo.Controls.Add(lblCanBoValue);
            pnlCanBo.Controls.Add(lblCanBoTitle);
            pnlCanBo.Location = new Point(918, 12);
            pnlCanBo.Name = "pnlCanBo";
            pnlCanBo.Size = new Size(166, 114);
            pnlCanBo.TabIndex = 12;
            // 
            // lblCanBoValue
            // 
            lblCanBoValue.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            lblCanBoValue.AutoSize = true;
            lblCanBoValue.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCanBoValue.Location = new Point(71, 47);
            lblCanBoValue.Name = "lblCanBoValue";
            lblCanBoValue.Size = new Size(0, 41);
            lblCanBoValue.TabIndex = 10;
            lblCanBoValue.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlDangVien
            // 
            pnlDangVien.Anchor = AnchorStyles.Top;
            pnlDangVien.Controls.Add(lblDangVienValue);
            pnlDangVien.Controls.Add(lblDangVienTitle);
            pnlDangVien.Location = new Point(1106, 12);
            pnlDangVien.Name = "pnlDangVien";
            pnlDangVien.Size = new Size(166, 114);
            pnlDangVien.TabIndex = 12;
            // 
            // lblDangVienValue
            // 
            lblDangVienValue.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            lblDangVienValue.AutoSize = true;
            lblDangVienValue.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDangVienValue.Location = new Point(67, 47);
            lblDangVienValue.Name = "lblDangVienValue";
            lblDangVienValue.Size = new Size(0, 41);
            lblDangVienValue.TabIndex = 11;
            // 
            // btnBackup
            // 
            btnBackup.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBackup.Location = new Point(1334, 88);
            btnBackup.Name = "btnBackup";
            btnBackup.Size = new Size(101, 29);
            btnBackup.TabIndex = 13;
            btnBackup.Text = "💽 Backup";
            btnBackup.UseVisualStyleBackColor = true;
            btnBackup.Click += btnBackup_Click;
            // 
            // btnRestore
            // 
            btnRestore.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRestore.Location = new Point(1441, 88);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new Size(101, 29);
            btnRestore.TabIndex = 14;
            btnRestore.Text = "🔄️ Restore";
            btnRestore.UseVisualStyleBackColor = true;
            btnRestore.Click += btnRestore_Click;
            // 
            // lblCurrentUser
            // 
            lblCurrentUser.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblCurrentUser.AutoSize = true;
            lblCurrentUser.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCurrentUser.Location = new Point(1334, 27);
            lblCurrentUser.Name = "lblCurrentUser";
            lblCurrentUser.Size = new Size(104, 20);
            lblCurrentUser.TabIndex = 15;
            lblCurrentUser.Text = "👤 username";
            // 
            // btnLogout
            // 
            btnLogout.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLogout.BackColor = Color.Red;
            btnLogout.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLogout.ForeColor = SystemColors.ButtonHighlight;
            btnLogout.Location = new Point(1480, 9);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(62, 52);
            btnLogout.TabIndex = 16;
            btnLogout.Text = "\u23fb";
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += btnLogout_Click;
            // 
            // btnUsers
            // 
            btnUsers.Location = new Point(112, 50);
            btnUsers.Name = "btnUsers";
            btnUsers.Size = new Size(133, 29);
            btnUsers.TabIndex = 17;
            btnUsers.Text = "👥 Tài khoản";
            btnUsers.UseVisualStyleBackColor = true;
            btnUsers.Click += btnUsers_Click;
            // 
            // btnAuditLog
            // 
            btnAuditLog.Location = new Point(112, 85);
            btnAuditLog.Name = "btnAuditLog";
            btnAuditLog.Size = new Size(133, 29);
            btnAuditLog.TabIndex = 18;
            btnAuditLog.Text = "📓 Nhật ký";
            btnAuditLog.UseVisualStyleBackColor = true;
            btnAuditLog.Click += btnAuditLog_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(329, 19);
            label1.Name = "label1";
            label1.Size = new Size(44, 20);
            label1.TabIndex = 19;
            label1.Text = "Nam:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(329, 54);
            label2.Name = "label2";
            label2.Size = new Size(32, 20);
            label2.TabIndex = 20;
            label2.Text = "Nữ:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(329, 85);
            label3.Name = "label3";
            label3.Size = new Size(62, 20);
            label3.TabIndex = 21;
            label3.Text = "Tuổi TB:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(329, 123);
            label4.Name = "label4";
            label4.Size = new Size(127, 20);
            label4.TabIndex = 22;
            label4.Text = "Đơn vị đông nhất:";
            // 
            // lblDonViDongNhat
            // 
            lblDonViDongNhat.AutoSize = true;
            lblDonViDongNhat.Location = new Point(477, 123);
            lblDonViDongNhat.Name = "lblDonViDongNhat";
            lblDonViDongNhat.Size = new Size(21, 20);
            lblDonViDongNhat.TabIndex = 27;
            lblDonViDongNhat.Text = "--";
            // 
            // lblTuoiTrungBinh
            // 
            lblTuoiTrungBinh.AutoSize = true;
            lblTuoiTrungBinh.Location = new Point(477, 85);
            lblTuoiTrungBinh.Name = "lblTuoiTrungBinh";
            lblTuoiTrungBinh.Size = new Size(17, 20);
            lblTuoiTrungBinh.TabIndex = 26;
            lblTuoiTrungBinh.Text = "0";
            // 
            // lblNu
            // 
            lblNu.AutoSize = true;
            lblNu.Location = new Point(477, 54);
            lblNu.Name = "lblNu";
            lblNu.Size = new Size(17, 20);
            lblNu.TabIndex = 25;
            lblNu.Text = "0";
            // 
            // lblNam
            // 
            lblNam.AutoSize = true;
            lblNam.Location = new Point(477, 19);
            lblNam.Name = "lblNam";
            lblNam.Size = new Size(17, 20);
            lblNam.TabIndex = 24;
            lblNam.Text = "0";
            // 
            // btnChangePassword
            // 
            btnChangePassword.Location = new Point(112, 120);
            btnChangePassword.Name = "btnChangePassword";
            btnChangePassword.Size = new Size(133, 29);
            btnChangePassword.TabIndex = 28;
            btnChangePassword.Text = "🔑 Đổi mật khẩu";
            btnChangePassword.UseVisualStyleBackColor = true;
            btnChangePassword.Click += btnChangePassword_Click;
            // 
            // FrmPersonnelList
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1554, 450);
            Controls.Add(btnChangePassword);
            Controls.Add(lblDonViDongNhat);
            Controls.Add(lblTuoiTrungBinh);
            Controls.Add(lblNu);
            Controls.Add(lblNam);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnAuditLog);
            Controls.Add(btnUsers);
            Controls.Add(btnLogout);
            Controls.Add(lblCurrentUser);
            Controls.Add(btnRestore);
            Controls.Add(btnBackup);
            Controls.Add(btnExport);
            Controls.Add(dgvPersonnel);
            Controls.Add(btnRefresh);
            Controls.Add(btnDelete);
            Controls.Add(btnEdit);
            Controls.Add(btnAdd);
            Controls.Add(txtSearch);
            Controls.Add(pnlTongCBCS);
            Controls.Add(pnlDangVien);
            Controls.Add(pnlCanBo);
            Controls.Add(pnlChienSi);
            Name = "FrmPersonnelList";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FrmPersonnelList";
            WindowState = FormWindowState.Maximized;
            Load += FrmPersonnelList_Load;
            ((System.ComponentModel.ISupportInitialize)dgvPersonnel).EndInit();
            pnlTongCBCS.ResumeLayout(false);
            pnlTongCBCS.PerformLayout();
            pnlChienSi.ResumeLayout(false);
            pnlChienSi.PerformLayout();
            pnlCanBo.ResumeLayout(false);
            pnlCanBo.PerformLayout();
            pnlDangVien.ResumeLayout(false);
            pnlDangVien.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtSearch;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;
        private DataGridView dgvPersonnel;
        private Button btnExport;
        private Label lblTongTitle;
        private Label lblChienSiTitle;
        private Label lblCanBoTitle;
        private Label lblDangVienTitle;
        private Panel pnlTongCBCS;
        private Panel pnlChienSi;
        private Panel pnlCanBo;
        private Panel pnlDangVien;
        private Label lblTongValue;
        private Label lblChienSiValue;
        private Label lblCanBoValue;
        private Label lblDangVienValue;
        private Button btnBackup;
        private Button btnRestore;
        private Label lblCurrentUser;
        private Button btnLogout;
        private Button btnUsers;
        private Button btnAuditLog;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label lblDonViDongNhat;
        private Label lblTuoiTrungBinh;
        private Label lblNu;
        private Label lblNam;
        private Button btnChangePassword;
    }
}