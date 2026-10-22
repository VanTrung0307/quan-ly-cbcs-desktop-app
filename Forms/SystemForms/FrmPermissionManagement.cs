using QuanLyCBCS.DTOs;
using QuanLyCBCS.Helpers;
using QuanLyCBCS.Services;
using QuanLyCBCS.Models;

namespace QuanLyCBCS.Forms.SystemForms
{
    public partial class FrmPermissionManagement : Form
    {
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

        public FrmPermissionManagement()
        {
            InitializeComponent();

            // Cho phép nhập tên tài khoản trực tiếp.
            cboRole.DropDownStyle = ComboBoxStyle.DropDown;

            // Gợi ý tài khoản khi người dùng gõ.
            cboRole.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboRole.AutoCompleteSource = AutoCompleteSource.ListItems;

            lblRole.Text = "Tài khoản:";

            // Không xử lý SelectedIndexChanged như cách cũ.
            // Chỉ tải quyền khi người dùng thực sự chọn một mục.
            cboRole.SelectedIndexChanged -= cboRole_SelectedIndexChanged;
            cboRole.SelectionChangeCommitted += cboRole_SelectedIndexChanged;

            // Nhấn vào ô sẽ mở danh sách tài khoản.
            cboRole.Click += (sender, e) =>
            {
                cboRole.DroppedDown = true;
            };
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
                MessageBox.Show(
                    ex.Message,
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

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
                _users = await _service.GetUsersAsync();

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
                DialogResult result = MessageBox.Show(
                    "Bạn có thay đổi quyền chưa lưu. " +
                    "Bạn có muốn bỏ các thay đổi đó để chuyển tài khoản không?",
                    "Xác nhận chuyển tài khoản",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

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
                MessageBox.Show(
                    ex.Message,
                    "Không thể tải quyền",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
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

        private void btnClose_Click(
            object sender,
            EventArgs e)
        {
            Close();
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

                MessageBox.Show(
                    $"Đã lưu phân quyền riêng cho tài khoản " +
                    $"'{selectedUser.Username}'.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Lỗi lưu phân quyền",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

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

        private void UpdateSaveButton()
        {
            btnSave.Enabled = _isDirty;
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
                MessageBox.Show(
                    "Bạn có thay đổi chưa được lưu.\n\nBạn có muốn lưu trước khi đóng?",
                    "Xác nhận",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

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
