using QuanLyCBCS.Helpers;
using QuanLyCBCS.Services;
using QuanLyCBCS.Models;

namespace QuanLyCBCS.Forms.UserForms
{
    public partial class FrmUserList : Form
    {
        private readonly UserService _userService = new();
        private readonly AuditLogService _auditService =
            new();
        private int _selectedUserId;

        public FrmUserList()
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
        }

        private async Task
            LoadUsersAsync()
        {
            SetupDataGridView();

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

            dgvUsers.Columns["FailedLoginCount"]
                .HeaderText = "Đăng nhập sai";

            dgvUsers.Columns["LockedUntil"]
                .HeaderText = "Khóa đến";

            dgvUsers.Columns["UserId"].Visible = false;

            dgvUsers.Columns["Role"]
                .DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            dgvUsers.Columns["FailedLoginCount"]
                .DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            dgvUsers.Columns["LockedUntil"]
                .DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            UpdateStatusBar();

            AddSttColumn();

            SetupTooltips();
        }

        private void SetupDataGridView()
        {
            DataGridViewHelper
                .SetupDefaultStyle(
                    dgvUsers);
        }

        private async void FrmUserList_Load(
            object sender,
            EventArgs e)
        {
            if (!PermissionHelper.CanManageUsers)
                return;

            await LoadUsersAsync();
        }

        private async void btnAddUser_Click(object sender, EventArgs e)
        {
            if (!PermissionHelper.CanManageUsers)
                return;

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
            if (!PermissionHelper.CanManageUsers)
                return;

            int userId =
                GetSelectedUserId();

            if (userId == 0)
                return;


            User? user =
                await _userService
                    .GetByIdAsync(userId);

            if (user == null)
                return;

            if (user.Username.Equals(
                "admin",
                StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(
                    "Không được phép xóa tài khoản Admin gốc!",
                    "Bảo mật hệ thống",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                await _auditService.AddLogAsync(
                    "PROTECTED_ACCOUNT",
                    $"{CurrentUser.Username} cố thao tác với Admin gốc");

                return;
            }

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
            if (!PermissionHelper.CanManageUsers)
                return;

            int userId =
                GetSelectedUserId();

            if (userId == 0)
                return;

            User? user =
                await _userService
                    .GetByIdAsync(userId);

            if (user == null)
                return;

            if (user.Username.Equals(
                "admin",
                StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(
                    "Không được reset mật khẩu Admin gốc!",
                    "Bảo mật hệ thống",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                await _auditService.AddLogAsync(
                    "PROTECTED_ACCOUNT",
                    $"{CurrentUser.Username} cố thao tác với Admin gốc");

                return;
            }

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

            await _auditService.AddLogAsync(
                "RESET_PASSWORD",
                $"Reset mật khẩu tài khoản ID: {_selectedUserId}");

            MessageBox.Show(
                "Reset mật khẩu thành công.\nMật khẩu mới: 123456",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void dgvUsers_CellFormatting(
            object sender,
            DataGridViewCellFormattingEventArgs e)
        {
            var style = e.CellStyle;

            if (style == null)
                return;

            if (e.RowIndex < 0)
                return;

            string columnName =
                dgvUsers.Columns[e.ColumnIndex]
                    .Name;

            if (columnName != "Role")
                return;

            if (columnName == "LockedUntil")
            {
                if (e.Value == null)
                {
                    e.Value = "Chưa khóa";
                    style.ForeColor =
                        Color.Gray;
                }
            }

            string role =
                e.Value?.ToString() ?? "";

            if (role == "Admin")
            {
                style.BackColor =
                    Color.FromArgb(39, 174, 96);

                style.ForeColor =
                    Color.White;

                style.SelectionBackColor =
                    Color.FromArgb(39, 174, 96);

                style.SelectionForeColor =
                    Color.White;
            }
            else
            {
                style.BackColor =
                    Color.FromArgb(52, 152, 219);

                style.ForeColor =
                    Color.White;

                style.SelectionBackColor =
                    Color.FromArgb(52, 152, 219);

                style.SelectionForeColor =
                    Color.White;
            }
        }

        private void UpdateStatusBar()
        {
            lblUserCount.Text =
                $"Đang hiển thị: {dgvUsers.Rows.Count} tài khoản";
        }

        private void AddSttColumn()
        {
            if (dgvUsers.Columns["STT"] == null)
            {
                DataGridViewTextBoxColumn sttColumn =
                    new DataGridViewTextBoxColumn();

                sttColumn.Name = "STT";
                sttColumn.HeaderText = "STT";
                sttColumn.Width = 50;
                sttColumn.ReadOnly = true;

                dgvUsers.Columns.Insert(
                    0,
                    sttColumn);
            }

            dgvUsers.Columns["STT"].Visible = true;
            dgvUsers.Columns["STT"].Frozen = true;

            for (int i = 0;
                 i < dgvUsers.Rows.Count;
                 i++)
            {
                dgvUsers.Rows[i]
                    .Cells["STT"]
                    .Value = i + 1;
            }
        }

        private void SetupTooltips()
        {
            toolTipMain.SetToolTip(
                btnAddUser,
                "Thêm tài khoản mới");

            toolTipMain.SetToolTip(
                btnDeleteUser,
                "Xóa vĩnh viễn tài khoản được chọn");

            toolTipMain.SetToolTip(
                btnResetPassword,
                "Đặt lại mật khẩu về: 123456");

            toolTipMain.SetToolTip(
                btnRefresh,
                "Tải lại danh sách tài khoản");

            toolTipMain.SetToolTip(
                dgvUsers,
                "Danh sách tài khoản hệ thống");
        }

        private async void btnRefresh_Click(
            object sender,
            EventArgs e)
        {
            await LoadUsersAsync();
        }

        private async void btnUnlock_Click(
            object sender,
            EventArgs e)
        {
            int userId = GetSelectedUserId();

            User? user =
            await _userService
                .GetByIdAsync(userId);

            if (user == null)
                return;

            if (user.Username.Equals(
                    "admin",
                    StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(
                    "Admin gốc không cần mở khóa.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            if (userId == 0)
            {
                MessageBox.Show(
                    "Vui lòng chọn tài khoản cần mở khóa!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            DialogResult confirm =
                MessageBox.Show(
                    "Bạn có chắc muốn mở khóa tài khoản này?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            await _userService.UnlockUserAsync(
                userId);

            await _auditService.AddLogAsync(
                "UNLOCK_ACCOUNT",
                $"Mở khóa tài khoản ID: {userId}");

            MessageBox.Show(
                "Mở khóa thành công!",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            await LoadUsersAsync();
        }

        private async void dgvUsers_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            _selectedUserId =
                Convert.ToInt32(
                    dgvUsers.Rows[e.RowIndex]
                        .Cells["UserId"]
                        .Value);

            string username =
                dgvUsers.Rows[e.RowIndex]
                    .Cells["Username"]
                    .Value?
                    .ToString() ?? "";

            btnDeleteUser.Enabled =
                !username.Equals(
                    "admin",
                    StringComparison.OrdinalIgnoreCase);

            btnResetPassword.Enabled =
                !username.Equals(
                    "admin",
                    StringComparison.OrdinalIgnoreCase);
        }
    }
}
