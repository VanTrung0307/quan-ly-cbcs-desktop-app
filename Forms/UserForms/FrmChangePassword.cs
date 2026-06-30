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

            toolTipMain.SetToolTip(
                txtOldPassword,
                "Mật khẩu hiện tại");

            toolTipMain.SetToolTip(
                txtNewPassword,
                "Mật khẩu mới");

            toolTipMain.SetToolTip(
                txtConfirmPassword,
                "Xác nhận mật khẩu mới");

            toolTipMain.SetToolTip(
                btnSave,
                "Lưu thay đổi");

            toolTipMain.SetToolTip(
                btnCancel,
                "Đóng cửa sổ");
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

                await AuditHelper.LogAsync(
                    AuditAction.ChangePassword,
                    $"Người dùng '{CurrentUser.Username}' đã đổi mật khẩu.");

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
