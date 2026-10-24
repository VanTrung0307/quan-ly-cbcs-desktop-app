using QuanLyCBCS.Helpers;
using QuanLyCBCS.Data;
using QuanLyCBCS.DTOs;
using QuanLyCBCS.Models;
using QuanLyCBCS.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCBCS.Forms.Home
{
    public partial class FrmHome : Form
    {
        // Màu giao diện theo bản thiết kế
        private readonly Color sidebarColor = Color.FromArgb(19, 54, 94);
        private readonly Color primaryColor = Color.FromArgb(0, 105, 235);
        private readonly Color backgroundColor = Color.FromArgb(239, 247, 255);
        private readonly Color textColor = Color.FromArgb(10, 43, 105);

        private Panel? pnlSidebar;
        private Panel? pnlTop;
        private Panel? pnlContent;
        private Panel? pnlStatus;

        private Label? lblBrand;
        private Label? lblVersion;
        private Label? lblUser;
        private Label? lblStatus;

        // Các nút điều hướng bên trái
        private readonly List<Button> _navigationButtons = new List<Button>();

        // Lưu mục đang được chọn
        private string _currentPage = "Trang chủ";
        public FrmHome()
        {
            InitializeComponent();
            BuildMainLayout();
            Shown += async (sender, e) => await ShowHomeDashboardAsync();
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
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 296F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            pnlSidebar = new GradientPanel
            {
                Name = "pnlSidebar",
                Dock = DockStyle.Fill,
                Margin = Padding.Empty,
                GradientStart = Color.FromArgb(13, 43, 78),
                GradientEnd = Color.FromArgb(29, 67, 112),
                BackColor = sidebarColor,
                DrawWaves = true
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
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 82F));
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            pnlTop = new GradientPanel
            {
                Name = "pnlTop",
                Dock = DockStyle.Fill,
                Margin = Padding.Empty,
                GradientStart = Color.FromArgb(18, 103, 201),
                GradientEnd = Color.FromArgb(225, 240, 255),
                BackColor = Color.FromArgb(225, 240, 255),
                DrawWaves = true,
                GradientHorizontal = true
            };

            pnlContent = new Panel
            {
                Name = "pnlContent",
                Dock = DockStyle.Fill,
                Margin = Padding.Empty,
                BackColor = backgroundColor,
                Padding = new Padding(10)
            };

            lblUser = new Label
            {
                Name = "lblUser",
                Text = $"♟  {CurrentUser.Username} ({CurrentUser.Role})",
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
                BackColor = Color.Transparent,
                Padding = new Padding(12, 7, 10, 7),
                Margin = Padding.Empty
            };
            // Chừa vùng trống bên trái; tài khoản nằm sát bên phải,
            // ngay cạnh nút đăng xuất giống bản thiết kế.
            headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
            headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 54F));
            headerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            var btnLogout = new RoundedLogoutButton
            {
                Name = "btnLogout",
                Text = string.Empty,
                Dock = DockStyle.None,
                BackColor = Color.FromArgb(210, 0, 0),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Symbol", 14F, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter,
                Padding = Padding.Empty,
                CornerRadius = 10,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = Padding.Empty,
                UseVisualStyleBackColor = false,
                Size = new Size(42, 42),
                Anchor = AnchorStyles.None
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Click += (sender, e) =>
            {
                using (var confirmDialog = new LogoutConfirmationDialog())
                {
                    if (confirmDialog.ShowDialog(this) == DialogResult.Yes)
                        Close();
                }
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
                BackColor = Color.FromArgb(24, 59, 98)
            };
            lblStatus = new Label
            {
                Name = "lblStatus",
                Text = $"♟  {CurrentUser.Username}    |    ◈  {CurrentUser.Role}    |    ▦  {DateTime.Now:dd/MM/yyyy HH:mm:ss}    |    ▤  Hiển thị: — CBCS",
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
                BackColor = Color.Transparent
            };

            var picLogo = new PictureBox
            {
                Name = "picLogo",
                Location = new Point(18, 12),
                Size = new Size(52, 58),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };
            if (Icon != null)
                picLogo.Image = Icon.ToBitmap();

            lblBrand = new Label
            {
                Name = "lblBrand",
                Text = "Quản lý CBCS",
                Location = new Point(78, 14),
                Size = new Size(168, 30),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent
            };

            lblVersion = new Label
            {
                Name = "lblVersion",
                Text = "v3.0",
                Location = new Point(79, 43),
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

        private async void NavigationButton_Click(object? sender, EventArgs e)
        {
            if (sender is not Button btn || btn.Tag == null)
                return;

            string pageName = btn.Tag.ToString()!;
            UpdateNavigationHighlight(pageName);

            if (pnlContent == null)
                return;

            if (pageName == "Trang chủ")
            {
                await ShowHomeDashboardAsync();
                return;
            }

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

        // ========================= TRANG CHỦ =========================
        private async Task ShowHomeDashboardAsync()
        {
            if (pnlContent == null)
                return;

            pnlContent.SuspendLayout();
            pnlContent.Controls.Clear();
            pnlContent.ResumeLayout(true);

            try
            {
                List<Personnel> allPersonnel;
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    allPersonnel = await db.Personnel
                        .AsNoTracking()
                        .OrderByDescending(x => x.CreatedAt)
                        .ToListAsync();
                }

                MissionService missionService = new MissionService();
                Task<List<Mission>> recentMissionsTask = missionService.GetRecentMissionsAsync();
                Task<List<PersonnelMissionStatisticDto>> topPersonnelTask = missionService.GetTopPersonnelAsync();

                await Task.WhenAll(recentMissionsTask, topPersonnelTask);

                RenderHomeDashboard(
                    allPersonnel,
                    recentMissionsTask.Result,
                    topPersonnelTask.Result);
            }
            catch (Exception ex)
            {
                Label errorLabel = new Label
                {
                    Dock = DockStyle.Top,
                    AutoSize = false,
                    Height = 70,
                    ForeColor = Color.Firebrick,
                    Font = new Font("Segoe UI", 10F),
                    Text = "Không thể tải dữ liệu Trang chủ. Vui lòng kiểm tra kết nối cơ sở dữ liệu.\r\n" + ex.Message,
                    Padding = new Padding(8),
                    BackColor = Color.White
                };
                pnlContent.Controls.Add(errorLabel);
                errorLabel.BringToFront();
            }
        }

        private void RenderHomeDashboard(
            List<Personnel> personnel,
            List<Mission> recentMissions,
            List<PersonnelMissionStatisticDto> topPersonnel)
        {
            if (pnlContent == null)
                return;

            if (lblStatus != null)
                lblStatus.Text = $"♟  {CurrentUser.Username}    |    ◈  {CurrentUser.Role}    |    ▦  {DateTime.Now:dd/MM/yyyy HH:mm:ss}    |    ▤  Hiển thị: {Math.Min(4, personnel.Count)}/{personnel.Count} CBCS";

            // Giữ tiêu đề hiện có, phần dashboard được đặt bên dưới.
            var dashboard = new TableLayoutPanel
            {
                Name = "pnlHomeDashboard",
                Dock = DockStyle.Fill,
                BackColor = backgroundColor,
                Margin = Padding.Empty,
                Padding = new Padding(0, 2, 0, 0),
                ColumnCount = 1,
                RowCount = 3
            };
            dashboard.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            // Tăng chiều cao vùng tổng quan để tỷ lệ gần bản thiết kế hơn;
            // bảng CBCS và hai bảng phía dưới vẫn giữ đủ không gian hiển thị.
            dashboard.RowStyles.Add(new RowStyle(SizeType.Percent, 28F));
            dashboard.RowStyles.Add(new RowStyle(SizeType.Percent, 36F));
            dashboard.RowStyles.Add(new RowStyle(SizeType.Percent, 36F));

            // Hàng 1: thống kê chung bên trái và bốn thẻ tổng quan bên phải.
            var overview = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 2, 0, 8),
                Padding = Padding.Empty,
                BackColor = backgroundColor,
                ColumnCount = 2,
                RowCount = 1
            };
            overview.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 27.5F));
            overview.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 72.5F));
            overview.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            int maleCount = personnel.Count(x => string.Equals(x.GioiTinh?.Trim(), "Nam", StringComparison.OrdinalIgnoreCase));
            int femaleCount = personnel.Count(x => string.Equals(x.GioiTinh?.Trim(), "Nữ", StringComparison.OrdinalIgnoreCase));
            var ages = personnel.Where(x => x.NgaySinh.HasValue).Select(x => CalculateAge(x.NgaySinh!.Value)).Where(x => x >= 0 && x < 120).ToList();
            string averageAge = ages.Count == 0 ? "—" : ages.Average().ToString("0.0");
            var largestUnit = personnel
                .Where(x => !string.IsNullOrWhiteSpace(x.DonVi))
                .GroupBy(x => x.DonVi.Trim())
                .OrderByDescending(g => g.Count())
                .ThenBy(g => g.Key)
                .FirstOrDefault();
            string largestUnitText = largestUnit == null ? "—" : largestUnit.Key;

            Panel statsPanel = CreateWhitePanel();
            statsPanel.Padding = new Padding(4);
            statsPanel.Margin = new Padding(6, 0, 8, 0);
            var statsLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 5,
                Padding = new Padding(7, 4, 7, 8),
                BackColor = Color.FromArgb(250, 253, 255),
                Margin = Padding.Empty
            };
            // Clip nền của layout con theo góc bo của thẻ cha. Nếu để hình chữ nhật,
            // nền trắng của TableLayoutPanel sẽ phủ lên bốn góc bo của RoundedSurfacePanel.
            ApplyRoundedControlRegion(statsLayout, 11);
            statsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            statsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 23F));
            for (int i = 0; i < 4; i++) statsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 19.25F));
            var statsTitle = new GradientPanel
            {
                Dock = DockStyle.Fill,
                GradientStart = Color.FromArgb(218, 237, 255),
                GradientEnd = Color.FromArgb(234, 246, 255),
                CornerRadius = 10,
                DrawWaves = false,
                Margin = Padding.Empty,
                Padding = new Padding(8, 0, 3, 0),
                BackColor = Color.FromArgb(230, 243, 255)
            };
            Control statsHeading = CreateSectionHeading("\uE9D9", "THỐNG KÊ CHUNG", 11.5F);
            statsHeading.BackColor = Color.Transparent;
            MakeBackgroundTransparent(statsHeading);
            statsTitle.Controls.Add(statsHeading);
            statsLayout.Controls.Add(statsTitle, 0, 0);
            statsLayout.Controls.Add(CreateStatLine("", "Nam:", maleCount.ToString()), 0, 1);
            statsLayout.Controls.Add(CreateStatLine("", "Nữ:", femaleCount.ToString()), 0, 2);
            statsLayout.Controls.Add(CreateStatLine("", "Tuổi TB:", averageAge), 0, 3);
            statsLayout.Controls.Add(CreateStatLine("", "Đơn vị đông nhất:", largestUnitText), 0, 4);
            statsPanel.Controls.Add(statsLayout);
            overview.Controls.Add(statsPanel, 0, 0);

            var cards = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Margin = Padding.Empty,
                Padding = new Padding(0, 10, 0, 12),
                BackColor = backgroundColor,
                ColumnCount = 4,
                RowCount = 1
            };
            for (int i = 0; i < 4; i++) cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            cards.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            cards.Controls.Add(CreateMetricCard("", "Tổng CBCS", personnel.Count.ToString(), Color.FromArgb(0, 119, 255)), 0, 0);
            cards.Controls.Add(CreateMetricCard("\uE77B", "Chiến sĩ", personnel.Count(x => x.PersonnelType == 1).ToString(), Color.FromArgb(29, 132, 207)), 1, 0);
            cards.Controls.Add(CreateMetricCard("", "Cán bộ", personnel.Count(x => x.PersonnelType == 2).ToString(), Color.FromArgb(0, 166, 91)), 2, 0);
            cards.Controls.Add(CreateMetricCard("☭", "Đảng viên", personnel.Count(x => x.DangVien).ToString(), Color.FromArgb(151, 45, 207)), 3, 0);
            overview.Controls.Add(cards, 1, 0);
            // Tạo khoảng thở giữa khu vực thống kê và danh sách CBCS như bản thiết kế.
            overview.Margin = new Padding(0, 2, 0, 10);

            // Hàng 2: danh sách CBCS mới cập nhật.
            DataGridView personnelGrid;
            Panel personnelPanel = CreateGridSection("\uE8D4", "DANH SÁCH CÁN BỘ CHIẾN SĨ", out personnelGrid);
            personnelPanel.Margin = new Padding(0, 0, 0, 12);
            DataTable personnelTable = new DataTable();
            personnelTable.Columns.Add("STT");
            personnelTable.Columns.Add("Số hiệu");
            personnelTable.Columns.Add("Họ tên");
            personnelTable.Columns.Add("Đơn vị");
            personnelTable.Columns.Add("Giới tính");
            personnelTable.Columns.Add("Ngày sinh");
            personnelTable.Columns.Add("Quê quán");
            personnelTable.Columns.Add("Hộ khẩu thường trú");
            personnelTable.Columns.Add("Cấp bậc");
            personnelTable.Columns.Add("Chức vụ");
            personnelTable.Columns.Add("CCCD");
            personnelTable.Columns.Add("Số điện thoại");
            var recentPersonnel = personnel.OrderByDescending(x => x.CreatedAt).Take(4).ToList();
            for (int i = 0; i < recentPersonnel.Count; i++)
            {
                Personnel x = recentPersonnel[i];
                personnelTable.Rows.Add(
                    (i + 1).ToString(), x.SoHieu, x.HoTen, x.DonVi, x.GioiTinh,
                    x.NgaySinh.HasValue ? x.NgaySinh.Value.ToString("dd/MM/yyyy") : "",
                    FirstNonEmpty(x.QueQuanMoi, x.QueQuanCu, x.TinhMoi, x.TinhCu),
                    FirstNonEmpty(x.HKTTMoi, x.HKTTCu), x.CapBacHam, x.ChucVu, x.CCCD, x.SoDienThoai);
            }
            if (personnelTable.Rows.Count == 0)
                personnelTable.Rows.Add("", "", "Chưa có dữ liệu CBCS", "", "", "", "", "", "", "", "", "");
            personnelGrid.DataSource = personnelTable;
            // Màu dòng theo bản thiết kế: dòng đầu xanh nhạt, dòng thứ ba vàng kem,
            // các dòng còn lại trắng/xanh rất nhạt để dễ rà soát thông tin.
            void ApplyPersonnelRowPalette()
            {
                for (int rowIndex = 0; rowIndex < personnelGrid.Rows.Count; rowIndex++)
                {
                    DataGridViewRow row = personnelGrid.Rows[rowIndex];
                    if (rowIndex == 0)
                        row.DefaultCellStyle.BackColor = Color.FromArgb(150, 218, 248);
                    else if (rowIndex == 2)
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 249, 226);
                    else if (rowIndex % 2 == 1)
                        row.DefaultCellStyle.BackColor = Color.White;
                    else
                        row.DefaultCellStyle.BackColor = Color.FromArgb(246, 250, 255);
                }
            }
            personnelGrid.DataBindingComplete += (sender, e) => ApplyPersonnelRowPalette();
            ApplyPersonnelRowPalette();
            SetColumnWidths(personnelGrid, new[] { 48, 104, 170, 96, 84, 112, 174, 190, 110, 126, 144, 138 });
            dashboard.Controls.Add(overview, 0, 0);
            dashboard.Controls.Add(personnelPanel, 0, 1);

            // Hàng 3: công tác gần đây và top CBCS tham gia nhiều công tác.
            var bottom = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Margin = Padding.Empty,
                Padding = Padding.Empty,
                BackColor = backgroundColor,
                ColumnCount = 2,
                RowCount = 1
            };
            bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            bottom.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            DataGridView missionGrid;
            Panel missionPanel = CreateGridSection("\uE9D5", "5 CÔNG TÁC GẦN NHẤT", out missionGrid);
            missionPanel.Margin = new Padding(0, 0, 6, 0);
            DataTable missionTable = new DataTable();
            missionTable.Columns.Add("STT");
            missionTable.Columns.Add("Tên nhiệm vụ");
            missionTable.Columns.Add("Loại nhiệm vụ");
            missionTable.Columns.Add("Cho phép chồng");
            missionTable.Columns.Add("Thời gian bắt đầu");
            var orderedMissions = recentMissions
                .OrderByDescending(x => x.StartTime)
                .Take(5)
                .ToList();
            for (int i = 0; i < orderedMissions.Count; i++)
            {
                Mission m = orderedMissions[i];
                missionTable.Rows.Add(i + 1, m.MissionName, m.MissionType ?? "", m.AllowOverlap ? "Có" : "Không", m.StartTime.ToString("dd/MM/yyyy HH:mm"));
            }
            if (missionTable.Rows.Count == 0)
                missionTable.Rows.Add("", "Chưa có dữ liệu công tác", "", "", "");
            missionGrid.DataSource = missionTable;
            SetColumnWidths(missionGrid, new[] { 48, 180, 122, 128, 174 });

            DataGridView topGrid;
            Panel topPanel = CreateGridSection("\uE7C1", "TOP 5 CBCS CÔNG TÁC NHIỀU NHẤT", out topGrid);
            topPanel.Margin = new Padding(6, 0, 0, 0);
            DataTable topTable = new DataTable();
            topTable.Columns.Add("STT");
            topTable.Columns.Add("Số hiệu");
            topTable.Columns.Add("Họ tên");
            topTable.Columns.Add("Đơn vị");
            topTable.Columns.Add("Cấp bậc hàm");
            topTable.Columns.Add("Số lần");
            var orderedTopPersonnel = (topPersonnel ?? new List<PersonnelMissionStatisticDto>())
                .OrderByDescending(x => x.SoLanCongTac)
                .ThenBy(x => x.HoTen)
                .Take(5)
                .ToList();
            for (int i = 0; i < orderedTopPersonnel.Count; i++)
            {
                PersonnelMissionStatisticDto x = orderedTopPersonnel[i];
                topTable.Rows.Add(i + 1, x.SoHieu, x.HoTen, x.DonVi, x.CapBacHam ?? "", x.SoLanCongTac);
            }
            if (topTable.Rows.Count == 0)
                topTable.Rows.Add("", "", "Chưa có dữ liệu CBCS", "", "", "");
            topGrid.DataSource = topTable;
            SetColumnWidths(topGrid, new[] { 48, 110, 168, 98, 134, 74 });

            bottom.Controls.Add(missionPanel, 0, 0);
            bottom.Controls.Add(topPanel, 1, 0);
            dashboard.Controls.Add(bottom, 0, 2);

            // Dashboard chiếm toàn bộ vùng nội dung bên dưới thanh tài khoản.
            pnlContent.SuspendLayout();
            pnlContent.Controls.Add(dashboard);
            dashboard.BringToFront();
            pnlContent.ResumeLayout(true);
        }

        private static int CalculateAge(DateTime birthDate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age;
        }

        private static string FirstNonEmpty(params string?[] values)
        {
            return values.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x)) ?? "";
        }

        private static void titleBringToFront(Control parent)
        {
            Control? title = parent.Controls.Cast<Control>().FirstOrDefault(x => x.Name == "lblWelcome");
            title?.BringToFront();
        }

        private Panel CreateWhitePanel()
        {
            return new RoundedSurfacePanel
            {
                BackColor = Color.White,
                Dock = DockStyle.Fill,
                Margin = Padding.Empty,
                Padding = new Padding(2),
                CornerRadius = 18,
                BorderColor = Color.FromArgb(72, 135, 210),
                BorderStyle = BorderStyle.None
            };
        }

        private Control CreateSectionHeading(string glyph, string text, float fontSize = 11F)
        {
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Margin = Padding.Empty,
                Padding = Padding.Empty,
                BackColor = Color.FromArgb(250, 253, 255)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            var icon = new Label
            {
                Text = glyph,
                Dock = DockStyle.Fill,
                ForeColor = primaryColor,
                Font = new Font("Segoe MDL2 Assets", 18F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = Padding.Empty
            };
            var label = new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                ForeColor = textColor,
                Font = new Font("Segoe UI", fontSize, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = Padding.Empty,
                AutoEllipsis = true
            };
            layout.Controls.Add(icon, 0, 0);
            layout.Controls.Add(label, 1, 0);
            return layout;
        }

        private static void MakeBackgroundTransparent(Control parent)
        {
            foreach (Control child in parent.Controls)
            {
                child.BackColor = Color.Transparent;
                if (child.HasChildren) MakeBackgroundTransparent(child);
            }
        }

        private Control CreateStatLine(string glyph, string labelText, string value)
        {
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                Margin = Padding.Empty,
                Padding = new Padding(8, 0, 8, 0),
                BackColor = Color.White
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 28F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 57F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 43F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layout.Controls.Add(new Label
            {
                Text = glyph,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe MDL2 Assets", 14F, FontStyle.Bold),
                ForeColor = textColor,
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = Padding.Empty
            }, 0, 0);
            layout.Controls.Add(new Label
            {
                Text = labelText,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F),
                ForeColor = textColor,
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = Padding.Empty,
                AutoEllipsis = true
            }, 1, 0);
            layout.Controls.Add(new Label
            {
                Text = value,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = textColor,
                TextAlign = ContentAlignment.MiddleRight,
                Margin = Padding.Empty,
                AutoEllipsis = true
            }, 2, 0);
            return layout;
        }

        private Panel CreateMetricCard(string glyph, string caption, string value, Color color)
        {
            Color endColor = ControlPaint.Light(color, 0.22F);
            var card = new GradientPanel
            {
                Dock = DockStyle.Fill,
                GradientStart = color,
                GradientEnd = endColor,
                CornerRadius = 12,
                DrawWaves = true,
                Margin = new Padding(5, 7, 5, 7),
                Padding = new Padding(8),
                BackColor = color
            };
            var icon = new Label
            {
                Text = glyph,
                Dock = DockStyle.Left,
                Width = 70,
                ForeColor = Color.White,
                Font = new Font(glyph == "☭" ? "Segoe UI Symbol" : "Segoe MDL2 Assets", glyph == "☭" ? 32F : 34F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            var textLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = Color.Transparent,
                Margin = Padding.Empty,
                Padding = new Padding(5, 20, 0, 4)
            };
            textLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            textLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 43F));
            textLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 57F));
            textLayout.Controls.Add(new Label
            {
                Text = caption,
                Dock = DockStyle.Fill,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F),
                TextAlign = ContentAlignment.BottomCenter,
                Padding = new Padding(0, 0, 0, 7),
                AutoEllipsis = true,
                Margin = Padding.Empty
            }, 0, 0);
            textLayout.Controls.Add(new Label
            {
                Text = value,
                Dock = DockStyle.Fill,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 27F, FontStyle.Bold),
                TextAlign = ContentAlignment.TopCenter,
                Padding = new Padding(0, 5, 0, 0),
                Margin = Padding.Empty
            }, 0, 1);
            card.Controls.Add(textLayout);
            card.Controls.Add(icon);
            return card;
        }

        private Panel CreateGridSection(string glyph, string heading, out DataGridView grid)
        {
            Panel panel = CreateWhitePanel();

            // Dùng TableLayoutPanel thay vì chồng hai control Dock Top/Fill.
            // Cách này bảo đảm tiêu đề khu vực và tiêu đề cột của DataGridView
            // có hai vùng riêng, tránh tình trạng hàng tiêu đề bị che khuất.
            var sectionLayout = new TableLayoutPanel
            {
                Name = "layout" + heading.Replace(" ", "").Replace("/", ""),
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Margin = Padding.Empty,
                Padding = new Padding(5, 2, 5, 6),
                BackColor = Color.White
            };
            // Cắt phần nền trắng theo góc bo để không còn bốn góc vuông che đường viền.
            ApplyRoundedControlRegion(sectionLayout, 14);
            sectionLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            sectionLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            sectionLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            var header = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(8, 3, 8, 3),
                Margin = Padding.Empty
            };
            Control sectionHeading = CreateSectionHeading(glyph, heading, 10.5F);
            header.Controls.Add(sectionHeading);

            if (heading == "5 CÔNG TÁC GẦN NHẤT" ||
                heading == "TOP 5 CBCS CÔNG TÁC NHIỀU NHẤT")
            {
                // Dùng một nhãn duy nhất để chữ và dấu chevron luôn thẳng hàng theo chiều dọc.
                var viewAll = new Label
                {
                    Name = heading.StartsWith("5 CÔNG TÁC") ? "lnkViewAllMissions" : "lnkViewAllTopPersonnel",
                    Text = "Xem tất cả   ›",
                    AutoSize = false,
                    Size = new Size(142, 32),
                    Location = new Point(Math.Max(0, header.ClientSize.Width - 150), 5),
                    BackColor = Color.White,
                    ForeColor = primaryColor,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleRight,
                    Padding = new Padding(0, 0, 4, 0),
                    Cursor = Cursors.Hand
                };
                void PositionViewAll()
                {
                    viewAll.Location = new Point(Math.Max(0, header.ClientSize.Width - 150), 5);
                }
                header.Resize += (sender, e) => PositionViewAll();
                EventHandler navigateToSection = (sender, e) =>
                {
                    string destination = heading.StartsWith("5 CÔNG TÁC") ? "Công tác" : "Quản lý CBCS";
                    Button? nav = _navigationButtons.FirstOrDefault(b => string.Equals(b.Tag?.ToString(), destination, StringComparison.Ordinal));
                    if (nav != null) NavigationButton_Click(nav, EventArgs.Empty);
                };
                viewAll.Click += navigateToSection;
                header.Controls.Add(viewAll);
                viewAll.BringToFront();
            }

            grid = new DataGridView
            {
                Name = "grid" + heading.Replace(" ", "").Replace("/", ""),
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = Color.FromArgb(164, 195, 231),
                EnableHeadersVisualStyles = false,
                ColumnHeadersVisible = true,
                ColumnHeadersHeight = 44,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                RowTemplate = { Height = 42 },
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                AllowUserToResizeColumns = true,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoGenerateColumns = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                ScrollBars = ScrollBars.Both,
                Font = new Font("Segoe UI", 9.5F),
                CellBorderStyle = DataGridViewCellBorderStyle.Single
            };
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(31, 59, 91);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(4, 2, 4, 2);
            grid.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.Single;
            grid.AdvancedCellBorderStyle.All = DataGridViewAdvancedCellBorderStyle.Single;
            grid.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.Single;
            grid.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.Single;
            grid.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.Single;
            grid.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.Single;
            grid.DefaultCellStyle.BackColor = Color.White;
            grid.DefaultCellStyle.ForeColor = textColor;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(195, 229, 250);
            grid.DefaultCellStyle.SelectionForeColor = textColor;
            grid.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(246, 250, 255);
            grid.RowHeadersDefaultCellStyle.BackColor = Color.White;
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            // Capture a local reference instead of the out parameter in event lambdas (CS1628).
            DataGridView roundedGrid = grid;
            roundedGrid.Resize += (sender, e) => ApplyRoundedGridRegion(roundedGrid, 7);
            roundedGrid.HandleCreated += (sender, e) => ApplyRoundedGridRegion(roundedGrid, 7);

            sectionLayout.Controls.Add(header, 0, 0);
            sectionLayout.Controls.Add(grid, 0, 1);
            panel.Controls.Add(sectionLayout);
            return panel;
        }

        /// <summary>
        /// Giới hạn vùng vẽ của layout con theo hình bo góc. Điều này ngăn nền hình chữ nhật
        /// của TableLayoutPanel che mất các góc bo của thẻ RoundedSurfacePanel bên ngoài.
        /// </summary>
        private static void ApplyRoundedControlRegion(Control control, int radius)
        {
            if (control == null) return;

            void UpdateRegion()
            {
                if (control.Width < 3 || control.Height < 3)
                {
                    control.Region = null;
                    return;
                }

                int diameter = Math.Min(radius * 2, Math.Min(control.Width, control.Height));
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddArc(0, 0, diameter, diameter, 180, 90);
                    path.AddArc(control.Width - diameter - 1, 0, diameter, diameter, 270, 90);
                    path.AddArc(control.Width - diameter - 1, control.Height - diameter - 1, diameter, diameter, 0, 90);
                    path.AddArc(0, control.Height - diameter - 1, diameter, diameter, 90, 90);
                    path.CloseFigure();
                    Region? previousRegion = control.Region;
                    control.Region = new Region(path);
                    previousRegion?.Dispose();
                }
            }

            control.Resize += (sender, e) => UpdateRegion();
            control.HandleCreated += (sender, e) => UpdateRegion();
            UpdateRegion();
        }

        private static void ApplyRoundedGridRegion(DataGridView grid, int radius)
        {
            if (grid == null || grid.Width < 3 || grid.Height < 3) return;
            int d = Math.Min(radius * 2, Math.Min(grid.Width, grid.Height));
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(0, 0, d, d, 180, 90);
                path.AddArc(grid.Width - d - 1, 0, d, d, 270, 90);
                path.AddArc(grid.Width - d - 1, grid.Height - d - 1, d, d, 0, 90);
                path.AddArc(0, grid.Height - d - 1, d, d, 90, 90);
                path.CloseFigure();
                grid.Region = new Region(path);
            }
        }

        private static void SetColumnWidths(DataGridView grid, int[] weights)
        {
            void ApplyWidths()
            {
                for (int i = 0; i < grid.Columns.Count; i++)
                {
                    DataGridViewColumn column = grid.Columns[i];
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    int weight = i < weights.Length ? Math.Max(1, weights[i]) : 80;
                    column.MinimumWidth = i == 0 ? 38 : Math.Max(65, Math.Min(weight, 150));
                    if (i < weights.Length)
                        column.FillWeight = weight;
                }
            }

            grid.DataBindingComplete += (sender, e) => ApplyWidths();
            grid.SizeChanged += (sender, e) => ApplyWidths();
            if (grid.Columns.Count > 0)
                ApplyWidths();
        }

        /// <summary>Panel có nền chuyển sắc, tương thích với các thuộc tính GradientStart/GradientEnd.</summary>
        internal sealed class GradientPanel : Panel
        {
            public Color GradientStart { get; set; } = Color.FromArgb(13, 43, 78);
            public Color GradientEnd { get; set; } = Color.FromArgb(29, 67, 112);
            public bool GradientHorizontal { get; set; } = false;
            public bool DrawWaves { get; set; } = false;
            public int CornerRadius { get; set; } = 0;

            public GradientPanel()
            {
                DoubleBuffered = true;
                ResizeRedraw = true;
            }

            protected override void OnResize(EventArgs e)
            {
                base.OnResize(e);
                if (CornerRadius <= 0 || Width <= 1 || Height <= 1)
                {
                    Region = null;
                    return;
                }

                int diameter = Math.Min(CornerRadius * 2, Math.Min(Width, Height));
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddArc(0, 0, diameter, diameter, 180, 90);
                    path.AddArc(Width - diameter - 1, 0, diameter, diameter, 270, 90);
                    path.AddArc(Width - diameter - 1, Height - diameter - 1, diameter, diameter, 0, 90);
                    path.AddArc(0, Height - diameter - 1, diameter, diameter, 90, 90);
                    path.CloseFigure();
                    Region = new Region(path);
                }
            }

            protected override void OnPaintBackground(PaintEventArgs e)
            {
                if (ClientRectangle.Width <= 0 || ClientRectangle.Height <= 0)
                    return;

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    ClientRectangle,
                    GradientStart,
                    GradientEnd,
                    GradientHorizontal ? LinearGradientMode.Horizontal : LinearGradientMode.Vertical))
                {
                    if (CornerRadius > 0)
                    {
                        int diameter = Math.Min(CornerRadius * 2, Math.Min(Width, Height));
                        using (GraphicsPath path = new GraphicsPath())
                        {
                            path.AddArc(0, 0, diameter, diameter, 180, 90);
                            path.AddArc(Width - diameter - 1, 0, diameter, diameter, 270, 90);
                            path.AddArc(Width - diameter - 1, Height - diameter - 1, diameter, diameter, 0, 90);
                            path.AddArc(0, Height - diameter - 1, diameter, diameter, 90, 90);
                            path.CloseFigure();
                            e.Graphics.FillPath(brush, path);
                        }
                    }
                    else
                    {
                        e.Graphics.FillRectangle(brush, ClientRectangle);
                    }
                }

                // Soft layered wave shapes inspired by the reference dashboard.
                // They are deliberately translucent so labels and controls remain legible.
                if (DrawWaves && Width > 20 && Height > 20)
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    using (var waveBrush1 = new SolidBrush(Color.FromArgb(24, Color.White)))
                    using (var waveBrush2 = new SolidBrush(Color.FromArgb(22, 0, 93, 190)))
                    using (var waveBrush3 = new SolidBrush(Color.FromArgb(16, 255, 255, 255)))
                    using (var wave1 = new GraphicsPath())
                    using (var wave2 = new GraphicsPath())
                    using (var wave3 = new GraphicsPath())
                    {
                        int h = Height;
                        int w = Width;
                        int band = Math.Max(28, h / 3);
                        wave1.StartFigure();
                        wave1.AddBezier(-w * 0.05f, h - band, w * 0.22f, h - band * 1.65f,
                            w * 0.52f, h - band * 0.15f, w * 1.05f, h - band * 0.78f);
                        wave1.AddLine(w + 2, h + 2, -w * 0.05f, h + 2);
                        wave1.CloseFigure();
                        wave2.StartFigure();
                        wave2.AddBezier(-w * 0.04f, h - band * 0.50f, w * 0.28f, h - band * 1.25f,
                            w * 0.63f, h - band * 0.05f, w * 1.04f, h - band * 0.40f);
                        wave2.AddLine(w + 2, h + 2, -w * 0.04f, h + 2);
                        wave2.CloseFigure();
                        wave3.StartFigure();
                        wave3.AddBezier(-w * 0.05f, h - band * 0.95f, w * 0.24f, h - band * 1.35f,
                            w * 0.58f, h - band * 0.55f, w * 1.04f, h - band * 0.90f);
                        wave3.AddLine(w + 2, h + 2, -w * 0.05f, h + 2);
                        wave3.CloseFigure();
                        e.Graphics.FillPath(waveBrush1, wave1);
                        e.Graphics.FillPath(waveBrush2, wave2);
                        e.Graphics.FillPath(waveBrush3, wave3);
                    }
                }
            }
        }

        internal sealed class RoundedSurfacePanel : Panel
        {
            public int CornerRadius { get; set; } = 9;
            public Color BorderColor { get; set; } = Color.FromArgb(91, 151, 218);

            public RoundedSurfacePanel()
            {
                DoubleBuffered = true;
                ResizeRedraw = true;
                BackColor = Color.White;
                BorderStyle = BorderStyle.None;
            }

            protected override void OnResize(EventArgs e)
            {
                base.OnResize(e);
                // Không cắt Region theo đúng mép control: việc clip làm mất nửa nét viền
                // tại các góc bo. Các control con đã được đặt lùi vào trong bằng Padding.
                Region = null;
                Invalidate();
            }

            private GraphicsPath CreateRoundedPath(float inset)
            {
                float w = Width - 1f - inset * 2f;
                float h = Height - 1f - inset * 2f;
                float d = Math.Min(CornerRadius * 2f, Math.Min(w, h));
                var path = new GraphicsPath();
                if (w < 2f || h < 2f) return path;
                path.AddArc(inset, inset, d, d, 180, 90);
                path.AddArc(inset + w - d, inset, d, d, 270, 90);
                path.AddArc(inset + w - d, inset + h - d, d, d, 0, 90);
                path.AddArc(inset, inset + h - d, d, d, 90, 90);
                path.CloseFigure();
                return path;
            }

            protected override void OnPaintBackground(PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.Clear(Parent?.BackColor ?? BackColor);
                using (GraphicsPath path = CreateRoundedPath(0.5f))
                using (SolidBrush brush = new SolidBrush(BackColor))
                {
                    if (path.PointCount > 0) e.Graphics.FillPath(brush, path);
                }
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                if (Width < 3 || Height < 3) return;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                // Vẽ viền phía trong để toàn bộ nét 2px vẫn nằm trong vùng hiển thị.
                using (GraphicsPath path = CreateRoundedPath(1.25f))
                using (Pen pen = new Pen(BorderColor, 2.0f) { LineJoin = LineJoin.Round })
                {
                    if (path.PointCount > 0) e.Graphics.DrawPath(pen, path);
                }
            }
        }

        /// <summary>
        /// Hộp thoại xác nhận đăng xuất theo giao diện thiết kế riêng,
        /// thay thế MessageBox mặc định của Windows.
        /// </summary>
        internal sealed class LogoutConfirmationDialog : Form
        {
            private readonly Color navy = Color.FromArgb(20, 61, 105);
            private readonly Color blue = Color.FromArgb(0, 105, 235);
            private readonly Button btnLogout;
            private readonly Button btnCancel;
            private readonly Button btnClose;

            public LogoutConfirmationDialog()
            {
                Text = "Xác nhận đăng xuất";
                FormBorderStyle = FormBorderStyle.None;
                StartPosition = FormStartPosition.CenterParent;
                ClientSize = new Size(720, 340);
                MinimumSize = new Size(720, 340);
                MaximumSize = new Size(720, 340);
                ShowInTaskbar = false;
                ShowIcon = false;
                BackColor = Color.White;
                Font = new Font("Segoe UI", 11F, FontStyle.Regular);
                KeyPreview = true;
                DoubleBuffered = true;

                var header = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 66,
                    BackColor = navy,
                    Padding = new Padding(22, 0, 14, 0)
                };
                header.Paint += (sender, e) =>
                {
                    using (var brush = new LinearGradientBrush(header.ClientRectangle,
                        Color.FromArgb(20, 59, 99), Color.FromArgb(43, 112, 180), 0F))
                        e.Graphics.FillRectangle(brush, header.ClientRectangle);
                };

                var titleIcon = new LogoutGlyph
                {
                    Dock = DockStyle.Left,
                    Width = 40,
                    Margin = Padding.Empty,
                    BackColor = Color.Transparent,
                    ForeColor = Color.White,
                    Glyph = LogoutGlyphType.Exit
                };
                var title = new Label
                {
                    Text = "Xác nhận đăng xuất",
                    Dock = DockStyle.Fill,
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(14, 0, 0, 0),
                    BackColor = Color.Transparent
                };
                btnClose = new Button
                {
                    Text = "×",
                    Dock = DockStyle.Right,
                    Width = 42,
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.White,
                    BackColor = Color.Transparent,
                    Font = new Font("Segoe UI Light", 26F, FontStyle.Regular),
                    Cursor = Cursors.Hand,
                    TabStop = false
                };
                btnClose.FlatAppearance.BorderSize = 0;
                btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 255, 255, 255);
                btnClose.FlatAppearance.MouseDownBackColor = Color.FromArgb(100, 255, 255, 255);
                btnClose.Click += (sender, e) => { DialogResult = DialogResult.Cancel; Close(); };
                header.Controls.Add(title);
                header.Controls.Add(btnClose);
                header.Controls.Add(titleIcon);

                var body = new Panel
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.White,
                    Padding = new Padding(30, 16, 26, 14)
                };
                var bodyLayout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.Transparent,
                    Margin = Padding.Empty,
                    Padding = Padding.Empty,
                    ColumnCount = 2,
                    RowCount = 1
                };
                bodyLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 135F));
                bodyLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                bodyLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                var question = new QuestionMarkBadge
                {
                    Dock = DockStyle.Fill,
                    Margin = new Padding(0, 0, 12, 0),
                    BackColor = Color.White
                };
                var textLayout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.Transparent,
                    ColumnCount = 1,
                    RowCount = 2,
                    Margin = Padding.Empty,
                    Padding = new Padding(0, 2, 0, 0)
                };
                textLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                textLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
                textLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                var message = new Label
                {
                    Text = "Bạn có chắc chắn muốn đăng xuất khỏi hệ thống?",
                    Dock = DockStyle.Fill,
                    ForeColor = Color.FromArgb(19, 51, 91),
                    Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                    TextAlign = ContentAlignment.BottomLeft,
                    AutoEllipsis = false,
                    BackColor = Color.Transparent
                };
                var description = new Label
                {
                    Text = "Bạn sẽ cần đăng nhập lại để tiếp tục sử dụng các chức năng\ncủa hệ thống.",
                    Dock = DockStyle.Fill,
                    ForeColor = Color.FromArgb(93, 116, 149),
                    Font = new Font("Segoe UI", 11F, FontStyle.Regular),
                    TextAlign = ContentAlignment.TopLeft,
                    Padding = new Padding(2, 6, 0, 0),
                    BackColor = Color.Transparent
                };
                textLayout.Controls.Add(message, 0, 0);
                textLayout.Controls.Add(description, 0, 1);
                bodyLayout.Controls.Add(question, 0, 0);
                bodyLayout.Controls.Add(textLayout, 1, 0);
                body.Controls.Add(bodyLayout);

                var footer = new Panel
                {
                    Dock = DockStyle.Bottom,
                    Height = 76,
                    BackColor = Color.FromArgb(239, 246, 253),
                    Padding = new Padding(18, 14, 20, 14)
                };
                footer.Paint += (sender, e) =>
                {
                    using (var pen = new Pen(Color.FromArgb(225, 235, 247), 1F))
                        e.Graphics.DrawLine(pen, 0, 0, footer.Width, 0);
                };
                var buttons = new FlowLayoutPanel
                {
                    Dock = DockStyle.Right,
                    Width = 318,
                    FlowDirection = FlowDirection.LeftToRight,
                    WrapContents = false,
                    BackColor = Color.Transparent,
                    Padding = Padding.Empty,
                    Margin = Padding.Empty
                };
                btnLogout = new LogoutActionButton
                {
                    Text = "  Đăng xuất",
                    Width = 166,
                    Height = 44,
                    Margin = new Padding(0, 0, 10, 0),
                    BackColor = Color.FromArgb(19, 70, 130),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    Glyph = LogoutGlyphType.Exit,
                    IsPrimary = true
                };
                btnLogout.Click += (sender, e) => { DialogResult = DialogResult.Yes; Close(); };
                btnCancel = new LogoutActionButton
                {
                    Text = "Hủy",
                    Width = 122,
                    Height = 44,
                    Margin = Padding.Empty,
                    BackColor = Color.FromArgb(247, 250, 254),
                    ForeColor = Color.FromArgb(76, 99, 130),
                    Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    IsPrimary = false
                };
                btnCancel.Click += (sender, e) => { DialogResult = DialogResult.Cancel; Close(); };
                buttons.Controls.Add(btnLogout);
                buttons.Controls.Add(btnCancel);
                footer.Controls.Add(buttons);

                Controls.Add(body);
                Controls.Add(footer);
                Controls.Add(header);
                AcceptButton = btnLogout;
                CancelButton = btnCancel;
                KeyDown += (sender, e) =>
                {
                    if (e.KeyCode == Keys.Escape)
                    {
                        DialogResult = DialogResult.Cancel;
                        Close();
                    }
                };
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = RoundedLogoutButtonPath(new Rectangle(1, 1, Width - 3, Height - 3), 14))
                using (var pen = new Pen(Color.FromArgb(79, 157, 226), 2F))
                    e.Graphics.DrawPath(pen, path);
            }

            private static GraphicsPath RoundedLogoutButtonPath(Rectangle bounds, int radius)
            {
                int d = radius * 2;
                var path = new GraphicsPath();
                path.AddArc(bounds.Left, bounds.Top, d, d, 180, 90);
                path.AddArc(bounds.Right - d, bounds.Top, d, d, 270, 90);
                path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
                path.AddArc(bounds.Left, bounds.Bottom - d, d, d, 90, 90);
                path.CloseFigure();
                return path;
            }
        }

        internal enum LogoutGlyphType { Exit }

        internal sealed class LogoutGlyph : Control
        {
            public LogoutGlyphType Glyph { get; set; }
            public LogoutGlyph() { SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true); }
            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var pen = new Pen(ForeColor, 3.2F) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round })
                {
                    float x = Width / 2F - 7F, y = Height / 2F;
                    e.Graphics.DrawLine(pen, x + 2, y, x + 22, y);
                    e.Graphics.DrawLine(pen, x + 15, y - 7, x + 22, y);
                    e.Graphics.DrawLine(pen, x + 15, y + 7, x + 22, y);
                    e.Graphics.DrawLine(pen, x - 9, y - 14, x - 9, y + 14);
                    e.Graphics.DrawLine(pen, x - 9, y - 14, x + 3, y - 14);
                    e.Graphics.DrawLine(pen, x - 9, y + 14, x + 3, y + 14);
                }
            }
        }

        internal sealed class QuestionMarkBadge : Control
        {
            public QuestionMarkBadge() { SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true); }
            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                float d = Math.Min(Math.Min(Width - 20, Height - 20), 145);
                var r = new RectangleF((Width - d) / 2F, (Height - d) / 2F, d, d);
                using (var outer = new SolidBrush(Color.FromArgb(224, 238, 255))) e.Graphics.FillEllipse(outer, r);
                r.Inflate(-15, -15);
                using (var middle = new SolidBrush(Color.FromArgb(193, 220, 255))) e.Graphics.FillEllipse(middle, r);
                r.Inflate(-15, -15);
                using (var inner = new SolidBrush(Color.FromArgb(37, 126, 226))) e.Graphics.FillEllipse(inner, r);
                using (var font = new Font("Segoe UI", d * 0.53F, FontStyle.Bold, GraphicsUnit.Pixel))
                using (var brush = new SolidBrush(Color.White))
                using (var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    e.Graphics.DrawString("?", font, brush, r, format);
            }
        }

        internal sealed class LogoutActionButton : Button
        {
            public LogoutGlyphType Glyph { get; set; }
            public bool IsPrimary { get; set; }
            private bool _hovered, _pressed;
            public LogoutActionButton()
            {
                FlatStyle = FlatStyle.Flat;
                FlatAppearance.BorderSize = 0;
                UseVisualStyleBackColor = false;
                SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
                UpdateRoundedRegion();
                Resize += (sender, e) => UpdateRoundedRegion();
                HandleCreated += (sender, e) => UpdateRoundedRegion();
            }

            protected override void OnPaintBackground(PaintEventArgs e)
            {
                // Region của control đã được cắt bo góc, vì vậy nền chỉ được
                // tô trong vùng bo góc và không tạo khung chữ nhật phía sau.
                using (var brush = new SolidBrush(BackColor))
                    e.Graphics.FillRectangle(brush, ClientRectangle);
            }

            private void UpdateRoundedRegion()
            {
                if (Width < 2 || Height < 2 || !IsHandleCreated) return;
                using (var path = CreateRoundedPath(new Rectangle(0, 0, Width, Height), 10))
                {
                    Region? oldRegion = Region;
                    Region = new Region(path);
                    oldRegion?.Dispose();
                }
            }

            private static GraphicsPath CreateRoundedPath(Rectangle bounds, int radius)
            {
                int d = radius * 2;
                var path = new GraphicsPath();
                path.AddArc(bounds.Left, bounds.Top, d, d, 180, 90);
                path.AddArc(bounds.Right - d - 1, bounds.Top, d, d, 270, 90);
                path.AddArc(bounds.Right - d - 1, bounds.Bottom - d - 1, d, d, 0, 90);
                path.AddArc(bounds.Left, bounds.Bottom - d - 1, d, d, 90, 90);
                path.CloseFigure();
                return path;
            }

            protected override void OnMouseEnter(EventArgs e) { base.OnMouseEnter(e); _hovered = true; Invalidate(); }
            protected override void OnMouseLeave(EventArgs e) { base.OnMouseLeave(e); _hovered = false; _pressed = false; Invalidate(); }
            protected override void OnMouseDown(MouseEventArgs e) { base.OnMouseDown(e); if (e.Button == MouseButtons.Left) _pressed = true; Invalidate(); }
            protected override void OnMouseUp(MouseEventArgs e) { base.OnMouseUp(e); _pressed = false; Invalidate(); }
            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Color fill = IsPrimary
                    ? (_pressed ? Color.FromArgb(10, 45, 89) : _hovered ? Color.FromArgb(31, 104, 183) : Color.FromArgb(19, 70, 130))
                    : (_pressed ? Color.FromArgb(225, 236, 249) : _hovered ? Color.FromArgb(232, 242, 253) : Color.FromArgb(247, 250, 254));
                Color border = IsPrimary ? Color.FromArgb(0, 111, 245) : Color.FromArgb(190, 209, 232);
                using (var path = new GraphicsPath())
                {
                    int r = 10, d = r * 2;
                    path.AddArc(0, 0, d, d, 180, 90);
                    path.AddArc(Width - d - 1, 0, d, d, 270, 90);
                    path.AddArc(Width - d - 1, Height - d - 1, d, d, 0, 90);
                    path.AddArc(0, Height - d - 1, d, d, 90, 90);
                    path.CloseFigure();
                    using (var b = new SolidBrush(fill)) e.Graphics.FillPath(b, path);
                    using (var p = new Pen(border, IsPrimary ? 2F : 1.7F)) e.Graphics.DrawPath(p, path);
                }
                if (IsPrimary)
                {
                    using (var pen = new Pen(Color.White, 2.8F) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round })
                    {
                        float cy = Height / 2F, x = 27F;
                        e.Graphics.DrawLine(pen, x - 7, cy - 12, x - 7, cy + 12);
                        e.Graphics.DrawLine(pen, x - 7, cy - 12, x + 3, cy - 12);
                        e.Graphics.DrawLine(pen, x - 7, cy + 12, x + 3, cy + 12);
                        e.Graphics.DrawLine(pen, x + 1, cy, x + 16, cy);
                        e.Graphics.DrawLine(pen, x + 10, cy - 7, x + 16, cy);
                        e.Graphics.DrawLine(pen, x + 10, cy + 7, x + 16, cy);
                    }
                    TextRenderer.DrawText(e.Graphics, "Đăng xuất", Font, new Rectangle(58, 0, Width - 65, Height), ForeColor,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
                }
                else
                {
                    TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, ForeColor,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
                }
            }
        }

        internal sealed class RoundedLogoutButton : Button
        {
            public int CornerRadius { get; set; } = 10;
            private bool _isHovered;
            private bool _isPressed;

            public RoundedLogoutButton()
            {
                FlatStyle = FlatStyle.Flat;
                FlatAppearance.BorderSize = 0;
                UseVisualStyleBackColor = false;
                ResizeRedraw = true;
                BackColor = Color.FromArgb(210, 0, 0);
                SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            }

            protected override void OnMouseEnter(EventArgs e)
            {
                base.OnMouseEnter(e);
                _isHovered = true;
                Invalidate();
            }

            protected override void OnMouseLeave(EventArgs e)
            {
                base.OnMouseLeave(e);
                _isHovered = false;
                _isPressed = false;
                Invalidate();
            }

            protected override void OnMouseDown(MouseEventArgs e)
            {
                base.OnMouseDown(e);
                if (e.Button == MouseButtons.Left) _isPressed = true;
                Invalidate();
            }

            protected override void OnMouseUp(MouseEventArgs e)
            {
                base.OnMouseUp(e);
                _isPressed = false;
                Invalidate();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                // Draw a standard power glyph with a clear gap at 12 o'clock and a centered stem.
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Color faceColor = _isPressed
                    ? Color.FromArgb(155, 0, 0)
                    : _isHovered ? Color.FromArgb(235, 35, 35) : Color.FromArgb(210, 0, 0);
                using (var brush = new SolidBrush(faceColor))
                using (var path = new GraphicsPath())
                using (var pen = new Pen(Color.White, 3.4F) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round })
                {
                    e.Graphics.Clear(Parent?.BackColor ?? Color.Transparent);
                    int d = Math.Min(CornerRadius * 2, Math.Min(Width, Height));
                    using (var round = new GraphicsPath())
                    {
                        round.AddArc(0, 0, d, d, 180, 90);
                        round.AddArc(Width - d - 1, 0, d, d, 270, 90);
                        round.AddArc(Width - d - 1, Height - d - 1, d, d, 0, 90);
                        round.AddArc(0, Height - d - 1, d, d, 90, 90);
                        round.CloseFigure();
                        e.Graphics.FillPath(brush, round);
                    }
                    float cx = Width / 2f;
                    float cxOffset = 0f;
                    float cy = Height / 2f;
                    float r = Math.Min(Width, Height) * 0.205f;
                    // Gap at 12 o'clock, matching the standard power glyph.
                    e.Graphics.DrawArc(pen, cx - r + cxOffset, cy - r, r * 2, r * 2, 315f, 270f);
                    e.Graphics.DrawLine(pen, cx + cxOffset, cy - r - 3.0f, cx + cxOffset, cy - 0.5f);
                }
            }

            protected override void OnResize(EventArgs e)
            {
                base.OnResize(e);
                if (Width < 2 || Height < 2) return;

                int d = Math.Min(CornerRadius * 2, Math.Min(Width, Height));
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddArc(0, 0, d, d, 180, 90);
                    path.AddArc(Width - d - 1, 0, d, d, 270, 90);
                    path.AddArc(Width - d - 1, Height - d - 1, d, d, 0, 90);
                    path.AddArc(0, Height - d - 1, d, d, 90, 90);
                    path.CloseFigure();
                    Region = new Region(path);
                }
            }
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
