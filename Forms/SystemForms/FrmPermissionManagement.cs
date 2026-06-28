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
            if (_loading)
                return;

            BeginInvoke(() =>
            {
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
    }
}
