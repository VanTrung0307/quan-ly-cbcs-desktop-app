using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyCBCS.Forms.UpdateForms
{
    partial class FrmUpdate
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlRoot;
        private Panel pnlHeader;
        private Label lblHeaderIcon;
        private Label lblHeaderTitle;
        private Label lblHeaderSubtitle;
        private TableLayoutPanel layoutMain;
        private Panel pnlLeft;
        private Panel pnlRight;
        private Panel pnlCurrent;
        private Panel pnlCurrentTop;
        private Label lblCurrentTitle;
        private Label lblCurrentVersion;
        private Label lblCurrentStatus;
        private Label lblLastCheck;
        private Label lblUpdatedBy;
        private FlowLayoutPanel flowActions;
        private Button btnCheckUpdate;
        private Button btnUpdateOptions;
        private Button btnUpdateHistory;
        private Panel pnlVersion;
        private Label lblVersionHeader;
        private TabControl tabVersion;
        private TabPage tabRelease;
        private TabPage tabHistory;
        private Panel pnlReleaseContent;
        private Label lblLatestVersion;
        private Label lblReleaseType;
        private Label lblReleaseDate;
        private Label lblPackageSize;
        private Label lblReleaseNotesTitle;
        private RichTextBox txtReleaseNotes;
        private Panel pnlReleaseActions;
        private Button btnDownloadUpdate;
        private Button btnViewDetails;
        private Panel pnlSystem;
        private Label lblSystemHeader;
        private TableLayoutPanel tableSystem;
        private Label lblSystemVersionValue;
        private Label lblPlatformValue;
        private Label lblDatabaseValue;
        private Label lblOsValue;
        private Label lblAppSizeValue;
        private Panel pnlOptions;
        private Label lblOptionsHeader;
        private UpdateOptionCheckBox chkAutoCheck;
        private UpdateOptionCheckBox chkPreDownload;
        private UpdateOptionCheckBox chkNotify;
        private UpdateOptionCheckBox chkOnlyPatches;
        private Panel pnlNotice;
        private Label lblNoticeHeader;
        private Label lblNoticeText;
        private Panel pnlDownload;
        private Label lblDownloadStatus;
        private ProgressBar progressDownload;
        private Label lblDownloadPercent;
        private Button btnOpenDownloadFolder;
        private ListBox lstUpdateHistory;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            pnlRoot = new Panel();
            pnlHeader = new Panel();
            lblHeaderIcon = new Label();
            lblHeaderTitle = new Label();
            lblHeaderSubtitle = new Label();
            layoutMain = new TableLayoutPanel();
            pnlLeft = new Panel();
            pnlRight = new Panel();
            pnlCurrent = new Panel();
            pnlCurrentTop = new Panel();
            lblCurrentTitle = new Label();
            lblCurrentVersion = new Label();
            lblCurrentStatus = new Label();
            lblLastCheck = new Label();
            lblUpdatedBy = new Label();
            flowActions = new FlowLayoutPanel();
            btnCheckUpdate = new Button();
            btnUpdateOptions = new Button();
            btnUpdateHistory = new Button();
            pnlVersion = new Panel();
            lblVersionHeader = new Label();
            tabVersion = new TabControl();
            tabRelease = new TabPage();
            tabHistory = new TabPage();
            pnlReleaseContent = new Panel();
            lblLatestVersion = new Label();
            lblReleaseType = new Label();
            lblReleaseDate = new Label();
            lblPackageSize = new Label();
            lblReleaseNotesTitle = new Label();
            txtReleaseNotes = new RichTextBox();
            pnlReleaseActions = new Panel();
            btnDownloadUpdate = new Button();
            btnViewDetails = new Button();
            pnlSystem = new Panel();
            lblSystemHeader = new Label();
            tableSystem = new TableLayoutPanel();
            lblSystemVersionValue = new Label();
            lblPlatformValue = new Label();
            lblDatabaseValue = new Label();
            lblOsValue = new Label();
            lblAppSizeValue = new Label();
            pnlOptions = new Panel();
            lblOptionsHeader = new Label();
            chkAutoCheck = new UpdateOptionCheckBox();
            chkPreDownload = new UpdateOptionCheckBox();
            chkNotify = new UpdateOptionCheckBox();
            chkOnlyPatches = new UpdateOptionCheckBox();
            pnlNotice = new Panel();
            lblNoticeHeader = new Label();
            lblNoticeText = new Label();
            pnlDownload = new Panel();
            lblDownloadStatus = new Label();
            progressDownload = new ProgressBar();
            lblDownloadPercent = new Label();
            btnOpenDownloadFolder = new Button();
            lstUpdateHistory = new ListBox();

            SuspendLayout();

            // Form
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(239, 246, 255);
            ClientSize = new Size(1600, 850);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            MinimumSize = new Size(1280, 720);
            Name = "FrmUpdate";
            Text = "Cập nhật ứng dụng";
            StartPosition = FormStartPosition.CenterScreen;

            // Root
            pnlRoot.Dock = DockStyle.Fill;
            pnlRoot.Padding = new Padding(16, 0, 16, 12);
            pnlRoot.BackColor = Color.FromArgb(239, 246, 255);
            pnlRoot.Controls.Add(layoutMain);
            pnlRoot.Controls.Add(pnlHeader);

            // Header - compact banner matching the reference design
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 104;
            pnlHeader.Margin = new Padding(0);
            pnlHeader.BackColor = Color.FromArgb(0, 91, 190);
            pnlHeader.Paint += (sender, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.Clear(pnlRoot.BackColor);

                Rectangle bannerRect = new Rectangle(0, 0, pnlHeader.Width - 1, pnlHeader.Height - 1);
                using var bannerPath = RoundedRect(bannerRect, 14);
                using var bannerBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
                    bannerRect, Color.FromArgb(0, 112, 224), Color.FromArgb(0, 70, 174), 0F);
                e.Graphics.FillPath(bannerBrush, bannerPath);

                // Keep the icon and text directly on the same continuous banner gradient.

                // Subtle star and laurel watermark on the right.
                using var watermarkBrush = new SolidBrush(Color.FromArgb(58, 210, 232, 255));
                using var star = new System.Drawing.Drawing2D.GraphicsPath();
                PointF center = new PointF(pnlHeader.Width - 104, pnlHeader.Height / 2F);
                PointF[] starPoints = new PointF[10];
                for (int i = 0; i < starPoints.Length; i++)
                {
                    double angle = -Math.PI / 2 + i * Math.PI / 5;
                    float radius = i % 2 == 0 ? 29F : 13F;
                    starPoints[i] = new PointF(center.X + (float)Math.Cos(angle) * radius,
                                               center.Y + (float)Math.Sin(angle) * radius);
                }
                star.AddPolygon(starPoints);
                e.Graphics.FillPath(watermarkBrush, star);

                using var ringPen = new Pen(Color.FromArgb(58, 210, 232, 255), 4F);
                for (int side = -1; side <= 1; side += 2)
                {
                    using var branch = new System.Drawing.Drawing2D.GraphicsPath();
                    PointF startPoint = new PointF(center.X + side * 46F, center.Y + 27F);
                    PointF control1 = new PointF(center.X + side * 79F, center.Y + 12F);
                    PointF control2 = new PointF(center.X + side * 81F, center.Y - 25F);
                    PointF endPoint = new PointF(center.X + side * 47F, center.Y - 32F);
                    branch.AddBezier(startPoint, control1, control2, endPoint);
                    e.Graphics.DrawPath(ringPen, branch);

                    for (int i = 0; i < 5; i++)
                    {
                        float t = i / 4F;
                        float x = center.X + side * (50F + 24F * t);
                        float y = center.Y + 20F - 48F * t;
                        e.Graphics.DrawEllipse(ringPen, x - 6F, y - 4F, 12F, 8F);
                    }
                }
            };

            pnlHeader.Padding = new Padding(0);
            lblHeaderIcon.Location = new Point(10, 8);
            lblHeaderIcon.Size = new Size(88, 88);
            lblHeaderIcon.Text = "⟳";
            lblHeaderIcon.TextAlign = ContentAlignment.MiddleCenter;
            lblHeaderIcon.Font = new Font("Segoe UI Symbol", 39F, FontStyle.Regular);
            lblHeaderIcon.ForeColor = Color.White;
            lblHeaderIcon.BackColor = Color.Transparent;
            lblHeaderTitle.Location = new Point(114, 12);
            lblHeaderTitle.Size = new Size(760, 39);
            lblHeaderTitle.Text = "CẬP NHẬT ỨNG DỤNG";
            lblHeaderTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblHeaderTitle.ForeColor = Color.White;
            lblHeaderTitle.BackColor = Color.Transparent;
            lblHeaderSubtitle.Location = new Point(116, 56);
            lblHeaderSubtitle.Size = new Size(1000, 25);
            lblHeaderSubtitle.Text = "Kiểm tra, tải và cài đặt các bản vá lỗi, cải tiến và nâng cấp mới cho phần mềm Quản lý CBCS";
            lblHeaderSubtitle.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            lblHeaderSubtitle.ForeColor = Color.White;
            lblHeaderSubtitle.BackColor = Color.Transparent;
            pnlHeader.Controls.Add(lblHeaderSubtitle);
            pnlHeader.Controls.Add(lblHeaderTitle);
            pnlHeader.Controls.Add(lblHeaderIcon);

            // Main layout
            layoutMain.Dock = DockStyle.Fill;
            layoutMain.Padding = new Padding(0, 18, 0, 0);
            layoutMain.BackColor = Color.FromArgb(239, 246, 255);
            layoutMain.ColumnCount = 2;
            layoutMain.RowCount = 1;
            layoutMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 63.5F));
            layoutMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36.5F));
            layoutMain.Controls.Add(pnlLeft, 0, 0);
            layoutMain.Controls.Add(pnlRight, 1, 0);
            pnlLeft.Dock = DockStyle.Fill;
            pnlLeft.Padding = new Padding(0, 0, 12, 0);
            pnlLeft.BackColor = Color.FromArgb(239, 246, 255);
            pnlRight.Dock = DockStyle.Fill;
            pnlRight.Padding = new Padding(4, 0, 0, 8);
            // Vùng chứa ba thẻ phải trong suốt về mặt thị giác: chỉ các thẻ riêng lẻ
            // được phép có nền trắng, không để lộ một mảng trắng hình chữ nhật phía sau.
            pnlRight.BackColor = Color.FromArgb(239, 246, 255);

            // Current version card
            pnlCurrent.Dock = DockStyle.Top;
            pnlCurrent.Height = 288;
            pnlCurrent.BackColor = Color.White;
            // Nền của thẻ được giữ đồng nhất; không vẽ thêm một lớp nền chữ nhật
            // chồng lên bề mặt thẻ khi FrmUpdate được nhúng trong FrmHome.
            pnlCurrent.Padding = new Padding(0);
            pnlCurrent.BorderStyle = BorderStyle.None;
            pnlCurrent.Region = new Region(RoundedRect(
                new Rectangle(0, 0, pnlCurrent.Width, pnlCurrent.Height), 12));
            pnlCurrent.SizeChanged += (sender, e) =>
            {
                pnlCurrent.Region?.Dispose();
                pnlCurrent.Region = new Region(RoundedRect(
                    new Rectangle(0, 0, pnlCurrent.Width, pnlCurrent.Height), 12));
            };
            // Minh hoa man hinh ung dung trong the phien ban hien tai.
            var pnlDevice = new Panel
            {
                Location = new Point(24, 16),
                Size = new Size(260, 190),
                BackColor = Color.Transparent
            };
            pnlDevice.Paint += (sender, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using var frameBrush = new SolidBrush(Color.FromArgb(69, 139, 211));
                using var screenBrush = new SolidBrush(Color.FromArgb(32, 91, 151));
                using var standBrush = new SolidBrush(Color.FromArgb(105, 166, 224));
                using (var path = RoundedRect(new Rectangle(8, 8, 244, 132), 12)) e.Graphics.FillPath(frameBrush, path);
                using (var path = RoundedRect(new Rectangle(16, 16, 228, 116), 8)) e.Graphics.FillPath(screenBrush, path);
                e.Graphics.FillRectangle(standBrush, 112, 140, 36, 18);
                using (var path = RoundedRect(new Rectangle(82, 157, 96, 8), 4)) e.Graphics.FillPath(standBrush, path);
            };
            pnlDevice.Controls.Add(new Label { Text = "🛡", Font = new Font("Segoe UI Emoji", 31F), ForeColor = Color.White, BackColor = Color.Transparent, TextAlign = ContentAlignment.MiddleCenter, Location = new Point(91, 19), Size = new Size(78, 50) });
            pnlDevice.Controls.Add(new Label { Text = "Quản lý CBCS", Font = new Font("Segoe UI", 14F, FontStyle.Bold), ForeColor = Color.White, BackColor = Color.Transparent, TextAlign = ContentAlignment.MiddleCenter, Location = new Point(35, 69), Size = new Size(190, 28) });
            pnlDevice.Controls.Add(new Label { Text = "v3.0", Font = new Font("Segoe UI", 11.5F, FontStyle.Bold), ForeColor = Color.White, BackColor = Color.Transparent, TextAlign = ContentAlignment.MiddleCenter, Location = new Point(35, 95), Size = new Size(190, 22) });
            var lblDeviceCheck = new Panel
            {
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                BackColor = Color.FromArgb(34, 184, 72),
                Location = new Point(236, 8),
                Size = new Size(48, 48)
            };
            using (var badgePath = new System.Drawing.Drawing2D.GraphicsPath())
            {
                badgePath.AddEllipse(0, 0, 48, 48);
                lblDeviceCheck.Region = new Region(badgePath);
            }
            lblDeviceCheck.Paint += (sender, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                TextRenderer.DrawText(e.Graphics, "✓", lblDeviceCheck.Font,
                    new Rectangle(0, -1, lblDeviceCheck.Width, lblDeviceCheck.Height),
                    Color.White, TextFormatFlags.HorizontalCenter |
                    TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            };
            var pnlCloudArtwork = new Panel
            {
                Name = "pnlCloudArtwork",
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(Math.Max(300, pnlCurrent.ClientSize.Width - 238), 18),
                Size = new Size(220, 190),
                BackColor = Color.Transparent
            };
            pnlCloudArtwork.Paint += (sender, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using var pale = new SolidBrush(Color.FromArgb(70, 139, 190, 245));
                using var cloud = new SolidBrush(Color.FromArgb(115, 166, 232));
                using var white = new SolidBrush(Color.FromArgb(245, 250, 255));
                e.Graphics.FillEllipse(pale, 12, 24, 42, 42);
                e.Graphics.FillEllipse(pale, 155, 2, 48, 48);
                e.Graphics.FillEllipse(pale, 150, 126, 52, 52);
                e.Graphics.FillEllipse(pale, 5, 130, 48, 48);
                e.Graphics.FillEllipse(cloud, 65, 53, 80, 75);
                e.Graphics.FillEllipse(cloud, 92, 31, 76, 88);
                e.Graphics.FillEllipse(cloud, 132, 60, 45, 55);
                e.Graphics.FillRectangle(cloud, 82, 83, 78, 40);
                Point[] arrow = { new Point(120, 54), new Point(94, 82), new Point(109, 82), new Point(109, 111), new Point(132, 111), new Point(132, 82), new Point(147, 82) };
                e.Graphics.FillPolygon(white, arrow);
                DrawGear(e.Graphics, 28, 24, 17, Color.FromArgb(85, 145, 195, 240));
                DrawGear(e.Graphics, 188, 26, 20, Color.FromArgb(85, 145, 195, 240));
                DrawGear(e.Graphics, 177, 154, 18, Color.FromArgb(85, 145, 195, 240));
                DrawGear(e.Graphics, 34, 160, 18, Color.FromArgb(85, 145, 195, 240));
            };
            pnlCurrent.Controls.Add(pnlCloudArtwork);
            pnlCurrent.Controls.Add(pnlDevice);
            pnlCurrent.Controls.Add(lblDeviceCheck);
            pnlCurrent.Controls.Add(lblUpdatedBy);
            pnlCurrent.Controls.Add(lblLastCheck);
            pnlCurrent.Controls.Add(lblCurrentStatus);
            pnlCurrent.Controls.Add(lblCurrentVersion);
            pnlCurrent.Controls.Add(lblCurrentTitle);
            // Đưa artwork và dấu tích lên trên cùng sau khi đã thêm toàn bộ control.
            pnlCloudArtwork.BringToFront();
            lblDeviceCheck.BringToFront();
            pnlCurrent.Resize += (sender, e) =>
            {
                pnlCloudArtwork.Location = new Point(
                    Math.Max(300, pnlCurrent.ClientSize.Width - pnlCloudArtwork.Width - 18), 18);
                pnlCloudArtwork.BringToFront();
                lblDeviceCheck.BringToFront();
            };
            pnlCurrentTop.Dock = DockStyle.Top;
            pnlCurrentTop.Height = 6;
            pnlCurrentTop.BackColor = Color.FromArgb(0, 105, 220);
            pnlCurrentTop.Visible = false;
            pnlCurrent.Controls.Add(pnlCurrentTop);
            lblDeviceCheck.BringToFront();
            lblCurrentTitle.Location = new Point(300, 12);
            lblCurrentTitle.Size = new Size(400, 25);
            lblCurrentTitle.Text = "Phiên bản hiện tại";
            lblCurrentTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblCurrentTitle.ForeColor = Color.FromArgb(30, 45, 70);
            lblCurrentTitle.BackColor = Color.Transparent;
            lblCurrentVersion.Location = new Point(300, 36);
            lblCurrentVersion.Size = new Size(360, 40);
            lblCurrentVersion.Text = "v—";
            lblCurrentVersion.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblCurrentVersion.ForeColor = Color.FromArgb(8, 36, 125);
            lblCurrentVersion.BackColor = Color.Transparent;
            lblCurrentStatus.Location = new Point(294, 84);
            lblCurrentStatus.Size = new Size(300, 34);
            lblCurrentStatus.Text = "● Chưa kiểm tra cập nhật";
            lblCurrentStatus.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCurrentStatus.ForeColor = Color.FromArgb(90, 105, 125);
            lblCurrentStatus.Padding = new Padding(10, 2, 8, 2);
            lblCurrentStatus.TextAlign = ContentAlignment.MiddleLeft;
            lblCurrentStatus.BackColor = Color.FromArgb(238, 243, 249);
            lblCurrentStatus.Region = new Region(RoundedRect(new Rectangle(0, 0, lblCurrentStatus.Width, lblCurrentStatus.Height), 8));
            lblCurrentStatus.SizeChanged += (sender, e) =>
            {
                lblCurrentStatus.Region?.Dispose();
                lblCurrentStatus.Region = new Region(RoundedRect(new Rectangle(0, 0, lblCurrentStatus.Width, lblCurrentStatus.Height), 8));
            };
            lblLastCheck.Location = new Point(300, 132);
            lblLastCheck.Size = new Size(560, 25);
            lblLastCheck.Text = "▦  Cập nhật lần cuối: Chưa có";
            lblLastCheck.ForeColor = Color.FromArgb(20, 65, 135);
            lblLastCheck.BackColor = Color.Transparent;
            lblLastCheck.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblUpdatedBy.Location = new Point(300, 164);
            lblUpdatedBy.Size = new Size(560, 25);
            lblUpdatedBy.Text = "♟  Người cập nhật: —";
            lblUpdatedBy.ForeColor = Color.FromArgb(20, 65, 135);
            lblUpdatedBy.BackColor = Color.Transparent;
            lblUpdatedBy.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);

            // Actions
            flowActions.Dock = DockStyle.Bottom;
            flowActions.Height = 58;
            flowActions.BackColor = Color.FromArgb(239, 246, 255);
            flowActions.Margin = Padding.Empty;
            flowActions.Margin = new Padding(0);
            // Khoang tho giua the phien ban hien tai, hang nut va the thong tin phien ban
            // theo dung ty le cua ban thiet ke khi FrmUpdate duoc nhung trong FrmHome.
            flowActions.Padding = new Padding(18, 8, 18, 8);
            flowActions.WrapContents = false;
            flowActions.Controls.Add(btnCheckUpdate);
            flowActions.Controls.Add(btnUpdateOptions);
            flowActions.Controls.Add(btnUpdateHistory);
            ConfigureButton(btnCheckUpdate, "⟳  Kiểm tra cập nhật", 220, true);
            ConfigureButton(btnUpdateOptions, "⚙  Tùy chọn cập nhật", 220, false);
            ConfigureButton(btnUpdateHistory, "◷  Lịch sử cập nhật", 220, false);

            btnCheckUpdate.Margin = new Padding(6, 4, 6, 4);
            btnUpdateOptions.Margin = new Padding(6, 4, 6, 4);
            btnUpdateHistory.Margin = new Padding(6, 4, 6, 4);
            btnCheckUpdate.Height = 42;
            btnUpdateOptions.Height = 42;
            btnUpdateHistory.Height = 42;

            Action resizeActionButtons = () =>
            {
                int available = Math.Max(270, flowActions.ClientSize.Width
                    - flowActions.Padding.Left - flowActions.Padding.Right - 24);
                int buttonWidth = Math.Max(90, available / 3);
                btnCheckUpdate.Width = buttonWidth;
                btnUpdateOptions.Width = buttonWidth;
                btnUpdateHistory.Width = buttonWidth;
                btnCheckUpdate.Margin = new Padding(0, 0, 12, 0);
                btnUpdateOptions.Margin = new Padding(0, 0, 12, 0);
                btnUpdateHistory.Margin = new Padding(0);
                ApplyRoundedButton(btnCheckUpdate, 9);
                ApplyRoundedButton(btnUpdateOptions, 9);
                ApplyRoundedButton(btnUpdateHistory, 9);
            };
            flowActions.Resize += (sender, e) => resizeActionButtons();
            btnCheckUpdate.SizeChanged += (sender, e) => ApplyRoundedButton(btnCheckUpdate, 9);
            btnUpdateOptions.SizeChanged += (sender, e) => ApplyRoundedButton(btnUpdateOptions, 9);
            btnUpdateHistory.SizeChanged += (sender, e) => ApplyRoundedButton(btnUpdateHistory, 9);
            resizeActionButtons();

            // Version information card - bố cục theo bản thiết kế tham chiếu.
            pnlVersion.Dock = DockStyle.Top;
            pnlVersion.Height = 410;
            pnlVersion.Margin = new Padding(0, 12, 0, 0);
            pnlVersion.BackColor = Color.Transparent;
            ApplyCardSurface(pnlVersion);
            pnlVersion.Padding = new Padding(0);
            pnlVersion.BorderStyle = BorderStyle.None;
            pnlVersion.Resize += (sender, e) =>
            {
                if (pnlVersion.Width > 4 && pnlVersion.Height > 4)
                {
                    pnlVersion.Region?.Dispose();
                    pnlVersion.Region = new Region(RoundedRect(
                        new Rectangle(0, 0, pnlVersion.Width - 1, pnlVersion.Height - 1), 12));
                }
            };
            pnlVersion.Controls.Add(tabVersion);
            pnlVersion.Controls.Add(lblVersionHeader);

            lblVersionHeader.Dock = DockStyle.Top;
            lblVersionHeader.Height = 40;
            lblVersionHeader.Text = "🚀  Thông tin phiên bản";
            lblVersionHeader.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblVersionHeader.ForeColor = Color.FromArgb(8, 36, 125);
            lblVersionHeader.BackColor = Color.FromArgb(229, 240, 255);
            lblVersionHeader.TextAlign = ContentAlignment.MiddleLeft;
            lblVersionHeader.Padding = new Padding(18, 0, 0, 0);

            tabVersion.Dock = DockStyle.Fill;
            tabVersion.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            tabVersion.BackColor = Color.White;
            tabVersion.Appearance = TabAppearance.FlatButtons;
            tabVersion.Padding = new Point(8, 3);
            tabVersion.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabVersion.ItemSize = new Size(178, 34);
            tabVersion.SizeMode = TabSizeMode.Fixed;
            tabVersion.DrawItem += (sender, e) =>
            {
                bool selected = e.Index == tabVersion.SelectedIndex;
                // Thu hẹp mỗi tab vài pixel để tạo khe hở rõ ràng giữa hai tab.
                Rectangle tabBounds = Rectangle.Inflate(e.Bounds, -3, -1);
                tabBounds.Y += 1;
                tabBounds.Height = Math.Max(0, tabBounds.Height - 2);

                using var background = new SolidBrush(selected
                    ? Color.FromArgb(0, 91, 220)
                    : Color.FromArgb(235, 243, 255));
                using var border = new Pen(Color.FromArgb(190, 211, 242));
                using var tabPath = RoundedRect(tabBounds, 7);
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.FillPath(background, tabPath);
                e.Graphics.DrawPath(border, tabPath);
                TextRenderer.DrawText(e.Graphics, tabVersion.TabPages[e.Index].Text,
                    new Font("Segoe UI", 9F, FontStyle.Bold), tabBounds,
                    selected ? Color.White : Color.FromArgb(8, 36, 125),
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                    TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
            };
            tabVersion.Controls.Add(tabRelease);
            tabVersion.Controls.Add(tabHistory);
            tabRelease.Text = "Phiên bản mới";
            tabHistory.Text = "Lịch sử thay đổi";
            tabRelease.BackColor = Color.White;
            tabHistory.BackColor = Color.White;
            tabRelease.UseVisualStyleBackColor = false;
            tabHistory.UseVisualStyleBackColor = false;
            tabRelease.Padding = new Padding(0);
            tabHistory.Padding = new Padding(0);
            tabRelease.Paint += (sender, e) => e.Graphics.Clear(Color.White);
            tabHistory.Paint += (sender, e) => e.Graphics.Clear(Color.White);

            pnlReleaseContent.Dock = DockStyle.Fill;
            pnlReleaseContent.Padding = new Padding(12);
            pnlReleaseContent.BackColor = Color.White;
            pnlReleaseContent.BorderStyle = BorderStyle.None;
            pnlReleaseContent.Paint += (sender, e) =>
            {
                if (pnlReleaseContent.Width < 4 || pnlReleaseContent.Height < 4)
                    return;

                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using var borderPen = new Pen(Color.FromArgb(205, 222, 248), 1F);
                using var borderPath = RoundedRect(
                    new Rectangle(1, 1, pnlReleaseContent.Width - 3, pnlReleaseContent.Height - 3), 9);
                e.Graphics.DrawPath(borderPen, borderPath);

                int dividerX = Math.Max(185, Math.Min(265, (int)(pnlReleaseContent.ClientSize.Width * 0.285F)));
                using var dividerPen = new Pen(Color.FromArgb(205, 222, 248), 1F);
                e.Graphics.DrawLine(dividerPen, dividerX, 14, dividerX,
                    pnlReleaseContent.Height - 14);
            };
            pnlReleaseContent.Controls.Add(txtReleaseNotes);
            pnlReleaseContent.Controls.Add(lblReleaseNotesTitle);
            pnlReleaseContent.Controls.Add(lblPackageSize);
            pnlReleaseContent.Controls.Add(lblReleaseDate);
            pnlReleaseContent.Controls.Add(lblReleaseType);
            pnlReleaseContent.Controls.Add(lblLatestVersion);

            Action layoutReleaseContent = () =>
            {
                int dividerX = Math.Max(185, Math.Min(265, (int)(pnlReleaseContent.ClientSize.Width * 0.285F)));
                int leftX = 24;
                int notesLeft = dividerX + 14;
                int rightPadding = 14;

                lblLatestVersion.Location = new Point(leftX, 22);
                lblLatestVersion.Size = new Size(Math.Max(130, dividerX - leftX - 12), 44);
                lblReleaseType.Location = new Point(leftX, 72);
                lblReleaseType.Size = new Size(Math.Max(130, dividerX - leftX - 18), 28);
                lblReleaseDate.Location = new Point(leftX, 126);
                lblReleaseDate.Size = new Size(Math.Max(130, dividerX - leftX - 18), 26);
                lblPackageSize.Location = new Point(leftX, 160);
                lblPackageSize.Size = new Size(Math.Max(130, dividerX - leftX - 18), 26);

                lblReleaseNotesTitle.Location = new Point(notesLeft, 18);
                lblReleaseNotesTitle.Size = new Size(Math.Max(180,
                    pnlReleaseContent.ClientSize.Width - notesLeft - rightPadding), 28);
                txtReleaseNotes.Location = new Point(notesLeft, 50);
                txtReleaseNotes.Size = new Size(Math.Max(160,
                    pnlReleaseContent.ClientSize.Width - notesLeft - rightPadding),
                    Math.Max(90, pnlReleaseContent.ClientSize.Height - 66));
                txtReleaseNotes.WordWrap = true;
                pnlReleaseContent.Invalidate();
            };
            pnlReleaseContent.Resize += (sender, e) =>
            {
                layoutReleaseContent();
                pnlReleaseContent.Region?.Dispose();
                pnlReleaseContent.Region = new Region(RoundedRect(
                    new Rectangle(0, 0, pnlReleaseContent.Width, pnlReleaseContent.Height), 10));
            };
            tabRelease.Controls.Add(pnlReleaseContent);

            lblLatestVersion.Text = "v—";
            lblLatestVersion.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblLatestVersion.ForeColor = Color.FromArgb(8, 36, 125);

            lblReleaseType.Text = "● Chưa có dữ liệu";
            lblReleaseType.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblReleaseType.ForeColor = Color.FromArgb(90, 105, 125);
            lblReleaseType.BackColor = Color.FromArgb(238, 243, 249);
            lblReleaseType.TextAlign = ContentAlignment.MiddleLeft;
            lblReleaseType.Padding = new Padding(7, 0, 4, 0);
            lblReleaseType.SizeChanged += (sender, e) =>
            {
                if (lblReleaseType.Width > 2 && lblReleaseType.Height > 2)
                {
                    lblReleaseType.Region?.Dispose();
                    lblReleaseType.Region = new Region(RoundedRect(
                        new Rectangle(0, 0, lblReleaseType.Width - 1,
                            lblReleaseType.Height - 1), 7));
                }
            };

            lblReleaseDate.Text = "▦  Ngày phát hành: —";
            lblReleaseDate.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblReleaseDate.ForeColor = Color.FromArgb(25, 45, 80);

            lblPackageSize.Text = "⇩  Dung lượng: —";
            lblPackageSize.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPackageSize.ForeColor = Color.FromArgb(25, 45, 80);

            lblReleaseNotesTitle.Text = "Nội dung cập nhật";
            lblReleaseNotesTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblReleaseNotesTitle.ForeColor = Color.FromArgb(8, 36, 125);

            txtReleaseNotes.BorderStyle = BorderStyle.None;
            txtReleaseNotes.ReadOnly = true;
            txtReleaseNotes.BackColor = Color.White;
            txtReleaseNotes.Font = new Font("Segoe UI", 9.5F);
            txtReleaseNotes.ForeColor = Color.FromArgb(35, 45, 65);
            txtReleaseNotes.Text = "Nhấn “Kiểm tra cập nhật” để tải thông tin phiên bản mới từ GitHub.";
            layoutReleaseContent();

            pnlReleaseActions.Dock = DockStyle.Bottom;
            pnlReleaseActions.Height = 70;
            pnlReleaseActions.Padding = new Padding(0, 7, 0, 10);
            pnlReleaseActions.BackColor = Color.White;
            pnlReleaseActions.Controls.Add(btnViewDetails);
            pnlReleaseActions.Controls.Add(btnDownloadUpdate);
            ConfigureButton(btnDownloadUpdate, "⇩  Tải và cài đặt phiên bản v3.1.0", 335, true);
            ConfigureButton(btnViewDetails, "↗  Xem chi tiết", 168, false);
            btnDownloadUpdate.BackColor = Color.FromArgb(0, 145, 45);
            btnDownloadUpdate.ForeColor = Color.White;
            btnDownloadUpdate.FlatStyle = FlatStyle.Flat;
            btnDownloadUpdate.FlatAppearance.BorderSize = 0;
            btnDownloadUpdate.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 165, 55);
            btnDownloadUpdate.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 120, 38);
            btnDownloadUpdate.Cursor = Cursors.Hand;
            Action centerReleaseButtons = () =>
            {
                int totalWidth = btnDownloadUpdate.Width + 18 + btnViewDetails.Width;
                int startX = Math.Max(8, (pnlReleaseActions.ClientSize.Width - totalWidth) / 2);
                btnDownloadUpdate.Location = new Point(startX, 7);
                btnViewDetails.Location = new Point(startX + btnDownloadUpdate.Width + 18, 7);
            };
            pnlReleaseActions.Resize += (sender, e) => centerReleaseButtons();
            btnDownloadUpdate.SizeChanged += (sender, e) =>
            {
                if (btnDownloadUpdate.Width > 2 && btnDownloadUpdate.Height > 2)
                {
                    btnDownloadUpdate.Region?.Dispose();
                    btnDownloadUpdate.Region = new Region(RoundedRect(
                        new Rectangle(1, 1, btnDownloadUpdate.Width - 2,
                            btnDownloadUpdate.Height - 2), 9));
                }
            };
            btnViewDetails.SizeChanged += (sender, e) => ApplyRoundedButton(btnViewDetails, 9);
            centerReleaseButtons();
            if (btnDownloadUpdate.Width > 2 && btnDownloadUpdate.Height > 2)
                btnDownloadUpdate.Region = new Region(RoundedRect(
                    new Rectangle(1, 1, btnDownloadUpdate.Width - 2,
                        btnDownloadUpdate.Height - 2), 9));
            ApplyRoundedButton(btnViewDetails, 9);
            btnDownloadUpdate.Enabled = false;
            pnlVersion.Controls.Add(pnlReleaseActions);

            // Right column system info
            pnlSystem.Dock = DockStyle.Top;
            pnlSystem.Height = 270;
            pnlSystem.BackColor = Color.White;
            ApplyCardSurface(pnlSystem);
            pnlSystem.Padding = new Padding(10, 10, 10, 10);
            pnlSystem.BorderStyle = BorderStyle.None;
            ApplyRoundedCardRegion(pnlSystem, 12);
            pnlSystem.Controls.Add(tableSystem);
            pnlSystem.Controls.Add(lblSystemHeader);
            lblSystemHeader.Dock = DockStyle.Top;
            lblSystemHeader.Height = 38;
            lblSystemHeader.Text = "ⓘ  Thông tin hệ thống";
            lblSystemHeader.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblSystemHeader.ForeColor = Color.FromArgb(8, 36, 125);
            lblSystemHeader.BackColor = Color.FromArgb(229, 240, 255);
            lblSystemHeader.TextAlign = ContentAlignment.MiddleLeft;
            lblSystemHeader.Padding = new Padding(10, 0, 0, 0);
            ApplyRoundedHeaderRegion(lblSystemHeader, 8);
            tableSystem.Dock = DockStyle.Fill;
            tableSystem.ColumnCount = 2;
            tableSystem.RowCount = 5;
            tableSystem.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
            tableSystem.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            for (int i = 0; i < 5; i++)
                tableSystem.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableSystem.BackColor = Color.White;
            tableSystem.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            tableSystem.Paint += (sender, e) =>
            {
                using var separatorPen = new Pen(Color.FromArgb(205, 224, 250), 1F);
                int rowHeight = tableSystem.ClientSize.Height / 5;
                for (int row = 1; row < 5; row++)
                {
                    int y = row * rowHeight;
                    e.Graphics.DrawLine(separatorPen, 4, y, tableSystem.ClientSize.Width - 4, y);
                }
            };
            AddSystemRow("Phiên bản hiện tại:", lblSystemVersionValue, 0);
            AddSystemRow("Nền tảng:", lblPlatformValue, 1);
            AddSystemRow("Cơ sở dữ liệu:", lblDatabaseValue, 2);
            AddSystemRow("Hệ điều hành:", lblOsValue, 3);
            AddSystemRow("Dung lượng ứng dụng:", lblAppSizeValue, 4);
            lblSystemVersionValue.Text = "—";
            lblPlatformValue.Text = ".NET 8 (WinForms)";
            lblDatabaseValue.Text = "SQL Server";
            lblOsValue.Text = System.Environment.OSVersion.VersionString;
            lblAppSizeValue.Text = "Đang xác định";

            // Options
            pnlOptions.Dock = DockStyle.Top;
            pnlOptions.Height = 215;
            pnlOptions.Margin = new Padding(0, 14, 0, 0);
            pnlOptions.Padding = new Padding(10, 10, 10, 10);
            pnlOptions.BackColor = Color.White;
            ApplyCardSurface(pnlOptions);
            pnlOptions.BorderStyle = BorderStyle.None;
            ApplyRoundedCardRegion(pnlOptions, 12);
            pnlOptions.Controls.Add(chkOnlyPatches);
            pnlOptions.Controls.Add(chkNotify);
            pnlOptions.Controls.Add(chkPreDownload);
            pnlOptions.Controls.Add(chkAutoCheck);
            pnlOptions.Controls.Add(lblOptionsHeader);
            lblOptionsHeader.Dock = DockStyle.Top;
            lblOptionsHeader.Height = 38;
            lblOptionsHeader.Text = "⚙  Tùy chọn cập nhật";
            lblOptionsHeader.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblOptionsHeader.ForeColor = Color.FromArgb(8, 36, 125);
            lblOptionsHeader.BackColor = Color.FromArgb(229, 240, 255);
            lblOptionsHeader.TextAlign = ContentAlignment.MiddleLeft;
            lblOptionsHeader.Padding = new Padding(10, 0, 0, 0);
            ApplyRoundedHeaderRegion(lblOptionsHeader, 8);
            ConfigureCheck(chkAutoCheck, "Tự động kiểm tra cập nhật khi khởi động", 50);
            ConfigureCheck(chkPreDownload, "Cho phép tải trước bản cập nhật", 88);
            ConfigureCheck(chkNotify, "Thông báo khi có phiên bản mới", 126);
            ConfigureCheck(chkOnlyPatches, "Chỉ nhận bản vá lỗi", 164);
            pnlOptions.Resize += (sender, e) =>
            {
                int available = Math.Max(130, pnlOptions.ClientSize.Height - 58);
                int rowHeight = available / 4;
                ConfigureCheck(chkAutoCheck, chkAutoCheck.Text, 48);
                ConfigureCheck(chkPreDownload, chkPreDownload.Text, 48 + rowHeight);
                ConfigureCheck(chkNotify, chkNotify.Text, 48 + rowHeight * 2);
                ConfigureCheck(chkOnlyPatches, chkOnlyPatches.Text, 48 + rowHeight * 3);
            };
            chkAutoCheck.Checked = true;
            chkNotify.Checked = true;

            // Notice
            pnlNotice.Dock = DockStyle.Top;
            pnlNotice.Height = 190;
            pnlNotice.Margin = new Padding(0, 14, 0, 0);
            pnlNotice.Padding = new Padding(10, 10, 10, 10);
            pnlNotice.BackColor = Color.FromArgb(222, 237, 255);
            pnlNotice.BorderStyle = BorderStyle.None;
            ApplyRoundedCardRegion(pnlNotice, 12);
            pnlNotice.Paint += (sender, e) =>
            {
                if (pnlNotice.Width < 4 || pnlNotice.Height < 4)
                    return;
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using var surfacePath = RoundedRect(
                    new Rectangle(1, 1, pnlNotice.Width - 3, pnlNotice.Height - 3), 12);
                using var surfaceBrush = new SolidBrush(Color.FromArgb(222, 237, 255));
                using var borderPen = new Pen(Color.FromArgb(218, 232, 250), 1F);
                e.Graphics.FillPath(surfaceBrush, surfacePath);
                e.Graphics.DrawPath(borderPen, surfacePath);
            };
            pnlNotice.Controls.Add(lblNoticeText);
            pnlNotice.Controls.Add(lblNoticeHeader);
            lblNoticeHeader.Dock = DockStyle.Top;
            lblNoticeHeader.Height = 38;
            lblNoticeHeader.Text = "ⓘ  Lưu ý";
            lblNoticeHeader.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblNoticeHeader.ForeColor = Color.FromArgb(8, 36, 125);
            lblNoticeHeader.BackColor = Color.FromArgb(222, 237, 255);
            lblNoticeHeader.TextAlign = ContentAlignment.MiddleLeft;
            lblNoticeHeader.Padding = new Padding(10, 0, 0, 0);
            lblNoticeText.Location = new Point(10, 48);
            lblNoticeText.Size = new Size(420, 124);
            lblNoticeText.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlNotice.Resize += (sender, e) =>
            {
                lblNoticeText.Location = new Point(10, 48);
                lblNoticeText.Size = new Size(Math.Max(100, pnlNotice.ClientSize.Width - 20),
                                              Math.Max(80, pnlNotice.ClientSize.Height - 58));
            };
            lblNoticeText.BackColor = Color.FromArgb(222, 237, 255);
            lblNoticeText.Padding = new Padding(12, 8, 10, 6);
            lblNoticeText.Text = "• Nên sao lưu dữ liệu trước khi cài đặt bản nâng cấp.\r\n\r\n• Trong quá trình cập nhật, vui lòng không tắt ứng dụng.\r\n\r\n• Sau khi cập nhật, ứng dụng sẽ tự động khởi động lại.";
            lblNoticeText.Font = new Font("Segoe UI", 9.5F);
            lblNoticeText.ForeColor = Color.FromArgb(35, 50, 75);

            // Download progress
            pnlDownload.Dock = DockStyle.Bottom;
            pnlDownload.Height = 82;
            pnlDownload.Padding = new Padding(10, 8, 10, 4);
            pnlDownload.BackColor = Color.FromArgb(230, 240, 255);
            pnlDownload.Visible = false;
            pnlDownload.Controls.Add(btnOpenDownloadFolder);
            pnlDownload.Controls.Add(lblDownloadPercent);
            pnlDownload.Controls.Add(progressDownload);
            pnlDownload.Controls.Add(lblDownloadStatus);
            lblDownloadStatus.Dock = DockStyle.Top;
            lblDownloadStatus.Height = 22;
            lblDownloadStatus.Text = "Đang chuẩn bị...";
            progressDownload.Dock = DockStyle.Bottom;
            progressDownload.Height = 16;
            lblDownloadPercent.Dock = DockStyle.Right;
            lblDownloadPercent.Width = 55;
            lblDownloadPercent.Text = "0%";
            lblDownloadPercent.TextAlign = ContentAlignment.MiddleRight;
            btnOpenDownloadFolder.Dock = DockStyle.Right;
            btnOpenDownloadFolder.Width = 150;
            btnOpenDownloadFolder.Text = "Mở thư mục tải";
            btnOpenDownloadFolder.Visible = false;
            btnOpenDownloadFolder.Click += btnOpenDownloadFolder_Click;

            // History
            lstUpdateHistory.Dock = DockStyle.Fill;
            lstUpdateHistory.Font = new Font("Segoe UI", 9F);
            lstUpdateHistory.BorderStyle = BorderStyle.None;
            tabHistory.Controls.Add(lstUpdateHistory);

            // Compose columns
            pnlLeft.Controls.Add(pnlVersion);
            pnlLeft.Controls.Add(pnlCurrent);
            pnlCurrent.Controls.Add(flowActions);
            pnlRight.Controls.Add(pnlNotice);
            pnlRight.Controls.Add(pnlOptions);
            pnlRight.Controls.Add(pnlSystem);

            pnlRoot.Controls.Add(pnlDownload);
            Controls.Add(pnlRoot);

            btnCheckUpdate.Click += btnCheckUpdate_Click;
            btnUpdateOptions.Click += btnUpdateOptions_Click;
            btnUpdateHistory.Click += btnUpdateHistory_Click;
            btnDownloadUpdate.Click += btnDownloadUpdate_Click;
            btnViewDetails.Click += btnViewDetails_Click;

            ResumeLayout(false);
        }

        private void ApplyRoundedCardRegion(Panel panel, int radius)
        {
            void UpdateRegion()
            {
                if (panel.Width < 4 || panel.Height < 4)
                    return;

                panel.Region?.Dispose();
                panel.Region = new Region(RoundedRect(
                    new Rectangle(0, 0, panel.Width - 1, panel.Height - 1), radius));
            }

            panel.SizeChanged += (sender, e) => UpdateRegion();
            UpdateRegion();
        }

        private void ApplyRoundedHeaderRegion(Control control, int radius)
        {
            void UpdateRegion()
            {
                if (control.Width < 4 || control.Height < 4)
                    return;

                control.Region?.Dispose();
                control.Region = new Region(RoundedRect(
                    new Rectangle(0, 0, control.Width - 1, control.Height - 1), radius));
            }

            control.SizeChanged += (sender, e) => UpdateRegion();
            UpdateRegion();
        }

        private void ApplyCardSurface(Panel panel)
        {
            panel.BorderStyle = BorderStyle.None;
            panel.Paint += (sender, e) =>
            {
                if (panel.Width < 4 || panel.Height < 4)
                    return;

                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                using var shadowPath = RoundedRect(
                    new Rectangle(2, 3, panel.Width - 5, panel.Height - 6), 12);
                using var shadowBrush = new SolidBrush(Color.FromArgb(18, 35, 91, 160));
                e.Graphics.FillPath(shadowBrush, shadowPath);

                using var surfacePath = RoundedRect(
                    new Rectangle(1, 1, panel.Width - 5, panel.Height - 7), 12);
                using var surfaceBrush = new SolidBrush(Color.White);
                using var borderPen = new Pen(Color.FromArgb(218, 232, 250), 1F);
                e.Graphics.FillPath(surfaceBrush, surfacePath);
                e.Graphics.DrawPath(borderPen, surfacePath);
            };
        }

        private static void DrawGear(Graphics graphics, float centerX, float centerY, float radius, Color color)
        {
            using var pen = new Pen(color, Math.Max(2F, radius * 0.22F));
            graphics.DrawEllipse(pen, centerX - radius * 0.62F, centerY - radius * 0.62F,
                                 radius * 1.24F, radius * 1.24F);
            for (int i = 0; i < 8; i++)
            {
                double angle = i * Math.PI / 4;
                float x1 = centerX + (float)Math.Cos(angle) * radius * 0.68F;
                float y1 = centerY + (float)Math.Sin(angle) * radius * 0.68F;
                float x2 = centerX + (float)Math.Cos(angle) * radius;
                float y2 = centerY + (float)Math.Sin(angle) * radius;
                graphics.DrawLine(pen, x1, y1, x2, y2);
            }
            using var hub = new SolidBrush(color);
            graphics.FillEllipse(hub, centerX - radius * 0.2F, centerY - radius * 0.2F,
                                 radius * 0.4F, radius * 0.4F);
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

        private void ConfigureButton(Button button, string text, int width, bool primary)
        {
            button.Width = width;
            button.Height = 50;
            button.Margin = new Padding(0, 0, 22, 0);
            button.Text = text;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = primary ? 0 : 1;
            button.FlatAppearance.BorderColor = Color.FromArgb(155, 190, 240);
            button.BackColor = primary ? Color.FromArgb(0, 91, 220) : Color.FromArgb(247, 250, 255);
            button.ForeColor = primary ? Color.White : Color.FromArgb(8, 36, 125);
            button.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
        }


        private void ApplyRoundedButton(Button button, int radius)
        {
            if (button.Width < 3 || button.Height < 3)
                return;

            // Đặt đường cắt lùi vào 1 px để FlatAppearance vẽ đủ bốn cạnh,
            // không bị cắt mất nét viền tại các góc bo.
            button.Region?.Dispose();
            button.Region = new Region(RoundedRect(
                new Rectangle(1, 1, button.Width - 2, button.Height - 2), radius));
            bool primary = button == btnCheckUpdate;
            button.FlatAppearance.BorderSize = 0;
            button.Paint -= DrawActionButtonBorder;
            if (!primary)
                button.Paint += DrawActionButtonBorder;
            button.Invalidate();
        }

        private void DrawActionButtonBorder(object sender, PaintEventArgs e)
        {
            if (sender is not Button button || button.Width < 4 || button.Height < 4)
                return;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var pen = new Pen(Color.FromArgb(155, 190, 240), 1F);
            using var path = RoundedRect(
                new Rectangle(1, 1, button.Width - 3, button.Height - 3), 9);
            e.Graphics.DrawPath(pen, path);
        }

        private void ConfigureCheck(UpdateOptionCheckBox checkBox, string text, int top)
        {
            checkBox.Location = new Point(14, top);
            checkBox.Size = new Size(Math.Max(180, pnlOptions.ClientSize.Width - 28), 32);
            checkBox.Text = text;
            checkBox.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            checkBox.ForeColor = Color.FromArgb(25, 40, 65);
            checkBox.BackColor = Color.White;
            checkBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            checkBox.Invalidate();
        }

        private void AddSystemRow(string title, Label value, int row)
        {
            var name = new Label
            {
                Text = title,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(35, 50, 75),
                Padding = new Padding(2, 0, 0, 0)
            };
            value.Dock = DockStyle.Fill;
            value.TextAlign = ContentAlignment.MiddleRight;
            value.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            value.ForeColor = Color.FromArgb(15, 35, 75);
            tableSystem.Controls.Add(name, 0, row);
            tableSystem.Controls.Add(value, 1, row);
        }
    }

    /// <summary>
    /// Checkbox tuỳ chỉnh để đồng bộ màu xanh và kích thước với giao diện ứng dụng.
    /// </summary>
    internal sealed class UpdateOptionCheckBox : CheckBox
    {
        public UpdateOptionCheckBox()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            AutoSize = false;
            Appearance = System.Windows.Forms.Appearance.Normal;
            FlatStyle = FlatStyle.Flat;
            TextAlign = ContentAlignment.MiddleLeft;
            UseVisualStyleBackColor = false;
            TabStop = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            const int boxSize = 16;
            int boxY = (Height - boxSize) / 2;
            var box = new Rectangle(1, boxY, boxSize, boxSize);
            bool isChecked = Checked;
            Color blue = Color.FromArgb(0, 91, 220);

            using (var path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                // Bo góc checkbox nhẹ để đồng bộ với ngôn ngữ thiết kế của ứng dụng.
                const int cornerRadius = 4;
                int diameter = cornerRadius * 2;
                path.AddArc(box.Left, box.Top, diameter, diameter, 180, 90);
                path.AddArc(box.Right - diameter, box.Top, diameter, diameter, 270, 90);
                path.AddArc(box.Right - diameter, box.Bottom - diameter, diameter, diameter, 0, 90);
                path.AddArc(box.Left, box.Bottom - diameter, diameter, diameter, 90, 90);
                path.CloseFigure();

                using var fill = new SolidBrush(isChecked ? blue : Color.White);
                using var border = new Pen(blue, 1.4F);
                e.Graphics.FillPath(fill, path);
                e.Graphics.DrawPath(border, path);
            }

            if (isChecked)
            {
                using var tickPen = new Pen(Color.White, 2.1F)
                {
                    StartCap = System.Drawing.Drawing2D.LineCap.Round,
                    EndCap = System.Drawing.Drawing2D.LineCap.Round
                };
                e.Graphics.DrawLines(tickPen, new[]
                {
                    new Point(4, boxY + 8),
                    new Point(7, boxY + 11),
                    new Point(14, boxY + 4)
                });
            }

            var textRect = new Rectangle(27, 0, Math.Max(0, Width - 29), Height);
            TextRenderer.DrawText(e.Graphics, Text, Font, textRect, ForeColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
                TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);

            if (Focused)
            {
                var focusRect = Rectangle.Inflate(ClientRectangle, -1, -1);
                ControlPaint.DrawFocusRectangle(e.Graphics, focusRect);
            }
        }

        protected override void OnCheckedChanged(EventArgs e)
        {
            base.OnCheckedChanged(e);
            Invalidate();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
        }
    }

}