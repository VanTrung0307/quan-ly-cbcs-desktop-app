namespace QuanLyCBCS.Forms.MissionForms
{
    partial class FrmMissionPersonnel
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
            pnlHeader = new Panel();
            lblMissionId = new Label();
            lblMissionName = new Label();
            lblTitle = new Label();
            lblSearch = new Label();
            txtSearch = new TextBox();
            btnRefresh = new Button();
            grpAvailable = new GroupBox();
            dgvAvailable = new DataGridView();
            colSttAvailable = new DataGridViewTextBoxColumn();
            colSoHieuAvailable = new DataGridViewTextBoxColumn();
            colHoTenAvailable = new DataGridViewTextBoxColumn();
            colDonViAvailable = new DataGridViewTextBoxColumn();
            colCapBacAvailable = new DataGridViewTextBoxColumn();
            colSoLanCT = new DataGridViewTextBoxColumn();
            pnlAction = new Panel();
            btnRemove = new Button();
            btnAdd = new Button();
            grpAssigned = new GroupBox();
            dgvAssigned = new DataGridView();
            colSttAssigned = new DataGridViewTextBoxColumn();
            colSoHieuAssigned = new DataGridViewTextBoxColumn();
            colHoTenAssigned = new DataGridViewTextBoxColumn();
            colSoLanCTAssigned = new DataGridViewTextBoxColumn();
            colSDonViAssigned = new DataGridViewTextBoxColumn();
            colCapBacHamAssigned = new DataGridViewTextBoxColumn();
            colNgayPhanCongAssigned = new DataGridViewTextBoxColumn();
            panel1 = new Panel();
            lblAssignedCount = new Label();
            lblAvailableCount = new Label();
            btnClose = new Button();
            pnlHeader.SuspendLayout();
            grpAvailable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAvailable).BeginInit();
            pnlAction.SuspendLayout();
            grpAssigned.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAssigned).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(44, 62, 80);
            pnlHeader.Controls.Add(lblMissionId);
            pnlHeader.Controls.Add(lblMissionName);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1382, 90);
            pnlHeader.TabIndex = 0;
            // 
            // lblMissionId
            // 
            lblMissionId.AutoSize = true;
            lblMissionId.Location = new Point(1281, 60);
            lblMissionId.Name = "lblMissionId";
            lblMissionId.Size = new Size(89, 20);
            lblMissionId.TabIndex = 2;
            lblMissionId.Text = "lblMissionId";
            lblMissionId.Visible = false;
            // 
            // lblMissionName
            // 
            lblMissionName.AutoSize = true;
            lblMissionName.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblMissionName.ForeColor = Color.White;
            lblMissionName.Location = new Point(500, 60);
            lblMissionName.Name = "lblMissionName";
            lblMissionName.Size = new Size(196, 25);
            lblMissionName.TabIndex = 1;
            lblMissionName.Text = "Công tác: Chưa chọn";
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(1382, 60);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "PHÂN CÔNG CBCS";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(49, 111);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(112, 20);
            lblSearch.TabIndex = 1;
            lblSearch.Text = "Tìm kiếm CBCS:";
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(167, 108);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Nhập họ tên, số hiệu...";
            txtSearch.Size = new Size(350, 27);
            txtSearch.TabIndex = 2;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(628, 185);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(120, 40);
            btnRefresh.TabIndex = 3;
            btnRefresh.Text = "⟳ Làm mới";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += FrmMissionPersonnel_Load;
            // 
            // grpAvailable
            // 
            grpAvailable.Controls.Add(dgvAvailable);
            grpAvailable.Location = new Point(34, 162);
            grpAvailable.Name = "grpAvailable";
            grpAvailable.Size = new Size(550, 500);
            grpAvailable.TabIndex = 5;
            grpAvailable.TabStop = false;
            grpAvailable.Text = "CBCS CHƯA PHÂN CÔNG";
            // 
            // dgvAvailable
            // 
            dgvAvailable.AllowUserToAddRows = false;
            dgvAvailable.AllowUserToDeleteRows = false;
            dgvAvailable.AllowUserToResizeRows = false;
            dgvAvailable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAvailable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAvailable.Columns.AddRange(new DataGridViewColumn[] { colSttAvailable, colSoHieuAvailable, colHoTenAvailable, colDonViAvailable, colCapBacAvailable, colSoLanCT });
            dgvAvailable.Dock = DockStyle.Fill;
            dgvAvailable.Location = new Point(3, 23);
            dgvAvailable.Name = "dgvAvailable";
            dgvAvailable.ReadOnly = true;
            dgvAvailable.RowHeadersVisible = false;
            dgvAvailable.RowHeadersWidth = 51;
            dgvAvailable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAvailable.Size = new Size(544, 474);
            dgvAvailable.TabIndex = 6;
            dgvAvailable.CellDoubleClick += dgvAvailable_CellDoubleClick;
            // 
            // colSttAvailable
            // 
            colSttAvailable.HeaderText = "STT";
            colSttAvailable.MinimumWidth = 6;
            colSttAvailable.Name = "colSttAvailable";
            colSttAvailable.ReadOnly = true;
            // 
            // colSoHieuAvailable
            // 
            colSoHieuAvailable.DataPropertyName = "SoHieu";
            colSoHieuAvailable.HeaderText = "Số hiệu";
            colSoHieuAvailable.MinimumWidth = 6;
            colSoHieuAvailable.Name = "colSoHieuAvailable";
            colSoHieuAvailable.ReadOnly = true;
            // 
            // colHoTenAvailable
            // 
            colHoTenAvailable.DataPropertyName = "HoTen";
            colHoTenAvailable.HeaderText = "Họ tên";
            colHoTenAvailable.MinimumWidth = 6;
            colHoTenAvailable.Name = "colHoTenAvailable";
            colHoTenAvailable.ReadOnly = true;
            // 
            // colDonViAvailable
            // 
            colDonViAvailable.DataPropertyName = "DonVi";
            colDonViAvailable.HeaderText = "Đơn vị";
            colDonViAvailable.MinimumWidth = 6;
            colDonViAvailable.Name = "colDonViAvailable";
            colDonViAvailable.ReadOnly = true;
            // 
            // colCapBacAvailable
            // 
            colCapBacAvailable.DataPropertyName = "CapBacHam";
            colCapBacAvailable.HeaderText = "Cấp bậc";
            colCapBacAvailable.MinimumWidth = 6;
            colCapBacAvailable.Name = "colCapBacAvailable";
            colCapBacAvailable.ReadOnly = true;
            // 
            // colSoLanCT
            // 
            colSoLanCT.DataPropertyName = "SoLanCongTac";
            colSoLanCT.HeaderText = "Số lần CT";
            colSoLanCT.MinimumWidth = 6;
            colSoLanCT.Name = "colSoLanCT";
            colSoLanCT.ReadOnly = true;
            // 
            // pnlAction
            // 
            pnlAction.Controls.Add(btnRemove);
            pnlAction.Controls.Add(btnAdd);
            pnlAction.Location = new Point(628, 376);
            pnlAction.Name = "pnlAction";
            pnlAction.Size = new Size(120, 125);
            pnlAction.TabIndex = 6;
            // 
            // btnRemove
            // 
            btnRemove.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnRemove.Location = new Point(20, 64);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new Size(80, 40);
            btnRemove.TabIndex = 8;
            btnRemove.Text = "◀◀";
            btnRemove.UseVisualStyleBackColor = true;
            btnRemove.Click += btnRemove_Click;
            // 
            // btnAdd
            // 
            btnAdd.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAdd.Location = new Point(20, 18);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(80, 40);
            btnAdd.TabIndex = 7;
            btnAdd.Text = "▶▶";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAssign_Click;
            // 
            // grpAssigned
            // 
            grpAssigned.Controls.Add(dgvAssigned);
            grpAssigned.Location = new Point(795, 162);
            grpAssigned.Name = "grpAssigned";
            grpAssigned.Size = new Size(550, 500);
            grpAssigned.TabIndex = 7;
            grpAssigned.TabStop = false;
            grpAssigned.Text = "CBCS ĐÃ PHÂN CÔNG";
            // 
            // dgvAssigned
            // 
            dgvAssigned.AllowUserToAddRows = false;
            dgvAssigned.AllowUserToDeleteRows = false;
            dgvAssigned.AllowUserToResizeRows = false;
            dgvAssigned.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAssigned.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAssigned.Columns.AddRange(new DataGridViewColumn[] { colSttAssigned, colSoHieuAssigned, colHoTenAssigned, colSoLanCTAssigned, colSDonViAssigned, colCapBacHamAssigned, colNgayPhanCongAssigned });
            dgvAssigned.Dock = DockStyle.Fill;
            dgvAssigned.Location = new Point(3, 23);
            dgvAssigned.Name = "dgvAssigned";
            dgvAssigned.ReadOnly = true;
            dgvAssigned.RowHeadersVisible = false;
            dgvAssigned.RowHeadersWidth = 51;
            dgvAssigned.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAssigned.Size = new Size(544, 474);
            dgvAssigned.TabIndex = 6;
            dgvAssigned.CellDoubleClick += dgvAssigned_CellDoubleClick;
            // 
            // colSttAssigned
            // 
            colSttAssigned.HeaderText = "STT";
            colSttAssigned.MinimumWidth = 6;
            colSttAssigned.Name = "colSttAssigned";
            colSttAssigned.ReadOnly = true;
            // 
            // colSoHieuAssigned
            // 
            colSoHieuAssigned.DataPropertyName = "SoHieu";
            colSoHieuAssigned.HeaderText = "Số hiệu";
            colSoHieuAssigned.MinimumWidth = 6;
            colSoHieuAssigned.Name = "colSoHieuAssigned";
            colSoHieuAssigned.ReadOnly = true;
            // 
            // colHoTenAssigned
            // 
            colHoTenAssigned.DataPropertyName = "HoTen";
            colHoTenAssigned.HeaderText = "Họ tên";
            colHoTenAssigned.MinimumWidth = 6;
            colHoTenAssigned.Name = "colHoTenAssigned";
            colHoTenAssigned.ReadOnly = true;
            // 
            // colSoLanCTAssigned
            // 
            colSoLanCTAssigned.DataPropertyName = "SoLanCongTac";
            colSoLanCTAssigned.HeaderText = "Số lần CT";
            colSoLanCTAssigned.MinimumWidth = 6;
            colSoLanCTAssigned.Name = "colSoLanCTAssigned";
            colSoLanCTAssigned.ReadOnly = true;
            // 
            // colSDonViAssigned
            // 
            colSDonViAssigned.DataPropertyName = "DonVi";
            colSDonViAssigned.HeaderText = "Đơn vị";
            colSDonViAssigned.MinimumWidth = 6;
            colSDonViAssigned.Name = "colSDonViAssigned";
            colSDonViAssigned.ReadOnly = true;
            // 
            // colCapBacHamAssigned
            // 
            colCapBacHamAssigned.DataPropertyName = "CapBacHam";
            colCapBacHamAssigned.HeaderText = "Cấp bậc";
            colCapBacHamAssigned.MinimumWidth = 6;
            colCapBacHamAssigned.Name = "colCapBacHamAssigned";
            colCapBacHamAssigned.ReadOnly = true;
            // 
            // colNgayPhanCongAssigned
            // 
            colNgayPhanCongAssigned.DataPropertyName = "NgayPhanCong";
            colNgayPhanCongAssigned.HeaderText = "Ngày phân công";
            colNgayPhanCongAssigned.MinimumWidth = 6;
            colNgayPhanCongAssigned.Name = "colNgayPhanCongAssigned";
            colNgayPhanCongAssigned.ReadOnly = true;
            // 
            // panel1
            // 
            panel1.Controls.Add(lblAssignedCount);
            panel1.Controls.Add(lblAvailableCount);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 713);
            panel1.Name = "panel1";
            panel1.Size = new Size(1382, 40);
            panel1.TabIndex = 8;
            // 
            // lblAssignedCount
            // 
            lblAssignedCount.AutoSize = true;
            lblAssignedCount.Location = new Point(798, 11);
            lblAssignedCount.Name = "lblAssignedCount";
            lblAssignedCount.Size = new Size(156, 20);
            lblAssignedCount.TabIndex = 1;
            lblAssignedCount.Text = "Đã phân công: 0 CBCS";
            // 
            // lblAvailableCount
            // 
            lblAvailableCount.AutoSize = true;
            lblAvailableCount.Location = new Point(37, 11);
            lblAvailableCount.Name = "lblAvailableCount";
            lblAvailableCount.Size = new Size(171, 20);
            lblAvailableCount.TabIndex = 0;
            lblAvailableCount.Text = "Chưa phân công: 0 CBCS";
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.Firebrick;
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(628, 630);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(120, 40);
            btnClose.TabIndex = 9;
            btnClose.Text = "❌ Đóng";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // FrmMissionPersonnel
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1382, 753);
            Controls.Add(btnClose);
            Controls.Add(panel1);
            Controls.Add(grpAssigned);
            Controls.Add(pnlAction);
            Controls.Add(grpAvailable);
            Controls.Add(btnRefresh);
            Controls.Add(txtSearch);
            Controls.Add(lblSearch);
            Controls.Add(pnlHeader);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmMissionPersonnel";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Phân công CBCS";
            Load += FrmMissionPersonnel_Load;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            grpAvailable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvAvailable).EndInit();
            pnlAction.ResumeLayout(false);
            grpAssigned.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvAssigned).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlHeader;
        private Label lblTitle;
        private Label lblMissionName;
        private Label lblSearch;
        private TextBox txtSearch;
        private Button btnRefresh;
        private GroupBox grpAvailable;
        private DataGridView dgvAvailable;
        private Panel pnlAction;
        private Button btnRemove;
        private Button btnAdd;
        private GroupBox grpAssigned;
        private DataGridView dgvAssigned;
        private Panel panel1;
        private Label lblAvailableCount;
        private Label lblAssignedCount;
        private Button btnClose;
        private Label lblMissionId;
        private DataGridViewTextBoxColumn colSttAvailable;
        private DataGridViewTextBoxColumn colSoHieuAvailable;
        private DataGridViewTextBoxColumn colHoTenAvailable;
        private DataGridViewTextBoxColumn colDonViAvailable;
        private DataGridViewTextBoxColumn colCapBacAvailable;
        private DataGridViewTextBoxColumn colSoLanCT;
        private DataGridViewTextBoxColumn colSttAssigned;
        private DataGridViewTextBoxColumn colSoHieuAssigned;
        private DataGridViewTextBoxColumn colHoTenAssigned;
        private DataGridViewTextBoxColumn colSoLanCTAssigned;
        private DataGridViewTextBoxColumn colSDonViAssigned;
        private DataGridViewTextBoxColumn colCapBacHamAssigned;
        private DataGridViewTextBoxColumn colNgayPhanCongAssigned;
    }
}