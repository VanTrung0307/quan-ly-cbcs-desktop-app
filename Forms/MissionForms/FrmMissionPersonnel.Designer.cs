using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace QuanLyCBCS.Forms.MissionForms
{
    partial class FrmMissionPersonnel
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlHeader = new GradientHeaderPanel();
            lblHeaderIcon = new PeopleIconPanel();
            lblTitle = new Label();
            lblMissionName = new Label();
            lblMissionId = new Label();
            pnlSearch = new RoundedPanel();
            lblSearch = new Label();
            pnlSearchInput = new RoundedPanel();
            txtSearch = new TextBox();
            bodyLayout = new TableLayoutPanel();
            pnlAvailableCard = new RoundedPanel();
            availableCardLayout = new TableLayoutPanel();
            pnlAvailableTitle = new RoundedPanel();
            lblAvailableIcon = new Label();
            lblAvailableTitle = new Label();
            pnlAvailableGrid = new RoundedPanel();
            dgvAvailable = new FramelessDataGridView();
            colSttAvailable = new DataGridViewTextBoxColumn();
            colSoHieuAvailable = new DataGridViewTextBoxColumn();
            colHoTenAvailable = new DataGridViewTextBoxColumn();
            colDonViAvailable = new DataGridViewTextBoxColumn();
            colCapBacAvailable = new DataGridViewTextBoxColumn();
            colSoLanCT = new DataGridViewTextBoxColumn();
            centerPanel = new Panel();
            btnRefresh = new RoundedActionButton();
            pnlAction = new Panel();
            btnAdd = new ChevronButton { Direction = ChevronDirection.Right };
            btnRemove = new ChevronButton { Direction = ChevronDirection.Left };
            btnClose = new RoundedActionButton();
            pnlAssignedCard = new RoundedPanel();
            assignedCardLayout = new TableLayoutPanel();
            pnlAssignedTitle = new RoundedPanel();
            lblAssignedIcon = new Label();
            lblAssignedTitle = new Label();
            pnlAssignedGrid = new RoundedPanel();
            dgvAssigned = new FramelessDataGridView();
            colSttAssigned = new DataGridViewTextBoxColumn();
            colSoHieuAssigned = new DataGridViewTextBoxColumn();
            colHoTenAssigned = new DataGridViewTextBoxColumn();
            colSoLanCTAssigned = new DataGridViewTextBoxColumn();
            colSDonViAssigned = new DataGridViewTextBoxColumn();
            colCapBacHamAssigned = new DataGridViewTextBoxColumn();
            colNgayPhanCongAssigned = new DataGridViewTextBoxColumn();
            pnlAssignedEmpty = new Panel();
            lblAssignedEmptyIcon = new ClipboardPeopleIcon();
            lblAssignedEmptyText = new Label();
            footerLayout = new TableLayoutPanel();
            var searchHost = new Panel();
            lblAvailableCount = new Label();
            lblAssignedCount = new Label();

            ((System.ComponentModel.ISupportInitialize)dgvAvailable).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvAssigned).BeginInit();
            pnlHeader.SuspendLayout();
            pnlSearch.SuspendLayout();
            pnlSearchInput.SuspendLayout();
            bodyLayout.SuspendLayout();
            pnlAvailableCard.SuspendLayout();
            pnlAvailableTitle.SuspendLayout();
            pnlAvailableGrid.SuspendLayout();
            centerPanel.SuspendLayout();
            pnlAction.SuspendLayout();
            pnlAssignedCard.SuspendLayout();
            pnlAssignedTitle.SuspendLayout();
            pnlAssignedGrid.SuspendLayout();
            footerLayout.SuspendLayout();
            SuspendLayout();

            // HEADER: logo nhóm người lớn; tiêu đề và tên công tác cùng trục trái.
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 80;
            pnlHeader.Padding = new Padding(14, 4, 14, 4);
            pnlHeader.StartColor = Color.FromArgb(4, 49, 105);
            pnlHeader.EndColor = Color.FromArgb(0, 88, 184);
            pnlHeader.Controls.Add(lblHeaderIcon);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblMissionName);
            pnlHeader.Controls.Add(lblMissionId);

            lblHeaderIcon.SetBounds(12, 5, 70, 66);
            lblHeaderIcon.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;

            lblTitle.SetBounds(94, 3, 760, 43);
            lblTitle.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Text = "PHÂN CÔNG CBCS";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblTitle.AutoSize = false;
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Padding = new Padding(0);

            lblMissionName.SetBounds(96, 45, 760, 24);
            lblMissionName.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            lblMissionName.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblMissionName.ForeColor = Color.White;
            lblMissionName.Text = "Công tác: Chưa chọn";
            lblMissionName.TextAlign = ContentAlignment.MiddleLeft;
            lblMissionName.AutoSize = false;
            lblMissionName.BackColor = Color.Transparent;
            lblMissionName.Padding = new Padding(0);
            lblMissionId.Visible = false;

            // SEARCH STRIP: full-width rounded card, label on one line.
            pnlSearch.Dock = DockStyle.Fill;
            pnlSearch.Margin = new Padding(0);
            pnlSearch.Padding = new Padding(8, 6, 12, 6);
            pnlSearch.FillColor = Color.FromArgb(232, 242, 255);
            pnlSearch.BorderColor = Color.FromArgb(222, 235, 252);
            pnlSearch.BorderSize = 1;
            pnlSearch.Radius = 15;
            pnlSearch.Controls.Add(pnlSearchInput);
            pnlSearch.Controls.Add(lblSearch);

            lblSearch.Dock = DockStyle.Left;
            lblSearch.Width = 142;
            lblSearch.Font = new Font("Segoe UI", 8.6F, FontStyle.Bold);
            lblSearch.ForeColor = Color.FromArgb(12, 57, 111);
            lblSearch.Text = "⌕  Tìm kiếm CBCS:";
            lblSearch.AutoEllipsis = false;
            lblSearch.BackColor = Color.Transparent;
            lblSearch.TextAlign = ContentAlignment.MiddleLeft;
            lblSearch.AutoSize = false;

            pnlSearchInput.Dock = DockStyle.Left;
            pnlSearchInput.Width = 335;
            pnlSearchInput.Margin = new Padding(8, 0, 0, 0);
            pnlSearchInput.Padding = new Padding(10, 2, 10, 2);
            pnlSearchInput.FillColor = Color.White;
            pnlSearchInput.BorderColor = Color.FromArgb(166, 202, 246);
            pnlSearchInput.BorderSize = 1;
            pnlSearchInput.Radius = 18;
            pnlSearchInput.Controls.Add(txtSearch);

            txtSearch.Dock = DockStyle.Fill;
            txtSearch.BorderStyle = BorderStyle.None;
            txtSearch.Font = new Font("Segoe UI", 10F);
            txtSearch.ForeColor = Color.FromArgb(55, 68, 85);
            txtSearch.PlaceholderText = "Nhập số hiệu, họ tên CBCS...";
            txtSearch.TextChanged += txtSearch_TextChanged;

            // Outer search host creates even left/right inset around the rounded strip.
            searchHost.Dock = DockStyle.Top;
            searchHost.Height = 56;
            searchHost.Padding = new Padding(14, 5, 14, 5);
            searchHost.BackColor = Color.FromArgb(246, 250, 255);
            searchHost.Controls.Add(pnlSearch);

            // BODY: symmetric 44 / 12 / 44 proportions.
            bodyLayout.Dock = DockStyle.Fill;
            bodyLayout.BackColor = Color.FromArgb(246, 250, 255);
            bodyLayout.Padding = new Padding(14, 5, 14, 5);
            bodyLayout.ColumnCount = 3;
            bodyLayout.RowCount = 1;
            bodyLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 44.25F));
            bodyLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.5F));
            bodyLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 44.25F));
            bodyLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            bodyLayout.Controls.Add(pnlAvailableCard, 0, 0);
            bodyLayout.Controls.Add(centerPanel, 1, 0);
            bodyLayout.Controls.Add(pnlAssignedCard, 2, 0);

            // LEFT CARD
            ConfigureCard(pnlAvailableCard);
            pnlAvailableCard.Padding = new Padding(0);
            ConfigureCardContentLayout(availableCardLayout);
            availableCardLayout.Controls.Add(pnlAvailableTitle, 0, 0);
            availableCardLayout.Controls.Add(pnlAvailableGrid, 0, 2);
            pnlAvailableCard.Controls.Add(availableCardLayout);

            ConfigureSectionTitle(pnlAvailableTitle);
            pnlAvailableTitle.Controls.Add(lblAvailableTitle);
            pnlAvailableTitle.Controls.Add(lblAvailableIcon);
            lblAvailableIcon.Dock = DockStyle.Left;
            lblAvailableIcon.Width = 42;
            lblAvailableIcon.Font = new Font("Segoe UI Emoji", 17F, FontStyle.Regular);
            lblAvailableIcon.ForeColor = Color.FromArgb(0, 91, 190);
            lblAvailableIcon.Text = "👥";
            lblAvailableIcon.TextAlign = ContentAlignment.MiddleCenter;
            lblAvailableIcon.BackColor = Color.Transparent;
            ConfigureSectionLabel(lblAvailableTitle, "CBCS CHƯA PHÂN CÔNG (0)");

            ConfigureGridHost(pnlAvailableGrid);
            pnlAvailableGrid.Controls.Add(dgvAvailable);
            ConfigureGrid(dgvAvailable);
            dgvAvailable.Columns.AddRange(new DataGridViewColumn[] {
                colSttAvailable, colSoHieuAvailable, colHoTenAvailable,
                colDonViAvailable, colCapBacAvailable, colSoLanCT
            });
            dgvAvailable.CellDoubleClick += dgvAvailable_CellDoubleClick;

            colSttAvailable.Name = "colSttAvailable";
            ConfigureColumn(colSttAvailable, "STT", null, 7, 40, true);
            colSttAvailable.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colSttAvailable.Width = 42;
            colSttAvailable.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            ConfigureColumn(colSoHieuAvailable, "Số hiệu", "SoHieu", 17, 72);
            ConfigureColumn(colHoTenAvailable, "Họ tên", "HoTen", 21, 88);
            ConfigureColumn(colDonViAvailable, "Đơn vị", "DonVi", 18, 72);
            ConfigureColumn(colCapBacAvailable, "Cấp bậc", "CapBacHam", 18, 72);
            ConfigureColumn(colSoLanCT, "Số lần CT", "SoLanCongTac", 17, 66);

            // CENTER ACTIONS
            centerPanel.Dock = DockStyle.Fill;
            centerPanel.Margin = new Padding(4, 0, 4, 0);
            centerPanel.BackColor = Color.Transparent;
            centerPanel.Controls.Add(btnRefresh);
            centerPanel.Controls.Add(pnlAction);
            centerPanel.Controls.Add(btnClose);

            ConfigureButton(btnRefresh, "⟳  Làm mới", Color.White, Color.FromArgb(0, 91, 190), Color.FromArgb(91, 160, 230));
            btnRefresh.Size = new Size(104, 36);
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.BorderColor = Color.FromArgb(91, 160, 230);
            btnRefresh.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRefresh.Click += FrmMissionPersonnel_Load;

            pnlAction.BackColor = Color.Transparent;
            pnlAction.Size = new Size(78, 92);
            pnlAction.Controls.Add(btnAdd);
            pnlAction.Controls.Add(btnRemove);
            ConfigureButton(btnAdd, string.Empty, Color.FromArgb(0, 105, 210), Color.White, Color.FromArgb(0, 105, 210));
            btnAdd.Size = new Size(76, 40);
            btnAdd.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
            btnAdd.BorderColor = Color.FromArgb(0, 91, 190);
            btnAdd.Click += btnAssign_Click;
            ConfigureButton(btnRemove, string.Empty, Color.White, Color.FromArgb(0, 91, 190), Color.FromArgb(0, 105, 210));
            btnRemove.Size = new Size(76, 40);
            btnRemove.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
            btnRemove.BorderColor = Color.FromArgb(0, 105, 210);
            btnRemove.Click += btnRemove_Click;

            ConfigureButton(btnClose, "✖  Đóng", Color.FromArgb(235, 55, 58), Color.White, Color.FromArgb(214, 45, 50));
            btnClose.Size = new Size(100, 38);
            btnClose.Click += btnClose_Click;

            centerPanel.Resize += (s, e) => PositionCenterControls();

            // RIGHT CARD
            ConfigureCard(pnlAssignedCard);
            pnlAssignedCard.Padding = new Padding(0);
            ConfigureCardContentLayout(assignedCardLayout);
            assignedCardLayout.Controls.Add(pnlAssignedTitle, 0, 0);
            assignedCardLayout.Controls.Add(pnlAssignedGrid, 0, 2);
            pnlAssignedCard.Controls.Add(assignedCardLayout);

            ConfigureSectionTitle(pnlAssignedTitle);
            pnlAssignedTitle.Controls.Add(lblAssignedTitle);
            pnlAssignedTitle.Controls.Add(lblAssignedIcon);
            lblAssignedIcon.Dock = DockStyle.Left;
            lblAssignedIcon.Width = 42;
            lblAssignedIcon.Font = new Font("Segoe UI Emoji", 17F, FontStyle.Regular);
            lblAssignedIcon.ForeColor = Color.FromArgb(0, 91, 190);
            lblAssignedIcon.Text = "👥";
            lblAssignedIcon.TextAlign = ContentAlignment.MiddleCenter;
            lblAssignedIcon.BackColor = Color.Transparent;
            ConfigureSectionLabel(lblAssignedTitle, "CBCS ĐÃ PHÂN CÔNG (0)");

            ConfigureGridHost(pnlAssignedGrid);
            pnlAssignedGrid.Controls.Add(dgvAssigned);
            ConfigureGrid(dgvAssigned);
            dgvAssigned.Columns.AddRange(new DataGridViewColumn[] {
                colSttAssigned, colSoHieuAssigned, colHoTenAssigned,
                colSoLanCTAssigned, colSDonViAssigned, colCapBacHamAssigned,
                colNgayPhanCongAssigned
            });
            dgvAssigned.CellDoubleClick += dgvAssigned_CellDoubleClick;

            colSttAssigned.Name = "colSttAssigned";
            ConfigureColumn(colSttAssigned, "STT", null, 7, 40, true);
            colSttAssigned.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colSttAssigned.Width = 42;
            colSttAssigned.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            ConfigureColumn(colSoHieuAssigned, "Số hiệu", "SoHieu", 15, 68);
            ConfigureColumn(colHoTenAssigned, "Họ tên", "HoTen", 18, 78);
            ConfigureColumn(colSoLanCTAssigned, "Số lần CT", "SoLanCongTac", 12, 60);
            ConfigureColumn(colSDonViAssigned, "Đơn vị", "DonVi", 13, 62);
            ConfigureColumn(colCapBacHamAssigned, "Cấp bậc", "CapBacHam", 13, 62);
            colNgayPhanCongAssigned.Name = "colNgayPhanCongAssigned";
            ConfigureColumn(colNgayPhanCongAssigned, "Ngày phân công", "NgayPhanCong", 19, 94);

            // Empty-state nằm dưới header cột, không che tiêu đề.
            pnlAssignedEmpty.BackColor = Color.White;
            pnlAssignedEmpty.Visible = true;
            pnlAssignedEmpty.Controls.Add(lblAssignedEmptyIcon);
            pnlAssignedEmpty.Controls.Add(lblAssignedEmptyText);
            lblAssignedEmptyIcon.Size = new Size(170, 128);
            lblAssignedEmptyIcon.BackColor = Color.Transparent;
            lblAssignedEmptyText.Size = new Size(340, 38);
            lblAssignedEmptyText.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblAssignedEmptyText.ForeColor = Color.FromArgb(115, 130, 150);
            lblAssignedEmptyText.Text = "Chưa có CBCS được phân công";
            lblAssignedEmptyText.TextAlign = ContentAlignment.MiddleCenter;
            pnlAssignedEmpty.Resize += (s, e) => PositionEmptyState();
            dgvAssigned.Resize += (s, e) => PositionEmptyPanel();
            dgvAssigned.Controls.Add(pnlAssignedEmpty);

            // FOOTER: 2 count cards aligned with their corresponding grids.
            footerLayout.Dock = DockStyle.Bottom;
            footerLayout.Height = 50;
            footerLayout.BackColor = Color.FromArgb(246, 250, 255);
            footerLayout.Padding = new Padding(14, 4, 14, 6);
            footerLayout.ColumnCount = 3;
            footerLayout.RowCount = 1;
            footerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 44.25F));
            footerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.5F));
            footerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 44.25F));
            footerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            footerLayout.Controls.Add(lblAvailableCount, 0, 0);
            footerLayout.Controls.Add(lblAssignedCount, 2, 0);
            ConfigureCountLabel(lblAvailableCount, "ⓘ  Chưa phân công: 0 CBCS");
            ConfigureCountLabel(lblAssignedCount, "ⓘ  Đã phân công: 0 CBCS");
            lblAvailableCount.Margin = new Padding(0, 0, 7, 0);
            lblAssignedCount.Margin = new Padding(7, 0, 0, 0);

            // Form root and sizing.
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(246, 250, 255);
            ClientSize = new Size(1360, 780);
            MinimumSize = new Size(1200, 700);
            Controls.Add(bodyLayout);
            Controls.Add(footerLayout);
            Controls.Add(searchHost);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.Sizable;
            MaximizeBox = false;
            MinimizeBox = true;
            Name = "FrmMissionPersonnel";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Phân công CBCS";
            ShowIcon = false;
            ShowInTaskbar = false;
            Load += FrmMissionPersonnel_Load;
            Shown += (s, e) =>
            {
                PositionCenterControls();
                PositionEmptyPanel();
                Icon = null;
            };

            pnlHeader.ResumeLayout(false);
            pnlSearch.ResumeLayout(false);
            pnlSearchInput.ResumeLayout(false);
            pnlSearchInput.PerformLayout();
            bodyLayout.ResumeLayout(false);
            pnlAvailableCard.ResumeLayout(false);
            pnlAvailableTitle.ResumeLayout(false);
            pnlAvailableGrid.ResumeLayout(false);
            centerPanel.ResumeLayout(false);
            pnlAction.ResumeLayout(false);
            pnlAssignedCard.ResumeLayout(false);
            pnlAssignedTitle.ResumeLayout(false);
            pnlAssignedGrid.ResumeLayout(false);
            footerLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvAvailable).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvAssigned).EndInit();
            ResumeLayout(false);
        }

        private void ConfigureCardContentLayout(TableLayoutPanel layout)
        {
            // Dedicated rows keep the section title independent from the DataGridView.
            // The middle spacer guarantees a visible gap and prevents any header overlap.
            layout.Dock = DockStyle.Fill;
            layout.Margin = new Padding(0);
            layout.Padding = new Padding(0);
            layout.BackColor = Color.Transparent;
            layout.ColumnCount = 1;
            layout.RowCount = 3;
            layout.ColumnStyles.Clear();
            layout.RowStyles.Clear();
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 8F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        }

        private void ConfigureCard(RoundedPanel panel)
        {
            panel.Dock = DockStyle.Fill;
            panel.Margin = new Padding(4, 7, 4, 7);
            panel.FillColor = Color.White;
            // Outer card itself is borderless: prevents the dark hairline seen around
            // nested rounded panels on some Windows scaling settings.
            panel.BorderColor = Color.Transparent;
            panel.BorderSize = 0;
            panel.Radius = 14;
        }

        private void ConfigureSectionTitle(RoundedPanel panel)
        {
            panel.Dock = DockStyle.Fill;
            panel.Height = 46;
            panel.Margin = new Padding(0);
            panel.FillColor = Color.FromArgb(220, 235, 255);
            panel.BorderColor = Color.FromArgb(190, 216, 247);
            panel.BorderSize = 1;
            panel.Radius = 11;
        }

        private void ConfigureSectionLabel(Label label, string text)
        {
            label.Dock = DockStyle.Fill;
            label.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            label.ForeColor = Color.FromArgb(12, 57, 111);
            label.Text = text;
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.AutoSize = false;
            label.BackColor = Color.Transparent;
            label.Padding = new Padding(0, 0, 4, 0);
            label.UseMnemonic = false;
        }

        private void ConfigureGridHost(RoundedPanel panel)
        {
            panel.Dock = DockStyle.Fill;
            panel.Margin = new Padding(0);
            panel.Padding = new Padding(2);
            panel.FillColor = Color.White;
            panel.BorderColor = Color.FromArgb(211, 229, 250);
            panel.BorderSize = 1;
            panel.Radius = 12;
        }

        private void ConfigureGrid(DataGridView grid)
        {
            grid.Dock = DockStyle.Fill;
            grid.Margin = new Padding(0);
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.None;
            // The WinForms DataGridView can retain a one-pixel native client edge
            // even with BorderStyle.None on some Windows/DPI combinations.
            // FramelessDataGridView removes WS_BORDER and WS_EX_CLIENTEDGE at
            // the HWND level; keep these explicit settings as a second safeguard.
            grid.Margin = Padding.Empty;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            grid.AutoGenerateColumns = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.ColumnHeadersHeight = 43;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.EnableHeadersVisualStyles = false;

            // Use only supported WinForms border styles. DataGridViewCellBorderStyle.Custom
            // is not a valid value for the CellBorderStyle property and throws
            // ArgumentException when the form initializes the grid for assignment.
            // The native outer frame is already removed by FramelessDataGridView;
            // keep subtle internal separators using the standard Single style.
            grid.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            grid.GridColor = Color.FromArgb(220, 233, 249);
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.RowTemplate.Height = 32;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.ScrollBars = ScrollBars.Vertical;
            // Do not assign a native Region to DataGridView. Its HWND can leave
            // dark one-pixel seams at the top/right edges on some Windows builds.
            // The surrounding RoundedPanel supplies the rounded outer silhouette.
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            grid.DefaultCellStyle.ForeColor = Color.FromArgb(35, 48, 65);
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(48, 151, 220);
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 249, 255);
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 68, 112);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            grid.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            // The rounded host panel clips the grid. Do not assign a Region to the
            // native DataGridView itself: it can leave a dark one-pixel edge on Windows.
        }

        private void ConfigureColumn(DataGridViewTextBoxColumn column, string header, string dataProperty, float fill, int minWidth, bool readOnly = true)
        {
            if (string.IsNullOrWhiteSpace(column.Name))
                column.Name = !string.IsNullOrWhiteSpace(dataProperty) ? dataProperty : header;
            column.HeaderText = header;
            column.DataPropertyName = dataProperty ?? string.Empty;
            column.ReadOnly = readOnly;
            column.FillWeight = fill;
            column.MinimumWidth = minWidth;
            column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void ConfigureButton(Button button, string text, Color back, Color fore, Color border)
        {
            button.Text = text;
            button.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            button.ForeColor = fore;
            button.BackColor = back;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = border;
            // These controls paint their own anti-aliased rounded outline.
            // A native FlatAppearance border adds a rectangular frame underneath.
            button.FlatAppearance.BorderSize =
                (button is RoundedActionButton || button is ChevronButton) ? 0 : 2;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(232, 242, 255);
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(211, 230, 252);
            button.Cursor = Cursors.Hand;
            button.UseVisualStyleBackColor = false;
            if (button is RoundedActionButton rounded)
            {
                rounded.CornerRadius = 9;
                rounded.BorderThickness = 1.7f;
                rounded.BorderColor = border;
                rounded.Invalidate();
            }
        }

        private void ConfigureCountLabel(Label label, string text)
        {
            label.Dock = DockStyle.Fill;
            label.BackColor = Color.FromArgb(220, 236, 255);
            label.ForeColor = Color.FromArgb(12, 57, 111);
            label.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            label.Padding = new Padding(14, 0, 0, 0);
            label.Text = text;
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.Resize += (s, e) => ApplyRoundedRegion(label, 9);
            ApplyRoundedRegion(label, 9);
        }

        private void PositionCenterControls()
        {
            if (centerPanel == null || btnRefresh == null) return;
            int w = centerPanel.ClientSize.Width;
            int h = centerPanel.ClientSize.Height;
            btnRefresh.Location = new Point(Math.Max(0, (w - btnRefresh.Width) / 2), 22);
            pnlAction.Location = new Point(Math.Max(0, (w - pnlAction.Width) / 2), Math.Max(92, (h - pnlAction.Height) / 2));
            btnAdd.Location = new Point(1, 0);
            btnRemove.Location = new Point(3, 48);
            btnClose.Location = new Point(Math.Max(0, (w - btnClose.Width) / 2), Math.Max(0, h - btnClose.Height - 9));
        }

        private void PositionEmptyPanel()
        {
            if (dgvAssigned == null || pnlAssignedEmpty == null) return;
            pnlAssignedEmpty.Location = new Point(0, dgvAssigned.ColumnHeadersHeight);
            pnlAssignedEmpty.Size = new Size(dgvAssigned.ClientSize.Width, Math.Max(0, dgvAssigned.ClientSize.Height - dgvAssigned.ColumnHeadersHeight));
            PositionEmptyState();
        }

        private void PositionEmptyState()
        {
            if (pnlAssignedEmpty == null || lblAssignedEmptyIcon == null) return;
            lblAssignedEmptyIcon.Location = new Point(Math.Max(0, (pnlAssignedEmpty.ClientSize.Width - lblAssignedEmptyIcon.Width) / 2), Math.Max(8, (pnlAssignedEmpty.ClientSize.Height - 145) / 2 - 10));
            lblAssignedEmptyText.Location = new Point(Math.Max(0, (pnlAssignedEmpty.ClientSize.Width - lblAssignedEmptyText.Width) / 2), lblAssignedEmptyIcon.Bottom + 2);
        }

        private void ApplyRoundedRegion(Control control, int radius)
        {
            if (control.Width <= 0 || control.Height <= 0) return;
            using (GraphicsPath path = new GraphicsPath())
            {
                int d = radius * 2;
                path.AddArc(0, 0, d, d, 180, 90);
                path.AddArc(control.Width - d - 1, 0, d, d, 270, 90);
                path.AddArc(control.Width - d - 1, control.Height - d - 1, d, d, 0, 90);
                path.AddArc(0, control.Height - d - 1, d, d, 90, 90);
                path.CloseFigure();
                control.Region = new Region(path);
            }
        }

        #region Fields
        private GradientHeaderPanel pnlHeader;
        private PeopleIconPanel lblHeaderIcon;
        private Label lblTitle;
        private Label lblMissionName;
        private Label lblMissionId;
        private RoundedPanel pnlSearch;
        private Label lblSearch;
        private RoundedPanel pnlSearchInput;
        private TextBox txtSearch;
        private TableLayoutPanel bodyLayout;
        private RoundedPanel pnlAvailableCard;
        private TableLayoutPanel availableCardLayout;
        private RoundedPanel pnlAvailableTitle;
        private Label lblAvailableIcon;
        private Label lblAvailableTitle;
        private RoundedPanel pnlAvailableGrid;
        private FramelessDataGridView dgvAvailable;
        private DataGridViewTextBoxColumn colSttAvailable;
        private DataGridViewTextBoxColumn colSoHieuAvailable;
        private DataGridViewTextBoxColumn colHoTenAvailable;
        private DataGridViewTextBoxColumn colDonViAvailable;
        private DataGridViewTextBoxColumn colCapBacAvailable;
        private DataGridViewTextBoxColumn colSoLanCT;
        private Panel centerPanel;
        private RoundedActionButton btnRefresh;
        private Panel pnlAction;
        private ChevronButton btnRemove;
        private ChevronButton btnAdd;
        private RoundedActionButton btnClose;
        private RoundedPanel pnlAssignedCard;
        private TableLayoutPanel assignedCardLayout;
        private RoundedPanel pnlAssignedTitle;
        private Label lblAssignedIcon;
        private Label lblAssignedTitle;
        private RoundedPanel pnlAssignedGrid;
        private FramelessDataGridView dgvAssigned;
        private DataGridViewTextBoxColumn colSttAssigned;
        private DataGridViewTextBoxColumn colSoHieuAssigned;
        private DataGridViewTextBoxColumn colHoTenAssigned;
        private DataGridViewTextBoxColumn colSoLanCTAssigned;
        private DataGridViewTextBoxColumn colSDonViAssigned;
        private DataGridViewTextBoxColumn colCapBacHamAssigned;
        private DataGridViewTextBoxColumn colNgayPhanCongAssigned;
        private Panel pnlAssignedEmpty;
        private ClipboardPeopleIcon lblAssignedEmptyIcon;
        private Label lblAssignedEmptyText;
        private TableLayoutPanel footerLayout;
        private Label lblAvailableCount;
        private Label lblAssignedCount;
        #endregion
    }

    /// <summary>Custom GDI+ rounded button. Avoids FlatStyle border clipping at rounded corners.</summary>
    internal class RoundedActionButton : Button
    {
        public Color BorderColor { get; set; } = Color.FromArgb(91, 160, 230);
        public float BorderThickness { get; set; } = 1.7f;
        public float CornerRadius { get; set; } = 9f;
        private bool _hover;
        private bool _pressed;

        public RoundedActionButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            UseVisualStyleBackColor = false;
            Cursor = Cursors.Hand;
            TextAlign = ContentAlignment.MiddleCenter;
            Resize += (s, e) => UpdateRoundedRegion();
            HandleCreated += (s, e) => UpdateRoundedRegion();
        }

        private void UpdateRoundedRegion()
        {
            if (Width < 2 || Height < 2) return;
            using (var path = CreateRoundPath(
                new RectangleF(0, 0, Width - 1, Height - 1), CornerRadius + 1f))
                Region = new Region(path);
        }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; _pressed = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnMouseDown(MouseEventArgs mevent) { _pressed = true; Invalidate(); base.OnMouseDown(mevent); }
        protected override void OnMouseUp(MouseEventArgs mevent) { _pressed = false; Invalidate(); base.OnMouseUp(mevent); }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            var rect = new RectangleF(BorderThickness / 2f, BorderThickness / 2f,
                Math.Max(0, Width - BorderThickness - 1), Math.Max(0, Height - BorderThickness - 1));
            Color fill = BackColor;
            if (!Enabled) fill = Color.FromArgb(225, 232, 241);
            else if (_pressed) fill = Color.FromArgb(218, 234, 252);
            else if (_hover && BackColor == Color.White) fill = Color.FromArgb(239, 246, 255);
            using (var path = CreateRoundPath(rect, CornerRadius))
            using (var brush = new SolidBrush(fill)) e.Graphics.FillPath(brush, path);
            using (var path = CreateRoundPath(rect, CornerRadius))
            using (var pen = new Pen(BorderColor, BorderThickness)) e.Graphics.DrawPath(pen, path);
            TextRenderer.DrawText(e.Graphics, Text, Font, Rectangle.Round(rect),
                Enabled ? ForeColor : Color.Gray,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
        }

        internal static GraphicsPath CreateRoundPath(RectangleF r, float radius)
        {
            var path = new GraphicsPath();
            float d = Math.Min(radius * 2f, Math.Min(r.Width, r.Height));
            if (d <= 0) { path.AddRectangle(r); return path; }
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    internal enum ChevronDirection { Left, Right }

    /// <summary>Button with vector-drawn double chevrons; independent of installed fonts.</summary>
    internal class ChevronButton : Button
    {
        public ChevronDirection Direction { get; set; } = ChevronDirection.Right;
        public Color BorderColor { get; set; } = Color.FromArgb(0, 105, 210);

        public ChevronButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Text = string.Empty;
            Cursor = Cursors.Hand;
            Resize += (s, e) => UpdateRoundedRegion();
            HandleCreated += (s, e) => UpdateRoundedRegion();
        }

        private void UpdateRoundedRegion()
        {
            if (Width < 2 || Height < 2) return;
            using (var path = RoundedActionButton.CreateRoundPath(
                new RectangleF(0, 0, Width - 1, Height - 1), 10f))
                Region = new Region(path);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var bounds = new RectangleF(1.2f, 1.2f, Math.Max(0, Width - 2.4f), Math.Max(0, Height - 2.4f));
            using (var path = RoundedActionButton.CreateRoundPath(bounds, 9f))
            using (var brush = new SolidBrush(Enabled ? BackColor : Color.FromArgb(225, 232, 241)))
                e.Graphics.FillPath(brush, path);
            using (var path = RoundedActionButton.CreateRoundPath(bounds, 9f))
            using (var pen = new Pen(BorderColor, 1.8f))
                e.Graphics.DrawPath(pen, path);

            Color arrowColor = Enabled ? ForeColor : Color.Gray;
            using (var pen = new Pen(arrowColor, 3.2f))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                float cx = ClientSize.Width / 2f;
                float cy = ClientSize.Height / 2f;
                float spacing = 9f;
                float halfH = 9f;
                float halfW = 7f;
                for (int i = 0; i < 2; i++)
                {
                    float x = cx + (i == 0 ? -spacing / 2f : spacing / 2f);
                    if (Direction == ChevronDirection.Right)
                    {
                        e.Graphics.DrawLine(pen, x - halfW, cy - halfH, x + halfW, cy);
                        e.Graphics.DrawLine(pen, x + halfW, cy, x - halfW, cy + halfH);
                    }
                    else
                    {
                        e.Graphics.DrawLine(pen, x + halfW, cy - halfH, x - halfW, cy);
                        e.Graphics.DrawLine(pen, x - halfW, cy, x + halfW, cy + halfH);
                    }
                }
            }
        }
    }

    internal class GradientHeaderPanel : Panel
    {
        public Color StartColor { get; set; } = Color.FromArgb(5, 48, 105);
        public Color EndColor { get; set; } = Color.FromArgb(0, 88, 184);
        public GradientHeaderPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var brush = new LinearGradientBrush(ClientRectangle, StartColor, EndColor, LinearGradientMode.Horizontal))
                e.Graphics.FillRectangle(brush, ClientRectangle);
        }
    }

    internal class PeopleIconPanel : Control
    {
        public PeopleIconPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var brush = new SolidBrush(Color.White))
            {
                float cx = ClientSize.Width / 2f;
                float cy = ClientSize.Height / 2f;
                // Ba khuôn mặt
                e.Graphics.FillEllipse(brush, cx - 10, cy - 28, 20, 20);
                e.Graphics.FillEllipse(brush, cx - 31, cy - 20, 17, 17);
                e.Graphics.FillEllipse(brush, cx + 14, cy - 20, 17, 17);
                // Vai và thân người ở giữa
                e.Graphics.FillEllipse(brush, cx - 24, cy - 5, 48, 35);
                // Hai vai hai bên
                e.Graphics.FillEllipse(brush, cx - 39, cy + 1, 27, 27);
                e.Graphics.FillEllipse(brush, cx + 12, cy + 1, 27, 27);
            }
        }
    }

    internal class ClipboardPeopleIcon : Control
    {
        public ClipboardPeopleIcon()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
        }

        protected override void OnPaintBackground(PaintEventArgs e) { base.OnPaintBackground(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Color c = Color.FromArgb(166, 205, 255);
            using (var pen = new Pen(c, 4f))
            using (var fill = new SolidBrush(Color.White))
            using (var people = new SolidBrush(c))
            {
                float cx = ClientSize.Width / 2f;
                float top = 12f;
                RectangleF board = new RectangleF(cx - 25, top + 9, 50, 64);
                using (var path = new GraphicsPath())
                {
                    float r = 5f;
                    path.AddArc(board.X, board.Y, r * 2, r * 2, 180, 90);
                    path.AddArc(board.Right - r * 2, board.Y, r * 2, r * 2, 270, 90);
                    path.AddArc(board.Right - r * 2, board.Bottom - r * 2, r * 2, r * 2, 0, 90);
                    path.AddArc(board.X, board.Bottom - r * 2, r * 2, r * 2, 90, 90);
                    path.CloseFigure();
                    e.Graphics.DrawPath(pen, path);
                }
                e.Graphics.DrawArc(pen, cx - 9, top + 1, 18, 17, 180, 180);
                e.Graphics.DrawLine(pen, cx - 12, top + 17, cx + 12, top + 17);
                for (int i = 0; i < 3; i++)
                    e.Graphics.DrawLine(pen, cx - 14, top + 31 + i * 10, cx + 14, top + 31 + i * 10);
                // Small group of personnel in front of the lower-right clipboard corner.
                e.Graphics.FillEllipse(people, cx + 5, top + 49, 13, 13);
                e.Graphics.FillEllipse(people, cx + 19, top + 52, 11, 11);
                e.Graphics.FillEllipse(people, cx - 7, top + 53, 11, 11);
                e.Graphics.FillEllipse(people, cx + 1, top + 62, 28, 16);
                e.Graphics.FillEllipse(people, cx + 17, top + 64, 20, 13);
                e.Graphics.FillEllipse(people, cx - 10, top + 65, 20, 13);
                // Small decorative strokes at the upper-right of the clipboard.
                using (var accent = new Pen(c, 3.2f))
                {
                    e.Graphics.DrawLine(accent, cx + 36, top + 4, cx + 42, top - 2);
                    e.Graphics.DrawLine(accent, cx + 43, top + 13, cx + 51, top + 11);
                    e.Graphics.DrawLine(accent, cx + 33, top - 3, cx + 35, top - 10);
                }
            }
        }
    }

    /// <summary>
    /// Borderless DataGridView that keeps the native WinForms painting and
    /// data-binding pipeline intact. The non-client frame is removed only
    /// after the HWND is created; no UserPaint or CreateParams overrides.
    /// </summary>
    internal class FramelessDataGridView : DataGridView
    {
        private const int GWL_STYLE = -16;
        private const int GWL_EXSTYLE = -20;
        private const int WS_BORDER = unchecked((int)0x00800000);
        private const int WS_EX_CLIENTEDGE = 0x00000200;
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOZORDER = 0x0004;
        private const uint SWP_FRAMECHANGED = 0x0020;

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern int GetWindowLong32(IntPtr hWnd, int nIndex);

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        private static extern bool SetWindowPos(
            IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        public FramelessDataGridView()
        {
            BorderStyle = BorderStyle.None;
            Margin = Padding.Empty;
        }

        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            // Clip the first and last visible header cells to rounded top corners.
            // The lower corners remain square so the header joins the data rows cleanly.
            if (e.RowIndex == -1 && e.ColumnIndex >= 0 &&
                e.ColumnIndex < Columns.Count && Columns[e.ColumnIndex].Visible)
            {
                int firstDisplayIndex = int.MaxValue;
                int lastDisplayIndex = int.MinValue;
                foreach (DataGridViewColumn column in Columns)
                {
                    if (!column.Visible) continue;
                    if (column.DisplayIndex < firstDisplayIndex)
                        firstDisplayIndex = column.DisplayIndex;
                    if (column.DisplayIndex > lastDisplayIndex)
                        lastDisplayIndex = column.DisplayIndex;
                }

                bool roundLeft = Columns[e.ColumnIndex].DisplayIndex == firstDisplayIndex;
                bool roundRight = Columns[e.ColumnIndex].DisplayIndex == lastDisplayIndex;
                if (roundLeft || roundRight)
                {
                    Rectangle r = e.CellBounds;
                    int radius = Math.Min(9, Math.Min(r.Width, r.Height) / 3);
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.StartFigure();
                        path.AddLine(r.Left + (roundLeft ? radius : 0), r.Top,
                                     r.Right - (roundRight ? radius : 0), r.Top);
                        if (roundRight)
                            path.AddArc(r.Right - radius * 2, r.Top, radius * 2, radius * 2, 270, 90);
                        else
                            path.AddLine(r.Right, r.Top, r.Right, r.Top);
                        path.AddLine(r.Right, r.Top + (roundRight ? radius : 0), r.Right, r.Bottom);
                        path.AddLine(r.Right, r.Bottom, r.Left, r.Bottom);
                        path.AddLine(r.Left, r.Bottom, r.Left, r.Top + (roundLeft ? radius : 0));
                        if (roundLeft)
                            path.AddArc(r.Left, r.Top, radius * 2, radius * 2, 180, 90);
                        else
                            path.AddLine(r.Left, r.Top, r.Left, r.Top);
                        path.CloseFigure();

                        GraphicsState state = e.Graphics.Save();
                        try
                        {
                            e.Graphics.SetClip(path, CombineMode.Intersect);
                            base.OnCellPainting(e);
                        }
                        finally
                        {
                            e.Graphics.Restore(state);
                        }
                    }
                    return;
                }
            }

            base.OnCellPainting(e);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            // Remove only the native non-client frame. Do not alter the
            // DataGridView's painting styles, child windows, or data source.
            int style = GetWindowLong32(Handle, GWL_STYLE);
            SetWindowLong32(Handle, GWL_STYLE, style & ~WS_BORDER);

            int exStyle = GetWindowLong32(Handle, GWL_EXSTYLE);
            SetWindowLong32(Handle, GWL_EXSTYLE, exStyle & ~WS_EX_CLIENTEDGE);

            SetWindowPos(Handle, IntPtr.Zero, 0, 0, 0, 0,
                SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
        }
    }

    internal class RoundedPanel : Panel
    {
        public Color FillColor { get; set; } = Color.White;
        public Color BorderColor { get; set; } = Color.FromArgb(220, 232, 248);
        public int BorderSize { get; set; } = 1;
        public int Radius { get; set; } = 10;

        public RoundedPanel()
        {
            DoubleBuffered = true;
            Resize += (s, e) => UpdateRegion();
        }

        private void UpdateRegion()
        {
            if (Width < 2 || Height < 2) return;
            using (var path = CreateRoundPath(new Rectangle(0, 0, Width - 1, Height - 1), Radius))
                Region = new Region(path);
            Invalidate();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var path = CreateRoundPath(new Rectangle(0, 0, Width - 1, Height - 1), Radius))
            using (var brush = new SolidBrush(FillColor))
                e.Graphics.FillPath(brush, path);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (BorderSize <= 0) return;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var path = CreateRoundPath(new Rectangle(BorderSize, BorderSize, Width - BorderSize * 2 - 1, Height - BorderSize * 2 - 1), Radius))
            using (var pen = new Pen(BorderColor, BorderSize))
                e.Graphics.DrawPath(pen, path);
        }

        private static GraphicsPath CreateRoundPath(Rectangle bounds, int radius)
        {
            var path = new GraphicsPath();
            int d = Math.Max(2, radius * 2);
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
