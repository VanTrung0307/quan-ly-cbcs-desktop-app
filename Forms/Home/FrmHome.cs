using QuanLyCBCS.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace QuanLyCBCS.Forms.Home
{
    public partial class FrmHome : Form
    {
        // Màu giao diện theo bản thiết kế
        private readonly Color sidebarColor = Color.FromArgb(19, 54, 94);
        private readonly Color primaryColor = Color.FromArgb(0, 105, 235);
        private readonly Color backgroundColor = Color.FromArgb(239, 246, 255);
        private readonly Color textColor = Color.FromArgb(10, 43, 105);

        private Panel? pnlSidebar;
        private Panel? pnlTop;
        private Panel? pnlContent;
        private Panel? pnlStatus;

        private Label? lblBrand;
        private Label? lblVersion;
        private Label? lblUser;

        // Các nút điều hướng bên trái
        private readonly List<Button> _navigationButtons = new List<Button>();

        // Lưu mục đang được chọn
        private string _currentPage = "Trang chủ";
        public FrmHome()
        {
            InitializeComponent();
            BuildMainLayout();
        }

        private void BuildMainLayout()
        {
            Text = "Quản lý CBCS";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            MinimumSize = new Size(1200, 700);
            BackColor = backgroundColor;
            Font = new Font("Segoe UI", 10F);
            Controls.Clear();

            // Bố cục chính: sidebar chiếm toàn bộ chiều cao vùng làm việc;
            // thanh trạng thái nằm riêng bên dưới và kéo dài hết chiều rộng form.
            var rootLayout = new TableLayoutPanel
            {
                Name = "rootLayout",
                Dock = DockStyle.Fill,
                Margin = Padding.Empty,
                Padding = Padding.Empty,
                BackColor = backgroundColor,
                ColumnCount = 1,
                RowCount = 2
            };
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));

            var mainLayout = new TableLayoutPanel
            {
                Name = "mainLayout",
                Dock = DockStyle.Fill,
                Margin = Padding.Empty,
                Padding = Padding.Empty,
                BackColor = backgroundColor,
                ColumnCount = 2,
                RowCount = 1
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 248F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            pnlSidebar = new Panel
            {
                Name = "pnlSidebar",
                Dock = DockStyle.Fill,
                Margin = Padding.Empty,
                BackColor = sidebarColor
            };

            var rightLayout = new TableLayoutPanel
            {
                Name = "rightLayout",
                Dock = DockStyle.Fill,
                Margin = Padding.Empty,
                Padding = Padding.Empty,
                BackColor = backgroundColor,
                ColumnCount = 1,
                RowCount = 2
            };
            rightLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            pnlTop = new Panel
            {
                Name = "pnlTop",
                Dock = DockStyle.Fill,
                Margin = Padding.Empty,
                BackColor = Color.FromArgb(224, 240, 255)
            };

            pnlContent = new Panel
            {
                Name = "pnlContent",
                Dock = DockStyle.Fill,
                Margin = Padding.Empty,
                BackColor = backgroundColor,
                Padding = new Padding(10)
            };

            Label lblWelcome = new Label
            {
                Name = "lblWelcome",
                Text = "TRANG CHỦ",
                Dock = DockStyle.Top,
                Height = 55,
                ForeColor = textColor,
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };
            pnlContent.Controls.Add(lblWelcome);

            lblUser = new Label
            {
                Name = "lblUser",
                Text = $"{CurrentUser.Username} ({CurrentUser.Role})",
                Dock = DockStyle.Fill,
                ForeColor = textColor,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleRight,
                Padding = new Padding(0, 0, 18, 0),
                Cursor = Cursors.Hand,
                Margin = Padding.Empty
            };

            // Thanh trên chỉ còn thông tin tài khoản và nút đăng xuất; bỏ ô tìm kiếm.
            var headerLayout = new TableLayoutPanel
            {
                Name = "headerLayout",
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = Color.FromArgb(224, 240, 255),
                Padding = new Padding(12, 7, 10, 7),
                Margin = Padding.Empty
            };
            // Chừa vùng trống bên trái; tài khoản nằm sát bên phải,
            // ngay cạnh nút đăng xuất giống bản thiết kế.
            headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
            headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 48F));
            headerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            var btnLogout = new Button
            {
                Name = "btnLogout",
                Text = "⏻",
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(210, 0, 0),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Symbol", 20F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new Padding(2, 0, 0, 0),
                UseVisualStyleBackColor = false
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Click += (sender, e) =>
            {
                var result = MessageBox.Show(
                    "Bạn có chắc chắn muốn đăng xuất?",
                    "Xác nhận đăng xuất",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                    Close();
            };

            // Cột 0 là khoảng đệm; không cần ô tìm kiếm trên trang chủ.
            headerLayout.Controls.Add(lblUser, 1, 0);
            headerLayout.Controls.Add(btnLogout, 2, 0);
            pnlTop.Controls.Add(headerLayout);

            pnlStatus = new Panel
            {
                Name = "pnlStatus",
                Dock = DockStyle.Fill,
                Margin = Padding.Empty,
                BackColor = Color.FromArgb(27, 65, 107)
            };
            Label lblStatus = new Label
            {
                Name = "lblStatus",
                Text = $"●  {CurrentUser.Username}    |    ◈  {CurrentUser.Role}    |    Quản lý CBCS v3.0",
                Dock = DockStyle.Fill,
                ForeColor = Color.White,
                Padding = new Padding(12, 0, 0, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = Padding.Empty
            };
            pnlStatus.Controls.Add(lblStatus);

            rightLayout.Controls.Add(pnlTop, 0, 0);
            rightLayout.Controls.Add(pnlContent, 0, 1);
            mainLayout.Controls.Add(pnlSidebar, 0, 0);
            mainLayout.Controls.Add(rightLayout, 1, 0);
            rootLayout.Controls.Add(mainLayout, 0, 0);
            rootLayout.Controls.Add(pnlStatus, 0, 1);
            Controls.Add(rootLayout);

            BuildSidebarNavigation();
        }

        private void BuildSidebarNavigation()
        {
            if (pnlSidebar == null)
                return;

            pnlSidebar.SuspendLayout();
            pnlSidebar.Controls.Clear();

            // Header được giữ riêng ở đầu sidebar; menu chỉ bắt đầu bên dưới header.
            var pnlSidebarHeader = new Panel
            {
                Name = "pnlSidebarHeader",
                Dock = DockStyle.Top,
                Height = 88,
                Margin = Padding.Empty,
                BackColor = sidebarColor
            };

            var picLogo = new PictureBox
            {
                Name = "picLogo",
                Location = new Point(14, 17),
                Size = new Size(40, 48),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };
            if (Icon != null)
                picLogo.Image = Icon.ToBitmap();

            lblBrand = new Label
            {
                Name = "lblBrand",
                Text = "Quản lý CBCS",
                Location = new Point(62, 15),
                Size = new Size(178, 30),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent
            };

            lblVersion = new Label
            {
                Name = "lblVersion",
                Text = "v2.0",
                Location = new Point(63, 44),
                Size = new Size(100, 23),
                ForeColor = Color.FromArgb(120, 210, 255),
                Font = new Font("Segoe UI", 10F),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent
            };
            pnlSidebarHeader.Controls.Add(picLogo);
            pnlSidebarHeader.Controls.Add(lblBrand);
            pnlSidebarHeader.Controls.Add(lblVersion);

            string[] menuItems =
            {
                "Trang chủ", "Quản lý CBCS", "Công tác", "Nhật ký",
                "Báo cáo", "Thống kê", "Tài khoản", "Đổi mật khẩu",
                "Phân quyền", "Sao lưu / Phục hồi", "Danh mục", "Hướng dẫn"
            };

            // Glyphs Segoe MDL2 Assets: biểu tượng rõ nét, đồng bộ và có nét đặc.
            string[] menuIcons =
            {
                "\uEA8A", // Trang chủ
                "\uE716", // Quản lý CBCS - nhóm người
                "\uEB4E", // Công tác - lịch
                "\uF739", // Nhật ký - lịch sử
                "\uE9F9", // Báo cáo - tài liệu
                "\uE9D2", // Thống kê - biểu đồ
                "\uE7EE", // Tài khoản - người dùng
                "\uE755", // Đổi mật khẩu - khóa
                "\uE8D7", // Phân quyền
                "\uE8F7", // Sao lưu / Phục hồi - lưu trữ
                "\uE8FD", // Danh mục
                "\uE946"  // Hướng dẫn
            };

            _navigationButtons.Clear();
            var pnlNavigation = new TableLayoutPanel
            {
                Name = "pnlNavigation",
                Dock = DockStyle.Fill,
                Margin = Padding.Empty,
                Padding = new Padding(0, 5, 0, 5),
                BackColor = sidebarColor,
                ColumnCount = 1,
                RowCount = menuItems.Length,
                GrowStyle = TableLayoutPanelGrowStyle.FixedSize
            };
            pnlNavigation.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            float rowPercent = 100F / menuItems.Length;
            for (int i = 0; i < menuItems.Length; i++)
            {
                pnlNavigation.RowStyles.Add(new RowStyle(SizeType.Percent, rowPercent));

                var btn = new RoundedNavigationButton
                {
                    Name = "btnNav" + i,
                    Text = menuItems[i],
                    Tag = menuItems[i],
                    IconGlyph = menuIcons[i],
                    Dock = DockStyle.Fill,
                    Margin = new Padding(9, 2, 9, 2),
                    SelectedColor = primaryColor,
                    NormalColor = sidebarColor,
                    HoverColor = Color.FromArgb(30, 83, 145),
                    ForeColor = Color.White,
                    IconColor = Color.White
                };
                btn.Click += NavigationButton_Click;
                btn.MouseEnter += (sender, e) => btn.Invalidate();
                btn.MouseLeave += (sender, e) => btn.Invalidate();

                _navigationButtons.Add(btn);
                pnlNavigation.Controls.Add(btn, 0, i);
            }

            // Thêm header trước, navigation sau; TableLayoutPanel tự quản lý vùng Fill.
            pnlSidebar.Controls.Add(pnlNavigation);
            pnlSidebar.Controls.Add(pnlSidebarHeader);
            pnlSidebarHeader.Dock = DockStyle.Top;
            pnlNavigation.Dock = DockStyle.Fill;

            pnlSidebar.ResumeLayout(true);
            UpdateNavigationHighlight("Trang chủ");
        }

        private void UpdateNavigationHighlight(string pageName)
        {
            _currentPage = pageName;

            foreach (Button item in _navigationButtons)
            {
                bool isSelected =
                    string.Equals(
                        item.Tag?.ToString(),
                        pageName,
                        StringComparison.Ordinal);

                if (item is RoundedNavigationButton navButton)
                {
                    navButton.Selected = isSelected;
                    navButton.Invalidate();
                }
                else
                {
                    item.BackColor = isSelected
                        ? primaryColor
                        : sidebarColor;

                    item.ForeColor = Color.White;
                }
            }
        }

        private void NavigationButton_Click(object? sender, EventArgs e)
        {
            if (sender is not Button btn || btn.Tag == null)
                return;

            string pageName = btn.Tag.ToString()!;

            // Cập nhật mục đang chọn trên sidebar
            UpdateNavigationHighlight(pageName);

            if (pnlContent == null)
                return;

            // Tạm thời hiển thị tiêu đề trang
            // Sẽ thay bằng nội dung thực tế ở bước tiếp theo
            pnlContent.Controls.Clear();

            Label lblTitle = new Label
            {
                Name = "lblPageTitle",
                Text = pageName.ToUpperInvariant(),
                Dock = DockStyle.Top,
                Height = 60,
                ForeColor = textColor,
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };

            pnlContent.Controls.Add(lblTitle);
        }

        internal sealed class RoundedNavigationButton : Button
        {
            public string IconGlyph { get; set; } = "\uE80F";

            public bool Selected { get; set; }

            public Color SelectedColor { get; set; } =
                Color.FromArgb(0, 105, 235);

            public Color NormalColor { get; set; } =
                Color.FromArgb(19, 54, 94);

            public Color HoverColor { get; set; } =
                Color.FromArgb(30, 83, 145);

            public Color IconColor { get; set; } = Color.White;

            public RoundedNavigationButton()
            {
                SetStyle(
                    ControlStyles.UserPaint |
                    ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.OptimizedDoubleBuffer |
                    ControlStyles.ResizeRedraw,
                    true);

                FlatStyle = FlatStyle.Flat;
                FlatAppearance.BorderSize = 0;
                UseVisualStyleBackColor = false;
                BackColor = NormalColor;
                ForeColor = Color.White;
                Cursor = Cursors.Hand;
                TextAlign = ContentAlignment.MiddleLeft;
                Font = new Font("Segoe UI", 10.5F);
                Margin = Padding.Empty;
                Padding = Padding.Empty;
                TabStop = true;
            }

            protected override void OnMouseEnter(EventArgs e)
            {
                base.OnMouseEnter(e);
                Invalidate();
            }

            protected override void OnMouseLeave(EventArgs e)
            {
                base.OnMouseLeave(e);
                Invalidate();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Parent?.BackColor ?? NormalColor);

                Color fillColor = Selected
                    ? SelectedColor
                    : (ClientRectangle.Contains(PointToClient(MousePosition))
                        ? HoverColor
                        : NormalColor);

                Rectangle rect = new Rectangle(
                    0, 0,
                    Math.Max(1, Width - 1),
                    Math.Max(1, Height - 1));

                using (GraphicsPath path = CreateRoundedPath(rect, 10))
                using (SolidBrush backgroundBrush = new SolidBrush(fillColor))
                {
                    g.FillPath(backgroundBrush, path);
                }

                // Biểu tượng bên trái.
                using (Font iconFont = new Font(
                    "Segoe MDL2 Assets", 16F, FontStyle.Bold))
                using (SolidBrush iconBrush = new SolidBrush(IconColor))
                {
                    Rectangle iconRect = new Rectangle(14, 0, 28, Height);

                    TextRenderer.DrawText(
                        g,
                        IconGlyph,
                        iconFont,
                        iconRect,
                        IconColor,
                        TextFormatFlags.HorizontalCenter |
                        TextFormatFlags.VerticalCenter |
                        TextFormatFlags.NoPadding);
                }

                // Tên mục điều hướng.
                using (SolidBrush textBrush = new SolidBrush(ForeColor))
                {
                    Font textFont = new Font(
                        "Segoe UI",
                        10.5F,
                        Selected ? FontStyle.Bold : FontStyle.Regular);

                    try
                    {
                        Rectangle textRect = new Rectangle(
                            52, 0,
                            Math.Max(0, Width - 58),
                            Height);

                        TextRenderer.DrawText(
                            g,
                            Text,
                            textFont,
                            textRect,
                            ForeColor,
                            TextFormatFlags.Left |
                            TextFormatFlags.VerticalCenter |
                            TextFormatFlags.EndEllipsis |
                            TextFormatFlags.NoPadding);
                    }
                    finally
                    {
                        textFont.Dispose();
                    }
                }

                // Viền focus mảnh khi người dùng điều hướng bằng bàn phím.
                if (Focused && ShowFocusCues)
                {
                    Rectangle focusRect = Rectangle.Inflate(rect, -4, -4);

                    using (Pen focusPen = new Pen(
                        Color.FromArgb(180, Color.White), 1F))
                    {
                        focusPen.DashStyle = DashStyle.Dot;
                        g.DrawRectangle(
                            focusPen,
                            focusRect.X,
                            focusRect.Y,
                            focusRect.Width,
                            focusRect.Height);
                    }
                }
            }

            private static GraphicsPath CreateRoundedPath(
                Rectangle bounds,
                int radius)
            {
                GraphicsPath path = new GraphicsPath();

                if (bounds.Width <= 0 || bounds.Height <= 0)
                    return path;

                int diameter = Math.Min(
                    radius * 2,
                    Math.Min(bounds.Width, bounds.Height));

                Rectangle arc = new Rectangle(
                    bounds.X,
                    bounds.Y,
                    diameter,
                    diameter);

                path.AddArc(arc, 180, 90);

                arc.X = bounds.Right - diameter;
                path.AddArc(arc, 270, 90);

                arc.Y = bounds.Bottom - diameter;
                path.AddArc(arc, 0, 90);

                arc.X = bounds.X;
                path.AddArc(arc, 90, 90);

                path.CloseFigure();
                return path;
            }
        }
    }
}
