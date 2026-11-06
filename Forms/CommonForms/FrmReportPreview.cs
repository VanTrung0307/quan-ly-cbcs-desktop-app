using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using QuanLyCBCS.Forms.Auth;

namespace QuanLyCBCS.Forms.CommonForms
{
    public partial class FrmReportPreview : Form
    {
        private readonly string _originalReportText;
        private FlowLayoutPanel _contentFlow = null!;
        private readonly Color navy = Color.FromArgb(12, 31, 70);
        private readonly Color blue = Color.FromArgb(0, 125, 235);
        private readonly Color paleBlue = Color.FromArgb(229, 241, 255);
        private readonly Color borderBlue = Color.FromArgb(202, 222, 250);

        public FrmReportPreview(string reportText)
        {
            InitializeComponent();

            // Vẽ nút bằng GDI+ để biểu tượng và đường viền luôn liền nét.
            btnCopy.Text = string.Empty;
            btnClose.Text = string.Empty;
            btnCopy.FlatStyle = FlatStyle.Flat;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnCopy.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.BorderSize = 0;
            btnCopy.Paint += BtnCopy_Paint;
            btnClose.Paint += BtnClose_Paint;

            _originalReportText = reportText ?? string.Empty;
            BuildReportView(_originalReportText);
            ApplyRoundedCorners();
            Shown += (s, e) =>
            {
                Rectangle workArea = Screen.FromControl(this).WorkingArea;
                int targetWidth = Math.Min((int)(workArea.Width * 0.885), 1760);
                int targetHeight = Math.Min((int)(workArea.Height * 0.94), 980);
                Size = new Size(Math.Max(MinimumSize.Width, targetWidth),
                                Math.Max(MinimumSize.Height, targetHeight));
                StartPosition = FormStartPosition.CenterParent;
                CenterToParent();
                ApplyRoundedCorners();
                LayoutReport();
            };
            pnlReportContainer.Resize += (s, e) => LayoutReport();
        }

        private void BuildReportView(string reportText)
        {
            rtbReport.Text = reportText;
            rtbReport.Visible = false;

            _contentFlow = new FlowLayoutPanel
            {
                Name = "pnlReportContent",
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Color.White,
                Padding = new Padding(0, 2, 0, 12),
                Margin = Padding.Empty
            };
            pnlReportContainer.Controls.Add(_contentFlow);
            _contentFlow.BringToFront();

            string[] lines = (reportText ?? string.Empty)
                .Replace("\r\n", "\n").Replace('\r', '\n')
                .Split('\n').Select(x => x.Trim()).Where(x => x.Length > 0).ToArray();

            string date = FindLine(lines, x => x.IndexOf("ngày", StringComparison.OrdinalIgnoreCase) >= 0);
            // Bảo đảm nhãn ngày luôn hiển thị đầy đủ ngày/tháng/năm nếu nguồn chỉ có "HH:mm ngày".
            if (!string.IsNullOrWhiteSpace(date) &&
                Regex.IsMatch(date, @"\bngày\s*$", RegexOptions.IgnoreCase))
                date = date.TrimEnd() + " " + DateTime.Now.ToString("dd/MM/yyyy");
            string commander = FindLine(lines, x => x.StartsWith("Trực chỉ huy", StringComparison.OrdinalIgnoreCase));
            string duty = FindLine(lines, x => x.StartsWith("Trực ban", StringComparison.OrdinalIgnoreCase));
            string total = FindLine(lines, x => StartsWithAny(x, "Tổng quân số", "Tổng quân số:"));
            string present = FindLine(lines, x => StartsWithAny(x, "Có mặt"));
            string absent = FindLine(lines, x => StartsWithAny(x, "Vắng mặt"));
            // Giữ nguyên tiêu đề nguồn để tìm đúng vị trí trong mảng lines.
            string absentHeading = FindLine(lines, x => x.StartsWith("I.") || x.StartsWith("1."));
            // Kết luận phải dùng cùng nguồn số liệu với thanh Tổng quân số.
            // Tránh lấy nhầm một dòng "Có mặt: 0" ở phần khác của nội dung báo cáo.
            string conclusionPresent = present;
            string conclusionAbsent = absent;

            AddTitle();
            if (!string.IsNullOrWhiteSpace(date)) AddDateBadge(date);
            AddDutyCard(commander, duty);
            AddDivider();
            AddStatistics(total, present, absent);
            AddDivider();

            string displayAbsentHeading = absentHeading;
            if (displayAbsentHeading.Equals("I. DANH SÁCH VẮNG", StringComparison.OrdinalIgnoreCase) ||
                displayAbsentHeading.Equals("I.  DANH SÁCH VẮNG", StringComparison.OrdinalIgnoreCase))
                displayAbsentHeading = "I. DANH SÁCH VẮNG MẶT";

            AddSectionBar(string.IsNullOrWhiteSpace(displayAbsentHeading)
                ? "I. DANH SÁCH VẮNG MẶT" : displayAbsentHeading, "▤");

            // Hiển thị toàn bộ nội dung nằm trong mục danh sách vắng.
            // Chỉ hiện câu "Không có CBCS vắng mặt." khi mục này thực sự không có dữ liệu.
            AddAbsentSectionContent(lines, absentHeading);

            // Các mục tiếp theo (nếu có) vẫn được giữ lại.
            AddRemainingSections(lines, absentHeading);

            AddDivider();
            AddSectionBar("KẾT LUẬN", "⚑");
            AddConclusionCard(
                string.IsNullOrWhiteSpace(conclusionPresent) ? present : conclusionPresent,
                string.IsNullOrWhiteSpace(conclusionAbsent) ? absent : conclusionAbsent);
        }


        private void AddTitle()
        {
            Panel p = CreateBlock(72);
            p.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                int cy = p.Height / 2, center = p.Width / 2;
                using (Pen pen = new Pen(Color.FromArgb(0, 139, 255), 1.5f))
                {
                    e.Graphics.DrawLine(pen, 155, cy, Math.Max(156, center - 300), cy);
                    e.Graphics.DrawLine(pen, center + 300, cy, p.Width - 155, cy);
                }
                DrawBars(e.Graphics, new Point(center - 248, cy), 34);
            };
            Label title = new Label
            {
                Text = "BÁO CÁO QUÂN SỐ",
                Font = new Font("Segoe UI", 27F, FontStyle.Bold),
                ForeColor = navy,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            p.Controls.Add(title);
            _contentFlow.Controls.Add(p);
        }

        private void DrawBars(Graphics g, Point center, int h)
        {
            using (Pen pen = new Pen(blue, 5))
            {
                g.DrawLine(pen, center.X - 21, center.Y + 15, center.X + 31, center.Y + 15);
                g.DrawLine(pen, center.X - 14, center.Y + 11, center.X - 14, center.Y - 1);
                g.DrawLine(pen, center.X, center.Y + 11, center.X, center.Y - 10);
                g.DrawLine(pen, center.X + 14, center.Y + 11, center.X + 14, center.Y - 23);
            }
        }


        private void AddDateBadge(string date)
        {
            Panel p = CreateBlock(58);
            Panel badge = new Panel
            {
                Name = "pnlDateBadge",
                BackColor = Color.FromArgb(239, 246, 255),
                Size = new Size(340, 50),
                Location = new Point(0, 3)
            };
            badge.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath path = RoundedRect(new Rectangle(0, 0, badge.Width - 1, badge.Height - 1), 9))
                using (Pen pen = new Pen(borderBlue, 1)) e.Graphics.DrawPath(pen, path);
            };
            Panel icon = new Panel { BackColor = Color.Transparent };
            icon.SetBounds(15, 6, 36, 36);
            icon.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(blue, 2.4F))
                {
                    g.DrawRectangle(pen, 5, 7, 25, 23);
                    g.DrawLine(pen, 5, 13, 30, 13);
                    g.DrawLine(pen, 11, 3, 11, 10);
                    g.DrawLine(pen, 24, 3, 24, 10);
                    g.DrawLine(pen, 12, 19, 16, 23);
                    g.DrawLine(pen, 16, 23, 23, 16);
                }
            };
            Label text = MakeLabel(date, 16F, FontStyle.Regular, navy);
            text.SetBounds(62, 5, 272, 38);
            text.AutoSize = false;
            badge.Controls.Add(icon); badge.Controls.Add(text);
            p.Controls.Add(badge);
            ApplyRoundedRegion(badge, 9);
            _contentFlow.Controls.Add(p);
        }


        private void AddDutyCard(string commander, string duty)
        {
            Panel card = CreateBlock(98);
            card.Name = "pnlDutyCard";
            card.BackColor = Color.FromArgb(235, 244, 255);
            card.Padding = new Padding(20, 8, 14, 8);
            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath path = RoundedRect(new Rectangle(0, 0, card.Width - 1, card.Height - 1), 8))
                using (Pen pen = new Pen(borderBlue, 1)) e.Graphics.DrawPath(pen, path);
            };
            AddDutyRow(card, "◉", "Trực chỉ huy", ExtractDutyValue(commander, "Trực chỉ huy", "Chưa cập nhật"), 5);
            AddDutyRow(card, "♟", "Trực ban", ExtractDutyValue(duty, "Trực ban", "Chưa phân công"), 49);
            _contentFlow.Controls.Add(card);
        }



        private string ExtractDutyValue(string line, string label, string fallback)
        {
            if (string.IsNullOrWhiteSpace(line)) return fallback;

            string value = Regex.Replace(
                line.Trim(),
                @"^" + Regex.Escape(label) + @"\s*:?\s*",
                string.Empty,
                RegexOptions.IgnoreCase);

            value = Regex.Replace(value, @"^:\s*", string.Empty).Trim();
            return string.IsNullOrWhiteSpace(value) ? fallback : value;
        }

        private void AddDutyRow(Panel parent, string glyph, string labelText, string valueText, int top)
        {
            Panel icon = new Panel { BackColor = Color.Transparent };
            icon.SetBounds(10, top + 1, 40, 36);
            icon.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (SolidBrush brush = new SolidBrush(blue))
                using (Pen outline = new Pen(blue, 2.0F))
                {
                    if (glyph == "◉")
                    {
                        g.DrawEllipse(outline, 8, 2, 23, 23);
                        g.FillEllipse(brush, 16, 7, 8, 8);
                        g.FillEllipse(brush, 12, 17, 16, 8);
                    }
                    else
                    {
                        g.FillEllipse(brush, 15, 1, 10, 10);
                        g.FillEllipse(brush, 3, 7, 8, 8);
                        g.FillEllipse(brush, 29, 7, 8, 8);
                        g.FillEllipse(brush, 11, 15, 18, 14);
                        g.FillEllipse(brush, 0, 18, 13, 11);
                        g.FillEllipse(brush, 27, 18, 13, 11);
                    }
                }
            };

            // Dùng cột nhãn đủ rộng để dấu ":" không bị cắt ở DPI/scale khác nhau.
            Label label = MakeLabel(labelText.TrimEnd(':', ' ') + ":", 16F, FontStyle.Regular, navy);
            label.SetBounds(58, top, 205, 38);
            label.AutoSize = false;
            label.AutoEllipsis = false;
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.UseMnemonic = false;
            label.Padding = new Padding(0);

            Label value = MakeLabel(valueText, 16F, FontStyle.Regular, navy);
            value.SetBounds(276, top, Math.Max(150, parent.ClientSize.Width - 292), 38);
            value.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            value.AutoSize = false;
            value.AutoEllipsis = false;
            value.TextAlign = ContentAlignment.MiddleLeft;
            value.UseMnemonic = false;
            value.Padding = new Padding(0);

            parent.Controls.Add(icon);
            parent.Controls.Add(label);
            parent.Controls.Add(value);
        }

        private void AddStatistics(string total, string present, string absent)
        {
            Panel bar = CreateBlock(70);
            bar.Name = "pnlStatistics";
            bar.BackColor = Color.FromArgb(226, 239, 255);
            bar.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (SolidBrush firstCell = new SolidBrush(Color.FromArgb(207, 229, 255)))
                    e.Graphics.FillRectangle(firstCell, 1, 1, (int)((bar.Width - 2) * 0.27), bar.Height - 2);
                using (GraphicsPath path = RoundedRect(new Rectangle(0, 0, bar.Width - 1, bar.Height - 1), 8))
                using (Pen pen = new Pen(borderBlue, 1)) e.Graphics.DrawPath(pen, path);
            };
            TableLayoutPanel table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = Padding.Empty,
                Padding = new Padding(8, 0, 8, 0)
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 27F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 24.33F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 24.33F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 24.34F));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            table.Controls.Add(StatHeaderCell(), 0, 0);
            table.Controls.Add(StatCell("♟", "Tổng quân số", ExtractValue(total), blue), 1, 0);
            table.Controls.Add(StatCell("✓", "Có mặt", ExtractValue(present), Color.FromArgb(0, 177, 92)), 2, 0);
            table.Controls.Add(StatCell("✖", "Vắng mặt", ExtractValue(absent), Color.FromArgb(235, 55, 62)), 3, 0);
            bar.Controls.Add(table);
            _contentFlow.Controls.Add(bar);
        }

        private Control StatHeaderCell()
        {
            Panel cell = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent, Margin = Padding.Empty };
            cell.Paint += (s, e) =>
            {
                using (SolidBrush brush = new SolidBrush(blue))
                {
                    int x = 22, baseline = cell.Height / 2 + 13;
                    e.Graphics.FillRectangle(brush, x, baseline - 13, 7, 13);
                    e.Graphics.FillRectangle(brush, x + 12, baseline - 23, 7, 23);
                    e.Graphics.FillRectangle(brush, x + 24, baseline - 33, 7, 33);
                    e.Graphics.FillRectangle(brush, x - 3, baseline + 2, 38, 3);
                }
            };
            Label text = MakeLabel("Tổng quân số", 18F, FontStyle.Bold, navy);
            text.SetBounds(68, 10, 210, 48);
            cell.Controls.Add(text);
            return cell;
        }


        private Control StatCell(string glyph, string caption, string value, Color color)
        {
            Panel cell = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent, Margin = Padding.Empty };
            cell.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(145, 174, 215), 1))
                    e.Graphics.DrawLine(pen, 0, 12, 0, cell.Height - 12);
            };
            Control icon = CreateStatIcon(caption, color);
            icon.SetBounds(8, 13, 42, 42);
            Label text = MakeLabel(caption + ": ", 16F, FontStyle.Regular, navy);
            text.AutoSize = true; text.Location = new Point(54, 22);
            Label number = MakeLabel(value, 21F, FontStyle.Bold, navy);
            number.AutoSize = true;
            number.Location = new Point(54 + TextRenderer.MeasureText(caption + ": ", text.Font).Width, 17);
            cell.Controls.Add(icon); cell.Controls.Add(text); cell.Controls.Add(number);
            return cell;
        }

        private Control CreateStatIcon(string caption, Color color)
        {
            Panel icon = new Panel { BackColor = Color.Transparent };
            icon.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                if (caption == "Có mặt" || caption == "Vắng mặt")
                {
                    using (SolidBrush fill = new SolidBrush(color))
                        g.FillEllipse(fill, 5, 5, 32, 32);
                    using (Pen mark = new Pen(Color.White, 3.2F))
                    {
                        mark.StartCap = LineCap.Round; mark.EndCap = LineCap.Round;
                        if (caption == "Có mặt")
                        {
                            g.DrawLine(mark, 13, 22, 19, 28);
                            g.DrawLine(mark, 19, 28, 30, 15);
                        }
                        else
                        {
                            g.DrawLine(mark, 15, 15, 27, 27);
                            g.DrawLine(mark, 27, 15, 15, 27);
                        }
                    }
                }
                else
                {
                    using (SolidBrush fill = new SolidBrush(color))
                    {
                        g.FillEllipse(fill, 15, 3, 12, 12);
                        g.FillEllipse(fill, 2, 8, 10, 10);
                        g.FillEllipse(fill, 30, 8, 10, 10);
                        g.FillEllipse(fill, 11, 20, 20, 16);
                        g.FillEllipse(fill, 0, 22, 14, 12);
                        g.FillEllipse(fill, 28, 22, 14, 12);
                    }
                }
            };
            return icon;
        }

        private void AddDivider()
        {
            Panel line = CreateBlock(24);
            line.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(65, 94, 143), 1.4F))
                {
                    pen.DashPattern = new float[] { 4, 4 };
                    e.Graphics.DrawLine(pen, 0, line.Height / 2, line.Width, line.Height / 2);
                }
            };
            _contentFlow.Controls.Add(line);
        }

        private void AddSectionBar(string text, string glyph)
        {
            Panel bar = CreateBlock(48);
            bar.Name = "pnlSectionBar";
            bar.BackColor = Color.FromArgb(216, 234, 255);
            bar.Paint += (s, e) =>
            {
                using (GraphicsPath path = RoundedRect(new Rectangle(0, 0, bar.Width - 1, bar.Height - 1), 8))
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(216, 234, 255)))
                    e.Graphics.FillPath(brush, path);
            };
            Label icon = MakeLabel(glyph, 25F, FontStyle.Bold, blue);
            icon.SetBounds(20, 3, 42, 42);
            Label title = MakeLabel(text, 19F, FontStyle.Bold, navy);
            title.SetBounds(66, 3, Math.Max(200, bar.Width - 80), 42);
            bar.Controls.Add(icon);
            bar.Controls.Add(title);
            _contentFlow.Controls.Add(bar);
        }

        private void AddBodyText(string text)
        {
            Panel body = CreateBlock(58);
            Label label = MakeLabel(text, 17F, FontStyle.Regular, navy);
            label.SetBounds(48, 5, Math.Max(300, body.Width - 60), 46);
            body.Controls.Add(label);
            _contentFlow.Controls.Add(body);
        }



        private void AddConclusionCard(string present, string absent)
        {
            // Kết luận dùng cùng số liệu với thanh Tổng quân số, nhưng trình bày
            // trên toàn chiều rộng để không bị cắt nhãn hoặc chữ số ở DPI khác nhau.
            Panel card = CreateBlock(72);
            card.Name = "pnlConclusionCard";
            card.Margin = Padding.Empty;
            card.BackColor = Color.FromArgb(244, 248, 255);
            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath path = RoundedRect(new Rectangle(0, 0, card.Width - 1, card.Height - 1), 8))
                using (Pen pen = new Pen(borderBlue, 1.2F))
                    e.Graphics.DrawPath(pen, path);
                using (Pen divider = new Pen(Color.FromArgb(175, 197, 230), 1))
                    e.Graphics.DrawLine(divider, card.Width / 2, 12, card.Width / 2, card.Height - 12);
            };

            TableLayoutPanel table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = Padding.Empty,
                Padding = new Padding(8, 0, 8, 0)
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            table.Controls.Add(CreateConclusionCell(
                "Có mặt:", ExtractValue(present).PadLeft(2, '0'),
                Color.FromArgb(0, 177, 92)), 0, 0);
            table.Controls.Add(CreateConclusionCell(
                "Vắng mặt:", ExtractValue(absent).PadLeft(2, '0'),
                Color.FromArgb(235, 55, 62)), 1, 0);

            card.Controls.Add(table);
            _contentFlow.Controls.Add(card);
        }

        private Control CreateConclusionCell(string caption, string value, Color color)
        {
            Panel cell = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Margin = Padding.Empty
            };

            Control icon = CreateStatIcon(
                caption.StartsWith("Có", StringComparison.Ordinal) ? "Có mặt" : "Vắng mặt",
                color);
            icon.SetBounds(18, 15, 38, 40);

            Label label = MakeLabel(caption, 18F, FontStyle.Regular, navy);
            label.SetBounds(66, 13, 150, 44);
            label.AutoSize = false;
            label.AutoEllipsis = false;

            Label number = MakeLabel(value, 19F, FontStyle.Bold, navy);
            number.SetBounds(222, 13, 70, 44);
            number.AutoSize = false;
            number.AutoEllipsis = false;

            cell.Controls.Add(icon);
            cell.Controls.Add(label);
            cell.Controls.Add(number);
            return cell;
        }

        private void AddAbsentSectionContent(string[] lines, string heading)
        {
            int start = Array.FindIndex(lines, x =>
                !string.IsNullOrWhiteSpace(heading) &&
                string.Equals(x, heading, StringComparison.OrdinalIgnoreCase));

            if (start < 0)
            {
                AddBodyText("Không có CBCS vắng mặt.");
                return;
            }

            List<string> details = new List<string>();
            for (int i = start + 1; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (IsReportSectionBoundary(line)) break;
                if (string.IsNullOrWhiteSpace(line) || line.Contains("====") ||
                    line.Contains("----") || line.StartsWith("HẾT", StringComparison.OrdinalIgnoreCase))
                    continue;
                details.Add(line);
            }

            // Nếu có dữ liệu chi tiết thì không được để câu thông báo rỗng che khuất danh sách.
            bool hasActualDetails = details.Any(x =>
                !x.StartsWith("Không có CBCS vắng mặt", StringComparison.OrdinalIgnoreCase));

            if (!hasActualDetails)
            {
                AddBodyText("Không có CBCS vắng mặt.");
                return;
            }

            foreach (string line in details)
            {
                if (line.StartsWith("Không có CBCS vắng mặt", StringComparison.OrdinalIgnoreCase))
                    continue;
                AddBodyText(line);
            }
        }

        private void AddRemainingSections(string[] lines, string absentHeading)
        {
            int start = Array.FindIndex(lines, x =>
                !string.IsNullOrWhiteSpace(absentHeading) &&
                string.Equals(x, absentHeading, StringComparison.OrdinalIgnoreCase));
            if (start < 0) return;

            // Bỏ qua toàn bộ mục I; bắt đầu từ mục tiếp theo để tránh lặp danh sách vắng.
            int nextSection = -1;
            for (int i = start + 1; i < lines.Length; i++)
            {
                if (IsReportSectionBoundary(lines[i]) &&
                    !lines[i].StartsWith("KẾT LUẬN", StringComparison.OrdinalIgnoreCase))
                {
                    nextSection = i;
                    break;
                }
                if (lines[i].StartsWith("KẾT LUẬN", StringComparison.OrdinalIgnoreCase))
                    return;
            }
            if (nextSection < 0) return;

            for (int i = nextSection; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (line.StartsWith("KẾT LUẬN", StringComparison.OrdinalIgnoreCase)) break;
                if (IsReportSectionBoundary(line))
                {
                    AddDivider();
                    AddSectionBar(line, "▤");
                }
                else if (!string.IsNullOrWhiteSpace(line) && !line.Contains("====") &&
                         !line.Contains("----") && !line.StartsWith("HẾT", StringComparison.OrdinalIgnoreCase))
                {
                    AddBodyText(line);
                }
            }
        }

        private bool IsReportSectionBoundary(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return false;
            string value = line.Trim();
            return value.StartsWith("II.", StringComparison.OrdinalIgnoreCase) ||
                   value.StartsWith("III.", StringComparison.OrdinalIgnoreCase) ||
                   value.StartsWith("IV.", StringComparison.OrdinalIgnoreCase) ||
                   value.StartsWith("V.", StringComparison.OrdinalIgnoreCase) ||
                   value.StartsWith("2.", StringComparison.OrdinalIgnoreCase) ||
                   value.StartsWith("3.", StringComparison.OrdinalIgnoreCase) ||
                   value.StartsWith("KẾT LUẬN", StringComparison.OrdinalIgnoreCase);
        }

        private Panel CreateBlock(int height)
        {
            return new Panel
            {
                Width = Math.Max(500, _contentFlow.ClientSize.Width - 12),
                Height = height,
                Margin = new Padding(0, 0, 0, 0),
                BackColor = Color.White
            };
        }

        private void LayoutReport()
        {
            if (_contentFlow == null || pnlReportContainer == null) return;
            int width = Math.Max(500, _contentFlow.ClientSize.Width - 12);
            foreach (Control control in _contentFlow.Controls)
            {
                // Tất cả khối nội dung, bao gồm Kết luận, dùng cùng chiều rộng thực tế.
                // Không thu hẹp riêng card Kết luận vì sẽ làm nhãn/số liệu dễ bị chồng lấn.
                control.Width = width;
                if (control is Panel roundedPanel)
                {
                    int radius = roundedPanel.Name == "pnlDutyCard" ? 8 :
                                 roundedPanel.Name == "pnlStatistics" ? 8 :
                                 roundedPanel.Name == "pnlSectionBar" ? 8 :
                                 roundedPanel.Name == "pnlConclusionCard" ? 8 : 0;
                    if (radius > 0) ApplyRoundedRegion(roundedPanel, radius);
                }
                foreach (Control child in control.Controls)
                {
                    if (child is Label label && label.Dock == DockStyle.None &&
                        (label.Name == "stretch"))
                        label.Width = Math.Max(100, width - label.Left - 20);
                }
            }
            _contentFlow.PerformLayout();
            // Giữ thanh cuộn dọc mảnh như bản thiết kế ngay cả khi nội dung ngắn.
            _contentFlow.AutoScrollMinSize = new Size(0, _contentFlow.ClientSize.Height + 2);
        }

        private Label MakeLabel(string text, float size, FontStyle style, Color color)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", size, style, GraphicsUnit.Point),
                ForeColor = color,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = false
            };
        }

        private string FindLine(string[] lines, Func<string, bool> predicate)
        {
            return lines.FirstOrDefault(predicate) ?? string.Empty;
        }

        private string FindLastLine(string[] lines, Func<string, bool> predicate)
        {
            return lines.LastOrDefault(predicate) ?? string.Empty;
        }

        private bool StartsWithAny(string value, params string[] starts)
        {
            return starts.Any(x => value.StartsWith(x, StringComparison.OrdinalIgnoreCase));
        }

        private string ExtractValue(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return "0";
            Match m = Regex.Match(line, @":\s*(\d+)");
            if (m.Success) return m.Groups[1].Value;
            m = Regex.Match(line, @"\b(\d+)\b");
            return m.Success ? m.Groups[1].Value : "0";
        }

        private GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void pnlHeader_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = pnlHeader.ClientRectangle;
            using (LinearGradientBrush brush = new LinearGradientBrush(rect,
                Color.FromArgb(0, 111, 205), Color.FromArgb(0, 67, 148), LinearGradientMode.Horizontal))
                e.Graphics.FillRectangle(brush, rect);

            using (GraphicsPath wave = new GraphicsPath())
            {
                wave.AddBezier(0, pnlHeader.Height - 17, pnlHeader.Width * .28f, pnlHeader.Height - 2,
                    pnlHeader.Width * .58f, pnlHeader.Height - 34, pnlHeader.Width, pnlHeader.Height - 18);
                wave.AddLine(pnlHeader.Width, pnlHeader.Height, 0, pnlHeader.Height);
                wave.CloseFigure();
                using (SolidBrush b = new SolidBrush(Color.FromArgb(25, 255, 255, 255)))
                    e.Graphics.FillPath(b, wave);
            }

            using (Pen eye = new Pen(Color.White, 3.2F))
            using (SolidBrush pupil = new SolidBrush(Color.White))
            {
                e.Graphics.DrawEllipse(eye, 29, 25, 39, 25);
                e.Graphics.FillEllipse(pupil, 43, 31, 11, 11);
            }
            PointF emblemCenter = new PointF(pnlHeader.Width - 112, 39);
            using (SolidBrush watermark = new SolidBrush(Color.FromArgb(42, 255, 255, 255)))
                e.Graphics.FillPolygon(watermark, CreateStar(emblemCenter, 24, 11, 5));
            DrawHeaderLaurel(e.Graphics, emblemCenter);
            using (Pen bottom = new Pen(Color.FromArgb(90, 255, 255, 255), 1))
                e.Graphics.DrawLine(bottom, 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height);
        }

        private void DrawHeaderLaurel(Graphics g, PointF center)
        {
            GraphicsState state = g.Save();
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (Pen branch = new Pen(Color.FromArgb(34, 255, 255, 255), 2.2F))
            using (SolidBrush leaf = new SolidBrush(Color.FromArgb(30, 255, 255, 255)))
            {
                // Hai nhánh nguyệt quế ôm lấy ngôi sao, cùng tông watermark trên header.
                g.DrawArc(branch, center.X - 47, center.Y - 30, 94, 72, 205, 125);
                g.DrawArc(branch, center.X - 47, center.Y - 30, 94, 72, 30, 125);
                for (int i = 0; i < 7; i++)
                {
                    float y = center.Y + 25 - i * 8;
                    float x = center.X - 34 + i * 2.8f;
                    g.FillEllipse(leaf, x - 8, y - 5, 14, 7);
                    g.FillEllipse(leaf, center.X + 20 - i * 2.8f, y - 5, 14, 7);
                }
            }
            g.Restore(state);
        }

        private PointF[] CreateStar(PointF center, float outerRadius, float innerRadius, int points)
        {
            PointF[] result = new PointF[points * 2];
            for (int i = 0; i < result.Length; i++)
            {
                double angle = -Math.PI / 2 + i * Math.PI / points;
                float radius = i % 2 == 0 ? outerRadius : innerRadius;
                result[i] = new PointF(center.X + (float)Math.Cos(angle) * radius,
                    center.Y + (float)Math.Sin(angle) * radius);
            }
            return result;
        }

        private void BtnCopy_Paint(object? sender, PaintEventArgs e)
        {
            Button button = (Button)sender!;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Rectangle bounds = new Rectangle(1, 1, button.Width - 3, button.Height - 3);
            using (GraphicsPath path = RoundedRect(bounds, 9))
            using (SolidBrush fill = new SolidBrush(Color.FromArgb(0, 105, 220)))
                g.FillPath(fill, path);

            DrawCenteredButtonContent(g, button, "Sao chép", Color.White, true);
        }

        private void BtnClose_Paint(object? sender, PaintEventArgs e)
        {
            Button button = (Button)sender!;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Rectangle bounds = new Rectangle(1, 1, button.Width - 3, button.Height - 3);
            using (GraphicsPath path = RoundedRect(bounds, 9))
            using (SolidBrush fill = new SolidBrush(Color.White))
            using (Pen border = new Pen(Color.FromArgb(220, 35, 35), 1.5F))
            {
                g.FillPath(fill, path);
                g.DrawPath(border, path);
            }

            DrawCenteredButtonContent(g, button, "Đóng", Color.FromArgb(205, 38, 38), false);
        }

        private void DrawCenteredButtonContent(Graphics g, Button button, string text, Color textColor, bool copyIcon)
        {
            // Căn cả cụm biểu tượng + chữ vào chính giữa nút, không căn từng phần độc lập.
            Size textSize = TextRenderer.MeasureText(
                text, button.Font, new Size(500, button.Height),
                TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);

            int iconSize = 20;
            int gap = 9;
            int groupWidth = iconSize + gap + textSize.Width;
            int left = (button.ClientSize.Width - groupWidth) / 2;
            int iconTop = (button.ClientSize.Height - iconSize) / 2;
            int textTop = (button.ClientSize.Height - textSize.Height) / 2;

            using (Pen iconPen = new Pen(textColor, 1.8F))
            {
                iconPen.LineJoin = LineJoin.Round;
                iconPen.StartCap = LineCap.Round;
                iconPen.EndCap = LineCap.Round;

                if (copyIcon)
                {
                    using (Pen copyPen = new Pen(textColor, 2.2F))
                    {
                        copyPen.LineJoin = LineJoin.Round;
                        copyPen.StartCap = LineCap.Round;
                        copyPen.EndCap = LineCap.Round;

                        // Khung phía sau: nằm phía trên bên phải
                        Rectangle back = new Rectangle(
                            left + 8, iconTop, 12, 16);

                        // Khung phía trước: nằm phía dưới bên trái
                        Rectangle front = new Rectangle(
                            left, iconTop + 7, 12, 16);

                        using (GraphicsPath backPath = RoundedRect(back, 5))
                        using (GraphicsPath frontPath = RoundedRect(front, 5))
                        {
                            g.DrawPath(copyPen, backPath);
                            g.DrawPath(copyPen, frontPath);
                        }
                    }
                }
                else
                {
                    float cx = left + iconSize / 2f;
                    float cy = button.ClientSize.Height / 2f;
                    g.DrawLine(iconPen, cx - 5.5F, cy - 5.5F, cx + 5.5F, cy + 5.5F);
                    g.DrawLine(iconPen, cx + 5.5F, cy - 5.5F, cx - 5.5F, cy + 5.5F);
                }
            }

            TextRenderer.DrawText(
                g, text, button.Font,
                new Point(left + iconSize + gap, textTop),
                textColor,
                TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);
        }

        private void panel1_Paint(object? sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(Color.FromArgb(222, 233, 249), 1))
                e.Graphics.DrawLine(pen, 0, 0, panel1.Width, 0);
        }

        private void panel1_Resize(object? sender, EventArgs e)
        {
            if (btnCopy == null || btnClose == null || panel1 == null) return;
            int gap = 24;
            int totalWidth = btnCopy.Width + gap + btnClose.Width;
            int left = Math.Max(12, (panel1.ClientSize.Width - totalWidth) / 2);
            int top = (panel1.ClientSize.Height - btnCopy.Height) / 2;
            btnCopy.Location = new Point(left, top);
            btnClose.Location = new Point(left + btnCopy.Width + gap, top);
        }

        private void ApplyRoundedCorners()
        {
            if (Width <= 0 || Height <= 0) return;
            ApplyRoundedRegion(btnCopy, 9);
            ApplyRoundedRegion(btnClose, 9);
            using (GraphicsPath path = new GraphicsPath())
            {
                int radius = 18, d = radius * 2;
                path.AddArc(0, 0, d, d, 180, 90);
                path.AddArc(Width - d - 1, 0, d, d, 270, 90);
                path.AddArc(Width - d - 1, Height - d - 1, d, d, 0, 90);
                path.AddArc(0, Height - d - 1, d, d, 90, 90);
                path.CloseFigure();
                Region = new Region(path);
            }
        }


        private void ApplyRoundedRegion(Control control, int radius)
        {
            if (control == null || control.Width < radius * 2 || control.Height < radius * 2) return;
            using (GraphicsPath path = RoundedRect(new Rectangle(0, 0, control.Width, control.Height), radius))
            {
                Region? old = control.Region;
                control.Region = new Region(path);
                old?.Dispose();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ApplyRoundedCorners();
            LayoutReport();
        }

        private void btnClose_Click(object sender, EventArgs e) { Close(); }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_originalReportText)) return;
            Clipboard.SetText(_originalReportText);

            // Hiển thị hộp thoại thông báo thiết kế riêng, căn giữa FrmReportPreview.
            FrmLoginMessage.ShowMessage(
                this,
                LoginMessageType.Information,
                "Thông báo",
                "Đã sao chép nội dung báo cáo.");
        }
    }
}