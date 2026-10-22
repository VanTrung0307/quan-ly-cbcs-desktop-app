
using ClosedXML.Excel;
using QuanLyCBCS.Helpers;
using QuanLyCBCS.Models;
using System.Globalization;

namespace QuanLyCBCS.Services;

public class ExcelImportService
{
    public List<Personnel> ReadPersonnel(string filePath)
    {
        using XLWorkbook workbook = new(filePath);

        List<Personnel> result = new();

        bool foundValidSheet = false;

        foreach (IXLWorksheet worksheet in workbook.Worksheets)
        {
            int personnelType;

            string sheetName = NormalizeHeader(worksheet.Name);

            if (sheetName == "chien si")
                personnelType = 1;
            else if (sheetName == "can bo")
                personnelType = 2;
            else
                continue;

            foundValidSheet = true;

            IXLRow? lastUsedRow = worksheet.LastRowUsed();

            if (lastUsedRow == null)
                continue;

            int lastRow = lastUsedRow.RowNumber();
            int lastColumn =
                worksheet.LastColumnUsed()?.ColumnNumber() ?? 0;

            if (lastColumn == 0)
                continue;

            // Tìm hàng tiêu đề thay vì mặc định hàng đầu tiên.
            // File xuất hiện tại có tiêu đề ở hàng 5.
            int headerRow = 0;

            Dictionary<string, int> columns = new();

            for (int row = 1; row <= Math.Min(lastRow, 20); row++)
            {
                Dictionary<string, int> candidate = new();

                for (int col = 1; col <= lastColumn; col++)
                {
                    string header = NormalizeHeader(
                        worksheet.Cell(row, col).GetString());

                    if (!string.IsNullOrWhiteSpace(header))
                        candidate[header] = col;
                }

                if (candidate.ContainsKey("so hieu")
                    && candidate.ContainsKey("ho ten"))
                {
                    headerRow = row;
                    columns = candidate;
                    break;
                }
            }

            if (headerRow == 0)
            {
                throw new InvalidDataException(
                    $"Sheet '{worksheet.Name}' không tìm thấy " +
                    "hàng tiêu đề có cột Số hiệu và Họ tên.");
            }

            for (int row = headerRow + 1; row <= lastRow; row++)
            {
                string soHieu = ReadText(
                    worksheet, row, columns, "so hieu");

                string hoTen = ReadText(
                    worksheet, row, columns, "ho ten");

                // Bỏ qua dòng trống hoặc dòng chỉ có STT.
                if (string.IsNullOrWhiteSpace(soHieu)
                    && string.IsNullOrWhiteSpace(hoTen))
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(soHieu))
                {
                    throw new InvalidDataException(
                        $"Sheet '{worksheet.Name}', dòng {row}: " +
                        "chưa có Số hiệu.");
                }

                if (string.IsNullOrWhiteSpace(hoTen))
                {
                    throw new InvalidDataException(
                        $"Sheet '{worksheet.Name}', dòng {row}: " +
                        "chưa có Họ tên.");
                }

                Personnel p = new()
                {
                    // Phân loại theo sheet, không lấy STT làm ID.
                    PersonnelType = personnelType,

                    SoHieu = soHieu,
                    HoTen = hoTen,

                    DonVi = ReadText(
                        worksheet, row, columns, "don vi"),

                    GioiTinh = ReadText(
                        worksheet, row, columns, "gioi tinh"),

                    NgaySinh = ReadDate(
                        worksheet, row, columns, "ngay sinh"),

                    NoiSinh = ReadText(
                        worksheet, row, columns, "noi sinh"),

                    QueQuanMoi = ReadText(
                        worksheet, row, columns, "que quan moi"),

                    QueQuanCu = ReadText(
                        worksheet, row, columns, "que quan cu"),

                    HKTTMoi = ReadText(
                        worksheet, row, columns, "hktt moi"),

                    HKTTCu = ReadText(
                        worksheet, row, columns, "hktt cu"),

                    ViTriCongTac = ReadText(
                        worksheet, row, columns, "vi tri cong tac"),

                    CapBacHam = ReadText(
                        worksheet, row, columns, "cap bac ham"),

                    NgayNhanCapBac = ReadDate(
                        worksheet, row, columns, "ngay nhan cap bac"),

                    ThangNamVaoNganh = ReadDate(
                        worksheet, row, columns, "vao nganh"),

                    ThangNamNhanCongTac = ReadDate(
                        worksheet, row, columns, "nhan cong tac"),

                    TrinhDoTinHoc = ReadText(
                        worksheet, row, columns, "tin hoc"),

                    TrinhDoNgoaiNgu = ReadText(
                        worksheet, row, columns, "ngoai ngu"),

                    TrinhDoVanHoa = ReadText(
                        worksheet, row, columns, "van hoa"),

                    TrinhDoChuyenMon = ReadText(
                        worksheet, row, columns, "chuyen mon"),

                    ChuyenNganhDaoTao = ReadText(
                        worksheet, row, columns, "chuyen nganh"),

                    ThangNamTotNghiep = ReadDate(
                        worksheet, row, columns, "tot nghiep"),

                    DangVien = ReadBoolean(
                        worksheet, row, columns, "dang vien"),

                    NgayVaoDangDuBi = ReadDate(
                        worksheet, row, columns, "dang du bi"),

                    NgayVaoDangChinhThuc = ReadDate(
                        worksheet, row, columns, "dang chinh thuc"),

                    TrinhDoLyLuanChinhTri = ReadText(
                        worksheet, row, columns, "llct"),

                    CCCD = ReadText(
                        worksheet, row, columns, "cccd"),

                    NgayCapCCCD = ReadDate(
                        worksheet, row, columns, "ngay cap cccd"),

                    SoDienThoai = ReadText(
                        worksheet, row, columns, "so dien thoai"),

                    HeSoLuong = ReadDecimal(
                        worksheet, row, columns, "he so luong"),

                    ChucVu = ReadText(
                        worksheet, row, columns, "chuc vu"),

                    NgayBoNhiem = ReadDate(
                        worksheet, row, columns, "ngay bo nhiem")
                };

                // TinhMoi và TinhCu không có cột tương ứng
                // trong file Excel xuất hiện tại nên để null.
                // STT và các cột thâm niên là dữ liệu tính toán,
                // không nhập ngược vào database.

                if (!string.IsNullOrWhiteSpace(p.GioiTinh)
                    && p.GioiTinh != "Nam"
                    && p.GioiTinh != "Nữ")
                {
                    throw new InvalidDataException(
                        $"Sheet '{worksheet.Name}', dòng {row}: " +
                        "Giới tính phải là Nam hoặc Nữ.");
                }

                result.Add(p);
            }
        }

        if (!foundValidSheet)
        {
            throw new InvalidDataException(
                "File không có sheet 'CHIEN SI' hoặc 'CAN BO'.");
        }

        if (result.Count == 0)
        {
            throw new InvalidDataException(
                "Không tìm thấy hồ sơ CBCS nào trong file.");
        }

        return result;
    }

    private static string NormalizeHeader(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return "";

        string normalized =
            VietnameseHelper.RemoveVietnameseSigns(value)
                .Trim()
                .ToLowerInvariant();

        return string.Join(
            " ",
            normalized.Split(
                (char[]?)null,
                StringSplitOptions.RemoveEmptyEntries));
    }

    private static string ReadText(
        IXLWorksheet worksheet,
        int row,
        Dictionary<string, int> columns,
        string header)
    {
        if (!columns.TryGetValue(header, out int col))
            return "";

        return worksheet.Cell(row, col)
            .GetFormattedString()
            .Trim();
    }

    private static DateTime? ReadDate(
        IXLWorksheet worksheet,
        int row,
        Dictionary<string, int> columns,
        string header)
    {
        if (!columns.TryGetValue(header, out int col))
            return null;

        IXLCell cell = worksheet.Cell(row, col);

        if (cell.IsEmpty())
            return null;

        if (cell.TryGetValue<DateTime>(out DateTime date))
            return date;

        string value = cell.GetFormattedString().Trim();

        if (string.IsNullOrWhiteSpace(value))
            return null;

        string[] formats =
        {
            "dd/MM/yyyy",
            "d/M/yyyy",
            "dd-MM-yyyy",
            "d-M-yyyy",
            "MM/yyyy",
            "M/yyyy",
            "yyyy-MM-dd"
        };

        if (DateTime.TryParseExact(
            value,
            formats,
            CultureInfo.GetCultureInfo("vi-VN"),
            DateTimeStyles.None,
            out date))
        {
            // Tháng/năm được quy về ngày đầu tháng.
            if (value.Length <= 7)
                return new DateTime(date.Year, date.Month, 1);

            return date;
        }

        if (DateTime.TryParse(
            value,
            CultureInfo.GetCultureInfo("vi-VN"),
            DateTimeStyles.None,
            out date))
        {
            return date;
        }

        throw new InvalidDataException(
            $"Cột '{header}', dòng Excel {row}: " +
            $"ngày '{value}' không hợp lệ.");
    }

    private static decimal? ReadDecimal(
        IXLWorksheet worksheet,
        int row,
        Dictionary<string, int> columns,
        string header)
    {
        if (!columns.TryGetValue(header, out int col))
            return null;

        IXLCell cell = worksheet.Cell(row, col);

        if (cell.IsEmpty())
            return null;

        if (cell.TryGetValue<decimal>(out decimal number))
            return number;

        string value = cell.GetFormattedString().Trim();

        if (string.IsNullOrWhiteSpace(value))
            return null;

        if (decimal.TryParse(
            value,
            NumberStyles.Any,
            CultureInfo.GetCultureInfo("vi-VN"),
            out number))
        {
            return number;
        }

        if (decimal.TryParse(
            value,
            NumberStyles.Any,
            CultureInfo.InvariantCulture,
            out number))
        {
            return number;
        }

        throw new InvalidDataException(
            $"Cột '{header}', dòng Excel {row}: " +
            $"giá trị '{value}' không phải số hợp lệ.");
    }

    private static bool ReadBoolean(
        IXLWorksheet worksheet,
        int row,
        Dictionary<string, int> columns,
        string header)
    {
        string value = NormalizeHeader(
            ReadText(worksheet, row, columns, header));

        if (string.IsNullOrWhiteSpace(value)
            || value == "khong"
            || value == "no"
            || value == "false"
            || value == "0")
        {
            return false;
        }

        if (value == "co"
            || value == "yes"
            || value == "true"
            || value == "1")
        {
            return true;
        }

        throw new InvalidDataException(
            $"Cột '{header}', dòng Excel {row}: " +
            "hãy nhập Có hoặc Không.");
    }
}
