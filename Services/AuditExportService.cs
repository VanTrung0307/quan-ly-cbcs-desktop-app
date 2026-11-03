using ClosedXML.Excel;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyCBCS.Services
{
    public class AuditExportService
    {
        public void Export(DataGridView dgv)
        {
            if (dgv == null)
                throw new ArgumentNullException(nameof(dgv));

            using SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "Excel Workbook (*.xlsx)|*.xlsx",
                Title = "Xuất nhật ký hệ thống",
                FileName = $"AuditLog_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
                DefaultExt = "xlsx",
                AddExtension = true,
                OverwritePrompt = true
            };

            if (saveDialog.ShowDialog(dgv.FindForm()) != DialogResult.OK)
                return;

            string fileName = saveDialog.FileName;

            using XLWorkbook workbook = new XLWorkbook();

            IXLWorksheet worksheet =
                workbook.Worksheets.Add("Audit Log");

            // Tiêu đề báo cáo.
            worksheet.Cell("A1").Value = "NHẬT KÝ HỆ THỐNG";
            worksheet.Range("A1:E1").Merge();
            worksheet.Cell("A1").Style.Font.Bold = true;
            worksheet.Cell("A1").Style.Font.FontSize = 18;
            worksheet.Cell("A1").Style.Alignment.Horizontal =
                XLAlignmentHorizontalValues.Center;

            worksheet.Cell("A3").Value =
                $"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";

            // Header: chỉ xuất các cột đang hiển thị.
            int column = 1;

            foreach (DataGridViewColumn dgvColumn in dgv.Columns)
            {
                if (!dgvColumn.Visible)
                    continue;

                worksheet.Cell(5, column).Value = dgvColumn.HeaderText;
                column++;
            }

            int totalColumns = column - 1;

            if (totalColumns <= 0)
                return;

            var header = worksheet.Range(5, 1, 5, totalColumns);
            header.Style.Font.Bold = true;
            header.Style.Fill.BackgroundColor = XLColor.LightSteelBlue;
            header.Style.Alignment.Horizontal =
                XLAlignmentHorizontalValues.Center;

            // Dữ liệu.
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
                        dgvRow.Cells[dgvColumn.Name].Value?.ToString() ?? string.Empty;

                    column++;
                }

                row++;
            }

            int totalRows = dgv.Rows
                .Cast<DataGridViewRow>()
                .Count(r => !r.IsNewRow);

            if (row > 6)
            {
                worksheet.Range(5, 1, row - 1, totalColumns)
                    .SetAutoFilter();
            }

            // Đường viền bảng.
            var usedRange = worksheet.RangeUsed();

            if (usedRange != null)
            {
                usedRange.Style.Border.OutsideBorder =
                    XLBorderStyleValues.Thin;
                usedRange.Style.Border.InsideBorder =
                    XLBorderStyleValues.Thin;
            }

            // Tự căn độ rộng và cố định phần tiêu đề.
            worksheet.Columns().AdjustToContents();
            worksheet.SheetView.FreezeRows(5);

            // Lưu file trước khi thông báo thành công.
            workbook.SaveAs(fileName);

            using (var successDialog =
                new ExportSuccessDialog(totalRows, fileName))
            {
                successDialog.ShowDialog(dgv.FindForm());
            }

            // Giữ nguyên hành vi mở file Excel sau khi xuất.
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = fileName,
                    UseShellExecute = true
                });
            }
            catch
            {
                // File vẫn được xuất thành công; bỏ qua nếu máy không mở được Excel.
            }
        }
    }

    /// <summary>
    /// Hộp thoại xác nhận xuất Excel thành công.
    /// </summary>
    internal sealed class ExportSuccessDialog : Form
    {
        private readonly int _recordCount;
        private readonly string _filePath;

        private readonly Panel _headerPanel;
        private readonly Panel _bodyPanel;
        private readonly Panel _footerPanel;
        private readonly Label _pathValueLabel;
        private readonly Button _copyButton;
        private readonly RoundedOkButton _okButton;
        private readonly Button _closeButton;

        private readonly Color _navy = Color.FromArgb(0, 55, 125);
        private readonly Color _blue = Color.FromArgb(0, 105, 205);
        private readonly Color _lightBlue = Color.FromArgb(232, 241, 253);
        private readonly Color _textColor = Color.FromArgb(32, 52, 82);

        public ExportSuccessDialog(int recordCount, string filePath)
        {
            _recordCount = recordCount;
            _filePath = filePath;

            Text = "Thông báo";
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            ClientSize = new Size(560, 300);
            MinimumSize = new Size(520, 280);
            BackColor = Color.White;
            Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            DoubleBuffered = true;
            KeyPreview = true;

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.White,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 58F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));

            _headerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = _navy,
                Margin = Padding.Empty
            };
            _headerPanel.Paint += HeaderPanel_Paint;

            _bodyPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Margin = Padding.Empty
            };

            _footerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = _lightBlue,
                Margin = Padding.Empty
            };

            layout.Controls.Add(_headerPanel, 0, 0);
            layout.Controls.Add(_bodyPanel, 0, 1);
            layout.Controls.Add(_footerPanel, 0, 2);
            Controls.Add(layout);

            _pathValueLabel = new Label();
            _copyButton = new Button();
            _okButton = new RoundedOkButton();
            _closeButton = new Button();

            BuildHeader();
            BuildBody();
            BuildFooter();

            Resize += (_, _) => ApplyRoundedRegion();
            Shown += (_, _) =>
            {
                ApplyRoundedRegion();
                _okButton.Focus();
            };

            KeyDown += (_, e) =>
            {
                if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
            };
        }

        private void BuildHeader()
        {
            var infoCircle = new Panel
            {
                Size = new Size(36, 36),
                Location = new Point(20, 11),
                BackColor = Color.Transparent
            };
            infoCircle.Paint += (_, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var circleBrush = new SolidBrush(Color.White);
                using var textBrush = new SolidBrush(_navy);
                using var iconFont = new Font("Segoe UI", 20F, FontStyle.Bold);
                e.Graphics.FillEllipse(circleBrush, 1, 1,
                    infoCircle.Width - 3, infoCircle.Height - 3);
                var iconSize = e.Graphics.MeasureString("i", iconFont);
                e.Graphics.DrawString("i", iconFont, textBrush,
                    (infoCircle.Width - iconSize.Width) / 2F,
                    (infoCircle.Height - iconSize.Height) / 2F - 2F);
            };

            var title = new Label
            {
                Text = "Thông báo",
                AutoSize = true,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(66, 15),
                BackColor = Color.Transparent
            };

            _closeButton.Text = string.Empty;
            _closeButton.Size = new Size(34, 34);
            _closeButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _closeButton.Location = new Point(ClientSize.Width - 44, 11);
            _closeButton.FlatStyle = FlatStyle.Flat;
            _closeButton.FlatAppearance.BorderSize = 0;
            _closeButton.FlatAppearance.MouseOverBackColor =
                Color.FromArgb(35, 255, 255, 255);
            _closeButton.FlatAppearance.MouseDownBackColor =
                Color.FromArgb(55, 255, 255, 255);
            _closeButton.BackColor = Color.Transparent;
            _closeButton.ForeColor = Color.White;
            _closeButton.Font = new Font("Segoe UI Light", 22F);
            _closeButton.Cursor = Cursors.Hand;
            _closeButton.Click += (_, _) => Close();
            _closeButton.Paint += (_, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var pen = new Pen(Color.White, 2.2F);
                e.Graphics.DrawLine(pen, 10, 10, 24, 24);
                e.Graphics.DrawLine(pen, 24, 10, 10, 24);
            };

            _headerPanel.Controls.Add(infoCircle);
            _headerPanel.Controls.Add(title);
            _headerPanel.Controls.Add(_closeButton);
        }

        private void BuildBody()
        {
            // Biểu tượng thông tin lớn bên trái, gồm quầng sáng và vòng tròn xanh.
            var infoBadge = new Panel
            {
                Size = new Size(68, 68),
                Location = new Point(27, 48),
                BackColor = Color.Transparent
            };
            infoBadge.Paint += (_, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var outer = new SolidBrush(Color.FromArgb(218, 239, 255));
                using var inner = new SolidBrush(Color.FromArgb(0, 132, 224));
                using var iconFont = new Font("Segoe UI", 39F, FontStyle.Bold);
                using var iconBrush = new SolidBrush(Color.White);
                e.Graphics.FillEllipse(outer, 0, 0, 68, 68);
                e.Graphics.FillEllipse(inner, 8, 8, 52, 52);
                var glyph = e.Graphics.MeasureString("i", iconFont);
                e.Graphics.DrawString("i", iconFont, iconBrush,
                    (68 - glyph.Width) / 2F, (68 - glyph.Height) / 2F - 2F);
            };

            // Đường phân cách dọc giữa biểu tượng và phần nội dung.
            var separator = new Panel
            {
                BackColor = Color.FromArgb(0, 125, 225),
                Location = new Point(108, 18),
                Size = new Size(2, 140)
            };

            var heading = new Label
            {
                Text = "Xuất Excel thành công!",
                AutoSize = true,
                Font = new Font("Segoe UI", 17F, FontStyle.Bold),
                ForeColor = _navy,
                Location = new Point(132, 13),
                BackColor = Color.Transparent
            };

            // Icon tài liệu được vẽ bằng vector để không phụ thuộc font ký tự.
            var recordsIcon = new Panel
            {
                Location = new Point(132, 53),
                Size = new Size(22, 25),
                BackColor = Color.Transparent
            };
            recordsIcon.Paint += (_, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var pen = new Pen(_blue, 1.8F);
                e.Graphics.DrawRectangle(pen, 2, 1, 17, 22);
                e.Graphics.DrawLine(pen, 5, 8, 16, 8);
                e.Graphics.DrawLine(pen, 5, 13, 16, 13);
                e.Graphics.DrawLine(pen, 5, 18, 14, 18);
            };

            var recordsText = new Label
            {
                Text = $"Đã xuất: {_recordCount:N0} bản ghi.",
                AutoSize = true,
                Font = new Font("Segoe UI", 11.5F, FontStyle.Regular),
                ForeColor = _textColor,
                Location = new Point(165, 56),
                BackColor = Color.Transparent
            };

            // Icon thư mục cũng được vẽ riêng, không dùng glyph dễ gây đè chữ.
            var folderIcon = new Panel
            {
                Location = new Point(130, 91),
                Size = new Size(25, 22),
                BackColor = Color.Transparent
            };
            folderIcon.Paint += (_, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var pen = new Pen(_blue, 1.8F);
                using var path = new GraphicsPath();
                path.AddLine(2, 6, 2, 19);
                path.AddLine(2, 19, 23, 19);
                path.AddLine(23, 19, 23, 7);
                path.AddLine(23, 7, 12, 7);
                path.AddLine(12, 7, 9, 3);
                path.AddLine(9, 3, 2, 3);
                path.CloseFigure();
                e.Graphics.DrawPath(pen, path);
                e.Graphics.DrawLine(pen, 2, 8, 23, 8);
            };

            var pathCaption = new Label
            {
                Text = "Đường dẫn:",
                AutoSize = true,
                Font = new Font("Segoe UI", 11.5F, FontStyle.Regular),
                ForeColor = _textColor,
                Location = new Point(165, 91),
                BackColor = Color.Transparent
            };

            var pathContainer = new Panel
            {
                Location = new Point(165, 128),
                Size = new Size(378, 35),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = Color.FromArgb(247, 250, 255)
            };
            pathContainer.Paint += (_, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var pen = new Pen(Color.FromArgb(190, 210, 235), 1.3F);
                using var path = CreateRoundedPath(
                    new Rectangle(0, 0, pathContainer.Width - 1,
                        pathContainer.Height - 1), 9);
                e.Graphics.DrawPath(pen, path);
            };

            _pathValueLabel.Text = _filePath;
            _pathValueLabel.AutoEllipsis = true;
            _pathValueLabel.TextAlign = ContentAlignment.MiddleLeft;
            _pathValueLabel.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular);
            _pathValueLabel.ForeColor = _textColor;
            _pathValueLabel.BackColor = Color.Transparent;
            _pathValueLabel.Location = new Point(10, 2);
            _pathValueLabel.Size = new Size(320, 31);
            _pathValueLabel.Anchor = AnchorStyles.Top |
                                     AnchorStyles.Left |
                                     AnchorStyles.Right;

            // Nút sao chép: vẽ hai tờ giấy chồng nhau, giống biểu tượng trong mẫu.
            _copyButton.Text = string.Empty;
            _copyButton.ForeColor = _navy;
            _copyButton.BackColor = Color.Transparent;
            _copyButton.FlatStyle = FlatStyle.Flat;
            _copyButton.FlatAppearance.BorderSize = 0;
            _copyButton.FlatAppearance.MouseOverBackColor =
                Color.FromArgb(225, 238, 255);
            _copyButton.FlatAppearance.MouseDownBackColor =
                Color.FromArgb(205, 225, 250);
            _copyButton.Size = new Size(38, 31);
            _copyButton.Location = new Point(pathContainer.Width - 40, 2);
            _copyButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _copyButton.Cursor = Cursors.Hand;
            _copyButton.Click += CopyButton_Click;
            _copyButton.Paint += CopyButton_VectorPaint;

            var toolTip = new ToolTip();
            toolTip.SetToolTip(_copyButton, "Sao chép đường dẫn");

            pathContainer.Controls.Add(_pathValueLabel);
            pathContainer.Controls.Add(_copyButton);

            _bodyPanel.Controls.Add(infoBadge);
            _bodyPanel.Controls.Add(separator);
            _bodyPanel.Controls.Add(heading);
            _bodyPanel.Controls.Add(recordsIcon);
            _bodyPanel.Controls.Add(recordsText);
            _bodyPanel.Controls.Add(folderIcon);
            _bodyPanel.Controls.Add(pathCaption);
            _bodyPanel.Controls.Add(pathContainer);

            _bodyPanel.Resize += (_, _) =>
            {
                // Giữ mép phải của ô đường dẫn cách mép hộp thoại 16 px.
                pathContainer.Width = Math.Max(300, _bodyPanel.ClientSize.Width - 181);
                _pathValueLabel.Width = pathContainer.Width - 56;
                _copyButton.Left = pathContainer.Width - _copyButton.Width - 2;
            };
        }

        private void BuildFooter()
        {
            _okButton.Text = "✓   OK";
            _okButton.Size = new Size(126, 44);
            _okButton.Location = new Point(
                _footerPanel.ClientSize.Width - 150, 10);
            _okButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _okButton.BackColor = _footerPanel.BackColor;
            _okButton.ForeColor = Color.White;
            _okButton.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            _okButton.BaseColor = Color.FromArgb(0, 105, 205);
            _okButton.HoverColor = Color.FromArgb(0, 91, 185);
            _okButton.PressedColor = Color.FromArgb(0, 78, 165);
            _okButton.BorderColor = Color.White;
            _okButton.CornerRadius = 10;
            _okButton.BorderThickness = 2;
            _okButton.Cursor = Cursors.Hand;
            _okButton.Click += (_, _) =>
            {
                DialogResult = DialogResult.OK;
                Close();
            };
            _footerPanel.Controls.Add(_okButton);

            _footerPanel.Resize += (_, _) =>
            {
                _okButton.Left = _footerPanel.ClientSize.Width -
                                 _okButton.Width - 20;
            };
        }

        private void CopyButton_VectorPaint(object? sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var pen = new Pen(_navy, 1.8F);
            e.Graphics.DrawRectangle(pen, 13, 6, 15, 17);
            e.Graphics.DrawRectangle(pen, 8, 10, 15, 17);
        }

        private void CopyButton_Click(object? sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(_filePath);
                _copyButton.Text = "✓";
                _copyButton.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
                _copyButton.Paint -= CopyButton_VectorPaint;
                _copyButton.ForeColor = Color.FromArgb(0, 150, 80);
                var tip = new ToolTip();
                tip.Show("Đã sao chép đường dẫn!", _copyButton,
                    0, -28, 1600);
            }
            catch
            {
                MessageBox.Show(this,
                    "Không thể sao chép đường dẫn vào bộ nhớ tạm.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void HeaderPanel_Paint(object? sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var brush = new LinearGradientBrush(
                _headerPanel.ClientRectangle,
                Color.FromArgb(0, 57, 128),
                Color.FromArgb(0, 112, 195),
                LinearGradientMode.Horizontal);
            e.Graphics.FillRectangle(brush, _headerPanel.ClientRectangle);

            // Họa tiết trang trí mờ ở phía phải header.
            using var decorationPen = new Pen(
                Color.FromArgb(48, 180, 220, 255), 2F);
            e.Graphics.DrawArc(decorationPen,
                _headerPanel.Width - 310, -95, 270, 190, 15, 145);
            e.Graphics.DrawArc(decorationPen,
                _headerPanel.Width - 265, -75, 220, 150, 20, 140);
        }

        private void ApplyRoundedRegion()
        {
            if (Width <= 0 || Height <= 0)
                return;

            using var path = CreateRoundedPath(
                new Rectangle(0, 0, Width, Height), 14);
            Region = new Region(path);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var pen = new Pen(Color.FromArgb(185, 210, 238), 1.5F);
            using var path = CreateRoundedPath(
                new Rectangle(1, 1, Width - 3, Height - 3), 14);
            e.Graphics.DrawPath(pen, path);
        }

        private static GraphicsPath CreateRoundedPath(
            Rectangle bounds, int radius)
        {
            int diameter = Math.Max(1, radius * 2);
            var path = new GraphicsPath();

            path.AddArc(bounds.Left, bounds.Top,
                diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Top,
                diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter,
                diameter, diameter, 0, 90);
            path.AddArc(bounds.Left, bounds.Bottom - diameter,
                diameter, diameter, 90, 90);
            path.CloseFigure();

            return path;
        }
    }

    /// <summary>
    /// Nút OK tự vẽ: cả nền xanh và đường viền trắng đều có góc bo tròn,
    /// tránh hiện tượng chỉ bo góc phần viền nhưng nền xanh vẫn vuông.
    /// </summary>
    internal sealed class RoundedOkButton : Button
    {
        public Color BaseColor { get; set; } = Color.FromArgb(0, 105, 205);
        public Color HoverColor { get; set; } = Color.FromArgb(0, 91, 185);
        public Color PressedColor { get; set; } = Color.FromArgb(0, 78, 165);
        public Color BorderColor { get; set; } = Color.White;
        public int CornerRadius { get; set; } = 10;
        public int BorderThickness { get; set; } = 2;

        private bool _hovered;
        private bool _pressed;

        public RoundedOkButton()
        {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            BackColor = Color.Transparent;
            Cursor = Cursors.Hand;
            TabStop = true;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _hovered = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _hovered = false;
            _pressed = false;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                _pressed = true;
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _pressed = false;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // Xóa nền bằng màu panel cha để bốn góc bo hiển thị đúng nền footer.
            Color parentColor = Parent != null ? Parent.BackColor : SystemColors.Control;
            e.Graphics.Clear(parentColor);

            int inset = Math.Max(1, BorderThickness);
            Rectangle fillBounds = new Rectangle(
                inset, inset,
                Math.Max(1, ClientSize.Width - inset * 2 - 1),
                Math.Max(1, ClientSize.Height - inset * 2 - 1));

            using (GraphicsPath fillPath = CreateRoundedPath(fillBounds, CornerRadius))
            using (SolidBrush fillBrush = new SolidBrush(
                _pressed ? PressedColor : (_hovered ? HoverColor : BaseColor)))
            {
                e.Graphics.FillPath(fillBrush, fillPath);
            }

            Rectangle borderBounds = new Rectangle(
                1, 1, Math.Max(1, ClientSize.Width - 3),
                Math.Max(1, ClientSize.Height - 3));
            using (GraphicsPath borderPath = CreateRoundedPath(borderBounds, CornerRadius))
            using (Pen borderPen = new Pen(BorderColor, BorderThickness))
            {
                borderPen.Alignment = PenAlignment.Center;
                e.Graphics.DrawPath(borderPen, borderPath);
            }

            TextRenderer.DrawText(
                e.Graphics,
                Text,
                Font,
                ClientRectangle,
                ForeColor,
                TextFormatFlags.HorizontalCenter |
                TextFormatFlags.VerticalCenter |
                TextFormatFlags.SingleLine |
                TextFormatFlags.NoPadding);
        }

        private static GraphicsPath CreateRoundedPath(Rectangle bounds, int radius)
        {
            int diameter = Math.Max(2, Math.Min(radius * 2,
                Math.Min(bounds.Width, bounds.Height)));
            var path = new GraphicsPath();
            path.AddArc(bounds.Left, bounds.Top, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Top, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter,
                diameter, diameter, 0, 90);
            path.AddArc(bounds.Left, bounds.Bottom - diameter,
                diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
