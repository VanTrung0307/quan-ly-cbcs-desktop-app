namespace QuanLyCBCS.Forms.PersonnelForms
{
    partial class FrmPersonnelReport
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
            pnlFilter = new Panel();
            btnExport = new Button();
            btnRefresh = new Button();
            dtpReportDate = new DateTimePicker();
            lblReportDate = new Label();
            pnlStatistics = new Panel();
            pnlAbsent = new Panel();
            lblAbsentCount = new Label();
            lblAbsentTitle = new Label();
            pnlPresent = new Panel();
            lblPresentCount = new Label();
            lblPresentTitle = new Label();
            pnlTotal = new Panel();
            lblTotalCount = new Label();
            lblTotalTitle = new Label();
            dgvPersonnelReport = new DataGridView();
            statusStrip = new StatusStrip();
            lblStatus = new ToolStripStatusLabel();
            pnlFilter.SuspendLayout();
            pnlStatistics.SuspendLayout();
            pnlAbsent.SuspendLayout();
            pnlPresent.SuspendLayout();
            pnlTotal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPersonnelReport).BeginInit();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Bold);
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(936, 45);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "BÁO CÁO QUÂN SỐ";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlFilter
            // 
            pnlFilter.Controls.Add(btnExport);
            pnlFilter.Controls.Add(btnRefresh);
            pnlFilter.Controls.Add(dtpReportDate);
            pnlFilter.Controls.Add(lblReportDate);
            pnlFilter.Dock = DockStyle.Top;
            pnlFilter.Location = new Point(0, 45);
            pnlFilter.Name = "pnlFilter";
            pnlFilter.Size = new Size(936, 70);
            pnlFilter.TabIndex = 1;
            // 
            // btnExport
            // 
            btnExport.Location = new Point(830, 17);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(94, 29);
            btnExport.TabIndex = 3;
            btnExport.Text = "Xuất Excel";
            btnExport.UseVisualStyleBackColor = true;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(730, 17);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(94, 29);
            btnRefresh.TabIndex = 2;
            btnRefresh.Text = "Làm mới";
            btnRefresh.UseVisualStyleBackColor = true;
            // 
            // dtpReportDate
            // 
            dtpReportDate.CustomFormat = "dd/MM/yyyy HH:mm";
            dtpReportDate.Format = DateTimePickerFormat.Custom;
            dtpReportDate.Location = new Point(123, 21);
            dtpReportDate.Name = "dtpReportDate";
            dtpReportDate.Size = new Size(150, 27);
            dtpReportDate.TabIndex = 1;
            // 
            // lblReportDate
            // 
            lblReportDate.AutoSize = true;
            lblReportDate.Location = new Point(12, 26);
            lblReportDate.Name = "lblReportDate";
            lblReportDate.Size = new Size(105, 20);
            lblReportDate.TabIndex = 0;
            lblReportDate.Text = "Ngày báo cáo:";
            // 
            // pnlStatistics
            // 
            pnlStatistics.Controls.Add(pnlAbsent);
            pnlStatistics.Controls.Add(pnlPresent);
            pnlStatistics.Controls.Add(pnlTotal);
            pnlStatistics.Dock = DockStyle.Top;
            pnlStatistics.Location = new Point(0, 115);
            pnlStatistics.Name = "pnlStatistics";
            pnlStatistics.Size = new Size(936, 100);
            pnlStatistics.TabIndex = 2;
            // 
            // pnlAbsent
            // 
            pnlAbsent.BackColor = Color.MistyRose;
            pnlAbsent.Controls.Add(lblAbsentCount);
            pnlAbsent.Controls.Add(lblAbsentTitle);
            pnlAbsent.Location = new Point(618, 6);
            pnlAbsent.Name = "pnlAbsent";
            pnlAbsent.Size = new Size(200, 80);
            pnlAbsent.TabIndex = 3;
            // 
            // lblAbsentCount
            // 
            lblAbsentCount.AutoSize = true;
            lblAbsentCount.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblAbsentCount.Location = new Point(131, 15);
            lblAbsentCount.Name = "lblAbsentCount";
            lblAbsentCount.Size = new Size(46, 54);
            lblAbsentCount.TabIndex = 1;
            lblAbsentCount.Text = "0";
            // 
            // lblAbsentTitle
            // 
            lblAbsentTitle.AutoSize = true;
            lblAbsentTitle.Location = new Point(6, 32);
            lblAbsentTitle.Name = "lblAbsentTitle";
            lblAbsentTitle.Size = new Size(73, 20);
            lblAbsentTitle.TabIndex = 0;
            lblAbsentTitle.Text = "Vắng mặt";
            // 
            // pnlPresent
            // 
            pnlPresent.BackColor = Color.LightGreen;
            pnlPresent.Controls.Add(lblPresentCount);
            pnlPresent.Controls.Add(lblPresentTitle);
            pnlPresent.Location = new Point(361, 6);
            pnlPresent.Name = "pnlPresent";
            pnlPresent.Size = new Size(200, 80);
            pnlPresent.TabIndex = 2;
            // 
            // lblPresentCount
            // 
            lblPresentCount.AutoSize = true;
            lblPresentCount.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblPresentCount.Location = new Point(131, 15);
            lblPresentCount.Name = "lblPresentCount";
            lblPresentCount.Size = new Size(46, 54);
            lblPresentCount.TabIndex = 1;
            lblPresentCount.Text = "0";
            // 
            // lblPresentTitle
            // 
            lblPresentTitle.AutoSize = true;
            lblPresentTitle.Location = new Point(6, 32);
            lblPresentTitle.Name = "lblPresentTitle";
            lblPresentTitle.Size = new Size(57, 20);
            lblPresentTitle.TabIndex = 0;
            lblPresentTitle.Text = "Có mặt";
            // 
            // pnlTotal
            // 
            pnlTotal.BackColor = Color.LightSteelBlue;
            pnlTotal.Controls.Add(lblTotalCount);
            pnlTotal.Controls.Add(lblTotalTitle);
            pnlTotal.Location = new Point(103, 6);
            pnlTotal.Name = "pnlTotal";
            pnlTotal.Size = new Size(200, 80);
            pnlTotal.TabIndex = 0;
            // 
            // lblTotalCount
            // 
            lblTotalCount.AutoSize = true;
            lblTotalCount.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTotalCount.Location = new Point(131, 15);
            lblTotalCount.Name = "lblTotalCount";
            lblTotalCount.Size = new Size(46, 54);
            lblTotalCount.TabIndex = 1;
            lblTotalCount.Text = "0";
            // 
            // lblTotalTitle
            // 
            lblTotalTitle.AutoSize = true;
            lblTotalTitle.Location = new Point(6, 32);
            lblTotalTitle.Name = "lblTotalTitle";
            lblTotalTitle.Size = new Size(99, 20);
            lblTotalTitle.TabIndex = 0;
            lblTotalTitle.Text = "Tổng quân số";
            // 
            // dgvPersonnelReport
            // 
            dgvPersonnelReport.AllowUserToAddRows = false;
            dgvPersonnelReport.AllowUserToDeleteRows = false;
            dgvPersonnelReport.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPersonnelReport.Dock = DockStyle.Fill;
            dgvPersonnelReport.Location = new Point(0, 215);
            dgvPersonnelReport.MultiSelect = false;
            dgvPersonnelReport.Name = "dgvPersonnelReport";
            dgvPersonnelReport.ReadOnly = true;
            dgvPersonnelReport.RowHeadersVisible = false;
            dgvPersonnelReport.RowHeadersWidth = 51;
            dgvPersonnelReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPersonnelReport.Size = new Size(936, 466);
            dgvPersonnelReport.TabIndex = 3;
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(20, 20);
            statusStrip.Items.AddRange(new ToolStripItem[] { lblStatus });
            statusStrip.Location = new Point(0, 655);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(936, 26);
            statusStrip.TabIndex = 4;
            statusStrip.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(68, 20);
            lblStatus.Text = "Sẵn sàng";
            // 
            // FrmPersonnelReport
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(936, 681);
            Controls.Add(statusStrip);
            Controls.Add(dgvPersonnelReport);
            Controls.Add(pnlStatistics);
            Controls.Add(pnlFilter);
            Controls.Add(lblTitle);
            Name = "FrmPersonnelReport";
            Text = "FrmPersonnelReport";
            Load += FrmPersonnelReport_Load;
            pnlFilter.ResumeLayout(false);
            pnlFilter.PerformLayout();
            pnlStatistics.ResumeLayout(false);
            pnlAbsent.ResumeLayout(false);
            pnlAbsent.PerformLayout();
            pnlPresent.ResumeLayout(false);
            pnlPresent.PerformLayout();
            pnlTotal.ResumeLayout(false);
            pnlTotal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPersonnelReport).EndInit();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Panel pnlFilter;
        private Button btnExport;
        private Button btnRefresh;
        private DateTimePicker dtpReportDate;
        private Label lblReportDate;
        private Panel pnlStatistics;
        private Panel pnlTotal;
        private Label lblTotalCount;
        private Label lblTotalTitle;
        private Panel pnlPresent;
        private Label lblPresentCount;
        private Label lblPresentTitle;
        private Panel pnlAbsent;
        private Label lblAbsentCount;
        private Label lblAbsentTitle;
        private DataGridView dgvPersonnelReport;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;
    }
}