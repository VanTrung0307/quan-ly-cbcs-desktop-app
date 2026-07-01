using ClosedXML.Excel;
using System.Windows.Forms;

namespace QuanLyCBCS.Services
{
    public class AuditExportService
    {
        public void Export(DataGridView dgv)
        {
            using SaveFileDialog saveDialog = new();

            saveDialog.Filter =
                "Excel Workbook (*.xlsx)|*.xlsx";

            saveDialog.Title =
                "Xuất nhật ký hệ thống";

            saveDialog.FileName =
                $"AuditLog_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

            if (saveDialog.ShowDialog() != DialogResult.OK)
                return;

            string fileName = saveDialog.FileName;

            using XLWorkbook workbook = new();

            IXLWorksheet worksheet =
                workbook.Worksheets.Add("Audit Log");

            //====================================================
            // Tiêu đề
            //====================================================

            worksheet.Cell("A1").Value =
                "NHẬT KÝ HỆ THỐNG";

            worksheet.Range("A1:E1").Merge();

            worksheet.Cell("A1").Style.Font.Bold = true;
            worksheet.Cell("A1").Style.Font.FontSize = 18;
            worksheet.Cell("A1").Style.Alignment.Horizontal =
                XLAlignmentHorizontalValues.Center;

            worksheet.Cell("A3").Value =
                $"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";

            //====================================================
            // Header
            //====================================================

            int column = 1;

            foreach (DataGridViewColumn dgvColumn in dgv.Columns)
            {
                if (!dgvColumn.Visible)
                    continue;

                worksheet.Cell(5, column).Value =
                    dgvColumn.HeaderText;

                column++;
            }

            var header =
                worksheet.Range(
                    5,
                    1,
                    5,
                    column - 1);

            header.Style.Font.Bold = true;

            header.Style.Fill.BackgroundColor =
                XLColor.LightSteelBlue;

            header.Style.Alignment.Horizontal =
                XLAlignmentHorizontalValues.Center;

            //====================================================
            // Data
            //====================================================

            int row = 6;

            foreach (DataGridViewRow dgvRow in dgv.Rows)
            {
                if (dgvRow.IsNewRow)
                    continue;

                column = 1;

                foreach (DataGridViewColumn dgvColumn in dgv.Columns)
                {
                    if (!dgvColumn.Visible)
                        continue;

                    worksheet.Cell(row, column).Value =
                        dgvRow.Cells[dgvColumn.Name]
                            .Value?
                            .ToString();

                    column++;
                }

                row++;
            }

            //====================================================
            // Border
            //====================================================

            var usedRange = worksheet.RangeUsed();

            if (usedRange is not null)
            {
                usedRange.Style.Border.OutsideBorder =
                    XLBorderStyleValues.Thin;

                usedRange.Style.Border.InsideBorder =
                    XLBorderStyleValues.Thin;
            }

            //====================================================
            // Auto Fit + Freeze
            //====================================================

            worksheet.Columns().AdjustToContents();

            worksheet.SheetView.FreezeRows(5);

            //====================================================
            // Save
            //====================================================
            
            int totalRows = dgv.Rows
                .Cast<DataGridViewRow>()
                .Count(r => !r.IsNewRow);

            workbook.SaveAs(fileName);

            MessageBox.Show(
                $"Xuất Excel thành công!\n\n" +
                $"Đã xuất: {totalRows} bản ghi.\n\n" +
                $"Đường dẫn:\n{fileName}",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            try
            {
                System.Diagnostics.Process.Start(
                    new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = fileName,
                        UseShellExecute = true
                    });
            }
            catch
            {
                // Không làm gì nếu máy chưa cài Excel
            }
        }
    }
}