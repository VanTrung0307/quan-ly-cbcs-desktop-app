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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPersonnelList));
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
            pnlThongKe = new Panel();
            lblThongke = new Label();
            pnlSearch = new Panel();
            statusStrip1 = new StatusStrip();
            lblUser = new ToolStripStatusLabel();
            lblRole = new ToolStripStatusLabel();
            lblTime = new ToolStripStatusLabel();
            lblRecordCount = new ToolStripStatusLabel();
            cboDonVi = new ComboBox();
            cboCapBac = new ComboBox();
            cboChucVu = new ComboBox();
            toolTip1 = new ToolTip(components);
            btnMission = new Button();
            btnStatistics = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvPersonnel).BeginInit();
            pnlTongCBCS.SuspendLayout();
            pnlChienSi.SuspendLayout();
            pnlCanBo.SuspendLayout();
            pnlDangVien.SuspendLayout();
            pnlThongKe.SuspendLayout();
            pnlSearch.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // txtSearch
            // 
            txtSearch.BorderStyle = BorderStyle.None;
            txtSearch.Font = new Font("Segoe UI", 11F);
            txtSearch.ForeColor = Color.FromArgb(40, 40, 40);
            txtSearch.Location = new Point(12, 6);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "🔍 Tìm theo họ tên, số hiệu, CCCD, đơn vị...";
            txtSearch.Size = new Size(489, 25);
            txtSearch.TabIndex = 0;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // btnAdd
            // 
            btnAdd.Font = new Font("Segoe UI", 10F);
            btnAdd.Location = new Point(12, 59);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(164, 36);
            btnAdd.TabIndex = 1;
            btnAdd.Text = "➕ Thêm";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnEdit
            // 
            btnEdit.Font = new Font("Segoe UI", 10F);
            btnEdit.Location = new Point(12, 104);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(164, 36);
            btnEdit.TabIndex = 2;
            btnEdit.Text = "✏️ Sửa";
            btnEdit.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            btnDelete.Font = new Font("Segoe UI", 10F);
            btnDelete.Location = new Point(12, 146);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(164, 36);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "🗑️ Xóa";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.BackColor = Color.FromArgb(52, 73, 94);
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(1710, 204);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(120, 36);
            btnRefresh.TabIndex = 4;
            btnRefresh.Text = "⟳ Làm mới";
            btnRefresh.UseVisualStyleBackColor = false;
            // 
            // dgvPersonnel
            // 
            dgvPersonnel.AllowUserToAddRows = false;
            dgvPersonnel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvPersonnel.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPersonnel.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPersonnel.Location = new Point(1, 246);
            dgvPersonnel.MultiSelect = false;
            dgvPersonnel.Name = "dgvPersonnel";
            dgvPersonnel.ReadOnly = true;
            dgvPersonnel.RowHeadersVisible = false;
            dgvPersonnel.RowHeadersWidth = 51;
            dgvPersonnel.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPersonnel.Size = new Size(1840, 318);
            dgvPersonnel.TabIndex = 5;
            dgvPersonnel.CellClick += dgvPersonnel_CellClick;
            dgvPersonnel.CellDoubleClick += dgvPersonnel_CellDoubleClick;
            // 
            // btnExport
            // 
            btnExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExport.BackColor = Color.FromArgb(39, 174, 96);
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.ForeColor = Color.White;
            btnExport.Location = new Point(1584, 206);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(120, 36);
            btnExport.TabIndex = 6;
            btnExport.Text = "📊 Excel";
            btnExport.UseVisualStyleBackColor = false;
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
            pnlTongCBCS.Location = new Point(832, 9);
            pnlTongCBCS.Margin = new Padding(15);
            pnlTongCBCS.Name = "pnlTongCBCS";
            pnlTongCBCS.Size = new Size(166, 114);
            pnlTongCBCS.TabIndex = 11;
            // 
            // lblTongValue
            // 
            lblTongValue.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            lblTongValue.AutoSize = true;
            lblTongValue.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTongValue.Location = new Point(65, 47);
            lblTongValue.Name = "lblTongValue";
            lblTongValue.Size = new Size(0, 54);
            lblTongValue.TabIndex = 8;
            // 
            // pnlChienSi
            // 
            pnlChienSi.Anchor = AnchorStyles.Top;
            pnlChienSi.Controls.Add(lblChienSiValue);
            pnlChienSi.Controls.Add(lblChienSiTitle);
            pnlChienSi.Location = new Point(1021, 9);
            pnlChienSi.Margin = new Padding(15);
            pnlChienSi.Name = "pnlChienSi";
            pnlChienSi.Size = new Size(166, 114);
            pnlChienSi.TabIndex = 12;
            // 
            // lblChienSiValue
            // 
            lblChienSiValue.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            lblChienSiValue.AutoSize = true;
            lblChienSiValue.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblChienSiValue.Location = new Point(61, 47);
            lblChienSiValue.Name = "lblChienSiValue";
            lblChienSiValue.Size = new Size(0, 54);
            lblChienSiValue.TabIndex = 9;
            // 
            // pnlCanBo
            // 
            pnlCanBo.Anchor = AnchorStyles.Top;
            pnlCanBo.Controls.Add(lblCanBoValue);
            pnlCanBo.Controls.Add(lblCanBoTitle);
            pnlCanBo.Location = new Point(1208, 9);
            pnlCanBo.Margin = new Padding(15);
            pnlCanBo.Name = "pnlCanBo";
            pnlCanBo.Size = new Size(166, 114);
            pnlCanBo.TabIndex = 12;
            // 
            // lblCanBoValue
            // 
            lblCanBoValue.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            lblCanBoValue.AutoSize = true;
            lblCanBoValue.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblCanBoValue.Location = new Point(71, 47);
            lblCanBoValue.Name = "lblCanBoValue";
            lblCanBoValue.Size = new Size(0, 54);
            lblCanBoValue.TabIndex = 10;
            lblCanBoValue.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlDangVien
            // 
            pnlDangVien.Anchor = AnchorStyles.Top;
            pnlDangVien.Controls.Add(lblDangVienValue);
            pnlDangVien.Controls.Add(lblDangVienTitle);
            pnlDangVien.Location = new Point(1396, 9);
            pnlDangVien.Margin = new Padding(15);
            pnlDangVien.Name = "pnlDangVien";
            pnlDangVien.Size = new Size(166, 114);
            pnlDangVien.TabIndex = 12;
            // 
            // lblDangVienValue
            // 
            lblDangVienValue.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            lblDangVienValue.AutoSize = true;
            lblDangVienValue.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblDangVienValue.Location = new Point(67, 47);
            lblDangVienValue.Name = "lblDangVienValue";
            lblDangVienValue.Size = new Size(0, 54);
            lblDangVienValue.TabIndex = 11;
            // 
            // btnBackup
            // 
            btnBackup.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBackup.BackColor = Color.FromArgb(52, 152, 219);
            btnBackup.FlatAppearance.BorderSize = 0;
            btnBackup.FlatStyle = FlatStyle.Flat;
            btnBackup.ForeColor = Color.White;
            btnBackup.Location = new Point(1584, 162);
            btnBackup.Name = "btnBackup";
            btnBackup.Size = new Size(120, 36);
            btnBackup.TabIndex = 13;
            btnBackup.Text = "💽 Backup";
            btnBackup.UseVisualStyleBackColor = false;
            btnBackup.Click += btnBackup_Click;
            // 
            // btnRestore
            // 
            btnRestore.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRestore.BackColor = Color.FromArgb(243, 156, 18);
            btnRestore.FlatAppearance.BorderSize = 0;
            btnRestore.FlatStyle = FlatStyle.Flat;
            btnRestore.ForeColor = Color.White;
            btnRestore.Location = new Point(1710, 162);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new Size(120, 36);
            btnRestore.TabIndex = 14;
            btnRestore.Text = "🔄️ Restore";
            btnRestore.UseVisualStyleBackColor = false;
            btnRestore.Click += btnRestore_Click;
            // 
            // lblCurrentUser
            // 
            lblCurrentUser.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblCurrentUser.AutoSize = true;
            lblCurrentUser.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCurrentUser.Location = new Point(1622, 27);
            lblCurrentUser.Name = "lblCurrentUser";
            lblCurrentUser.Size = new Size(104, 20);
            lblCurrentUser.TabIndex = 15;
            lblCurrentUser.Text = "👤 username";
            // 
            // btnLogout
            // 
            btnLogout.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLogout.BackColor = Color.Red;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLogout.ForeColor = SystemColors.ButtonHighlight;
            btnLogout.Location = new Point(1768, 9);
            btnLogout.Name = "btnLogout";
            btnLogout.Padding = new Padding(5);
            btnLogout.Size = new Size(48, 48);
            btnLogout.TabIndex = 16;
            btnLogout.Text = "\u23fb";
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += btnLogout_Click;
            // 
            // btnUsers
            // 
            btnUsers.Font = new Font("Segoe UI", 10F);
            btnUsers.Location = new Point(12, 188);
            btnUsers.Name = "btnUsers";
            btnUsers.Size = new Size(164, 36);
            btnUsers.TabIndex = 17;
            btnUsers.Text = "👥 Tài khoản";
            btnUsers.UseVisualStyleBackColor = true;
            btnUsers.Click += btnUsers_Click;
            // 
            // btnAuditLog
            // 
            btnAuditLog.Font = new Font("Segoe UI", 10F);
            btnAuditLog.Location = new Point(182, 101);
            btnAuditLog.Name = "btnAuditLog";
            btnAuditLog.Size = new Size(164, 36);
            btnAuditLog.TabIndex = 18;
            btnAuditLog.Text = "📓 Nhật ký";
            btnAuditLog.UseVisualStyleBackColor = true;
            btnAuditLog.Click += btnAuditLog_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(13, 54);
            label1.Name = "label1";
            label1.Size = new Size(69, 20);
            label1.TabIndex = 19;
            label1.Text = "👨 Nam:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(13, 74);
            label2.Name = "label2";
            label2.Size = new Size(57, 20);
            label2.TabIndex = 20;
            label2.Text = "👩 Nữ:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(13, 94);
            label3.Name = "label3";
            label3.Size = new Size(87, 20);
            label3.TabIndex = 21;
            label3.Text = "🎂 Tuổi TB:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(13, 114);
            label4.Name = "label4";
            label4.Size = new Size(152, 20);
            label4.TabIndex = 22;
            label4.Text = "🏢 Đơn vị đông nhất:";
            // 
            // lblDonViDongNhat
            // 
            lblDonViDongNhat.AutoSize = true;
            lblDonViDongNhat.Location = new Point(174, 114);
            lblDonViDongNhat.Name = "lblDonViDongNhat";
            lblDonViDongNhat.Size = new Size(21, 20);
            lblDonViDongNhat.TabIndex = 27;
            lblDonViDongNhat.Text = "--";
            // 
            // lblTuoiTrungBinh
            // 
            lblTuoiTrungBinh.AutoSize = true;
            lblTuoiTrungBinh.Location = new Point(174, 94);
            lblTuoiTrungBinh.Name = "lblTuoiTrungBinh";
            lblTuoiTrungBinh.Size = new Size(17, 20);
            lblTuoiTrungBinh.TabIndex = 26;
            lblTuoiTrungBinh.Text = "0";
            // 
            // lblNu
            // 
            lblNu.AutoSize = true;
            lblNu.Location = new Point(174, 74);
            lblNu.Name = "lblNu";
            lblNu.Size = new Size(17, 20);
            lblNu.TabIndex = 25;
            lblNu.Text = "0";
            // 
            // lblNam
            // 
            lblNam.AutoSize = true;
            lblNam.Location = new Point(174, 54);
            lblNam.Name = "lblNam";
            lblNam.Size = new Size(17, 20);
            lblNam.TabIndex = 24;
            lblNam.Text = "0";
            // 
            // btnChangePassword
            // 
            btnChangePassword.Font = new Font("Segoe UI", 10F);
            btnChangePassword.Location = new Point(182, 146);
            btnChangePassword.Name = "btnChangePassword";
            btnChangePassword.Size = new Size(164, 36);
            btnChangePassword.TabIndex = 28;
            btnChangePassword.Text = "🔑 Đổi mật khẩu";
            btnChangePassword.UseVisualStyleBackColor = true;
            btnChangePassword.Click += btnChangePassword_Click;
            // 
            // pnlThongKe
            // 
            pnlThongKe.Controls.Add(lblThongke);
            pnlThongKe.Controls.Add(lblDonViDongNhat);
            pnlThongKe.Controls.Add(label4);
            pnlThongKe.Controls.Add(lblNam);
            pnlThongKe.Controls.Add(lblNu);
            pnlThongKe.Controls.Add(lblTuoiTrungBinh);
            pnlThongKe.Controls.Add(label3);
            pnlThongKe.Controls.Add(label2);
            pnlThongKe.Controls.Add(label1);
            pnlThongKe.Location = new Point(539, 11);
            pnlThongKe.Margin = new Padding(15);
            pnlThongKe.Name = "pnlThongKe";
            pnlThongKe.Size = new Size(278, 143);
            pnlThongKe.TabIndex = 29;
            // 
            // lblThongke
            // 
            lblThongke.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            lblThongke.AutoSize = true;
            lblThongke.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            lblThongke.Location = new Point(87, 11);
            lblThongke.Name = "lblThongke";
            lblThongke.Size = new Size(96, 23);
            lblThongke.TabIndex = 9;
            lblThongke.Text = "THỐNG KÊ";
            // 
            // pnlSearch
            // 
            pnlSearch.BackColor = Color.White;
            pnlSearch.Controls.Add(txtSearch);
            pnlSearch.Location = new Point(12, 12);
            pnlSearch.Name = "pnlSearch";
            pnlSearch.Padding = new Padding(5);
            pnlSearch.Size = new Size(509, 41);
            pnlSearch.TabIndex = 30;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { lblUser, lblRole, lblTime, lblRecordCount });
            statusStrip1.Location = new Point(0, 538);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1842, 26);
            statusStrip1.TabIndex = 31;
            statusStrip1.Text = "statusStrip1";
            // 
            // lblUser
            // 
            lblUser.Name = "lblUser";
            lblUser.Size = new Size(55, 20);
            lblUser.Text = "lblUser";
            // 
            // lblRole
            // 
            lblRole.Name = "lblRole";
            lblRole.Size = new Size(56, 20);
            lblRole.Text = "lblRole";
            // 
            // lblTime
            // 
            lblTime.Name = "lblTime";
            lblTime.Size = new Size(59, 20);
            lblTime.Text = "lblTime";
            // 
            // lblRecordCount
            // 
            lblRecordCount.Name = "lblRecordCount";
            lblRecordCount.Size = new Size(154, 20);
            lblRecordCount.Text = "📋 Hiển thị: 0/0 CBCS";
            // 
            // cboDonVi
            // 
            cboDonVi.FormattingEnabled = true;
            cboDonVi.Location = new Point(907, 212);
            cboDonVi.Name = "cboDonVi";
            cboDonVi.Size = new Size(166, 28);
            cboDonVi.TabIndex = 32;
            cboDonVi.Text = "Đơn vị";
            cboDonVi.SelectedIndexChanged += FilterChanged;
            // 
            // cboCapBac
            // 
            cboCapBac.FormattingEnabled = true;
            cboCapBac.Location = new Point(1096, 212);
            cboCapBac.Name = "cboCapBac";
            cboCapBac.Size = new Size(166, 28);
            cboCapBac.TabIndex = 33;
            cboCapBac.Text = "Cấp bậc";
            cboCapBac.SelectedIndexChanged += FilterChanged;
            // 
            // cboChucVu
            // 
            cboChucVu.FormattingEnabled = true;
            cboChucVu.Location = new Point(1283, 212);
            cboChucVu.Name = "cboChucVu";
            cboChucVu.Size = new Size(166, 28);
            cboChucVu.TabIndex = 34;
            cboChucVu.Text = "Chức vụ";
            cboChucVu.SelectedIndexChanged += FilterChanged;
            // 
            // btnMission
            // 
            btnMission.Font = new Font("Segoe UI", 10F);
            btnMission.Location = new Point(182, 59);
            btnMission.Name = "btnMission";
            btnMission.Size = new Size(164, 36);
            btnMission.TabIndex = 35;
            btnMission.Text = "📋 Công tác";
            btnMission.UseVisualStyleBackColor = true;
            btnMission.Click += btnMission_Click;
            // 
            // btnStatistics
            // 
            btnStatistics.Location = new Point(182, 188);
            btnStatistics.Name = "btnStatistics";
            btnStatistics.Size = new Size(164, 36);
            btnStatistics.TabIndex = 36;
            btnStatistics.Text = "📊 Thống kê CT";
            btnStatistics.UseVisualStyleBackColor = true;
            btnStatistics.Click += btnStatistics_Click;
            // 
            // FrmPersonnelList
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1842, 564);
            Controls.Add(btnStatistics);
            Controls.Add(btnMission);
            Controls.Add(cboChucVu);
            Controls.Add(cboCapBac);
            Controls.Add(cboDonVi);
            Controls.Add(statusStrip1);
            Controls.Add(pnlThongKe);
            Controls.Add(btnChangePassword);
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
            Controls.Add(pnlTongCBCS);
            Controls.Add(pnlDangVien);
            Controls.Add(pnlCanBo);
            Controls.Add(pnlChienSi);
            Controls.Add(pnlSearch);
            Icon = (Icon)resources.GetObject("$this.Icon");
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
            pnlThongKe.ResumeLayout(false);
            pnlThongKe.PerformLayout();
            pnlSearch.ResumeLayout(false);
            pnlSearch.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
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
        private Panel pnlThongKe;
        private Label lblThongke;
        private Panel pnlSearch;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lblUser;
        private ToolStripStatusLabel lblRole;
        private ToolStripStatusLabel lblTime;
        private ToolStripStatusLabel lblRecordCount;
        private ComboBox cboDonVi;
        private ComboBox cboCapBac;
        private ComboBox cboChucVu;
        private ToolTip toolTip1;
        private Button btnMission;
        private Button btnStatistics;
    }
}