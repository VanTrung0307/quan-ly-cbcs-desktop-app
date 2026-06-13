using QuanLyCBCS.Helpers;
using QuanLyCBCS.Models;
using QuanLyCBCS.Services;

namespace QuanLyCBCS.Forms.UserForms
{
    public partial class FrmUserDetail : Form
    {
        private readonly UserService _userService = new();
        public FrmUserDetail()
        {
            InitializeComponent();

            PermissionHelper.CheckAdmin();

            cboRole.Items.Add("Admin");
            cboRole.Items.Add("User");

            _userService =
                new UserService();

            cboRole.SelectedIndex = 0;
        }

        private async void btnSave_Click(
            object sender,
            EventArgs e)
        {
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
                    Role = role
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
