using QuanLyCBCS.Helpers;
using QuanLyCBCS.Services;
using QuanLyCBCS.Models;
using QuanLyCBCS.Forms.Auth;

namespace QuanLyCBCS.Forms.UserForms
{
    public partial class FrmUserList : Form
    {
        private readonly UserService _userService = new();

        private int _selectedUserId;

        public FrmUserList()
        {
            InitializeComponent();

            // Bo góc các vùng chính để đồng nhất với thiết kế.
            ApplyRoundedRegion(pnlHeader, 12);
            ApplyRoundedRegion(pnlToolbar, 12);
            ApplyRoundedRegion(pnlStatus, 10);
            ApplyRoundedRegion(pnlGridSurface, 12);

            ApplyRoundedRegion(btnAddUser, 8);
            ApplyRoundedRegion(btnDeleteUser, 8);
            ApplyRoundedRegion(btnResetPassword, 8);
            ApplyRoundedRegion(btnRefresh, 8);
            ApplyRoundedRegion(btnUnlock, 8);

            // Đồng bộ icon theo cùng cách hiển thị với các nút còn lại:
            // dùng ký tự Unicode trực tiếp trong Text, không tạo Image riêng.
            btnAddUser.Text = "➕ Thêm tài khoản";
            btnAddUser.UseMnemonic = false;

            // Mở rộng nút để hiển thị trọn nhãn; đặt lại vị trí các nút sau
            // theo mép phải thực tế của nút đầu tiên để khoảng cách luôn đồng đều.
            const int desiredAddButtonWidth = 220;
            const int toolbarButtonGap = 8;
            int oldDeleteLeft = btnDeleteUser.Left;

            btnAddUser.Width = desiredAddButtonWidth;
            btnAddUser.TextAlign = ContentAlignment.MiddleCenter;
            btnAddUser.Padding = Padding.Empty;

            int newDeleteLeft = btnAddUser.Left + btnAddUser.Width + toolbarButtonGap;
            int shiftAfterAddButton = newDeleteLeft - oldDeleteLeft;

            btnDeleteUser.Left = newDeleteLeft;
            btnResetPassword.Left += shiftAfterAddButton;
            btnRefresh.Left += shiftAfterAddButton;
            btnUnlock.Left += shiftAfterAddButton;

            // Cập nhật Region sau khi đổi Width; nếu không, Region cũ từ Designer
            // sẽ cắt phần nút mới được mở rộng và làm chữ bị khuất.
            ApplyRoundedRegion(btnAddUser, 8);

            btnDeleteUser.Image = null;
            btnDeleteUser.Text = "🗑️ Xóa";
            btnDeleteUser.TextAlign = ContentAlignment.MiddleCenter;
            btnDeleteUser.TextImageRelation = TextImageRelation.Overlay;
            btnDeleteUser.UseMnemonic = false;

            btnRefresh.Image = null;
            btnRefresh.Text = "🔁 Làm mới";
            btnRefresh.TextAlign = ContentAlignment.MiddleCenter;
            btnRefresh.TextImageRelation = TextImageRelation.Overlay;
            btnRefresh.UseMnemonic = false;

            pnlHeader.SizeChanged += (_, _) => ApplyRoundedRegion(pnlHeader, 12);
            pnlToolbar.SizeChanged += (_, _) => ApplyRoundedRegion(pnlToolbar, 12);
            pnlStatus.SizeChanged += (_, _) => ApplyRoundedRegion(pnlStatus, 10);
            pnlGridSurface.SizeChanged += (_, _) => ApplyRoundedRegion(pnlGridSurface, 12);

            btnAddUser.SizeChanged += (_, _) => ApplyRoundedRegion(btnAddUser, 8);
            btnDeleteUser.SizeChanged += (_, _) => ApplyRoundedRegion(btnDeleteUser, 8);
            btnResetPassword.SizeChanged += (_, _) => ApplyRoundedRegion(btnResetPassword, 8);
            btnRefresh.SizeChanged += (_, _) => ApplyRoundedRegion(btnRefresh, 8);
            btnUnlock.SizeChanged += (_, _) => ApplyRoundedRegion(btnUnlock, 8);

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

        // Vẽ nền chuyển sắc và họa tiết huy hiệu chìm ở góc phải banner.
        private void pnlHeader_Paint(object? sender, PaintEventArgs e)
        {
            Rectangle bounds = pnlHeader.ClientRectangle;
            if (e.Graphics == null || bounds.Width <= 0 || bounds.Height <= 0)
                return;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                bounds,
                Color.FromArgb(0, 91, 184),
                Color.FromArgb(0, 69, 151),
                System.Drawing.Drawing2D.LinearGradientMode.Horizontal))
            {
                e.Graphics.FillRectangle(brush, bounds);
            }

            // Huy hiệu ngôi sao và vòng nguyệt quế dạng watermark.
            float cx = bounds.Right - 150;
            float cy = bounds.Height / 2f;
            using (var watermarkPen = new Pen(Color.FromArgb(34, 170, 210, 255), 5F))
            using (var leafPen = new Pen(Color.FromArgb(30, 170, 210, 255), 3F))
            using (var starBrush = new SolidBrush(Color.FromArgb(35, 190, 220, 255)))
            {
                e.Graphics.DrawArc(watermarkPen, cx - 61, cy - 37, 122, 88, 205, 130);
                e.Graphics.DrawArc(watermarkPen, cx - 61, cy - 37, 122, 88, 25, 130);

                for (int i = 0; i < 6; i++)
                {
                    float y = cy - 28 + i * 10;
                    e.Graphics.DrawEllipse(leafPen, cx - 68 + i * 3, y, 13, 7);
                    e.Graphics.DrawEllipse(leafPen, cx + 55 - i * 3, y, 13, 7);
                }

                PointF[] star =
                {
                    new PointF(cx, cy - 34), new PointF(cx + 10, cy - 10),
                    new PointF(cx + 35, cy - 10), new PointF(cx + 15, cy + 5),
                    new PointF(cx + 22, cy + 31), new PointF(cx, cy + 15),
                    new PointF(cx - 22, cy + 31), new PointF(cx - 15, cy + 5),
                    new PointF(cx - 35, cy - 10), new PointF(cx - 10, cy - 10)
                };
                e.Graphics.FillPolygon(starBrush, star);
            }
        }

        // Viền mảnh của vùng chứa bảng, thay cho đường viền đen mặc định.
        private void pnlGridSurface_Paint(object? sender, PaintEventArgs e)
        {
            Rectangle bounds = pnlGridSurface.ClientRectangle;
            if (e.Graphics == null || bounds.Width <= 1 || bounds.Height <= 1)
                return;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using (var pen = new Pen(Color.FromArgb(222, 232, 245), 1F))
            using (var path = CreateRoundedPath(new Rectangle(0, 0, bounds.Width - 1, bounds.Height - 1), 12))
            {
                e.Graphics.DrawPath(pen, path);
            }
        }

        // Nền sáng của thanh công cụ, có viền xanh rất nhẹ.
        private void pnlToolbar_Paint(object? sender, PaintEventArgs e)
        {
            Rectangle bounds = pnlToolbar.ClientRectangle;
            if (e.Graphics == null || bounds.Width <= 0 || bounds.Height <= 0)
                return;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                bounds,
                Color.FromArgb(250, 252, 255),
                Color.FromArgb(241, 247, 255),
                System.Drawing.Drawing2D.LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(brush, bounds);
            }

            using (var pen = new Pen(Color.FromArgb(220, 228, 240), 1F))
            using (var path = CreateRoundedPath(new Rectangle(0, 0, bounds.Width - 1, bounds.Height - 1), 12))
            {
                e.Graphics.DrawPath(pen, path);
            }
        }

        private static System.Drawing.Drawing2D.GraphicsPath CreateRoundedPath(Rectangle bounds, int radius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            int d = Math.Max(2, radius * 2);
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static void ApplyRoundedRegion(Control control, int radius)
        {
            if (control.Width <= 0 || control.Height <= 0)
                return;

            using (var path = CreateRoundedPath(
                new Rectangle(0, 0, control.Width, control.Height), radius))
            {
                Region? oldRegion = control.Region;
                control.Region = new Region(path);
                oldRegion?.Dispose();
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

            // Chỉ hiển thị các trường có trong bản thiết kế.
            foreach (DataGridViewColumn column in dgvUsers.Columns)
                column.Visible = false;

            string[] visibleColumns =
            {
                "Username", "Role", "FailedLoginCount", "LockedUntil",
                "MustChangePassword", "PermissionsCustomized"
            };

            foreach (string columnName in visibleColumns)
            {
                if (dgvUsers.Columns.Contains(columnName))
                    dgvUsers.Columns[columnName].Visible = true;
            }

            dgvUsers.Columns["Username"].HeaderText = "Tên đăng nhập";
            dgvUsers.Columns["Role"].HeaderText = "Vai trò";
            dgvUsers.Columns["FailedLoginCount"].HeaderText = "Đăng nhập sai";
            dgvUsers.Columns["LockedUntil"].HeaderText = "Khóa đến";
            if (dgvUsers.Columns.Contains("MustChangePassword"))
                dgvUsers.Columns["MustChangePassword"].HeaderText = "Bắt buộc đổi mật khẩu";
            if (dgvUsers.Columns.Contains("PermissionsCustomized"))
                dgvUsers.Columns["PermissionsCustomized"].HeaderText = "Tùy chỉnh quyền";

            // Tỷ lệ cột bám theo bản thiết kế: STT hẹp, Username và các cột cuối rộng hơn.
            dgvUsers.Columns["Username"].FillWeight = 160;
            dgvUsers.Columns["Role"].FillWeight = 148;
            dgvUsers.Columns["FailedLoginCount"].FillWeight = 142;
            dgvUsers.Columns["LockedUntil"].FillWeight = 143;
            if (dgvUsers.Columns.Contains("MustChangePassword"))
                dgvUsers.Columns["MustChangePassword"].FillWeight = 156;
            if (dgvUsers.Columns.Contains("PermissionsCustomized"))
                dgvUsers.Columns["PermissionsCustomized"].FillWeight = 163;

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
            DataGridViewHelper.SetupDefaultStyle(dgvUsers);

            dgvUsers.EnableHeadersVisualStyles = false;
            dgvUsers.BackgroundColor = Color.White;
            dgvUsers.BorderStyle = BorderStyle.None;
            dgvUsers.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvUsers.GridColor = Color.FromArgb(218, 230, 244);
            dgvUsers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvUsers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 72, 151);
            dgvUsers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUsers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvUsers.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvUsers.ColumnHeadersHeight = 51;
            dgvUsers.RowTemplate.Height = 52;
            dgvUsers.DefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvUsers.DefaultCellStyle.ForeColor = Color.FromArgb(0, 55, 110);
            dgvUsers.DefaultCellStyle.BackColor = Color.White;
            dgvUsers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(165, 210, 248);
            dgvUsers.DefaultCellStyle.SelectionForeColor = Color.FromArgb(0, 55, 110);
            dgvUsers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(235, 244, 253);
            dgvUsers.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(232, 241, 251);
            dgvUsers.RowHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 105, 210);
            dgvUsers.RowHeadersDefaultCellStyle.ForeColor = Color.FromArgb(0, 80, 160);
            dgvUsers.RowHeadersWidth = 48;
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
            dgvUsers.AllowUserToResizeRows = false;
            dgvUsers.MultiSelect = false;
            dgvUsers.ReadOnly = true;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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

                await AuditHelper.LogAsync(
                    AuditAction.ProtectedAccount,
                    $"Người dùng '{CurrentUser.Username}' cố gắng xóa tài khoản Admin gốc.");

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

            DialogResult result = FrmLoginMessage.ShowConfirmation(
                this,
                "Xác nhận xóa tài khoản",
                $"Bạn có chắc chắn muốn xóa tài khoản {user.Username}?");

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

                await AuditHelper.LogAsync(
                    AuditAction.ProtectedAccount,
                    $"Người dùng '{CurrentUser.Username}' cố gắng reset mật khẩu Admin gốc.");

                return;
            }

            DialogResult result = FrmLoginMessage.ShowConfirmation(
                this,
                "Xác nhận reset mật khẩu",
                $"Bạn có chắc chắn muốn reset mật khẩu của tài khoản {user.Username} về 123456?");

            if (result != DialogResult.Yes)
                return;

            await _userService
                .ResetPasswordAsync(
                    userId);

            FrmLoginMessage.ShowMessage(
                this,
                LoginMessageType.Success,
                "Reset mật khẩu thành công",
                $"Mật khẩu của tài khoản {user.Username} đã được đặt lại thành công.\\nMật khẩu mới: 123456");
        }

        private void dgvUsers_CellFormatting(
            object sender,
            DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            string columnName = dgvUsers.Columns[e.ColumnIndex].Name;

            if (columnName == "LockedUntil")
            {
                if (e.Value == null || e.Value == DBNull.Value ||
                    string.IsNullOrWhiteSpace(e.Value.ToString()))
                {
                    e.Value = string.Empty;
                    e.FormattingApplied = true;
                }
                else if (DateTime.TryParse(e.Value.ToString(), out DateTime lockedUntil))
                {
                    e.Value = lockedUntil.ToString("dd/MM/yyyy HH:mm");
                    e.FormattingApplied = true;
                }
            }

            if (columnName == "FailedLoginCount" ||
                columnName == "LockedUntil" ||
                columnName == "MustChangePassword" ||
                columnName == "PermissionsCustomized" ||
                columnName == "STT")
            {
                if (e.CellStyle != null)
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private void dgvUsers_CellPainting(
            object sender,
            DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 ||
                dgvUsers.Columns[e.ColumnIndex].Name != "Role")
                return;

            Graphics? graphics = e.Graphics;
            if (graphics == null)
                return;

            e.Handled = true;
            e.PaintBackground(e.CellBounds, true);

            string role = Convert.ToString(e.FormattedValue) ?? string.Empty;
            bool isAdmin = role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
            Color badgeColor = isAdmin
                ? Color.FromArgb(42, 183, 91)
                : Color.FromArgb(199, 230, 250);

            Rectangle badge = new Rectangle(
                e.CellBounds.X + 26,
                e.CellBounds.Y + 5,
                Math.Max(70, e.CellBounds.Width - 52),
                e.CellBounds.Height - 10);

            using (var path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                int radius = Math.Min(14, badge.Height / 2);
                int diameter = radius * 2;
                path.AddArc(badge.X, badge.Y, diameter, diameter, 180, 90);
                path.AddArc(badge.Right - diameter, badge.Y, diameter, diameter, 270, 90);
                path.AddArc(badge.Right - diameter, badge.Bottom - diameter, diameter, diameter, 0, 90);
                path.AddArc(badge.X, badge.Bottom - diameter, diameter, diameter, 90, 90);
                path.CloseFigure();

                using (var brush = new SolidBrush(badgeColor))
                    graphics.FillPath(brush, path);
            }

            string badgeText = isAdmin ? "♛  Admin" : "♟  User";
            Color textColor = isAdmin ? Color.White : Color.FromArgb(0, 65, 125);
            TextRenderer.DrawText(
                graphics,
                badgeText,
                new Font("Segoe UI", 10F, FontStyle.Bold),
                badge,
                textColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

            e.Paint(e.CellBounds, DataGridViewPaintParts.Border);
        }

        private void UpdateStatusBar()
        {
            int count = 0;
            foreach (DataGridViewRow row in dgvUsers.Rows)
            {
                if (!row.IsNewRow)
                    count++;
            }

            lblUserCount.Text =
                $"Đang hiển thị: {count} tài khoản";
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
            dgvUsers.Columns["STT"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvUsers.Columns["STT"].Width = 95;
            dgvUsers.Columns["STT"].MinimumWidth = 95;
            dgvUsers.Columns["STT"].DefaultCellStyle.Font =
                new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvUsers.Columns["STT"].DefaultCellStyle.ForeColor =
                Color.FromArgb(0, 55, 110);

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
                btnUnlock,
                "Mở khóa tài khoản đang được chọn");

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
            if (!PermissionHelper.CanManageUsers)
                return;

            int userId = GetSelectedUserId();
            if (userId == 0)
            {
                FrmLoginMessage.ShowMessage(
                    this,
                    LoginMessageType.Information,
                    "Thông báo",
                    "Vui lòng chọn tài khoản cần mở khóa!");

                return;
            }

            User? user =
                await _userService.GetByIdAsync(userId);

            if (user == null)
            {
                FrmLoginMessage.ShowMessage(
                    this,
                    LoginMessageType.Information,
                    "Thông báo",
                    "Không tìm thấy thông tin tài khoản cần mở khóa.");

                return;
            }

            if (user.Username.Equals(
                    "admin",
                    StringComparison.OrdinalIgnoreCase))
            {
                FrmLoginMessage.ShowMessage(
                    this,
                    LoginMessageType.Information,
                    "Thông báo",
                    "Admin gốc không cần mở khóa.");

                return;
            }

            DialogResult confirm =
                FrmLoginMessage.ShowConfirmation(
                    this,
                    "Xác nhận mở khóa",
                    $"Bạn có chắc chắn muốn mở khóa tài khoản {user.Username}?");

            if (confirm != DialogResult.Yes)
                return;

            await _userService.UnlockUserAsync(userId);

            FrmLoginMessage.ShowMessage(
                this,
                LoginMessageType.Success,
                "Mở khóa thành công",
                $"Tài khoản {user.Username} đã được mở khóa thành công.");

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