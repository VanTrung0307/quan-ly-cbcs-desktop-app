using QuanLyCBCS.Helpers;
using QuanLyCBCS.Models;
using QuanLyCBCS.Services;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QuanLyCBCS.Forms.UserForms
{
    public partial class FrmUserDetail : Form
    {
        private readonly UserService _userService = new();
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

            toolTipMain.SetToolTip(
                txtUsername,
                "Tên đăng nhập hệ thống");

            toolTipMain.SetToolTip(
                txtPassword,
                "Mật khẩu đăng nhập");

            toolTipMain.SetToolTip(
                cboRole,
                "Vai trò người dùng");

            toolTipMain.SetToolTip(
                btnSave,
                "Lưu thông tin");

            toolTipMain.SetToolTip(
                btnCancel,
                "Đóng cửa sổ");
        }

        private async void btnSave_Click(
            object sender,
            EventArgs e)
        {
            if (!PermissionHelper.CanManageUsers)
            {
                MessageBox.Show(
                    "Bạn không có quyền thực hiện chức năng này.");

                return;
            }

            string username =
                txtUsername.Text.Trim();

            string password =
                txtPassword.Text.Trim();

            string role =
                cboRole.Text;

            if (string.IsNullOrWhiteSpace(
                username))
            {
                MessageBox.Show(
                    "Nhập tên đăng nhập");

                return;
            }

            if (string.IsNullOrWhiteSpace(
                password))
            {
                MessageBox.Show(
                    "Nhập mật khẩu");

                return;
            }

            bool exists =
                await _userService
                    .ExistsUsernameAsync(
                        username);

            if (exists)
            {
                MessageBox.Show(
                    "Tên đăng nhập đã tồn tại");

                return;
            }

            User user =
                new User
                {
                    Username = username,
                    Password =
                        PasswordHelper.HashPassword(
                            password),
                    Role = role,
                    MustChangePassword = true
                };

            await _userService
                .AddAsync(user);

            DialogResult =
                DialogResult.OK;

            Close();
        }

        private void btnCancel_Click(
            object sender,
            EventArgs e)
        {
            Close();
        }
    }
}
