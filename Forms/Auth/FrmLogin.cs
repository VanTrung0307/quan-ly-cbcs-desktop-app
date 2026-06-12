using QuanLyCBCS.Data;
using QuanLyCBCS.Forms.PersonnelForms;
using QuanLyCBCS.Helpers;
using QuanLyCBCS.Models;
using QuanLyCBCS.Services;
using Microsoft.EntityFrameworkCore;

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
                using ApplicationDbContext db =
                    new ApplicationDbContext();

                User? user =
                    await db.Users
                        .FirstOrDefaultAsync(
                            x =>
                                x.Username ==
                                txtUsername.Text.Trim());

                if (user == null)
                {
                    MessageBox.Show(
                        "Sai tài khoản hoặc mật khẩu.");

                    return;
                }

                bool isValid =
                    PasswordHelper.VerifyPassword(
                        txtPassword.Text.Trim(),
                        user.Password);

                if (!isValid)
                {
                    MessageBox.Show(
                        "Sai tài khoản hoặc mật khẩu.");

                    return;
                }

                CurrentUser.UserId =
                    user.UserId;

                CurrentUser.Username =
                    user.Username;

                CurrentUser.Role =
                    user.Role;

                AuditLogService auditService =
                    new AuditLogService();

                await auditService
                    .AddLogAsync(
                        "LOGIN",
                        "Đăng nhập hệ thống");

                FrmPersonnelList frm =
                    new FrmPersonnelList();

                this.Hide();

                frm.ShowDialog();

                this.Show();

                txtUsername.Clear();

                txtPassword.Clear();

                txtUsername.Focus();
            }
            catch
            {
                MessageBox.Show(
                    "Đăng nhập thất bại.",
                    "Thông báo");
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
