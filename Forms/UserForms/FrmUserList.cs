using QuanLyCBCS.Helpers;
using QuanLyCBCS.Services;
using QuanLyCBCS.Models;

namespace QuanLyCBCS.Forms.UserForms
{
    public partial class FrmUserList : Form
    {
        private readonly UserService _userService = new();

        public FrmUserList()
        {
            InitializeComponent();

            PermissionHelper.CheckAdmin();
        }

        private async Task
            LoadUsersAsync()
        {
            dgvUsers.DataSource = null;

            dgvUsers.DataSource =
                await _userService
                    .GetAllAsync();

            dgvUsers.Columns["Password"]
                .Visible = false;

            dgvUsers.Columns["UserId"]
                .HeaderText = "ID";

            dgvUsers.Columns["Username"]
                .HeaderText = "Tên đăng nhập";

            dgvUsers.Columns["Role"]
                .HeaderText = "Vai trò";
        }

        private async void FrmUserList_Load(
            object sender,
            EventArgs e)
        {
            await LoadUsersAsync();
        }

        private async void btnAddUser_Click(object sender, EventArgs e)
        {
            FrmUserDetail frm =
                new FrmUserDetail();

            if (frm.ShowDialog()
                == DialogResult.OK)
            {
                await LoadUsersAsync();
            }
        }

        private int GetSelectedUserId()
        {
            if (dgvUsers.CurrentRow == null)
                return 0;

            return Convert.ToInt32(
                dgvUsers.CurrentRow
                    .Cells["UserId"]
                    .Value);
        }

        private async void btnDeleteUser_Click(object sender, EventArgs e)
        {
            int userId =
        GetSelectedUserId();

            if (userId == 0)
                return;

            User? user =
                await _userService
                    .GetByIdAsync(userId);

            if (user == null)
                return;

            if (user.Username ==
                CurrentUser.Username)
            {
                MessageBox.Show(
                    "Không thể xóa tài khoản đang đăng nhập.");

                return;
            }

            if (user.Role == "Admin")
            {
                int adminCount =
                    await _userService
                        .GetAdminCountAsync();

                if (adminCount <= 1)
                {
                    MessageBox.Show(
                        "Không thể xóa Admin cuối cùng.");

                    return;
                }
            }

            DialogResult result =
                MessageBox.Show(
                    $"Xóa tài khoản {user.Username} ?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            await _userService
                .DeleteAsync(userId);

            await LoadUsersAsync();
        }

        private async void btnResetPassword_Click(
            object sender,
            EventArgs e)
        {
            int userId =
                GetSelectedUserId();

            if (userId == 0)
                return;

            User? user =
                await _userService
                    .GetByIdAsync(userId);

            if (user == null)
                return;

            DialogResult result =
                MessageBox.Show(
                    $"Reset mật khẩu của {user.Username} về 123456 ?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            await _userService
                .ResetPasswordAsync(
                    userId);

            MessageBox.Show(
                "Reset mật khẩu thành công.\nMật khẩu mới: 123456",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}
