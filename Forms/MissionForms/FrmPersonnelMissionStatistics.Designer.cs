namespace QuanLyCBCS.Forms.MissionForms
{
    partial class FrmPersonnelMissionStatistics
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
            lblTitle = new Label();
            dgvStatistics = new DataGridView();
            colSTT = new DataGridViewTextBoxColumn();
            colSoHieu = new DataGridViewTextBoxColumn();
            colHoTen = new DataGridViewTextBoxColumn();
            colDonVi = new DataGridViewTextBoxColumn();
            colCapBac = new DataGridViewTextBoxColumn();
            colSoLanCongTac = new DataGridViewTextBoxColumn();
            pnlHeader = new Panel();
            btnExportExcel = new Button();
            btnClose = new Button();
            btnRefresh = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvStatistics).BeginInit();
            pnlHeader.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(1182, 50);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "THỐNG KÊ CÔNG TÁC CBCS";
            lblTitle.TextAlign = ContentAlignment.TopCenter;
            // 
            // dgvStatistics
            // 
            dgvStatistics.BackgroundColor = SystemColors.Control;
            dgvStatistics.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvStatistics.Columns.AddRange(new DataGridViewColumn[] { colSTT, colSoHieu, colHoTen, colDonVi, colCapBac, colSoLanCongTac });
            dgvStatistics.Location = new Point(194, 144);
            dgvStatistics.Name = "dgvStatistics";
            dgvStatistics.RowHeadersWidth = 51;
            dgvStatistics.Size = new Size(802, 497);
            dgvStatistics.TabIndex = 1;
            // 
            // colSTT
            // 
            colSTT.HeaderText = "STT";
            colSTT.MinimumWidth = 6;
            colSTT.Name = "colSTT";
            colSTT.ReadOnly = true;
            colSTT.Width = 125;
            // 
            // colSoHieu
            // 
            colSoHieu.DataPropertyName = "SoHieu";
            colSoHieu.HeaderText = "Số hiệu";
            colSoHieu.MinimumWidth = 6;
            colSoHieu.Name = "colSoHieu";
            colSoHieu.ReadOnly = true;
            colSoHieu.Width = 125;
            // 
            // colHoTen
            // 
            colHoTen.DataPropertyName = "HoTen";
            colHoTen.HeaderText = "Họ tên";
            colHoTen.MinimumWidth = 6;
            colHoTen.Name = "colHoTen";
            colHoTen.ReadOnly = true;
            colHoTen.Width = 125;
            // 
            // colDonVi
            // 
            colDonVi.DataPropertyName = "DonVi";
            colDonVi.HeaderText = "Đơn vị";
            colDonVi.MinimumWidth = 6;
            colDonVi.Name = "colDonVi";
            colDonVi.ReadOnly = true;
            colDonVi.Width = 125;
            // 
            // colCapBac
            // 
            colCapBac.DataPropertyName = "CapBacHam";
            colCapBac.HeaderText = "Cấp bậc";
            colCapBac.MinimumWidth = 6;
            colCapBac.Name = "colCapBac";
            colCapBac.ReadOnly = true;
            colCapBac.Width = 125;
            // 
            // colSoLanCongTac
            // 
            colSoLanCongTac.DataPropertyName = "SoLanCongTac";
            colSoLanCongTac.HeaderText = "Số lần công tác";
            colSoLanCongTac.MinimumWidth = 6;
            colSoLanCongTac.Name = "colSoLanCongTac";
            colSoLanCongTac.ReadOnly = true;
            colSoLanCongTac.Width = 125;
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(44, 62, 80);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1182, 90);
            pnlHeader.TabIndex = 11;
            // 
            // btnExportExcel
            // 
            btnExportExcel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExportExcel.BackColor = Color.FromArgb(39, 174, 96);
            btnExportExcel.FlatStyle = FlatStyle.Flat;
            btnExportExcel.ForeColor = Color.White;
            btnExportExcel.Location = new Point(876, 96);
            btnExportExcel.Name = "btnExportExcel";
            btnExportExcel.Size = new Size(120, 36);
            btnExportExcel.TabIndex = 7;
            btnExportExcel.Text = "📊 Excel";
            btnExportExcel.UseVisualStyleBackColor = false;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.Firebrick;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(1002, 96);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(120, 36);
            btnClose.TabIndex = 10;
            btnClose.Text = "❌ Đóng";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.BackColor = Color.FromArgb(52, 73, 94);
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(750, 96);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(120, 36);
            btnRefresh.TabIndex = 8;
            btnRefresh.Text = "⟳ Làm mới";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // FrmPersonnelMissionStatistics
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1182, 653);
            Controls.Add(btnRefresh);
            Controls.Add(btnExportExcel);
            Controls.Add(btnClose);
            Controls.Add(dgvStatistics);
            Controls.Add(pnlHeader);
            Name = "FrmPersonnelMissionStatistics";
            Text = "FrmPersonnelMissionStatistics";
            Load += FrmPersonnelMissionStatistics_Load;
            ((System.ComponentModel.ISupportInitialize)dgvStatistics).EndInit();
            pnlHeader.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Label lblTitle;
        private DataGridView dgvStatistics;
        private DataGridViewTextBoxColumn colSTT;
        private DataGridViewTextBoxColumn colSoHieu;
        private DataGridViewTextBoxColumn colHoTen;
        private DataGridViewTextBoxColumn colDonVi;
        private DataGridViewTextBoxColumn colCapBac;
        private DataGridViewTextBoxColumn colSoLanCongTac;
        private Panel pnlHeader;
        private Button btnExportExcel;
        private Button btnClose;
        private Button btnRefresh;
    }
}