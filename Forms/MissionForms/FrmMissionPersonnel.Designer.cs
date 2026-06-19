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
            lblTitle = new Label();
            lblMissionName = new Label();
            lblSearch = new Label();
            txtSearch = new TextBox();
            btnRefresh = new Button();
            grpAvailable = new GroupBox();
            dgvAvailable = new DataGridView();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            Column4 = new DataGridViewTextBoxColumn();
            Column5 = new DataGridViewTextBoxColumn();
            pnlAction = new Panel();
            btnAdd = new Button();
            btnRemove = new Button();
            grpAssigned = new GroupBox();
            dgvAssigned = new DataGridView();
            panel1 = new Panel();
            lblAvailableCount = new Label();
            lblAssignedCount = new Label();
            btnClose = new Button();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            Column7 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            Column6 = new DataGridViewTextBoxColumn();
            lblMissionId = new Label();
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
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(628, 185);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(120, 40);
            btnRefresh.TabIndex = 3;
            btnRefresh.Text = "⟳ Làm mới";
            btnRefresh.UseVisualStyleBackColor = true;
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
            dgvAvailable.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5 });
            dgvAvailable.Dock = DockStyle.Fill;
            dgvAvailable.Location = new Point(3, 23);
            dgvAvailable.Name = "dgvAvailable";
            dgvAvailable.ReadOnly = true;
            dgvAvailable.RowHeadersVisible = false;
            dgvAvailable.RowHeadersWidth = 51;
            dgvAvailable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAvailable.Size = new Size(544, 474);
            dgvAvailable.TabIndex = 6;
            // 
            // Column1
            // 
            Column1.HeaderText = "STT";
            Column1.MinimumWidth = 6;
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            // 
            // Column2
            // 
            Column2.HeaderText = "Số hiệu";
            Column2.MinimumWidth = 6;
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            // 
            // Column3
            // 
            Column3.HeaderText = "Họ tên";
            Column3.MinimumWidth = 6;
            Column3.Name = "Column3";
            Column3.ReadOnly = true;
            // 
            // Column4
            // 
            Column4.HeaderText = "Đơn vị";
            Column4.MinimumWidth = 6;
            Column4.Name = "Column4";
            Column4.ReadOnly = true;
            // 
            // Column5
            // 
            Column5.HeaderText = "Cấp bậc";
            Column5.MinimumWidth = 6;
            Column5.Name = "Column5";
            Column5.ReadOnly = true;
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
            // btnAdd
            // 
            btnAdd.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAdd.Location = new Point(20, 18);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(80, 40);
            btnAdd.TabIndex = 7;
            btnAdd.Text = "▶▶";
            btnAdd.UseVisualStyleBackColor = true;
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
            dgvAssigned.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, Column7, dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5, Column6 });
            dgvAssigned.Dock = DockStyle.Fill;
            dgvAssigned.Location = new Point(3, 23);
            dgvAssigned.Name = "dgvAssigned";
            dgvAssigned.ReadOnly = true;
            dgvAssigned.RowHeadersVisible = false;
            dgvAssigned.RowHeadersWidth = 51;
            dgvAssigned.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAssigned.Size = new Size(544, 474);
            dgvAssigned.TabIndex = 6;
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
            // lblAvailableCount
            // 
            lblAvailableCount.AutoSize = true;
            lblAvailableCount.Location = new Point(37, 11);
            lblAvailableCount.Name = "lblAvailableCount";
            lblAvailableCount.Size = new Size(171, 20);
            lblAvailableCount.TabIndex = 0;
            lblAvailableCount.Text = "Chưa phân công: 0 CBCS";
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
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "STT";
            dataGridViewTextBoxColumn1.MinimumWidth = 6;
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.HeaderText = "Số hiệu";
            dataGridViewTextBoxColumn2.MinimumWidth = 6;
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.HeaderText = "Họ tên";
            dataGridViewTextBoxColumn3.MinimumWidth = 6;
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // Column7
            // 
            Column7.HeaderText = "Số lần CT";
            Column7.MinimumWidth = 6;
            Column7.Name = "Column7";
            Column7.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.HeaderText = "Đơn vị";
            dataGridViewTextBoxColumn4.MinimumWidth = 6;
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.HeaderText = "Cấp bậc";
            dataGridViewTextBoxColumn5.MinimumWidth = 6;
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // Column6
            // 
            Column6.HeaderText = "Ngày phân công";
            Column6.MinimumWidth = 6;
            Column6.Name = "Column6";
            Column6.ReadOnly = true;
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
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewTextBoxColumn Column5;
        private Panel pnlAction;
        private Button btnRemove;
        private Button btnAdd;
        private GroupBox grpAssigned;
        private DataGridView dgvAssigned;
        private Panel panel1;
        private Label lblAvailableCount;
        private Label lblAssignedCount;
        private Button btnClose;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn Column7;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn Column6;
        private Label lblMissionId;
    }
}