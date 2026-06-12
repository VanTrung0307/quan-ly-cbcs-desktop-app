using DocumentFormat.OpenXml.Wordprocessing;
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

        private readonly int _personnelId;

        private readonly AuditLogService _auditService =
    new AuditLogService();
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
        }

        private void chkDangVien_CheckedChanged(
            object sender,
            EventArgs e)
        {
            dtDangDuBi.Enabled =
                chkDangVien.Checked;

            dtDangChinhThuc.Enabled =
                chkDangVien.Checked;
        }

        private async Task<bool> ValidateInputAsync()
        {
            // Họ tên

            if (string.IsNullOrWhiteSpace(
                txtHoTen.Text))
            {
                MessageBox.Show(
                    "Vui lòng nhập họ tên");

                txtHoTen.Focus();

                return false;
            }

            // Số hiệu

            if (string.IsNullOrWhiteSpace(
                txtSoHieu.Text))
            {
                MessageBox.Show(
                    "Vui lòng nhập số hiệu");

                txtSoHieu.Focus();

                return false;
            }

            // CCCD

            if (!string.IsNullOrWhiteSpace(
                txtCCCD.Text))
            {
                if (!ValidationHelper
                    .IsCCCD(txtCCCD.Text))
                {
                    MessageBox.Show(
                        "CCCD không hợp lệ");

                    txtCCCD.Focus();

                    return false;
                }
            }

            // Số điện thoại

            if (!string.IsNullOrWhiteSpace(
                txtSoDienThoai.Text))
            {
                if (!ValidationHelper
                    .IsPhoneNumber(
                        txtSoDienThoai.Text))
                {
                    MessageBox.Show(
                        "Số điện thoại không hợp lệ");

                    txtSoDienThoai.Focus();

                    return false;
                }
            }

            // Trùng số hiệu

            bool soHieuExists =
                await _service
                .IsSoHieuExistsAsync(
                    txtSoHieu.Text);

            if (_mode == FormMode.Add
                && soHieuExists)
            {
                MessageBox.Show(
                    "Số hiệu đã tồn tại");

                txtSoHieu.Focus();

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
                txtCapBacHam.Text.Trim();

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

            await _auditService
                .AddLogAsync(
                    "ADD_PERSONNEL",
                    p.HoTen);

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
            if (_mode == FormMode.Add)
            {
                await SaveAddAsync();
            }
            else if (_mode == FormMode.Edit)
            {
                await SaveEditAsync();
            }
        }

        private async Task LoadPersonnelAsync()
        {

            var p =
                await _service
                .GetByIdAsync(_personnelId);

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

            txtCapBacHam.Text =
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
            if (_mode == FormMode.Edit)
            {
                await LoadPersonnelAsync();
            }
        }

        private async Task SaveEditAsync()
        {
            bool isValid =
                await ValidateInputAsync();

            if (!isValid)
                return;

            var p =
                await _service
                .GetByIdAsync(_personnelId);

            if (p == null)
                return;

            FillPersonnel(p);

            await _service.UpdateAsync(p);

            await _auditService
                .AddLogAsync(
                    "UPDATE_PERSONNEL",
                    p.HoTen);

            MessageBox.Show(
                "Cập nhật thành công");

            DialogResult =
                DialogResult.OK;

            Close();
        }
    }
}
