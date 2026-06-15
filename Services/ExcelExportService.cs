using ClosedXML.Excel;
using QuanLyCBCS.Helpers;
using QuanLyCBCS.Models;

namespace QuanLyCBCS.Services;

public class ExcelExportService
{
    public void ExportPersonnel(
        List<Personnel> personnelList,
        string filePath)
    {
        using var workbook =
            new XLWorkbook();

        var chienSi =
            personnelList
            .Where(x => x.PersonnelType == 1)
            .ToList();

        var canBo =
            personnelList
            .Where(x => x.PersonnelType == 2)
            .ToList();

        CreateChienSiSheet(
            workbook,
            chienSi);

        CreateCanBoSheet(
            workbook,
            canBo);

        workbook.SaveAs(filePath);
    }

    // =====================================
    // CHIEN SI
    // =====================================

    private void CreateChienSiSheet(
        XLWorkbook workbook,
        List<Personnel> data)
    {
        var ws =
            workbook.Worksheets
            .Add("CHIEN SI");

        ws.Cell(1, 1).Value =
            "DANH SÁCH CHIẾN SĨ";

        ws.Range(1, 1, 1, 31).Merge();

        ws.Cell(1, 1).Style.Font.Bold = true;
        ws.Cell(1, 1).Style.Font.FontSize = 18;
        ws.Cell(1, 1).Style.Alignment.Horizontal =
            XLAlignmentHorizontalValues.Center;

        ws.Cell(2, 1).Value =
            $"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm}";

        ws.Cell(3, 1).Value =
            $"Người xuất: {CurrentUser.Username}";

        CreateHeader(
            ws,
            5);

        int row = 6;

        for (int i = 0; i < data.Count; i++)
        {
            var p = data[i];

            ws.Cell(row, 1).Value = i + 1;
            ws.Cell(row, 2).Value = p.SoHieu;
            ws.Cell(row, 3).Value = p.HoTen;
            ws.Cell(row, 4).Value = p.DonVi;
            ws.Cell(row, 5).Value = p.GioiTinh;
            ws.Cell(row, 6).Value = FormatDate(p.NgaySinh);
            ws.Cell(row, 7).Value = p.NoiSinh;

            ws.Cell(row, 8).Value = p.QueQuanMoi;
            ws.Cell(row, 9).Value = p.QueQuanCu;

            ws.Cell(row, 10).Value = p.HKTTMoi;
            ws.Cell(row, 11).Value = p.HKTTCu;

            ws.Cell(row, 12).Value = p.ViTriCongTac;

            ws.Cell(row, 13).Value = p.CapBacHam;
            ws.Cell(row, 14).Value = FormatDate(p.NgayNhanCapBac);

            ws.Cell(row, 15).Value =
                FormatMonthYear(
                    p.ThangNamVaoNganh);

            ws.Cell(row, 16).Value =
                CalculateThamNien(
                    p.ThangNamVaoNganh);

            ws.Cell(row, 17).Value =
                FormatMonthYear(
                    p.ThangNamNhanCongTac);

            ws.Cell(row, 18).Value =
                CalculateThamNien(
                    p.ThangNamNhanCongTac);

            ws.Cell(row, 19).Value =
                p.TrinhDoTinHoc;

            ws.Cell(row, 20).Value =
                p.TrinhDoNgoaiNgu;

            ws.Cell(row, 21).Value =
                p.TrinhDoVanHoa;

            ws.Cell(row, 22).Value =
                p.TrinhDoChuyenMon;

            ws.Cell(row, 23).Value =
                p.ChuyenNganhDaoTao;

            ws.Cell(row, 24).Value =
                FormatMonthYear(
                    p.ThangNamTotNghiep);

            ws.Cell(row, 25).Value =
                FormatDangVien(
                    p.DangVien);

            ws.Cell(row, 26).Value =
                FormatDate(
                    p.NgayVaoDangDuBi);

            ws.Cell(row, 27).Value =
                FormatDate(
                    p.NgayVaoDangChinhThuc);

            ws.Cell(row, 28).Value =
                p.TrinhDoLyLuanChinhTri;

            ws.Cell(row, 29).Value =
                p.CCCD;

            ws.Cell(row, 30).Value =
                FormatDate(
                    p.NgayCapCCCD);

            ws.Cell(row, 31).Value =
                p.SoDienThoai;

            row++;
        }

        FormatWorksheet(
            ws,
            31,
            5); ;
            }

    // =====================================
    // CAN BO
    // =====================================

    private void CreateCanBoSheet(
        XLWorkbook workbook,
        List<Personnel> data)
    {
        var ws =
            workbook.Worksheets
            .Add("CAN BO");

        ws.Cell(1, 1).Value =
            "DANH SÁCH CÁN BỘ";

        ws.Range(1, 1, 1, 34).Merge();

        ws.Cell(1, 1).Style.Font.Bold = true;
        ws.Cell(1, 1).Style.Font.FontSize = 18;
        ws.Cell(1, 1).Style.Alignment.Horizontal =
            XLAlignmentHorizontalValues.Center;

        ws.Cell(2, 1).Value =
            $"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm}";

        ws.Cell(3, 1).Value =
            $"Người xuất: {CurrentUser.Username}";

        CreateHeader(
            ws,
            5);

        ws.Cell(5, 32).Value = "Hệ số lương";
        ws.Cell(5, 33).Value = "Chức vụ";
        ws.Cell(5, 34).Value = "Ngày bổ nhiệm";

        int row = 6;

        for (int i = 0; i < data.Count; i++)
        {
            var p = data[i];

            ws.Cell(row, 1).Value = i + 1;
            ws.Cell(row, 2).Value = p.SoHieu;
            ws.Cell(row, 3).Value = p.HoTen;
            ws.Cell(row, 4).Value = p.DonVi;
            ws.Cell(row, 5).Value = p.GioiTinh;
            ws.Cell(row, 6).Value = FormatDate(p.NgaySinh);
            ws.Cell(row, 7).Value = p.NoiSinh;

            ws.Cell(row, 8).Value = p.QueQuanMoi;
            ws.Cell(row, 9).Value = p.QueQuanCu;

            ws.Cell(row, 10).Value = p.HKTTMoi;
            ws.Cell(row, 11).Value = p.HKTTCu;

            ws.Cell(row, 12).Value = p.ViTriCongTac;

            ws.Cell(row, 13).Value = p.CapBacHam;
            ws.Cell(row, 14).Value = FormatDate(p.NgayNhanCapBac);

            ws.Cell(row, 15).Value =
                FormatMonthYear(
                    p.ThangNamVaoNganh);

            ws.Cell(row, 16).Value =
                CalculateThamNien(
                    p.ThangNamVaoNganh);

            ws.Cell(row, 17).Value =
                FormatMonthYear(
                    p.ThangNamNhanCongTac);

            ws.Cell(row, 18).Value =
                CalculateThamNien(
                    p.ThangNamNhanCongTac);

            ws.Cell(row, 19).Value =
                p.TrinhDoTinHoc;

            ws.Cell(row, 20).Value =
                p.TrinhDoNgoaiNgu;

            ws.Cell(row, 21).Value =
                p.TrinhDoVanHoa;

            ws.Cell(row, 22).Value =
                p.TrinhDoChuyenMon;

            ws.Cell(row, 23).Value =
                p.ChuyenNganhDaoTao;

            ws.Cell(row, 24).Value =
                FormatMonthYear(
                    p.ThangNamTotNghiep);

            ws.Cell(row, 25).Value =
                FormatDangVien(
                    p.DangVien);

            ws.Cell(row, 26).Value =
                FormatDate(
                    p.NgayVaoDangDuBi);

            ws.Cell(row, 27).Value =
                FormatDate(
                    p.NgayVaoDangChinhThuc);

            ws.Cell(row, 28).Value =
                p.TrinhDoLyLuanChinhTri;

            ws.Cell(row, 29).Value =
                p.CCCD;

            ws.Cell(row, 30).Value =
                FormatDate(
                    p.NgayCapCCCD);

            ws.Cell(row, 31).Value =
                p.SoDienThoai;

            ws.Cell(row, 32).Value =
                p.HeSoLuong;

            ws.Cell(row, 33).Value =
                p.ChucVu;

            ws.Cell(row, 34).Value =
                FormatDate(
                    p.NgayBoNhiem);

            row++;
        }

        FormatWorksheet(
            ws,
            34,
            5);
            }

    private void CreateHeader(
        IXLWorksheet ws,
        int headerRow)
    {
        ws.Cell(headerRow, 1).Value = "STT";
        ws.Cell(headerRow, 2).Value = "Số hiệu";
        ws.Cell(headerRow, 3).Value = "Họ tên";
        ws.Cell(headerRow, 4).Value = "Đơn vị";
        ws.Cell(headerRow, 5).Value = "Giới tính";
        ws.Cell(headerRow, 6).Value = "Ngày sinh";
        ws.Cell(headerRow, 7).Value = "Nơi sinh";
        ws.Cell(headerRow, 8).Value = "Quê quán mới";
        ws.Cell(headerRow, 9).Value = "Quê quán cũ";
        ws.Cell(headerRow, 10).Value = "HKTT mới";
        ws.Cell(headerRow, 11).Value = "HKTT cũ";
        ws.Cell(headerRow, 12).Value = "Vị trí công tác";
        ws.Cell(headerRow, 13).Value = "Cấp bậc hàm";
        ws.Cell(headerRow, 14).Value = "Ngày nhận cấp bậc";
        ws.Cell(headerRow, 15).Value = "Vào ngành";
        ws.Cell(headerRow, 16).Value = "Thâm niên ngành";
        ws.Cell(headerRow, 17).Value = "Nhận công tác";
        ws.Cell(headerRow, 18).Value = "Thâm niên đơn vị";
        ws.Cell(headerRow, 19).Value = "Tin học";
        ws.Cell(headerRow, 20).Value = "Ngoại ngữ";
        ws.Cell(headerRow, 21).Value = "Văn hóa";
        ws.Cell(headerRow, 22).Value = "Chuyên môn";
        ws.Cell(headerRow, 23).Value = "Chuyên ngành";
        ws.Cell(headerRow, 24).Value = "Tốt nghiệp";
        ws.Cell(headerRow, 25).Value = "Đảng viên";
        ws.Cell(headerRow, 26).Value = "Đảng dự bị";
        ws.Cell(headerRow, 27).Value = "Đảng chính thức";
        ws.Cell(headerRow, 28).Value = "LLCT";
        ws.Cell(headerRow, 29).Value = "CCCD";
        ws.Cell(headerRow, 30).Value = "Ngày cấp CCCD";
        ws.Cell(headerRow, 31).Value = "Số điện thoại";
    }

    private void FormatWorksheet(
        IXLWorksheet ws,
        int totalColumns,
        int headerRow)
    {
        var headerRange =
            ws.Range(
                headerRow,
                1,
                headerRow,
                totalColumns);

        headerRange.Style.Font.Bold = true;

        headerRange.Style.Alignment.Horizontal =
            XLAlignmentHorizontalValues.Center;

        headerRange.Style.Fill.BackgroundColor =
            XLColor.LightBlue;

        ws.SheetView.FreezeRows(5);

        var usedRange =
            ws.RangeUsed();

        if (usedRange != null)
        {
            usedRange.Style.Border.OutsideBorder =
                XLBorderStyleValues.Thin;

            usedRange.Style.Border.InsideBorder =
                XLBorderStyleValues.Thin;
        }

        // FILTER CHỈ TỪ HEADER TRỞ XUỐNG
        var lastRow =
            ws.LastRowUsed();

        if (lastRow != null)
        {
            ws.Range(
                headerRow,
                1,
                lastRow.RowNumber(),
                totalColumns)
                .SetAutoFilter();
        }

        ws.Columns().AdjustToContents();
    }

    private string FormatDate(
    DateTime? date)
    {
        return date?.ToString("dd/MM/yyyy")
               ?? string.Empty;
    }

    private string FormatMonthYear(
    DateTime? date)
    {
        return date?.ToString("MM/yyyy")
               ?? string.Empty;
    }

    private string FormatDangVien(
        bool dangVien)
    {
        return dangVien
            ? "Có"
            : "Không";
    }

    private string CalculateThamNien(
    DateTime? fromDate)
    {
        if (fromDate is not DateTime start)
            return string.Empty;

        DateTime now = DateTime.Now;

        int years = now.Year - start.Year;
        int months = now.Month - start.Month;

        if (months < 0)
        {
            years--;
            months += 12;
        }

        return $"{years} năm {months} tháng";
    }
}