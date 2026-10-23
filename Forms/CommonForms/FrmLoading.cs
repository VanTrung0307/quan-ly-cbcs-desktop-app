
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyCBCS.Forms.CommonForms
{
    public partial class FrmLoading : Form
    {
        private Label lblTitle = null!;
        private Label lblSubtitle = null!;
        private Label lblStatus = null!;
        private Label lblVersion = null!;
        private PictureBox picLogo = null!;
        private LoadingProgressBar loadingBar = null!;
        private System.Windows.Forms.Timer animationTimer = null!;

        private int progressValue = 0;
        private int statusIndex = 0;

        private readonly string[] loadingMessages =
        {
            "Đang khởi tạo các thành phần...",
            "Đang kết nối cơ sở dữ liệu...",
            "Đang tải dữ liệu hệ thống...",
            "Đang hoàn tất khởi tạo..."
        };

        public FrmLoading()
        {
            InitializeComponent();
            BuildModernLayout();
        }

        private void BuildModernLayout()
        {
            // Loại bỏ các control của giao diện cũ.
            foreach (Control control in Controls.Cast<Control>().ToArray())
            {
                Controls.Remove(control);
                control.Dispose();
            }

            SuspendLayout();

            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1280, 720);
            MinimumSize = new Size(1280, 720);
            MaximumSize = new Size(1280, 720);

            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;
            ShowInTaskbar = false;
            TopMost = true;
            BackColor = Color.FromArgb(111, 146, 190);
            DoubleBuffered = true;

            // Bo góc cửa sổ.
            using (GraphicsPath path =
                CreateRoundedPath(ClientRectangle, 20))
            {
                Region = new Region(path);
            }

            // Logo CSCĐ.
            picLogo = new PictureBox
            {
                Image = Properties.Resources.logoCSCD,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent,
                Size = new Size(250, 270),
                Location = new Point(515, 45),
                TabStop = false
            };

            // Tiêu đề chính.
            lblTitle = new Label
            {
                Text = "ĐANG KHỞI TẠO HỆ THỐNG",
                Font = new Font("Segoe UI", 34F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 43, 91),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(100, 350),
                Size = new Size(1080, 58)
            };

            // Dòng mô tả.
            lblSubtitle = new Label
            {
                Text = "Vui lòng chờ trong giây lát...",
                Font = new Font("Segoe UI", 20F),
                ForeColor = Color.FromArgb(91, 117, 158),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(100, 414),
                Size = new Size(1080, 38)
            };

            // Thanh tiến trình tự vẽ.
            loadingBar = new LoadingProgressBar
            {
                Location = new Point(260, 468),
                Size = new Size(760, 38),
                Value = 0
            };

            // Trạng thái tải.
            lblStatus = new Label
            {
                Text = loadingMessages[0],
                Font = new Font("Segoe UI", 16F),
                ForeColor = Color.FromArgb(76, 103, 148),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(160, 518),
                Size = new Size(960, 34)
            };

            // Phiên bản phần mềm.
            lblVersion = new Label
            {
                Text = "Quản lý CBCS v2.0",
                Font = new Font("Segoe UI", 16F),
                ForeColor = Color.FromArgb(111, 135, 174),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(490, 604),
                Size = new Size(300, 32)
            };

            Controls.Add(picLogo);
            Controls.Add(lblTitle);
            Controls.Add(lblSubtitle);
            Controls.Add(loadingBar);
            Controls.Add(lblStatus);
            Controls.Add(lblVersion);

            // Đưa nội dung lên trên nền trang trí.
            picLogo.BringToFront();
            lblTitle.BringToFront();
            lblSubtitle.BringToFront();
            loadingBar.BringToFront();
            lblStatus.BringToFront();
            lblVersion.BringToFront();

            // Tiến trình minh họa khi chưa có tiến độ thực tế.
            animationTimer = new System.Windows.Forms.Timer
            {
                Interval = 100
            };

            animationTimer.Tick += (s, e) =>
            {
                if (progressValue < 92)
                {
                    progressValue++;
                    loadingBar.Value = progressValue;

                    int newStatusIndex =
                        Math.Min(progressValue / 24,
                            loadingMessages.Length - 1);

                    if (newStatusIndex != statusIndex)
                    {
                        statusIndex = newStatusIndex;
                        lblStatus.Text =
                            loadingMessages[statusIndex];
                    }
                }
            };

            ResumeLayout(true);
        }

        protected override void OnPaintBackground(
            PaintEventArgs e)
        {
            // Vẽ nền mặc định trước; sau đó vẽ đè thiết kế tùy chỉnh.
            // Không gọi base.OnPaintBackground ở cuối vì sẽ che mất thiết kế.
            base.OnPaintBackground(e);

            Graphics g = e.Graphics;

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // Nền ngoài xanh xám.
            g.Clear(Color.FromArgb(111, 146, 190));

            Rectangle card = new Rectangle(
                18, 18,
                ClientSize.Width - 36,
                ClientSize.Height - 36);

            using (GraphicsPath cardPath =
                CreateRoundedPath(card, 22))
            {
                // Nền trắng xanh nhạt.
                using (LinearGradientBrush brush =
                    new LinearGradientBrush(
                        card,
                        Color.FromArgb(255, 255, 255),
                        Color.FromArgb(239, 245, 255),
                        LinearGradientMode.Vertical))
                {
                    g.FillPath(brush, cardPath);
                }

                // Giới hạn các họa tiết trong khung chính.
                GraphicsState state = g.Save();
                g.SetClip(cardPath);

                DrawBottomWaves(g, card);
                DrawDecorativeDots(g);
                DrawWatermark(g);

                g.Restore(state);

                // Viền mảnh quanh khung.
                using (Pen borderPen = new Pen(
                    Color.FromArgb(205, 224, 250), 1.5F))
                {
                    g.DrawPath(borderPen, cardPath);
                }
            }
        }

        private void DrawBottomWaves(Graphics g, Rectangle card)
        {
            int bottom = card.Bottom - 2;

            // Lớp sóng xanh nhạt.
            using (GraphicsPath wave1 = new GraphicsPath())
            {
                wave1.StartFigure();
                wave1.AddBezier(
                    card.Left, bottom - 140,
                    card.Left + 240, bottom - 55,
                    card.Left + 410, bottom + 12,
                    card.Left + 650, bottom - 15);

                wave1.AddBezier(
                    card.Left + 650, bottom - 15,
                    card.Left + 900, bottom - 35,
                    card.Left + 1040, bottom - 135,
                    card.Right, bottom - 125);

                wave1.AddLine(card.Right, bottom,
                    card.Left, bottom);

                wave1.CloseFigure();

                using (SolidBrush brush = new SolidBrush(
                    Color.FromArgb(190, 193, 222, 255)))
                {
                    g.FillPath(brush, wave1);
                }
            }

            // Lớp sóng xanh trung bình.
            using (GraphicsPath wave2 = new GraphicsPath())
            {
                wave2.StartFigure();
                wave2.AddBezier(
                    card.Left, bottom - 105,
                    card.Left + 250, bottom - 5,
                    card.Left + 470, bottom + 8,
                    card.Left + 700, bottom - 8);

                wave2.AddBezier(
                    card.Left + 700, bottom - 8,
                    card.Left + 930, bottom - 20,
                    card.Left + 1070, bottom - 95,
                    card.Right, bottom - 75);

                wave2.AddLine(card.Right, bottom,
                    card.Left, bottom);

                wave2.CloseFigure();

                using (SolidBrush brush = new SolidBrush(
                    Color.FromArgb(180, 78, 166, 255)))
                {
                    g.FillPath(brush, wave2);
                }
            }

            // Lớp sóng xanh đậm phía dưới.
            using (GraphicsPath wave3 = new GraphicsPath())
            {
                wave3.StartFigure();
                wave3.AddBezier(
                    card.Left, bottom - 70,
                    card.Left + 260, bottom + 5,
                    card.Left + 460, bottom + 3,
                    card.Left + 700, bottom - 5);

                wave3.AddBezier(
                    card.Left + 700, bottom - 5,
                    card.Left + 930, bottom - 10,
                    card.Left + 1080, bottom - 65,
                    card.Right, bottom - 50);

                wave3.AddLine(card.Right, bottom,
                    card.Left, bottom);

                wave3.CloseFigure();

                using (SolidBrush brush = new SolidBrush(
                    Color.FromArgb(225, 0, 77, 164)))
                {
                    g.FillPath(brush, wave3);
                }
            }
        }

        private void DrawDecorativeDots(Graphics g)
        {
            using (SolidBrush brush = new SolidBrush(
                Color.FromArgb(95, 191, 216, 250)))
            {
                // Cụm chấm phía trên bên trái.
                for (int row = 0; row < 6; row++)
                {
                    for (int col = 0; col < 6; col++)
                    {
                        g.FillEllipse(
                            brush,
                            65 + col * 18,
                            70 + row * 20,
                            7, 7);
                    }
                }

                // Cụm chấm phía dưới bên phải.
                for (int row = 0; row < 5; row++)
                {
                    for (int col = 0; col < 6; col++)
                    {
                        g.FillEllipse(
                            brush,
                            1115 + col * 18,
                            440 + row * 18,
                            7, 7);
                    }
                }
            }
        }

        private void DrawWatermark(Graphics g)
        {
            // Huy hiệu cách điệu, dùng làm họa tiết nền mờ.
            using (GraphicsPath shield = new GraphicsPath())
            {
                shield.StartFigure();
                shield.AddLine(965, 65, 1185, 65);
                shield.AddLine(1185, 65, 1172, 220);
                shield.AddBezier(
                    1172, 220,
                    1155, 285,
                    1085, 330,
                    1075, 335);

                shield.AddBezier(
                    1075, 335,
                    1060, 327,
                    995, 285,
                    978, 220);

                shield.CloseFigure();

                using (Pen pen = new Pen(
                    Color.FromArgb(35, 135, 177, 238), 13))
                {
                    g.DrawPath(pen, shield);
                }

                // Ngôi sao mờ ở giữa huy hiệu.
                PointF[] star = new PointF[10];
                float cx = 1075;
                float cy = 155;
                float outer = 48;
                float inner = 21;

                for (int i = 0; i < 10; i++)
                {
                    double angle =
                        -Math.PI / 2 + i * Math.PI / 5;

                    float radius = i % 2 == 0 ? outer : inner;

                    star[i] = new PointF(
                        cx + (float)Math.Cos(angle) * radius,
                        cy + (float)Math.Sin(angle) * radius);
                }

                using (SolidBrush brush = new SolidBrush(
                    Color.FromArgb(24, 135, 177, 238)))
                {
                    g.FillPolygon(brush, star);
                }
            }
        }

        private static GraphicsPath CreateRoundedPath(
            Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            int diameter = radius * 2;

            path.AddArc(
                rect.X, rect.Y, diameter, diameter, 180, 90);

            path.AddArc(
                rect.Right - diameter, rect.Y,
                diameter, diameter, 270, 90);

            path.AddArc(
                rect.Right - diameter,
                rect.Bottom - diameter,
                diameter, diameter, 0, 90);

            path.AddArc(
                rect.X, rect.Bottom - diameter,
                diameter, diameter, 90, 90);

            path.CloseFigure();
            return path;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            animationTimer?.Start();
        }

        // Thanh tiến trình bo góc, tô màu chuyển sắc.
        private sealed class LoadingProgressBar : Control
        {
            private int value;

            public int Value
            {
                get => value;
                set
                {
                    this.value = Math.Max(0, Math.Min(100, value));
                    Invalidate();
                }
            }

            public LoadingProgressBar()
            {
                SetStyle(
                    ControlStyles.UserPaint |
                    ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.OptimizedDoubleBuffer |
                    ControlStyles.ResizeRedraw,
                    true);

                BackColor = this.BackColor;
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);

                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                Rectangle track = new Rectangle(
                    0, 0, Width - 1, Height - 1);

                using (GraphicsPath trackPath =
                    CreateRoundedPath(track, Height / 2))
                using (SolidBrush trackBrush = new SolidBrush(
                    Color.FromArgb(222, 231, 245)))
                {
                    g.FillPath(trackBrush, trackPath);
                }

                int fillWidth =
                    (int)((Width - 1) * value / 100.0);

                if (fillWidth > 0)
                {
                    Rectangle fill = new Rectangle(
                        0, 0,
                        Math.Min(fillWidth, Width - 1),
                        Height - 1);

                    using (GraphicsPath fillPath =
                        CreateRoundedPath(fill, Height / 2))
                    using (LinearGradientBrush brush =
                        new LinearGradientBrush(
                            fill,
                            Color.FromArgb(0, 94, 255),
                            Color.FromArgb(0, 177, 239),
                            LinearGradientMode.Horizontal))
                    {
                        g.FillPath(brush, fillPath);
                    }
                }

                // Phần trăm ở bên phải, giống bản thiết kế.
                string percentText = $"{value}%";

                using (Font font = new Font(
                    "Segoe UI", 21F, FontStyle.Bold))
                using (SolidBrush textBrush = new SolidBrush(
                    Color.FromArgb(15, 43, 91)))
                {
                    TextRenderer.DrawText(
                        g,
                        percentText,
                        font,
                        new Rectangle(
                            Width - 105, 0, 90, Height),
                        textBrush.Color,
                        TextFormatFlags.Right |
                        TextFormatFlags.VerticalCenter |
                        TextFormatFlags.NoPadding);
                }
            }
        }
    }
}
