using OpenXmlDrawing = DocumentFormat.OpenXml.Drawing;
using QuanLyCBCS.DTOs.BackupRestore;
using QuanLyCBCS.Forms.Auth;
using QuanLyCBCS.Helpers.BackupRestore;
using QuanLyCBCS.Services.BackupRestore;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCBCS.Forms.DataForms
{
    [SupportedOSPlatform("windows6.1")]
    public partial class FrmBackRestore : Form
    {
        private readonly BackupRestoreService _backupService;

        private RoundedTextBox txtBackupDirectory = null!;
        private RoundedTextBox txtBackupFileName = null!;
        private RoundedTextBox txtRestoreFile = null!;

        private Button btnChooseDirectory = null!;
        private Button btnChooseRestoreFile = null!;
        private Button btnBackup = null!;
        private Button btnRestore = null!;
        private Button btnRefresh = null!;
        private Button btnDelete = null!;

        private DataGridView dgvBackups = null!;
        private ListView lvActivity = null!;

        private Label lblFooter = null!;
        private RoundedStatusPanel lblBackupStatus = null!;
        private Label lblRestoreStatus = null!;

        private bool _busy;
        private int _hoverOpenRowIndex = -1;

        private readonly Color _primary =
            Color.FromArgb(0, 99, 220);

        private readonly Color _darkBlue =
            Color.FromArgb(8, 48, 105);

        private readonly Color _background =
            Color.FromArgb(239, 247, 255);

        public FrmBackRestore()
        {
            InitializeComponent();

            _backupService = new BackupRestoreService();

            BuildInterface();

            Load += FrmBackRestore_Load;
        }

        // =====================================================
        // 1. KHỞI TẠO FORM
        // =====================================================

        private void FrmBackRestore_Load(object? sender, EventArgs e)
        {
            txtBackupDirectory.Text =
                BackupRestoreHelper.LoadBackupDirectory();

            txtBackupFileName.Text =
                BackupRestoreHelper.GenerateBackupFileName();

            UpdateFooter();

            RefreshBackupList();
            RefreshActivityList();
        }

        // =====================================================
        // 2. XÂY DỰNG GIAO DIỆN
        // =====================================================

        private void BuildInterface()
        {
            SuspendLayout();

            Controls.Clear();

            BackColor = _background;
            Font = new Font("Segoe UI", 9.5F);
            FormBorderStyle = FormBorderStyle.None;
            Dock = DockStyle.Fill;
            Padding = Padding.Empty;

            MinimumSize = new Size(1050, 650);

            TableLayoutPanel root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = _background,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };

            // Banner cao 76px, có khoảng đệm 10px phía trên/dưới.
            root.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 96));

            root.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100));

            root.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 38));

            // =====================================================
            // BANNER - SAO LƯU / PHỤC HỒI DỮ LIỆU
            // =====================================================
            Panel banner = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Margin = new Padding(11, 10, 11, 10),
                Padding = new Padding(22, 4, 10, 4)
            };

            // Vẽ nền gradient, bo góc và watermark ở góc phải.
            banner.Paint += (sender, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                Rectangle rect = new Rectangle(
                    0, 0,
                    Math.Max(1, banner.ClientSize.Width - 1),
                    Math.Max(1, banner.ClientSize.Height - 1));

                using (GraphicsPath path = CreateBannerRoundPath(rect, 12))
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    rect,
                    Color.FromArgb(0, 103, 215),
                    Color.FromArgb(0, 62, 137),
                    LinearGradientMode.Horizontal))
                {
                    g.FillPath(brush, path);
                }

                DrawBannerWatermark(g, banner.ClientSize);
            };

            // Biểu tượng database + đồng bộ.
            Label bannerIcon = new Label
            {
                Text = string.Empty,
                Dock = DockStyle.Left,
                Width = 75,
                BackColor = Color.Transparent
            };

            bannerIcon.Paint += (sender, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                using (Pen pen = new Pen(Color.White, 3.0F))
                {
                    Rectangle body = new Rectangle(18, 12, 38, 43);

                    // Miệng và thân cơ sở dữ liệu.
                    g.DrawEllipse(pen, body.X, body.Y, body.Width, 13);
                    g.DrawLine(pen, body.Left, body.Y + 6,
                        body.Left, body.Bottom - 6);
                    g.DrawLine(pen, body.Right, body.Y + 6,
                        body.Right, body.Bottom - 6);
                    g.DrawArc(pen, body.X, body.Bottom - 13,
                        body.Width, 13, 0, 180);

                    // Các tầng dữ liệu.
                    g.DrawArc(pen, body.X, body.Y + 14,
                        body.Width, 13, 0, 180);
                    g.DrawArc(pen, body.X, body.Y + 27,
                        body.Width, 13, 0, 180);

                    // Mũi tên phục hồi ở góc dưới bên phải.
                    g.DrawArc(pen, 37, 37, 28, 28, 35, 285);
                    Point[] arrow =
                    {
                        new Point(58, 37),
                        new Point(65, 38),
                        new Point(62, 45)
                    };
                    g.DrawLines(pen, arrow);
                }
            };

            Panel bannerText = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            Label title = new Label
            {
                Text = "SAO LƯU / PHỤC HỒI DỮ LIỆU",
                Dock = DockStyle.Top,
                Height = 37,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };

            Label subtitle = new Label
            {
                Text = "Sao lưu, phục hồi toàn bộ cơ sở dữ liệu và dữ liệu hệ thống",
                Dock = DockStyle.Fill,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F),
                TextAlign = ContentAlignment.TopLeft
            };

            bannerText.Controls.Add(subtitle);
            bannerText.Controls.Add(title);
            banner.Controls.Add(bannerText);
            banner.Controls.Add(bannerIcon);

            // CONTENT
            TableLayoutPanel content = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = _background,
                Margin = Padding.Empty
            };

            // Tăng nhẹ chiều cao hàng Sao lưu/Phục hồi để nút Phục hồi
            // (cao 48px) hiển thị trọn vẹn, đồng thời chỉ khôi phục một
            // khoảng đệm nhỏ bên dưới phần chú thích/lưu ý.
            // Phần chiều cao còn lại vẫn dành cho Danh sách/Lịch sử.
            content.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 280));

            content.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100));

            // HÀNG THAO TÁC
            TableLayoutPanel operationRow =
                new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 1,
                    Margin = new Padding(0, 0, 0, 12)
                };

            operationRow.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 50));

            operationRow.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 50));

            operationRow.Controls.Add(
                BuildOperationCard(true), 0, 0);

            operationRow.Controls.Add(
                BuildOperationCard(false), 1, 0);

            // HÀNG DANH SÁCH VÀ LỊCH SỬ
            TableLayoutPanel lowerRow =
                new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 1,
                    Margin = Padding.Empty
                };

            lowerRow.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 60));

            lowerRow.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 40));

            lowerRow.Controls.Add(
                BuildBackupListCard(), 0, 0);

            lowerRow.Controls.Add(
                BuildActivityCard(), 1, 0);

            content.Controls.Add(operationRow, 0, 0);
            content.Controls.Add(lowerRow, 0, 1);

            // FOOTER
            Panel footer = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(220, 237, 255),
                BorderColor = Color.FromArgb(220, 237, 255),
                CornerRadius = 8,
                Margin = new Padding(0, 6, 0, 0),
                Padding = new Padding(10, 0, 5, 0)
            };

            lblFooter = new Label
            {
                Dock = DockStyle.Fill,
                ForeColor = _darkBlue,
                Font = new Font("Segoe UI", 9F,
                    FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };

            footer.Controls.Add(lblFooter);

            root.Controls.Add(banner, 0, 0);
            root.Controls.Add(content, 0, 1);
            root.Controls.Add(footer, 0, 2);

            Controls.Add(root);

            ResumeLayout(true);
        }

        // =====================================================
        // 3. THẺ BACKUP / RESTORE
        // =====================================================

        // =====================================================
        // HỖ TRỢ VẼ BANNER
        // =====================================================
        private static GraphicsPath CreateBannerRoundPath(
            Rectangle rect, int radius)
        {
            int diameter = Math.Max(2, radius * 2);
            GraphicsPath path = new GraphicsPath();

            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y,
                diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter,
                diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter,
                diameter, diameter, 90, 90);
            path.CloseFigure();

            return path;
        }

        private static void DrawBannerWatermark(Graphics g, Size size)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            float centerX = size.Width - 85F;
            float centerY = size.Height / 2F;

            // Ngôi sao trung tâm mờ.
            using (SolidBrush starBrush = new SolidBrush(
                Color.FromArgb(30, 255, 255, 255)))
            {
                PointF[] star = new PointF[10];

                for (int i = 0; i < star.Length; i++)
                {
                    double angle = -Math.PI / 2 + i * Math.PI / 5;
                    float radius = i % 2 == 0 ? 30F : 13F;

                    star[i] = new PointF(
                        centerX + (float)Math.Cos(angle) * radius,
                        centerY + (float)Math.Sin(angle) * radius);
                }

                g.FillPolygon(starBrush, star);
            }

            // Vòng nguyệt quế hai bên ngôi sao.
            using (Pen leafPen = new Pen(
                Color.FromArgb(30, 255, 255, 255), 2.0F))
            {
                g.DrawArc(leafPen,
                    centerX - 53F, centerY - 37F,
                    106F, 82F, 125F, 105F);

                g.DrawArc(leafPen,
                    centerX - 53F, centerY - 37F,
                    106F, 82F, -50F, 105F);

                for (int i = 0; i < 7; i++)
                {
                    float y = centerY + 23F - i * 9F;
                    float leftX = centerX - 42F + i * 2F;
                    float rightX = centerX + 42F - i * 2F;

                    g.DrawEllipse(leafPen, leftX - 5F, y - 4F, 11F, 6F);
                    g.DrawEllipse(leafPen, rightX - 5F, y - 4F, 11F, 6F);
                }
            }
        }

        private Panel BuildOperationCard(bool backup)
        {
            RoundedPanel card = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderColor = Color.FromArgb(218, 230, 244),
                CornerRadius = 13,
                Margin = backup
                    ? new Padding(0, 0, 6, 0)
                    : new Padding(6, 0, 0, 0),
                Padding = new Padding(8)
            };

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.White
            };

            // Cố định chiều cao vùng nhập liệu để chú thích/cảnh báo chỉ
            // chiếm đúng phần cần thiết; khoảng trống còn lại chuyển xuống
            // vùng thao tác, giữ nút ở sát mép dưới của thẻ.
            layout.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 76));

            layout.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 117));

            layout.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100));

            // HEADER
            RoundedPanel header = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(215, 232, 255),
                CornerRadius = 9,
                BorderColor = Color.Transparent
            };

            Label icon = new Label
            {
                Text = string.Empty,
                Dock = DockStyle.Left,
                Width = backup ? 88 : 130,
                ForeColor = Color.FromArgb(0, 79, 166),
                Font = new Font("Segoe UI Symbol", 24F,
                    FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            if (backup)
            {
                // Vẽ biểu tượng cơ sở dữ liệu kèm mũi tên sao lưu.
                icon.Paint += (sender, e) =>
                {
                    Graphics g = e.Graphics;
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    using (Pen pen = new Pen(
                        Color.FromArgb(0, 79, 166), 2.8F))
                    {
                        Rectangle db = new Rectangle(18, 10, 36, 37);
                        g.DrawEllipse(pen, db.X, db.Y, db.Width, 12);
                        g.DrawLine(pen, db.Left, db.Y + 6, db.Left, db.Bottom - 6);
                        g.DrawLine(pen, db.Right, db.Y + 6, db.Right, db.Bottom - 6);
                        g.DrawArc(pen, db.X, db.Bottom - 12, db.Width, 12, 0, 180);
                        g.DrawArc(pen, db.X, db.Y + 13, db.Width, 12, 0, 180);
                        g.DrawLine(pen, 64, 43, 64, 18);
                        g.DrawLine(pen, 64, 18, 57, 26);
                        g.DrawLine(pen, 64, 18, 71, 26);
                        g.DrawLine(pen, 55, 43, 73, 43);
                    }
                };
            }
            else
            {
                // Biểu tượng cơ sở dữ liệu kèm mũi tên đi xuống,
                // theo đúng ngôn ngữ hình ảnh của bản thiết kế.
                icon.Paint += (sender, e) =>
                {
                    Graphics g = e.Graphics;
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    using (Pen pen = new Pen(
                        Color.FromArgb(0, 79, 166), 3.0F))
                    {
                        Rectangle db = new Rectangle(35, 15, 42, 48);
                        g.DrawEllipse(pen, db.X, db.Y, db.Width, 13);
                        g.DrawLine(pen, db.Left, db.Y + 6, db.Left, db.Bottom - 7);
                        g.DrawLine(pen, db.Right, db.Y + 6, db.Right, db.Bottom - 7);
                        g.DrawArc(pen, db.X, db.Bottom - 13, db.Width, 13, 0, 180);
                        g.DrawArc(pen, db.X, db.Y + 17, db.Width, 13, 0, 180);

                        // Mũi tên phục hồi nằm trong vòng tròn nhỏ.
                        g.DrawEllipse(pen, 67, 43, 29, 29);
                        g.DrawLine(pen, 81, 49, 81, 64);
                        g.DrawLine(pen, 81, 64, 75, 58);
                        g.DrawLine(pen, 81, 64, 87, 58);
                    }
                };
            }

            Panel headerText = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 7, 0, 4)
            };

            Label heading = new Label
            {
                Text = backup
                    ? "SAO LƯU DỮ LIỆU"
                    : "PHỤC HỒI DỮ LIỆU",
                Dock = DockStyle.Top,
                Height = backup ? 31 : 38,
                ForeColor = _darkBlue,
                Font = new Font("Segoe UI", backup ? 13.5F : 16F,
                    FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };

            Label subheading = new Label
            {
                Text = backup
                    ? "Tạo bản sao lưu toàn bộ cơ sở dữ liệu hệ thống"
                    : "Khôi phục hệ thống từ file sao lưu (.bak)",
                Dock = DockStyle.Fill,
                ForeColor = _darkBlue,
                Font = new Font("Segoe UI", backup ? 9F : 10F),
                TextAlign = ContentAlignment.MiddleLeft
            };

            headerText.Controls.Add(subheading);
            headerText.Controls.Add(heading);

            header.Controls.Add(headerText);
            header.Controls.Add(icon);

            // FIELDS
            Panel fields = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = Padding.Empty
            };

            if (backup)
                BuildBackupFields(fields);
            else
                BuildRestoreFields(fields);

            // ACTION
            Panel action = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            Button actionButton = CreateButton(
                string.Empty,
                backup
                    ? Color.FromArgb(25, 174, 79)
                    : Color.FromArgb(0, 99, 230),
                Color.White,
                true);
            actionButton.AccessibleName = backup ? "Sao lưu ngay" : "Phục hồi ngay";

            // Hai nút dùng cùng kích thước để đối xứng tuyệt đối.
            actionButton.Size = new Size(195, 42);
            actionButton.Paint += (sender, e) =>
                DrawDatabaseActionButtonContent(
                    e.Graphics, actionButton.ClientRectangle,
                    actionButton.Font, actionButton.ForeColor,
                    backup ? "Sao lưu ngay" : "Phục hồi ngay", backup);

            // Hai thẻ có cùng chiều cao vùng nhập liệu và cùng kích thước
            // nút, vì vậy hai nút luôn nằm cùng cao độ và cân đối nhau.
            actionButton.Anchor = AnchorStyles.Top;

            Action positionActionButton = () =>
            {
                int x = (action.ClientSize.Width - actionButton.Width) / 2;
                const int y = 4;

                actionButton.Location = new Point(x, y);
            };

            positionActionButton();
            action.Resize += (s, e) => positionActionButton();

            if (backup)
            {
                btnBackup = actionButton;
                btnBackup.Click += BtnBackup_Click;
            }
            else
            {
                btnRestore = actionButton;
                btnRestore.Click += BtnRestore_Click;
            }

            action.Controls.Add(actionButton);

            layout.Controls.Add(header, 0, 0);
            layout.Controls.Add(fields, 0, 1);
            layout.Controls.Add(action, 0, 2);

            card.Controls.Add(layout);

            return card;
        }

        // =====================================================
        // 4. BACKUP FIELDS
        // =====================================================

        private void BuildBackupFields(Panel parent)
        {
            // Cố định chiều cao vùng nhập liệu để thanh thông tin không bị
            // kéo giãn chiếm hết khoảng trống như giao diện hiện tại.
            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 100,
                ColumnCount = 3,
                RowCount = 3,
                BackColor = Color.White,
                Margin = Padding.Empty,
                Padding = Padding.Empty,
                GrowStyle = TableLayoutPanelGrowStyle.FixedSize
            };

            // Nhãn bên trái, ô nhập ở giữa và nút chọn thư mục bên phải.
            layout.ColumnStyles.Add(
                new ColumnStyle(SizeType.Absolute, 145));
            layout.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 100));
            layout.ColumnStyles.Add(
                new ColumnStyle(SizeType.Absolute, 112));

            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32));

            Label pathLabel = CreateFieldLabel(
                "Đường dẫn lưu trữ:");

            txtBackupDirectory = CreateTextBox();

            txtBackupDirectory.ReadOnly = true;

            btnChooseDirectory = CreateButton(
                string.Empty,
                Color.FromArgb(239, 247, 255),
                _primary,
                false);
            // Tự vẽ toàn bộ viền bo góc để tránh viền FlatStyle bị khuyết cạnh.
            btnChooseDirectory.FlatAppearance.BorderSize = 0;
            btnChooseDirectory.Paint += (sender, e) =>
                DrawRoundedButtonBorder(
                    e.Graphics, btnChooseDirectory.ClientRectangle,
                    Color.FromArgb(105, 169, 245), 1.2F, 7);
            btnChooseDirectory.AccessibleName = "Chọn thư mục lưu trữ";
            btnChooseDirectory.Dock = DockStyle.Fill;
            btnChooseDirectory.Font =
                new Font("Segoe UI", 9F, FontStyle.Bold);
            btnChooseDirectory.Paint += (sender, e) =>
                DrawFolderButtonContent(
                    e.Graphics, btnChooseDirectory.ClientRectangle,
                    btnChooseDirectory.Font, btnChooseDirectory.ForeColor, "Chọn");
            btnChooseDirectory.Margin = new Padding(3, 2, 3, 2);
            btnChooseDirectory.Click += BtnChooseDirectory_Click;

            Label fileLabel = CreateFieldLabel(
                "Tên file sao lưu:");

            txtBackupFileName = CreateTextBox();

            // Thanh thông tin được thay mới hoàn toàn: control tự vẽ nền,
            // viền, bốn góc bo tròn và nội dung chữ; không đặt Label con lên
            // trên nên không còn hiện tượng che hai góc dưới.
            lblBackupStatus = new RoundedStatusPanel
            {
                Dock = DockStyle.Fill,
                Text = "Sao lưu sẽ tạo một file .bak chứa toàn bộ dữ liệu của hệ thống.",
                BackColor = Color.FromArgb(225, 240, 255),
                ForeColor = _darkBlue,
                BorderColor = Color.FromArgb(190, 219, 250),
                CornerRadius = 9,
                Font = new Font("Segoe UI", 9F),
                TextPadding = new Padding(11, 0, 10, 0),
                Margin = new Padding(0, 1, 0, 1)
            };

            txtBackupDirectory.Margin = new Padding(3, 3, 5, 3);
            txtBackupFileName.Margin = new Padding(3, 3, 5, 3);
            txtBackupDirectory.Dock = DockStyle.Fill;
            txtBackupFileName.Dock = DockStyle.Fill;
            pathLabel.Padding = new Padding(2, 0, 0, 0);
            fileLabel.Padding = new Padding(2, 0, 0, 0);

            layout.Controls.Add(pathLabel, 0, 0);
            layout.Controls.Add(txtBackupDirectory, 1, 0);
            layout.Controls.Add(btnChooseDirectory, 2, 0);

            layout.Controls.Add(fileLabel, 0, 1);
            // Ô tên file có cùng chiều rộng với ô đường dẫn phía trên.
            layout.Controls.Add(txtBackupFileName, 1, 1);

            layout.SetColumnSpan(lblBackupStatus, 3);
            layout.Controls.Add(lblBackupStatus, 0, 2);

            parent.Controls.Add(layout);
        }

        // =====================================================
        // 5. RESTORE FIELDS
        // =====================================================

        private void BuildRestoreFields(Panel parent)
        {
            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 2,
                BackColor = Color.White,
                Margin = Padding.Empty,
                Padding = new Padding(0, 5, 0, 0)
            };

            // Tỷ lệ nhãn / ô nhập / nút chọn file theo bản thiết kế.
            layout.ColumnStyles.Add(
                new ColumnStyle(SizeType.Absolute, 155));
            layout.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 100));
            // Tăng vùng nút Chọn file để biểu tượng và chữ không bị cắt.
            layout.ColumnStyles.Add(
                new ColumnStyle(SizeType.Absolute, 180));

            // Thu gọn hàng chọn file để ô nhập thấp hơn, gọn hơn.
            layout.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 44));
            // Thu gọn chiều cao cảnh báo, vẫn đủ chỗ cho tiêu đề và hai dòng lưu ý.
            layout.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 64));

            Label fileLabel = CreateFieldLabel("Chọn file sao lưu:");
            fileLabel.Margin = new Padding(12, 3, 4, 3);

            txtRestoreFile = CreateTextBox();
            txtRestoreFile.ReadOnly = true;
            txtRestoreFile.Text = "Chưa chọn file...";
            txtRestoreFile.Font = new Font("Segoe UI", 10F);
            txtRestoreFile.TextAlignment = HorizontalAlignment.Left;
            txtRestoreFile.Margin = new Padding(4, 2, 8, 2);

            btnChooseRestoreFile = CreateButton(
                string.Empty,
                Color.FromArgb(239, 247, 255),
                _primary,
                false);
            btnChooseRestoreFile.AccessibleName = "Chọn file sao lưu";
            btnChooseRestoreFile.Dock = DockStyle.Fill;
            btnChooseRestoreFile.Margin = new Padding(4, 2, 10, 2);
            btnChooseRestoreFile.Font =
                new Font("Segoe UI", 10F, FontStyle.Bold);
            btnChooseRestoreFile.FlatAppearance.BorderSize = 0;
            btnChooseRestoreFile.Paint += (sender, e) =>
            {
                DrawRoundedButtonBorder(
                    e.Graphics,
                    btnChooseRestoreFile.ClientRectangle,
                    Color.FromArgb(105, 169, 245), 1.2F, 8);

                DrawFolderButtonContent(
                    e.Graphics,
                    btnChooseRestoreFile.ClientRectangle,
                    new Font("Segoe UI", 10F, FontStyle.Bold),
                    btnChooseRestoreFile.ForeColor,
                    "Chọn file");
            };
            btnChooseRestoreFile.Click += BtnChooseRestoreFile_Click;

            // Khung cảnh báo vàng nhạt, bo góc và có viền cam.
            RoundedPanel warning = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(255, 244, 218),
                BorderColor = Color.FromArgb(255, 220, 145),
                CornerRadius = 11,
                Margin = new Padding(8, 1, 8, 1),
                Padding = new Padding(9, 2, 8, 2)
            };

            Label warningIcon = new Label
            {
                Text = "⚠",
                Dock = DockStyle.Left,
                Width = 34,
                ForeColor = Color.FromArgb(245, 151, 0),
                Font = new Font("Segoe UI Symbol", 17F, FontStyle.Bold),
                TextAlign = ContentAlignment.TopCenter,
                Padding = new Padding(0, 0, 0, 0)
            };

            Panel warningContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = Padding.Empty
            };

            Label warningTitle = new Label
            {
                Text = "Lưu ý:",
                Dock = DockStyle.Top,
                Height = 20,
                ForeColor = Color.FromArgb(72, 52, 20),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };

            Label warningDetails = new Label
            {
                Text =
                    "• Việc phục hồi sẽ ghi đè toàn bộ dữ liệu hiện tại.\r\n" +
                    "• Hãy đảm bảo đã sao lưu dữ liệu trước khi phục hồi.",
                Dock = DockStyle.Fill,
                ForeColor = Color.FromArgb(72, 52, 20),
                Font = new Font("Segoe UI", 9F),
                TextAlign = ContentAlignment.TopLeft
            };

            warningContent.Controls.Add(warningDetails);
            warningContent.Controls.Add(warningTitle);
            warning.Controls.Add(warningContent);
            warning.Controls.Add(warningIcon);

            lblRestoreStatus = new Label
            {
                Visible = false
            };

            layout.Controls.Add(fileLabel, 0, 0);
            layout.Controls.Add(txtRestoreFile, 1, 0);
            layout.Controls.Add(btnChooseRestoreFile, 2, 0);

            layout.SetColumnSpan(warning, 3);
            layout.Controls.Add(warning, 0, 1);

            parent.Controls.Add(layout);
        }

        // =====================================================
        // 6. DANH SÁCH BACKUP
        // =====================================================

        private Panel BuildBackupListCard()
        {
            Panel card = CreateSectionCard();

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Padding = new Padding(8),
                BackColor = Color.White
            };

            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            // Header bo góc theo bản thiết kế.
            RoundedPanel header = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(228, 241, 255),
                BorderColor = Color.FromArgb(228, 241, 255),
                CornerRadius = 9,
                Padding = new Padding(8, 0, 7, 0)
            };

            Panel titleArea = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(0)
            };

            Label titleIcon = new Label
            {
                Dock = DockStyle.Left,
                Width = 28,
                Text = string.Empty,
                BackColor = Color.Transparent
            };
            titleIcon.Paint += (sender, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(Color.FromArgb(0, 76, 160), 1.8F))
                {
                    // Biểu tượng cơ sở dữ liệu nhỏ, nét mảnh giống mẫu.
                    Rectangle body = new Rectangle(5, 13, 13, 15);
                    e.Graphics.DrawEllipse(pen, body.X, body.Y, body.Width, 5);
                    e.Graphics.DrawLine(pen, body.Left, body.Y + 2, body.Left, body.Bottom - 2);
                    e.Graphics.DrawLine(pen, body.Right, body.Y + 2, body.Right, body.Bottom - 2);
                    e.Graphics.DrawArc(pen, body.X, body.Bottom - 5, body.Width, 5, 0, 180);
                    e.Graphics.DrawArc(pen, body.X, body.Y + 5, body.Width, 5, 0, 180);
                }
            };

            Label title = new Label
            {
                Text = "DANH SÁCH BẢN SAO LƯU",
                Dock = DockStyle.Fill,
                ForeColor = _darkBlue,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent
            };

            titleArea.Controls.Add(title);
            titleArea.Controls.Add(titleIcon);

            // Tăng vùng chứa và giảm khoảng cách giữa hai nút để không bị cắt nút Xóa.
            FlowLayoutPanel buttons = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                Width = 230,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 1, 1, 1),
                Margin = Padding.Empty
            };

            btnRefresh = CreateButton(
                "↻  Làm mới",
                Color.White,
                _primary,
                false);
            btnRefresh.Size = new Size(112, 32);
            btnRefresh.Margin = new Padding(2, 2, 4, 2);
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(135, 184, 245);
            btnRefresh.Paint += (sender, e) =>
                DrawRoundedButtonBorder(
                    e.Graphics, btnRefresh.ClientRectangle,
                    Color.FromArgb(135, 184, 245), 1.2F, 7);
            btnRefresh.Click += (s, e) =>
            {
                RefreshBackupList();
                RefreshActivityList();
            };

            btnDelete = CreateButton(
                "Xóa",
                Color.White,
                Color.FromArgb(220, 53, 69),
                false);
            btnDelete.Size = new Size(88, 32);
            btnDelete.Margin = new Padding(2, 2, 2, 2);
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatAppearance.BorderColor = Color.FromArgb(238, 150, 160);
            btnDelete.Text = string.Empty;
            btnDelete.Paint += (sender, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                DrawRoundedButtonBorder(
                    g, btnDelete.ClientRectangle,
                    Color.FromArgb(238, 150, 160), 1.2F, 7);

                // Vẽ biểu tượng thùng rác và chữ Xóa cùng nhau.
                float iconX = 19F;
                float centerY = btnDelete.ClientSize.Height / 2F;
                using (Pen pen = new Pen(Color.FromArgb(220, 53, 69), 1.6F))
                {
                    g.DrawLine(pen, iconX - 7, centerY - 7, iconX + 7, centerY - 7);
                    g.DrawLine(pen, iconX - 3, centerY - 10, iconX + 3, centerY - 10);
                    g.DrawLine(pen, iconX - 5, centerY - 6, iconX - 4, centerY + 8);
                    g.DrawLine(pen, iconX + 5, centerY - 6, iconX + 4, centerY + 8);
                    g.DrawLine(pen, iconX - 4, centerY + 8, iconX + 4, centerY + 8);
                    g.DrawLine(pen, iconX - 1, centerY - 3, iconX - 1, centerY + 5);
                    g.DrawLine(pen, iconX + 2, centerY - 3, iconX + 2, centerY + 5);
                }

                Rectangle textBounds = new Rectangle(32, 0,
                    Math.Max(0, btnDelete.ClientSize.Width - 36),
                    btnDelete.ClientSize.Height);
                TextRenderer.DrawText(
                    g, "Xóa", btnDelete.Font, textBounds,
                    Color.FromArgb(220, 53, 69),
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
                    TextFormatFlags.SingleLine | TextFormatFlags.NoPadding);
            };
            btnDelete.Click += BtnDelete_Click;

            buttons.Controls.Add(btnRefresh);
            buttons.Controls.Add(btnDelete);
            header.Controls.Add(titleArea);
            header.Controls.Add(buttons);

            dgvBackups = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                EnableHeadersVisualStyles = false,
                Font = new Font("Segoe UI", 9F),
                GridColor = Color.FromArgb(226, 235, 245),
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersHeight = 34,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing
            };

            dgvBackups.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 76, 160);
            dgvBackups.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvBackups.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvBackups.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvBackups.ColumnHeadersDefaultCellStyle.Padding = new Padding(5, 0, 3, 0);
            dgvBackups.DefaultCellStyle.SelectionBackColor = Color.FromArgb(205, 235, 255);
            dgvBackups.DefaultCellStyle.SelectionForeColor = _darkBlue;
            dgvBackups.DefaultCellStyle.Padding = new Padding(3, 0, 3, 0);
            dgvBackups.RowTemplate.Height = 36;

            dgvBackups.Columns.Add("colSTT", "STT");
            dgvBackups.Columns.Add("colName", "Tên file");
            dgvBackups.Columns.Add("colTime", "Thời gian sao lưu");
            dgvBackups.Columns.Add("colSize", "Kích thước");
            dgvBackups.Columns.Add("colPath", "Đường dẫn");

            dgvBackups.Columns["colSTT"]!.FillWeight = 8;
            dgvBackups.Columns["colName"]!.FillWeight = 22;
            dgvBackups.Columns["colTime"]!.FillWeight = 18;
            dgvBackups.Columns["colSize"]!.FillWeight = 15;
            dgvBackups.Columns["colPath"]!.FillWeight = 29;

            DataGridViewButtonColumn openColumn = new DataGridViewButtonColumn
            {
                Name = "colOpen",
                HeaderText = "",
                Text = string.Empty,
                UseColumnTextForButtonValue = false,
                FillWeight = 8,
                FlatStyle = FlatStyle.Flat,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.White,
                    ForeColor = _primary,
                    SelectionBackColor = Color.FromArgb(205, 235, 255),
                    SelectionForeColor = _primary,
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Padding = new Padding(4, 3, 4, 3)
                }
            };
            dgvBackups.Columns.Add(openColumn);

            // Khung ngoài bo góc; DataGridView được cắt theo Region bo góc riêng.
            RoundedPanel gridFrame = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderColor = Color.FromArgb(190, 213, 240),
                CornerRadius = 9,
                Padding = new Padding(1),
                Margin = new Padding(0, 2, 0, 0)
            };
            gridFrame.Controls.Add(dgvBackups);

            dgvBackups.HandleCreated += (sender, e) => ApplyRoundedControlRegion(dgvBackups, 8);
            dgvBackups.Resize += (sender, e) => ApplyRoundedControlRegion(dgvBackups, 8);

            dgvBackups.CellPainting += (sender, e) =>
            {
                // Vẽ riêng hai ô Header ngoài cùng để bo góc trên trái/phải.
                if (e.RowIndex == -1 &&
                    (e.ColumnIndex == 0 ||
                     e.ColumnIndex == dgvBackups.Columns.Count - 1))
                {
                    bool roundLeft = e.ColumnIndex == 0;
                    bool roundRight = e.ColumnIndex == dgvBackups.Columns.Count - 1;
                    Rectangle headerBounds = e.CellBounds;

                    using (GraphicsPath headerPath = CreateHeaderTopRoundedPath(
                        headerBounds, 8, roundLeft, roundRight))
                    using (SolidBrush headerBrush = new SolidBrush(
                        dgvBackups.ColumnHeadersDefaultCellStyle.BackColor))
                    {
                        e.Graphics!.SmoothingMode = SmoothingMode.AntiAlias;
                        e.Graphics.FillPath(headerBrush, headerPath);
                    }

                    string headerText =
                        Convert.ToString(e.FormattedValue) ?? string.Empty;

                    Rectangle textBounds = Rectangle.Inflate(
                        headerBounds, -6, 0);

                    TextRenderer.DrawText(
                        e.Graphics!,
                        headerText,
                        e.CellStyle?.Font ?? dgvBackups.Font,
                        textBounds,
                        e.CellStyle?.ForeColor ?? dgvBackups.ForeColor,
                        TextFormatFlags.Left |
                        TextFormatFlags.VerticalCenter |
                        TextFormatFlags.EndEllipsis |
                        TextFormatFlags.SingleLine);

                    using (Pen separator = new Pen(Color.White, 1F))
                        e.Graphics.DrawLine(separator,
                            headerBounds.Right - 1, headerBounds.Top + 1,
                            headerBounds.Right - 1, headerBounds.Bottom - 1);

                    e.Handled = true;
                    return;
                }

                if (e.RowIndex < 0 || e.ColumnIndex < 0 ||
                    dgvBackups.Columns[e.ColumnIndex].Name != "colOpen")
                    return;

                e.PaintBackground(e.CellBounds, true);

                Rectangle buttonBounds = new Rectangle(
                    e.CellBounds.X + Math.Max(3, (e.CellBounds.Width - 31) / 2),
                    e.CellBounds.Y + Math.Max(3, (e.CellBounds.Height - 25) / 2),
                    Math.Min(31, e.CellBounds.Width - 6),
                    Math.Min(25, e.CellBounds.Height - 6));

                bool isFolderHovered = e.RowIndex == _hoverOpenRowIndex;
                Color folderBackColor = isFolderHovered
                    ? Color.FromArgb(226, 241, 255)
                    : Color.White;
                Color folderBorderColor = isFolderHovered
                    ? _primary
                    : Color.FromArgb(100, 165, 235);

                using (GraphicsPath path = CreateControlRoundPath(buttonBounds, 6))
                using (SolidBrush fill = new SolidBrush(folderBackColor))
                using (Pen border = new Pen(folderBorderColor, isFolderHovered ? 1.5F : 1F))
                {
                    e.Graphics!.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(fill, path);
                    e.Graphics.DrawPath(border, path);
                }

                // Folder icon đậm hơn khi rê chuột vào nút.
                float x = buttonBounds.X + (buttonBounds.Width - 17) / 2F;
                float y = buttonBounds.Y + (buttonBounds.Height - 13) / 2F;
                using (Pen folderPen = new Pen(
                    isFolderHovered ? Color.FromArgb(0, 76, 160) : _primary,
                    isFolderHovered ? 1.65F : 1.35F))
                {
                    e.Graphics.DrawLine(folderPen, x, y + 3, x + 6, y + 3);
                    e.Graphics.DrawLine(folderPen, x + 6, y + 3, x + 8, y + 5);
                    e.Graphics.DrawLine(folderPen, x + 8, y + 5, x + 17, y + 5);
                    e.Graphics.DrawRectangle(folderPen, x, y + 4, 17, 9);
                }

                e.Handled = true;
            };

            // Theo dõi đúng vùng nút Folder (không kích hoạt hover cho cả ô).
            dgvBackups.MouseMove += (sender, e) =>
            {
                DataGridView.HitTestInfo hit = dgvBackups.HitTest(e.X, e.Y);
                int nextHoverRow = -1;

                if (hit.RowIndex >= 0 && hit.ColumnIndex >= 0 &&
                    dgvBackups.Columns[hit.ColumnIndex].Name == "colOpen")
                {
                    Rectangle cellBounds = dgvBackups.GetCellDisplayRectangle(
                        hit.ColumnIndex, hit.RowIndex, false);

                    Rectangle folderBounds = new Rectangle(
                        cellBounds.X + Math.Max(3, (cellBounds.Width - 31) / 2),
                        cellBounds.Y + Math.Max(3, (cellBounds.Height - 25) / 2),
                        Math.Min(31, cellBounds.Width - 6),
                        Math.Min(25, cellBounds.Height - 6));

                    if (folderBounds.Contains(e.Location))
                        nextHoverRow = hit.RowIndex;
                }

                if (_hoverOpenRowIndex != nextHoverRow)
                {
                    int previousRow = _hoverOpenRowIndex;
                    _hoverOpenRowIndex = nextHoverRow;

                    if (previousRow >= 0 && previousRow < dgvBackups.Rows.Count)
                        dgvBackups.InvalidateCell(
                            dgvBackups.Columns["colOpen"]!.Index, previousRow);

                    if (_hoverOpenRowIndex >= 0)
                        dgvBackups.InvalidateCell(
                            dgvBackups.Columns["colOpen"]!.Index, _hoverOpenRowIndex);
                }
            };

            dgvBackups.MouseLeave += (sender, e) =>
            {
                if (_hoverOpenRowIndex >= 0)
                {
                    int previousRow = _hoverOpenRowIndex;
                    _hoverOpenRowIndex = -1;
                    if (previousRow < dgvBackups.Rows.Count)
                        dgvBackups.InvalidateCell(
                            dgvBackups.Columns["colOpen"]!.Index, previousRow);
                }
            };

            dgvBackups.CellDoubleClick += DgvBackups_CellDoubleClick;
            dgvBackups.CellClick += (sender, e) =>
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
                    dgvBackups.Columns[e.ColumnIndex].Name == "colOpen")
                {
                    DgvBackups_CellDoubleClick(sender,
                        new DataGridViewCellEventArgs(e.ColumnIndex, e.RowIndex));
                }
            };

            layout.Controls.Add(header, 0, 0);
            layout.Controls.Add(gridFrame, 0, 1);
            card.Controls.Add(layout);
            return card;
        }

        // =====================================================
        // 7. LỊCH SỬ THAO TÁC
        // =====================================================

        private Panel BuildActivityCard()
        {
            Panel card = CreateSectionCard();

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Padding = new Padding(8)
            };

            layout.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 42));

            layout.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100));

            RoundedPanel title = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(228, 241, 255),
                BorderColor = Color.FromArgb(228, 241, 255),
                CornerRadius = 9,
                Padding = new Padding(7, 0, 6, 0)
            };

            Label titleIcon = new Label
            {
                Dock = DockStyle.Left,
                Width = 27,
                Text = string.Empty,
                BackColor = Color.Transparent
            };
            titleIcon.Paint += (sender, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(Color.FromArgb(0, 76, 160), 1.8F))
                {
                    // Clipboard: tờ giấy có kẹp phía trên và các dòng nội dung.
                    Rectangle paper = new Rectangle(7, 10, 13, 18);
                    g.DrawRectangle(pen, paper);
                    g.DrawLine(pen, 10, 8, 17, 8);
                    g.DrawLine(pen, 11, 6, 16, 6);
                    g.DrawLine(pen, 10, 15, 17, 15);
                    g.DrawLine(pen, 10, 19, 17, 19);
                    g.DrawLine(pen, 10, 23, 16, 23);
                }
            };

            Label titleText = new Label
            {
                Text = "LỊCH SỬ THAO TÁC",
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ForeColor = _darkBlue,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };
            title.Controls.Add(titleText);
            title.Controls.Add(titleIcon);

            lvActivity = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = false,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 9F),
                HideSelection = false,
                OwnerDraw = true,
                Scrollable = true,
                SmallImageList = new ImageList
                {
                    ImageSize = new Size(1, 58),
                    ColorDepth = ColorDepth.Depth32Bit
                }
            };

            lvActivity.DrawColumnHeader += (sender, e) =>
            {
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(228, 241, 255)))
                    e.Graphics.FillRectangle(brush, e.Bounds);
                using (Font headerFont = new Font("Segoe UI", 9F, FontStyle.Bold))
                    TextRenderer.DrawText(e.Graphics, e.Header?.Text ?? string.Empty,
                        headerFont, e.Bounds, _darkBlue,
                        TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
            };
            lvActivity.DrawItem += (sender, e) => e.DrawDefault = false;
            lvActivity.DrawSubItem += (sender, e) =>
            {
                if (e.Item == null)
                    return;

                ListViewItem item = e.Item;

                e.DrawBackground();
                if (e.ColumnIndex == 0)
                {
                    string status = Convert.ToString(e.Item?.Tag) ?? string.Empty;
                    bool success = status.IndexOf("thành công", StringComparison.OrdinalIgnoreCase) >= 0;
                    bool failed = status.IndexOf("thất bại", StringComparison.OrdinalIgnoreCase) >= 0;
                    bool isRestore = item.Text.IndexOf("phục hồi", StringComparison.OrdinalIgnoreCase) >= 0;

                    Color statusColor;
                    string symbol;
                    if (failed)
                    {
                        statusColor = Color.FromArgb(220, 53, 69);
                        symbol = "×";
                    }
                    else if (success && isRestore)
                    {
                        // Phục hồi thành công: vòng tròn xanh dương, chữ i như mẫu.
                        statusColor = Color.FromArgb(30, 157, 218);
                        symbol = "i";
                    }
                    else if (success)
                    {
                        // Sao lưu thành công: vòng tròn xanh lá, dấu kiểm.
                        statusColor = Color.FromArgb(30, 174, 91);
                        symbol = "✓";
                    }
                    else
                    {
                        statusColor = Color.FromArgb(30, 157, 218);
                        symbol = "i";
                    }

                    Rectangle badge = new Rectangle(e.Bounds.X + 9,
                        e.Bounds.Y + 12, 29, 29);
                    using (SolidBrush iconBrush = new SolidBrush(statusColor))
                        e.Graphics.FillEllipse(iconBrush, badge);
                    using (Font symbolFont = new Font("Segoe UI", 14F, FontStyle.Bold))
                        TextRenderer.DrawText(e.Graphics, symbol, symbolFont,
                            badge, Color.White, TextFormatFlags.HorizontalCenter |
                            TextFormatFlags.VerticalCenter);

                    string displayTitle = item.Text ?? string.Empty;
                    if (!string.IsNullOrWhiteSpace(status) &&
                        displayTitle.IndexOf(status, StringComparison.OrdinalIgnoreCase) < 0)
                        displayTitle += " " + status.ToLowerInvariant();

                    Rectangle titleBounds = new Rectangle(
                        e.Bounds.X + 47, e.Bounds.Y + 7,
                        Math.Max(20, e.Bounds.Width - 52), 23);
                    Rectangle detailBounds = new Rectangle(
                        e.Bounds.X + 47, e.Bounds.Y + 29,
                        Math.Max(20, e.Bounds.Width - 52), 20);
                    using (Font titleFont = new Font("Segoe UI", 9F, FontStyle.Bold))
                        TextRenderer.DrawText(
                            e.Graphics,
                            displayTitle,
                            titleFont,
                            titleBounds,
                            Color.FromArgb(25, 45, 70),
                            TextFormatFlags.VerticalCenter |
                            TextFormatFlags.EndEllipsis);

                    string detail = item.SubItems != null &&
                                    item.SubItems.Count > 2
                        ? item.SubItems[2].Text ?? string.Empty
                        : string.Empty;
                    using (Font detailFont = new Font("Segoe UI", 8F))
                        TextRenderer.DrawText(e.Graphics, detail, detailFont,
                            detailBounds, Color.FromArgb(85, 105, 125),
                            TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
                }
                else
                {
                    Rectangle dateBounds = new Rectangle(e.Bounds.X + 2,
                        e.Bounds.Y + 8, e.Bounds.Width - 8, 22);
                    using (Font dateFont = new Font("Segoe UI", 8.5F))
                        TextRenderer.DrawText(
                            e.Graphics,
                            e.SubItem?.Text ?? string.Empty,
                            dateFont,
                            dateBounds,
                            Color.FromArgb(35, 50, 70),
                            TextFormatFlags.VerticalCenter |
                            TextFormatFlags.Right |
                            TextFormatFlags.EndEllipsis);
                }
            };

            lvActivity.Columns.Add("Nội dung thao tác", 300);
            lvActivity.Columns.Add("Thời gian", 130);
            layout.Controls.Add(title, 0, 0);
            layout.Controls.Add(lvActivity, 0, 1);

            card.Controls.Add(layout);

            return card;
        }

        // =====================================================
        // 8. HÀM TẠO CONTROL
        // =====================================================

        private Panel CreateSectionCard()
        {
            return new RoundedPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderColor = Color.FromArgb(218, 230, 244),
                CornerRadius = 12,
                Margin = new Padding(4),
                Padding = Padding.Empty
            };
        }

        private Label CreateFieldLabel(string text)
        {
            return new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                ForeColor = _darkBlue,
                Font = new Font("Segoe UI", 9F),
                TextAlign = ContentAlignment.MiddleLeft
            };
        }

        private RoundedTextBox CreateTextBox()
        {
            return new RoundedTextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.5F),
                Margin = new Padding(3, 4, 5, 4),
                CornerRadius = 7
            };
        }

        private Button CreateButton(
            string text,
            Color backColor,
            Color foreColor,
            bool prominent)
        {
            Button button = new Button
            {
                Text = text,
                BackColor = backColor,
                ForeColor = foreColor,
                FlatStyle = FlatStyle.Flat,
                Font = new Font(
                    "Segoe UI",
                    prominent ? 10.5F : 9F,
                    FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(3),
                UseVisualStyleBackColor = false
            };

            button.FlatAppearance.BorderSize =
                prominent ? 0 : 1;

            button.FlatAppearance.BorderColor =
                Color.FromArgb(135, 184, 245);
            button.Padding = new Padding(4, 0, 4, 0);
            button.Resize += (sender, e) => ApplyRoundedButtonRegion(button);
            button.HandleCreated += (sender, e) => ApplyRoundedButtonRegion(button);

            return button;
        }

        // =====================================================
        // 9. CHỌN THƯ MỤC BACKUP
        // =====================================================

        private void BtnChooseDirectory_Click(
            object? sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog =
                new FolderBrowserDialog())
            {
                dialog.Description =
                    "Chọn thư mục lưu file sao lưu dữ liệu.";

                dialog.ShowNewFolderButton = true;

                string current =
                    txtBackupDirectory.Text.Trim();

                if (Directory.Exists(current))
                    dialog.SelectedPath = current;

                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                txtBackupDirectory.Text = dialog.SelectedPath;

                BackupRestoreHelper.SaveBackupDirectory(
                    dialog.SelectedPath);

                UpdateFooter();
                RefreshBackupList();
            }
        }

        // =====================================================
        // 10. CHỌN FILE RESTORE
        // =====================================================

        private void BtnChooseRestoreFile_Click(
            object? sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Chọn file sao lưu dữ liệu";

                dialog.Filter =
                    "SQL Server Backup (*.bak)|*.bak";

                dialog.Multiselect = false;

                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                txtRestoreFile.Text = dialog.FileName;

                lblRestoreStatus.Text =
                    "Đã chọn: " + Path.GetFileName(dialog.FileName);

                lblRestoreStatus.Visible = true;
            }
        }

        // =====================================================
        // 11. BACKUP
        // =====================================================

        private async void BtnBackup_Click(
            object? sender, EventArgs e)
        {
            if (_busy)
                return;

            string directory =
                txtBackupDirectory.Text.Trim();

            if (string.IsNullOrWhiteSpace(directory))
            {
                FrmLoginMessage.ShowMessage(this,
                    LoginMessageType.Information,
                    "Chưa chọn thư mục",
                    "Vui lòng chọn thư mục lưu trữ trước khi sao lưu.");

                BtnChooseDirectory_Click(sender, e);
                return;
            }

            if (!Directory.Exists(directory))
            {
                FrmLoginMessage.ShowMessage(this,
                    LoginMessageType.Information,
                    "Thư mục không hợp lệ",
                    "Thư mục lưu trữ không tồn tại. Vui lòng chọn lại.");

                return;
            }

            string fileName =
                BackupRestoreHelper.NormalizeBackupFileName(
                    txtBackupFileName.Text);

            if (!BackupRestoreHelper.IsValidBackupFileName(fileName))
            {
                FrmLoginMessage.ShowMessage(this,
                    LoginMessageType.Information,
                    "Tên file không hợp lệ",
                    "Tên file sao lưu không hợp lệ.");

                return;
            }

            string backupPath =
                Path.Combine(directory, fileName);

            if (File.Exists(backupPath))
            {
                DialogResult overwrite = FrmLoginMessage.ShowMessage(this,
                    LoginMessageType.Information,
                    "Xác nhận ghi đè",
                    "File sao lưu đã tồn tại. Bạn có muốn ghi đè file này không?",
                    MessageBoxButtons.YesNo);

                if (overwrite != DialogResult.Yes)
                    return;
            }

            SetBusy(true);

            lblBackupStatus.Text = "●  Đang sao lưu dữ liệu...";
            lblBackupStatus.ForeColor = _primary;

            try
            {
                await Task.Run(() =>
                {
                    _backupService.BackupDatabase(backupPath);
                });

                _backupService.WriteHistory(
                    "Sao lưu dữ liệu",
                    Path.GetFileName(backupPath),
                    true);

                lblBackupStatus.Text =
                    "✓  Sao lưu thành công: " +
                    Path.GetFileName(backupPath);

                lblBackupStatus.ForeColor =
                    Color.FromArgb(0, 130, 65);

                txtBackupFileName.Text =
                    BackupRestoreHelper.GenerateBackupFileName();

                RefreshBackupList();
                RefreshActivityList();

                FrmLoginMessage.ShowMessage(this,
                    LoginMessageType.Success,
                    "Sao lưu thành công",
                    "Sao lưu toàn bộ dữ liệu thành công.\r\n" +
                    Path.GetFileName(backupPath));
            }
            catch (Exception ex)
            {
                _backupService.WriteHistory(
                    "Sao lưu dữ liệu",
                    ex.Message,
                    false);

                lblBackupStatus.Text =
                    "✕  Sao lưu thất bại: " + ex.Message;

                lblBackupStatus.ForeColor = Color.Firebrick;

                RefreshActivityList();

                FrmLoginMessage.ShowMessage(this,
                    LoginMessageType.InvalidCredentials,
                    "Lỗi sao lưu",
                    "Không thể sao lưu dữ liệu.\r\n\r\n" + ex.Message);
            }
            finally
            {
                SetBusy(false);
            }
        }

        // =====================================================
        // 12. RESTORE
        // =====================================================

        private async void BtnRestore_Click(
            object? sender, EventArgs e)
        {
            if (_busy)
                return;

            string backupPath =
                txtRestoreFile.Text.Trim();

            if (!BackupRestoreHelper.IsValidBackupFile(backupPath))
            {
                FrmLoginMessage.ShowMessage(this,
                    LoginMessageType.Information,
                    "Chưa chọn file",
                    "Vui lòng chọn một file sao lưu .bak hợp lệ.");

                return;
            }

            DialogResult confirm = FrmLoginMessage.ShowMessage(this,
                LoginMessageType.Information,
                "Xác nhận phục hồi dữ liệu",
                "Dữ liệu hiện tại trong database " +
                BackupRestoreHelper.DatabaseName +
                " sẽ bị thay thế hoàn toàn bằng dữ liệu trong file backup.\r\n\r\n" +
                "Hãy sao lưu dữ liệu trước khi tiếp tục.\r\n\r\n" +
                "Bạn có chắc chắn muốn phục hồi không?",
                MessageBoxButtons.YesNo);

            if (confirm != DialogResult.Yes)
                return;

            SetBusy(true);

            lblRestoreStatus.Text =
                "Đang kiểm tra và phục hồi dữ liệu...";

            lblRestoreStatus.Visible = true;

            try
            {
                await Task.Run(() =>
                {
                    _backupService.RestoreDatabase(backupPath);
                });

                _backupService.WriteHistory(
                    "Phục hồi dữ liệu",
                    Path.GetFileName(backupPath),
                    true);

                lblRestoreStatus.Text =
                    "✓  Phục hồi thành công: " +
                    Path.GetFileName(backupPath);

                RefreshBackupList();
                RefreshActivityList();

                FrmLoginMessage.ShowMessage(this,
                    LoginMessageType.Success,
                    "Phục hồi thành công",
                    "Phục hồi cơ sở dữ liệu thành công.\r\n" +
                    "Vui lòng khởi động lại ứng dụng để tải lại dữ liệu.");
            }
            catch (Exception ex)
            {
                _backupService.WriteHistory(
                    "Phục hồi dữ liệu",
                    ex.Message,
                    false);

                lblRestoreStatus.Text =
                    "✕  Phục hồi thất bại: " + ex.Message;

                RefreshActivityList();

                FrmLoginMessage.ShowMessage(this,
                    LoginMessageType.InvalidCredentials,
                    "Lỗi phục hồi",
                    "Không thể phục hồi dữ liệu.\r\n\r\n" + ex.Message);
            }
            finally
            {
                SetBusy(false);
            }
        }

        // =====================================================
        // 13. XÓA FILE BACKUP
        // =====================================================

        private void BtnDelete_Click(
            object? sender, EventArgs e)
        {
            if (dgvBackups.CurrentRow == null)
            {
                FrmLoginMessage.ShowMessage(this,
                    LoginMessageType.Information,
                    "Chưa chọn bản sao lưu",
                    "Vui lòng chọn bản sao lưu cần xóa.");

                return;
            }

            string path =
                Convert.ToString(
                    dgvBackups.CurrentRow.Tag) ?? string.Empty;

            if (!BackupRestoreHelper.IsValidBackupFile(path))
                return;

            DialogResult confirm = FrmLoginMessage.ShowMessage(this,
                LoginMessageType.Information,
                "Xác nhận xóa",
                "Bạn có chắc chắn muốn xóa file sao lưu sau không?\r\n" +
                Path.GetFileName(path),
                MessageBoxButtons.YesNo);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                File.Delete(path);

                _backupService.WriteHistory(
                    "Xóa bản sao lưu",
                    Path.GetFileName(path),
                    true);

                RefreshBackupList();
                RefreshActivityList();
            }
            catch (Exception ex)
            {
                _backupService.WriteHistory(
                    "Xóa bản sao lưu",
                    ex.Message,
                    false);

                FrmLoginMessage.ShowMessage(this,
                    LoginMessageType.InvalidCredentials,
                    "Lỗi xóa file",
                    "Không thể xóa file sao lưu.\r\n\r\n" + ex.Message);
            }
        }

        // =====================================================
        // 14. MỞ VỊ TRÍ FILE BACKUP
        // =====================================================

        private void DgvBackups_CellDoubleClick(
            object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            string path =
                Convert.ToString(
                    dgvBackups.Rows[e.RowIndex].Tag) ?? string.Empty;

            if (string.IsNullOrWhiteSpace(path) ||
                !File.Exists(path))
                return;

            try
            {
                System.Diagnostics.Process.Start(
                    new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "explorer.exe",
                        Arguments = "/select,\"" + path + "\"",
                        UseShellExecute = true
                    });
            }
            catch (Exception ex)
            {
                FrmLoginMessage.ShowMessage(this,
                    LoginMessageType.Information,
                    "Không thể mở thư mục",
                    ex.Message);
            }
        }

        // =====================================================
        // 15. REFRESH DANH SÁCH BACKUP
        // =====================================================

        private void RefreshBackupList()
        {
            if (dgvBackups == null)
                return;

            dgvBackups.Rows.Clear();

            string directory =
                txtBackupDirectory.Text.Trim();

            try
            {
                var backups =
                    _backupService.GetBackupList(directory);

                foreach (BackupInfoDTO item in backups)
                {
                    int row = dgvBackups.Rows.Add(
                        item.STT,
                        item.FileName,
                        item.BackupTime.ToString(
                            "dd/MM/yyyy HH:mm"),
                        item.FileSizeText,
                        item.DirectoryPath,
                        "📁");

                    dgvBackups.Rows[row].Tag = item.FullPath;
                }
            }
            catch (Exception ex)
            {
                lblFooter.Text =
                    "Không thể tải danh sách bản sao lưu: " +
                    ex.Message;
            }

            UpdateFooter();
        }

        // =====================================================
        // 16. REFRESH LỊCH SỬ
        // =====================================================

        private void RefreshActivityList()
        {
            if (lvActivity == null)
                return;

            lvActivity.BeginUpdate();

            lvActivity.Items.Clear();

            try
            {
                var history = _backupService.GetHistory();

                foreach (BackupHistoryDTO item in history)
                {
                    ListViewItem row = new ListViewItem(item.ActionName);
                    row.Tag = item.Status;
                    row.SubItems.Add(item.ActionTime.ToString("dd/MM/yyyy HH:mm"));
                    row.SubItems.Add(item.Detail);
                    lvActivity.Items.Add(row);
                }
            }
            catch
            {
                // Không làm gián đoạn giao diện.
            }

            lvActivity.EndUpdate();
        }

        // =====================================================
        // 17. FOOTER
        // =====================================================

        private void UpdateFooter()
        {
            string directory =
                txtBackupDirectory.Text.Trim();

            if (string.IsNullOrWhiteSpace(directory))
            {
                lblFooter.Text =
                    "ℹ  Chưa chọn thư mục lưu trữ. " +
                    "Hãy chọn thư mục trước khi sao lưu.";

                return;
            }

            int count = 0;

            if (Directory.Exists(directory))
            {
                count = _backupService
                    .GetBackupList(directory).Count;
            }

            lblFooter.Text =
                "ⓘ  Thư mục mặc định:  " + directory +
                "     |     Tổng số bản sao lưu: " +
                count.ToString("N0");
        }

        // =====================================================
        // 18. TRẠNG THÁI BUSY
        // =====================================================

        private void SetBusy(bool busy)
        {
            _busy = busy;

            btnBackup.Enabled = !busy;
            btnRestore.Enabled = !busy;
            btnChooseDirectory.Enabled = !busy;
            btnChooseRestoreFile.Enabled = !busy;
            btnRefresh.Enabled = !busy;
            btnDelete.Enabled = !busy;

            Cursor = busy
                ? Cursors.WaitCursor
                : Cursors.Default;
        }

        // =====================================================
        // 19. RESIZE
        // =====================================================

        private static void ApplyRoundedControlRegion(Control control, int radius)
        {
            if (control == null || control.Width < 2 || control.Height < 2)
                return;

            using (GraphicsPath path = CreateControlRoundPath(
                new Rectangle(0, 0, control.Width, control.Height), radius))
            {
                Region? oldRegion = control.Region;
                control.Region = new Region(path);
                oldRegion?.Dispose();
            }
        }

        private static void ApplyRoundedButtonRegion(Button button)
        {
            if (button.Width < 2 || button.Height < 2) return;
            using (GraphicsPath path = CreateControlRoundPath(
                new Rectangle(0, 0, button.Width, button.Height), 8))
            {
                Region? oldRegion = button.Region;
                button.Region = new Region(path);
                oldRegion?.Dispose();
            }
        }

        private static GraphicsPath CreateHeaderTopRoundedPath(
            Rectangle rect, int radius, bool roundLeft, bool roundRight)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = Math.Max(2, Math.Min(radius * 2, rect.Height * 2));
            float left = rect.Left;
            float top = rect.Top;
            float right = rect.Right;
            float bottom = rect.Bottom;

            path.StartFigure();
            path.AddLine(
                left + (roundLeft ? diameter / 2F : 0F), top,
                right - (roundRight ? diameter / 2F : 0F), top);

            if (roundRight)
                path.AddArc(right - diameter, top, diameter, diameter, 270F, 90F);
            else
                path.AddLine(right, top, right, top + diameter / 2F);

            path.AddLine(right, top + (roundRight ? diameter / 2F : 0F), right, bottom);
            path.AddLine(right, bottom, left, bottom);

            if (roundLeft)
                path.AddArc(left, top, diameter, diameter, 180F, 90F);
            else
                path.AddLine(left, bottom, left, top);

            path.CloseFigure();
            return path;
        }

        private static GraphicsPath CreateControlRoundPath(Rectangle rect, int radius)
        {
            int d = Math.Min(Math.Max(2, radius * 2), Math.Min(rect.Width, rect.Height));
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static void DrawRoundedButtonBorder(
            Graphics graphics, Rectangle bounds, Color borderColor,
            float borderWidth, int radius)
        {
            if (bounds.Width < 4 || bounds.Height < 4)
                return;

            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // Lùi nét vẽ vào trong 1,5 px để chống hiện tượng nét viền
            // bị Region/ClientRectangle cắt mất ở cạnh trên, dưới và hai bên.
            const float inset = 1.5F;
            RectangleF rect = new RectangleF(
                bounds.X + inset,
                bounds.Y + inset,
                bounds.Width - (inset * 2F) - 1F,
                bounds.Height - (inset * 2F) - 1F);

            float diameter = Math.Min(
                radius * 2F,
                Math.Min(rect.Width, rect.Height));

            using (GraphicsPath path = new GraphicsPath())
            using (Pen pen = new Pen(borderColor, borderWidth))
            {
                path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
                path.AddArc(rect.Right - diameter, rect.Y,
                    diameter, diameter, 270, 90);
                path.AddArc(rect.Right - diameter, rect.Bottom - diameter,
                    diameter, diameter, 0, 90);
                path.AddArc(rect.X, rect.Bottom - diameter,
                    diameter, diameter, 90, 90);
                path.CloseFigure();
                pen.Alignment = PenAlignment.Center;
                graphics.DrawPath(pen, path);
            }
        }

        private static void DrawFolderButtonContent(
            Graphics graphics, Rectangle bounds, Font font,
            Color foreColor, string text)
        {
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Size textSize = TextRenderer.MeasureText(
                text, font, Size.Empty, TextFormatFlags.NoPadding);
            const int iconWidth = 20;
            const int gap = 7;
            int totalWidth = iconWidth + gap + textSize.Width;
            int startX = Math.Max(5, (bounds.Width - totalWidth) / 2);
            int iconY = bounds.Y + (bounds.Height - 16) / 2;

            using (Pen pen = new Pen(foreColor, 1.8F))
            {
                graphics.DrawLine(pen, startX, iconY + 3, startX + 6, iconY + 3);
                graphics.DrawLine(pen, startX + 6, iconY + 3, startX + 9, iconY + 6);
                graphics.DrawLine(pen, startX + 9, iconY + 6, startX + 20, iconY + 6);
                graphics.DrawRectangle(pen, startX, iconY + 5, 20, 11);
            }

            Rectangle textBounds = new Rectangle(
                startX + iconWidth + gap, bounds.Y,
                Math.Max(0, bounds.Right - (startX + iconWidth + gap)),
                bounds.Height);
            TextRenderer.DrawText(graphics, text, font, textBounds,
                foreColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
                TextFormatFlags.SingleLine | TextFormatFlags.NoPadding);
        }

        private static void DrawDatabaseActionButtonContent(
            Graphics graphics, Rectangle bounds, Font font,
            Color foreColor, string text, bool backup)
        {
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Size textSize = TextRenderer.MeasureText(
                text, font, Size.Empty, TextFormatFlags.NoPadding);
            const int iconWidth = 26;
            const int gap = 8;
            int totalWidth = iconWidth + gap + textSize.Width;
            int startX = Math.Max(8, (bounds.Width - totalWidth) / 2);
            int iconY = bounds.Y + (bounds.Height - 22) / 2;

            using (Pen pen = new Pen(foreColor, 1.8F))
            {
                graphics.DrawEllipse(pen, startX, iconY, 16, 6);
                graphics.DrawLine(pen, startX, iconY + 3, startX, iconY + 17);
                graphics.DrawLine(pen, startX + 16, iconY + 3, startX + 16, iconY + 17);
                graphics.DrawArc(pen, startX, iconY + 11, 16, 6, 0, 180);
                if (backup)
                {
                    graphics.DrawLine(pen, startX + 22, iconY + 16, startX + 22, iconY + 3);
                    graphics.DrawLine(pen, startX + 22, iconY + 3, startX + 18, iconY + 7);
                    graphics.DrawLine(pen, startX + 22, iconY + 3, startX + 26, iconY + 7);
                }
                else
                {
                    graphics.DrawLine(pen, startX + 22, iconY + 3, startX + 22, iconY + 16);
                    graphics.DrawLine(pen, startX + 22, iconY + 16, startX + 18, iconY + 12);
                    graphics.DrawLine(pen, startX + 22, iconY + 16, startX + 26, iconY + 12);
                }
            }

            Rectangle textBounds = new Rectangle(
                startX + iconWidth + gap, bounds.Y,
                Math.Max(0, bounds.Right - (startX + iconWidth + gap)),
                bounds.Height);
            TextRenderer.DrawText(graphics, text, font, textBounds,
                foreColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
                TextFormatFlags.SingleLine | TextFormatFlags.NoPadding);
        }

        // Ô nhập bọc bằng Panel để có viền bo góc đồng nhất trên WinForms.
        private sealed class RoundedTextBox : Panel
        {
            private readonly TextBox _textBox;
            public int CornerRadius { get; set; } = 7;

            [AllowNull]
            public override string Text
            {
                get => _textBox?.Text ?? string.Empty;
                set
                {
                    if (_textBox != null)
                        _textBox.Text = value ?? string.Empty;
                }
            }

            public bool ReadOnly
            {
                get => _textBox.ReadOnly;
                set => _textBox.ReadOnly = value;
            }

            public HorizontalAlignment TextAlignment
            {
                get => _textBox.TextAlign;
                set => _textBox.TextAlign = value;
            }

            [AllowNull]
            public override Font Font
            {
                get => base.Font;
                set
                {
                    Font font = value ?? SystemFonts.DefaultFont;

                    base.Font = font;

                    if (_textBox != null)
                        _textBox.Font = font;
                }
            }

            public RoundedTextBox()
            {
                DoubleBuffered = true;
                BackColor = Color.White;
                Padding = new Padding(7, 2, 7, 2);
                _textBox = new TextBox
                {
                    BorderStyle = BorderStyle.None,
                    Dock = DockStyle.Fill,
                    Font = new Font("Segoe UI", 9.5F),
                    BackColor = Color.White,
                    Margin = Padding.Empty
                };
                Controls.Add(_textBox);
                Resize += (sender, e) => UpdateRoundedRegion();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle bounds = new Rectangle(0, 0, Width - 1, Height - 1);
                if (bounds.Width <= 0 || bounds.Height <= 0) return;
                using (GraphicsPath path = CreateControlRoundPath(bounds, CornerRadius))
                using (Pen pen = new Pen(Color.FromArgb(190, 202, 218), 1F))
                    e.Graphics.DrawPath(pen, path);
            }

            private void UpdateRoundedRegion()
            {
                if (Width < 2 || Height < 2) return;
                using (GraphicsPath path = CreateControlRoundPath(
                    new Rectangle(0, 0, Width, Height), CornerRadius))
                {
                    Region? oldRegion = Region;
                    Region = new Region(path);
                    oldRegion?.Dispose();
                }
            }
        }

        // Thanh trạng thái sao lưu độc lập.
        // Không sử dụng Region để cắt control: Region có thể làm vùng vẽ bị
        // clip theo kích thước cell của TableLayoutPanel, khiến hai góc dưới
        // trông như bị một mảng trắng che mất.
        //
        // Thay vào đó, control luôn vẽ nền cha trên toàn bộ hình chữ nhật,
        // sau đó tô nền thông báo theo GraphicsPath bo đủ bốn góc. Vì vậy,
        // phần ngoài đường bo sẽ hiển thị màu nền của vùng chứa (màu trắng).
        private sealed class RoundedStatusPanel : Panel
        {
            public int CornerRadius { get; set; } = 9;

            public Color BorderColor { get; set; } =
                Color.FromArgb(190, 219, 250);

            public Padding TextPadding { get; set; } =
                new Padding(11, 2, 10, 2);

            public RoundedStatusPanel()
            {
                DoubleBuffered = true;
                ResizeRedraw = true;

                SetStyle(
                    ControlStyles.UserPaint |
                    ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.OptimizedDoubleBuffer |
                    ControlStyles.ResizeRedraw,
                    true);
            }

            protected override void OnPaintBackground(PaintEventArgs e)
            {
                if (ClientSize.Width < 2 || ClientSize.Height < 2)
                    return;

                Graphics graphics = e.Graphics;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                // Xóa toàn bộ vùng control bằng màu nền của Panel cha.
                // Đây là phần nền nhìn thấy tại bốn góc bo tròn.
                Color outsideColor = Parent?.BackColor ?? Color.White;
                using (SolidBrush outsideBrush = new SolidBrush(outsideColor))
                {
                    graphics.FillRectangle(
                        outsideBrush,
                        ClientRectangle);
                }

                RectangleF bounds = new RectangleF(
                    0.5F,
                    0.5F,
                    ClientSize.Width - 1F,
                    ClientSize.Height - 1F);

                using (GraphicsPath path = CreateStatusRoundPath(
                    bounds, CornerRadius))
                using (SolidBrush backgroundBrush = new SolidBrush(BackColor))
                {
                    graphics.FillPath(backgroundBrush, path);
                }
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);

                if (ClientSize.Width < 2 || ClientSize.Height < 2)
                    return;

                Graphics graphics = e.Graphics;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                RectangleF bounds = new RectangleF(
                    0.5F,
                    0.5F,
                    ClientSize.Width - 1F,
                    ClientSize.Height - 1F);

                using (GraphicsPath path = CreateStatusRoundPath(
                    bounds, CornerRadius))
                using (Pen pen = new Pen(BorderColor, 1F))
                {
                    graphics.DrawPath(pen, path);
                }

                Rectangle textBounds = new Rectangle(
                    TextPadding.Left,
                    TextPadding.Top,
                    Math.Max(
                        0,
                        ClientSize.Width -
                        TextPadding.Left -
                        TextPadding.Right),
                    Math.Max(
                        0,
                        ClientSize.Height -
                        TextPadding.Top -
                        TextPadding.Bottom));

                TextRenderer.DrawText(
                    graphics,
                    Text,
                    Font,
                    textBounds,
                    ForeColor,
                    TextFormatFlags.Left |
                    TextFormatFlags.VerticalCenter |
                    TextFormatFlags.EndEllipsis |
                    TextFormatFlags.NoPrefix);
            }

            private static GraphicsPath CreateStatusRoundPath(
                RectangleF rect, int radius)
            {
                float diameter = Math.Max(
                    2F,
                    Math.Min(
                        radius * 2F,
                        Math.Min(rect.Width, rect.Height)));

                GraphicsPath path = new GraphicsPath();

                path.AddArc(
                    rect.X,
                    rect.Y,
                    diameter,
                    diameter,
                    180F,
                    90F);

                path.AddArc(
                    rect.Right - diameter,
                    rect.Y,
                    diameter,
                    diameter,
                    270F,
                    90F);

                path.AddArc(
                    rect.Right - diameter,
                    rect.Bottom - diameter,
                    diameter,
                    diameter,
                    0F,
                    90F);

                path.AddArc(
                    rect.X,
                    rect.Bottom - diameter,
                    diameter,
                    diameter,
                    90F,
                    90F);

                path.CloseFigure();
                return path;
            }
        }

        // Panel bo góc dùng chung cho các thẻ giao diện.
        private sealed class RoundedPanel : Panel
        {
            public int CornerRadius { get; set; } = 10;
            public Color BorderColor { get; set; } = Color.Transparent;

            public RoundedPanel()
            {
                DoubleBuffered = true;
                ResizeRedraw = true;
            }

            // Vẽ nền theo đúng GraphicsPath để cả bốn góc, đặc biệt hai góc dưới,
            // không bị nền mặc định của Panel phủ vuông trở lại.
            protected override void OnPaintBackground(PaintEventArgs e)
            {
                if (Width <= 1 || Height <= 1) return;

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle bounds = new Rectangle(0, 0, Width - 1, Height - 1);
                using (GraphicsPath path = CreateRoundPath(bounds, CornerRadius))
                using (SolidBrush brush = new SolidBrush(BackColor))
                {
                    e.Graphics.FillPath(brush, path);
                }
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle bounds = new Rectangle(0, 0, Width - 1, Height - 1);
                if (bounds.Width <= 0 || bounds.Height <= 0) return;

                using (GraphicsPath path = CreateRoundPath(bounds, CornerRadius))
                using (Pen pen = new Pen(BorderColor, 1F))
                {
                    if (BorderColor != Color.Transparent)
                        e.Graphics.DrawPath(pen, path);
                }
            }

            protected override void OnHandleCreated(EventArgs e)
            {
                base.OnHandleCreated(e);
                UpdateRoundedRegion();
            }

            protected override void OnResize(EventArgs eventargs)
            {
                base.OnResize(eventargs);
                UpdateRoundedRegion();
                Invalidate();
            }

            private void UpdateRoundedRegion()
            {
                if (Width <= 1 || Height <= 1) return;

                using (GraphicsPath path = CreateRoundPath(
                    new Rectangle(0, 0, Width, Height), CornerRadius))
                {
                    Region = new Region(path);
                }
            }

            private static GraphicsPath CreateRoundPath(Rectangle rect, int radius)
            {
                int diameter = Math.Max(2, radius * 2);
                GraphicsPath path = new GraphicsPath();
                path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
                path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
                path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
                path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
                path.CloseFigure();
                return path;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (lvActivity != null &&
                lvActivity.Columns.Count > 1)
            {
                int availableWidth =
                    Math.Max(220, lvActivity.ClientSize.Width - 4);

                int timeWidth =
                    Math.Max(118, (int)(availableWidth * 0.29));

                lvActivity.Columns[1].Width = timeWidth;
                lvActivity.Columns[0].Width =
                    Math.Max(100, availableWidth - timeWidth);

                if (lvActivity.Columns.Count > 2)
                    lvActivity.Columns[2].Width = 0;
            }
        }
    }
}