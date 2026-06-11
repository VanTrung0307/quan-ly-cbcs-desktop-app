using ClosedXML.Excel;
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

        CreateHeader(ws);

        int row = 2;

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

        FormatWorksheet(ws, 31);
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

        CreateHeader(ws);

        ws.Cell(1, 32).Value = "Hệ số lương";
        ws.Cell(1, 33).Value = "Chức vụ";
        ws.Cell(1, 34).Value = "Ngày bổ nhiệm";

        int row = 2;

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

        FormatWorksheet(ws, 34);
    }

    private void CreateHeader(
        IXLWorksheet ws)
    {
        ws.Cell(1, 1).Value = "STT";
        ws.Cell(1, 2).Value = "Số hiệu";
        ws.Cell(1, 3).Value = "Họ tên";
        ws.Cell(1, 4).Value = "Đơn vị";
        ws.Cell(1, 5).Value = "Giới tính";
        ws.Cell(1, 6).Value = "Ngày sinh";
        ws.Cell(1, 7).Value = "Nơi sinh";
        ws.Cell(1, 8).Value = "Quê quán mới";
        ws.Cell(1, 9).Value = "Quê quán cũ";
        ws.Cell(1, 10).Value = "HKTT mới";
        ws.Cell(1, 11).Value = "HKTT cũ";
        ws.Cell(1, 12).Value = "Vị trí công tác";
        ws.Cell(1, 13).Value = "Cấp bậc hàm";
        ws.Cell(1, 14).Value = "Ngày nhận cấp bậc";
        ws.Cell(1, 15).Value = "Vào ngành";
        ws.Cell(1, 16).Value = "Thâm niên ngành";
        ws.Cell(1, 17).Value = "Nhận công tác";
        ws.Cell(1, 18).Value = "Thâm niên đơn vị";
        ws.Cell(1, 19).Value = "Tin học";
        ws.Cell(1, 20).Value = "Ngoại ngữ";
        ws.Cell(1, 21).Value = "Văn hóa";
        ws.Cell(1, 22).Value = "Chuyên môn";
        ws.Cell(1, 23).Value = "Chuyên ngành";
        ws.Cell(1, 24).Value = "Tốt nghiệp";
        ws.Cell(1, 25).Value = "Đảng viên";
        ws.Cell(1, 26).Value = "Đảng dự bị";
        ws.Cell(1, 27).Value = "Đảng chính thức";
        ws.Cell(1, 28).Value = "LLCT";
        ws.Cell(1, 29).Value = "CCCD";
        ws.Cell(1, 30).Value = "Ngày cấp CCCD";
        ws.Cell(1, 31).Value = "Số điện thoại";
    }

    private void FormatWorksheet(
        IXLWorksheet ws,
        int totalColumns)
    {
        var headerRange =
            ws.Range(
                1,
                1,
                1,
                totalColumns);

        headerRange.Style.Font.Bold = true;

        headerRange.Style.Alignment.Horizontal =
            XLAlignmentHorizontalValues.Center;

        headerRange.Style.Fill.BackgroundColor =
            XLColor.LightBlue;

        ws.SheetView.FreezeRows(1);

        var usedRange =
            ws.RangeUsed();

        if (usedRange != null)
        {
            usedRange.Style.Border
                .OutsideBorder =
                XLBorderStyleValues.Thin;

            usedRange.Style.Border
                .InsideBorder =
                XLBorderStyleValues.Thin;

            usedRange.SetAutoFilter();
        }

        ws.Column(1)
            .Style.Alignment.Horizontal =
            XLAlignmentHorizontalValues.Center;

        ws.Column(6)
            .Style.Alignment.Horizontal =
            XLAlignmentHorizontalValues.Center;

        ws.Column(14)
            .Style.Alignment.Horizontal =
            XLAlignmentHorizontalValues.Center;

        ws.Column(24)
            .Style.Alignment.Horizontal =
            XLAlignmentHorizontalValues.Center;

        ws.Column(26)
            .Style.Alignment.Horizontal =
            XLAlignmentHorizontalValues.Center;

        ws.Column(27)
            .Style.Alignment.Horizontal =
            XLAlignmentHorizontalValues.Center;

        ws.Column(30)
            .Style.Alignment.Horizontal =
            XLAlignmentHorizontalValues.Center;

        ws.Columns()
            .AdjustToContents();
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