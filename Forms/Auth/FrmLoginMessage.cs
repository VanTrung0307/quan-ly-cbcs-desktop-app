
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace QuanLyCBCS.Forms.Auth
{
    public enum LoginMessageType
    {
        UsernameRequired,
        PasswordRequired,
        ForgotPassword,
        InvalidCredentials,
        AccountLocked,
        Information,
        Success
    }

    public partial class FrmLoginMessage : Form
    {
        private readonly LoginMessageType _messageType;

        private readonly Color _navy = Color.FromArgb(12, 31, 72);
        private readonly Color _blue = Color.FromArgb(0, 112, 243);
        private readonly Color _footerColor = Color.FromArgb(242, 247, 253);
        private readonly Color _lineColor = Color.FromArgb(218, 229, 243);


        private Panel pnlFooter = null!;
        private Panel pnlIcon = null!;

        private Label lblTitle = null!;
        private Label lblDescription = null!;

        private Button btnOK = null!;
        private Button btnClose = null!;

        public FrmLoginMessage(
            LoginMessageType messageType,
            string title,
            string description)
        {
            _messageType = messageType;

            InitializeDialog(title, description);
        }

        private void InitializeDialog(string title, string description)
        {
            // Cấu hình cửa sổ
            Text = "Quản lý CBCS";
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            Size = new Size(650, 250);
            MinimumSize = new Size(650, 250);
            MaximumSize = new Size(650, 250);
            BackColor = Color.White;
            Font = new Font("Segoe UI", 10F);
            KeyPreview = true;

            // Bo góc cửa sổ
            ApplyRoundedCorners();

            // Khu vực chân hộp thoại
            pnlFooter = new Panel
            {
                Location = new Point(0, 174),
                Size = new Size(650, 76),
                BackColor = _footerColor
            };

            pnlFooter.Paint += delegate (object? sender, PaintEventArgs e)
            {
                using (Pen pen = new Pen(_lineColor, 1))
                {
                    e.Graphics.DrawLine(pen, 0, 0, 650, 0);
                }
            };

            Controls.Add(pnlFooter);

            // Biểu tượng lớn bên trái
            pnlIcon = new Panel
            {
                Location = new Point(32, 51),
                Size = new Size(88, 88),
                BackColor = Color.Transparent
            };

            pnlIcon.Paint += PnlIcon_Paint;
            Controls.Add(pnlIcon);

            // Tiêu đề
            lblTitle = new Label
            {
                Location = new Point(145, 57),
                Size = new Size(445, 34),
                Text = title,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = _navy,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = false,
                BackColor = Color.Transparent
            };

            Controls.Add(lblTitle);

            // Nội dung thông báo
            lblDescription = new Label
            {
                Location = new Point(146, 98),
                Size = new Size(450, 52),
                Text = description,
                Font = new Font("Segoe UI", 11F, FontStyle.Regular),
                ForeColor = Color.FromArgb(66, 87, 128),
                TextAlign = ContentAlignment.TopLeft,
                AutoSize = false,
                BackColor = Color.Transparent
            };

            Controls.Add(lblDescription);

            // Nút đóng ở góc trên bên phải
            btnClose = new Button
            {
                Location = new Point(ClientSize.Width - 40, 8),
                Size = new Size(32, 32),

                // Không dùng Text để tránh chữ X bị vẽ hai lần
                Text = string.Empty,

                ForeColor = Color.FromArgb(15, 30, 65),
                BackColor = Color.White,

                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TabStop = false,
                UseVisualStyleBackColor = false,

                Padding = Padding.Empty,
                Margin = Padding.Empty
            };

            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.White;
            btnClose.FlatAppearance.MouseDownBackColor = Color.White;

            // Tự vẽ duy nhất dấu X và nền hover
            btnClose.Paint += (s, e) =>
            {
                // Kiểm tra kiểu và null trước khi sử dụng
                if (!(s is Button btn))
                    return;

                Graphics g = e.Graphics;

                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                bool isHover = btn.ClientRectangle.Contains(
                    btn.PointToClient(Cursor.Position));

                // Vùng hover nhỏ hơn nút, không chạm mép hộp thoại
                Rectangle hoverRect = new Rectangle(4, 4, 24, 24);

                if (isHover)
                {
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        int radius = 6;
                        int diameter = radius * 2;

                        path.AddArc(
                            hoverRect.X, hoverRect.Y,
                            diameter, diameter, 180, 90);

                        path.AddArc(
                            hoverRect.Right - diameter, hoverRect.Y,
                            diameter, diameter, 270, 90);

                        path.AddArc(
                            hoverRect.Right - diameter,
                            hoverRect.Bottom - diameter,
                            diameter, diameter, 0, 90);

                        path.AddArc(
                            hoverRect.X,
                            hoverRect.Bottom - diameter,
                            diameter, diameter, 90, 90);

                        path.CloseFigure();

                        using (SolidBrush brush = new SolidBrush(
                            Color.FromArgb(255, 235, 235)))
                        {
                            g.FillPath(brush, path);
                        }
                    }
                }

                // Vẽ dấu X bằng hai nét chéo, không dùng ký tự "×"
                Color xColor = isHover
                    ? Color.FromArgb(239, 49, 57)
                    : Color.FromArgb(15, 30, 65);

                using (Pen pen = new Pen(xColor, 2.0F))
                {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;

                    g.DrawLine(pen, 12, 12, 20, 20);
                    g.DrawLine(pen, 20, 12, 12, 20);
                }
            };

            btnClose.MouseEnter += (s, e) => btnClose.Invalidate();
            btnClose.MouseLeave += (s, e) => btnClose.Invalidate();

            btnClose.Click += (s, e) => Close();

            Controls.Add(btnClose);
            btnClose.BringToFront();

            // Nút OK bo góc
            btnOK = new Button
            {
                Location = new Point(484, 14),
                Size = new Size(134, 48),

                Text = "OK",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),

                ForeColor = Color.White,
                BackColor = _blue,

                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TabIndex = 0,
                UseVisualStyleBackColor = false
            };

            btnOK.FlatAppearance.BorderSize = 0;

            // Màu khi rê chuột
            btnOK.FlatAppearance.MouseOverBackColor =
                Color.FromArgb(0, 94, 218);

            // Màu khi nhấn
            btnOK.FlatAppearance.MouseDownBackColor =
                Color.FromArgb(0, 79, 190);

            // Bo góc khi nút thay đổi kích thước
            btnOK.Resize += (s, e) =>
            {
                SetButtonRoundRegion(btnOK, 8);
            };

            // Áp dụng bo góc ngay khi khởi tạo
            SetButtonRoundRegion(btnOK, 8);

            // Xử lý sự kiện OK
            btnOK.Click += delegate
            {
                DialogResult = DialogResult.OK;
                Close();
            };

            pnlFooter.Controls.Add(btnOK);
            btnOK.BringToFront();

            // Enter xác nhận, Esc đóng hộp thoại
            AcceptButton = btnOK;
            CancelButton = btnClose;

            KeyDown += FrmLoginMessage_KeyDown;
        }

        private void SetButtonRoundRegion(Button button, int radius)
        {
            if (button == null || button.Width <= 0 || button.Height <= 0)
                return;

            using (GraphicsPath path = new GraphicsPath())
            {
                int diameter = radius * 2;
                Rectangle rect = new Rectangle(
                    0, 0,
                    button.Width,
                    button.Height
                );

                path.AddArc(
                    rect.X, rect.Y,
                    diameter, diameter,
                    180, 90
                );

                path.AddArc(
                    rect.Right - diameter, rect.Y,
                    diameter, diameter,
                    270, 90
                );

                path.AddArc(
                    rect.Right - diameter,
                    rect.Bottom - diameter,
                    diameter, diameter,
                    0, 90
                );

                path.AddArc(
                    rect.X,
                    rect.Bottom - diameter,
                    diameter, diameter,
                    90, 90
                );

                path.CloseFigure();

                button.Region?.Dispose();
                button.Region = new Region(path);
            }
        }

        private void FrmLoginMessage_KeyDown(
            object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
                e.Handled = true;
            }
        }

        private void ApplyRoundedCorners()
        {
            if (Width <= 0 || Height <= 0)
                return;

            using (GraphicsPath path = CreateRoundedPath(
                new Rectangle(0, 0, Width, Height), 18))
            {
                Region = new Region(path);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ApplyRoundedCorners();
        }

        // Vẽ biểu tượng bằng GDI+, không cần cài thư viện icon bên ngoài
        private void PnlIcon_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Color accent = GetAccentColor();
            Color pale = GetPaleColor();

            using (SolidBrush background = new SolidBrush(pale))
            {
                g.FillEllipse(background, 0, 0, 88, 88);
            }

            using (SolidBrush accentBrush = new SolidBrush(accent))
            using (Pen accentPen = new Pen(accent, 5))
            using (Pen whitePen = new Pen(Color.White, 3))
            {
                accentPen.StartCap = LineCap.Round;
                accentPen.EndCap = LineCap.Round;
                accentPen.LineJoin = LineJoin.Round;

                switch (_messageType)
                {
                    case LoginMessageType.UsernameRequired:
                        // Đầu người
                        g.FillEllipse(accentBrush, 34, 20, 20, 20);

                        // Vai và thân người
                        using (GraphicsPath body = CreateRoundedPath(
                            new Rectangle(22, 45, 44, 25), 12))
                        {
                            g.FillPath(accentBrush, body);
                        }
                        break;

                    case LoginMessageType.PasswordRequired:
                        DrawLock(g, accentBrush, accentPen, whitePen);
                        break;

                    case LoginMessageType.ForgotPassword:
                    case LoginMessageType.Information:
                        // Chữ i
                        g.FillEllipse(accentBrush, 40, 22, 8, 8);

                        using (SolidBrush infoBrush = new SolidBrush(accent))
                        {
                            g.FillRectangle(infoBrush, 40, 36, 8, 31);
                        }
                        break;

                    case LoginMessageType.InvalidCredentials:
                        // Dấu chấm than
                        g.FillEllipse(accentBrush, 40, 61, 8, 8);

                        using (SolidBrush warningBrush = new SolidBrush(accent))
                        {
                            using (GraphicsPath mark = CreateRoundedPath(
                                new Rectangle(40, 20, 8, 34), 4))
                            {
                                g.FillPath(warningBrush, mark);
                            }
                        }
                        break;

                    case LoginMessageType.AccountLocked:
                        // Dấu X
                        g.DrawLine(accentPen, 29, 29, 59, 59);
                        g.DrawLine(accentPen, 59, 29, 29, 59);
                        break;

                    case LoginMessageType.Success:
                        // Dấu tích
                        g.DrawLine(accentPen, 23, 45, 37, 58);
                        g.DrawLine(accentPen, 37, 58, 65, 29);
                        break;

                    default:
                        g.FillEllipse(accentBrush, 40, 22, 8, 8);
                        g.FillRectangle(accentBrush, 40, 36, 8, 31);
                        break;
                }
            }
        }

        private void DrawLock(
            Graphics g,
            Brush fillBrush,
            Pen linePen,
            Pen whitePen)
        {
            // Quai khóa
            g.DrawArc(linePen, 29, 17, 30, 35, 180, 180);

            // Thân khóa
            using (GraphicsPath lockBody = CreateRoundedPath(
                new Rectangle(24, 39, 40, 32), 6))
            {
                g.FillPath(fillBrush, lockBody);
            }

            // Lỗ khóa
            using (SolidBrush whiteBrush = new SolidBrush(Color.White))
            {
                g.FillEllipse(whiteBrush, 41, 48, 7, 7);
                g.FillRectangle(whiteBrush, 43, 54, 3, 8);
            }
        }

        private Color GetAccentColor()
        {
            switch (_messageType)
            {
                case LoginMessageType.UsernameRequired:
                case LoginMessageType.ForgotPassword:
                case LoginMessageType.Information:
                    return Color.FromArgb(0, 112, 243);

                case LoginMessageType.PasswordRequired:
                case LoginMessageType.AccountLocked:
                    return Color.FromArgb(239, 49, 57);

                case LoginMessageType.InvalidCredentials:
                    return Color.FromArgb(255, 153, 0);

                case LoginMessageType.Success:
                    return Color.FromArgb(22, 163, 74);

                default:
                    return Color.FromArgb(0, 112, 243);
            }
        }

        private Color GetPaleColor()
        {
            switch (_messageType)
            {
                case LoginMessageType.UsernameRequired:
                case LoginMessageType.ForgotPassword:
                case LoginMessageType.Information:
                    return Color.FromArgb(224, 239, 255);

                case LoginMessageType.PasswordRequired:
                case LoginMessageType.AccountLocked:
                    return Color.FromArgb(255, 229, 231);

                case LoginMessageType.InvalidCredentials:
                    return Color.FromArgb(255, 241, 220);

                case LoginMessageType.Success:
                    return Color.FromArgb(220, 252, 231);

                default:
                    return Color.FromArgb(224, 239, 255);
            }
        }

        private static GraphicsPath CreateRoundedPath(
            Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            if (diameter > bounds.Width)
                diameter = bounds.Width;

            if (diameter > bounds.Height)
                diameter = bounds.Height;

            Rectangle arc = new Rectangle(
                bounds.X, bounds.Y, diameter, diameter);

            path.AddArc(arc, 180, 90);

            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        // Hàm tiện ích để gọi hộp thoại từ FrmLogin
        public static DialogResult ShowMessage(
            IWin32Window owner,
            LoginMessageType messageType,
            string title,
            string description)
        {
            using (FrmLoginMessage dialog =
                new FrmLoginMessage(messageType, title, description))
            {
                if (owner != null)
                    return dialog.ShowDialog(owner);

                return dialog.ShowDialog();
            }
        }
    }
}
