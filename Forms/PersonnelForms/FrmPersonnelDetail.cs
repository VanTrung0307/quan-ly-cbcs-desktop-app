using QuanLyCBCS.Enums;
using QuanLyCBCS.Helpers;
using QuanLyCBCS.Models;
using QuanLyCBCS.Services;

namespace QuanLyCBCS.Forms.PersonnelForms
{
    public partial class FrmPersonnelDetail : Form
    {
        private readonly PersonnelService _service;

        private readonly FormMode _mode;

        private readonly int? _personnelId;

        private readonly PersonnelHistoryService _historyService =
            new PersonnelHistoryService();

        public FrmPersonnelDetail(
            FormMode mode,
            int personnelId = 0)
        {
            InitializeComponent();

            _service = new PersonnelService();

            _mode = mode;

            _personnelId = personnelId;

            cboPersonnelType.SelectedIndex = 0;

            dtNgaySinh.Format =
                DateTimePickerFormat.Custom;

            dtNgaySinh.CustomFormat =
                "dd/MM/yyyy";

            StartPosition =
                FormStartPosition.CenterScreen;

            txtSoHieu.TextChanged +=
                (_, _) => ClearError(txtSoHieu);

            txtHoTen.TextChanged +=
                (_, _) => ClearError(txtHoTen);

            txtCCCD.TextChanged +=
                (_, _) => ClearError(txtCCCD);

            txtSoDienThoai.TextChanged +=
                (_, _) => ClearError(txtSoDienThoai);

            txtCCCD.KeyPress += TxtNumberOnly_KeyPress;

            txtSoDienThoai.KeyPress += TxtNumberOnly_KeyPress;

            txtHeSoLuong.KeyPress += TxtHeSoLuong_KeyPress;

            txtHeSoLuong.Leave += TxtHeSoLuong_Leave;

            ToolTipHelper
                .SetupPersonnelDetailToolTips(
                    this,
                    toolTipMain);

            FormNavigationHelper
                .SetupEnterNavigation(
                    this);

            txtHoTen.Leave += txtHoTen_Leave;
            txtSoHieu.Leave += txtSoHieu_Leave;
            txtNoiSinh.Leave += txtNoiSinh_Leave;
            txtQueQuanMoi.Leave += Address_Leave;
            txtQueQuanCu.Leave += Address_Leave;
            txtTinhMoi.Leave += Address_Leave;
            txtTinhCu.Leave += Address_Leave;

            dtNgaySinh.MaxDate = DateTime.Today;

            dtNgaySinh.ValueChanged += (_, _) => UpdateAge();
        }

        private void chkDangVien_CheckedChanged(
            object sender,
            EventArgs e)
        {
            dtDangDuBi.Enabled =
                chkDangVien.Checked;

            dtDangChinhThuc.Enabled =
                chkDangVien.Checked;

            bool enabled =
                chkDangVien.Checked;

            dtDangDuBi.Enabled =
                enabled;

            dtDangChinhThuc.Enabled =
                enabled;

            lblDangDuBi.Enabled =
                enabled;

            lblDangChinhThuc.Enabled =
                enabled;
        }

        private async Task<bool> ValidateInputAsync()
        {
            bool soHieuExists =
                await _service
                    .IsSoHieuExistsAsync(
                        txtSoHieu.Text);

            if (_mode == FormMode.Add
                && soHieuExists)
            {
                epValidation.SetError(
                    txtSoHieu,
                    "Số hiệu đã tồn tại");

                FocusControl(txtSoHieu);

                return false;
            }

            bool cccdExists =
                await _service
                    .IsCCCDExistsAsync(
                        txtCCCD.Text.Trim(),
                        _mode == FormMode.Edit
                            ? _personnelId
                            : null);

            if (cccdExists)
            {
                epValidation.SetError(
                    txtCCCD,
                    "CCCD đã tồn tại");

                FocusControl(txtCCCD);

                return false;
            }

            bool phoneExists =
                await _service
                    .IsPhoneExistsAsync(
                        txtSoDienThoai.Text.Trim(),
                        _mode == FormMode.Edit
                            ? _personnelId
                            : null);

            if (phoneExists)
            {
                epValidation.SetError(
                    txtSoDienThoai,
                    "Số điện thoại đã tồn tại");

                FocusControl(txtSoDienThoai);

                return false;
            }

            if (dtNgaySinh.Value > DateTime.Today)
            {
                epValidation.SetError(
                    dtNgaySinh,
                    "Ngày sinh không hợp lệ");

                return false;
            }

            return true;
        }

        private void FillPersonnel(Personnel p)
        {
            // =====================
            // BASIC
            // =====================

            p.PersonnelType =
                cboPersonnelType
                .SelectedIndex + 1;

            p.SoHieu =
                txtSoHieu.Text.Trim();

            p.HoTen =
                txtHoTen.Text.Trim();

            p.DonVi =
                txtDonVi.Text.Trim();

            p.GioiTinh =
                cboGioiTinh.Text;

            p.NgaySinh =
                dtNgaySinh.Value;

            p.NoiSinh =
                txtNoiSinh.Text.Trim();

            // =====================
            // ADDRESS
            // =====================

            p.QueQuanMoi =
                txtQueQuanMoi.Text.Trim();

            p.QueQuanCu =
                txtQueQuanCu.Text.Trim();

            p.TinhMoi =
                txtTinhMoi.Text.Trim();

            p.TinhCu =
                txtTinhCu.Text.Trim();

            p.HKTTMoi =
                txtHKTTMoi.Text.Trim();

            p.HKTTCu =
                txtHKTTCu.Text.Trim();

            // =====================
            // WORK
            // =====================

            p.ViTriCongTac =
                txtViTriCongTac.Text.Trim();

            p.CapBacHam =
                cboCapBacHam.Text.Trim();

            p.NgayNhanCapBac =
                dtNgayNhanCapBac.Value;

            p.ThangNamVaoNganh =
                dtThangNamVaoNganh.Value;

            p.ThangNamNhanCongTac =
                dtThangNamNhanCongTac.Value;

            p.ChucVu =
                txtChucVu.Text.Trim();

            p.NgayBoNhiem =
                dtNgayBoNhiem.Value;

            // Hệ số lương

            if (decimal.TryParse(
                txtHeSoLuong.Text,
                out decimal heSoLuong))
            {
                p.HeSoLuong =
                    heSoLuong;
            }

            // =====================
            // EDUCATION
            // =====================

            p.TrinhDoTinHoc =
                txtTrinhDoTinHoc.Text.Trim();

            p.TrinhDoNgoaiNgu =
                txtTrinhDoNgoaiNgu.Text.Trim();

            p.TrinhDoVanHoa =
                txtTrinhDoVanHoa.Text.Trim();

            p.TrinhDoChuyenMon =
                txtTrinhDoChuyenMon.Text.Trim();

            p.ChuyenNganhDaoTao =
                txtChuyenNganhDaoTao.Text.Trim();

            p.ThangNamTotNghiep =
                dtThangNamTotNghiep.Value;

            p.TrinhDoLyLuanChinhTri =
                txtTrinhDoLyLuanChinhTri.Text.Trim();

            // =====================
            // PARTY
            // =====================

            p.DangVien =
                chkDangVien.Checked;

            if (chkDangVien.Checked)
            {
                p.NgayVaoDangDuBi =
                    dtDangDuBi.Value;

                p.NgayVaoDangChinhThuc =
                    dtDangChinhThuc.Value;
            }

            // =====================
            // OTHER
            // =====================

            p.CCCD =
                txtCCCD.Text.Trim();

            p.NgayCapCCCD =
                dtNgayCapCCCD.Value;

            p.SoDienThoai =
                txtSoDienThoai.Text.Trim();
        }

        private async Task SaveAddAsync()
        {
            bool isValid =
                await ValidateInputAsync();

            if (!isValid)
                return;

            Personnel p =
                new Personnel();

            FillPersonnel(p);

            await _service.AddAsync(p);

            await AuditHelper.LogAsync(
                AuditAction.AddPersonnel,
                $"Thêm CBCS '{p.HoTen}' (Số hiệu: {p.SoHieu})");

            await _historyService
                .AddHistoryAsync(
                    p.PersonnelID,
                    p.HoTen,
                    "CREATE_PERSONNEL",
                    $"Thêm mới CBCS '{p.HoTen}'");

            MessageBox.Show(
                "Thêm CBCS thành công");

            DialogResult =
                DialogResult.OK;

            Close();
        }

        private async void btnSave_Click(
            object sender,
            EventArgs e)
        {
            if (!ValidateForm())
                return;

            if (_mode == FormMode.Add)
            {
                await SaveAddAsync();
            }
            else
            {
                await SaveEditAsync();
            }
        }

        private async Task LoadPersonnelAsync()
        {

            if (!_personnelId.HasValue)
            {
                MessageBox.Show(
                    "Không xác định CBCS.");

                return;
            }

            var p =
                await _service
                    .GetByIdAsync(
                        _personnelId.Value);

            if (p == null)
            {
                MessageBox.Show("Không tìm thấy dữ liệu");
                return;
            }

            cboPersonnelType.SelectedIndex =
                p.PersonnelType - 1;

            txtSoHieu.Text =
                p.SoHieu;

            txtHoTen.Text =
                p.HoTen;

            txtDonVi.Text =
                p.DonVi;

            cboGioiTinh.Text =
                p.GioiTinh;

            dtNgaySinh.Value =
                p.NgaySinh ?? DateTime.Now;

            txtNoiSinh.Text =
                p.NoiSinh;

            txtCCCD.Text =
                p.CCCD;

            txtSoDienThoai.Text =
                p.SoDienThoai;

            chkDangVien.Checked =
                p.DangVien;

            // ADDRESS

            txtQueQuanMoi.Text =
                p.QueQuanMoi;

            txtQueQuanCu.Text =
                p.QueQuanCu;

            txtTinhMoi.Text =
                p.TinhMoi;

            txtTinhCu.Text =
                p.TinhCu;

            txtHKTTMoi.Text =
                p.HKTTMoi;

            txtHKTTCu.Text =
                p.HKTTCu;

            // WORK

            txtViTriCongTac.Text =
                p.ViTriCongTac;

            cboCapBacHam.Text =
                p.CapBacHam;

            txtChucVu.Text =
                p.ChucVu;

            txtHeSoLuong.Text =
                p.HeSoLuong.ToString();

            dtNgayNhanCapBac.Value =
                p.NgayNhanCapBac ?? DateTime.Now;

            dtThangNamVaoNganh.Value =
                p.ThangNamVaoNganh ?? DateTime.Now;

            dtThangNamNhanCongTac.Value =
                p.ThangNamNhanCongTac ?? DateTime.Now;

            dtNgayBoNhiem.Value =
                p.NgayBoNhiem ?? DateTime.Now;

            // EDUCATION

            txtTrinhDoTinHoc.Text =
                p.TrinhDoTinHoc;

            txtTrinhDoNgoaiNgu.Text =
                p.TrinhDoNgoaiNgu;

            txtTrinhDoVanHoa.Text =
                p.TrinhDoVanHoa;

            txtTrinhDoChuyenMon.Text =
                p.TrinhDoChuyenMon;

            txtChuyenNganhDaoTao.Text =
                p.ChuyenNganhDaoTao;

            txtTrinhDoLyLuanChinhTri.Text =
                p.TrinhDoLyLuanChinhTri;

            dtThangNamTotNghiep.Value =
                p.ThangNamTotNghiep ?? DateTime.Now;

            // PARTY

            if (p.DangVien)
            {
                dtDangDuBi.Value =
                    p.NgayVaoDangDuBi ?? DateTime.Now;

                dtDangChinhThuc.Value =
                    p.NgayVaoDangChinhThuc ?? DateTime.Now;
            }

            // OTHER

            dtNgayCapCCCD.Value =
                p.NgayCapCCCD ?? DateTime.Now;
        }

        private async void FrmPersonnelDetail_Load(
            object sender,
            EventArgs e)
        {
            SetupButtons();

            SetupTabControl();

            SetupTextBoxes();

            SetupHeader();

            if (_mode == FormMode.Edit)
            {
                await LoadPersonnelAsync();
            }

            UpdateAge();
        }

        private async Task SaveEditAsync()
        {
            if (!_personnelId.HasValue)
            {
                MessageBox.Show(
                    "Không xác định CBCS.");

                return;
            }

            bool isValid =
                await ValidateInputAsync();

            if (!isValid)
                return;

            var p =
                await _service
                .GetByIdAsync(_personnelId.Value);

            if (p == null)
                return;

            FillPersonnel(p);

            await _service.UpdateAsync(p);

            await AuditHelper.LogAsync(
                AuditAction.UpdatePersonnel,
                $"Cập nhật hồ sơ CBCS '{p.HoTen}' (Số hiệu: {p.SoHieu})");

            await _historyService
                .AddHistoryAsync(
                    p.PersonnelID,
                    p.HoTen,
                    "UPDATE_PERSONNEL",
                    $"Cập nhật hồ sơ CBCS '{p.HoTen}'");

            MessageBox.Show(
                "Cập nhật thành công");

            DialogResult =
                DialogResult.OK;

            Close();
        }

        private void StyleActionButton(
            Button button,
            Color color)
        {
            button.FlatStyle =
                FlatStyle.Flat;

            button.FlatAppearance.BorderSize = 0;

            button.BackColor =
                color;

            button.ForeColor =
                Color.White;

            button.Font =
                new Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold);

            RoundedControlHelper
                .SetRounded(
                    button,
                    12);
        }

        private void SetupButtons()
        {
            StyleActionButton(
                btnSave,
                Color.FromArgb(
                    39,
                    174,
                    96));

            StyleActionButton(
                btnCancel,
                Color.FromArgb(
                    231,
                    76,
                    60));
        }

        private void SetupTabControl()
        {
            tabMain.Appearance =
                TabAppearance.FlatButtons;

            tabMain.SizeMode =
                TabSizeMode.Fixed;

            tabMain.ItemSize =
                new Size(
                    120,
                    35);
        }

        private void StyleTextBox(
            TextBox txt)
        {
            txt.Font =
                new Font(
                    "Segoe UI",
                    10);

            txt.BorderStyle =
                BorderStyle.FixedSingle;
        }

        private void SetupTextBoxes()
        {
            StyleTextBox(txtSoHieu);

            StyleTextBox(txtHoTen);

            StyleTextBox(txtNoiSinh);

            StyleTextBox(txtCCCD);

            StyleTextBox(txtSoDienThoai);

            StyleTextBox(txtChucVu);

            StyleTextBox(txtDonVi);
        }

        private void SetupHeader()
        {
            if (_mode == FormMode.Add)
            {
                lblTitle.Text =
                    "THÊM CBCS MỚI";

                lblSubTitle.Text =
                    "Nhập thông tin cán bộ chiến sĩ";
            }
            else
            {
                lblTitle.Text =
                    "CẬP NHẬT CBCS";

                lblSubTitle.Text =
                    "Chỉnh sửa hồ sơ cán bộ chiến sĩ";
            }
        }

        private void ClearValidationErrors()
        {
            epValidation.Clear();
        }

        private bool ValidateForm()
        {
            epValidation.Clear();

            Control? firstErrorControl = null;

            bool isValid = true;

            if (!ValidationHelper.ValidateRequired(
                txtSoHieu,
                epValidation,
                "Nhập số hiệu"))
            {
                firstErrorControl ??= txtSoHieu;
                isValid = false;
            }

            if (!ValidationHelper.ValidateRequired(
                txtHoTen,
                epValidation,
                "Nhập họ tên"))
            {
                firstErrorControl ??= txtHoTen;
                isValid = false;
            }

            if (!ValidationHelper.ValidateCCCD(
                txtCCCD,
                epValidation))
            {
                firstErrorControl ??= txtCCCD;
                isValid = false;
            }

            if (!ValidationHelper.ValidatePhone(
                txtSoDienThoai,
                epValidation))
            {
                firstErrorControl ??= txtSoDienThoai;
                isValid = false;
            }

            if (!string.IsNullOrWhiteSpace(
                    txtHeSoLuong.Text)
                &&
                !ValidationHelper.IsHeSoLuong(
                    txtHeSoLuong.Text))
            {
                epValidation.SetError(
                    txtHeSoLuong,
                    "Hệ số lương không hợp lệ");

                firstErrorControl ??=
                    txtHeSoLuong;

                isValid = false;
            }

            if (cboGioiTinh.SelectedIndex < 0)
            {
                epValidation.SetError(
                    cboGioiTinh,
                    "Chọn giới tính");

                firstErrorControl ??= cboGioiTinh;

                isValid = false;
            }

            if (cboPersonnelType.SelectedIndex < 0)
            {
                epValidation.SetError(
                    cboPersonnelType,
                    "Chọn loại CBCS");

                firstErrorControl ??= cboPersonnelType;

                isValid = false;
            }

            if (firstErrorControl != null)
            {
                FocusControl(
                    firstErrorControl);
            }

            if (!isValid)
            {
                FocusControl(firstErrorControl!);

                return false;
            }

            return isValid;
        }

        private void txtHoTen_TextChanged(
            object sender,
            EventArgs e)
        {
            ClearError(txtHoTen);
        }

        private void txtCCCD_TextChanged(
            object sender,
            EventArgs e)
        {
            ClearError(txtCCCD);
        }

        private void txtSoHieu_TextChanged(
            object sender,
            EventArgs e)
        {
            ClearError(txtSoHieu);
        }

        private void ClearError(
            Control control)
        {
            epValidation.SetError(
                control,
                string.Empty);
        }

        private void FocusControl(
            Control control)
        {
            Control? current = control;

            while (current != null)
            {
                if (current is TabPage page)
                {
                    if (page.Parent is TabControl tabControl)
                    {
                        tabControl.SelectedTab = page;
                    }

                    break;
                }

                current = current.Parent;
            }

            control.Focus();
        }

        private void TxtNumberOnly_KeyPress(
            object? sender,
            KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TxtHeSoLuong_KeyPress(
            object? sender,
            KeyPressEventArgs e)
        {
            TextBox txt =
                (TextBox)sender!;

            // Backspace

            if (char.IsControl(e.KeyChar))
                return;

            // Số

            if (char.IsDigit(e.KeyChar))
            {
                string futureText =
                    txt.Text.Insert(
                        txt.SelectionStart,
                        e.KeyChar.ToString());

                int dotIndex =
                    futureText.IndexOf('.');

                if (dotIndex >= 0)
                {
                    string decimalPart =
                        futureText[(dotIndex + 1)..];

                    if (decimalPart.Length > 1)
                    {
                        e.Handled = true;
                    }
                }

                return;
            }

            // Dấu chấm

            if (e.KeyChar == '.')
            {
                if (txt.Text.Contains('.'))
                {
                    e.Handled = true;
                }

                return;
            }

            e.Handled = true;
        }

        private void TxtHeSoLuong_Leave(
            object? sender,
            EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(
                txtHeSoLuong.Text))
                return;

            if (decimal.TryParse(
                txtHeSoLuong.Text,
                out decimal value))
            {
                txtHeSoLuong.Text =
                    value.ToString("0.0");
            }
            else
            {
                txtHeSoLuong.Text = "";
            }
        }

        private void txtHoTen_Leave(
            object? sender,
            EventArgs e)
        {
            txtHoTen.Text =
                ValidationHelper
                    .ToTitleCase(
                        txtHoTen.Text);
        }

        private void txtSoHieu_Leave(
            object? sender,
            EventArgs e)
        {
            txtSoHieu.Text =
                txtSoHieu.Text
                    .Trim()
                    .ToUpper();
        }

        private void txtNoiSinh_Leave(
            object? sender,
            EventArgs e)
        {
            txtNoiSinh.Text =
                ValidationHelper
                    .ToTitleCase(
                        txtNoiSinh.Text);
        }

        private void Address_Leave(
            object? sender,
            EventArgs e)
        {
            TextBox txt =
                (TextBox)sender!;

            txt.Text =
                ValidationHelper
                    .ToTitleCase(
                        txt.Text);
        }

        private void UpdateAge()
        {
            int age =
                DateTime.Today.Year
                - dtNgaySinh.Value.Year;

            if (dtNgaySinh.Value.Date >
                DateTime.Today.AddYears(-age))
            {
                age--;
            }

            lblTuoi.Text =
                $"- {age} tuổi";
        }
    }
}
