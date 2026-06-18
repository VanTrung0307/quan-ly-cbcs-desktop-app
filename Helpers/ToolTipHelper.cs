using QuanLyCBCS.Forms.PersonnelForms;
using QuanLyCBCS.Forms.MissionForms;

namespace QuanLyCBCS.Helpers
{
    public static class ToolTipHelper
    {
        public static void SetupPersonnelDetailToolTips(
            FrmPersonnelDetail frm,
            ToolTip toolTip)
        {
            #region TAB THÔNG TIN CƠ BẢN

            toolTip.SetToolTip(
                frm.txtSoHieu,
                "Số hiệu CBCS");

            toolTip.SetToolTip(
                frm.txtHoTen,
                "Họ và tên đầy đủ");

            toolTip.SetToolTip(
                frm.cboPersonnelType,
                "Chọn loại cán bộ chiến sĩ");

            toolTip.SetToolTip(
                frm.cboGioiTinh,
                "Chọn giới tính");

            toolTip.SetToolTip(
                frm.dtNgaySinh,
                "Ngày tháng năm sinh");

            toolTip.SetToolTip(
                frm.txtNoiSinh,
                "Nơi sinh theo giấy khai sinh");

            toolTip.SetToolTip(
                frm.txtCCCD,
                "Căn cước công dân");

            toolTip.SetToolTip(
                frm.dtNgayCapCCCD,
                "Ngày cấp CCCD");

            toolTip.SetToolTip(
                frm.txtSoDienThoai,
                "Số điện thoại liên hệ");

            #endregion

            #region TAB ĐỊA CHỈ

            toolTip.SetToolTip(
                frm.txtQueQuanMoi,
                "Quê quán theo đơn vị hành chính mới");

            toolTip.SetToolTip(
                frm.txtQueQuanCu,
                "Quê quán theo đơn vị hành chính trước sáp nhập");

            toolTip.SetToolTip(
                frm.txtHKTTMoi,
                "Hộ khẩu thường trú theo đơn vị hành chính mới");

            toolTip.SetToolTip(
                frm.txtHKTTCu,
                "Hộ khẩu thường trú theo đơn vị hành chính trước sáp nhập");

            toolTip.SetToolTip(
                frm.txtTinhMoi,
                "Tỉnh hoặc thành phố sau sáp nhập");

            toolTip.SetToolTip(
                frm.txtTinhCu,
                "Tỉnh hoặc thành phố trước sáp nhập");

            #endregion

            #region TAB CÔNG TÁC

            toolTip.SetToolTip(
                frm.txtDonVi,
                "Đơn vị hiện đang công tác");

            toolTip.SetToolTip(
                frm.txtViTriCongTac,
                "Vị trí công tác hiện tại");

            toolTip.SetToolTip(
                frm.cboCapBacHam,
                "Chọn cấp bậc hàm hiện tại");

            toolTip.SetToolTip(
                frm.txtChucVu,
                "Chức vụ hiện tại");

            toolTip.SetToolTip(
                frm.dtNgayNhanCapBac,
                "Ngày nhận cấp bậc hiện tại");

            toolTip.SetToolTip(
                frm.dtNgayBoNhiem,
                "Ngày được bổ nhiệm chức vụ");

            toolTip.SetToolTip(
                frm.dtThangNamNhanCongTac,
                "Thời điểm nhận công tác tại đơn vị");

            toolTip.SetToolTip(
                frm.dtThangNamVaoNganh,
                "Thời điểm chính thức vào ngành");

            toolTip.SetToolTip(
                frm.txtHeSoLuong,
                "Hệ số lương hiện hưởng");

            #endregion

            #region TAB ĐÀO TẠO

            toolTip.SetToolTip(
                frm.txtTrinhDoTinHoc,
                "Trình độ tin học: A, B, C, MOS, IC3, Ứng dụng CNTT cơ bản...");

            toolTip.SetToolTip(
                frm.txtTrinhDoNgoaiNgu,
                "Trình độ ngoại ngữ: A1, B1, IELTS, TOEIC, HSK, JLPT...");

            toolTip.SetToolTip(
                frm.txtTrinhDoVanHoa,
                "Trình độ văn hóa hoặc học vấn cao nhất");

            toolTip.SetToolTip(
                frm.txtTrinhDoChuyenMon,
                "Chuyên môn chính được đào tạo");

            toolTip.SetToolTip(
                frm.txtChuyenNganhDaoTao,
                "Chuyên ngành đào tạo theo văn bằng");

            toolTip.SetToolTip(
                frm.dtThangNamTotNghiep,
                "Chọn tháng và năm tốt nghiệp");

            toolTip.SetToolTip(
                frm.txtTrinhDoLyLuanChinhTri,
                "Trình độ lý luận chính trị hiện có");

            #endregion

            #region TAB ĐẢNG

            toolTip.SetToolTip(
                frm.chkDangVien,
                "Tích chọn nếu cán bộ chiến sĩ là đảng viên Đảng Cộng sản Việt Nam");

            toolTip.SetToolTip(
                frm.dtDangDuBi,
                "Ngày được kết nạp vào Đảng với tư cách đảng viên dự bị");

            toolTip.SetToolTip(
                frm.dtDangChinhThuc,
                "Ngày được công nhận là đảng viên chính thức");

            #endregion
        }

        public static void SetupPersonnelDetailToolTips(
            FrmMissionDetail frm,
            ToolTip toolTip)
        {

            toolTip.SetToolTip(
                frm.txtMissionName,
                "Nhập tên công tác");

            toolTip.SetToolTip(
                frm.cboMissionType,
                "Chọn loại công tác");

            toolTip.SetToolTip(
                frm.txtLocation,
                "Địa điểm thực hiện công tác");

            toolTip.SetToolTip(
                frm.dtpStartTime,
                "Thời gian bắt đầu");

            toolTip.SetToolTip(
                frm.dtpEndTime,
                "Thời gian kết thúc");

            toolTip.SetToolTip(
                frm.rtbDescription,
                "Ghi chú hoặc mô tả công tác");

            toolTip.SetToolTip(
                frm.chkIsActive,
                "Đánh dấu công tác đang hoạt động");
        }
    }
}