using QuanLyCBCS.DTOs;
using QuanLyCBCS.Helpers;
using QuanLyCBCS.Services;
using QuanLyCBCS.Models;
using QuanLyCBCS.Forms.Auth;
using System.Runtime.InteropServices;

namespace QuanLyCBCS.Forms.SystemForms
{
    public partial class FrmPermissionManagement : Form
    {
        // CheckedListBox native drawing can bypass DrawItem in some handle
        // recreation/layout scenarios. Override OnDrawItem so each row is
        // painted deterministically by this form, including the zebra bands.
        private sealed class PermissionCheckedListBox : CheckedListBox
        {
            public Action<DrawItemEventArgs>? CustomDrawItem { get; set; }

            protected override void OnDrawItem(DrawItemEventArgs e)
            {
                if (CustomDrawItem != null)
                    CustomDrawItem(e);
                else
                    base.OnDrawItem(e);
            }
        }

        // Win32 ListBox messages: CheckedListBox is backed by a native list box.
        // Applying LB_SETITEMHEIGHT after handle creation guarantees the native
        // rows use the calculated height (not merely the managed ItemHeight value).
        private const int LB_SETITEMHEIGHT = 0x01A0;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(
            IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
        private List<User> _users = new();

        private bool _initializingUsers;

        private int _loadedUserId;

        private readonly PermissionService _service =
            new PermissionService();

        private List<PermissionDto> _permissions =
            new();

        private bool _loading;

        private HashSet<int> _originalPermissions = new();

        private bool _isDirty;

        private readonly Stack<PermissionState> _undoStack = new();
        private readonly Stack<PermissionState> _redoStack = new();

        private bool _isUndoRedo = false;

        private PermissionState? _pendingUndoState;

        // Bộ tìm kiếm tài khoản tùy chỉnh; không sử dụng popup AutoComplete mặc định của WinForms.
        private TextBox _txtAccountSearch = null!;
        private Button _btnAccountDropdown = null!;
        private Panel _pnlAccountDropdown = null!;
        private ListBox _lstAccountSuggestions = null!;
        private bool _updatingAccountSearch;

        public FrmPermissionManagement()
        {
            InitializeComponent();
            ConfigurePermissionVisuals();

            // Tính lại sau khi Form đã hiển thị và hoàn tất layout lần đầu.
            // Điều này tránh lấy ClientSize quá sớm (khi control chưa có kích thước cuối).
            Shown += (sender, e) => QueuePermissionRowLayout();
            Layout += (sender, e) => QueuePermissionRowLayout();
            clbPermission.HandleCreated += (sender, e) => QueuePermissionRowLayout();

            // ComboBox chỉ giữ dữ liệu và tài khoản đang chọn ở chế độ ẩn.
            // Giao diện nhập/tìm và danh sách gợi ý được dựng riêng để không
            // xuất hiện popup mặc định của WinForms.
            cboRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cboRole.AutoCompleteMode = AutoCompleteMode.None;
            cboRole.AutoCompleteSource = AutoCompleteSource.None;
            cboRole.Visible = false;

            lblRole.Text = "Tài khoản:";
            InitializeAccountSearchDropdown();

            // Không dùng SelectionChangeCommitted của ComboBox ẩn.
            cboRole.SelectedIndexChanged -= cboRole_SelectedIndexChanged;
            cboRole.SelectionChangeCommitted -= cboRole_SelectedIndexChanged;
        }

        private void InitializeAccountSearchDropdown()
        {
            _txtAccountSearch = new TextBox
            {
                BorderStyle = BorderStyle.None,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(31, 52, 79),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                // Chừa khoảng cách rõ ràng sau icon người dùng và trước nút mũi tên.
                Location = new Point(44, 7),
                Size = new Size(Math.Max(60, pnlAccountSelector.ClientSize.Width - 80), 22),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                TabIndex = 1
            };

            _btnAccountDropdown = new Button
            {
                Text = "▾",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 65, 90),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                UseVisualStyleBackColor = false,
                Size = new Size(30, 28),
                Dock = DockStyle.Right,
                Margin = Padding.Empty,
                TabStop = false
            };

            _pnlAccountDropdown = new Panel
            {
                Visible = false,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Size = new Size(pnlAccountSelector.Width, 0),
                Location = new Point(pnlAccountSelector.Left, pnlAccountSelector.Bottom + 2),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            _lstAccountSuggestions = new ListBox
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(31, 52, 79),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                IntegralHeight = false,
                ItemHeight = 27,
                SelectionMode = SelectionMode.One
            };

            _lstAccountSuggestions.DrawMode = DrawMode.OwnerDrawFixed;
            _lstAccountSuggestions.DrawItem += (sender, e) =>
            {
                if (e.Index < 0) return;
                bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
                Color back = selected ? Color.FromArgb(0, 105, 205) : Color.White;
                Color fore = selected ? Color.White : Color.FromArgb(31, 52, 79);
                using (var brush = new SolidBrush(back)) e.Graphics.FillRectangle(brush, e.Bounds);
                TextRenderer.DrawText(e.Graphics,
                    _lstAccountSuggestions.Items[e.Index]?.ToString() ?? string.Empty,
                    _lstAccountSuggestions.Font, e.Bounds, fore,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.LeftAndRightPadding);
            };

            _pnlAccountDropdown.Controls.Add(_lstAccountSuggestions);
            pnlAccountCard.Controls.Add(_pnlAccountDropdown);

            // Ẩn ComboBox lưu dữ liệu và thay bằng textbox + nút mũi tên giao diện.
            _txtAccountSearch.TextChanged += (sender, e) =>
            {
                if (_updatingAccountSearch) return;
                FilterAccountSuggestions(_txtAccountSearch.Text);
                ShowAccountDropdown();
            };
            _txtAccountSearch.Enter += (sender, e) =>
            {
                _txtAccountSearch.SelectAll();
                FilterAccountSuggestions(string.Empty);
                ShowAccountDropdown();
            };
            _txtAccountSearch.Click += (sender, e) =>
            {
                if (!_pnlAccountDropdown.Visible)
                {
                    FilterAccountSuggestions(string.Empty);
                    ShowAccountDropdown();
                }
            };
            _txtAccountSearch.KeyDown += AccountSearch_KeyDown;
            _btnAccountDropdown.Click += (sender, e) =>
            {
                if (_pnlAccountDropdown.Visible)
                    HideAccountDropdown();
                else
                {
                    FilterAccountSuggestions(string.Empty);
                    ShowAccountDropdown();
                    _txtAccountSearch.Focus();
                }
            };
            _lstAccountSuggestions.MouseClick += (sender, e) => SelectSuggestedAccount();
            _lstAccountSuggestions.KeyDown += (sender, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    SelectSuggestedAccount();
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    HideAccountDropdown();
                    _txtAccountSearch.Focus();
                }
            };

            // Giữ icon người dùng hiện có, thay riêng phần ComboBox bằng giao diện mới.
            pnlAccountSelector.Controls.Remove(cboRole);
            // Đưa ComboBox ẩn về Form để vẫn được quản lý vòng đời/dispose.
            cboRole.Location = new Point(-200, -200);
            cboRole.Size = new Size(1, 1);
            Controls.Add(cboRole);
            cboRole.Visible = false;
            pnlAccountSelector.Controls.Add(_txtAccountSearch);
            pnlAccountSelector.Controls.Add(_btnAccountDropdown);
            lblAccountUserIcon.BringToFront();
            _txtAccountSearch.BringToFront();
            _btnAccountDropdown.BringToFront();

            pnlAccountSelector.Resize += (sender, e) =>
            {
                LayoutAccountSearchControls();
                PositionAccountDropdown();
            };
            pnlAccountSelector.Layout += (sender, e) => LayoutAccountSearchControls();
            LayoutAccountSearchControls();
            pnlAccountCard.Resize += (sender, e) => PositionAccountDropdown();
        }

        private void LayoutAccountSearchControls()
        {
            if (_txtAccountSearch == null || _btnAccountDropdown == null || pnlAccountSelector == null)
                return;

            // Neo nut bang DockStyle.Right de WinForms tu neo vao mep phai
            // thuc te cua o chon, khong phu thuoc toa do tinh/DPI scaling.
            // Padding phai nho de nut gan sat vien phai cua o tim kiem.
            pnlAccountSelector.Padding = new Padding(5, 2, 1, 2);
            _btnAccountDropdown.Dock = DockStyle.Right;
            _btnAccountDropdown.Width = 30;
            _btnAccountDropdown.Margin = Padding.Empty;
            _btnAccountDropdown.Height = Math.Max(26, pnlAccountSelector.ClientSize.Height - 4);

            const int textLeft = 44;
            _txtAccountSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _txtAccountSearch.Location = new Point(textLeft, 7);
            _txtAccountSearch.Width = Math.Max(60, _btnAccountDropdown.Left - textLeft - 6);
            _txtAccountSearch.Height = 22;
            _btnAccountDropdown.BringToFront();
        }

        private void SetAccountSearchText(string value)
        {
            if (_txtAccountSearch == null) return;
            _updatingAccountSearch = true;
            _txtAccountSearch.Text = value;
            _txtAccountSearch.SelectionStart = _txtAccountSearch.TextLength;
            _updatingAccountSearch = false;
        }

        private void FilterAccountSuggestions(string keyword)
        {
            if (_lstAccountSuggestions == null) return;
            string filter = (keyword ?? string.Empty).Trim();
            var matches = _users
                .Where(user => !string.Equals(
                    user.Username, "admin", StringComparison.OrdinalIgnoreCase))
                .Where(user => string.IsNullOrWhiteSpace(filter) ||
                    (user.Username ?? string.Empty).IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0)
                .OrderBy(user => user.Username)
                .ToList();

            _lstAccountSuggestions.BeginUpdate();
            _lstAccountSuggestions.Items.Clear();
            foreach (User user in matches)
                _lstAccountSuggestions.Items.Add(user.Username);
            _lstAccountSuggestions.EndUpdate();

            int rows = Math.Min(matches.Count, 6);
            _pnlAccountDropdown.Height = rows == 0 ? 0 : rows * _lstAccountSuggestions.ItemHeight + 4;
            PositionAccountDropdown();
        }

        private void PositionAccountDropdown()
        {
            if (_pnlAccountDropdown == null || pnlAccountSelector == null) return;
            _pnlAccountDropdown.Width = pnlAccountSelector.Width;
            _pnlAccountDropdown.Left = pnlAccountSelector.Left;
            _pnlAccountDropdown.Top = pnlAccountSelector.Bottom + 2;
        }

        private void ShowAccountDropdown()
        {
            if (_users.Count == 0 || _lstAccountSuggestions.Items.Count == 0)
            {
                HideAccountDropdown();
                return;
            }
            PositionAccountDropdown();
            _pnlAccountDropdown.Visible = true;
            _pnlAccountDropdown.BringToFront();
        }

        private void HideAccountDropdown()
        {
            if (_pnlAccountDropdown != null)
                _pnlAccountDropdown.Visible = false;
        }

        private void AccountSearch_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down && _lstAccountSuggestions.Items.Count > 0)
            {
                _lstAccountSuggestions.Focus();
                _lstAccountSuggestions.SelectedIndex = 0;
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter && _lstAccountSuggestions.Items.Count > 0)
            {
                _lstAccountSuggestions.SelectedIndex = 0;
                SelectSuggestedAccount();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                HideAccountDropdown();
                e.Handled = true;
            }
        }

        private void SelectSuggestedAccount()
        {
            int index = _lstAccountSuggestions.SelectedIndex;
            if (index < 0 || index >= _lstAccountSuggestions.Items.Count) return;

            string username = _lstAccountSuggestions.Items[index]?.ToString() ?? string.Empty;
            User? selectedUser = _users.FirstOrDefault(
                user => string.Equals(user.Username, username, StringComparison.OrdinalIgnoreCase));
            if (selectedUser == null) return;

            _initializingUsers = true;
            try
            {
                cboRole.SelectedItem = selectedUser;
            }
            finally
            {
                _initializingUsers = false;
            }

            SetAccountSearchText(selectedUser.Username);
            HideAccountDropdown();
            cboRole_SelectedIndexChanged(this, EventArgs.Empty);
            // Nếu người dùng từ chối bỏ thay đổi chưa lưu, handler khôi phục tài khoản cũ.
            SetAccountSearchText((cboRole.SelectedItem as User)?.Username ?? string.Empty);
            _txtAccountSearch.Focus();
        }

        private async void FrmPermissionManagement_Load(
            object sender,
            EventArgs e)
        {
            try
            {
                PermissionHelper.Require("Permission.Manage");
            }
            catch (UnauthorizedAccessException ex)
            {
                FrmLoginMessage.ShowMessage(
                    this,
                    LoginMessageType.Information,
                    "Thông báo",
                    ex.Message);

                Close();
                return;
            }

            UpdateSaveButton();

            await LoadUsersAsync();
        }

        private async Task LoadUsersAsync()
        {
            _initializingUsers = true;

            try
            {
                // Tài khoản admin mặc định có toàn quyền hệ thống, không quản lý
                // phân quyền tại màn hình này và không được xuất hiện trong ô chọn.
                _users = (await _service.GetUsersAsync())
                    .Where(user => !string.Equals(
                        user.Username,
                        "admin",
                        StringComparison.OrdinalIgnoreCase))
                    .OrderBy(user => user.Username)
                    .ToList();

                cboRole.DataSource = null;
                cboRole.DisplayMember = nameof(User.Username);
                cboRole.ValueMember = nameof(User.UserId);
                cboRole.DataSource = _users;

                if (_users.Count == 0)
                {
                    clbPermission.Items.Clear();
                    btnSave.Enabled = false;
                    _loadedUserId = 0;
                    return;
                }

                // Giữ tài khoản đang chọn nếu tài khoản đó vẫn còn tồn tại.
                User? previousUser = _users.FirstOrDefault(
                    x => x.UserId == _loadedUserId);

                cboRole.SelectedItem = previousUser ?? _users[0];
                SetAccountSearchText((cboRole.SelectedItem as User)?.Username ?? string.Empty);
            }
            finally
            {
                _initializingUsers = false;
            }

            // DataSource vừa được nạp bằng code nên tự tải quyền ở đây.
            await LoadPermissionAsync();
        }

        private async void cboRole_SelectedIndexChanged(
            object? sender,
            EventArgs e)
        {
            if (_initializingUsers || _loading)
                return;

            if (cboRole.SelectedItem is not User selectedUser)
                return;

            // Nếu đang có thay đổi chưa lưu, hỏi trước khi chuyển tài khoản.
            if (_isDirty)
            {
                DialogResult result = FrmLoginMessage.ShowMessage(
                    this,
                    LoginMessageType.Information,
                    "Xác nhận chuyển tài khoản",
                    "Bạn có thay đổi quyền chưa lưu. " +
                    "Bạn có muốn bỏ các thay đổi đó để chuyển tài khoản không?",
                    MessageBoxButtons.YesNo);

                if (result != DialogResult.Yes)
                {
                    // Quay lại tài khoản đang được chỉnh trước đó.
                    _initializingUsers = true;

                    try
                    {
                        User? previousUser = _users.FirstOrDefault(
                            x => x.UserId == _loadedUserId);

                        if (previousUser != null)
                            cboRole.SelectedItem = previousUser;
                    }
                    finally
                    {
                        _initializingUsers = false;
                    }

                    return;
                }
            }

            await LoadPermissionAsync();
        }

        private async Task LoadPermissionAsync()
        {
            if (cboRole.SelectedItem is not User selectedUser)
                return;

            _loading = true;

            try
            {
                _permissions = await _service.GetUserPermissionsAsync(
                    selectedUser.UserId);

                _loadedUserId = selectedUser.UserId;

                // Xóa và nạp đủ item trước khi tính lại chiều cao.
                // Nạp đầy đủ item trước khi tính chiều cao từng dòng để phép chia
                // luôn dựa trên đúng số quyền đang hiển thị.
                clbPermission.BeginUpdate();
                try
                {
                    clbPermission.Items.Clear();

                    foreach (PermissionDto permission in _permissions)
                    {
                        int index = clbPermission.Items.Add(
                            $"{permission.PermissionName} " +
                            $"({permission.PermissionKey})");

                        clbPermission.SetItemChecked(
                            index,
                            permission.IsGranted);
                    }
                }
                finally
                {
                    clbPermission.EndUpdate();
                }

                // CheckedListBox đôi khi giữ native renderer mặc định sau khi
                // Items được nạp động. Buộc tạo lại chế độ OwnerDrawFixed sau
                // khi nạp dữ liệu để Windows phát sinh DrawItem cho từng dòng.
                ReinitializePermissionOwnerDraw();

                // Chỉ tính lại chiều cao sau khi danh sách đã có đầy đủ item.
                int permissionCount = clbPermission.Items.Count;
                int loadedUserId = selectedUser.UserId;
                UpdatePermissionItemHeight(permissionCount);
                QueuePermissionRowLayout();

                // Chạy lại sau layout để lấy ClientSize cuối cùng của bảng.
                if (IsHandleCreated)
                {
                    BeginInvoke(new Action(() =>
                    {
                        if (!IsDisposed && !clbPermission.IsDisposed &&
                            _loadedUserId == loadedUserId)
                        {
                            UpdatePermissionItemHeight(permissionCount);
                        }
                    }));
                }

                // Lưu trạng thái ban đầu để xác định có thay đổi hay không.
                _originalPermissions = _permissions
                    .Where(x => x.IsGranted)
                    .Select(x => x.PermissionId)
                    .ToHashSet();

                _isDirty = false;

                _undoStack.Clear();
                _redoStack.Clear();

                _undoStack.Push(CaptureState());

                UpdateUndoRedoButtons();
                UpdateSaveButton();
            }
            catch (Exception ex)
            {
                FrmLoginMessage.ShowMessage(
                    this,
                    LoginMessageType.InvalidCredentials,
                    "Không thể tải quyền",
                    ex.Message);
            }
            finally
            {
                _loading = false;
            }
        }

        private async void btnRefresh_Click(
            object sender,
            EventArgs e)
        {
            await LoadUsersAsync();
        }

        private async Task<bool> SavePermissionsAsync()
        {
            if (cboRole.SelectedItem is not User selectedUser)
                return false;

            try
            {
                btnSave.Enabled = false;

                List<int> selectedPermissions = new();

                for (int i = 0; i < clbPermission.Items.Count; i++)
                {
                    if (clbPermission.GetItemChecked(i))
                    {
                        selectedPermissions.Add(
                            _permissions[i].PermissionId);
                    }
                }

                // Lưu theo UserId, không lưu theo Role.
                await _service.SaveUserPermissionsAsync(
                    selectedUser.UserId,
                    selectedPermissions);

                _originalPermissions = selectedPermissions.ToHashSet();
                _isDirty = false;

                _undoStack.Clear();
                _redoStack.Clear();
                _undoStack.Push(CaptureState());

                UpdateUndoRedoButtons();
                UpdateSaveButton();

                FrmLoginMessage.ShowMessage(
                    this,
                    LoginMessageType.Success,
                    "Lưu phân quyền thành công",
                    $"Đã lưu phân quyền riêng cho tài khoản '{selectedUser.Username}'.");

                return true;
            }
            catch (Exception ex)
            {
                FrmLoginMessage.ShowMessage(
                    this,
                    LoginMessageType.InvalidCredentials,
                    "Lỗi lưu phân quyền",
                    ex.Message);

                UpdateSaveButton();
                return false;
            }
        }

        private async void btnSave_Click(
            object sender,
            EventArgs e)
        {
            await SavePermissionsAsync();
        }

        private void ConfigurePermissionVisuals()
        {
            // Icon danh sách được vẽ vector thay cho ký tự font, bảo đảm giống
            // biểu tượng ba dòng trong bản thiết kế và không phụ thuộc font máy.
            lblPermissionHeaderIcon.Text = string.Empty;
            lblPermissionHeaderIcon.Paint += (sender, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (Pen linePen = new Pen(Color.FromArgb(0, 91, 181), 2.8F))
                using (SolidBrush squareBrush = new SolidBrush(Color.FromArgb(0, 91, 181)))
                {
                    linePen.StartCap = System.Drawing.Drawing2D.LineCap.Square;
                    linePen.EndCap = System.Drawing.Drawing2D.LineCap.Square;
                    int centerY = lblPermissionHeaderIcon.ClientSize.Height / 2;
                    int startX = 12;
                    for (int i = -1; i <= 1; i++)
                    {
                        int y = centerY + i * 10;
                        g.FillRectangle(squareBrush, startX, y - 2, 5, 5);
                        g.DrawLine(linePen, startX + 10, y, startX + 31, y);
                    }
                }
            };

            ConfigureRoundedButton(btnUndo, Color.White, Color.FromArgb(230, 241, 255), Color.FromArgb(0, 91, 181));
            ConfigureRoundedButton(btnRedo, Color.White, Color.FromArgb(230, 241, 255), Color.FromArgb(45, 65, 90));
            ConfigureRoundedButton(btnSave, Color.FromArgb(0, 105, 205), Color.FromArgb(0, 84, 170), Color.White);
            ConfigureRoundedButton(btnRefresh, Color.White, Color.FromArgb(230, 241, 255), Color.FromArgb(0, 91, 181));

            // Bảo đảm danh sách luôn dùng chế độ tự vẽ để hiển thị checkbox và
            // nền xanh/trắng xen kẽ, không rơi về giao diện mặc định của Windows.
            // CheckedListBox sử dụng OwnerDrawFixed để áp dụng chiều cao đồng đều.
            // Dùng một ItemHeight chung cho cả 12 dòng để phủ gần kín vùng bảng;
            // không dùng OwnerDrawVariable/MeasureItem vì CheckedListBox không
            // hỗ trợ chiều cao biến thiên ổn định như ListBox thông thường.
            clbPermission.IntegralHeight = false;
            EnsurePermissionOwnerDraw();
            clbPermission.Resize += (sender, e) => QueuePermissionRowLayout();

            // CheckedListBox có thể tạo lại native handle khi layout/DPI thay đổi.
            // Gắn lại chế độ OwnerDraw và sự kiện vẽ sau mỗi lần handle được tạo,
            // tránh trường hợp control quay về checkbox/nền mặc định của Windows.
            clbPermission.HandleCreated += (sender, e) =>
            {
                EnsurePermissionOwnerDraw();
                clbPermission.Invalidate(true);
            };
        }

        private void EnsurePermissionOwnerDraw()
        {
            if (clbPermission == null || clbPermission.IsDisposed)
                return;

            clbPermission.IntegralHeight = false;
            clbPermission.DrawMode = DrawMode.OwnerDrawFixed;

            if (clbPermission is PermissionCheckedListBox customList)
                customList.CustomDrawItem = e => clbPermission_DrawItem(clbPermission, e);
        }

        private void ReinitializePermissionOwnerDraw()
        {
            if (clbPermission == null || clbPermission.IsDisposed)
                return;

            // Không chuyển qua DrawMode.Normal vì thao tác đó có thể làm
            // CheckedListBox đồng bộ lại renderer native và bỏ qua màu nền.
            EnsurePermissionOwnerDraw();
            clbPermission.BackColor = Color.White;
            clbPermission.Invalidate(true);
            clbPermission.Update();
        }

        private void ConfigureRoundedButton(Button button, Color normalColor, Color hoverColor, Color normalTextColor)
        {
            button.FlatStyle = FlatStyle.Flat;
            // Tự vẽ viền sau khi nút đã vẽ nền. BorderSize=0 tránh việc
            // FlatAppearance bị Region bo góc cắt mất cạnh phải/dưới.
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = hoverColor;
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(211, 229, 250);
            button.UseVisualStyleBackColor = false;
            button.Cursor = Cursors.Hand;
            button.Tag = new ButtonVisualState(normalColor, hoverColor, normalTextColor);

            Action applyRoundedRegion = () =>
            {
                if (button.Width <= 2 || button.Height <= 2) return;
                using (var path = CreateRoundedRectanglePath(
                    new Rectangle(0, 0, button.Width - 1, button.Height - 1), 9))
                {
                    Region? oldRegion = button.Region;
                    button.Region = new Region(path);
                    oldRegion?.Dispose();
                }
            };

            button.Resize += (sender, e) => applyRoundedRegion();
            button.Paint += (sender, e) => DrawRoundedButtonBorder(button, e.Graphics);
            button.MouseEnter += (sender, e) => ApplyButtonHover(button, true);
            button.MouseLeave += (sender, e) => ApplyButtonHover(button, false);
            applyRoundedRegion();
            ApplyButtonHover(button, false);
        }

        private static System.Drawing.Drawing2D.GraphicsPath CreateRoundedRectanglePath(Rectangle bounds, int radius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            radius = Math.Max(2, Math.Min(radius, Math.Min(bounds.Width, bounds.Height) / 2));
            int diameter = radius * 2;
            path.AddArc(bounds.Left, bounds.Top, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Top, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.Left, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void DrawRoundedButtonBorder(Button button, Graphics graphics)
        {
            if (button.Width < 4 || button.Height < 4) return;
            bool hovered = button.Tag is ButtonVisualState state && state.IsHovered;
            Color borderColor = !button.Enabled && button == btnSave
                ? Color.FromArgb(190, 199, 210)
                : hovered
                    ? Color.FromArgb(0, 76, 155)
                    : button == btnRedo
                        ? Color.FromArgb(145, 165, 190)
                        : Color.FromArgb(0, 105, 205);

            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using (var path = CreateRoundedRectanglePath(
                new Rectangle(1, 1, button.Width - 3, button.Height - 3), 8))
            using (var pen = new Pen(borderColor, 1F))
            {
                pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;
                graphics.DrawPath(pen, path);
            }
        }

        private void ApplyButtonHover(Button button, bool hovered)
        {
            if (button.Tag is not ButtonVisualState state) return;
            state.IsHovered = hovered;

            if (!button.Enabled && button == btnSave)
            {
                button.BackColor = Color.FromArgb(220, 224, 230);
                button.ForeColor = Color.FromArgb(112, 124, 140);
            }
            else
            {
                button.BackColor = hovered ? state.HoverColor : state.NormalColor;
                button.ForeColor = state.TextColor;
            }

            button.Invalidate();
        }

        private void QueuePermissionRowLayout()
        {
            if (IsDisposed || clbPermission == null || clbPermission.IsDisposed)
                return;

            if (!IsHandleCreated || !clbPermission.IsHandleCreated)
            {
                UpdatePermissionItemHeight();
                return;
            }

            // Defer until WinForms finishes the current layout/resize pass.
            try
            {
                BeginInvoke(new Action(() =>
                {
                    if (!IsDisposed && !clbPermission.IsDisposed)
                        UpdatePermissionItemHeight();
                }));
            }
            catch (InvalidOperationException)
            {
                // Handle may be recreated/disposed during a resize.
            }
        }

        private void UpdatePermissionItemHeight()
        {
            UpdatePermissionItemHeight(clbPermission?.Items.Count ?? 0);
        }

        private void UpdatePermissionItemHeight(int itemCount)
        {
            CheckedListBox? list = clbPermission;
            if (list == null || list.IsDisposed)
                return;

            list.IntegralHeight = false;
            EnsurePermissionOwnerDraw();

            int actualCount = list.Items.Count;
            itemCount = Math.Min(Math.Max(0, itemCount), actualCount);
            if (itemCount <= 0)
            {
                list.ItemHeight = 30;
                list.Invalidate();
                return;
            }

            int availableHeight = list.ClientSize.Height;
            if (availableHeight <= 0)
                return;

            // Chia toàn bộ chiều cao thực tế cho đủ số quyền. Dùng floor để
            // 12 dòng không vượt khỏi đáy vùng danh sách; phần dư tối đa 11 px.
            int rowHeight = Math.Max(1, availableHeight / itemCount);

            // Cập nhật thuộc tính managed và gửi trực tiếp thông điệp native.
            // CheckedListBox dựa trên native ListBox; LB_SETITEMHEIGHT buộc
            // vùng hiển thị của từng dòng nhận đúng chiều cao vừa tính.
            if (list.ItemHeight != rowHeight)
                list.ItemHeight = rowHeight;

            if (list.IsHandleCreated)
            {
                SendMessage(list.Handle, LB_SETITEMHEIGHT, IntPtr.Zero,
                    new IntPtr(rowHeight));
            }

            list.Invalidate();
            list.Update();
        }

        private sealed class ButtonVisualState
        {
            public Color NormalColor { get; }
            public Color HoverColor { get; }
            public Color TextColor { get; }
            public bool IsHovered { get; set; }

            public ButtonVisualState(Color normalColor, Color hoverColor, Color textColor)
            {
                NormalColor = normalColor;
                HoverColor = hoverColor;
                TextColor = textColor;
            }
        }

        private void UpdateSaveButton()
        {
            btnSave.Enabled = _isDirty;
            btnSave.BackColor = _isDirty
                ? Color.FromArgb(0, 105, 205)
                : Color.FromArgb(220, 224, 230);
            btnSave.ForeColor = _isDirty
                ? Color.White
                : Color.FromArgb(112, 124, 140);
            if (btnSave.Tag is ButtonVisualState)
                ApplyButtonHover(btnSave, false);
        }

        // Giữ Undo và Redo luôn nằm sát mép phải của header, kể cả khi form
        // được nhúng vào FrmHome hoặc thay đổi kích thước/DPI.
        private void pnlPermissionHeader_Resize(object? sender, EventArgs e)
        {
            if (pnlPermissionHeader == null || btnUndo == null || btnRedo == null)
                return;

            const int buttonWidth = 96;
            const int buttonHeight = 36;
            const int gap = 10;
            const int rightPadding = 18;

            int y = Math.Max(0, (pnlPermissionHeader.ClientSize.Height - buttonHeight) / 2);
            int redoX = Math.Max(0, pnlPermissionHeader.ClientSize.Width - rightPadding - buttonWidth);
            int undoX = Math.Max(8, redoX - gap - buttonWidth);

            btnUndo.SetBounds(undoX, y, buttonWidth, buttonHeight);
            btnRedo.SetBounds(redoX, y, buttonWidth, buttonHeight);
        }

        private void pnlFooter_Resize(object? sender, EventArgs e)
        {
            const int gap = 14;
            const int rightPadding = 18;

            int totalWidth = btnSave.Width + btnRefresh.Width + gap;
            int x = Math.Max(10, pnlFooter.ClientSize.Width - rightPadding - totalWidth);
            int y = (pnlFooter.ClientSize.Height - btnSave.Height) / 2;

            btnSave.Location = new Point(x, y);
            btnRefresh.Location = new Point(btnSave.Right + gap, y);
        }

        private void clbPermission_DrawItem(
            object? sender,
            DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= clbPermission.Items.Count)
                return;

            bool isChecked = clbPermission.GetItemChecked(e.Index);
            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            // Nền zebra: dòng chẵn xanh nhạt, dòng lẻ trắng.
            // Tô hết chiều ngang ClientSize để dải màu phủ trọn từng hàng,
            // không dừng ở vùng checkbox hoặc vùng chữ.
            Color rowColor = isSelected
                ? Color.FromArgb(195, 222, 249)
                : (e.Index % 2 == 0
                    ? Color.FromArgb(218, 234, 252)
                    : Color.White);

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Rectangle rowBounds = new Rectangle(
                e.Bounds.Left,
                e.Bounds.Top,
                Math.Max(e.Bounds.Width, clbPermission.ClientSize.Width - e.Bounds.Left),
                e.Bounds.Height);
            using (SolidBrush backgroundBrush = new SolidBrush(rowColor))
                e.Graphics.FillRectangle(backgroundBrush, rowBounds);

            // Checkbox vector tự vẽ để có kích thước, màu xanh và dấu tick đồng nhất
            // với bản thiết kế, không phụ thuộc Visual Styles của Windows.
            const int checkSize = 18;
            int checkX = e.Bounds.Left + 12;
            int checkY = e.Bounds.Top + (e.Bounds.Height - checkSize) / 2;
            Rectangle checkBounds = new Rectangle(checkX, checkY, checkSize, checkSize);

            using (var checkPath = new System.Drawing.Drawing2D.GraphicsPath())
            {
                int radius = 4;
                checkPath.AddArc(checkBounds.Left, checkBounds.Top, radius * 2, radius * 2, 180, 90);
                checkPath.AddArc(checkBounds.Right - radius * 2, checkBounds.Top, radius * 2, radius * 2, 270, 90);
                checkPath.AddArc(checkBounds.Right - radius * 2, checkBounds.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
                checkPath.AddArc(checkBounds.Left, checkBounds.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
                checkPath.CloseFigure();

                if (isChecked)
                {
                    using (var checkBrush = new SolidBrush(Color.FromArgb(0, 105, 195)))
                        e.Graphics.FillPath(checkBrush, checkPath);
                    using (var tickPen = new Pen(Color.White, 2.1F))
                    {
                        tickPen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                        tickPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                        tickPen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                        e.Graphics.DrawLines(tickPen, new[]
                        {
                            new Point(checkBounds.Left + 4, checkBounds.Top + 9),
                            new Point(checkBounds.Left + 7, checkBounds.Top + 12),
                            new Point(checkBounds.Left + 14, checkBounds.Top + 5)
                        });
                    }
                }
                else
                {
                    using (var fillBrush = new SolidBrush(Color.White))
                        e.Graphics.FillPath(fillBrush, checkPath);
                    using (var borderPen = new Pen(Color.FromArgb(0, 105, 195), 1.3F))
                        e.Graphics.DrawPath(borderPen, checkPath);
                }
            }

            string itemText = clbPermission.Items[e.Index]?.ToString() ?? string.Empty;
            int keyStart = itemText.IndexOf('(');

            string permissionName = keyStart >= 0
                ? itemText.Substring(0, keyStart).TrimEnd()
                : itemText;
            string permissionKey = keyStart >= 0
                ? itemText.Substring(keyStart).TrimStart()
                : string.Empty;

            using Font nameFont = new Font(clbPermission.Font, FontStyle.Bold);
            using Font keyFont = new Font(clbPermission.Font, FontStyle.Regular);

            int textX = checkBounds.Right + 9;
            int textY = e.Bounds.Top;
            int textHeight = e.Bounds.Height;
            Color textColor = Color.FromArgb(25, 48, 78);

            Size nameSize = TextRenderer.MeasureText(
                e.Graphics, permissionName, nameFont,
                new Size(int.MaxValue, textHeight),
                TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);

            Rectangle nameBounds = new Rectangle(
                textX, textY, Math.Max(0, nameSize.Width + 2), textHeight);

            TextRenderer.DrawText(
                e.Graphics, permissionName, nameFont, nameBounds, textColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
                TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);

            if (!string.IsNullOrEmpty(permissionKey))
            {
                Rectangle keyBounds = new Rectangle(
                    nameBounds.Right + 3, textY,
                    Math.Max(0, e.Bounds.Right - nameBounds.Right - 8), textHeight);

                TextRenderer.DrawText(
                    e.Graphics, permissionKey, keyFont, keyBounds, textColor,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
                    TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding |
                    TextFormatFlags.SingleLine);
            }

            if ((e.State & DrawItemState.Focus) == DrawItemState.Focus)
                e.DrawFocusRectangle();
        }

        private void clbPermission_ItemCheck(
            object sender,
            ItemCheckEventArgs e)
        {
            if (_loading || _isUndoRedo)
                return;

            _pendingUndoState = CaptureState();

            BeginInvoke(() =>
            {
                if (_pendingUndoState != null)
                {
                    PushUndoState();

                    _pendingUndoState = null;
                }

                UpdateUndoRedoButtons();

                HashSet<int> currentPermissions =
                    new();

                for (int i = 0; i < clbPermission.Items.Count; i++)
                {
                    if (clbPermission.GetItemChecked(i))
                    {
                        currentPermissions.Add(
                            _permissions[i].PermissionId);
                    }
                }

                _isDirty =
                    !_originalPermissions.SetEquals(currentPermissions);

                UpdateSaveButton();
            });
        }

        protected override async void OnFormClosing(
            FormClosingEventArgs e)
        {
            if (!_isDirty)
            {
                base.OnFormClosing(e);
                return;
            }

            DialogResult result =
                FrmLoginMessage.ShowMessage(
                    this,
                    LoginMessageType.Information,
                    "Xác nhận đóng màn hình",
                    "Bạn có thay đổi chưa được lưu.\n\nBạn có muốn lưu trước khi đóng?",
                    MessageBoxButtons.YesNoCancel);

            switch (result)
            {
                case DialogResult.Yes:

                    e.Cancel = true;

                    bool success =
                        await SavePermissionsAsync();

                    if (success)
                    {
                        _isDirty = false;

                        Close();
                    }

                    break;

                case DialogResult.No:

                    _isDirty = false;

                    base.OnFormClosing(e);

                    break;

                case DialogResult.Cancel:

                    e.Cancel = true;

                    break;
            }
        }

        protected override bool ProcessCmdKey(
            ref Message msg,
            Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Control | Keys.Z:

                    if (btnUndo.Enabled)
                    {
                        Undo();
                        return true;
                    }

                    break;

                case Keys.Control | Keys.Y:

                    if (btnRedo.Enabled)
                    {
                        Redo();
                        return true;
                    }

                    break;
            }

            return base.ProcessCmdKey(
                ref msg,
                keyData);
        }

        private PermissionState CaptureState()
        {
            PermissionState state = new();

            for (int i = 0; i < clbPermission.Items.Count; i++)
            {
                state.Permissions.Add(
                    _permissions[i].PermissionKey,
                    clbPermission.GetItemChecked(i));
            }

            return state;
        }

        private void RestoreState(PermissionState state)
        {
            _isUndoRedo = true;

            try
            {
                for (int i = 0; i < _permissions.Count; i++)
                {
                    string key =
                        _permissions[i].PermissionKey;

                    if (state.Permissions.TryGetValue(
                        key,
                        out bool isChecked))
                    {
                        clbPermission.SetItemChecked(
                            i,
                            isChecked);
                    }
                }
            }
            finally
            {
                _isUndoRedo = false;
            }

            HashSet<int> currentPermissions =
                new();

            for (int i = 0; i < clbPermission.Items.Count; i++)
            {
                if (clbPermission.GetItemChecked(i))
                {
                    currentPermissions.Add(
                        _permissions[i].PermissionId);
                }
            }

            _isDirty =
                !_originalPermissions.SetEquals(currentPermissions);

            UpdateUndoRedoButtons();

            UpdateSaveButton();
        }

        private void PushUndoState()
        {
            PermissionState current =
                CaptureState();

            _undoStack.Push(current.Clone());

            _redoStack.Clear();

            UpdateUndoRedoButtons();
        }

        private void UpdateUndoRedoButtons()
        {
            btnUndo.Enabled =
                _undoStack.Count > 1;

            btnRedo.Enabled =
                _redoStack.Count > 0;
        }

        private void Undo()
        {
            if (_undoStack.Count <= 1)
                return;

            PermissionState current =
                _undoStack.Pop();

            _redoStack.Push(current);

            PermissionState previous =
                _undoStack.Peek();

            RestoreState(previous);

            UpdateUndoRedoButtons();
        }

        private void Redo()
        {
            if (_redoStack.Count == 0)
                return;

            PermissionState next =
                _redoStack.Pop();

            _undoStack.Push(next);

            RestoreState(next);

            UpdateUndoRedoButtons();
        }

        private void btnUndo_Click(
            object sender,
            EventArgs e)
        {
            Undo();
        }

        private void btnRedo_Click(
            object sender,
            EventArgs e)
        {
            Redo();
        }
    }
}