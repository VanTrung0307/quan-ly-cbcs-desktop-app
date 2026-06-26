using Microsoft.EntityFrameworkCore;
using QuanLyCBCS.Data;
using QuanLyCBCS.Forms.CommonForms;
using QuanLyCBCS.Forms.MissionForms;
using QuanLyCBCS.Forms.PersonnelForms;
using QuanLyCBCS.Forms.UserForms;
using QuanLyCBCS.Helpers;
using QuanLyCBCS.Models;
using QuanLyCBCS.Services;

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
            AuditLogService auditService =
                    new AuditLogService();
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
                    await auditService.AddLogAsync(
                        "FAILED_LOGIN",
                        $"Tài khoản không tồn tại: {username}");

                    MessageBox.Show(
                        "Sai tài khoản hoặc mật khẩu.");

                    return;
                }

                if (user.LockedUntil != null &&
                        user.LockedUntil > DateTime.Now)
                {
                    MessageBox.Show(
                        $"Tài khoản đang bị khóa đến: " +
                        user.LockedUntil.Value.ToString(
                            "dd/MM/yyyy HH:mm:ss"));

                    return;
                }

                bool isValid =
                    PasswordHelper.VerifyPassword(
                        txtPassword.Text.Trim(),
                        user.Password);

                if (!isValid)
                {
                    await auditService.AddLogAsync(
                        "FAILED_LOGIN",
                        $"Sai mật khẩu: {user.Username}");

                    user.FailedLoginCount++;

                    if (user.FailedLoginCount >= 5)
                    {
                        user.LockedUntil =
                            DateTime.Now.AddMinutes(15);

                        user.FailedLoginCount = 0;

                        await db.SaveChangesAsync();

                        await auditService.AddLogAsync(
                            "LOCK_ACCOUNT",
                            $"Tài khoản {user.Username} bị khóa 15 phút");

                        MessageBox.Show(
                            "Tài khoản đã bị khóa 15 phút do đăng nhập sai quá nhiều lần.");

                        return;
                    }

                    await db.SaveChangesAsync();

                    MessageBox.Show(
                        $"Sai tài khoản hoặc mật khẩu.\n" +
                        $"Lần sai: {user.FailedLoginCount}/5");

                    return;
                }

                CurrentUser.UserId =
                    user.UserId;

                CurrentUser.Username =
                    user.Username;

                CurrentUser.Role =
                    user.Role;

                PermissionService permissionService = new();

                CurrentUser.Permissions =
                    (await permissionService
                        .GetPermissionsAsync(user.Role))
                    .ToHashSet();

                List<string> permissions =
                    await permissionService.GetPermissionsAsync(user.Role);

                if (user.MustChangePassword)
                {
                    MessageBox.Show(
                        "Bạn phải đổi mật khẩu trước khi sử dụng hệ thống.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    using FrmChangePassword frmChange =
                        new FrmChangePassword();

                    if (frmChange.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    user =
                        await db.Users
                            .FirstOrDefaultAsync(
                                x => x.UserId == user.UserId);

                    if (user == null)
                    {
                        return;
                    }
                }

                user.FailedLoginCount = 0;
                user.LockedUntil = null;

                await db.SaveChangesAsync();

                await auditService
                    .AddLogAsync(
                        "LOGIN",
                        "Đăng nhập hệ thống");

                FrmLoading loading =
                    new FrmLoading();

                loading.Show();

                Application.DoEvents();

                await Task.Delay(300);

                FrmPersonnelList frm =
                    new FrmPersonnelList();

                loading.Hide();

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
