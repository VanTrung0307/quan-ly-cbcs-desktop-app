namespace QuanLyCBCS.Forms.PersonnelForms
{
    partial class FrmPersonnelReport
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblHeaderIcon = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.pnlFilter = new System.Windows.Forms.Panel();
            this.pnlFilterCard = new System.Windows.Forms.Panel();
            this.lblReportDate = new System.Windows.Forms.Label();
            this.lblReportDateIcon = new System.Windows.Forms.Label();
            this.dtpReportDate = new BorderlessDateTimePicker();
            this.pnlDatePicker = new System.Windows.Forms.Panel();
            this.btnRefresh = new RoundedOutlineButton();
            this.btnPreview = new EyePreviewButton();
            this.pnlStatistics = new System.Windows.Forms.TableLayoutPanel();
            this.pnlTotal = new System.Windows.Forms.Panel();
            this.pnlPresent = new System.Windows.Forms.Panel();
            this.pnlAbsent = new System.Windows.Forms.Panel();
            this.pnlGridHost = new System.Windows.Forms.Panel();
            this.dgvPersonnelReport = new System.Windows.Forms.DataGridView();
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.lblStatusIcon = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            this.pnlStatistics.SuspendLayout();
            this.pnlTotal.SuspendLayout();
            this.pnlPresent.SuspendLayout();
            this.pnlAbsent.SuspendLayout();
            this.pnlGridHost.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPersonnelReport)).BeginInit();
            this.pnlStatus.SuspendLayout();
            this.SuspendLayout();

            // Header banner
            this.pnlHeader.Controls.Add(this.lblHeaderIcon);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblSubtitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Size = new System.Drawing.Size(1365, 92);
            this.pnlHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlHeader_Paint);

            this.lblHeaderIcon.BackColor = System.Drawing.Color.Transparent;
            this.lblHeaderIcon.Font = new System.Drawing.Font("Segoe UI Symbol", 34F);
            this.lblHeaderIcon.ForeColor = System.Drawing.Color.White;
            this.lblHeaderIcon.Location = new System.Drawing.Point(24, 15);
            this.lblHeaderIcon.Size = new System.Drawing.Size(74, 68);
            this.lblHeaderIcon.Visible = false;
            this.lblHeaderIcon.Text = "";
            this.lblHeaderIcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.lblTitle.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 23F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(120, 10);
            this.lblTitle.Size = new System.Drawing.Size(1100, 43);
            this.lblTitle.Text = "BÁO CÁO QUÂN SỐ";

            this.lblSubtitle.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.lblSubtitle.BackColor = System.Drawing.Color.Transparent;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.White;
            this.lblSubtitle.Location = new System.Drawing.Point(120, 52);
            this.lblSubtitle.Size = new System.Drawing.Size(1100, 27);
            this.lblSubtitle.Text = "Thống kê, báo cáo tình hình quân số CBCS";

            // Filter strip
            this.pnlFilter.BackColor = System.Drawing.Color.FromArgb(245, 249, 255);
            this.pnlFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilter.Size = new System.Drawing.Size(1365, 72);
            this.pnlFilter.Controls.Add(this.pnlFilterCard);

            // Inner rounded card: its solid BackColor also prevents square
            // artifacts around the owner-drawn buttons' transparent corners.
            this.pnlFilterCard.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.pnlFilterCard.BackColor = System.Drawing.Color.FromArgb(234, 244, 255);
            this.pnlFilterCard.Location = new System.Drawing.Point(7, 7);
            this.pnlFilterCard.Size = new System.Drawing.Size(1351, 58);
            this.pnlFilterCard.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlFilterCard_Paint);
            this.pnlFilterCard.Controls.Add(this.lblReportDate);
            this.pnlFilterCard.Controls.Add(this.lblReportDateIcon);
            this.pnlFilterCard.Controls.Add(this.pnlDatePicker);
            this.pnlFilterCard.Controls.Add(this.btnRefresh);
            this.pnlFilterCard.Controls.Add(this.btnPreview);
            this.pnlDatePicker.Controls.Add(this.dtpReportDate);

            this.lblReportDate.AutoSize = false;
            this.lblReportDate.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblReportDate.ForeColor = System.Drawing.Color.FromArgb(15, 45, 82);
            this.lblReportDate.BackColor = System.Drawing.Color.Transparent;
            this.lblReportDate.Location = new System.Drawing.Point(55, 10);
            this.lblReportDate.Size = new System.Drawing.Size(148, 38);
            this.lblReportDate.Text = "Ngày báo cáo:";
            this.lblReportDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // Calendar icon beside the report-date label (drawn as a crisp vector).
            this.lblReportDateIcon.BackColor = System.Drawing.Color.Transparent;
            this.lblReportDateIcon.Location = new System.Drawing.Point(15, 9);
            this.lblReportDateIcon.Size = new System.Drawing.Size(30, 38);
            this.lblReportDateIcon.Paint += new System.Windows.Forms.PaintEventHandler(this.lblReportDateIcon_Paint);

            // Rounded white date field; the native DateTimePicker remains active
            // so calendar selection and time editing keep their original behavior.
            this.pnlDatePicker.BackColor = System.Drawing.Color.White;
            this.pnlDatePicker.Location = new System.Drawing.Point(213, 8);
            this.pnlDatePicker.Size = new System.Drawing.Size(250, 42);
            this.pnlDatePicker.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlDatePicker_Paint);

            this.dtpReportDate.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpReportDate.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.dtpReportDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpReportDate.Location = new System.Drawing.Point(8, 5);
            this.dtpReportDate.Size = new System.Drawing.Size(232, 32);
            this.dtpReportDate.CalendarFont = new System.Drawing.Font("Segoe UI", 10F);

            this.btnRefresh.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnRefresh.BackColor = System.Drawing.Color.White;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnRefresh.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(242, 248, 255);
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.FromArgb(0, 91, 195);
            this.btnRefresh.TabStop = false;
            this.btnRefresh.Location = new System.Drawing.Point(998, 8);
            this.btnRefresh.Size = new System.Drawing.Size(140, 42);
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.Text = "↻  Làm mới";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            this.btnPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnPreview.BackColor = System.Drawing.Color.FromArgb(0, 102, 224);
            this.btnPreview.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPreview.FlatAppearance.BorderSize = 0;
            this.btnPreview.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(0, 83, 190);
            this.btnPreview.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPreview.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnPreview.ForeColor = System.Drawing.Color.White;
            this.btnPreview.Location = new System.Drawing.Point(1148, 8);
            this.btnPreview.Size = new System.Drawing.Size(190, 42);
            this.btnPreview.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnPreview.Text = "Xem báo cáo";
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);

            // Statistics cards
            this.pnlStatistics.BackColor = System.Drawing.Color.FromArgb(245, 249, 255);
            this.pnlStatistics.ColumnCount = 3;
            this.pnlStatistics.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.333F));
            this.pnlStatistics.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.333F));
            this.pnlStatistics.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.334F));
            this.pnlStatistics.Controls.Add(this.pnlTotal, 0, 0);
            this.pnlStatistics.Controls.Add(this.pnlPresent, 1, 0);
            this.pnlStatistics.Controls.Add(this.pnlAbsent, 2, 0);
            this.pnlStatistics.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStatistics.Padding = new System.Windows.Forms.Padding(10, 10, 10, 15);
            this.pnlStatistics.RowCount = 1;
            this.pnlStatistics.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlStatistics.Size = new System.Drawing.Size(1365, 138);

            SetupCard(this.pnlTotal, System.Drawing.Color.FromArgb(218, 234, 255), 0);
            SetupCard(this.pnlPresent, System.Drawing.Color.FromArgb(218, 250, 232), 1);
            SetupCard(this.pnlAbsent, System.Drawing.Color.FromArgb(255, 231, 235), 2);

            // Data grid
            this.dgvPersonnelReport.AllowUserToAddRows = false;
            this.dgvPersonnelReport.AllowUserToDeleteRows = false;
            this.dgvPersonnelReport.AllowUserToResizeRows = false;
            this.dgvPersonnelReport.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPersonnelReport.BackgroundColor = System.Drawing.Color.White;
            this.dgvPersonnelReport.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPersonnelReport.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Single;
            this.dgvPersonnelReport.ColumnHeadersHeight = 41;
            this.dgvPersonnelReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvPersonnelReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPersonnelReport.Margin = new System.Windows.Forms.Padding(0);
            this.dgvPersonnelReport.MultiSelect = false;
            this.dgvPersonnelReport.ReadOnly = true;
            this.dgvPersonnelReport.RowHeadersVisible = false;
            this.dgvPersonnelReport.RowTemplate.Height = 40;
            this.dgvPersonnelReport.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.None;
            this.dgvPersonnelReport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            // Khung bảng tạo khoảng lề và nền ngoài giống card trong bản thiết kế.
            this.pnlGridHost.BackColor = System.Drawing.Color.FromArgb(245, 249, 255);
            this.pnlGridHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGridHost.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.pnlGridHost.Controls.Add(this.dgvPersonnelReport);

            // Bottom status: thanh thông tin dạng thẻ bo góc, thụt lề hai bên.
            this.pnlStatus.BackColor = System.Drawing.Color.FromArgb(245, 249, 255);
            this.pnlStatus.Controls.Add(this.lblStatusIcon);
            this.pnlStatus.Controls.Add(this.lblStatus);
            this.pnlStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlStatus.Size = new System.Drawing.Size(1365, 52);
            this.pnlStatus.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlStatus_Paint);

            this.lblStatusIcon.BackColor = System.Drawing.Color.Transparent;
            this.lblStatusIcon.Font = new System.Drawing.Font("Segoe UI Symbol", 15F, System.Drawing.FontStyle.Bold);
            this.lblStatusIcon.ForeColor = System.Drawing.Color.FromArgb(0, 104, 220);
            this.lblStatusIcon.Location = new System.Drawing.Point(23, 10);
            this.lblStatusIcon.Size = new System.Drawing.Size(30, 30);
            this.lblStatusIcon.Text = "ⓘ";
            this.lblStatusIcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.lblStatus.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(15, 54, 97);
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.Location = new System.Drawing.Point(61, 10);
            this.lblStatus.Size = new System.Drawing.Size(1260, 30);
            this.lblStatus.Text = "Đang hiển thị: 0 CBCS";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 249, 255);
            this.ClientSize = new System.Drawing.Size(1365, 766);
            this.Controls.Add(this.pnlGridHost);
            this.Controls.Add(this.pnlStatus);
            this.Controls.Add(this.pnlStatistics);
            this.Controls.Add(this.pnlFilter);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Name = "FrmPersonnelReport";
            this.Text = "Báo cáo quân số";
            this.Load += new System.EventHandler(this.FrmPersonnelReport_Load);

            this.pnlHeader.ResumeLayout(false);
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            this.pnlStatistics.ResumeLayout(false);
            this.pnlTotal.ResumeLayout(false);
            this.pnlPresent.ResumeLayout(false);
            this.pnlAbsent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPersonnelReport)).EndInit();
            this.pnlGridHost.ResumeLayout(false);
            this.pnlStatus.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void SetupCard(System.Windows.Forms.Panel panel, System.Drawing.Color color, int type)
        {
            panel.BackColor = color;
            panel.Dock = System.Windows.Forms.DockStyle.Fill;
            panel.Margin = type < 2
                ? new System.Windows.Forms.Padding(4, 3, 22, 3)
                : new System.Windows.Forms.Padding(22, 3, 4, 3);
            panel.Controls.Clear();

            // Icon được vẽ bằng GDI+ trong pnlCard_Paint để hiển thị đồng nhất.
            System.Windows.Forms.Label icon = new System.Windows.Forms.Label();
            icon.BackColor = System.Drawing.Color.Transparent;
            icon.Visible = false;
            icon.Name = type == 0 ? "lblTotalIcon" : type == 1 ? "lblPresentIcon" : "lblAbsentIcon";

            System.Windows.Forms.Label title = new System.Windows.Forms.Label();
            title.BackColor = System.Drawing.Color.Transparent;
            title.Font = new System.Drawing.Font("Segoe UI", 12.5F, System.Drawing.FontStyle.Bold);
            title.ForeColor = System.Drawing.Color.FromArgb(13, 43, 78);
            title.Location = new System.Drawing.Point(130, 25);
            title.Size = new System.Drawing.Size(180, 36);
            title.Text = type == 0 ? "Tổng quân số" : type == 1 ? "Có mặt" : "Vắng mặt";
            title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            title.Name = type == 0 ? "lblTotalTitle" : type == 1 ? "lblPresentTitle" : "lblAbsentTitle";

            System.Windows.Forms.Label count = new System.Windows.Forms.Label();
            count.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            count.AutoSize = false;
            count.BackColor = System.Drawing.Color.Transparent;
            count.Font = new System.Drawing.Font("Segoe UI", 30F, System.Drawing.FontStyle.Bold);
            count.ForeColor = System.Drawing.Color.FromArgb(10, 31, 62);
            count.Location = new System.Drawing.Point(panel.Width - 132, 8);
            count.Size = new System.Drawing.Size(88, 70);
            count.Text = "0";
            count.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            count.Name = type == 0 ? "lblTotalCount" : type == 1 ? "lblPresentCount" : "lblAbsentCount";

            panel.Controls.Add(icon);
            panel.Controls.Add(title);
            panel.Controls.Add(count);
            panel.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlCard_Paint);
            panel.Resize += (sender, args) =>
            {
                int dividerX = panel.Width - 176;
                title.Location = new System.Drawing.Point(130, Math.Max(0, (panel.Height - 36) / 2));
                title.Size = new System.Drawing.Size(Math.Max(100, dividerX - 145), 36);
                count.Location = new System.Drawing.Point(dividerX + (panel.Width - dividerX - 88) / 2,
                    Math.Max(0, (panel.Height - 70) / 2));
                count.Size = new System.Drawing.Size(88, 70);
            };

            if (type == 0) { this.lblTotalIcon = icon; this.lblTotalTitle = title; this.lblTotalCount = count; }
            else if (type == 1) { this.lblPresentIcon = icon; this.lblPresentTitle = title; this.lblPresentCount = count; }
            else { this.lblAbsentIcon = icon; this.lblAbsentTitle = title; this.lblAbsentCount = count; }
        }

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeaderIcon, lblTitle, lblSubtitle;
        private System.Windows.Forms.Panel pnlFilter;
        private System.Windows.Forms.Panel pnlFilterCard;
        private System.Windows.Forms.Label lblReportDate;
        private System.Windows.Forms.Label lblReportDateIcon;
        private BorderlessDateTimePicker dtpReportDate;
        private System.Windows.Forms.Panel pnlDatePicker;
        private RoundedOutlineButton btnRefresh;
        private EyePreviewButton btnPreview;
        private System.Windows.Forms.TableLayoutPanel pnlStatistics;
        private System.Windows.Forms.Panel pnlTotal, pnlPresent, pnlAbsent;
        private System.Windows.Forms.Label lblTotalIcon, lblTotalTitle, lblTotalCount;
        private System.Windows.Forms.Label lblPresentIcon, lblPresentTitle, lblPresentCount;
        private System.Windows.Forms.Label lblAbsentIcon, lblAbsentTitle, lblAbsentCount;
        private System.Windows.Forms.Panel pnlGridHost;
        private System.Windows.Forms.DataGridView dgvPersonnelReport;
        private System.Windows.Forms.Panel pnlStatus;
        private System.Windows.Forms.Label lblStatusIcon, lblStatus;
    }
}