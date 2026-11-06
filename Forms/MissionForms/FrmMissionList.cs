using QuanLyCBCS.Helpers;
using QuanLyCBCS.Models;
using QuanLyCBCS.Services;

namespace QuanLyCBCS.Forms.MissionForms
{
    public partial class FrmMissionList : Form
    {
        private readonly MissionService
            _missionService =
                new();

        private List<Mission>
            _allMissions =
                new();

        // Danh sách đang hiển thị sau khi áp dụng từ khóa tìm kiếm.
        private List<Mission>
            _displayedMissions =
                new();

        private readonly Font _priorityBoldFont;

        public FrmMissionList()
        {
            InitializeComponent();
            ApplyModernDesign();

            // Kích hoạt tìm kiếm ngay khi người dùng nhập hoặc xóa từ khóa.
            txtSearchMission.TextChanged += txtSearchMission_TextChanged;

            dgvMission.KeyDown += dgvMission_KeyDown;

            dgvMission.CellDoubleClick +=
                dgvMission_CellDoubleClick;

            dgvMission.CellFormatting +=
                dgvMission_CellFormatting;

            _priorityBoldFont =
                new Font(
                    dgvMission.Font,
                    FontStyle.Bold);
        }

        // Chỉ tùy chỉnh giao diện FrmMissionList; không thay đổi nghiệp vụ công tác.
        private readonly Color _navy = Color.FromArgb(17, 59, 103);
        private readonly Color _blue = Color.FromArgb(0, 103, 230);
        private readonly Color _lightBlue = Color.FromArgb(232, 244, 255);
        private readonly Dictionary<Button, (string Text, Image? Image)> _centeredActionContent = new();
        private Panel _emptyStatePanel = null!;
        private Label _emptyStateIcon = null!;
        private Label _emptyStateTitle = null!;
        private Label _emptyStateHint = null!;
        private Panel _headerIcon = null!;
        private Panel _kpiBackPanel = null!;
        private Panel _searchBackPanel = null!;
        private Panel _gridContainer = null!;

        private void ApplyModernDesign()
        {
            SuspendLayout();

            BackColor = Color.FromArgb(241, 248, 255);
            Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            MinimumSize = new Size(1100, 650);

            // Header: canh trái theo bản thiết kế, không thay đổi cấu trúc FrmHome.
            pnlHeader.BackColor = _navy;
            pnlHeader.Height = 92;
            pnlHeader.Dock = DockStyle.None;
            pnlHeader.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlHeader.SetBounds(0, 0, ClientSize.Width, 92);

            lblTitle.AutoSize = true;
            lblTitle.Text = "QUẢN LÝ CÔNG TÁC";
            lblTitle.Font = new Font("Segoe UI", 23F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(126, 8);

            lblSubTitle.AutoSize = true;
            lblSubTitle.Text = "Quản lý công tác và phân công CBCS";
            lblSubTitle.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            lblSubTitle.ForeColor = Color.FromArgb(225, 238, 252);
            lblSubTitle.Location = new Point(126, 55);

            // Biểu tượng cặp công tác vẽ bằng GDI+, không phụ thuộc font icon bên ngoài.
            _headerIcon = new Panel { BackColor = Color.Transparent, Size = new Size(78, 68), Location = new Point(16, 10) };
            _headerIcon.Paint += DrawBriefcaseIcon;
            pnlHeader.Controls.Add(_headerIcon);
            _headerIcon.BringToFront();

            // Thanh công cụ.
            pnlToolbar.BackColor = Color.FromArgb(241, 248, 255);
            pnlToolbar.Height = 136;
            pnlToolbar.Dock = DockStyle.None;
            pnlToolbar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlToolbar.SetBounds(0, pnlHeader.Bottom, ClientSize.Width, 136);

            StyleActionButton(btnAddMission, Color.FromArgb(0, 103, 230), Color.White);
            StyleActionButton(btnEditMission, Color.White, _blue);
            StyleActionButton(btnDeleteMission, Color.White, Color.FromArgb(220, 53, 69));
            StyleActionButton(btnMembers, Color.White, _blue);
            StyleActionButton(btnRefresh, Color.White, _blue);

            btnAddMission.Text = "Thêm";
            btnEditMission.Text = "Sửa";
            btnDeleteMission.Text = "Xóa";
            btnMembers.Text = "Phân công";
            btnRefresh.Text = "Làm mới";

            foreach (Button button in new[] { btnAddMission, btnEditMission, btnDeleteMission, btnMembers, btnRefresh })
            {
                button.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                button.FlatAppearance.BorderSize = 1;
                ApplyRoundedButton(button, 9);
            }
            btnAddMission.Image = CreateActionIcon(0, Color.White);
            btnEditMission.Image = CreateActionIcon(1, _blue);
            btnDeleteMission.Image = CreateActionIcon(2, Color.FromArgb(220, 53, 69));
            btnMembers.Image = CreateActionIcon(3, _blue);
            btnRefresh.Image = CreateActionIcon(4, _blue);

            foreach (Button button in new[] { btnAddMission, btnEditMission, btnDeleteMission, btnMembers, btnRefresh })
            {
                button.UseMnemonic = false;
                button.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                button.Padding = Padding.Empty;
                ConfigureCenteredActionButton(button);
            }

            // KPI: bốn ô cùng kích thước, nằm trên một hàng.
            StyleKpiPanel(panel1, label1, label5, "Tổng CT", "12", 0);
            StyleKpiPanel(panel2, label2, label6, "Đang CT", "5", 1);
            StyleKpiPanel(panel3, label3, label7, "Hôm nay", "3", 2);
            StyleKpiPanel(panel4, label4, label8, "CBCS TG", "28", 3);

            txtSearchMission.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtSearchMission.Font = new Font("Segoe UI", 10F);
            txtSearchMission.BackColor = Color.White;
            txtSearchMission.ForeColor = Color.FromArgb(45, 62, 80);
            txtSearchMission.BorderStyle = BorderStyle.None;

            // Bảng dữ liệu.
            dgvMission.Dock = DockStyle.Fill;
            dgvMission.BackgroundColor = Color.White;
            dgvMission.BorderStyle = BorderStyle.None;
            dgvMission.Margin = Padding.Empty;
            dgvMission.GridColor = Color.FromArgb(205, 224, 242);
            dgvMission.EnableHeadersVisualStyles = false;
            dgvMission.ColumnHeadersHeight = 44;
            dgvMission.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvMission.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(25, 70, 115);
            dgvMission.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvMission.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvMission.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvMission.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvMission.ColumnHeadersDefaultCellStyle.Padding = new Padding(4, 0, 4, 0);
            dgvMission.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            dgvMission.DefaultCellStyle.ForeColor = Color.FromArgb(35, 52, 70);
            dgvMission.DefaultCellStyle.BackColor = Color.White;
            dgvMission.DefaultCellStyle.SelectionBackColor = Color.FromArgb(48, 151, 218);
            dgvMission.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvMission.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(235, 245, 255);
            dgvMission.RowTemplate.Height = 36;
            dgvMission.AllowUserToAddRows = false;
            dgvMission.AllowUserToResizeRows = false;
            // Luôn bật thanh cuộn dọc để người dùng xem toàn bộ danh sách công tác.
            // Không cần thanh cuộn ngang vì các cột đang sử dụng chế độ Fill.
            dgvMission.ScrollBars = ScrollBars.Vertical;
            dgvMission.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvMission.RowHeadersVisible = false;
            dgvMission.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMission.MultiSelect = false;
            dgvMission.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMission.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvMission.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvMission.BorderStyle = BorderStyle.None;
            // Giữ nguyên vùng hiển thị chữ nhật của DataGridView để không cắt thanh cuộn native.
            dgvMission.Region = null;

            statusStripMain.BackColor = Color.FromArgb(224, 240, 255);
            statusStripMain.ForeColor = _navy;
            statusStripMain.Dock = DockStyle.Bottom;
            statusStripMain.SizingGrip = false;
            statusStripMain.Padding = new Padding(8, 2, 8, 2);
            // Bo riêng hai góc dưới của chân bảng dữ liệu.
            statusStripMain.Resize += (s, e) => ApplyRoundedStatusStripBottom();
            ApplyRoundedStatusStripBottom();
            lblRecordCount.ForeColor = _navy;
            lblRecordCount.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            // Đặt DataGridView và thanh trạng thái trong cùng một khung bo góc.
            // Cách này bảo đảm phần bảng có vùng Fill riêng, không bị Dock trực tiếp
            // cạnh tranh thứ tự bố trí với Header/Toolbar trên Form.
            CreateRoundedGridContainer();

            // Nền chung cho cụm KPI, tương tự bản thiết kế.
            _kpiBackPanel = new Panel { BackColor = Color.FromArgb(247, 251, 255) };
            _kpiBackPanel.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(218, 235, 250), 1F);
                var rect = _kpiBackPanel.ClientRectangle;
                rect.Width -= 1; rect.Height -= 1;
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using var path = RoundedRect(rect, 15);
                e.Graphics.DrawPath(pen, path);
            };
            pnlToolbar.Controls.Add(_kpiBackPanel);
            _kpiBackPanel.SendToBack();

            _searchBackPanel = new Panel { BackColor = BackColor };
            _searchBackPanel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                var rect = _searchBackPanel.ClientRectangle;
                rect.Width -= 1; rect.Height -= 1;
                using var path = RoundedRect(rect, 22);
                using var fill = new SolidBrush(Color.White);
                using var pen = new Pen(Color.FromArgb(210, 229, 247), 1F);
                e.Graphics.FillPath(fill, path);
                e.Graphics.DrawPath(pen, path);

                // Biểu tượng kính lúp ở đầu ô tìm kiếm.
                using var iconPen = new Pen(Color.FromArgb(0, 103, 190), 2F)
                { StartCap = System.Drawing.Drawing2D.LineCap.Round, EndCap = System.Drawing.Drawing2D.LineCap.Round };
                e.Graphics.DrawEllipse(iconPen, 15, 12, 15, 15);
                e.Graphics.DrawLine(iconPen, 27, 25, 34, 32);
            };
            pnlToolbar.Controls.Add(_searchBackPanel);
            _searchBackPanel.SendToBack();

            // Tự căn lại khi tab/form thay đổi kích thước.
            pnlToolbar.Resize += (s, e) => LayoutToolbar();
            Resize += (s, e) => LayoutMainContent();
            LayoutMainContent();
            LayoutToolbar();

            CreateEmptyState();
            ResumeLayout(true);
            LayoutMainContent();
        }

        private void CreateRoundedGridContainer()
        {
            if (_gridContainer != null && !_gridContainer.IsDisposed)
                return;

            // Gỡ DataGridView và StatusStrip khỏi Form trước khi đưa vào khung riêng.
            Controls.Remove(dgvMission);
            Controls.Remove(statusStripMain);

            _gridContainer = new Panel
            {
                Name = "pnlMissionGridContainer",
                Dock = DockStyle.None,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = Color.White,
                Padding = new Padding(1),
                Margin = Padding.Empty
            };

            _gridContainer.Resize += (s, e) =>
            {
                LayoutRoundedGridChildren();
                ApplyRoundedGridContainerRegion();
            };
            _gridContainer.Paint += (s, e) =>
            {
                if (_gridContainer.ClientSize.Width < 4 || _gridContainer.ClientSize.Height < 4)
                    return;

                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                Rectangle bounds = new Rectangle(0, 0, _gridContainer.Width - 1, _gridContainer.Height - 1);
                using var path = RoundedRect(bounds, 12);
                using var pen = new Pen(Color.FromArgb(190, 218, 243), 1F);
                e.Graphics.DrawPath(pen, path);
            };

            // Không dùng TableLayoutPanel/Dock Fill tại đây vì có thể khiến vùng Fill
            // tính sai chiều cao khi Form được nhúng vào FrmHome.
            dgvMission.Dock = DockStyle.None;
            dgvMission.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvMission.Margin = Padding.Empty;
            dgvMission.ScrollBars = ScrollBars.Vertical;
            dgvMission.ColumnHeadersVisible = true;
            dgvMission.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvMission.Region = null;

            statusStripMain.Dock = DockStyle.None;
            statusStripMain.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            statusStripMain.Margin = Padding.Empty;
            statusStripMain.AutoSize = false;
            statusStripMain.Height = 28;
            statusStripMain.SizingGrip = false;

            _gridContainer.Controls.Add(dgvMission);
            _gridContainer.Controls.Add(statusStripMain);
            Controls.Add(_gridContainer);

            // Đưa vùng dữ liệu xuống dưới Header/Toolbar; tọa độ được tính độc lập
            // nên thứ tự Dock/Z-order không thể đẩy bảng đè lên hai vùng phía trên.
            _gridContainer.SendToBack();
            pnlToolbar.BringToFront();
            pnlHeader.BringToFront();

            LayoutMainContent();
            ApplyRoundedGridContainerRegion();
        }

        private void LayoutMainContent()
        {
            if (pnlHeader == null || pnlToolbar == null ||
                _gridContainer == null || _gridContainer.IsDisposed)
                return;

            const int headerHeight = 92;
            const int toolbarHeight = 136;

            pnlHeader.SetBounds(0, 0, ClientSize.Width, headerHeight);
            pnlToolbar.SetBounds(0, headerHeight, ClientSize.Width, toolbarHeight);

            int gridTop = headerHeight + toolbarHeight;
            int gridHeight = Math.Max(80, ClientSize.Height - gridTop);
            _gridContainer.SetBounds(0, gridTop, ClientSize.Width, gridHeight);

            LayoutRoundedGridChildren();
            ApplyRoundedGridContainerRegion();
            LayoutToolbar();
        }

        private void LayoutRoundedGridChildren()
        {
            if (_gridContainer == null || _gridContainer.IsDisposed ||
                dgvMission == null || statusStripMain == null)
                return;

            int width = Math.Max(0, _gridContainer.ClientSize.Width - 2);
            int height = Math.Max(0, _gridContainer.ClientSize.Height - 2);
            int statusHeight = Math.Min(28, height);

            dgvMission.SetBounds(1, 1, width, Math.Max(0, height - statusHeight));
            statusStripMain.SetBounds(1, Math.Max(1, height - statusHeight + 1), width, statusHeight);
            dgvMission.BringToFront();
            statusStripMain.BringToFront();
        }

        private void ApplyRoundedGridContainerRegion()
        {
            if (_gridContainer == null || _gridContainer.IsDisposed ||
                _gridContainer.Width < 4 || _gridContainer.Height < 4)
                return;

            using var path = RoundedRect(
                new Rectangle(0, 0, _gridContainer.Width - 1, _gridContainer.Height - 1),
                12);

            _gridContainer.Region?.Dispose();
            _gridContainer.Region = new Region(path);
        }

        private void LayoutToolbar()
        {
            if (pnlToolbar == null || pnlToolbar.IsDisposed) return;

            int width = pnlToolbar.ClientSize.Width;

            // Hai cột nút được mở rộng để đủ chỗ cho icon và toàn bộ nhãn tiếng Việt.
            const int firstColumnWidth = 128;
            const int secondColumnWidth = 140;
            const int columnGap = 10;
            const int left = 12;
            int secondColumn = left + firstColumnWidth + columnGap;

            btnAddMission.SetBounds(left, 8, firstColumnWidth, 34);
            btnEditMission.SetBounds(left, 48, firstColumnWidth, 34);
            btnDeleteMission.SetBounds(left, 88, firstColumnWidth, 30);
            btnMembers.SetBounds(secondColumn, 8, secondColumnWidth, 34);
            btnRefresh.SetBounds(secondColumn, 48, secondColumnWidth, 34);

            foreach (Button button in new[] { btnAddMission, btnEditMission, btnDeleteMission, btnMembers, btnRefresh })
                button.Invalidate();

            int searchWidth = Math.Min(350, Math.Max(300, width / 3));
            int searchX = width - searchWidth - 18;

            // Thu hẹp cụm KPI để ưu tiên không gian cho các nút chức năng.
            int kpiLeft = secondColumn + secondColumnWidth + 18;
            int availableForKpi = Math.Max(0, searchX - kpiLeft - 22);
            int kpiWidth = Math.Max(145, availableForKpi / 4);
            int kpiTotal = kpiWidth * 4;

            if (_kpiBackPanel != null)
            {
                _kpiBackPanel.SetBounds(kpiLeft - 8, 4, kpiTotal + 16, 102);
                _kpiBackPanel.SendToBack();
            }

            Panel[] panels = { panel1, panel2, panel3, panel4 };
            Label[] titles = { label1, label2, label3, label4 };
            Label[] values = { label5, label6, label7, label8 };

            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].SetBounds(kpiLeft + i * kpiWidth, 4, kpiWidth, 102);
                panels[i].BringToFront();
                titles[i].SetBounds(55, 17, Math.Max(70, kpiWidth - 62), 27);
                values[i].SetBounds(55, 45, Math.Max(70, kpiWidth - 62), 48);
            }

            if (_searchBackPanel != null)
            {
                _searchBackPanel.SetBounds(searchX, 18, searchWidth, 48);
                _searchBackPanel.Invalidate();
            }

            // TextBox nằm hoàn toàn trong nền bo góc, chừa khoảng trống cho kính lúp.
            txtSearchMission.SetBounds(searchX + 43, 31, Math.Max(120, searchWidth - 58), 23);
            txtSearchMission.BackColor = Color.White;
            txtSearchMission.BorderStyle = BorderStyle.None;
            txtSearchMission.BringToFront();
        }

        private void ApplyRoundedButton(Button button, int radius)
        {
            // Không dùng FlatAppearance.BorderSize để tránh viền mặc định của Button
            // bị Region bo góc cắt mất ở bốn góc. Viền được tự vẽ ở lớp Paint.
            button.FlatAppearance.BorderSize = 0;

            void UpdateRegion(object? sender, EventArgs e)
            {
                if (button.Width <= 0 || button.Height <= 0) return;

                // Region chỉ bo góc nền; chừa khoảng an toàn để nét viền tự vẽ
                // nằm hoàn toàn bên trong vùng hiển thị, không bị clipping.
                using var path = RoundedRect(
                    new Rectangle(0, 0, Math.Max(1, button.Width - 1), Math.Max(1, button.Height - 1)),
                    radius);

                button.Region?.Dispose();
                button.Region = new Region(path);
                button.Invalidate();
            }

            button.Resize -= UpdateRegion;
            button.Resize += UpdateRegion;
            button.Paint -= DrawRoundedButtonBorder;
            button.Paint += DrawRoundedButtonBorder;
            UpdateRegion(button, EventArgs.Empty);
        }

        private void DrawRoundedButtonBorder(object? sender, PaintEventArgs e)
        {
            if (sender is not Button button || button.Width < 6 || button.Height < 6)
                return;

            // Viền nằm lệch vào trong 1.5 px để toàn bộ nét được vẽ,
            // kể cả tại các cung bo góc. Không còn viền chữ nhật mặc định.
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            Color borderColor = button.Enabled
                ? button.FlatAppearance.BorderColor
                : Color.FromArgb(190, 190, 190);

            using var path = RoundedRect(
                new Rectangle(1, 1, button.Width - 3, button.Height - 3), 8);
            using var pen = new Pen(borderColor, 1.35F)
            {
                Alignment = System.Drawing.Drawing2D.PenAlignment.Center,
                LineJoin = System.Drawing.Drawing2D.LineJoin.Round
            };

            e.Graphics.DrawPath(pen, path);
        }

        /// <summary>
        /// Bo hai góc dưới của thanh trạng thái để hoàn thiện đường bao đáy bảng.
        /// Hai góc phía trên vẫn vuông nhằm nối liền với DataGridView phía trên.
        /// </summary>
        private void ApplyRoundedStatusStripBottom()
        {
            if (statusStripMain == null || statusStripMain.IsDisposed ||
                statusStripMain.Width <= 0 || statusStripMain.Height <= 0)
                return;

            int radius = Math.Min(10, Math.Min(statusStripMain.Width / 2,
                                               statusStripMain.Height / 2));
            int diameter = radius * 2;
            int width = statusStripMain.Width;
            int height = statusStripMain.Height;

            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.StartFigure();
            path.AddLine(0, 0, width, 0);
            path.AddLine(width, 0, width, height - radius);
            path.AddArc(width - diameter, height - diameter,
                        diameter, diameter, 0, 90);
            path.AddLine(width - radius, height, radius, height);
            path.AddArc(0, height - diameter,
                        diameter, diameter, 90, 90);
            path.AddLine(0, height - radius, 0, 0);
            path.CloseFigure();

            statusStripMain.Region?.Dispose();
            statusStripMain.Region = new Region(path);
            path.Dispose();
        }

        private void DrawBriefcaseIcon(object? sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var outline = new Pen(Color.White, 3.5F) { LineJoin = System.Drawing.Drawing2D.LineJoin.Round };
            using var detail = new Pen(_navy, 3F) { LineJoin = System.Drawing.Drawing2D.LineJoin.Round };
            using var fill = new SolidBrush(Color.White);
            using var body = RoundedRect(new Rectangle(5, 20, 66, 42), 6);
            e.Graphics.FillPath(fill, body);
            e.Graphics.DrawPath(outline, body);
            e.Graphics.DrawArc(outline, 25, 7, 26, 23, 180, 180);
            e.Graphics.DrawLine(detail, 7, 35, 69, 35);
            e.Graphics.DrawRectangle(detail, 32, 32, 13, 9);
        }

        private static System.Drawing.Drawing2D.GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// Vẽ biểu tượng và nhãn như một cụm duy nhất, căn chính giữa nút.
        /// Cách này tránh lỗi WinForms căn Image và Text độc lập khiến cụm nội dung bị lệch.
        /// </summary>
        private void ConfigureCenteredActionButton(Button button)
        {
            string caption = button.Text;
            Image? icon = button.Image;

            _centeredActionContent[button] = (caption, icon);
            button.AccessibleName = caption;
            button.Text = string.Empty;
            button.Image = null;
            button.TextImageRelation = TextImageRelation.Overlay;
            button.ImageAlign = ContentAlignment.MiddleCenter;
            button.TextAlign = ContentAlignment.MiddleCenter;
            button.Paint -= DrawCenteredActionButtonContent;
            button.Paint += DrawCenteredActionButtonContent;
            button.Invalidate();
        }

        private void DrawCenteredActionButtonContent(object? sender, PaintEventArgs e)
        {
            if (sender is not Button button ||
                !_centeredActionContent.TryGetValue(button, out var content))
                return;

            const int iconTextGap = 6;
            const int iconFallbackWidth = 18;
            const int iconFallbackHeight = 18;
            TextFormatFlags flags = TextFormatFlags.NoPadding |
                                   TextFormatFlags.SingleLine |
                                   TextFormatFlags.NoPrefix;

            Size textSize = TextRenderer.MeasureText(
                e.Graphics, content.Text, button.Font,
                new Size(Math.Max(1, button.ClientSize.Width), button.ClientSize.Height), flags);

            int iconWidth = content.Image?.Width ?? iconFallbackWidth;
            int iconHeight = content.Image?.Height ?? iconFallbackHeight;
            int gap = content.Image == null ? 0 : iconTextGap;
            int totalWidth = iconWidth + gap + textSize.Width;
            int startX = Math.Max(0, (button.ClientSize.Width - totalWidth) / 2);
            int centerY = button.ClientSize.Height / 2;

            if (content.Image != null)
            {
                int iconY = centerY - iconHeight / 2;
                e.Graphics.DrawImage(content.Image,
                    new Rectangle(startX, iconY, iconWidth, iconHeight));
            }

            int textX = startX + iconWidth + gap;
            int textY = centerY - textSize.Height / 2;
            TextRenderer.DrawText(e.Graphics, content.Text, button.Font,
                new Point(textX, textY), button.ForeColor, flags);
        }

        private void StyleActionButton(Button button, Color backColor, Color foreColor)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor =
                foreColor == Color.White
                    ? Color.FromArgb(0, 79, 190)
                    : foreColor == Color.FromArgb(220, 53, 69)
                        ? Color.FromArgb(235, 143, 153)
                        : Color.FromArgb(125, 177, 226);
            // Viền được vẽ thủ công trong DrawRoundedButtonBorder để tránh
            // viền FlatStyle mặc định bị cắt ở các góc bo.
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor =
                backColor == Color.White
                    ? Color.FromArgb(232, 244, 255)
                    : ControlPaint.Light(backColor, 0.08F);
            button.FlatAppearance.MouseDownBackColor =
                backColor == Color.White
                    ? Color.FromArgb(218, 236, 252)
                    : ControlPaint.Dark(backColor, 0.08F);
            button.BackColor = backColor;
            button.ForeColor = foreColor;
            button.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            button.TextAlign = ContentAlignment.MiddleCenter;
            button.Padding = new Padding(3, 0, 3, 0);
            button.Cursor = Cursors.Hand;
            button.UseVisualStyleBackColor = false;
        }

        private Bitmap CreateActionIcon(int iconType, Color color)
        {
            Bitmap bitmap = new Bitmap(18, 18);
            using Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.ScaleTransform(0.9F, 0.9F);
            using Pen pen = new Pen(color, 2F)
            {
                StartCap = System.Drawing.Drawing2D.LineCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round,
                LineJoin = System.Drawing.Drawing2D.LineJoin.Round
            };

            switch (iconType)
            {
                case 0: // Thêm
                    g.DrawLine(pen, 10, 3, 10, 17);
                    g.DrawLine(pen, 3, 10, 17, 10);
                    break;
                case 1: // Sửa
                    g.DrawLine(pen, 5, 15, 14, 6);
                    g.DrawLine(pen, 13, 5, 16, 8);
                    g.DrawLine(pen, 4, 16, 8, 15);
                    g.DrawLine(pen, 4, 16, 5, 12);
                    break;
                case 2: // Xóa
                    g.DrawRectangle(pen, 5, 6, 10, 11);
                    g.DrawLine(pen, 3, 4, 17, 4);
                    g.DrawLine(pen, 8, 2, 12, 2);
                    g.DrawLine(pen, 8, 9, 8, 14);
                    g.DrawLine(pen, 12, 9, 12, 14);
                    break;
                case 3: // Phân công
                    g.DrawEllipse(pen, 7, 2, 6, 6);
                    g.DrawArc(pen, 3, 8, 14, 11, 180, 180);
                    g.DrawEllipse(pen, 1, 5, 4, 4);
                    g.DrawEllipse(pen, 15, 5, 4, 4);
                    break;
                case 4: // Làm mới
                    g.DrawArc(pen, 3, 3, 14, 14, 35, 285);
                    g.DrawLine(pen, 14, 3, 17, 3);
                    g.DrawLine(pen, 17, 3, 17, 7);
                    break;
            }
            return bitmap;
        }

        private void StyleKpiPanel(Panel panel, Label title, Label value, string titleText, string valueText, int iconType)
        {
            panel.BackColor = Color.FromArgb(247, 251, 255);
            panel.BorderStyle = BorderStyle.None;

            title.Text = titleText;
            title.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            title.ForeColor = _navy;
            title.AutoSize = false;
            title.TextAlign = ContentAlignment.MiddleLeft;
            title.Dock = DockStyle.None;

            value.Text = valueText;
            value.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            value.ForeColor = Color.FromArgb(20, 48, 83);
            value.AutoSize = false;
            value.TextAlign = ContentAlignment.MiddleLeft;
            value.Dock = DockStyle.None;

            panel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                if (panel != panel4)
                {
                    using Pen divider = new Pen(Color.FromArgb(174, 211, 246), 1F);
                    e.Graphics.DrawLine(divider, panel.Width - 1, 14, panel.Width - 1, panel.Height - 14);
                }

                DrawKpiIcon(e.Graphics, iconType);
            };
        }

        private void DrawKpiIcon(Graphics graphics, int iconType)
        {
            Color blue = Color.FromArgb(0, 111, 220);
            using var pen = new Pen(blue, 2.4F)
            {
                StartCap = System.Drawing.Drawing2D.LineCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round,
                LineJoin = System.Drawing.Drawing2D.LineJoin.Round
            };

            switch (iconType)
            {
                case 0: // Cặp công tác
                    using (var body = RoundedRect(new Rectangle(17, 34, 29, 21), 4))
                        graphics.DrawPath(pen, body);
                    graphics.DrawArc(pen, 25, 27, 13, 12, 180, 180);
                    graphics.DrawLine(pen, 18, 41, 45, 41);
                    graphics.DrawRectangle(pen, 29, 39, 6, 5);
                    break;

                case 1: // Đồng hồ cát
                    graphics.DrawLine(pen, 22, 27, 42, 27);
                    graphics.DrawLine(pen, 22, 56, 42, 56);
                    graphics.DrawLine(pen, 24, 28, 40, 55);
                    graphics.DrawLine(pen, 40, 28, 24, 55);
                    graphics.DrawLine(pen, 27, 39, 37, 39);
                    graphics.DrawLine(pen, 27, 45, 37, 45);
                    break;

                case 2: // Lịch
                    graphics.DrawRectangle(pen, 17, 31, 29, 26);
                    graphics.DrawLine(pen, 17, 38, 46, 38);
                    graphics.DrawLine(pen, 24, 27, 24, 35);
                    graphics.DrawLine(pen, 39, 27, 39, 35);
                    graphics.DrawLine(pen, 24, 44, 28, 44);
                    graphics.DrawLine(pen, 34, 44, 39, 44);
                    graphics.DrawLine(pen, 24, 50, 28, 50);
                    break;

                case 3: // Nhóm CBCS
                    graphics.DrawEllipse(pen, 26, 27, 10, 10);
                    graphics.DrawEllipse(pen, 16, 31, 8, 8);
                    graphics.DrawEllipse(pen, 38, 31, 8, 8);
                    graphics.DrawArc(pen, 21, 38, 21, 18, 180, 180);
                    graphics.DrawArc(pen, 12, 41, 17, 14, 190, 150);
                    graphics.DrawArc(pen, 34, 41, 17, 14, 200, 150);
                    break;
            }
        }

        private void CreateEmptyState()
        {
            _emptyStatePanel = new Panel
            {
                BackColor = Color.White,
                Visible = false
            };
            _emptyStateIcon = new Label
            {
                Text = string.Empty,
                BackColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.None,
                Height = 130
            };
            _emptyStateIcon.Paint += DrawEmptyStateIcon;
            _emptyStateTitle = new Label
            {
                Text = "Chưa có dữ liệu công tác",
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                ForeColor = Color.FromArgb(16, 67, 132),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.None,
                Height = 38
            };
            _emptyStateHint = new Label
            {
                Text = "Nhấn “Thêm” để tạo công tác mới.",
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.FromArgb(115, 128, 145),
                TextAlign = ContentAlignment.TopCenter,
                Dock = DockStyle.None,
                Height = 34
            };
            _emptyStatePanel.Controls.Add(_emptyStateHint);
            _emptyStatePanel.Controls.Add(_emptyStateTitle);
            _emptyStatePanel.Controls.Add(_emptyStateIcon);
            dgvMission.Controls.Add(_emptyStatePanel);
            dgvMission.SizeChanged += (s, e) => PositionEmptyState();
            PositionEmptyState();
        }

        private void DrawEmptyStateIcon(object? sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Color pale = Color.FromArgb(202, 229, 251);
            Color blue = Color.FromArgb(117, 185, 242);
            using var palePen = new Pen(pale, 7F) { LineJoin = System.Drawing.Drawing2D.LineJoin.Round };
            using var bluePen = new Pen(blue, 5F) { LineJoin = System.Drawing.Drawing2D.LineJoin.Round };
            using var white = new SolidBrush(Color.White);
            int cx = _emptyStateIcon.ClientSize.Width / 2;
            int cy = 62;
            using var paleFill = new SolidBrush(Color.FromArgb(226, 241, 255));
            e.Graphics.FillRectangle(paleFill, cx - 76, cy - 25, 102, 83);
            e.Graphics.DrawRectangle(palePen, cx - 45, cy - 42, 86, 91);
            e.Graphics.DrawLine(palePen, cx - 24, cy - 24, cx + 20, cy - 24);
            e.Graphics.DrawLine(palePen, cx - 24, cy - 3, cx + 20, cy - 3);
            e.Graphics.DrawLine(palePen, cx - 24, cy + 18, cx + 13, cy + 18);
            e.Graphics.DrawEllipse(bluePen, cx + 10, cy + 6, 42, 42);
            e.Graphics.DrawLine(bluePen, cx + 43, cy + 39, cx + 63, cy + 60);
            e.Graphics.DrawLine(bluePen, cx + 47, cy - 45, cx + 54, cy - 56);
            e.Graphics.DrawLine(bluePen, cx + 61, cy - 39, cx + 72, cy - 43);
        }

        private void PositionEmptyState()
        {
            if (_emptyStatePanel == null || dgvMission == null) return;

            int headerHeight = dgvMission.ColumnHeadersVisible ? dgvMission.ColumnHeadersHeight : 0;
            int contentHeight = Math.Max(0, dgvMission.ClientSize.Height - headerHeight - 2);
            int contentWidth = Math.Max(0, dgvMission.ClientSize.Width - 2);

            _emptyStatePanel.SetBounds(1, headerHeight + 1, contentWidth, contentHeight);

            int stackHeight = 202;
            int top = Math.Max(12, (contentHeight - stackHeight) / 2 + 22);
            _emptyStateIcon.SetBounds(0, top, contentWidth, 130);
            _emptyStateTitle.SetBounds(0, top + 130, contentWidth, 38);
            _emptyStateHint.SetBounds(0, top + 168, contentWidth, 34);
            _emptyStatePanel.BringToFront();
        }

        private void UpdateEmptyState()
        {
            if (_emptyStatePanel == null) return;
            _emptyStatePanel.Visible = _displayedMissions.Count == 0;

            _emptyStateTitle.Text = _allMissions.Count == 0
                ? "Chưa có dữ liệu công tác"
                : "Không tìm thấy công tác phù hợp";

            _emptyStateHint.Text = _allMissions.Count == 0
                ? "Nhấn “Thêm” để tạo công tác mới."
                : "Thử thay đổi từ khóa tìm kiếm.";

            if (_emptyStatePanel.Visible) PositionEmptyState();
        }

        private async Task
            LoadMissionAsync()
        {
            _allMissions =
                await _missionService
                    .GetAllAsync();

            // Giữ nguyên từ khóa hiện tại sau khi thêm, sửa hoặc làm mới dữ liệu.
            ApplyMissionSearch();
        }

        /// <summary>
        /// Tìm kiếm không phân biệt chữ hoa/chữ thường và dấu tiếng Việt.
        /// Dữ liệu gốc _allMissions luôn được giữ nguyên; chỉ danh sách hiển thị bị lọc.
        /// </summary>
        private void txtSearchMission_TextChanged(object? sender, EventArgs e)
        {
            ApplyMissionSearch();
        }

        private void ApplyMissionSearch()
        {
            string keyword = VietnameseHelper
                .RemoveVietnameseSigns(txtSearchMission.Text.Trim())
                .ToLowerInvariant();

            _displayedMissions = string.IsNullOrWhiteSpace(keyword)
                ? _allMissions.ToList()
                : _allMissions.Where(m =>
                    ContainsMissionKeyword(m.MissionName, keyword) ||
                    ContainsMissionKeyword(m.MissionType, keyword) ||
                    ContainsMissionKeyword(m.Location, keyword) ||
                    ContainsMissionKeyword(m.Description, keyword))
                  .ToList();

            dgvMission.DataSource = null;
            dgvMission.DataSource = _displayedMissions;
            dgvMission.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvMission.ScrollBars = ScrollBars.Vertical;

            CustomizeMissionGrid();
            AddSttColumn();

            // Đưa bảng về đầu danh sách sau mỗi lần nạp/lọc dữ liệu,
            // tránh giữ vị trí cuộn cũ khiến tiêu đề cột và các dòng đầu bị khuất.
            dgvMission.ColumnHeadersVisible = true;
            if (dgvMission.Rows.Count > 0)
            {
                try
                {
                    dgvMission.FirstDisplayedScrollingRowIndex = 0;
                }
                catch (InvalidOperationException)
                {
                    // Bảng có thể chưa hoàn tất bố trí; lần hiển thị kế tiếp sẽ bắt đầu từ đầu.
                }
            }
            dgvMission.ClearSelection();

            // Cập nhật lại bố cục sau khi nạp dữ liệu để thanh cuộn dọc
            // xuất hiện tự động khi tổng chiều cao các hàng vượt vùng hiển thị.
            dgvMission.PerformLayout();
            dgvMission.Invalidate();
            UpdateRecordCount(_displayedMissions.Count);
            UpdateEmptyState();
        }

        private static bool ContainsMissionKeyword(string? value, string keyword)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            string normalizedValue = VietnameseHelper
                .RemoveVietnameseSigns(value)
                .ToLowerInvariant();

            return normalizedValue.Contains(keyword);
        }

        private async void
        FrmMissionList_Load(
            object sender,
            EventArgs e)
        {
            await _missionService.RefreshMissionStatusAsync();
            await LoadMissionAsync();

            btnAddMission.Visible =
                PermissionHelper.CanManageMission;

            btnEditMission.Visible =
                PermissionHelper.CanManageMission;

            btnDeleteMission.Visible =
                PermissionHelper.CanManageMission;
        }

        private void
        UpdateRecordCount(
            int count)
        {
            lblRecordCount.Text =
                $"Đang hiển thị: {count} công tác";
        }

        private void
        CustomizeMissionGrid()
        {
            if (dgvMission.Columns.Count == 0)
                return;

            dgvMission.Columns[
                "MissionId"]
                .Visible = false;

            //dgvMission.Columns[
            //    "MissionPersonnel"]
            //    .Visible = false;

            dgvMission.Columns[
                "CreatedAt"]
                .Visible = false;

            dgvMission.Columns[
                "MissionName"]
                .HeaderText =
                    "Tên công tác";

            dgvMission.Columns[
                "MissionType"]
                .HeaderText =
                    "Loại công tác";

            dgvMission.Columns[
                "StartTime"]
                .HeaderText =
                    "Bắt đầu";

            dgvMission.Columns[
                "EndTime"]
                .HeaderText =
                    "Kết thúc";

            dgvMission.Columns[
                "Location"]
                .HeaderText =
                    "Địa điểm";

            dgvMission.Columns[
                "Description"]
                .HeaderText =
                    "Mô tả";

            SetMissionHeader("AllowOverlap", "Cho phép trùng lịch");
            SetMissionHeader("IsActive", "Trạng thái");
            SetMissionHeader("Priority", "Mức độ ưu tiên");
            SetMissionHeader("MissionPersonnel", "CBCS tham gia");
            SetMissionHeader("MissionPersonnels", "CBCS tham gia");

            foreach (DataGridViewColumn column in dgvMission.Columns)
            {
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            }

            dgvMission.Columns["Priority"]
                .DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;

            dgvMission.Columns["Priority"]
                .HeaderCell.Style.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;

            // Phân bổ chiều rộng theo toàn bộ vùng hiển thị, tránh phần header bị cụt ở bên phải.
            SetMissionColumnFill("STT", 0.55F, 48);
            SetMissionColumnFill("MissionName", 1.35F, 125);
            SetMissionColumnFill("MissionType", 1.30F, 125);
            SetMissionColumnFill("AllowOverlap", 1.40F, 135);
            SetMissionColumnFill("StartTime", 1.15F, 110);
            SetMissionColumnFill("EndTime", 1.15F, 110);
            SetMissionColumnFill("Location", 1.25F, 120);
            SetMissionColumnFill("Description", 1.15F, 110);
            SetMissionColumnFill("IsActive", 1.00F, 95);
            SetMissionColumnFill("Priority", 1.15F, 110);
            SetMissionColumnFill("MissionPersonnel", 1.35F, 130);
            SetMissionColumnFill("MissionPersonnels", 1.35F, 130);
        }

        private void SetMissionHeader(string columnName, string headerText)
        {
            if (dgvMission.Columns.Contains(columnName))
                dgvMission.Columns[columnName].HeaderText = headerText;
        }

        private void SetMissionColumnWidth(string columnName, int width)
        {
            if (dgvMission.Columns.Contains(columnName))
                dgvMission.Columns[columnName].Width = width;
        }


        private void SetMissionColumnFill(
            string columnName,
            float fillWeight,
            int minimumWidth)
        {
            if (dgvMission == null ||
                !dgvMission.Columns.Contains(columnName))
            {
                return;
            }

            DataGridViewColumn column =
                dgvMission.Columns[columnName];

            // Thiết lập độ rộng tối thiểu
            column.MinimumWidth = minimumWidth;

            // Cột Frozen không được sử dụng AutoSizeMode.Fill
            if (column.Frozen)
            {
                column.AutoSizeMode =
                    DataGridViewAutoSizeColumnMode.None;

                column.Width = Math.Max(
                    column.Width,
                    minimumWidth
                );

                return;
            }

            // Chỉ áp dụng Fill cho các cột không Frozen
            column.AutoSizeMode =
                DataGridViewAutoSizeColumnMode.Fill;

            column.FillWeight = fillWeight;
        }


        private void AddSttColumn()
        {
            if (dgvMission.Columns["STT"] == null)
            {
                DataGridViewTextBoxColumn sttColumn =
                    new DataGridViewTextBoxColumn();

                sttColumn.Name = "STT";
                sttColumn.HeaderText = "STT";
                sttColumn.Width = 50;
                sttColumn.ReadOnly = true;

                dgvMission.Columns.Insert(
                    0,
                    sttColumn);
            }

            // STT là cột cố định: tuyệt đối không để chế độ Fill kế thừa
            // từ dgvMission.AutoSizeColumnsMode = Fill.
            DataGridViewColumn stt =
                dgvMission.Columns["STT"];

            stt.Visible = true;
            stt.ReadOnly = true;
            stt.MinimumWidth = 55;
            stt.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            stt.Width = 55;
            stt.Frozen = true;
            stt.SortMode = DataGridViewColumnSortMode.NotSortable;
            stt.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            stt.DefaultCellStyle.Padding = Padding.Empty;

            // Gán giá trị dự phòng cho các dòng hiện có.
            // CellFormatting bên dưới tiếp tục sinh STT theo RowIndex,
            // vì vậy số thứ tự vẫn hiển thị ổn định sau khi lọc/sắp xếp/nạp lại.
            for (int i = 0; i < dgvMission.Rows.Count; i++)
            {
                if (!dgvMission.Rows[i].IsNewRow)
                    dgvMission.Rows[i].Cells["STT"].Value = i + 1;
            }
        }

        private async void btnAdd_Click(
            object sender,
            EventArgs e)
        {
            FrmMissionDetail frm =
                new();

            if (frm.ShowDialog()
                == DialogResult.OK)
            {
                await LoadMissionAsync();
            }
        }

        private async void btnEdit_Click(
            object sender,
            EventArgs e)
        {
            if (!TryGetSelectedMission("sửa", out Mission? mission)
                || mission is null)
            {
                return;
            }

            await OpenMissionEditorAsync(mission);
        }

        private async void btnDelete_Click(
            object sender,
            EventArgs e)
        {
            if (!TryGetSelectedMission("xóa", out Mission? mission)
                || mission is null)
            {
                return;
            }

            // Hiển thị hộp thoại xác nhận tùy chỉnh theo giao diện thiết kế.
            DialogResult result = ShowDeleteConfirmation(mission.MissionName);

            if (result != DialogResult.Yes)
                return;

            await _missionService
                .DeleteAsync(
                    mission.MissionId);

            await LoadMissionAsync();
        }


        /// <summary>
        /// Hiển thị hộp thoại xác nhận xóa công tác theo giao diện hiện đại.
        /// DialogResult.Yes chỉ được trả về khi người dùng nhấn nút Có.
        /// </summary>
        private DialogResult ShowDeleteConfirmation(string missionName)
        {
            using var dialog = new MissionDeleteConfirmDialog(missionName);
            return dialog.ShowDialog(this);
        }

        /// <summary>
        /// Hộp thoại xác nhận riêng của FrmMissionList.
        /// Không sửa FrmMissionList.Designer.cs và không ảnh hưởng nghiệp vụ dữ liệu.
        /// </summary>
        private sealed class MissionDeleteConfirmDialog : Form
        {
            private static readonly Color HeaderTop = Color.FromArgb(0, 105, 220);
            private static readonly Color HeaderBottom = Color.FromArgb(0, 70, 165);
            private static readonly Color AccentBlue = Color.FromArgb(0, 103, 230);
            private static readonly Color FooterBlue = Color.FromArgb(232, 244, 255);

            public MissionDeleteConfirmDialog(string missionName)
            {
                Text = "Xác nhận";
                FormBorderStyle = FormBorderStyle.None;
                StartPosition = FormStartPosition.CenterParent;
                ClientSize = new Size(430, 205);
                BackColor = Color.White;
                Font = new Font("Segoe UI", 10F, FontStyle.Regular);
                ShowInTaskbar = false;
                MinimizeBox = false;
                MaximizeBox = false;
                KeyPreview = true;

                BuildDialog(missionName);
                Shown += (s, e) => ApplyDialogRegion();
                Resize += (s, e) => ApplyDialogRegion();
            }

            protected override CreateParams CreateParams
            {
                get
                {
                    CreateParams cp = base.CreateParams;
                    cp.ClassStyle |= 0x00020000; // CS_DROPSHADOW
                    return cp;
                }
            }

            private void BuildDialog(string missionName)
            {
                // Thanh tiêu đề xanh chuyển sắc.
                Panel header = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 48,
                    BackColor = HeaderTop
                };
                header.Paint += (s, e) =>
                {
                    using var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                        header.ClientRectangle, HeaderTop, HeaderBottom,
                        System.Drawing.Drawing2D.LinearGradientMode.Horizontal);
                    e.Graphics.FillRectangle(brush, header.ClientRectangle);
                };

                // Biểu tượng dấu hỏi trong vòng tròn trắng ở thanh tiêu đề,
                // vẽ hoàn toàn bằng GDI+ để hiển thị đồng nhất, không phụ thuộc font ký tự.
                Panel headerQuestion = new Panel
                {
                    Size = new Size(28, 28),
                    Location = new Point(19, 10),
                    BackColor = Color.Transparent
                };
                headerQuestion.Paint += (s, e) =>
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    using var pen = new Pen(Color.White, 1.7F);
                    e.Graphics.DrawEllipse(
                        pen, 1, 1,
                        headerQuestion.Width - 3,
                        headerQuestion.Height - 3);

                    using var font = new Font("Segoe UI", 15F, FontStyle.Bold);
                    using var brush = new SolidBrush(Color.White);
                    var size = e.Graphics.MeasureString("?", font);

                    e.Graphics.DrawString(
                        "?", font, brush,
                        (headerQuestion.Width - size.Width) / 2F,
                        (headerQuestion.Height - size.Height) / 2F - 1F);
                };

                Label title = new Label
                {
                    Text = "Xác nhận",
                    AutoSize = true,
                    Location = new Point(61, 9),
                    ForeColor = Color.White,
                    BackColor = Color.Transparent,
                    Font = new Font("Segoe UI", 14F, FontStyle.Bold)
                };

                Label close = new Label
                {
                    Text = "×",
                    Size = new Size(38, 38),
                    Location = new Point(ClientSize.Width - 44, 5),
                    ForeColor = Color.White,
                    BackColor = Color.Transparent,
                    Font = new Font("Segoe UI", 22F, FontStyle.Regular),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Cursor = Cursors.Hand
                };
                close.Click += (s, e) => DialogResult = DialogResult.No;
                close.MouseEnter += (s, e) => close.BackColor = Color.FromArgb(35, 255, 255, 255);
                close.MouseLeave += (s, e) => close.BackColor = Color.Transparent;

                header.Controls.Add(headerQuestion);
                header.Controls.Add(title);
                header.Controls.Add(close);

                // Khu vực nội dung: biểu tượng hỏi, vạch phân cách và thông điệp.
                Panel body = new Panel
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.White
                };

                Label bodyQuestion = new Label
                {
                    Text = "?",
                    Size = new Size(49, 49),
                    Location = new Point(27, 19),
                    BackColor = Color.Transparent,
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 30F, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                bodyQuestion.Paint += (s, e) =>
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    using var brush = new SolidBrush(Color.FromArgb(0, 151, 220));
                    e.Graphics.FillEllipse(brush, 0, 0, bodyQuestion.Width - 1, bodyQuestion.Height - 1);
                    using var font = new Font("Segoe UI", 30F, FontStyle.Bold);
                    using var textBrush = new SolidBrush(Color.White);
                    var textSize = e.Graphics.MeasureString("?", font);
                    e.Graphics.DrawString("?", font, textBrush,
                        (bodyQuestion.Width - textSize.Width) / 2F,
                        (bodyQuestion.Height - textSize.Height) / 2F - 1F);
                };

                Panel separator = new Panel
                {
                    BackColor = Color.FromArgb(174, 205, 235),
                    Location = new Point(96, 18),
                    Size = new Size(1, 52)
                };

                Label message = new Label
                {
                    Text = $"Xóa công tác \"{missionName}\" ?",
                    Location = new Point(114, 12),
                    Size = new Size(294, 66),
                    ForeColor = Color.FromArgb(20, 48, 83),
                    BackColor = Color.White,
                    Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleLeft,
                    AutoEllipsis = true
                };

                body.Controls.Add(bodyQuestion);
                body.Controls.Add(separator);
                body.Controls.Add(message);

                // Chân hộp thoại và hai nút lựa chọn.
                Panel footer = new Panel
                {
                    Dock = DockStyle.Bottom,
                    Height = 70,
                    BackColor = FooterBlue
                };

                Button yesButton = CreateDialogButton("✓  Có", true);
                yesButton.SetBounds(111, 15, 118, 38);
                yesButton.Click += (s, e) => DialogResult = DialogResult.Yes;

                Button noButton = CreateDialogButton("×  Không", false);
                noButton.SetBounds(245, 15, 118, 38);
                noButton.Click += (s, e) => DialogResult = DialogResult.No;

                footer.Controls.Add(yesButton);
                footer.Controls.Add(noButton);

                Controls.Add(body);
                Controls.Add(footer);
                Controls.Add(header);

                AcceptButton = yesButton;
                CancelButton = noButton;
            }

            private static Button CreateDialogButton(string text, bool primary)
            {
                Color blue = Color.FromArgb(0, 103, 230);
                Button button = new Button
                {
                    Text = text,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = primary ? Color.White : Color.FromArgb(20, 48, 83),
                    BackColor = primary ? blue : Color.White,
                    FlatStyle = FlatStyle.Flat,
                    UseVisualStyleBackColor = false,
                    Cursor = Cursors.Hand,
                    TabStop = true
                };
                button.FlatAppearance.BorderSize = primary ? 0 : 1;
                button.FlatAppearance.BorderColor = blue;
                button.FlatAppearance.MouseOverBackColor = primary
                    ? Color.FromArgb(0, 88, 200)
                    : Color.FromArgb(241, 248, 255);
                button.FlatAppearance.MouseDownBackColor = primary
                    ? Color.FromArgb(0, 72, 175)
                    : Color.FromArgb(224, 240, 255);

                button.Resize += (s, e) =>
                {
                    using var path = CreateRoundedPath(
                        new Rectangle(0, 0, button.Width - 1, button.Height - 1), 8);
                    button.Region?.Dispose();
                    button.Region = new Region(path);
                };
                button.Paint += (s, e) =>
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    if (!primary)
                    {
                        using var path = CreateRoundedPath(
                            new Rectangle(1, 1, button.Width - 3, button.Height - 3), 8);
                        using var pen = new Pen(blue, 1.2F);
                        e.Graphics.DrawPath(pen, path);
                    }
                };
                return button;
            }

            private void ApplyDialogRegion()
            {
                if (ClientSize.Width <= 0 || ClientSize.Height <= 0) return;
                using var path = CreateRoundedPath(
                    new Rectangle(0, 0, ClientSize.Width - 1, ClientSize.Height - 1), 13);
                Region?.Dispose();
                Region = new Region(path);
            }

            private static System.Drawing.Drawing2D.GraphicsPath CreateRoundedPath(
                Rectangle bounds, int radius)
            {
                int diameter = radius * 2;
                var path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
                path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
                path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter,
                    diameter, diameter, 0, 90);
                path.AddArc(bounds.X, bounds.Bottom - diameter,
                    diameter, diameter, 90, 90);
                path.CloseFigure();
                return path;
            }
        }

        /// <summary>
        /// Hiển thị thông báo khi danh sách công tác đang trống.
        /// Dùng hộp thoại riêng để đồng bộ với giao diện xác nhận xóa.
        /// </summary>
        private void ShowInfoNotification(string message)
        {
            using var dialog = new MissionInfoDialog(message);
            dialog.ShowDialog(this);
        }

        /// <summary>
        /// Hộp thoại thông báo thông tin, dùng cho trạng thái chưa có dữ liệu.
        /// </summary>
        private sealed class MissionInfoDialog : Form
        {
            private static readonly Color HeaderTop = Color.FromArgb(0, 105, 220);
            private static readonly Color HeaderBottom = Color.FromArgb(0, 70, 165);
            private static readonly Color AccentBlue = Color.FromArgb(0, 151, 220);
            private static readonly Color FooterBlue = Color.FromArgb(232, 244, 255);

            public MissionInfoDialog(string message)
            {
                Text = "Thông báo";
                FormBorderStyle = FormBorderStyle.None;
                StartPosition = FormStartPosition.CenterParent;
                ClientSize = new Size(430, 198);
                BackColor = Color.White;
                Font = new Font("Segoe UI", 10F, FontStyle.Regular);
                ShowInTaskbar = false;
                MinimizeBox = false;
                MaximizeBox = false;
                KeyPreview = true;

                BuildDialog(message);
                Shown += (s, e) => ApplyDialogRegion();
                Resize += (s, e) => ApplyDialogRegion();
            }

            protected override CreateParams CreateParams
            {
                get
                {
                    CreateParams cp = base.CreateParams;
                    cp.ClassStyle |= 0x00020000; // CS_DROPSHADOW
                    return cp;
                }
            }

            private void BuildDialog(string messageText)
            {
                Panel header = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 48,
                    BackColor = HeaderTop
                };
                header.Paint += (s, e) =>
                {
                    using var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                        header.ClientRectangle, HeaderTop, HeaderBottom,
                        System.Drawing.Drawing2D.LinearGradientMode.Horizontal);
                    e.Graphics.FillRectangle(brush, header.ClientRectangle);
                };

                // Biểu tượng chữ i trong vòng tròn trắng ở thanh tiêu đề.
                Panel headerInfo = new Panel
                {
                    Size = new Size(28, 28),
                    Location = new Point(19, 10),
                    BackColor = Color.Transparent
                };
                headerInfo.Paint += (s, e) =>
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    using var pen = new Pen(Color.White, 1.7F);
                    e.Graphics.DrawEllipse(pen, 1, 1,
                        headerInfo.Width - 3, headerInfo.Height - 3);
                    using var font = new Font("Segoe UI", 16F, FontStyle.Bold);
                    using var brush = new SolidBrush(Color.White);
                    var size = e.Graphics.MeasureString("i", font);
                    e.Graphics.DrawString("i", font, brush,
                        (headerInfo.Width - size.Width) / 2F,
                        (headerInfo.Height - size.Height) / 2F - 1F);
                };

                Label title = new Label
                {
                    Text = "Thông báo",
                    AutoSize = true,
                    Location = new Point(61, 9),
                    ForeColor = Color.White,
                    BackColor = Color.Transparent,
                    Font = new Font("Segoe UI", 14F, FontStyle.Bold)
                };

                Label close = new Label
                {
                    Text = "×",
                    Size = new Size(38, 38),
                    Location = new Point(ClientSize.Width - 44, 5),
                    ForeColor = Color.White,
                    BackColor = Color.Transparent,
                    Font = new Font("Segoe UI", 22F, FontStyle.Regular),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Cursor = Cursors.Hand
                };
                close.Click += (s, e) => DialogResult = DialogResult.OK;
                close.MouseEnter += (s, e) => close.BackColor = Color.FromArgb(35, 255, 255, 255);
                close.MouseLeave += (s, e) => close.BackColor = Color.Transparent;

                header.Controls.Add(headerInfo);
                header.Controls.Add(title);
                header.Controls.Add(close);

                Panel body = new Panel
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.White
                };

                // Biểu tượng thông tin màu xanh ở phần nội dung.
                Panel bodyInfo = new Panel
                {
                    Size = new Size(49, 49),
                    Location = new Point(27, 19),
                    BackColor = Color.Transparent
                };
                bodyInfo.Paint += (s, e) =>
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    using var fill = new SolidBrush(AccentBlue);
                    e.Graphics.FillEllipse(fill, 0, 0, bodyInfo.Width - 1, bodyInfo.Height - 1);
                    using var font = new Font("Segoe UI", 29F, FontStyle.Bold);
                    using var brush = new SolidBrush(Color.White);
                    var size = e.Graphics.MeasureString("i", font);
                    e.Graphics.DrawString("i", font, brush,
                        (bodyInfo.Width - size.Width) / 2F,
                        (bodyInfo.Height - size.Height) / 2F - 1F);
                };

                Panel separator = new Panel
                {
                    BackColor = Color.FromArgb(174, 205, 235),
                    Location = new Point(96, 18),
                    Size = new Size(1, 52)
                };

                Label message = new Label
                {
                    Text = messageText,
                    Location = new Point(114, 12),
                    Size = new Size(294, 66),
                    ForeColor = Color.FromArgb(20, 48, 83),
                    BackColor = Color.White,
                    Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleLeft,
                    AutoEllipsis = false
                };

                body.Controls.Add(bodyInfo);
                body.Controls.Add(separator);
                body.Controls.Add(message);

                Panel footer = new Panel
                {
                    Dock = DockStyle.Bottom,
                    Height = 56,
                    BackColor = FooterBlue
                };

                Button okButton = CreateDialogButton("✓  OK", true);
                okButton.SetBounds(268, 10, 118, 34);
                okButton.Click += (s, e) => DialogResult = DialogResult.OK;
                footer.Controls.Add(okButton);

                Controls.Add(body);
                Controls.Add(footer);
                Controls.Add(header);

                AcceptButton = okButton;
                CancelButton = okButton;
            }

            private static Button CreateDialogButton(string text, bool primary)
            {
                Color blue = Color.FromArgb(0, 103, 230);
                Button button = new Button
                {
                    Text = text,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = blue,
                    FlatStyle = FlatStyle.Flat,
                    UseVisualStyleBackColor = false,
                    Cursor = Cursors.Hand,
                    TabStop = true
                };
                button.FlatAppearance.BorderSize = 0;
                button.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 88, 200);
                button.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 72, 175);
                button.Resize += (s, e) =>
                {
                    if (button.Width <= 0 || button.Height <= 0) return;
                    using var path = CreateRoundedPath(
                        new Rectangle(0, 0, button.Width - 1, button.Height - 1), 8);
                    button.Region?.Dispose();
                    button.Region = new Region(path);
                };
                return button;
            }

            private void ApplyDialogRegion()
            {
                if (ClientSize.Width <= 0 || ClientSize.Height <= 0) return;
                using var path = CreateRoundedPath(
                    new Rectangle(0, 0, ClientSize.Width - 1, ClientSize.Height - 1), 13);
                Region?.Dispose();
                Region = new Region(path);
            }

            private static System.Drawing.Drawing2D.GraphicsPath CreateRoundedPath(
                Rectangle bounds, int radius)
            {
                int diameter = radius * 2;
                var path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
                path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
                path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter,
                    diameter, diameter, 0, 90);
                path.AddArc(bounds.X, bounds.Bottom - diameter,
                    diameter, diameter, 90, 90);
                path.CloseFigure();
                return path;
            }
        }

        /// <summary>
        /// Kiểm tra dữ liệu và dòng công tác được người dùng chọn.
        /// Dùng SelectedRows thay vì CurrentRow vì CurrentRow có thể tự
        /// trỏ vào dòng đầu tiên ngay sau khi DataSource được nạp,
        /// dù người dùng chưa thực sự chọn công tác.
        /// </summary>
        private bool TryGetSelectedMission(
            string action,
            out Mission? mission)
        {
            mission = null;

            if (_allMissions == null || _allMissions.Count == 0)
            {
                string emptyMessage = action == "sửa"
                    ? "Hiện chưa có công tác nào để sửa"
                    : "Hiện chưa có công tác nào để xoá";

                ShowInfoNotification(emptyMessage);

                return false;
            }

            if (dgvMission.SelectedRows.Count == 0)
            {
                string selectionMessage = action == "sửa"
                    ? "Hãy chọn có công tác để sửa"
                    : "Hãy chọn có công tác để xoá";

                MessageBox.Show(
                    selectionMessage,
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            mission =
                dgvMission.SelectedRows[0].DataBoundItem as Mission;

            if (mission == null)
            {
                string selectionMessage = action == "sửa"
                    ? "Hãy chọn có công tác để sửa"
                    : "Hãy chọn có công tác để xoá";

                MessageBox.Show(
                    selectionMessage,
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            return true;
        }

        private async Task OpenMissionEditorAsync(Mission mission)
        {
            FrmMissionDetail frm =
                new(mission);

            if (frm.ShowDialog() == DialogResult.OK)
            {
                await LoadMissionAsync();
            }
        }

        private async void dgvMission_CellDoubleClick(
            object? sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex < 0)
                return;

            await EditMissionAsync();
        }

        private async Task EditMissionAsync()
        {
            if (!TryGetSelectedMission("sửa", out Mission? mission)
                || mission is null)
            {
                return;
            }

            await OpenMissionEditorAsync(mission);
        }

        private async void dgvMission_KeyDown(
            object? sender,
            KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                await EditMissionAsync();
            }
        }

        private void btnAssign_Click(
            object sender,
            EventArgs e)
        {
            // Khi chưa có dữ liệu công tác, sử dụng hộp thoại thông báo
            // riêng để đồng bộ giao diện với chức năng Sửa và Xóa.
            if (_allMissions == null
                || _allMissions.Count == 0
                || dgvMission.Rows.Count == 0)
            {
                ShowInfoNotification("Chưa có công tác để phân công.");
                return;
            }

            if (dgvMission.CurrentRow == null
                || dgvMission.CurrentRow.DataBoundItem is not Mission mission)
            {
                ShowInfoNotification("Vui lòng chọn công tác để phân công.");
                return;
            }

            FrmMissionPersonnel frm =
                new(
                    mission.MissionId,
                    mission.MissionName);

            frm.ShowDialog();
        }

        private void dgvMission_CellFormatting(
            object? sender,
            DataGridViewCellFormattingEventArgs e)

        {
            string columnName = dgvMission.Columns[e.ColumnIndex].Name;

            // Sinh STT trực tiếp khi DataGridView định dạng ô.
            // Cách này không phụ thuộc vào thời điểm DataSource tạo lại các dòng.
            if (columnName == "STT")
            {
                if (e.RowIndex >= 0)
                {
                    e.Value = (e.RowIndex + 1).ToString();

                    if (e.CellStyle != null)
                    {
                        e.CellStyle.Alignment =
                            DataGridViewContentAlignment.MiddleCenter;
                    }

                    e.FormattingApplied = true;
                }
                return;
            }

            if (columnName != "Priority" || e.Value == null)
                return;

            if (e.CellStyle is null)
                return;

            var style = e.CellStyle;

            int priority =
                Convert.ToInt32(e.Value);

            switch (priority)
            {
                case 4:

                    e.Value = "Khẩn";

                    style.BackColor =
                        Color.FromArgb(255, 199, 206);

                    style.ForeColor =
                        Color.DarkRed;

                    style.SelectionBackColor =
                        Color.Red;

                    style.SelectionForeColor =
                        Color.White;

                    style.Font =
                        _priorityBoldFont;

                    break;

                case 3:

                    e.Value = "Cao";

                    style.BackColor =
                        Color.FromArgb(255, 235, 156);

                    style.ForeColor =
                        Color.DarkOrange;

                    style.Font =
                        _priorityBoldFont;

                    break;

                case 2:

                    e.Value = "Bình thường";

                    style.BackColor =
                        Color.FromArgb(221, 235, 247);

                    style.ForeColor =
                        Color.DarkBlue;

                    break;

                case 1:

                    e.Value = "Thấp";

                    style.BackColor =
                        Color.Gainsboro;

                    style.ForeColor =
                        Color.DimGray;

                    break;
            }

            e.FormattingApplied = true;
        }

        private async void btnRefresh_Click(
            object? sender,
            EventArgs e)
        {
            await _missionService.RefreshMissionStatusAsync();
            await LoadMissionAsync();
        }
    }
}