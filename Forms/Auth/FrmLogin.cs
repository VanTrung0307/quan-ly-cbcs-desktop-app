using QuanLyCBCS.Forms.PersonnelForms;
using QuanLyCBCS.Services;
using QuanLyCBCS.Models;
using QuanLyCBCS.Helpers;

namespace QuanLyCBCS.Forms.Auth
{
    public partial class FrmLogin : Form
    {
        private readonly UserService _userService;
        public FrmLogin()
        {
            InitializeComponent();

            _userService =
                new UserService();

            this.AcceptButton =
                btnLogin;

            this.CancelButton =
                btnExit;
        }

        private async void btnLogin_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                string username =
                    txtUsername.Text.Trim();

                string password =
                    txtPassword.Text.Trim();

                if (string.IsNullOrWhiteSpace(username))
                {
                    MessageBox.Show(
                        "Nhập tên đăng nhập");

                    return;
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show(
                        "Nhập mật khẩu");

                    return;
                }

                User? user =
                    await _userService
                        .LoginAsync(
                            username,
                            password);

                if (user == null)
                {
                    MessageBox.Show(
                        "Sai tài khoản hoặc mật khẩu");

                    return;
                }

                CurrentUser.UserId =
                    user.UserId;

                CurrentUser.Username =
                    user.Username;

                CurrentUser.Role =
                    user.Role;

                MessageBox.Show(
                    $"Xin chào {user.Username}",
                    "Đăng nhập thành công");

                FrmPersonnelList frm =
                    new FrmPersonnelList();

                this.Hide();

                frm.ShowDialog();

                this.Show();

                txtUsername.Clear();

                txtPassword.Clear();

                txtUsername.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Lỗi");
            }
        }

        private void btnExit_Click(
            object sender,
            EventArgs e)
        {
            Application.Exit();
        }
    }
}
