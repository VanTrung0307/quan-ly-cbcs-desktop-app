using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyCBCS.Forms.MissionForms
{
    partial class FrmPersonnelMissionHistory
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPersonnelMissionHistory));
            rootLayout = new TableLayoutPanel();
            pnlHeader = new Panel();
            lblHeaderIcon = new Label();
            lblTitle = new Label();
            lblSubtitle = new Label();
            toolbar = new TableLayoutPanel();
            pnlPersonnelInfo = new Panel();
            lblPersonnelInfo = new Label();
            pnlToolbarSeparator = new Panel();
            pnlSearch = new Panel();
            txtSearch = new TextBox();
            lblSearchIcon = new Label();
            btnExcel = new Button();
            btnRefresh = new Button();
            btnClose = new Button();
            pnlGridCard = new Panel();
            dgvHistory = new DataGridView();
            colStt = new DataGridViewTextBoxColumn();
            colNgayPhanCong = new DataGridViewTextBoxColumn();
            colTenCongTac = new DataGridViewTextBoxColumn();
            colLoaiCongTac = new DataGridViewTextBoxColumn();
            colGhiChu = new DataGridViewTextBoxColumn();
            colDiaDiem = new DataGridViewTextBoxColumn();
            pnlFooter = new Panel();
            lblTotal = new Label();
            lblFooterIcon = new Label();
            rootLayout.SuspendLayout();
            pnlHeader.SuspendLayout();
            toolbar.SuspendLayout();
            pnlPersonnelInfo.SuspendLayout();
            pnlSearch.SuspendLayout();
            pnlGridCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHistory).BeginInit();
            pnlFooter.SuspendLayout();
            SuspendLayout();
            // 
            // rootLayout
            // 
            rootLayout.BackColor = Color.FromArgb(242, 247, 253);
            rootLayout.ColumnCount = 1;
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rootLayout.Controls.Add(pnlHeader, 0, 0);
            rootLayout.Controls.Add(toolbar, 0, 1);
            rootLayout.Controls.Add(pnlGridCard, 0, 2);
            rootLayout.Controls.Add(pnlFooter, 0, 3);
            rootLayout.Dock = DockStyle.Fill;
            rootLayout.Location = new Point(0, 0);
            rootLayout.Name = "rootLayout";
            rootLayout.Padding = new Padding(16);
            rootLayout.RowCount = 4;
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 120F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 88F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
            rootLayout.Size = new Size(1460, 746);
            rootLayout.TabIndex = 0;
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(0, 83, 166);
            pnlHeader.Controls.Add(lblHeaderIcon);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSubtitle);
            pnlHeader.Dock = DockStyle.Fill;
            pnlHeader.Location = new Point(16, 16);
            pnlHeader.Margin = new Padding(0, 0, 0, 16);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1428, 104);
            pnlHeader.TabIndex = 0;
            pnlHeader.Paint += PnlHeader_Paint;
            // 
            // lblHeaderIcon
            // 
            lblHeaderIcon.BackColor = Color.Transparent;
            lblHeaderIcon.Font = new Font("Segoe Fluent Icons", 40F);
            lblHeaderIcon.ForeColor = Color.White;
            lblHeaderIcon.Location = new Point(30, 19);
            lblHeaderIcon.Name = "lblHeaderIcon";
            lblHeaderIcon.Size = new Size(82, 68);
            lblHeaderIcon.TabIndex = 0;
            lblHeaderIcon.Text = "";
            lblHeaderIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            lblTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(120, 17);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(2128, 42);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "LỊCH SỬ CÔNG TÁC CBCS";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblSubtitle
            // 
            lblSubtitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblSubtitle.BackColor = Color.Transparent;
            lblSubtitle.Font = new Font("Segoe UI", 12F);
            lblSubtitle.ForeColor = Color.FromArgb(235, 245, 255);
            lblSubtitle.Location = new Point(122, 59);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(2128, 27);
            lblSubtitle.TabIndex = 2;
            lblSubtitle.Text = "Tra cứu lịch sử công tác, phân công nhiệm vụ của CBCS";
            lblSubtitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // toolbar
            // 
            toolbar.BackColor = Color.FromArgb(242, 247, 253);
            toolbar.ColumnCount = 6;
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 18F));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.66F));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.66F));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.68F));
            toolbar.Controls.Add(pnlPersonnelInfo, 0, 0);
            toolbar.Controls.Add(pnlToolbarSeparator, 1, 0);
            toolbar.Controls.Add(pnlSearch, 2, 0);
            toolbar.Controls.Add(btnExcel, 3, 0);
            toolbar.Controls.Add(btnRefresh, 4, 0);
            toolbar.Controls.Add(btnClose, 5, 0);
            toolbar.Dock = DockStyle.Fill;
            toolbar.Location = new Point(16, 136);
            toolbar.Margin = new Padding(0, 0, 0, 16);
            toolbar.Name = "toolbar";
            toolbar.RowCount = 1;
            toolbar.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            toolbar.Size = new Size(1428, 72);
            toolbar.TabIndex = 1;
            // 
            // pnlPersonnelInfo
            // 
            pnlPersonnelInfo.BackColor = Color.FromArgb(242, 247, 253);
            pnlPersonnelInfo.Controls.Add(lblPersonnelInfo);
            pnlPersonnelInfo.Dock = DockStyle.Fill;
            pnlPersonnelInfo.Location = new Point(0, 8);
            pnlPersonnelInfo.Margin = new Padding(0, 8, 10, 8);
            pnlPersonnelInfo.Name = "pnlPersonnelInfo";
            pnlPersonnelInfo.Size = new Size(342, 56);
            pnlPersonnelInfo.TabIndex = 0;
            pnlPersonnelInfo.Paint += RoundedCard_Paint;
            // 
            // lblPersonnelInfo
            // 
            lblPersonnelInfo.BackColor = Color.Transparent;
            lblPersonnelInfo.Dock = DockStyle.Fill;
            lblPersonnelInfo.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblPersonnelInfo.ForeColor = Color.FromArgb(9, 37, 79);
            lblPersonnelInfo.Location = new Point(0, 0);
            lblPersonnelInfo.Name = "lblPersonnelInfo";
            lblPersonnelInfo.Padding = new Padding(52, 0, 8, 0);
            lblPersonnelInfo.Size = new Size(342, 56);
            lblPersonnelInfo.TabIndex = 0;
            lblPersonnelInfo.Text = "CBCS:";
            lblPersonnelInfo.TextAlign = ContentAlignment.MiddleLeft;
            lblPersonnelInfo.Paint += PersonnelInfoIcon_Paint;
            // 
            // pnlToolbarSeparator
            // 
            pnlToolbarSeparator.BackColor = Color.Transparent;
            pnlToolbarSeparator.Dock = DockStyle.Fill;
            pnlToolbarSeparator.Location = new Point(352, 0);
            pnlToolbarSeparator.Margin = new Padding(0);
            pnlToolbarSeparator.Name = "pnlToolbarSeparator";
            pnlToolbarSeparator.Size = new Size(18, 72);
            pnlToolbarSeparator.TabIndex = 1;
            pnlToolbarSeparator.Paint += ToolbarSeparator_Paint;
            // 
            // pnlSearch
            // 
            pnlSearch.BackColor = Color.FromArgb(242, 247, 253);
            pnlSearch.Controls.Add(txtSearch);
            pnlSearch.Controls.Add(lblSearchIcon);
            pnlSearch.Dock = DockStyle.Fill;
            pnlSearch.Location = new Point(380, 8);
            pnlSearch.Margin = new Padding(10, 8, 10, 8);
            pnlSearch.Name = "pnlSearch";
            pnlSearch.Size = new Size(543, 56);
            pnlSearch.TabIndex = 2;
            pnlSearch.Paint += RoundedSearch_Paint;
            // 
            // txtSearch
            // 
            txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtSearch.BorderStyle = BorderStyle.None;
            txtSearch.Font = new Font("Segoe UI", 12F);
            txtSearch.Location = new Point(56, 13);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Tìm theo tên công tác, nhiệm vụ...";
            txtSearch.Size = new Size(643, 27);
            txtSearch.TabIndex = 0;
            // 
            // lblSearchIcon
            // 
            lblSearchIcon.BackColor = Color.Transparent;
            lblSearchIcon.Font = new Font("Segoe Fluent Icons", 18F);
            lblSearchIcon.ForeColor = Color.FromArgb(0, 91, 190);
            lblSearchIcon.Location = new Point(15, 5);
            lblSearchIcon.Name = "lblSearchIcon";
            lblSearchIcon.Size = new Size(34, 40);
            lblSearchIcon.TabIndex = 1;
            lblSearchIcon.Text = "";
            lblSearchIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnExcel
            // 
            btnExcel.Location = new Point(936, 3);
            btnExcel.Name = "btnExcel";
            btnExcel.Size = new Size(75, 23);
            btnExcel.TabIndex = 3;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(1100, 3);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(75, 23);
            btnRefresh.TabIndex = 4;
            // 
            // btnClose
            // 
            btnClose.Location = new Point(1264, 3);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(75, 23);
            btnClose.TabIndex = 5;
            // 
            // pnlGridCard
            // 
            pnlGridCard.BackColor = Color.FromArgb(242, 247, 253);
            pnlGridCard.Controls.Add(dgvHistory);
            pnlGridCard.Dock = DockStyle.Fill;
            pnlGridCard.Location = new Point(16, 224);
            pnlGridCard.Margin = new Padding(0);
            pnlGridCard.Name = "pnlGridCard";
            pnlGridCard.Padding = new Padding(2);
            pnlGridCard.Size = new Size(1428, 434);
            pnlGridCard.TabIndex = 2;
            pnlGridCard.Paint += GridCard_Paint;
            // 
            // dgvHistory
            // 
            dgvHistory.AllowUserToAddRows = false;
            dgvHistory.AllowUserToDeleteRows = false;
            dgvHistory.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(245, 250, 255);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(151, 215, 248);
            dgvHistory.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvHistory.BackgroundColor = Color.FromArgb(242, 247, 253);
            dgvHistory.BorderStyle = BorderStyle.None;
            dgvHistory.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(0, 69, 145);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.Padding = new Padding(5);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(0, 69, 145);
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvHistory.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvHistory.ColumnHeadersHeight = 50;
            dgvHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvHistory.Columns.AddRange(new DataGridViewColumn[] { colStt, colNgayPhanCong, colTenCongTac, colLoaiCongTac, colGhiChu, colDiaDiem });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 12F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(10, 35, 72);
            dataGridViewCellStyle3.Padding = new Padding(12, 0, 8, 0);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(151, 215, 248);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(10, 35, 72);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvHistory.DefaultCellStyle = dataGridViewCellStyle3;
            dgvHistory.Dock = DockStyle.Fill;
            dgvHistory.EnableHeadersVisualStyles = false;
            dgvHistory.GridColor = Color.FromArgb(174, 202, 232);
            dgvHistory.Location = new Point(2, 2);
            dgvHistory.MultiSelect = false;
            dgvHistory.Name = "dgvHistory";
            dgvHistory.ReadOnly = true;
            dgvHistory.RowHeadersVisible = false;
            dgvHistory.RowHeadersWidth = 51;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(165, 220, 248);
            dgvHistory.RowsDefaultCellStyle = dataGridViewCellStyle4;
            dgvHistory.RowTemplate.Height = 54;
            dgvHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHistory.Size = new Size(1424, 430);
            dgvHistory.TabIndex = 0;
            dgvHistory.CellPainting += DgvHistory_CellPainting;
            dgvHistory.DataError += DgvHistory_DataError;
            // 
            // colStt
            // 
            colStt.MinimumWidth = 6;
            colStt.Name = "colStt";
            colStt.ReadOnly = true;
            colStt.Width = 125;
            // 
            // colNgayPhanCong
            // 
            colNgayPhanCong.MinimumWidth = 6;
            colNgayPhanCong.Name = "colNgayPhanCong";
            colNgayPhanCong.ReadOnly = true;
            colNgayPhanCong.Width = 125;
            // 
            // colTenCongTac
            // 
            colTenCongTac.MinimumWidth = 6;
            colTenCongTac.Name = "colTenCongTac";
            colTenCongTac.ReadOnly = true;
            colTenCongTac.Width = 125;
            // 
            // colLoaiCongTac
            // 
            colLoaiCongTac.MinimumWidth = 6;
            colLoaiCongTac.Name = "colLoaiCongTac";
            colLoaiCongTac.ReadOnly = true;
            colLoaiCongTac.Width = 125;
            // 
            // colGhiChu
            // 
            colGhiChu.MinimumWidth = 6;
            colGhiChu.Name = "colGhiChu";
            colGhiChu.ReadOnly = true;
            colGhiChu.Width = 125;
            // 
            // colDiaDiem
            // 
            colDiaDiem.MinimumWidth = 6;
            colDiaDiem.Name = "colDiaDiem";
            colDiaDiem.ReadOnly = true;
            colDiaDiem.Width = 125;
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = Color.FromArgb(242, 247, 253);
            pnlFooter.Controls.Add(lblTotal);
            pnlFooter.Controls.Add(lblFooterIcon);
            pnlFooter.Dock = DockStyle.Fill;
            pnlFooter.Location = new Point(16, 674);
            pnlFooter.Margin = new Padding(0, 16, 0, 0);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Size = new Size(1428, 56);
            pnlFooter.TabIndex = 3;
            pnlFooter.Paint += RoundedFooter_Paint;
            // 
            // lblTotal
            // 
            lblTotal.BackColor = Color.Transparent;
            lblTotal.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblTotal.ForeColor = Color.FromArgb(9, 37, 79);
            lblTotal.Location = new Point(72, 0);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(420, 56);
            lblTotal.TabIndex = 0;
            lblTotal.Text = "Tổng: 0 bản ghi";
            lblTotal.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblFooterIcon
            // 
            lblFooterIcon.BackColor = Color.Transparent;
            lblFooterIcon.Font = new Font("Segoe Fluent Icons", 22F);
            lblFooterIcon.ForeColor = Color.FromArgb(0, 91, 190);
            lblFooterIcon.Location = new Point(18, 8);
            lblFooterIcon.Name = "lblFooterIcon";
            lblFooterIcon.Size = new Size(42, 40);
            lblFooterIcon.TabIndex = 1;
            lblFooterIcon.Text = "";
            lblFooterIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FrmPersonnelMissionHistory
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(242, 247, 253);
            ClientSize = new Size(1460, 746);
            Controls.Add(rootLayout);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(1120, 650);
            Name = "FrmPersonnelMissionHistory";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Lịch sử Công tác của CBCS";
            rootLayout.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            toolbar.ResumeLayout(false);
            pnlPersonnelInfo.ResumeLayout(false);
            pnlSearch.ResumeLayout(false);
            pnlSearch.PerformLayout();
            pnlGridCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvHistory).EndInit();
            pnlFooter.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void ConfigureToolbarButton(Button button, string kind, string caption)
        {
            button.BackColor = kind == "excel" ? Color.FromArgb(0, 169, 86) :
                kind == "close" ? Color.White : Color.White;
            button.Cursor = Cursors.Hand;
            button.Dock = DockStyle.Fill;
            button.FlatAppearance.BorderSize = 0;
            button.FlatStyle = FlatStyle.Flat;
            button.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            button.ForeColor = kind == "excel" ? Color.White :
                kind == "close" ? Color.FromArgb(20, 43, 75) : Color.FromArgb(0, 82, 174);
            button.Margin = new Padding(8, 8, 8, 8);
            button.Name = kind == "excel" ? "btnExcel" : kind == "refresh" ? "btnRefresh" : "btnClose";
            button.Tag = kind;
            button.Text = caption;
            button.UseVisualStyleBackColor = false;
            button.Paint += ToolbarButton_Paint;
            button.MouseEnter += (_, _) => button.Invalidate();
            button.MouseLeave += (_, _) => button.Invalidate();
        }

        private void ConfigureColumn(
            DataGridViewTextBoxColumn column,
            string header,
            string property,
            float weight,
            DataGridViewContentAlignment alignment)
        {
            column.DataPropertyName = property;
            column.HeaderText = header;
            column.MinimumWidth = 55;
            column.Name = property == "" ? "colStt" :
                property == "NgayPhanCong" ? "colNgayPhanCong" :
                property == "TenCongTac" ? "colTenCongTac" :
                property == "LoaiCongTac" ? "colLoaiCongTac" :
                property == "GhiChu" ? "colGhiChu" : "colDiaDiem";
            column.ReadOnly = true;
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            column.FillWeight = weight;
            column.DefaultCellStyle.Alignment = alignment;
            if (property == "")
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private TableLayoutPanel rootLayout;
        private Panel pnlHeader;
        private Label lblHeaderIcon;
        private Label lblTitle;
        private Label lblSubtitle;
        private TableLayoutPanel toolbar;
        private Panel pnlPersonnelInfo;
        private Label lblPersonnelInfo;
        private Panel pnlToolbarSeparator;
        private Panel pnlSearch;
        private Label lblSearchIcon;
        private TextBox txtSearch;
        private Button btnRefresh;
        private Button btnExcel;
        private Button btnClose;
        private Panel pnlGridCard;
        private DataGridView dgvHistory;
        private DataGridViewTextBoxColumn colStt;
        private DataGridViewTextBoxColumn colNgayPhanCong;
        private DataGridViewTextBoxColumn colTenCongTac;
        private DataGridViewTextBoxColumn colLoaiCongTac;
        private DataGridViewTextBoxColumn colGhiChu;
        private DataGridViewTextBoxColumn colDiaDiem;
        private Panel pnlFooter;
        private Label lblFooterIcon;
        private Label lblTotal;
    }
}
