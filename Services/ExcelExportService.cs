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
    }
}