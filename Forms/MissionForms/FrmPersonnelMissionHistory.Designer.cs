namespace QuanLyCBCS.Forms.MissionForms
{
    partial class FrmPersonnelMissionHistory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPersonnelMissionHistory));
            pnlHeader = new Panel();
            lblTitle = new Label();
            lblPersonnelInfo = new Label();
            txtSearch = new TextBox();
            btnRefresh = new Button();
            btnExcel = new Button();
            btnClose = new Button();
            dgvHistory = new DataGridView();
            colStt = new DataGridViewTextBoxColumn();
            colNgayPhanCong = new DataGridViewTextBoxColumn();
            colTenCongTac = new DataGridViewTextBoxColumn();
            colLoaiCongTac = new DataGridViewTextBoxColumn();
            colGhiChu = new DataGridViewTextBoxColumn();
            panel1 = new Panel();
            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHistory).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(44, 62, 80);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(885, 80);
            pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(885, 80);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "LỊCH SỬ CÔNG TÁC CBCS";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblPersonnelInfo
            // 
            lblPersonnelInfo.AutoSize = true;
            lblPersonnelInfo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblPersonnelInfo.Location = new Point(12, 18);
            lblPersonnelInfo.Name = "lblPersonnelInfo";
            lblPersonnelInfo.Size = new Size(64, 25);
            lblPersonnelInfo.TabIndex = 1;
            lblPersonnelInfo.Text = "CBCS:";
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(273, 16);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Tìm công tác...";
            txtSearch.Size = new Size(300, 27);
            txtSearch.TabIndex = 2;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(679, 16);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(94, 29);
            btnRefresh.TabIndex = 3;
            btnRefresh.Text = "↻ Làm mới";
            btnRefresh.UseVisualStyleBackColor = true;
            // 
            // btnExcel
            // 
            btnExcel.Location = new Point(579, 16);
            btnExcel.Name = "btnExcel";
            btnExcel.Size = new Size(94, 29);
            btnExcel.TabIndex = 4;
            btnExcel.Text = "📊 Excel";
            btnExcel.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            btnClose.Location = new Point(779, 16);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(94, 29);
            btnClose.TabIndex = 5;
            btnClose.Text = "✖ Đóng";
            btnClose.UseVisualStyleBackColor = true;
            // 
            // dgvHistory
            // 
            dgvHistory.AllowUserToAddRows = false;
            dgvHistory.AllowUserToDeleteRows = false;
            dgvHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHistory.Columns.AddRange(new DataGridViewColumn[] { colStt, colNgayPhanCong, colTenCongTac, colLoaiCongTac, colGhiChu });
            dgvHistory.Dock = DockStyle.Bottom;
            dgvHistory.Location = new Point(0, 135);
            dgvHistory.MultiSelect = false;
            dgvHistory.Name = "dgvHistory";
            dgvHistory.ReadOnly = true;
            dgvHistory.RowHeadersWidth = 51;
            dgvHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHistory.Size = new Size(885, 315);
            dgvHistory.TabIndex = 6;
            // 
            // colStt
            // 
            colStt.HeaderText = "STT";
            colStt.MinimumWidth = 6;
            colStt.Name = "colStt";
            colStt.ReadOnly = true;
            colStt.Width = 60;
            // 
            // colNgayPhanCong
            // 
            colNgayPhanCong.DataPropertyName = "NgayPhanCong";
            colNgayPhanCong.HeaderText = "Ngày phân công";
            colNgayPhanCong.MinimumWidth = 6;
            colNgayPhanCong.Name = "colNgayPhanCong";
            colNgayPhanCong.ReadOnly = true;
            colNgayPhanCong.Width = 150;
            // 
            // colTenCongTac
            // 
            colTenCongTac.DataPropertyName = "TenCongTac";
            colTenCongTac.HeaderText = "Tên công tác";
            colTenCongTac.MinimumWidth = 6;
            colTenCongTac.Name = "colTenCongTac";
            colTenCongTac.ReadOnly = true;
            colTenCongTac.Width = 300;
            // 
            // colLoaiCongTac
            // 
            colLoaiCongTac.DataPropertyName = "LoaiCongTac";
            colLoaiCongTac.HeaderText = "Loại công tác";
            colLoaiCongTac.MinimumWidth = 6;
            colLoaiCongTac.Name = "colLoaiCongTac";
            colLoaiCongTac.ReadOnly = true;
            colLoaiCongTac.Width = 200;
            // 
            // colGhiChu
            // 
            colGhiChu.DataPropertyName = "GhiChu";
            colGhiChu.HeaderText = "Ghi chú";
            colGhiChu.MinimumWidth = 6;
            colGhiChu.Name = "colGhiChu";
            colGhiChu.ReadOnly = true;
            colGhiChu.Width = 125;
            // 
            // panel1
            // 
            panel1.Controls.Add(txtSearch);
            panel1.Controls.Add(lblPersonnelInfo);
            panel1.Controls.Add(btnClose);
            panel1.Controls.Add(btnRefresh);
            panel1.Controls.Add(btnExcel);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 80);
            panel1.Name = "panel1";
            panel1.Size = new Size(885, 58);
            panel1.TabIndex = 7;
            // 
            // FrmPersonnelMissionHistory
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(885, 450);
            Controls.Add(panel1);
            Controls.Add(dgvHistory);
            Controls.Add(pnlHeader);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FrmPersonnelMissionHistory";
            StartPosition = FormStartPosition.CenterParent;
            Text = "FrmPersonnelMissionHistory";
            pnlHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvHistory).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Label lblTitle;
        private Label lblPersonnelInfo;
        private TextBox txtSearch;
        private Button btnRefresh;
        private Button btnExcel;
        private Button btnClose;
        private DataGridView dgvHistory;
        private Panel panel1;
        private DataGridViewTextBoxColumn colStt;
        private DataGridViewTextBoxColumn colNgayPhanCong;
        private DataGridViewTextBoxColumn colTenCongTac;
        private DataGridViewTextBoxColumn colLoaiCongTac;
        private DataGridViewTextBoxColumn colGhiChu;
    }
}