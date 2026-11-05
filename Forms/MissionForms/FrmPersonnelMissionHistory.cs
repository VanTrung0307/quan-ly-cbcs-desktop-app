using QuanLyCBCS.Forms.Auth;
using System.Diagnostics;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyCBCS.DTOs;
using QuanLyCBCS.Services;

namespace QuanLyCBCS.Forms.MissionForms
{
    public partial class FrmPersonnelMissionHistory : Form
    {
        private readonly int _personnelId;
        private readonly string _hoTen;
        private readonly MissionPersonnelService _missionPersonnelService = new();
        private List<PersonnelMissionHistoryDto> _history = new();

        public FrmPersonnelMissionHistory(int personnelId, string hoTen)
        {
            InitializeComponent();

            UpdateHeaderBackground();
            pnlHeader.Resize += (_, _) => UpdateHeaderBackground();
            dgvHistory.Resize += DgvHistory_Resize;
            UpdateHistoryGridRegion();

            _personnelId = personnelId;
            _hoTen = hoTen;

            lblPersonnelInfo.Text = $"CBCS: {hoTen}";
            Load += FrmPersonnelMissionHistory_Load;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            btnExcel.Click += BtnExcel_Click;
            btnRefresh.Click += BtnRefresh_Click;
            btnClose.Click += (_, _) => Close();
        }

        private async void FrmPersonnelMissionHistory_Load(object? sender, EventArgs e)
        {
            await LoadHistoryAsync();
        }

        private async void BtnRefresh_Click(object? sender, EventArgs e)
        {
            txtSearch.Clear();
            await LoadHistoryAsync();
        }

        /// <summary>
        /// Xuất danh sách lịch sử công tác đang hiển thị (tôn trọng nội dung tìm kiếm).
        /// </summary>
        private void BtnExcel_Click(object? sender, EventArgs e)
        {
            // Lấy đúng các dòng đang hiển thị trên bảng, vì người dùng có thể đang lọc.
            List<PersonnelMissionHistoryDto> exportItems = dgvHistory.Rows
                .Cast<DataGridViewRow>()
                .Where(row => !row.IsNewRow)
                .Select(row => row.DataBoundItem)
                .OfType<PersonnelMissionHistoryDto>()
                .ToList();

            if (exportItems.Count == 0)
            {
                using var emptyDialog = new FrmLoginMessage(
                    LoginMessageType.Information,
                    "Thông báo",
                    "Không có dữ liệu lịch sử công tác để xuất Excel.");
                emptyDialog.ShowDialog(this);
                return;
            }

            string safeName = MakeSafeFileName(_hoTen);
            using SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "Excel Workbook (*.xlsx)|*.xlsx",
                Title = "Xuất lịch sử công tác CBCS",
                FileName = $"LichSuCongTac_{safeName}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
                DefaultExt = "xlsx",
                AddExtension = true,
                OverwritePrompt = true
            };

            if (saveDialog.ShowDialog(this) != DialogResult.OK)
                return;

            string filePath = saveDialog.FileName;

            try
            {
                ExportHistoryToExcel(exportItems, filePath);

                using (var successDialog = new FrmLoginMessage(
                    LoginMessageType.Success,
                    "Xuất Excel thành công",
                    $"Đã xuất {exportItems.Count:N0} bản ghi lịch sử công tác của CBCS {_hoTen}."))
                {
                    successDialog.ShowDialog(this);
                }

                // Mở tệp bằng ứng dụng mặc định sau khi lưu thành công.
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = filePath,
                        UseShellExecute = true
                    });
                }
                catch
                {
                    // Không ảnh hưởng đến tệp đã xuất nếu máy không mở được Excel.
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    this,
                    $"Không thể xuất tệp Excel.\nChi tiết: {ex.Message}",
                    "Lỗi xuất Excel",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ExportHistoryToExcel(
            IReadOnlyList<PersonnelMissionHistoryDto> items,
            string filePath)
        {
            using XLWorkbook workbook = new XLWorkbook();
            IXLWorksheet worksheet = workbook.Worksheets.Add("Lịch sử công tác");

            // Tiêu đề báo cáo.
            worksheet.Cell(1, 1).Value = "LỊCH SỬ CÔNG TÁC CỦA CBCS";
            worksheet.Range(1, 1, 1, 6).Merge();
            worksheet.Cell(1, 1).Style.Font.Bold = true;
            worksheet.Cell(1, 1).Style.Font.FontSize = 16;
            worksheet.Cell(1, 1).Style.Font.FontColor = XLColor.White;
            worksheet.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#07509A");
            worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell(1, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Row(1).Height = 30;

            // Thông tin CBCS và thời điểm xuất.
            worksheet.Cell(2, 1).Value = "CBCS:";
            worksheet.Cell(2, 1).Style.Font.Bold = true;
            worksheet.Range(2, 2, 2, 6).Merge();
            worksheet.Cell(2, 2).Value = _hoTen;

            worksheet.Cell(3, 1).Value = "Ngày xuất:";
            worksheet.Cell(3, 1).Style.Font.Bold = true;
            worksheet.Cell(3, 2).Value = DateTime.Now;
            worksheet.Cell(3, 2).Style.DateFormat.Format = "dd/MM/yyyy HH:mm:ss";

            // Tiêu đề cột tương ứng với bảng trên màn hình.
            string[] headers =
            {
                "STT", "Ngày phân công", "Tên công tác",
                "Loại công tác", "Ghi chú", "Địa điểm"
            };

            for (int column = 0; column < headers.Length; column++)
                worksheet.Cell(5, column + 1).Value = headers[column];

            var headerRange = worksheet.Range(5, 1, 5, headers.Length);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#004591");
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            worksheet.Row(5).Height = 25;

            // Dữ liệu: xuất đúng tập bản ghi đang hiển thị sau khi tìm kiếm/lọc.
            int row = 6;
            for (int index = 0; index < items.Count; index++, row++)
            {
                PersonnelMissionHistoryDto item = items[index];

                worksheet.Cell(row, 1).Value = index + 1;
                worksheet.Cell(row, 2).Value = Convert.ToDateTime(item.NgayPhanCong);
                worksheet.Cell(row, 2).Style.DateFormat.Format = "dd/MM/yyyy HH:mm";
                worksheet.Cell(row, 3).Value = item.TenCongTac ?? string.Empty;
                worksheet.Cell(row, 4).Value = item.LoaiCongTac ?? string.Empty;
                worksheet.Cell(row, 5).Value = item.GhiChu ?? string.Empty;
                worksheet.Cell(row, 6).Value = GetOptionalText(item, "DiaDiem") ?? string.Empty;

                if (index % 2 == 1)
                {
                    worksheet.Range(row, 1, row, 6).Style.Fill.BackgroundColor =
                        XLColor.FromHtml("#F0F7FF");
                }
            }

            int lastRow = row - 1;
            var dataRange = worksheet.Range(5, 1, lastRow, 6);
            dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            dataRange.Style.Border.InsideBorderColor = XLColor.FromHtml("#B7CDE5");
            dataRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            worksheet.Range(6, 1, lastRow, 1).Style.Alignment.Horizontal =
                XLAlignmentHorizontalValues.Center;
            worksheet.Range(6, 2, lastRow, 2).Style.Alignment.Horizontal =
                XLAlignmentHorizontalValues.Center;
            worksheet.Range(6, 4, lastRow, 4).Style.Alignment.Horizontal =
                XLAlignmentHorizontalValues.Center;
            worksheet.Range(6, 5, lastRow, 6).Style.Alignment.WrapText = true;

            dataRange.SetAutoFilter();
            worksheet.SheetView.FreezeRows(5);
            worksheet.Columns(1, 6).AdjustToContents();

            // Giới hạn độ rộng để tệp dễ đọc, không bị kéo quá rộng bởi ghi chú dài.
            worksheet.Column(1).Width = 8;
            worksheet.Column(2).Width = 21;
            worksheet.Column(3).Width = Math.Min(Math.Max(worksheet.Column(3).Width, 24), 42);
            worksheet.Column(4).Width = Math.Min(Math.Max(worksheet.Column(4).Width, 18), 28);
            worksheet.Column(5).Width = Math.Min(Math.Max(worksheet.Column(5).Width, 22), 45);
            worksheet.Column(6).Width = Math.Min(Math.Max(worksheet.Column(6).Width, 18), 35);

            worksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;
            worksheet.PageSetup.FitToPages(1, 0);
            worksheet.PageSetup.Margins.Top = 0.5;
            worksheet.PageSetup.Margins.Bottom = 0.5;
            worksheet.PageSetup.Margins.Left = 0.3;
            worksheet.PageSetup.Margins.Right = 0.3;

            workbook.SaveAs(filePath);
        }

        private static string MakeSafeFileName(string value)
        {
            string result = value ?? string.Empty;
            foreach (char invalid in System.IO.Path.GetInvalidFileNameChars())
                result = result.Replace(invalid, '_');

            return string.IsNullOrWhiteSpace(result) ? "CBCS" : result.Trim();
        }

        private async Task LoadHistoryAsync()
        {
            _history = await _missionPersonnelService.GetPersonnelMissionHistoryAsync(_personnelId);
            ApplyHistoryFilter();
        }

        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            ApplyHistoryFilter();
        }

        private void ApplyHistoryFilter()
        {
            string keyword = txtSearch.Text.Trim();

            IEnumerable<PersonnelMissionHistoryDto> query = _history;
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(item =>
                    Contains(item.TenCongTac, keyword) ||
                    Contains(item.LoaiCongTac, keyword) ||
                    Contains(item.GhiChu, keyword) ||
                    Contains(GetOptionalText(item, "DiaDiem"), keyword) ||
                    Convert.ToDateTime(item.NgayPhanCong).ToString("dd/MM/yyyy HH:mm").Contains(keyword,
                        StringComparison.CurrentCultureIgnoreCase));
            }

            var displayed = query.ToList();
            dgvHistory.DataSource = null;
            dgvHistory.DataSource = displayed;

            for (int i = 0; i < dgvHistory.Rows.Count; i++)
                dgvHistory.Rows[i].Cells["colStt"].Value = i + 1;

            lblTotal.Text = $"Tổng: {displayed.Count:N0} bản ghi";
            CustomizeGrid();
        }

        private static bool Contains(string? source, string keyword)
        {
            return !string.IsNullOrEmpty(source) &&
                   source.IndexOf(keyword, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        private static string? GetOptionalText(object? source, string propertyName)
        {
            return source?.GetType().GetProperty(propertyName)?.GetValue(source)?.ToString();
        }

        private void CustomizeGrid()
        {
            if (dgvHistory.Columns["colNgayPhanCong"] != null)
            {
                dgvHistory.Columns["colNgayPhanCong"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            }

            dgvHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHistory.ClearSelection();
            dgvHistory.Invalidate();
        }

        #region Header, cards and buttons painting

        private void UpdateHeaderBackground()
        {
            if (pnlHeader.Width <= 0 || pnlHeader.Height <= 0)
                return;

            var bitmap = new Bitmap(pnlHeader.Width, pnlHeader.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                var bounds = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                using var gradient = new LinearGradientBrush(
                    bounds, Color.FromArgb(0, 105, 213), Color.FromArgb(0, 52, 119), 0F);
                g.FillRectangle(gradient, bounds);
                using var overlay = new LinearGradientBrush(
                    bounds, Color.FromArgb(35, Color.White), Color.FromArgb(0, Color.White), 0F);
                g.FillRectangle(overlay, bounds);
                using var watermarkBrush = new SolidBrush(Color.FromArgb(38, 190, 220, 255));
                using var watermarkFont = new Font("Segoe Fluent Icons", 72F, FontStyle.Regular);
                using var watermarkFormat = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString("\uE734", watermarkFont, watermarkBrush,
                    new RectangleF(bitmap.Width - 220, 5, 190, bitmap.Height - 10), watermarkFormat);
            }

            Image? oldImage = pnlHeader.BackgroundImage;
            pnlHeader.BackgroundImage = bitmap;
            pnlHeader.BackgroundImageLayout = ImageLayout.Stretch;
            oldImage?.Dispose();

            using var rounded = RoundedPath(
                new Rectangle(0, 0, pnlHeader.Width - 1, pnlHeader.Height - 1), 15);
            pnlHeader.Region?.Dispose();
            pnlHeader.Region = new Region(rounded);
        }

        private void UpdateHistoryGridRegion()
        {
            if (dgvHistory.Width <= 0 || dgvHistory.Height <= 0)
                return;

            using var rounded = RoundedPath(
                new Rectangle(0, 0, dgvHistory.Width - 1, dgvHistory.Height - 1), 12);
            dgvHistory.Region?.Dispose();
            dgvHistory.Region = new Region(rounded);
        }

        private void DgvHistory_Resize(object? sender, EventArgs e)
        {
            UpdateHistoryGridRegion();
        }

        private void PnlHeader_Paint(object? sender, PaintEventArgs e)
        {
            // Banner gradient and watermark are rendered into BackgroundImage.
        }

        private void RoundedCard_Paint(object? sender, PaintEventArgs e)
        {
            Panel panel = (Panel)sender!;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle bounds = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
            using var path = RoundedPath(bounds, 14);
            using var fill = new SolidBrush(Color.FromArgb(225, 239, 255));
            using var pen = new Pen(Color.FromArgb(215, 232, 252), 1F);
            e.Graphics.FillPath(fill, path);
            e.Graphics.DrawPath(pen, path);
        }

        private void RoundedSearch_Paint(object? sender, PaintEventArgs e)
        {
            Panel panel = (Panel)sender!;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle bounds = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
            using var path = RoundedPath(bounds, 16);
            using var fill = new SolidBrush(Color.White);
            using var pen = new Pen(Color.FromArgb(195, 218, 246), 1.3F);
            e.Graphics.FillPath(fill, path);
            e.Graphics.DrawPath(pen, path);
        }

        private void ToolbarSeparator_Paint(object? sender, PaintEventArgs e)
        {
            Panel panel = (Panel)sender!;
            using var pen = new Pen(Color.FromArgb(183, 207, 236), 1F);
            int y1 = Math.Max(8, (panel.Height - 42) / 2);
            e.Graphics.DrawLine(pen, panel.Width / 2, y1, panel.Width / 2, y1 + 42);
        }

        private void PersonnelInfoIcon_Paint(object? sender, PaintEventArgs e)
        {
            Label label = (Label)sender!;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var iconFont = new Font("Segoe Fluent Icons", 21F, FontStyle.Regular);
            using var brush = new SolidBrush(Color.FromArgb(0, 91, 190));
            using var format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            e.Graphics.DrawString("\uE77B", iconFont, brush,
                new RectangleF(13, 0, 34, label.Height), format);
        }

        private void ToolbarButton_Paint(object? sender, PaintEventArgs e)
        {
            Button button = (Button)sender!;
            string kind = Convert.ToString(button.Tag) ?? "";
            bool filled = kind == "excel";
            Color accent = Color.FromArgb(0, 104, 213);
            Color foreground = filled ? Color.White :
                kind == "close" ? Color.FromArgb(20, 43, 75) : accent;
            bool hovered = button.ClientRectangle.Contains(button.PointToClient(Cursor.Position));
            Color fill = filled ? Color.FromArgb(0, 169, 86) : Color.White;
            Color border = kind == "close" ? Color.FromArgb(111, 132, 158) :
                filled ? fill : accent;
            if (hovered)
            {
                fill = kind == "excel" ? Color.FromArgb(0, 143, 72) :
                    kind == "refresh" ? Color.FromArgb(230, 242, 255) :
                    Color.FromArgb(239, 244, 250);
                border = kind == "close"
                    ? Color.FromArgb(75, 103, 137)
                    : kind == "excel" ? Color.FromArgb(0, 130, 65) : Color.FromArgb(0, 82, 180);
            }

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.Clear(Color.FromArgb(242, 247, 253));

            Rectangle face = new Rectangle(1, 1, Math.Max(1, button.Width - 3), Math.Max(1, button.Height - 3));
            using var path = RoundedPath(face, 11);
            using var background = new SolidBrush(fill);
            using var outline = new Pen(border, 1.25F);
            g.FillPath(background, path);
            g.DrawPath(outline, path);

            string glyph = kind == "excel" ? "\uE9F9" :
                kind == "refresh" ? "\uE72C" : "\uE8BB";
            string caption = kind == "excel" ? "Excel" :
                kind == "refresh" ? "Làm mới" : "Đóng";

            using var iconFont = new Font("Segoe Fluent Icons", 13F, FontStyle.Regular);
            using var textFont = new Font("Segoe UI", 12F, FontStyle.Bold);
            SizeF textSize = g.MeasureString(caption, textFont);
            float iconWidth = 26F;
            float gap = 5F;
            float groupWidth = iconWidth + gap + textSize.Width;
            float startX = Math.Max(5F, (button.Width - groupWidth) / 2F);

            using var iconBrush = new SolidBrush(foreground);
            using var textBrush = new SolidBrush(foreground);
            using var iconFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            using var textFormat = new StringFormat
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center,
                FormatFlags = StringFormatFlags.NoWrap
            };

            // Fluent glyphs have asymmetric side bearings; shift the icon
            // slightly right without moving the caption or the centered group.
            float iconVisualOffset = 5F;
            g.DrawString(glyph, iconFont, iconBrush,
                new RectangleF(startX + iconVisualOffset, 0, iconWidth - 4F, button.Height), iconFormat);
            g.DrawString(caption, textFont, textBrush,
                new RectangleF(startX + iconWidth + gap, 0, textSize.Width + 3, button.Height), textFormat);
        }

        private void RoundedFooter_Paint(object? sender, PaintEventArgs e)
        {
            Panel panel = (Panel)sender!;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle bounds = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
            using var path = RoundedPath(bounds, 14);
            using var brush = new LinearGradientBrush(
                bounds, Color.FromArgb(225, 240, 255), Color.FromArgb(211, 231, 255), 0F);
            e.Graphics.FillPath(brush, path);
            using var pen = new Pen(Color.FromArgb(210, 229, 250), 1F);
            e.Graphics.DrawPath(pen, path);
        }

        private void GridCard_Paint(object? sender, PaintEventArgs e)
        {
            Panel panel = (Panel)sender!;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle bounds = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
            using var path = RoundedPath(bounds, 14);
            using var fill = new SolidBrush(Color.FromArgb(242, 247, 253));
            using var pen = new Pen(Color.FromArgb(190, 214, 241), 1.3F);
            e.Graphics.FillPath(fill, path);
            e.Graphics.DrawPath(pen, path);
        }

        private static GraphicsPath RoundedPath(Rectangle bounds, int radius)
        {
            int diameter = Math.Max(2, radius * 2);
            var path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }

        #endregion

        #region Grid cell rendering

        private void DgvHistory_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            string column = dgvHistory.Columns[e.ColumnIndex].Name;
            if (column != "colNgayPhanCong" && column != "colLoaiCongTac" && column != "colDiaDiem")
                return;

            // DataGridViewCellPaintingEventArgs.Graphics is nullable in the
            // reference annotations; guard it once before drawing.
            Graphics? graphics = e.Graphics;
            if (graphics is null)
            {
                e.Handled = true;
                return;
            }

            e.PaintBackground(e.ClipBounds, true);
            string value = Convert.ToString(e.FormattedValue) ?? "";
            Rectangle bounds = e.CellBounds;

            if (column == "colLoaiCongTac")
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    e.Handled = true;
                    return;
                }

                using var font = new Font("Segoe UI", 11F, FontStyle.Bold);
                SizeF textSize = graphics.MeasureString(value, font);
                float pillWidth = Math.Min(bounds.Width - 18, textSize.Width + 48);
                float pillHeight = 36F;
                float x = bounds.X + (bounds.Width - pillWidth) / 2F;
                float y = bounds.Y + (bounds.Height - pillHeight) / 2F;
                using var pillPath = RoundedPath(Rectangle.Round(new RectangleF(x, y, pillWidth, pillHeight)), 18);
                using var pillBrush = new SolidBrush(Color.FromArgb(224, 242, 255));
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.FillPath(pillBrush, pillPath);

                using var iconFont = new Font("Segoe Fluent Icons", 16F, FontStyle.Regular);
                using var iconBrush = new SolidBrush(Color.FromArgb(0, 94, 190));
                using var textBrush = new SolidBrush(Color.FromArgb(0, 91, 190));
                using var center = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                graphics.DrawString("\uE77B", iconFont, iconBrush,
                    new RectangleF(x + 7, y, 24, pillHeight), center);
                graphics.DrawString(value, font, textBrush,
                    new RectangleF(x + 32, y, Math.Max(1, pillWidth - 37), pillHeight), center);
                e.Handled = true;
                return;
            }

            bool isDate = column == "colNgayPhanCong";
            string glyph = isDate ? "\uE787" : "\uE707";
            using var glyphFont = new Font("Segoe Fluent Icons", 17F, FontStyle.Regular);
            using var glyphBrush = new SolidBrush(Color.FromArgb(0, 91, 190));
            using var textFont = new Font("Segoe UI", 12F, FontStyle.Regular);
            using var textBrush2 = new SolidBrush(Color.FromArgb(10, 35, 72));
            using var format = new StringFormat { LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.NoWrap };

            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.DrawString(glyph, glyphFont, glyphBrush,
                new RectangleF(bounds.X + 14, bounds.Y, 28, bounds.Height),
                new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            graphics.DrawString(value, textFont, textBrush2,
                new RectangleF(bounds.X + 48, bounds.Y, Math.Max(1, bounds.Width - 54), bounds.Height), format);
            e.Handled = true;
        }

        private void DgvHistory_DataError(object? sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        #endregion
    }
}
