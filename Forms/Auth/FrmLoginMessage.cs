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
        Success,
        Confirmation
    }

    public partial class FrmLoginMessage : Form
    {
        private readonly LoginMessageType _messageType;

        private readonly Color _navy = Color.FromArgb(12, 31, 72);
        private readonly Color _blue = Color.FromArgb(0, 112, 243);
        private readonly Color _footerColor = Color.FromArgb(232, 240, 253);
        private readonly Color _lineColor = Color.FromArgb(207, 222, 245);

        private Panel pnlHeader = null!;
        private Panel pnlFooter = null!;
        private Panel pnlIcon = null!;
        private Panel pnlDivider = null!;
        private Label lblTitle = null!;
        private Label lblDescription = null!;
        private Button btnOK = null!;
        private Button btnNo = null!;
        private Button btnClose = null!;

        public FrmLoginMessage(LoginMessageType messageType, string title, string description)
        {
            _messageType = messageType;
            InitializeDialog(title, description);
        }

        private void InitializeDialog(string title, string description)
        {
            // Hộp thoại nhỏ gọn, căn giữa cửa sổ cha.
            Text = "Quản lý CBCS";
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            ClientSize = new Size(560, 230);
            MinimumSize = new Size(560, 230);
            MaximumSize = new Size(560, 230);
            BackColor = Color.White;
            Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            KeyPreview = true;
            DoubleBuffered = true;

            // Thanh tiêu đề xanh.
            pnlHeader = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(560, 64),
                BackColor = _blue
            };
            pnlHeader.Paint += PnlHeader_Paint;
            Controls.Add(pnlHeader);

            lblTitle = new Label
            {
                Location = new Point(78, 0),
                Size = new Size(390, 64),
                Text = string.IsNullOrWhiteSpace(title) ? "Thông báo" : title,
                Font = new Font("Segoe UI", 19F, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = false,
                BackColor = Color.Transparent
            };
            pnlHeader.Controls.Add(lblTitle);

            // Biểu tượng thông tin nhỏ ở đầu thanh tiêu đề.
            Panel headerIcon = new Panel
            {
                Location = new Point(27, 15),
                Size = new Size(34, 34),
                BackColor = Color.Transparent
            };
            headerIcon.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (SolidBrush b = new SolidBrush(Color.White))
                    g.FillEllipse(b, 0, 0, 33, 33);
                using (Font f = new Font("Segoe UI", 20F, FontStyle.Bold))
                using (SolidBrush b = new SolidBrush(_blue))
                {
                    StringFormat sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    g.DrawString("i", f, b, new RectangleF(0, -1, 33, 34), sf);
                }
            };
            pnlHeader.Controls.Add(headerIcon);
            headerIcon.BringToFront();

            // Nút đóng trên thanh tiêu đề.
            btnClose = new Button
            {
                Location = new Point(510, 12),
                Size = new Size(36, 36),
                Text = string.Empty,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                TabStop = false,
                UseVisualStyleBackColor = false
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(45, 255, 255, 255);
            btnClose.FlatAppearance.MouseDownBackColor = Color.FromArgb(65, 255, 255, 255);
            btnClose.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen p = new Pen(Color.White, 2.5F))
                {
                    p.StartCap = LineCap.Round;
                    p.EndCap = LineCap.Round;
                    e.Graphics.DrawLine(p, 12, 12, 24, 24);
                    e.Graphics.DrawLine(p, 24, 12, 12, 24);
                }
            };
            btnClose.Click += (s, e) => Close();
            pnlHeader.Controls.Add(btnClose);
            btnClose.BringToFront();

            // Vùng nội dung trắng.
            pnlIcon = new Panel
            {
                Location = new Point(36, 86),
                Size = new Size(66, 66),
                BackColor = Color.Transparent
            };
            pnlIcon.Paint += PnlIcon_Paint;
            Controls.Add(pnlIcon);

            pnlDivider = new Panel
            {
                Location = new Point(126, 82),
                Size = new Size(3, 78),
                BackColor = _blue
            };
            Controls.Add(pnlDivider);

            lblDescription = new Label
            {
                Location = new Point(153, 78),
                Size = new Size(375, 88),
                Text = description ?? string.Empty,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                ForeColor = _navy,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = false,
                BackColor = Color.Transparent
            };
            Controls.Add(lblDescription);

            // Chân hộp thoại.
            pnlFooter = new Panel
            {
                Location = new Point(0, 174),
                Size = new Size(560, 56),
                BackColor = _footerColor
            };
            pnlFooter.Paint += (s, e) =>
            {
                using (Pen p = new Pen(_lineColor, 1F))
                    e.Graphics.DrawLine(p, 0, 0, pnlFooter.Width, 0);
            };
            Controls.Add(pnlFooter);

            bool isConfirmation = _messageType == LoginMessageType.Confirmation;

            btnOK = new Button
            {
                Location = isConfirmation ? new Point(166, 8) : new Point(426, 8),
                Size = new Size(112, 40),
                Text = isConfirmation ? "✓  Có" : "✓  OK",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = _blue,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TabIndex = 0,
                UseVisualStyleBackColor = false
            };
            btnOK.FlatAppearance.BorderSize = 0;
            btnOK.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 94, 218);
            btnOK.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 79, 190);
            btnOK.Resize += (s, e) => SetButtonRoundRegion(btnOK, 9);
            SetButtonRoundRegion(btnOK, 9);
            btnOK.Click += (s, e) =>
            {
                DialogResult = isConfirmation ? DialogResult.Yes : DialogResult.OK;
                Close();
            };
            pnlFooter.Controls.Add(btnOK);

            if (isConfirmation)
            {
                btnNo = new Button
                {
                    Location = new Point(292, 8),
                    Size = new Size(112, 40),
                    Text = "Không",
                    Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                    ForeColor = _navy,
                    BackColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand,
                    TabIndex = 1,
                    UseVisualStyleBackColor = false
                };
                btnNo.FlatAppearance.BorderColor = _lineColor;
                btnNo.FlatAppearance.BorderSize = 1;
                btnNo.FlatAppearance.MouseOverBackColor = Color.FromArgb(242, 247, 253);
                btnNo.FlatAppearance.MouseDownBackColor = Color.FromArgb(228, 238, 251);
                btnNo.Resize += (s, e) => SetButtonRoundRegion(btnNo, 9);
                SetButtonRoundRegion(btnNo, 9);
                btnNo.Click += (s, e) =>
                {
                    DialogResult = DialogResult.No;
                    Close();
                };
                pnlFooter.Controls.Add(btnNo);
                CancelButton = btnNo;
            }
            else
            {
                CancelButton = btnClose;
            }

            AcceptButton = btnOK;
            KeyDown += FrmLoginMessage_KeyDown;

            ApplyRoundedCorners();
        }

        private void PnlHeader_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (LinearGradientBrush brush = new LinearGradientBrush(
                pnlHeader.ClientRectangle,
                Color.FromArgb(0, 103, 202),
                Color.FromArgb(0, 75, 164),
                LinearGradientMode.Horizontal))
            {
                g.FillRectangle(brush, pnlHeader.ClientRectangle);
            }

            // Dải sáng nhẹ tạo chiều sâu, không làm rối thanh tiêu đề.
            using (Pen p = new Pen(Color.FromArgb(35, 255, 255, 255), 1F))
                g.DrawLine(p, 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
        }

        private void PnlIcon_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Color accent = GetAccentColor();
            Color pale = GetPaleColor();
            using (SolidBrush paleBrush = new SolidBrush(pale))
                g.FillEllipse(paleBrush, 0, 0, 66, 66);
            using (SolidBrush accentBrush = new SolidBrush(accent))
                g.FillEllipse(accentBrush, 7, 7, 52, 52);

            using (SolidBrush whiteBrush = new SolidBrush(Color.White))
            using (Pen whitePen = new Pen(Color.White, 4F))
            {
                whitePen.StartCap = LineCap.Round;
                whitePen.EndCap = LineCap.Round;
                whitePen.LineJoin = LineJoin.Round;

                switch (_messageType)
                {
                    case LoginMessageType.Success:
                        g.DrawLine(whitePen, 19, 34, 28, 43);
                        g.DrawLine(whitePen, 28, 43, 47, 23);
                        break;
                    case LoginMessageType.InvalidCredentials:
                    case LoginMessageType.AccountLocked:
                        g.DrawLine(whitePen, 23, 23, 43, 43);
                        g.DrawLine(whitePen, 43, 23, 23, 43);
                        break;
                    case LoginMessageType.Confirmation:
                        using (Font questionFont = new Font("Segoe UI", 35F, FontStyle.Bold))
                        using (StringFormat questionFormat = new StringFormat
                        {
                            Alignment = StringAlignment.Center,
                            LineAlignment = StringAlignment.Center
                        })
                            g.DrawString("?", questionFont, whiteBrush, new RectangleF(7, 2, 52, 58), questionFormat);
                        break;
                    default:
                        using (Font f = new Font("Segoe UI", 35F, FontStyle.Bold))
                        using (StringFormat sf = new StringFormat
                        {
                            Alignment = StringAlignment.Center,
                            LineAlignment = StringAlignment.Center
                        })
                            g.DrawString("i", f, whiteBrush, new RectangleF(7, 2, 52, 58), sf);
                        break;
                }
            }
        }

        private void SetButtonRoundRegion(Button button, int radius)
        {
            if (button == null || button.Width <= 0 || button.Height <= 0)
                return;

            using (GraphicsPath path = CreateRoundedPath(
                new Rectangle(0, 0, button.Width, button.Height), radius))
            {
                button.Region?.Dispose();
                button.Region = new Region(path);
            }
        }

        private void FrmLoginMessage_KeyDown(object? sender, KeyEventArgs e)
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
                new Rectangle(0, 0, Width, Height), 15))
            {
                Region?.Dispose();
                Region = new Region(path);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ApplyRoundedCorners();
        }

        private Color GetAccentColor()
        {
            switch (_messageType)
            {
                case LoginMessageType.PasswordRequired:
                case LoginMessageType.AccountLocked:
                    return Color.FromArgb(239, 49, 57);
                case LoginMessageType.InvalidCredentials:
                    return Color.FromArgb(235, 145, 20);
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
                case LoginMessageType.PasswordRequired:
                case LoginMessageType.AccountLocked:
                    return Color.FromArgb(255, 229, 231);
                case LoginMessageType.InvalidCredentials:
                    return Color.FromArgb(255, 241, 220);
                case LoginMessageType.Success:
                    return Color.FromArgb(220, 252, 231);
                default:
                    return Color.FromArgb(211, 231, 255);
            }
        }

        private static GraphicsPath CreateRoundedPath(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = Math.Min(radius * 2, Math.Min(bounds.Width, bounds.Height));
            Rectangle arc = new Rectangle(bounds.X, bounds.Y, diameter, diameter);
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

        public static DialogResult ShowConfirmation(
            IWin32Window owner,
            string title,
            string description)
        {
            using (FrmLoginMessage dialog = new FrmLoginMessage(
                LoginMessageType.Confirmation, title, description))
            {
                return owner != null ? dialog.ShowDialog(owner) : dialog.ShowDialog();
            }
        }

        public static DialogResult ShowMessage(
            IWin32Window owner,
            LoginMessageType messageType,
            string title,
            string description)
        {
            using (FrmLoginMessage dialog =
                new FrmLoginMessage(messageType, title, description))
            {
                return owner != null ? dialog.ShowDialog(owner) : dialog.ShowDialog();
            }
        }
    }
}
