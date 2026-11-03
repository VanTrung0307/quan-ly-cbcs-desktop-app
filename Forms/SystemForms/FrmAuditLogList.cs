using System.Drawing.Drawing2D;
using System.Collections;
using System.Data;
using System.Reflection;
using QuanLyCBCS.Helpers;
using QuanLyCBCS.Services;

namespace QuanLyCBCS.Forms.SystemForms
{
    public partial class FrmAuditLogList : Form
    {
        private readonly AuditLogService _auditService = new AuditLogService();
        private readonly AuditExportService _exportService = new AuditExportService();
        private readonly ToolTip _toolTip = new ToolTip();
        private readonly System.Windows.Forms.Timer _searchTimer = new System.Windows.Forms.Timer();

        private bool _isInitializing;
        private bool _isSearching;
        private object? _allAuditLogs;
        private object? _unfilteredAuditLogs;
        private int _currentPage = 1;

        private const int PageSize = 12;

        private Panel pnlPagination = null!;
        private FlowLayoutPanel flpPages = null!;
        private Button btnPageFirst = null!;
        private Button btnPagePrevious = null!;
        private Button btnPageNext = null!;
        private Button btnPageLast = null!;
        private Label lblPageSummary = null!;

        public FrmAuditLogList()
        {
            InitializeComponent();
            ApplyReferenceLayout();
            ApplyRoundedGridCorners();

            if (!PermissionHelper.IsAdmin)
            {
                Shown += (_, _) =>
                {
                    MessageBox.Show(this,
                        "Bạn không có quyền truy cập chức năng này.",
                        "Từ chối truy cập",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    Close();
                };
                return;
            }

            SetupTooltips();
            _searchTimer.Interval = 350;
            _searchTimer.Tick += SearchTimer_Tick;
        }

        private async void FrmAuditLogList_Load(object? sender, EventArgs e)
        {
            if (!PermissionHelper.IsAdmin || IsDisposed)
                return;

            _isInitializing = true;
            SetupDataGridView();

            dtpFromDate.Value = DateTime.Today;
            dtpToDate.Value = DateTime.Today;
            chkUseDateFilter.Checked = false;
            dtpFromDate.Enabled = false;
            dtpToDate.Enabled = false;

            cboQuickFilter.Items.Clear();
            cboQuickFilter.Items.AddRange(new object[]
            {
                "Tất cả", "LOGIN", "LOGOUT", "ADD_MISSION",
                "UPDATE_MISSION", "DELETE_MISSION",
                "ASSIGN_MISSION", "CREATE_USER",
                "UPDATE_USER", "DELETE_USER"
            });

            lblQuickFilter.Text = "Thao tác:";
            cboQuickFilter.SelectedIndex = 0;
            _isInitializing = false;

            await LoadAuditLogsAsync();

            if (!IsDisposed)
                BeginInvoke(new Action(FitAuditGridToRows));
        }

        private async Task LoadAuditLogsAsync()
        {
            if (IsDisposed || !IsHandleCreated)
                return;

            try
            {
                SetBusy(true);

                var data = await _auditService.GetAllAsync();

                if (IsDisposed)
                    return;

                BindAuditLogs(data);
                UpdateStatus(dgvAuditLogs.Rows.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,
                    "Không thể tải nhật ký hệ thống.\r\n" + ex.Message,
                    "Lỗi tải dữ liệu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                if (!IsDisposed)
                    SetBusy(false);
            }
        }

        private async Task SearchAuditLogsAsync()
        {
            if (_isInitializing || _isSearching || IsDisposed)
                return;

            _isSearching = true;

            try
            {
                SetBusy(true);

                string keyword = txtSearch.Text.Trim();

                DateTime? fromDate =
                    chkUseDateFilter.Checked
                    ? dtpFromDate.Value.Date
                    : null;

                DateTime? toDate =
                    chkUseDateFilter.Checked
                    ? dtpToDate.Value.Date
                    : null;

                if (fromDate.HasValue &&
                    toDate.HasValue &&
                    fromDate.Value > toDate.Value)
                {
                    MessageBox.Show(this,
                        "Ngày bắt đầu không được lớn hơn ngày kết thúc.",
                        "Khoảng thời gian không hợp lệ",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                var data = await _auditService.SearchAsync(
                    keyword, fromDate, toDate);

                if (IsDisposed)
                    return;

                BindAuditLogs(data);
                UpdateStatus(dgvAuditLogs.Rows.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,
                    "Không thể tìm kiếm nhật ký.\r\n" + ex.Message,
                    "Lỗi tìm kiếm",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                _isSearching = false;

                if (!IsDisposed)
                    SetBusy(false);
            }
        }

        private void BindAuditLogs(object data)
        {
            _unfilteredAuditLogs = data;
            ApplyActionFilter();
        }

        private void ApplyActionFilter()
        {
            if (_unfilteredAuditLogs == null)
                return;

            string action =
                cboQuickFilter.SelectedItem?.ToString() ?? "Tất cả";

            _allAuditLogs = FilterByAction(
                _unfilteredAuditLogs, action);

            _currentPage = 1;
            ShowCurrentPage();
        }

        private static object FilterByAction(
            object source, string action)
        {
            if (string.Equals(action, "Tất cả",
                StringComparison.OrdinalIgnoreCase))
                return source;

            if (source is DataTable table)
            {
                DataTable filtered = table.Clone();

                if (!table.Columns.Contains("Action"))
                    return source;

                foreach (DataRow row in table.Rows)
                {
                    if (string.Equals(
                        Convert.ToString(row["Action"]),
                        action,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        filtered.ImportRow(row);
                    }
                }

                return filtered;
            }

            if (source is IEnumerable enumerable &&
                source is not string)
            {
                Type? elementType = source.GetType()
                    .GetInterfaces()
                    .Where(t => t.IsGenericType &&
                        t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    .Select(t => t.GetGenericArguments()[0])
                    .FirstOrDefault();

                if (elementType == null)
                    return source;

                MethodInfo method = typeof(FrmAuditLogList)
                    .GetMethod(nameof(FilterTypedActions),
                        BindingFlags.NonPublic |
                        BindingFlags.Static)!
                    .MakeGenericMethod(elementType);

                return method.Invoke(
                    null, new object[] { enumerable, action }) ?? source;
            }

            return source;
        }

        private static List<T> FilterTypedActions<T>(
            IEnumerable source, string action)
        {
            PropertyInfo? property =
                typeof(T).GetProperty("Action");

            if (property == null)
                return source.Cast<T>().ToList();

            return source.Cast<T>()
                .Where(item => string.Equals(
                    Convert.ToString(property.GetValue(item)),
                    action,
                    StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        private void ShowCurrentPage()
        {
            if (_allAuditLogs == null || IsDisposed)
                return;

            dgvAuditLogs.SuspendLayout();

            try
            {
                object pageData = CreatePageData(
                    _allAuditLogs,
                    (_currentPage - 1) * PageSize,
                    PageSize);

                dgvAuditLogs.DataSource = null;
                dgvAuditLogs.DataSource = pageData;

                AddSttColumn();
                CustomizeAuditGrid();
                FitAuditGridToRows();
                UpdatePagination();
            }
            finally
            {
                dgvAuditLogs.ResumeLayout();
            }

            // Chạy lại sau khi WinForms hoàn tất DataBinding/Layout để lấy đúng
            // chiều cao các dòng thực tế, tránh DataGridView giữ chiều cao cũ.
            if (!IsDisposed && IsHandleCreated)
            {
                BeginInvoke(new Action(() =>
                {
                    if (IsDisposed || dgvAuditLogs.IsDisposed)
                        return;

                    FitAuditGridToRows();
                    dgvAuditLogs.Invalidate();
                    pnlGridHost.PerformLayout();
                }));
            }
        }

        private static object CreatePageData(
            object source, int skip, int take)
        {
            if (source is DataTable table)
            {
                DataTable page = table.Clone();

                int index = 0;

                foreach (DataRow row in table.Rows)
                {
                    if (index >= skip && index < skip + take)
                        page.ImportRow(row);

                    index++;

                    if (index >= skip + take)
                        break;
                }

                return page;
            }

            if (source is IEnumerable enumerable &&
                source is not string)
            {
                Type? elementType = source.GetType()
                    .GetInterfaces()
                    .Where(t => t.IsGenericType &&
                        t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    .Select(t => t.GetGenericArguments()[0])
                    .FirstOrDefault();

                if (elementType == null)
                    return source;

                MethodInfo method = typeof(FrmAuditLogList)
                    .GetMethod(nameof(CreateTypedPage),
                        BindingFlags.NonPublic |
                        BindingFlags.Static)!
                    .MakeGenericMethod(elementType);

                return method.Invoke(
                    null,
                    new object[] { enumerable, skip, take }) ?? source;
            }

            return source;
        }

        private static List<T> CreateTypedPage<T>(
            IEnumerable source, int skip, int take)
        {
            return source.Cast<T>()
                .Skip(skip)
                .Take(take)
                .ToList();
        }

        private int GetTotalRecordCount()
        {
            if (_allAuditLogs is DataTable table)
                return table.Rows.Count;

            if (_allAuditLogs is ICollection collection)
                return collection.Count;

            if (_allAuditLogs is IEnumerable enumerable &&
                _allAuditLogs is not string)
                return enumerable.Cast<object>().Count();

            return dgvAuditLogs.Rows.Count;
        }

        private int GetTotalPageCount()
        {
            return Math.Max(
                1,
                (int)Math.Ceiling(
                    GetTotalRecordCount() / (double)PageSize));
        }

        private void UpdatePagination()
        {
            if (pnlPagination == null)
                return;

            int total = GetTotalRecordCount();
            int pages = GetTotalPageCount();

            _currentPage = Math.Max(
                1, Math.Min(_currentPage, pages));

            lblPageSummary.Text = $"{_currentPage} / {pages}";

            btnPageFirst.Enabled = _currentPage > 1;
            btnPagePrevious.Enabled = _currentPage > 1;
            btnPageNext.Enabled = _currentPage < pages;
            btnPageLast.Enabled = _currentPage < pages;

            lblStatus.Text =
                $"Đang hiển thị: " +
                $"{Math.Min(PageSize, Math.Max(0, total - (_currentPage - 1) * PageSize)):N0}" +
                $" / {total:N0} bản ghi";

            flpPages.SuspendLayout();

            foreach (Control oldControl in flpPages.Controls)
                oldControl.Dispose();

            flpPages.Controls.Clear();

            int start = Math.Max(
                1, Math.Min(_currentPage - 2, pages - 4));

            int end = Math.Min(pages, start + 4);

            for (int page = start; page <= end; page++)
            {
                int target = page;

                Button button = CreatePageButton(
                    page.ToString(),
                    target == _currentPage);

                button.Click += (_, _) =>
                {
                    _currentPage = target;
                    ShowCurrentPage();
                };

                flpPages.Controls.Add(button);
            }

            if (pages > 5)
            {
                Label dots = new Label
                {
                    Text = "…",
                    AutoSize = false,
                    Width = 22,
                    Height = 34,
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.FromArgb(20, 54, 94)
                };

                flpPages.Controls.Add(dots);

                Button last = CreatePageButton(
                    pages.ToString(),
                    _currentPage == pages);

                last.Click += (_, _) =>
                {
                    _currentPage = pages;
                    ShowCurrentPage();
                };

                flpPages.Controls.Add(last);
            }

            flpPages.ResumeLayout();
        }

        private Button CreatePageButton(
            string text, bool selected)
        {
            Button button = new Button
            {
                Text = text,
                Width = 38,
                Height = 34,
                Margin = new Padding(2, 0, 2, 0),
                FlatStyle = FlatStyle.Flat,
                UseVisualStyleBackColor = false,
                Font = new Font(
                    "Segoe UI", 9F, FontStyle.Bold),

                BackColor = selected
                    ? Color.FromArgb(0, 105, 210)
                    : Color.White,

                ForeColor = selected
                    ? Color.White
                    : Color.FromArgb(20, 54, 94),

                FlatAppearance =
                {
                    BorderColor = Color.FromArgb(195, 215, 238),
                    BorderSize = 1,
                    MouseOverBackColor = selected
                        ? Color.FromArgb(0, 95, 195)
                        : Color.FromArgb(242, 248, 255),
                    MouseDownBackColor = Color.FromArgb(225, 239, 255)
                }
            };

            // Bo góc đồng nhất cho nút đầu/trước/số trang/tiếp/cuối.
            ApplyRoundedButton(button, 6);
            return button;
        }

        private void ApplyRoundedCard(Panel panel, int radius, Color borderColor)
        {
            void UpdateShape()
            {
                if (panel.ClientSize.Width < 2 || panel.ClientSize.Height < 2)
                    return;

                Rectangle bounds = new Rectangle(
                    0, 0, panel.ClientSize.Width - 1, panel.ClientSize.Height - 1);

                using (GraphicsPath path = RoundedRect(bounds, radius))
                {
                    Region? oldRegion = panel.Region;
                    panel.Region = new Region(path);
                    oldRegion?.Dispose();
                }
            }

            panel.Paint -= RoundedCard_Paint;
            panel.Paint += RoundedCard_Paint;
            panel.Resize -= RoundedCard_Resize;
            panel.Resize += RoundedCard_Resize;
            panel.Tag = borderColor;
            UpdateShape();

            void RoundedCard_Paint(object? sender, PaintEventArgs e)
            {
                if (panel.ClientSize.Width < 2 || panel.ClientSize.Height < 2)
                    return;

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle bounds = new Rectangle(
                    0, 0, panel.ClientSize.Width - 1, panel.ClientSize.Height - 1);

                using (GraphicsPath path = RoundedRect(bounds, radius))
                using (Pen pen = new Pen(borderColor, 1F))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }

            void RoundedCard_Resize(object? sender, EventArgs e)
            {
                UpdateShape();
                panel.Invalidate();
            }
        }

        private void ApplyRoundedButton(Button button, int radius)
        {
            void UpdateShape()
            {
                if (button.ClientSize.Width < 2 || button.ClientSize.Height < 2)
                    return;

                Rectangle bounds = new Rectangle(
                    0, 0, button.ClientSize.Width - 1, button.ClientSize.Height - 1);

                using (GraphicsPath path = RoundedRect(bounds, radius))
                {
                    Region? oldRegion = button.Region;
                    button.Region = new Region(path);
                    oldRegion?.Dispose();
                }
            }

            button.Resize -= RoundedButton_Resize;
            button.Resize += RoundedButton_Resize;
            UpdateShape();

            void RoundedButton_Resize(object? sender, EventArgs e)
            {
                UpdateShape();
                button.Invalidate();
            }
        }

        private void ConfigureActionButtons()
        {
            // Xuất Excel: nút xanh lá, không viền, icon Excel và nhãn căn giữa.
            btnExport.Text = string.Empty;
            btnExport.BackColor = Color.FromArgb(8, 163, 62);
            btnExport.ForeColor = Color.White;
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.FlatAppearance.MouseOverBackColor = Color.FromArgb(6, 150, 56);
            btnExport.FlatAppearance.MouseDownBackColor = Color.FromArgb(4, 132, 48);
            btnExport.UseVisualStyleBackColor = false;
            btnExport.Cursor = Cursors.Hand;
            btnExport.Padding = new Padding(0);
            btnExport.Paint -= BtnExport_Paint;
            btnExport.Paint += BtnExport_Paint;
            ApplyRoundedButton(btnExport, 8);

            // Làm mới: nền trắng, viền xanh dương rõ và icon refresh riêng.
            btnRefresh.Text = string.Empty;
            btnRefresh.BackColor = Color.White;
            btnRefresh.ForeColor = Color.FromArgb(20, 54, 94);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            // Tắt viền mặc định của Button; tự vẽ viền bo góc trong Paint
            // để đường viền bao kín đủ 4 cạnh, không bị renderer cắt mất cạnh phải.
            btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(73, 158, 255);
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatAppearance.MouseOverBackColor = Color.FromArgb(242, 248, 255);
            btnRefresh.FlatAppearance.MouseDownBackColor = Color.FromArgb(229, 241, 255);
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.Padding = new Padding(0);
            btnRefresh.Paint -= BtnRefresh_Paint;
            btnRefresh.Paint += BtnRefresh_Paint;
            ApplyRoundedButton(btnRefresh, 8);
        }

        private void BtnExport_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            const string excelGlyph = "\uE9F9";
            const string caption = "Xuất";
            using Font iconFont = new Font("Segoe MDL2 Assets", 15F, FontStyle.Regular);
            using Font textFont = new Font("Segoe UI", 12F, FontStyle.Bold);

            SizeF iconSize = g.MeasureString(excelGlyph, iconFont);
            SizeF textSize = g.MeasureString(caption, textFont);
            float gap = 5F;
            float totalWidth = iconSize.Width + gap + textSize.Width;
            float x = (btnExport.ClientSize.Width - totalWidth) / 2F;
            float yIcon = (btnExport.ClientSize.Height - iconSize.Height) / 2F - 1F;
            float yText = (btnExport.ClientSize.Height - textSize.Height) / 2F - 1F;

            using SolidBrush brush = new SolidBrush(Color.White);
            g.DrawString(excelGlyph, iconFont, brush, x, yIcon);
            g.DrawString(caption, textFont, brush, x + iconSize.Width + gap, yText);
        }

        private void BtnRefresh_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;

            const string refreshGlyph = "\uE72C";
            const string caption = "Làm mới";
            using Font iconFont = new Font("Segoe MDL2 Assets", 15F, FontStyle.Regular);
            using Font textFont = new Font("Segoe UI", 11.5F, FontStyle.Bold);

            SizeF iconSize = g.MeasureString(refreshGlyph, iconFont);
            SizeF textSize = g.MeasureString(caption, textFont);
            float gap = 6F;
            float totalWidth = iconSize.Width + gap + textSize.Width;
            float x = (btnRefresh.ClientSize.Width - totalWidth) / 2F;
            float yIcon = (btnRefresh.ClientSize.Height - iconSize.Height) / 2F - 1F;
            float yText = (btnRefresh.ClientSize.Height - textSize.Height) / 2F - 1F;

            using SolidBrush iconBrush = new SolidBrush(Color.FromArgb(0, 112, 210));
            using SolidBrush textBrush = new SolidBrush(Color.FromArgb(20, 54, 94));
            g.DrawString(refreshGlyph, iconFont, iconBrush, x, yIcon);
            g.DrawString(caption, textFont, textBrush, x + iconSize.Width + gap, yText);

            // Vẽ viền bo góc bằng nét 2px, đặt sâu vào trong control để
            // tránh bị Region bo góc hoặc mép control cắt mất cạnh phải/dưới.
            // Không dùng PenAlignment.Inset vì GDI+ có thể bỏ/clip một số cạnh
            // khi vẽ GraphicsPath trên Button của WinForms.
            int borderInset = 2;
            int borderWidth = Math.Max(1, btnRefresh.ClientSize.Width - borderInset * 2 - 1);
            int borderHeight = Math.Max(1, btnRefresh.ClientSize.Height - borderInset * 2 - 1);

            Rectangle borderBounds = new Rectangle(
                borderInset,
                borderInset,
                borderWidth,
                borderHeight);

            using (GraphicsPath borderPath = RoundedRect(borderBounds, 8))
            using (Pen borderPen = new Pen(
                Color.FromArgb(0, 112, 210), 2F))
            {
                borderPen.Alignment = PenAlignment.Center;
                borderPen.LineJoin = LineJoin.Round;
                g.DrawPath(borderPen, borderPath);
            }
        }

        private Label CreateFilterGlyph(string glyph, Point location, Size size, float fontSize)
        {
            return new Label
            {
                AutoSize = false,
                Text = glyph,
                Font = new Font("Segoe MDL2 Assets", fontSize, FontStyle.Regular),
                ForeColor = Color.FromArgb(0, 105, 205),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = location,
                Size = size,
                TabStop = false
            };
        }

        private void ApplyReferenceLayout()
        {
            pnlRoot.Padding = new Padding(8, 8, 8, 6);
            pnlRoot.Resize -= PnlRoot_ResponsiveContentLayout;
            pnlRoot.Resize += PnlRoot_ResponsiveContentLayout;

            pnlHeader.Height = 84;
            // Thu gọn chiều cao và căn khoảng đệm theo bản thiết kế tham chiếu.
            pnlFilterHost.Height = 120;
            pnlFilterHost.BackColor = Color.FromArgb(244, 248, 253);
            pnlFilterHost.Padding = new Padding(4, 8, 4, 8);
            pnlFooter.Height = 36;

            // Bảng và hai thanh thông tin được bố trí theo chiều cao dữ liệu thực tế.
            pnlGridHost.Dock = DockStyle.None;
            pnlFooter.Dock = DockStyle.None;

            // Tạo khoảng cách nhẹ giữa thẻ bộ lọc và bảng dữ liệu.
            // Phần diện tích còn lại dùng nền tổng thể, không kéo dài nền trắng của bảng.
            pnlGridHost.BackColor = Color.FromArgb(244, 248, 253);
            pnlGridHost.Padding = new Padding(1, 12, 1, 0);
            // Lấp đầy cả chiều rộng và chiều cao của vùng bảng.
            dgvAuditLogs.Dock = DockStyle.Fill;
            dgvAuditLogs.Margin = Padding.Empty;

            // Hai thẻ độc lập: bộ lọc bên trái và nhóm nút bên phải.
            // Nhóm nút thao tác theo kích thước và tỷ lệ của bản thiết kế tham chiếu.
            pnlActions.Width = 340;
            pnlActions.Padding = new Padding(10, 15, 8, 0);
            pnlActions.BackColor = Color.FromArgb(244, 248, 253);
            pnlFilter.BackColor = Color.FromArgb(235, 245, 255);
            pnlFilter.Padding = new Padding(14, 6, 14, 6);
            ApplyRoundedCard(pnlFilter, 14, Color.FromArgb(211, 229, 249));
            ApplyRoundedCard(pnlActions, 14, Color.FromArgb(211, 229, 249));

            // BIỂU TƯỢNG HISTORY - SEGOE MDL2 ASSETS
            lblHeaderIcon.Text = "\uE81C";
            lblHeaderIcon.Font = new Font(
                "Segoe MDL2 Assets",
                57F,
                FontStyle.Regular,
                GraphicsUnit.Pixel);

            lblHeaderIcon.ForeColor = Color.White;
            lblHeaderIcon.BackColor = Color.Transparent;
            lblHeaderIcon.TextAlign = ContentAlignment.MiddleCenter;
            lblHeaderIcon.Location = new Point(18, 4);
            lblHeaderIcon.Size = new Size(88, 76);

            lblTitle.Location = new Point(112, 8);
            lblTitle.Font = new Font(
                "Segoe UI", 22F, FontStyle.Bold);

            lblSubTitle.Location = new Point(115, 54);

            // Hàng 1: tài khoản + tìm kiếm + checkbox.
            lblFindAccount.Location = new Point(22, 20);
            lblFindAccount.Size = new Size(78, 28);
            lblFindAccount.TextAlign = ContentAlignment.MiddleLeft;

            // Ghép ô nhập và nút tìm kiếm trong cùng một thẻ bo góc.
            // Giữ nguyên các control gốc để không ảnh hưởng event tìm kiếm.
            pnlFilter.Controls.Remove(txtSearch);
            pnlFilter.Controls.Remove(btnSearch);

            Panel pnlSearchHost = new Panel
            {
                Name = "pnlSearchHost",
                Location = new Point(105, 12),
                Size = new Size(350, 36),
                BackColor = Color.White,
                Margin = new Padding(0)
            };
            ApplyRoundedCard(pnlSearchHost, 9, Color.FromArgb(190, 211, 235));

            Label lblSearchGlyph = CreateFilterGlyph("\uE8D4", new Point(8, 2), new Size(25, 32), 16F);
            lblSearchGlyph.ForeColor = Color.FromArgb(0, 105, 205);
            pnlSearchHost.Controls.Add(lblSearchGlyph);

            txtSearch.BorderStyle = BorderStyle.None;
            txtSearch.BackColor = Color.White;
            txtSearch.Font = new Font("Segoe UI", 10F);
            txtSearch.Location = new Point(36, 6);
            txtSearch.Size = new Size(245, 24);
            pnlSearchHost.Controls.Add(txtSearch);

            btnSearch.Location = new Point(294, 2);
            btnSearch.Size = new Size(52, 32);
            btnSearch.Text = "\uE721";
            btnSearch.Font = new Font("Segoe MDL2 Assets", 17F, FontStyle.Regular);
            btnSearch.ForeColor = Color.White;
            btnSearch.BackColor = Color.FromArgb(0, 105, 205);
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 92, 185);
            btnSearch.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 78, 160);
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.TextAlign = ContentAlignment.MiddleCenter;
            btnSearch.Padding = new Padding(0);
            // Bo tròn cả hai đầu nút tìm kiếm, không để góc vuông như phiên bản trước.
            ApplyRoundedButton(btnSearch, 8);
            pnlSearchHost.Controls.Add(btnSearch);
            pnlFilter.Controls.Add(pnlSearchHost);
            pnlSearchHost.BringToFront();

            chkUseDateFilter.Location = new Point(470, 17);
            chkUseDateFilter.Size = new Size(145, 28);
            chkUseDateFilter.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            chkUseDateFilter.ForeColor = Color.FromArgb(20, 54, 94);

            // Hàng 2: biểu tượng lịch, khoảng ngày và biểu tượng lọc.
            // Tăng vùng vẽ và chừa lề trái để glyph Segoe MDL2 không bị cắt nét.
            Label lblCalendarFrom = CreateFilterGlyph("\uE787", new Point(18, 62), new Size(36, 30), 18F);
            lblCalendarFrom.Name = "lblCalendarFromGlyph";
            Label lblCalendarTo = CreateFilterGlyph("\uE787", new Point(338, 62), new Size(36, 30), 18F);
            lblCalendarTo.Name = "lblCalendarToGlyph";
            Label lblFilterGlyph = CreateFilterGlyph("\uE71C", new Point(658, 62), new Size(36, 30), 19F);
            lblFilterGlyph.Name = "lblActionFilterGlyph";
            pnlFilter.Controls.Add(lblCalendarFrom);
            pnlFilter.Controls.Add(lblCalendarTo);
            pnlFilter.Controls.Add(lblFilterGlyph);

            lblFromDate.Location = new Point(48, 68);
            lblFromDate.TextAlign = ContentAlignment.MiddleLeft;
            lblToDate.Location = new Point(350, 68);
            lblToDate.TextAlign = ContentAlignment.MiddleLeft;
            lblQuickFilter.Location = new Point(640, 68);
            lblQuickFilter.Text = "Thao tác:";
            lblQuickFilter.TextAlign = ContentAlignment.MiddleLeft;

            dtpFromDate.Location = new Point(140, 62);
            dtpFromDate.Size = new Size(178, 30);
            dtpFromDate.Font = new Font("Segoe UI", 9.5F);
            dtpToDate.Location = new Point(435, 62);
            dtpToDate.Size = new Size(170, 30);
            dtpToDate.Font = new Font("Segoe UI", 9.5F);

            // Combobox nằm trong khung riêng để có đường viền bo góc liền mạch.
            pnlFilter.Controls.Remove(cboQuickFilter);
            Panel pnlActionFilterHost = new Panel
            {
                Name = "pnlActionFilterHost",
                Location = new Point(770, 61),
                Size = new Size(210, 33),
                BackColor = Color.White,
                Padding = new Padding(2)
            };
            ApplyRoundedCard(pnlActionFilterHost, 8, Color.FromArgb(190, 211, 235));
            cboQuickFilter.Location = new Point(3, 2);
            cboQuickFilter.Size = new Size(202, 28);
            cboQuickFilter.Font = new Font("Segoe UI", 9.5F);
            cboQuickFilter.FlatStyle = FlatStyle.Flat;
            cboQuickFilter.BackColor = Color.White;
            pnlActionFilterHost.Controls.Add(cboQuickFilter);
            pnlFilter.Controls.Add(pnlActionFilterHost);

            // Chia đều hàng lọc thành 3 vùng độc lập theo chiều rộng thực tế.
            // Cách bố trí này tránh DateTimePicker/ComboBox đè lên các glyph.
            pnlFilter.Resize -= PnlFilter_ResponsiveLayout;
            pnlFilter.Resize += PnlFilter_ResponsiveLayout;
            LayoutFilterGroups();

            // Thu nhỏ hai nút để cân đối trong thẻ thao tác, không làm thay đổi chức năng.
            btnExport.Location = new Point(8, 17);
            btnExport.Size = new Size(116, 40);
            btnRefresh.Location = new Point(132, 17);
            btnRefresh.Size = new Size(132, 40);
            ConfigureActionButtons();

            pnlPagination = new Panel
            {
                Name = "pnlPagination",
                Dock = DockStyle.None,
                Height = 48,
                BackColor = Color.FromArgb(235, 244, 254),
                Padding = new Padding(8, 6, 8, 6)
            };

            FlowLayoutPanel right = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                Width = 560,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };

            btnPageFirst = CreatePageButton("≪", false);
            btnPageFirst.Width = 34;
            btnPageFirst.Click += (_, _) =>
            {
                _currentPage = 1;
                ShowCurrentPage();
            };

            btnPagePrevious = CreatePageButton("‹", false);
            btnPagePrevious.Width = 34;
            btnPagePrevious.Click += (_, _) =>
            {
                if (_currentPage > 1)
                {
                    _currentPage--;
                    ShowCurrentPage();
                }
            };

            btnPageNext = CreatePageButton("›", false);
            btnPageNext.Width = 34;
            btnPageNext.Click += (_, _) =>
            {
                _currentPage++;
                ShowCurrentPage();
            };

            btnPageLast = CreatePageButton("≫", false);
            btnPageLast.Width = 34;
            btnPageLast.Click += (_, _) =>
            {
                _currentPage = GetTotalPageCount();
                ShowCurrentPage();
            };

            lblPageSummary = new Label
            {
                AutoSize = false,
                Width = 56,
                Height = 34,
                Text = "1 / 1",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(20, 54, 94)
            };

            flpPages = new FlowLayoutPanel
            {
                AutoSize = false,
                Width = 280,
                Height = 36,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Margin = new Padding(0)
            };

            right.Controls.Add(btnPageFirst);
            right.Controls.Add(btnPagePrevious);
            right.Controls.Add(flpPages);
            right.Controls.Add(btnPageNext);
            right.Controls.Add(btnPageLast);
            right.Controls.Add(lblPageSummary);

            pnlPagination.Controls.Add(right);

            pnlRoot.Controls.Add(pnlPagination);
            pnlRoot.Controls.SetChildIndex(pnlPagination, 3);

            // Bo góc hai thẻ footer: tổng số bản ghi và khu vực phân trang.
            ApplyRoundedCard(
                pnlFooter, 10, Color.FromArgb(211, 229, 249));
            ApplyRoundedCard(
                pnlPagination, 10, Color.FromArgb(211, 229, 249));

            pnlHeader.Paint -= pnlHeader_Paint;
            pnlHeader.Paint += pnlHeader_Paint;

            lblHeaderIcon.BringToFront();
            pnlHeader.Invalidate();

            dgvAuditLogs.CellPainting -= dgvAuditLogs_CellPainting;
            dgvAuditLogs.CellPainting += dgvAuditLogs_CellPainting;

            dgvAuditLogs.ColumnHeadersHeight = 40;
            dgvAuditLogs.RowTemplate.Height = 33;
        }

        private void FitAuditGridToRows()
        {
            if (dgvAuditLogs == null || dgvAuditLogs.IsDisposed ||
                pnlGridHost == null || pnlGridHost.IsDisposed ||
                pnlRoot == null || pnlRoot.IsDisposed ||
                pnlFooter == null || pnlFooter.IsDisposed ||
                pnlPagination == null || pnlPagination.IsDisposed)
                return;

            // Chiều cao bảng bám theo tiêu đề và số dòng đang hiển thị.
            int visibleRowsHeight = 0;
            foreach (DataGridViewRow row in dgvAuditLogs.Rows)
            {
                if (row.Visible)
                    visibleRowsHeight += row.Height;
            }

            int gridContentHeight =
                dgvAuditLogs.ColumnHeadersHeight + visibleRowsHeight + 2;
            int contentLeft = pnlRoot.Padding.Left;
            int contentWidth = Math.Max(100,
                pnlRoot.ClientSize.Width - pnlRoot.Padding.Left - pnlRoot.Padding.Right);
            int gridTop = pnlFilterHost.Bottom + 8;

            pnlGridHost.Dock = DockStyle.None;
            pnlGridHost.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlGridHost.Location = new Point(contentLeft, gridTop);
            pnlGridHost.Size = new Size(contentWidth,
                gridContentHeight + pnlGridHost.Padding.Vertical);

            dgvAuditLogs.Dock = DockStyle.Fill;
            dgvAuditLogs.Margin = Padding.Empty;
            dgvAuditLogs.BackgroundColor = Color.FromArgb(244, 248, 253);

            // Đặt thanh số bản ghi và phân trang ngay dưới dòng dữ liệu cuối.
            pnlFooter.Dock = DockStyle.None;
            pnlFooter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlFooter.Location = new Point(contentLeft, pnlGridHost.Bottom + 8);
            pnlFooter.Size = new Size(contentWidth, 36);

            pnlPagination.Dock = DockStyle.None;
            pnlPagination.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlPagination.Location = new Point(contentLeft, pnlFooter.Bottom);
            pnlPagination.Size = new Size(contentWidth, 48);

            Control? rightPanel = pnlPagination.Controls
                .Cast<Control>()
                .FirstOrDefault(c => c is FlowLayoutPanel);
            if (rightPanel != null)
            {
                rightPanel.Dock = DockStyle.None;
                rightPanel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                rightPanel.Location = new Point(
                    Math.Max(8, pnlPagination.ClientSize.Width - rightPanel.Width - 8), 6);
                rightPanel.Height = 36;
            }

            pnlGridHost.BackColor = Color.FromArgb(244, 248, 253);
            pnlRoot.PerformLayout();
            pnlGridHost.PerformLayout();
            dgvAuditLogs.PerformLayout();
            pnlFooter.PerformLayout();
            pnlPagination.PerformLayout();
            pnlGridHost.Invalidate();
            dgvAuditLogs.Invalidate();
        }

        private void PnlRoot_ResponsiveContentLayout(object? sender, EventArgs e)
        {
            if (IsDisposed || !IsHandleCreated)
                return;
            FitAuditGridToRows();
        }

        private void ApplyRoundedGridCorners()
        {
            const int radius = 12;

            void UpdateRoundedRegion(Control control)
            {
                if (control.IsDisposed ||
                    control.ClientSize.Width < 2 ||
                    control.ClientSize.Height < 2)
                    return;

                Rectangle bounds = new Rectangle(
                    0, 0,
                    control.ClientSize.Width - 1,
                    control.ClientSize.Height - 1);

                using (GraphicsPath path = RoundedRect(bounds, radius))
                {
                    Region? oldRegion = control.Region;
                    control.Region = new Region(path);
                    oldRegion?.Dispose();
                }
            }

            pnlGridHost.Resize -= GridHost_Resize;
            pnlGridHost.Resize += GridHost_Resize;
            dgvAuditLogs.Resize -= AuditGrid_Resize;
            dgvAuditLogs.Resize += AuditGrid_Resize;

            UpdateRoundedRegion(pnlGridHost);
            UpdateRoundedRegion(dgvAuditLogs);

            void GridHost_Resize(object? sender, EventArgs e)
            {
                UpdateRoundedRegion(pnlGridHost);
            }

            void AuditGrid_Resize(object? sender, EventArgs e)
            {
                UpdateRoundedRegion(dgvAuditLogs);
            }
        }

        private void PnlFilter_ResponsiveLayout(object? sender, EventArgs e)
        {
            LayoutFilterGroups();
        }

        private void LayoutFilterGroups()
        {
            if (pnlFilter == null || pnlFilter.IsDisposed || pnlFilter.ClientSize.Width <= 0)
                return;

            int left = 14;
            int usableWidth = Math.Max(720, pnlFilter.ClientSize.Width - 28);
            int groupWidth = usableWidth / 3;
            // Chừa lề trái cho glyph Segoe MDL2 và tăng vùng vẽ để không mất nét.
            const int iconInset = 6;
            const int iconWidth = 38;
            const int labelOffset = 48;
            const int controlOffset = 130;
            const int rowY = 62;
            const int controlHeight = 30;

            Control? calendarFrom = pnlFilter.Controls["lblCalendarFromGlyph"];
            Control? calendarTo = pnlFilter.Controls["lblCalendarToGlyph"];
            Control? actionGlyph = pnlFilter.Controls["lblActionFilterGlyph"];
            Control? actionHost = pnlFilter.Controls["pnlActionFilterHost"];

            // Nhóm 1: Từ ngày.
            int fromX = left;
            if (calendarFrom != null)
                calendarFrom.Bounds = new Rectangle(fromX + iconInset, rowY, iconWidth, controlHeight);
            lblFromDate.Location = new Point(fromX + labelOffset, rowY + 6);
            lblFromDate.Size = new Size(76, 22);
            int dateWidth = Math.Max(145, Math.Min(178, groupWidth - controlOffset - 8));
            dtpFromDate.Location = new Point(fromX + controlOffset, rowY);
            dtpFromDate.Size = new Size(dateWidth, controlHeight);

            // Nhóm 2: Đến ngày.
            int toX = left + groupWidth;
            if (calendarTo != null)
                calendarTo.Bounds = new Rectangle(toX + iconInset, rowY, iconWidth, controlHeight);
            lblToDate.Location = new Point(toX + labelOffset, rowY + 6);
            lblToDate.Size = new Size(76, 22);
            dtpToDate.Location = new Point(toX + controlOffset, rowY);
            dtpToDate.Size = new Size(dateWidth, controlHeight);

            // Nhóm 3: Lọc theo thao tác.
            int actionX = left + groupWidth * 2;
            if (actionGlyph != null)
                actionGlyph.Bounds = new Rectangle(actionX + iconInset, rowY, iconWidth, controlHeight);
            lblQuickFilter.Location = new Point(actionX + labelOffset, rowY + 6);
            lblQuickFilter.Size = new Size(76, 22);
            if (actionHost != null)
            {
                int comboWidth = Math.Max(150, Math.Min(230, groupWidth - controlOffset - 8));
                actionHost.Location = new Point(actionX + controlOffset, rowY - 1);
                actionHost.Size = new Size(comboWidth, 33);
                cboQuickFilter.Location = new Point(3, 2);
                cboQuickFilter.Size = new Size(comboWidth - 8, 28);
            }

            // Đảm bảo biểu tượng luôn nằm trên nền thẻ, không bị control kế bên che.
            calendarFrom?.BringToFront();
            calendarTo?.BringToFront();
            actionGlyph?.BringToFront();
        }

        private void dgvAuditLogs_CellPainting(
            object? sender,
            DataGridViewCellPaintingEventArgs e)
        {
            Graphics? graphics = e.Graphics;

            if (graphics == null ||
                e.RowIndex < 0 ||
                e.ColumnIndex < 0)
                return;

            string column =
                dgvAuditLogs.Columns[e.ColumnIndex].Name;

            if (column == "Action" &&
                e.Value is string action &&
                !string.IsNullOrWhiteSpace(action))
            {
                e.PaintBackground(e.ClipBounds, true);

                Color color =
                    action.Contains("LOGIN", StringComparison.OrdinalIgnoreCase)
                        ? Color.FromArgb(28, 177, 82)
                    : action.Contains("ADD", StringComparison.OrdinalIgnoreCase)
                        ? Color.FromArgb(21, 177, 133)
                    : action.Contains("ASSIGN", StringComparison.OrdinalIgnoreCase)
                        ? Color.FromArgb(0, 112, 210)
                    : action.Contains("DELETE", StringComparison.OrdinalIgnoreCase)
                        ? Color.FromArgb(220, 65, 65)
                    : Color.FromArgb(205, 222, 239);

                Color textColor =
                    color == Color.FromArgb(205, 222, 239)
                        ? Color.FromArgb(20, 54, 94)
                        : Color.White;

                int badgeWidth = Math.Min(
                    184,
                    Math.Max(0, e.CellBounds.Width - 16));

                Rectangle badge = new Rectangle(
                    e.CellBounds.Left +
                        (e.CellBounds.Width - badgeWidth) / 2,
                    e.CellBounds.Top + 6,
                    badgeWidth,
                    Math.Max(18, e.CellBounds.Height - 12));

                using (GraphicsPath path = RoundedRect(badge, 5))
                using (SolidBrush brush = new SolidBrush(color))
                {
                    graphics.FillPath(brush, path);
                }

                string actionIcon =
                    action.Contains("LOGIN", StringComparison.OrdinalIgnoreCase) ? "➜  " :
                    action.Contains("LOGOUT", StringComparison.OrdinalIgnoreCase) ? "↪  " :
                    action.Contains("ADD", StringComparison.OrdinalIgnoreCase) ? "+  " :
                    action.Contains("ASSIGN", StringComparison.OrdinalIgnoreCase) ? "♟  " :
                    action.Contains("DELETE", StringComparison.OrdinalIgnoreCase) ? "✕  " :
                    action.Contains("UPDATE", StringComparison.OrdinalIgnoreCase) ? "✎  " :
                    "•  ";

                using (Font actionFont =
                    new Font("Segoe UI", 9F, FontStyle.Bold))
                {
                    TextRenderer.DrawText(
                        graphics,
                        actionIcon + action,
                        actionFont,
                        badge,
                        textColor,
                        TextFormatFlags.HorizontalCenter |
                        TextFormatFlags.VerticalCenter |
                        TextFormatFlags.EndEllipsis |
                        TextFormatFlags.NoPrefix);
                }

                e.Handled = true;
            }
            else if (column == "CreatedAt")
            {
                e.PaintBackground(e.ClipBounds, true);

                int iconLeft = e.CellBounds.Left +
                    Math.Max(10, (e.CellBounds.Width - 190) / 2);

                Rectangle iconRect = new Rectangle(
                    iconLeft,
                    e.CellBounds.Top + 9,
                    16,
                    15);

                using (Pen pen =
                    new Pen(Color.FromArgb(0, 105, 195), 1.8F))
                {
                    graphics.DrawRectangle(pen, iconRect);

                    graphics.DrawLine(
                        pen,
                        iconRect.Left,
                        iconRect.Top + 4,
                        iconRect.Right,
                        iconRect.Top + 4);

                    graphics.DrawLine(
                        pen,
                        iconRect.Left + 4,
                        iconRect.Top - 2,
                        iconRect.Left + 4,
                        iconRect.Top + 2);

                    graphics.DrawLine(
                        pen,
                        iconRect.Right - 4,
                        iconRect.Top - 2,
                        iconRect.Right - 4,
                        iconRect.Top + 2);
                }

                string value =
                    e.FormattedValue?.ToString() ?? string.Empty;

                Rectangle textRect = new Rectangle(
                    iconRect.Right + 8,
                    e.CellBounds.Top,
                    Math.Max(0, e.CellBounds.Width - iconRect.Width - 20),
                    e.CellBounds.Height);

                TextRenderer.DrawText(
                    graphics,
                    value,
                    dgvAuditLogs.Font,
                    textRect,
                    Color.FromArgb(20, 40, 65),
                    TextFormatFlags.VerticalCenter |
                    TextFormatFlags.Left |
                    TextFormatFlags.EndEllipsis);

                e.Handled = true;
            }
        }

        private static GraphicsPath RoundedRect(
            Rectangle bounds, int radius)
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

        private void AddSttColumn()
        {
            if (dgvAuditLogs.Columns["STT"] == null)
            {
                dgvAuditLogs.Columns.Insert(0,
                    new DataGridViewTextBoxColumn
                    {
                        Name = "STT",
                        HeaderText = "STT",
                        ReadOnly = true,
                        Width = 62,
                        FillWeight = 6,
                        SortMode =
                            DataGridViewColumnSortMode.NotSortable
                    });
            }

            foreach (DataGridViewRow row in dgvAuditLogs.Rows)
            {
                if (!row.IsNewRow)
                    row.Cells["STT"].Value = row.Index + 1;
            }
        }

        private void CustomizeAuditGrid()
        {
            dgvAuditLogs.RowHeadersVisible = false;
            dgvAuditLogs.ReadOnly = true;
            dgvAuditLogs.AllowUserToAddRows = false;
            dgvAuditLogs.AllowUserToDeleteRows = false;

            dgvAuditLogs.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvAuditLogs.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.None;

            dgvAuditLogs.RowTemplate.Height = 33;

            dgvAuditLogs.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvAuditLogs.MultiSelect = false;

            SetColumn("AuditLogId", visible: false);

            SetColumn("STT", "STT",
                fillWeight: 6,
                alignment: DataGridViewContentAlignment.MiddleCenter);

            SetColumn("Username", "Tài khoản",
                fillWeight: 13,
                alignment: DataGridViewContentAlignment.MiddleCenter);

            SetColumn("Action", "Thao tác",
                fillWeight: 19,
                alignment: DataGridViewContentAlignment.MiddleCenter);

            SetColumn("Description", "Mô tả", fillWeight: 43);

            SetColumn("CreatedAt", "Thời gian",
                fillWeight: 24,
                alignment: DataGridViewContentAlignment.MiddleCenter);

            if (dgvAuditLogs.Columns["CreatedAt"] != null)
            {
                dgvAuditLogs.Columns["CreatedAt"]
                    .DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
            }

            foreach (DataGridViewColumn column in dgvAuditLogs.Columns)
            {
                column.SortMode =
                    DataGridViewColumnSortMode.NotSortable;

                column.HeaderCell.Style.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private void SetColumn(
            string name,
            string? header = null,
            bool visible = true,
            float? fillWeight = null,
            DataGridViewContentAlignment? alignment = null)
        {
            if (dgvAuditLogs.Columns[name] == null)
                return;

            DataGridViewColumn column =
                dgvAuditLogs.Columns[name];

            column.Visible = visible;

            if (header != null)
                column.HeaderText = header;

            if (fillWeight.HasValue)
                column.FillWeight = fillWeight.Value;

            if (alignment.HasValue)
                column.DefaultCellStyle.Alignment = alignment.Value;
        }

        private void SetupDataGridView()
        {
            DataGridViewHelper.SetupDefaultStyle(dgvAuditLogs);

            dgvAuditLogs.EnableHeadersVisualStyles = false;
            dgvAuditLogs.BackgroundColor = Color.FromArgb(244, 248, 253);
            dgvAuditLogs.BorderStyle = BorderStyle.None;
            dgvAuditLogs.GridColor = Color.FromArgb(207, 224, 244);

            dgvAuditLogs.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(0, 76, 150);

            dgvAuditLogs.ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;

            dgvAuditLogs.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 9.5F, FontStyle.Bold);

            dgvAuditLogs.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            dgvAuditLogs.ColumnHeadersHeight = 38;

            dgvAuditLogs.DefaultCellStyle.Font =
                new Font("Segoe UI", 9.2F);

            dgvAuditLogs.DefaultCellStyle.ForeColor =
                Color.FromArgb(25, 40, 60);

            dgvAuditLogs.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(145, 207, 245);

            dgvAuditLogs.DefaultCellStyle.SelectionForeColor =
                Color.FromArgb(15, 35, 60);

            dgvAuditLogs.AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(239, 246, 253);

            dgvAuditLogs.RowsDefaultCellStyle.BackColor =
                Color.White;

            dgvAuditLogs.RowTemplate.Height = 33;

            dgvAuditLogs.CellBorderStyle =
                DataGridViewCellBorderStyle.SingleHorizontal;
        }


        private void dgvAuditLogs_CellFormatting(
            object? sender,
            DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 ||
                e.ColumnIndex < 0 ||
                dgvAuditLogs.Columns[e.ColumnIndex].Name != "Action" ||
                e.Value is not string action ||
                string.IsNullOrWhiteSpace(action))
                return;

            // Kiểm tra CellStyle trước khi sử dụng để tránh CS8602.
            DataGridViewCellStyle? cellStyle = e.CellStyle;

            if (cellStyle == null)
                return;

            // Nền ô thao tác đồng bộ với màu nền hàng.
            cellStyle.BackColor = e.RowIndex % 2 == 0
                ? Color.White
                : Color.FromArgb(239, 246, 253);

            cellStyle.ForeColor = Color.FromArgb(25, 40, 60);
            cellStyle.SelectionBackColor = Color.FromArgb(145, 207, 245);
            cellStyle.SelectionForeColor = Color.FromArgb(15, 35, 60);
        }


        private void SetupTooltips()
        {
            _toolTip.SetToolTip(
                btnSearch,
                "Tìm kiếm nhật ký theo tài khoản hoặc nội dung");

            _toolTip.SetToolTip(
                btnRefresh,
                "Tải lại toàn bộ nhật ký");

            _toolTip.SetToolTip(
                btnExport,
                "Xuất dữ liệu nhật ký đang hiển thị ra Excel");

            _toolTip.SetToolTip(
                chkUseDateFilter,
                "Bật/tắt lọc nhật ký theo khoảng ngày");

            _toolTip.SetToolTip(
                cboQuickFilter,
                "Lọc nhật ký theo loại thao tác");
        }

        private void UpdateStatus(int count)
        {
            UpdatePagination();
        }

        private void SetBusy(bool busy)
        {
            UseWaitCursor = busy;

            btnSearch.Enabled = !busy;
            btnRefresh.Enabled = !busy;
            btnExport.Enabled = !busy;
        }

        private void pnlHeader_Paint(
            object? sender,
            PaintEventArgs e)
        {
            Rectangle bounds = pnlHeader.ClientRectangle;

            if (bounds.Width <= 0 || bounds.Height <= 0)
                return;

            using (LinearGradientBrush brush =
                new LinearGradientBrush(
                    bounds,
                    Color.FromArgb(0, 72, 143),
                    Color.FromArgb(0, 105, 190),
                    0F))
            {
                e.Graphics.FillRectangle(brush, bounds);
            }

            using (Pen wavePen =
                new Pen(Color.FromArgb(35, 255, 255, 255), 2F))
            {
                e.Graphics.DrawArc(
                    wavePen,
                    bounds.Width - 420,
                    -95,
                    450,
                    190,
                    5,
                    165);

                e.Graphics.DrawArc(
                    wavePen,
                    bounds.Width - 350,
                    -110,
                    390,
                    180,
                    5,
                    165);
            }

            // Không tự vẽ đồng hồ tại đây.
            // Biểu tượng History được hiển thị bằng lblHeaderIcon
            // sử dụng font Segoe MDL2 Assets với mã Unicode E81C.

            // Huy hiệu ngôi sao chìm ở góc phải banner.
            using (Pen watermark =
                new Pen(Color.FromArgb(38, 220, 240, 255), 3F))
            using (SolidBrush starBrush =
                new SolidBrush(Color.FromArgb(35, 220, 240, 255)))
            {
                int cx = bounds.Width - 190;
                int cy = 43;

                e.Graphics.DrawArc(
                    watermark,
                    cx - 65,
                    cy - 42,
                    130,
                    100,
                    190,
                    160);

                e.Graphics.DrawArc(
                    watermark,
                    cx - 78,
                    cy - 45,
                    156,
                    110,
                    205,
                    130);

                PointF[] star = Enumerable.Range(0, 10)
                    .Select(i =>
                    {
                        double a = -Math.PI / 2 + i * Math.PI / 5;
                        float r = i % 2 == 0 ? 29 : 13;

                        return new PointF(
                            cx + (float)Math.Cos(a) * r,
                            cy + (float)Math.Sin(a) * r);
                    })
                    .ToArray();

                e.Graphics.FillPolygon(starBrush, star);
            }
        }

        private void txtSearch_TextChanged(
            object? sender, EventArgs e)
        {
            if (_isInitializing)
                return;

            _searchTimer.Stop();
            _searchTimer.Start();
        }

        private async void txtSearch_KeyDown(
            object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            e.SuppressKeyPress = true;

            _searchTimer.Stop();

            await SearchAuditLogsAsync();
        }

        private async void SearchTimer_Tick(
            object? sender, EventArgs e)
        {
            _searchTimer.Stop();

            await SearchAuditLogsAsync();
        }

        private async void btnSearch_Click(
            object? sender, EventArgs e)
        {
            _searchTimer.Stop();

            await SearchAuditLogsAsync();
        }

        private async void btnRefresh_Click(
            object? sender, EventArgs e)
        {
            _searchTimer.Stop();

            _isInitializing = true;

            txtSearch.Clear();
            chkUseDateFilter.Checked = false;
            cboQuickFilter.SelectedIndex = 0;

            _isInitializing = false;

            await LoadAuditLogsAsync();
        }

        private void btnExport_Click(
            object? sender, EventArgs e)
        {
            if (dgvAuditLogs.Rows.Count == 0)
            {
                MessageBox.Show(
                    this,
                    "Không có dữ liệu để xuất.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            if (_allAuditLogs == null)
                return;

            object currentSource = dgvAuditLogs.DataSource!;

            try
            {
                dgvAuditLogs.DataSource = null;
                dgvAuditLogs.DataSource = _allAuditLogs;

                AddSttColumn();
                CustomizeAuditGrid();

                _exportService.Export(dgvAuditLogs);
            }
            finally
            {
                dgvAuditLogs.DataSource = null;
                dgvAuditLogs.DataSource = currentSource;

                AddSttColumn();
                CustomizeAuditGrid();

                UpdatePagination();
            }
        }

        private async void dtpFromDate_ValueChanged(
            object? sender, EventArgs e)
        {
            if (!_isInitializing && chkUseDateFilter.Checked)
                await SearchAuditLogsAsync();
        }

        private async void dtpToDate_ValueChanged(
            object? sender, EventArgs e)
        {
            if (!_isInitializing && chkUseDateFilter.Checked)
                await SearchAuditLogsAsync();
        }

        private async void chkUseDateFilter_CheckedChanged(
            object? sender, EventArgs e)
        {
            dtpFromDate.Enabled = chkUseDateFilter.Checked;
            dtpToDate.Enabled = chkUseDateFilter.Checked;

            if (!_isInitializing)
                await SearchAuditLogsAsync();
        }

        private void cboQuickFilter_SelectedIndexChanged(
            object? sender, EventArgs e)
        {
            if (_isInitializing ||
                cboQuickFilter.SelectedItem == null ||
                _unfilteredAuditLogs == null)
                return;

            ApplyActionFilter();
        }

        protected override void OnFormClosed(
            FormClosedEventArgs e)
        {
            _searchTimer.Stop();
            _searchTimer.Tick -= SearchTimer_Tick;
            _searchTimer.Dispose();

            _toolTip.Dispose();

            base.OnFormClosed(e);
        }
    }
}