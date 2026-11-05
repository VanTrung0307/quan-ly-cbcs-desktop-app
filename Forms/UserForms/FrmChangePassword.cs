using QuanLyCBCS.Helpers;
using QuanLyCBCS.Forms.Auth;
using QuanLyCBCS.Services;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Color = System.Drawing.Color;
using Font = System.Drawing.Font;

namespace QuanLyCBCS.Forms.UserForms
{
    public partial class FrmChangePassword : Form
    {
        private readonly UserService _service;
        private readonly bool _isMandatoryChange;

        /// <summary>
        /// Mặc định dùng như một màn hình đổi mật khẩu thông thường.
        /// Khi isMandatoryChange = true, form hoạt động như hộp thoại modal
        /// và chỉ trả về OK sau khi đổi mật khẩu thành công.
        /// </summary>
        public FrmChangePassword() : this(false)
        {
        }

        public FrmChangePassword(bool isMandatoryChange)
        {
            InitializeComponent();
            _service = new UserService();
            _isMandatoryChange = isMandatoryChange;

            if (_isMandatoryChange)
            {
                FormBorderStyle = FormBorderStyle.FixedDialog;
                ControlBox = true;
                MinimizeBox = false;
                MaximizeBox = false;
                ShowInTaskbar = false;
                StartPosition = FormStartPosition.CenterParent;
                WindowState = FormWindowState.Normal;
                ClientSize = new Size(1200, 760);
                MinimumSize = new Size(1100, 700);
                Text = "Đổi mật khẩu bắt buộc";
            }

            toolTipMain.SetToolTip(txtOldPassword, "Mật khẩu hiện tại");
            toolTipMain.SetToolTip(txtNewPassword, "Mật khẩu mới");
            toolTipMain.SetToolTip(txtConfirmPassword, "Xác nhận mật khẩu mới");
            toolTipMain.SetToolTip(btnToggleOld, "Hiện/ẩn mật khẩu hiện tại");
            toolTipMain.SetToolTip(btnToggleNew, "Hiện/ẩn mật khẩu mới");
            toolTipMain.SetToolTip(btnToggleConfirm, "Hiện/ẩn mật khẩu xác nhận");
            toolTipMain.SetToolTip(btnSave, "Lưu thay đổi");
            toolTipMain.SetToolTip(btnCancel, "Xóa nội dung đã nhập");

            Resize += FrmChangePassword_Resize;
            pnlContent.Paint += pnlContent_Paint;
            pnlFormArea.SizeChanged += (s, e) => LayoutFormControls();
            Shown += (s, e) =>
            {
                LayoutMainCard();
                LayoutFormControls();
            };
            LayoutMainCard();
            LayoutFormControls();
        }

        private void pnlContent_Paint(object? sender, PaintEventArgs e)
        {
            using (var background = new LinearGradientBrush(pnlContent.ClientRectangle,
                Color.FromArgb(244, 249, 255), Color.FromArgb(224, 239, 255), 90F))
                e.Graphics.FillRectangle(background, pnlContent.ClientRectangle);
            if (pnlCard == null) return;
            Rectangle r = pnlCard.Bounds;
            for (int i = 8; i >= 1; i--)
            {
                using (var brush = new SolidBrush(Color.FromArgb(5 + (9 - i) * 2, 20, 65, 120)))
                    e.Graphics.FillRectangle(brush, Rectangle.Inflate(r, i, i));
            }
        }

        private void FrmChangePassword_Resize(object? sender, EventArgs e)
        {
            LayoutMainCard();
        }

        private void LayoutMainCard()
        {
            if (pnlCard == null || pnlContent == null) return;

            int availableWidth = Math.Max(600, pnlContent.ClientSize.Width - 48);
            int cardWidth = Math.Min(1155, availableWidth);
            int availableHeight = Math.Max(390, pnlContent.ClientSize.Height - 48);
            int cardHeight = Math.Min(455, availableHeight);

            pnlCard.Size = new Size(cardWidth, cardHeight);
            pnlCard.Location = new Point(
                Math.Max(12, (pnlContent.ClientSize.Width - cardWidth) / 2),
                Math.Max(12, (pnlContent.ClientSize.Height - cardHeight) / 2));

            // Khi cửa sổ hẹp, thu hẹp vùng minh họa để ưu tiên không gian nhập liệu.
            tableCard.ColumnStyles[0].Width = cardWidth < 900 ? 23F : 28F;
            tableCard.ColumnStyles[1].Width = 100F - tableCard.ColumnStyles[0].Width;
            pnlIllustration.Visible = cardWidth >= 760;
            if (cardWidth < 760)
            {
                tableCard.ColumnStyles[0].Width = 0F;
                tableCard.ColumnStyles[1].Width = 100F;
            }

            LayoutFormControls();
        }

        /// <summary>
        /// Tự căn lại chiều rộng các hàng nhập và cụm nút theo vùng FormArea,
        /// tránh TextBox bị tràn và giữ hai nút cân đối ở giữa.
        /// </summary>
        private void LayoutFormControls()
        {
            if (pnlFormArea == null || pnlOldRow == null) return;

            int rowWidth = Math.Max(420,
                pnlFormArea.ClientSize.Width - pnlFormArea.Padding.Left - pnlFormArea.Padding.Right);

            Panel[] rows = { pnlOldRow, pnlNewRow, pnlConfirmRow };
            foreach (Panel row in rows)
            {
                row.Left = 0;
                row.Width = rowWidth;
            }

            // Đưa cụm nút lên gần hàng nhập cuối, căn giữa theo chiều ngang.
            int buttonWidth = 228;
            int buttonGap = 34;
            int groupWidth = buttonWidth * 2 + buttonGap;
            int startX = Math.Max(0, (pnlFormArea.ClientSize.Width - groupWidth) / 2);
            // Hạ cụm nút xuống để tạo khoảng cách rõ ràng với hàng xác nhận mật khẩu.
            int buttonY = Math.Min(300, Math.Max(295, pnlFormArea.ClientSize.Height - 100));

            btnSave.Size = new Size(buttonWidth, 54);
            btnCancel.Size = new Size(buttonWidth, 54);
            btnSave.Location = new Point(startX, buttonY);
            btnCancel.Location = new Point(startX + buttonWidth + buttonGap, buttonY);

            // Cập nhật Region theo đúng kích thước hiện hành.
            btnSave.Region = new Region(RoundedRect(
                new Rectangle(0, 0, btnSave.Width - 1, btnSave.Height - 1), 12));
            btnCancel.Region = new Region(RoundedRect(
                new Rectangle(0, 0, btnCancel.Width - 1, btnCancel.Height - 1), 12));

            foreach (Panel row in rows)
                row.Invalidate();
        }

        private void btnSave_Paint(object? sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            const string glyph = "\uE74E";
            const string caption = "Lưu";
            using (var iconFont = new Font("Segoe MDL2 Assets", 17F, FontStyle.Regular))
            using (var textFont = new Font("Segoe UI", 12F, FontStyle.Bold))
            {
                Size iconSize = TextRenderer.MeasureText(glyph, iconFont, Size.Empty,
                    TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);
                Size textSize = TextRenderer.MeasureText(caption, textFont, Size.Empty,
                    TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);
                int gap = 9;
                int totalWidth = iconSize.Width + gap + textSize.Width;
                int x = (btnSave.ClientSize.Width - totalWidth) / 2;
                int yIcon = (btnSave.ClientSize.Height - iconSize.Height) / 2;
                int yText = (btnSave.ClientSize.Height - textSize.Height) / 2;
                TextRenderer.DrawText(e.Graphics, glyph, iconFont,
                    new Point(x, yIcon), Color.White,
                    TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);
                TextRenderer.DrawText(e.Graphics, caption, textFont,
                    new Point(x + iconSize.Width + gap, yText), Color.White,
                    TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);
            }
        }

        private void btnCancel_Paint(object? sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle borderBounds = new Rectangle(1, 1,
                btnCancel.ClientSize.Width - 3, btnCancel.ClientSize.Height - 3);
            using (var path = RoundedRect(borderBounds, 12))
            using (var pen = new Pen(Color.FromArgb(239, 54, 65), 1.4F))
                e.Graphics.DrawPath(pen, path);

            using (var iconPen = new Pen(Color.FromArgb(235, 47, 57), 2.1F))
            using (var textFont = new Font("Segoe UI", 12F, FontStyle.Bold))
            {
                iconPen.StartCap = LineCap.Round;
                iconPen.EndCap = LineCap.Round;
                const int iconSize = 13;
                const int gap = 10;
                Size textSize = TextRenderer.MeasureText("Huỷ", textFont, Size.Empty,
                    TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);
                int totalWidth = iconSize + gap + textSize.Width;
                int x = (btnCancel.ClientSize.Width - totalWidth) / 2;
                int y = (btnCancel.ClientSize.Height - iconSize) / 2;
                e.Graphics.DrawLine(iconPen, x, y, x + iconSize, y + iconSize);
                e.Graphics.DrawLine(iconPen, x + iconSize, y, x, y + iconSize);
                TextRenderer.DrawText(e.Graphics, "Huỷ", textFont,
                    new Point(x + iconSize + gap, (btnCancel.ClientSize.Height - textSize.Height) / 2),
                    Color.FromArgb(235, 47, 57),
                    TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);
            }
        }

        private void TogglePassword(TextBox textBox, Button button)
        {
            // UseSystemPasswordChar = true nghĩa là mật khẩu đang bị ẩn.
            textBox.UseSystemPasswordChar = !textBox.UseSystemPasswordChar;
            button.Invalidate(); // Vẽ lại mắt mở / mắt nhắm theo trạng thái mới.
            textBox.Focus();
            textBox.SelectionStart = textBox.TextLength;
        }

        /// <summary>
        /// Vẽ biểu tượng mắt bằng GDI+ để không phụ thuộc mã glyph của Segoe MDL2 Assets.
        /// Mật khẩu đang ẩn: mắt có gạch chéo; đang hiện: mắt mở.
        /// </summary>
        private void ToggleButton_Paint(object? sender, PaintEventArgs e)
        {
            if (!(sender is Button button) || !(button.Tag is TextBox textBox))
                return;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            float cx = button.ClientSize.Width / 2F;
            float cy = button.ClientSize.Height / 2F;
            const float eyeWidth = 19F;
            const float eyeHeight = 12F;

            using (var pen = new Pen(Color.FromArgb(45, 65, 95), 1.8F))
            using (var pupilBrush = new SolidBrush(Color.FromArgb(45, 65, 95)))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                using (var eye = new GraphicsPath())
                {
                    eye.AddBezier(cx - eyeWidth / 2F, cy,
                        cx - eyeWidth / 4F, cy - eyeHeight,
                        cx + eyeWidth / 4F, cy - eyeHeight,
                        cx + eyeWidth / 2F, cy);
                    eye.AddBezier(cx + eyeWidth / 2F, cy,
                        cx + eyeWidth / 4F, cy + eyeHeight,
                        cx - eyeWidth / 4F, cy + eyeHeight,
                        cx - eyeWidth / 2F, cy);
                    g.DrawPath(pen, eye);
                }
                g.FillEllipse(pupilBrush, cx - 2.2F, cy - 2.2F, 4.4F, 4.4F);

                if (textBox.UseSystemPasswordChar)
                {
                    using (var slashPen = new Pen(Color.FromArgb(45, 65, 95), 2F))
                    {
                        slashPen.StartCap = LineCap.Round;
                        slashPen.EndCap = LineCap.Round;
                        g.DrawLine(slashPen, cx - 12F, cy + 12F, cx + 12F, cy - 12F);
                    }
                }
            }
        }

        private void btnToggleOld_Click(object sender, EventArgs e) => TogglePassword(txtOldPassword, btnToggleOld);
        private void btnToggleNew_Click(object sender, EventArgs e) => TogglePassword(txtNewPassword, btnToggleNew);
        private void btnToggleConfirm_Click(object sender, EventArgs e) => TogglePassword(txtConfirmPassword, btnToggleConfirm);

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Ở chế độ bắt buộc đổi mật khẩu, Hủy phải đóng hộp thoại.
            // FrmLogin sẽ nhận DialogResult.Cancel và không mở FrmHome.
            if (_isMandatoryChange)
            {
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            txtOldPassword.Clear();
            txtNewPassword.Clear();
            txtConfirmPassword.Clear();
            txtOldPassword.UseSystemPasswordChar = true;
            txtNewPassword.UseSystemPasswordChar = true;
            txtConfirmPassword.UseSystemPasswordChar = true;
            btnToggleOld.Invalidate();
            btnToggleNew.Invalidate();
            btnToggleConfirm.Invalidate();
            txtOldPassword.Focus();
        }

        private void pnlPageHeader_Paint(object sender, PaintEventArgs e)
        {
            using (var brush = new LinearGradientBrush(pnlPageHeader.ClientRectangle,
                Color.FromArgb(0, 103, 205), Color.FromArgb(0, 67, 151), 0F))
                e.Graphics.FillRectangle(brush, pnlPageHeader.ClientRectangle);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var brush = new SolidBrush(Color.FromArgb(35, 255, 255, 255)))
            using (var path = new GraphicsPath())
            {
                path.AddEllipse(pnlPageHeader.Width - 220, -65, 245, 210);
                e.Graphics.FillPath(brush, path);
            }
        }

        private void pnlIllustration_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle area = pnlIllustration.ClientRectangle;
            int cx = area.Width / 2;
            int cy = area.Height / 2;

            using (var circleBrush = new SolidBrush(Color.FromArgb(235, 244, 255)))
                g.FillEllipse(circleBrush, cx - 132, cy - 132, 264, 264);

            Point[] shield = {
                new Point(cx, cy - 98), new Point(cx + 77, cy - 65),
                new Point(cx + 70, cy + 35), new Point(cx, cy + 104),
                new Point(cx - 70, cy + 35), new Point(cx - 77, cy - 65)
            };
            using (var shieldBrush = new LinearGradientBrush(
                new Rectangle(cx - 80, cy - 100, 160, 210),
                Color.FromArgb(53, 151, 255), Color.FromArgb(0, 74, 183), 90F))
            using (var shieldPen = new Pen(Color.FromArgb(0, 92, 205), 4F))
            {
                g.FillPolygon(shieldBrush, shield);
                g.DrawPolygon(shieldPen, shield);
            }

            // Khóa trắng ở giữa khiên.
            using (var lockPen = new Pen(Color.White, 8F))
            {
                g.DrawArc(lockPen, cx - 23, cy - 44, 46, 52, 180, 180);
                using (var lockPath = RoundedRect(new Rectangle(cx - 34, cy - 12, 68, 58), 8))
                    g.DrawPath(lockPen, lockPath);
            }
            using (var lockBrush = new SolidBrush(Color.White))
                g.FillEllipse(lockBrush, cx - 5, cy + 5, 10, 10);

            // Dải mật khẩu minh họa.
            Rectangle passwordBox = new Rectangle(cx - 72, cy + 52, 144, 38);
            using (var boxBrush = new SolidBrush(Color.White))
            using (var boxPen = new Pen(Color.FromArgb(170, 207, 246), 2F))
            using (var path = RoundedRect(passwordBox, 17))
            {
                g.FillPath(boxBrush, path);
                g.DrawPath(boxPen, path);
            }
            using (var starBrush = new SolidBrush(Color.FromArgb(0, 96, 205)))
            using (var starFont = new Font("Segoe UI", 19F, FontStyle.Bold))
                g.DrawString("*****", starFont, starBrush, cx - 48, cy + 53);

            using (var arrowPen = new Pen(Color.FromArgb(57, 151, 255), 9F))
            {
                arrowPen.StartCap = LineCap.Round;
                arrowPen.EndCap = LineCap.ArrowAnchor;
                g.DrawArc(arrowPen, cx + 25, cy - 118, 115, 100, 205, 125);
                g.DrawArc(arrowPen, cx - 140, cy + 20, 95, 105, 35, 120);
            }
        }

        private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_isMandatoryChange && DialogResult == DialogResult.None)
                DialogResult = DialogResult.Cancel;

            base.OnFormClosing(e);
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string oldPassword = txtOldPassword.Text.Trim();
                string newPassword = txtNewPassword.Text.Trim();
                string confirmPassword = txtConfirmPassword.Text.Trim();

                if (string.IsNullOrWhiteSpace(oldPassword) ||
                    string.IsNullOrWhiteSpace(newPassword) ||
                    string.IsNullOrWhiteSpace(confirmPassword))
                {
                    FrmLoginMessage.ShowMessage(this, LoginMessageType.Information,
                        "Thiếu thông tin", "Vui lòng nhập đầy đủ các trường mật khẩu.");
                    return;
                }

                if (newPassword != confirmPassword)
                {
                    FrmLoginMessage.ShowMessage(this, LoginMessageType.InvalidCredentials,
                        "Thông tin không hợp lệ", "Xác nhận mật khẩu không khớp.");
                    txtConfirmPassword.Focus();
                    txtConfirmPassword.SelectAll();
                    return;
                }

                if (oldPassword == newPassword)
                {
                    FrmLoginMessage.ShowMessage(this, LoginMessageType.Information,
                        "Mật khẩu không hợp lệ", "Mật khẩu mới phải khác mật khẩu hiện tại.");
                    txtNewPassword.Focus();
                    txtNewPassword.SelectAll();
                    return;
                }

                btnSave.Enabled = false;
                // Nội dung nút được vẽ hoàn toàn trong btnSave_Paint; giữ Text rỗng để tránh vẽ chồng.
                btnSave.Text = string.Empty;

                await _service.ChangePasswordAsync(CurrentUser.UserId, oldPassword, newPassword);
                await AuditHelper.LogAsync(AuditAction.ChangePassword,
                    $"Người dùng '{CurrentUser.Username}' đã đổi mật khẩu.");

                FrmLoginMessage.ShowMessage(this, LoginMessageType.Success,
                    "Thành công", "Đổi mật khẩu thành công.");

                if (_isMandatoryChange)
                {
                    // Chỉ trả về OK sau khi dịch vụ đổi mật khẩu hoàn tất thành công.
                    DialogResult = DialogResult.OK;
                    Close();
                    return;
                }

                btnCancel_Click(sender, e);
            }
            catch (Exception ex)
            {
                LoginMessageType messageType = ex.Message.IndexOf(
                    "mật khẩu", StringComparison.OrdinalIgnoreCase) >= 0
                    ? LoginMessageType.InvalidCredentials
                    : LoginMessageType.Information;

                FrmLoginMessage.ShowMessage(this, messageType,
                    "Không thể đổi mật khẩu", ex.Message);
            }
            finally
            {
                if (!IsDisposed)
                {
                    btnSave.Enabled = true;
                    // Không gán caption vào Text vì btnSave_Paint đã tự vẽ biểu tượng và chữ "Lưu".
                    btnSave.Text = string.Empty;
                    btnSave.Invalidate();
                }
            }
        }
    }
}