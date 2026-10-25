using QuanLyCBCS.DTOs;
using QuanLyCBCS.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCBCS.Forms.CommonForms
{
    public partial class FrmNotificationCenter : Form
    {
        private readonly NotificationService _notificationService;
        private List<NotificationDto> _notifications = new List<NotificationDto>();
        private bool _isLoading;

        // Đánh dấu khi form được nhúng vào dropdown của FrmHome.
        private bool _isHostedInDropdown;
        private readonly Panel _headerDivider = new Panel();

        public void SetHostedInDropdown(bool hosted)
        {
            _isHostedInDropdown = hosted;

            if (hosted)
            {
                TopLevel = false;
                FormBorderStyle = FormBorderStyle.None;
                ShowInTaskbar = false;
                MinimumSize = Size.Empty;
                StartPosition = FormStartPosition.Manual;
                LayoutHostedView();
            }
        }

        public FrmNotificationCenter()
        {
            InitializeComponent();
            dgvNotifications.SendToBack();
            emptyStatePanel.BringToFront();
            _notificationService = new NotificationService();

            dgvNotifications.CellFormatting += dgvNotifications_CellFormatting;
            dgvNotifications.DataBindingComplete += dgvNotifications_DataBindingComplete;
            gridContainer.Resize += (_, __) => PositionEmptyState();
            gridContainer.Paint += GridContainer_Paint;
            headerPanel.Paint += HeaderPanel_Paint;
            iconPanel.Paint += IconPanel_Paint;
            _headerDivider.BackColor = Color.FromArgb(115, 190, 231);
            headerPanel.Controls.Add(_headerDivider);
            _headerDivider.BringToFront();
            searchIconPanel.Paint += SearchIconPanel_Paint;
            searchBorderPanel.Paint += SearchBorderPanel_Paint;
            searchBorderPanel.BorderStyle = BorderStyle.None;
            searchBorderPanel.BackColor = Color.White;
            iconPanel.BackColor = Color.Transparent;
            emptyBellPanel.Paint += EmptyBellPanel_Paint;
            Resize += (_, __) => { if (_isHostedInDropdown) LayoutHostedView(); };
        }

        // Bố cục riêng cho dropdown trong FrmHome: tự co giãn theo kích thước panel,
        // không để các kích thước thiết kế cho cửa sổ toàn màn hình tràn khỏi dropdown.
        private void LayoutHostedView()
        {
            if (!_isHostedInDropdown || IsDisposed || headerPanel == null || toolbarPanel == null ||
                gridContainer == null || dgvNotifications == null) return;

            int w = Math.Max(320, ClientSize.Width);
            int h = Math.Max(220, ClientSize.Height);
            int margin = Math.Max(9, Math.Min(13, w / 50));
            int headerH = Math.Max(58, Math.Min(68, (int)(h * 0.17)));
            int toolbarH = Math.Max(46, Math.Min(54, (int)(h * 0.14)));

            SuspendLayout();
            headerPanel.Height = headerH;
            toolbarPanel.Height = toolbarH;
            toolbarPanel.Padding = new Padding(margin, 5, margin, 5);

            // Biểu tượng lớn hơn một chút, không để nền trắng lọt ra ngoài góc bo.
            int iconSize = Math.Min(46, headerH - 10);
            iconPanel.SetBounds(margin, (headerH - iconSize) / 2, iconSize, iconSize);
            iconPanel.BackColor = Color.Transparent;
            ApplyRoundedRegion(iconPanel, 6);
            _headerDivider.SetBounds(iconPanel.Right + 9, (headerH - 30) / 2, 1, 30);
            _headerDivider.BringToFront();

            lblTitle.Text = "TRUNG TÂM THÔNG BÁO";
            lblTitle.Font = new Font("Segoe UI", Math.Max(12F, Math.Min(17F, w / 39F)), FontStyle.Bold, GraphicsUnit.Point);
            lblTitle.SetBounds(_headerDivider.Right + 10, 0,
                Math.Max(100, w - _headerDivider.Right - margin - 10), headerH);
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblTitle.AutoEllipsis = true;
            lblSubtitle.Visible = false;

            int buttonH = Math.Max(29, toolbarH - 12);
            int closeW = Math.Max(66, Math.Min(76, w / 8));
            int refreshW = Math.Max(90, Math.Min(98, w / 6));
            int right = w - margin;
            int toolbarContentYOffset = 5; // Hạ ô tìm kiếm và hai nút xuống nhẹ để cân đối với header.
            btnClose.SetBounds(right - closeW, (toolbarH - buttonH) / 2 + toolbarContentYOffset, closeW, buttonH);
            btnRefresh.SetBounds(btnClose.Left - 6 - refreshW, (toolbarH - buttonH) / 2 + toolbarContentYOffset, refreshW, buttonH);
            btnClose.Font = new Font("Segoe UI Semibold", 8.2F, FontStyle.Bold, GraphicsUnit.Point);
            btnRefresh.Font = new Font("Segoe UI Semibold", 8.2F, FontStyle.Bold, GraphicsUnit.Point);
            btnClose.Text = "✕ Đóng";
            btnRefresh.Text = "⟳ Làm mới";
            btnClose.Cursor = Cursors.Hand;
            btnRefresh.Cursor = Cursors.Hand;
            ApplyRoundedRegion(btnClose, 6);
            ApplyRoundedRegion(btnRefresh, 6);

            int searchX = margin;
            int searchW = Math.Max(100, btnRefresh.Left - margin - searchX - 8);
            int searchH = buttonH;
            searchBorderPanel.SetBounds(searchX, (toolbarH - searchH) / 2 + toolbarContentYOffset, searchW, searchH);
            searchBorderPanel.BorderStyle = BorderStyle.None;
            // Không cắt Region của ô tìm kiếm: đường viền tự vẽ cần hiển thị đủ cả 4 góc.
            searchBorderPanel.Region?.Dispose();
            searchBorderPanel.Region = null;
            searchIconPanel.SetBounds(3, 0, 27, searchH);
            txtSearch.Multiline = false;
            txtSearch.BorderStyle = BorderStyle.None;
            txtSearch.Font = new Font("Segoe UI", 8.8F, FontStyle.Regular, GraphicsUnit.Point);
            txtSearch.TextAlign = HorizontalAlignment.Left;
            txtSearch.Margin = Padding.Empty;
            // TextBox một dòng cần chiều cao native vừa với font. Căn chính TextBox
            // vào giữa ô ngoài, thay vì kéo TextBox cao bằng toàn bộ toolbar.
            int textBoxH = Math.Min(searchH, Math.Max(txtSearch.PreferredHeight, txtSearch.Font.Height + 8));
            txtSearch.SetBounds(31, Math.Max(0, (searchH - textBoxH) / 2),
                Math.Max(45, searchW - 36), textBoxH);

            // Vẽ viền bo góc trên Panel ngoài và chừa vùng trống để các control con
            // không che mất nét viền ở bốn góc.
            gridContainer.Dock = DockStyle.None;
            gridContainer.BorderStyle = BorderStyle.None;
            gridContainer.Margin = Padding.Empty;
            gridContainer.Padding = Padding.Empty;
            gridContainer.SetBounds(margin, headerH + toolbarH + margin,
                Math.Max(100, w - margin * 2), Math.Max(70, h - headerH - toolbarH - margin * 2));
            ApplyRoundedRegion(gridContainer, 9);
            dgvNotifications.Dock = DockStyle.None;
            dgvNotifications.SetBounds(3, 3, Math.Max(50, gridContainer.ClientSize.Width - 6),
                Math.Max(40, gridContainer.ClientSize.Height - 6));
            // Bo góc trực tiếp DataGridView để phần header màu xanh không bị vuông
            // đè lên góc bo của gridContainer. Được áp dụng lại mỗi lần layout đổi size.
            ApplyRoundedRegion(dgvNotifications, 8);
            dgvNotifications.ColumnHeadersHeight = 29;
            dgvNotifications.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold, GraphicsUnit.Point);
            dgvNotifications.DefaultCellStyle.Font = new Font("Segoe UI", 8.2F, FontStyle.Regular, GraphicsUnit.Point);
            dgvNotifications.RowTemplate.Height = 25;
            if (dgvNotifications.Columns.Contains("Type")) dgvNotifications.Columns["Type"].MinimumWidth = 62;
            if (dgvNotifications.Columns.Contains("Title")) dgvNotifications.Columns["Title"].MinimumWidth = 75;
            if (dgvNotifications.Columns.Contains("Message")) dgvNotifications.Columns["Message"].MinimumWidth = 90;
            if (dgvNotifications.Columns.Contains("Date")) dgvNotifications.Columns["Date"].MinimumWidth = 65;

            // Chuông trạng thái rỗng dùng hệ tọa độ vuông để không bị kéo dẹt.
            int bellSize = Math.Max(58, Math.Min(74, Math.Min(w / 7, h / 4)));
            emptyBellPanel.Size = new Size(bellSize, bellSize);
            lblEmptyTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            lblEmptyDescription.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            PositionEmptyState();
            ResumeLayout(true);
        }

        private static void ApplyRoundedRegion(Control control, int radius)
        {
            if (control.Width <= 0 || control.Height <= 0) return;
            control.Region?.Dispose();
            using (GraphicsPath path = RoundedRectangle(new Rectangle(0, 0, control.Width, control.Height), radius))
                control.Region = new Region(path);
        }

        private void HeaderPanel_Paint(object? sender, PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(
                headerPanel.ClientRectangle,
                Color.FromArgb(16, 54, 101),
                Color.FromArgb(43, 111, 177),
                LinearGradientMode.Horizontal))
            {
                e.Graphics.FillRectangle(brush, headerPanel.ClientRectangle);
            }
            using (Pen pen = new Pen(Color.FromArgb(32, 255, 255, 255), 2))
            {
                e.Graphics.DrawEllipse(pen, headerPanel.Width - 225, -70, 260, 260);
                e.Graphics.DrawEllipse(pen, headerPanel.Width - 208, -53, 226, 226);
            }
            using (Font starFont = new Font("Segoe UI Symbol", 74F, FontStyle.Regular, GraphicsUnit.Pixel))
            using (SolidBrush starBrush = new SolidBrush(Color.FromArgb(35, Color.White)))
            {
                e.Graphics.DrawString("★", starFont, starBrush, headerPanel.Width - 170, -28);
            }
        }

        private void IconPanel_Paint(object? sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath path = RoundedRectangle(iconPanel.ClientRectangle, 6))
            using (LinearGradientBrush brush = new LinearGradientBrush(
                iconPanel.ClientRectangle,
                Color.FromArgb(22, 77, 137),
                Color.FromArgb(40, 126, 207),
                LinearGradientMode.Vertical))
            {
                e.Graphics.FillPath(brush, path);
            }
            float scale = Math.Min(iconPanel.Width, iconPanel.Height) / 38f;
            e.Graphics.TranslateTransform((iconPanel.Width - 42 * scale) / 2f, (iconPanel.Height - 42 * scale) / 2f);
            e.Graphics.ScaleTransform(scale, scale);
            using (Pen bellPen = new Pen(Color.White, 2.2f) { LineJoin = LineJoin.Round, StartCap = LineCap.Round, EndCap = LineCap.Round })
            using (GraphicsPath bell = new GraphicsPath())
            {
                bell.StartFigure();
                bell.AddBezier(13, 29, 15, 26, 14, 22, 15, 16);
                bell.AddBezier(15, 16, 16, 8, 26, 8, 27, 16);
                bell.AddBezier(27, 16, 28, 22, 27, 26, 30, 29);
                bell.AddLine(30, 29, 12, 29);
                e.Graphics.DrawPath(bellPen, bell);
                e.Graphics.DrawArc(bellPen, 18, 28, 6, 5, 0, 180);
            }
            e.Graphics.ResetTransform();
        }

        private void GridContainer_Paint(object? sender, PaintEventArgs e)
        {
            if (gridContainer.Width < 4 || gridContainer.Height < 4) return;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath path = RoundedRectangle(
                new Rectangle(1, 1, gridContainer.Width - 3, gridContainer.Height - 3), 9))
            using (Pen pen = new Pen(Color.FromArgb(173, 190, 208), 1.2F))
            {
                e.Graphics.DrawPath(pen, path);
            }
        }

        private void SearchBorderPanel_Paint(object? sender, PaintEventArgs e)
        {
            if (searchBorderPanel.Width < 2 || searchBorderPanel.Height < 2) return;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath path = RoundedRectangle(
                new Rectangle(1, 1, searchBorderPanel.Width - 3, searchBorderPanel.Height - 3), 7))
            using (Pen pen = new Pen(Color.FromArgb(151, 195, 238), 1.4F))
            {
                e.Graphics.DrawPath(pen, path);
            }
        }

        private void SearchIconPanel_Paint(object? sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            float scale = Math.Min(searchIconPanel.Width, searchIconPanel.Height) / 27f;
            e.Graphics.TranslateTransform((searchIconPanel.Width - 27 * scale) / 2f, (searchIconPanel.Height - 27 * scale) / 2f);
            e.Graphics.ScaleTransform(scale, scale);
            using (Pen pen = new Pen(Color.FromArgb(69, 117, 169), 1.8f))
            {
                e.Graphics.DrawEllipse(pen, 5, 4, 12, 12);
                e.Graphics.DrawLine(pen, 15, 15, 21, 21);
            }
            e.Graphics.ResetTransform();
        }

        private void EmptyBellPanel_Paint(object? sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle r = new Rectangle(0, 0, emptyBellPanel.Width - 1, emptyBellPanel.Height - 1);
            using (SolidBrush background = new SolidBrush(Color.FromArgb(239, 247, 254)))
                e.Graphics.FillEllipse(background, r);
            float scale = Math.Min(emptyBellPanel.Width, emptyBellPanel.Height) / 66f;
            // Giữ nền tròn nguyên vẹn; chỉ tăng chiều rộng nét chuông để biểu tượng cân đối.
            float bellScaleX = scale * 1.28f;
            float bellScaleY = scale;
            e.Graphics.TranslateTransform((emptyBellPanel.Width - 66 * bellScaleX) / 2f, (emptyBellPanel.Height - 66 * bellScaleY) / 2f);
            e.Graphics.ScaleTransform(bellScaleX, bellScaleY);
            using (Pen bellPen = new Pen(Color.FromArgb(103, 174, 239), 2.4f) { LineJoin = LineJoin.Round, StartCap = LineCap.Round, EndCap = LineCap.Round })
            using (GraphicsPath bell = new GraphicsPath())
            {
                bell.StartFigure();
                bell.AddBezier(23, 45, 26, 40, 25, 32, 27, 22);
                bell.AddBezier(27, 22, 29, 12, 38, 12, 40, 22);
                bell.AddBezier(40, 22, 42, 32, 41, 40, 44, 45);
                bell.AddLine(44, 45, 23, 45);
                e.Graphics.DrawPath(bellPen, bell);
                e.Graphics.DrawArc(bellPen, 31, 44, 6, 5, 0, 180);
            }
            e.Graphics.ResetTransform();
        }

        private static GraphicsPath RoundedRectangle(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private async void FrmNotificationCenter_Load(object sender, EventArgs e)
        {
            await LoadNotificationsAsync();
        }

        private async Task LoadNotificationsAsync()
        {
            if (_isLoading) return;
            _isLoading = true;
            btnRefresh.Enabled = false;
            try
            {
                _notifications = (await _notificationService.GetNotificationsAsync()) ?? new List<NotificationDto>();
                ApplyFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải thông báo.\n" + ex.Message,
                    "Trung tâm thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _notifications = new List<NotificationDto>();
                ApplyFilter();
            }
            finally
            {
                _isLoading = false;
                btnRefresh.Enabled = true;
            }
        }

        private void ApplyFilter()
        {
            string keyword = (txtSearch.Text ?? string.Empty).Trim();
            IEnumerable<NotificationDto> query = _notifications;
            if (keyword.Length > 0)
            {
                query = query.Where(x =>
                    ContainsText(x.Title, keyword) ||
                    ContainsText(x.Message, keyword) ||
                    ContainsText(GetTypeLabel(x.Type), keyword));
            }

            dgvNotifications.DataSource = null;
            dgvNotifications.DataSource = query.Select(x => new NotificationRow
            {
                Type = GetTypeLabel(x.Type),
                Title = x.Title ?? string.Empty,
                Message = x.Message ?? string.Empty,
                Date = x.Date
            }).ToList();

            CustomizeGrid();
            UpdateEmptyState();
        }

        private static bool ContainsText(string source, string keyword)
        {
            return (source ?? string.Empty).IndexOf(keyword, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        private static string GetTypeLabel(string type)
        {
            switch (type)
            {
                case "BIRTHDAY": return "Sinh nhật";
                case "MISSION_ENDING": return "Sắp kết thúc";
                case "MISSION_OVERDUE": return "Quá hạn";
                default: return type ?? "Thông báo";
            }
        }

        private void CustomizeGrid()
        {
            if (dgvNotifications.Columns.Count == 0) return;

            SetColumn("Type", "Loại", 22F);
            SetColumn("Title", "Tiêu đề", 27F);
            SetColumn("Message", "Nội dung", 28F);
            SetColumn("Date", "Ngày", 23F);

            if (dgvNotifications.Columns.Contains("Date"))
                dgvNotifications.Columns["Date"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        private void SetColumn(string name, string header, float fillWeight)
        {
            if (!dgvNotifications.Columns.Contains(name)) return;
            dgvNotifications.Columns[name].HeaderText = header;
            dgvNotifications.Columns[name].FillWeight = fillWeight;
            dgvNotifications.Columns[name].MinimumWidth = 90;
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadNotificationsAsync();
        }

        private void btnClose_Click(
            object sender,
            EventArgs e)
        {
            if (_isHostedInDropdown && Parent != null)
            {
                // Khi nhúng trong FrmHome, chỉ ẩn dropdown.
                // Không đóng hoặc hủy form con.
                Parent.Visible = false;
                return;
            }

            // Khi mở độc lập, giữ nguyên chức năng đóng form.
            Close();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void dgvNotifications_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvNotifications.Rows.Count) return;
            DataGridViewRow row = dgvNotifications.Rows[e.RowIndex];
            if (row.DataBoundItem is not NotificationRow item) return;

            switch (item.Type)
            {
                case "Sinh nhật":
                    row.DefaultCellStyle.BackColor = Color.FromArgb(232, 249, 239);
                    break;
                case "Sắp kết thúc":
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 248, 225);
                    break;
                case "Quá hạn":
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 233);
                    break;
                default:
                    row.DefaultCellStyle.BackColor = e.RowIndex % 2 == 0 ? Color.White : Color.FromArgb(244, 249, 254);
                    break;
            }
        }

        private void dgvNotifications_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
        {
            CustomizeGrid();
            UpdateEmptyState();
        }

        private void UpdateEmptyState()
        {
            bool empty = dgvNotifications.Rows.Count == 0;
            emptyStatePanel.Visible = empty;
            PositionEmptyState();
            lblEmptyTitle.Text = string.IsNullOrWhiteSpace(txtSearch.Text)
                ? "Chưa có thông báo"
                : "Không tìm thấy thông báo";
            lblEmptyDescription.Text = string.IsNullOrWhiteSpace(txtSearch.Text)
                ? "Thông báo mới sẽ hiển thị tại đây"
                : "Thử tìm kiếm bằng từ khóa khác";
        }

        private void PositionEmptyState()
        {
            if (gridContainer == null || emptyStatePanel == null) return;
            int headerHeight = dgvNotifications.ColumnHeadersVisible ? dgvNotifications.ColumnHeadersHeight + 1 : 0;
            // Chừa 3 px quanh khung để viền bo của gridContainer luôn nhìn thấy.
            emptyStatePanel.Location = new Point(3, headerHeight + 3);
            emptyStatePanel.Size = new Size(Math.Max(0, gridContainer.ClientSize.Width - 6),
                Math.Max(0, gridContainer.ClientSize.Height - headerHeight - 6));
            ApplyRoundedRegion(emptyStatePanel, 8);

            int contentHeight = emptyStatePanel.Height;
            emptyBellPanel.Location = new Point(
                Math.Max(0, (emptyStatePanel.ClientSize.Width - emptyBellPanel.Width) / 2), 0);
            lblEmptyTitle.Width = emptyStatePanel.ClientSize.Width;
            lblEmptyTitle.Height = Math.Max(26, Math.Min(34, contentHeight / 7));
            lblEmptyDescription.Width = emptyStatePanel.ClientSize.Width;
            lblEmptyDescription.Height = Math.Max(22, Math.Min(28, contentHeight / 8));
            int blockHeight = emptyBellPanel.Height + 12 + lblEmptyTitle.Height + 2 + lblEmptyDescription.Height;
            int top = Math.Max(8, (contentHeight - blockHeight) / 2);
            emptyBellPanel.Top = top;
            lblEmptyTitle.Location = new Point(0, emptyBellPanel.Bottom + 8);
            lblEmptyDescription.Location = new Point(0, lblEmptyTitle.Bottom + 2);
        }

        private sealed class NotificationRow
        {
            public string Type { get; set; } = string.Empty;
            public string Title { get; set; } = string.Empty;
            public string Message { get; set; } = string.Empty;
            public DateTime Date { get; set; }
        }
    }
}