using ClosedXML.Excel;
using QuanLyCBCS.DTOs;

namespace QuanLyCBCS.Services;

public class PersonnelMissionStatisticExportService
{
    public void Export(
        List<PersonnelMissionStatisticDto> data,
        string filePath)
    {
        using XLWorkbook workbook = new();

        var worksheet =
            workbook.Worksheets.Add(
                "ThongKeCongTac");

        worksheet.Cell("A1").Value =
            "THỐNG KÊ CÔNG TÁC CBCS";

        worksheet.Range("A1:F1")
            .Merge();

        worksheet.Cell("A1")
            .Style
            .Font
            .Bold = true;

        worksheet.Cell("A1")
            .Style
            .Font
            .FontSize = 16;

        worksheet.Cell("A3").Value =
            $"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm}";

        worksheet.Cell("A5").Value = "STT";
        worksheet.Cell("B5").Value = "Số hiệu";
        worksheet.Cell("C5").Value = "Họ tên";
        worksheet.Cell("D5").Value = "Đơn vị";
        worksheet.Cell("E5").Value = "Cấp bậc";
        worksheet.Cell("F5").Value = "Số lần công tác";

        int row = 6;

        foreach (var item in data)
        {
            worksheet.Cell(row, 1).Value =
                row - 5;

            worksheet.Cell(row, 2).Value =
                item.SoHieu;

            worksheet.Cell(row, 3).Value =
                item.HoTen;

            worksheet.Cell(row, 4).Value =
                item.DonVi;

            worksheet.Cell(row, 5).Value =
                item.CapBacHam;

            worksheet.Cell(row, 6).Value =
                item.SoLanCongTac;

            row++;
        }

        worksheet.Columns()
            .AdjustToContents();

        workbook.SaveAs(filePath);
    }
}