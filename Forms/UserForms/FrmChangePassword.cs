using QuanLyCBCS.Helpers;
using QuanLyCBCS.Services;

namespace QuanLyCBCS.Forms.UserForms
{
    public partial class FrmChangePassword : Form
    {
        private readonly UserService _service;

        public FrmChangePassword()
        {
            InitializeComponent();

            _service = new UserService();
        }

        private async void btnSave_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                if (txtNewPassword.Text
                    != txtConfirmPassword.Text)
                {
                    MessageBox.Show(
                        "Xác nhận mật khẩu không khớp.");

                    return;
                }

                await _service.ChangePasswordAsync(
                    CurrentUser.UserId,
                    txtOldPassword.Text.Trim(),
                    txtNewPassword.Text.Trim());

                AuditLogService auditService =
                    new AuditLogService();

                await auditService.AddLogAsync(
                    "CHANGE_PASSWORD",
                    CurrentUser.Username);

                MessageBox.Show(
                    "Đổi mật khẩu thành công.");

                DialogResult =
                    DialogResult.OK;

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Thông báo");
            }
        }
    }
}
