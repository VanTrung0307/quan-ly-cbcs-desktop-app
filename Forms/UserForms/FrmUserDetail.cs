using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using QuanLyCBCS.Helpers;
using QuanLyCBCS.Forms.Auth;
using QuanLyCBCS.Models;
using QuanLyCBCS.Services;

namespace QuanLyCBCS.Forms.UserForms
{
    public partial class FrmUserDetail : Form
    {
        private readonly UserService _userService = new();

        protected override CreateParams CreateParams
        {
            get
            {
                const int CS_DROPSHADOW = 0x00020000;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        public FrmUserDetail()
        {
            InitializeComponent();

            if (!PermissionHelper.CanManageUsers)
            {
                MessageBox.Show(
                    "Bạn không có quyền quản lý tài khoản.",
                    "Từ chối truy cập",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                BeginInvoke(() => Close());
                return;
            }

            cboRole.Items.Add("Admin");
            cboRole.Items.Add("User");
            cboRole.SelectedIndex = 0;

            toolTipMain.SetToolTip(txtUsername, "Tên đăng nhập hệ thống");
            toolTipMain.SetToolTip(txtPassword, "Mật khẩu đăng nhập");
            toolTipMain.SetToolTip(cboRole, "Vai trò người dùng");
            toolTipMain.SetToolTip(btnSave, "Lưu thông tin");
            toolTipMain.SetToolTip(btnCancel, "Đóng cửa sổ");
            toolTipMain.SetToolTip(btnTogglePassword, "Hiện/ẩn mật khẩu");

            ApplyRoundedRegions();
        }

        private void ApplyRoundedRegions()
        {
            SetRoundedRegion(this, 16);
            SetRoundedRegion(panelHeader, 14);
            SetRoundedRegion(rowUsername, 12);
            SetRoundedRegion(rowPassword, 12);
            SetRoundedRegion(rowRole, 12);
            SetRoundedRegion(pnlUsernameInput, 6);
            SetRoundedRegion(pnlPasswordInput, 6);
            SetRoundedRegion(pnlRoleInput, 6);
            SetRoundedRegion(panelFooter, 12);
            SetRoundedRegion(btnSave, 9);
            SetRoundedRegion(btnCancel, 9);
        }

        private static void SetRoundedRegion(Control control, int radius)
        {
            if (control.Width <= 0 || control.Height <= 0)
                return;

            using GraphicsPath path = new();
            int d = radius * 2;
            Rectangle bounds = new(0, 0, control.Width - 1, control.Height - 1);
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            control.Region = new Region(path);
        }

        private void inputPanel_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is not Panel panel) return;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new(0, 0, panel.Width - 1, panel.Height - 1);
            using GraphicsPath path = RoundedRectangle(rect, 6);
            using Pen pen = new(Color.FromArgb(190, 215, 245), 1.2F);
            e.Graphics.DrawPath(pen, path);
        }

        private void btnCancel_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is not Button button) return;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new(1, 1, button.Width - 3, button.Height - 3);
            using GraphicsPath path = RoundedRectangle(rect, 9);
            using Pen pen = new(Color.FromArgb(239, 65, 65), 1.4F);
            e.Graphics.DrawPath(pen, path);
        }

        private static GraphicsPath RoundedRectangle(Rectangle bounds, int radius)
        {
            GraphicsPath path = new();
            int d = radius * 2;
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void panelHeader_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is not Panel panel)
                return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (LinearGradientBrush brush = new(
                panel.ClientRectangle,
                Color.FromArgb(0, 91, 190),
                Color.FromArgb(0, 65, 145),
                LinearGradientMode.Horizontal))
            {
                e.Graphics.FillRectangle(brush, panel.ClientRectangle);
            }

            // Watermark ngôi sao và vòng nguyệt quế mờ ở góc phải banner.
            using SolidBrush watermark = new(Color.FromArgb(34, 190, 220, 255));
            PointF[] star = CreateStar(
                new PointF(panel.Width - 92, 31), 23F, 10F, 5);
            e.Graphics.FillPolygon(watermark, star);

            using Pen laurelPen = new(Color.FromArgb(34, 190, 220, 255), 3F);
            e.Graphics.DrawArc(laurelPen, panel.Width - 139, 2, 65, 59, 105, 145);
            e.Graphics.DrawArc(laurelPen, panel.Width - 83, 2, 65, 59, -70, 145);

            for (int i = 0; i < 6; i++)
            {
                float y = 8 + i * 8;
                e.Graphics.DrawEllipse(laurelPen, panel.Width - 137 + i * 2, y, 8, 4);
                e.Graphics.DrawEllipse(laurelPen, panel.Width - 75 - i * 2, y, 8, 4);
            }
        }

        private static PointF[] CreateStar(PointF center, float outerRadius, float innerRadius, int points)
        {
            PointF[] vertices = new PointF[points * 2];
            for (int i = 0; i < vertices.Length; i++)
            {
                double angle = -Math.PI / 2 + i * Math.PI / points;
                float radius = i % 2 == 0 ? outerRadius : innerRadius;
                vertices[i] = new PointF(
                    center.X + (float)Math.Cos(angle) * radius,
                    center.Y + (float)Math.Sin(angle) * radius);
            }
            return vertices;
        }

        private void btnTogglePassword_Click(object? sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !txtPassword.UseSystemPasswordChar;
            btnTogglePassword.Text = txtPassword.UseSystemPasswordChar ? "\uE890" : "\uE7B3";
            txtPassword.Focus();
            txtPassword.SelectionStart = txtPassword.TextLength;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!PermissionHelper.CanManageUsers)
            {
                MessageBox.Show("Bạn không có quyền thực hiện chức năng này.");
                return;
            }

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string role = cboRole.Text;

            if (string.IsNullOrWhiteSpace(username))
            {
                FrmLoginMessage.ShowMessage(
                    this,
                    LoginMessageType.UsernameRequired,
                    "Thông báo",
                    "Nhập tên đăng nhập");

                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                FrmLoginMessage.ShowMessage(
                    this,
                    LoginMessageType.PasswordRequired,
                    "Thông báo",
                    "Nhập mật khẩu");

                txtPassword.Focus();
                return;
            }

            bool exists = await _userService.ExistsUsernameAsync(username);
            if (exists)
            {
                MessageBox.Show("Tên đăng nhập đã tồn tại");
                txtUsername.Focus();
                return;
            }

            User user = new()
            {
                Username = username,
                Password = PasswordHelper.HashPassword(password),
                Role = role,
                MustChangePassword = true
            };

            await _userService.AddAsync(user);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            Close();
        }
    }
}
