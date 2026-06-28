using QuanLyCBCS.DTOs;
using QuanLyCBCS.Helpers;
using QuanLyCBCS.Services;

namespace QuanLyCBCS.Forms.SystemForms
{
    public partial class FrmPermissionManagement : Form
    {

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

            await LoadRolesAsync();
        }

        private async Task LoadRolesAsync()
        {
            cboRole.DataSource =
                await _service.GetRolesAsync();

            if (cboRole.Items.Count > 0)
                cboRole.SelectedIndex = 0;
        }

        private async void cboRole_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            if (cboRole.SelectedItem == null)
                return;

            await LoadPermissionAsync();
        }

        private async Task LoadPermissionAsync()
        {
            _loading = true;

            try
            {
                string role =
                    cboRole.SelectedItem!.ToString()!;

                _permissions =
                    await _service.GetRolePermissionsAsync(role);

                clbPermission.Items.Clear();

                foreach (PermissionDto permission in _permissions)
                {
                    int index = clbPermission.Items.Add(
                        $"{permission.PermissionName} ({permission.PermissionKey})");

                    clbPermission.SetItemChecked(
                        index,
                        permission.IsGranted);
                }

                // Lưu trạng thái gốc
                _originalPermissions =
                    _permissions
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
            finally
            {
                _loading = false;
            }
        }

        private async void btnRefresh_Click(
            object sender,
            EventArgs e)
        {
            await LoadPermissionAsync();
        }

        private void btnClose_Click(
            object sender,
            EventArgs e)
        {
            Close();
        }

        private async Task<bool> SavePermissionsAsync()
        {
            if (cboRole.SelectedItem == null)
                return false;

            try
            {
                btnSave.Enabled = false;

                string role =
                    cboRole.SelectedItem.ToString()!;

                List<int> selectedPermissions =
                    new();

                for (int i = 0;
                     i < clbPermission.Items.Count;
                     i++)
                {
                    if (clbPermission.GetItemChecked(i))
                    {
                        selectedPermissions.Add(
                            _permissions[i].PermissionId);
                    }
                }

                await _service.SaveRolePermissionsAsync(
                    role,
                    selectedPermissions);

                _originalPermissions =
                    selectedPermissions.ToHashSet();

                _isDirty = false;

                _undoStack.Clear();
                _redoStack.Clear();

                _undoStack.Push(CaptureState());

                UpdateUndoRedoButtons();

                UpdateSaveButton();

                if (role == CurrentUser.Role)
                {
                    await _service
                        .RefreshCurrentUserPermissionsAsync();
                }

                MessageBox.Show(
                    "Lưu phân quyền thành công.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Lỗi",
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
