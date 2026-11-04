using QuanLyCBCS.DTOs;
using QuanLyCBCS.Forms.CommonForms;
using QuanLyCBCS.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCBCS.Forms.PersonnelForms
{
    // DateTimePicker native without WS_EX_CLIENTEDGE; the surrounding panel
    // supplies the single rounded border used by the report filter design.
    internal class BorderlessDateTimePicker : DateTimePicker
    {
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle &= ~0x00000200; // WS_EX_CLIENTEDGE
                return cp;
            }
        }
    }

    // Fully owner-drawn outline button. Avoids the native WinForms Button's
    // rectangular hover/focus rendering that was breaking the rounded border.
    internal sealed class RoundedOutlineButton : Button
    {
        private bool _hovered;

        public RoundedOutlineButton()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            TabStop = false;
            BackColor = Color.White;
            Cursor = Cursors.Hand;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _hovered = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _hovered = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.Clear(Parent?.BackColor ?? SystemColors.Control);

            RectangleF rect = new RectangleF(1.25F, 1.25F,
                Math.Max(1, Width - 2.5F), Math.Max(1, Height - 2.5F));
            float radius = Math.Min(15F, rect.Height / 2F);
            using (GraphicsPath path = CreateButtonPath(rect, radius))
            using (SolidBrush fill = new SolidBrush(!Enabled ? Color.FromArgb(248, 250, 253) :
                (_hovered ? Color.FromArgb(246, 250, 255) : Color.White)))
            using (Pen border = new Pen(Enabled ? Color.FromArgb(0, 102, 224) :
                Color.FromArgb(190, 205, 225), 1.35F))
            {
                g.FillPath(fill, path);
                g.DrawPath(border, path);
            }

            Color textColor = Enabled ? ForeColor : Color.Gray;
            TextRenderer.DrawText(g, Text, Font, ClientRectangle, textColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                TextFormatFlags.SingleLine | TextFormatFlags.NoPadding);
        }

        internal static GraphicsPath CreateButtonPath(RectangleF r, float radius)
        {
            float d = radius * 2F;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    // Owner-drawn primary action button with a clear eye glyph. This avoids
    // relying on a font-specific Unicode eye character that may disappear
    // or render differently across Windows installations.
    internal sealed class EyePreviewButton : Button
    {
        private bool _hovered;
        private bool _pressed;

        public EyePreviewButton()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            TabStop = false;
            Cursor = Cursors.Hand;
            BackColor = Color.FromArgb(0, 102, 224);
            ForeColor = Color.White;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _hovered = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _hovered = false;
            _pressed = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) _pressed = true;
            Invalidate();
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _pressed = false;
            Invalidate();
            base.OnMouseUp(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.Clear(Parent?.BackColor ?? SystemColors.Control);

            RectangleF bounds = new RectangleF(1F, 1F,
                Math.Max(1F, Width - 2F), Math.Max(1F, Height - 2F));
            float radius = Math.Min(16F, bounds.Height / 2F);
            using (GraphicsPath buttonPath = RoundedOutlineButton.CreateButtonPath(bounds, radius))
            using (SolidBrush fill = new SolidBrush(!Enabled ? Color.FromArgb(145, 181, 224) :
                (_pressed ? Color.FromArgb(0, 75, 180) :
                (_hovered ? Color.FromArgb(0, 87, 200) : Color.FromArgb(0, 102, 224)))))
            {
                g.FillPath(fill, buttonPath);
            }

            Size textSize = TextRenderer.MeasureText(Text, Font,
                new Size(Math.Max(20, Width - 50), Height),
                TextFormatFlags.SingleLine | TextFormatFlags.NoPadding);
            float iconWidth = 24F;
            float gap = 9F;
            float totalWidth = iconWidth + gap + textSize.Width;
            float startX = Math.Max(8F, (Width - totalWidth) / 2F);
            float centerY = Height / 2F;

            // Draw an almond-shaped eye and a filled pupil in white.
            using (GraphicsPath eye = new GraphicsPath())
            using (Pen eyePen = new Pen(Color.White, 2F))
            using (SolidBrush pupil = new SolidBrush(Color.White))
            {
                eye.AddBezier(startX, centerY,
                    startX + 6F, centerY - 8F,
                    startX + 17F, centerY - 8F,
                    startX + iconWidth, centerY);
                eye.AddBezier(startX + iconWidth, centerY,
                    startX + 17F, centerY + 8F,
                    startX + 6F, centerY + 8F,
                    startX, centerY);
                g.DrawPath(eyePen, eye);
                g.FillEllipse(pupil, startX + 9F, centerY - 3F, 6F, 6F);
            }

            Rectangle textBounds = new Rectangle(
                (int)(startX + iconWidth + gap), 0,
                Math.Max(1, Width - (int)(startX + iconWidth + gap) - 5), Height);
            TextRenderer.DrawText(g, Text, Font, textBounds, Enabled ? ForeColor : Color.WhiteSmoke,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
                TextFormatFlags.SingleLine | TextFormatFlags.NoPadding);
        }
    }

    public partial class FrmPersonnelReport : Form
    {
        private readonly PersonnelReportService _reportService;
        private bool _isLoading;

        public FrmPersonnelReport()
        {
            InitializeComponent();
            _reportService = new PersonnelReportService();
            FormBorderStyle = FormBorderStyle.None;
            DoubleBuffered = true;
            dgvPersonnelReport.CellPainting += dgvPersonnelReport_CellPainting;
        }

        private async Task LoadPersonnelReportAsync()
        {
            if (_isLoading) return;
            try
            {
                _isLoading = true;
                btnRefresh.Enabled = false;
                btnPreview.Enabled = false;
                lblStatus.Text = "Đang tải dữ liệu báo cáo...";

                List<PersonnelReportDto> data =
                    await _reportService.GetReportAsync(dtpReportDate.Value);

                dgvPersonnelReport.DataSource = null;
                dgvPersonnelReport.AutoGenerateColumns = true;
                dgvPersonnelReport.DataSource = data;
                AddSttColumn();
                CustomizeGrid();

                lblTotalCount.Text = data.Count.ToString("N0");
                lblPresentCount.Text = data.Count(x => x.IsPresent).ToString("N0");
                lblAbsentCount.Text = data.Count(x => !x.IsPresent).ToString("N0");
                lblStatus.Text = $"Đang hiển thị: {data.Count:N0} CBCS";
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Không thể tải dữ liệu báo cáo.";
                MessageBox.Show(ex.Message, "Lỗi tải báo cáo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isLoading = false;
                btnRefresh.Enabled = true;
                btnPreview.Enabled = true;
            }
        }

        private void CustomizeGrid()
        {
            if (dgvPersonnelReport.Columns.Count == 0) return;
            dgvPersonnelReport.SuspendLayout();

            HideColumn("PersonnelId");
            HideColumn("MissionStart");
            HideColumn("MissionEnd");
            SetHeader("SoHieu", "Số hiệu");
            SetHeader("HoTen", "Họ tên");
            SetHeader("DonVi", "Đơn vị");
            SetHeader("CapBacHam", "Cấp bậc");
            SetHeader("Status", "Trạng thái");
            SetHeader("IsPresent", "Có mặt");
            SetHeader("Reason", "Lý do");
            SetHeader("MissionName", "Nhiệm vụ");
            SetHeader("Location", "Địa điểm");

            dgvPersonnelReport.EnableHeadersVisualStyles = false;
            dgvPersonnelReport.BackgroundColor = Color.White;
            dgvPersonnelReport.BorderStyle = BorderStyle.None;
            dgvPersonnelReport.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvPersonnelReport.GridColor = Color.FromArgb(211, 224, 239);
            dgvPersonnelReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPersonnelReport.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvPersonnelReport.ColumnHeadersHeight = 41;
            dgvPersonnelReport.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvPersonnelReport.RowTemplate.Height = 40;
            dgvPersonnelReport.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvPersonnelReport.AllowUserToResizeRows = false;
            dgvPersonnelReport.ReadOnly = true;
            dgvPersonnelReport.RowHeadersVisible = false;
            dgvPersonnelReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPersonnelReport.MultiSelect = false;

            dgvPersonnelReport.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(9, 78, 145);
            dgvPersonnelReport.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvPersonnelReport.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 10.5F, FontStyle.Bold);
            dgvPersonnelReport.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
            dgvPersonnelReport.ColumnHeadersDefaultCellStyle.Padding = new Padding(4, 0, 4, 0);

            dgvPersonnelReport.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvPersonnelReport.DefaultCellStyle.ForeColor = Color.FromArgb(20, 38, 60);
            dgvPersonnelReport.DefaultCellStyle.BackColor = Color.FromArgb(226, 241, 255);
            dgvPersonnelReport.DefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 222, 250);
            dgvPersonnelReport.DefaultCellStyle.SelectionForeColor = Color.FromArgb(12, 39, 69);
            dgvPersonnelReport.DefaultCellStyle.Padding = new Padding(8, 2, 8, 2);
            dgvPersonnelReport.RowsDefaultCellStyle.BackColor = Color.FromArgb(226, 241, 255);
            dgvPersonnelReport.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 222, 250);
            dgvPersonnelReport.RowsDefaultCellStyle.SelectionForeColor = Color.FromArgb(12, 39, 69);
            dgvPersonnelReport.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            dgvPersonnelReport.AlternatingRowsDefaultCellStyle.ForeColor = Color.FromArgb(20, 38, 60);
            dgvPersonnelReport.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 222, 250);
            dgvPersonnelReport.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.FromArgb(12, 39, 69);

            if (dgvPersonnelReport.Columns.Contains("IsPresent"))
            {
                dgvPersonnelReport.Columns["IsPresent"].DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;
                dgvPersonnelReport.Columns["IsPresent"].FillWeight = 75;
            }

            // Tỷ lệ cột bám sát bản thiết kế ở độ phân giải 1648 x 928.
            SetFillWeight("STT", 50);
            SetFillWeight("SoHieu", 92);
            SetFillWeight("HoTen", 110);
            SetFillWeight("DonVi", 76);
            SetFillWeight("CapBacHam", 76);
            SetFillWeight("Status", 82);
            SetFillWeight("IsPresent", 98);
            SetFillWeight("Reason", 99);
            SetFillWeight("MissionName", 117);
            SetFillWeight("Location", 112);

            // Ép chiều cao từng dòng sau khi bind dữ liệu để không phụ thuộc RowTemplate.
            foreach (DataGridViewRow row in dgvPersonnelReport.Rows)
            {
                if (!row.IsNewRow) row.Height = 40;
            }
            dgvPersonnelReport.ResumeLayout();
        }


        private void dgvPersonnelReport_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dgvPersonnelReport.Columns[e.ColumnIndex].Name != "Status") return;

            e.Handled = true;
            e.PaintBackground(e.CellBounds, true);

            string status = Convert.ToString(e.FormattedValue) ?? string.Empty;
            bool present = status.IndexOf("Có mặt", StringComparison.OrdinalIgnoreCase) >= 0;
            Color fill = present ? Color.FromArgb(222, 250, 234) : Color.FromArgb(255, 230, 233);
            Color accent = present ? Color.FromArgb(0, 166, 78) : Color.FromArgb(220, 48, 65);

            int width = Math.Min(e.CellBounds.Width - 14, present ? 96 : 104);
            int height = 28;
            Rectangle pill = new Rectangle(
                e.CellBounds.Left + (e.CellBounds.Width - width) / 2,
                e.CellBounds.Top + (e.CellBounds.Height - height) / 2,
                Math.Max(40, width), height);

            using (GraphicsPath path = CreateRoundedPath(pill, 14))
            using (SolidBrush background = new SolidBrush(fill))
                e.Graphics?.FillPath(background, path);

            if (e.Graphics != null)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            }
            int cx = pill.Left + 16;
            int cy = pill.Top + pill.Height / 2;
            using (SolidBrush circle = new SolidBrush(accent))
                e.Graphics?.FillEllipse(circle, cx - 7, cy - 7, 14, 14);
            using (Pen mark = new Pen(Color.White, 1.8F))
            {
                mark.StartCap = LineCap.Round;
                mark.EndCap = LineCap.Round;
                if (present)
                    e.Graphics?.DrawLines(mark, new[] { new Point(cx - 4, cy), new Point(cx - 1, cy + 3), new Point(cx + 4, cy - 4) });
                else
                {
                    e.Graphics?.DrawLine(mark, cx - 3, cy - 3, cx + 3, cy + 3);
                    e.Graphics?.DrawLine(mark, cx + 3, cy - 3, cx - 3, cy + 3);
                }
            }

            using (SolidBrush textBrush = new SolidBrush(accent))
            using (Font font = new Font("Segoe UI", 9.5F, FontStyle.Bold))
            {
                Rectangle textRect = new Rectangle(cx + 10, pill.Top, pill.Right - cx - 12, pill.Height);
                if (e.Graphics != null)
                {
                    TextRenderer.DrawText(
                        e.Graphics,
                        status,
                        font,
                        textRect,
                        accent,
                        TextFormatFlags.Left |
                        TextFormatFlags.VerticalCenter |
                        TextFormatFlags.EndEllipsis
                    );
                }
            }
        }

        // Vẽ thanh trạng thái dạng thẻ bo góc, có lề hai bên như bản thiết kế.
        private void pnlStatus_Paint(object? sender, PaintEventArgs e)
        {
            if (pnlStatus == null || pnlStatus.Width <= 0 || pnlStatus.Height <= 0)
                return;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Rectangle bounds = new Rectangle(12, 4,
                Math.Max(1, pnlStatus.Width - 24), Math.Max(1, pnlStatus.Height - 8));
            using (GraphicsPath path = CreateRoundedPath(bounds, 12))
            using (SolidBrush fill = new SolidBrush(Color.FromArgb(222, 238, 255)))
            using (Pen border = new Pen(Color.FromArgb(213, 230, 249), 1F))
            {
                g.FillPath(fill, path);
                g.DrawPath(border, path);
            }
        }

        private void HideColumn(string name)
        {
            if (dgvPersonnelReport.Columns.Contains(name))
                dgvPersonnelReport.Columns[name].Visible = false;
        }

        private void SetHeader(string name, string text)
        {
            if (dgvPersonnelReport.Columns.Contains(name))
                dgvPersonnelReport.Columns[name].HeaderText = text;
        }

        private void SetFillWeight(string name, float weight)
        {
            if (dgvPersonnelReport.Columns.Contains(name))
                dgvPersonnelReport.Columns[name].FillWeight = weight;
        }

        private void AddSttColumn()
        {
            if (dgvPersonnelReport.Columns.Contains("STT"))
                dgvPersonnelReport.Columns.Remove("STT");

            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn
            {
                Name = "STT",
                HeaderText = "STT",
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                FillWeight = 45
            };
            dgvPersonnelReport.Columns.Insert(0, col);

            for (int i = 0; i < dgvPersonnelReport.Rows.Count; i++)
                if (!dgvPersonnelReport.Rows[i].IsNewRow)
                    dgvPersonnelReport.Rows[i].Cells["STT"].Value = i + 1;

            dgvPersonnelReport.Columns["STT"].DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
        }

        private async void FrmPersonnelReport_Load(object sender, EventArgs e)
        {
            ApplyRoundedCorners();
            await LoadPersonnelReportAsync();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadPersonnelReportAsync();
        }

        private async void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                btnPreview.Enabled = false;
                string report = await _reportService.BuildReportTextAsync(dtpReportDate.Value);
                using (FrmReportPreview frm = new FrmReportPreview(report))
                    frm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi xem báo cáo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { btnPreview.Enabled = true; }
        }

        private async void btnCopyReport_Click(object sender, EventArgs e)
        {
            try
            {
                string report = await _reportService.BuildReportTextAsync(dtpReportDate.Value);
                Clipboard.SetText(report);
                MessageBox.Show("Đã sao chép báo cáo vào Clipboard.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyRoundedCorners()
        {
            // Clip the white button surface to rounded corners; the outline is
            // painted slightly inset so the Region does not cut off its strokes.
            SetRoundedRegion(btnPreview, 18);
            SetRoundedRegion(pnlFilterCard, 14);
            SetRoundedRegion(pnlDatePicker, 13);
            SetRoundedRegion(pnlTotal, 18);
            SetRoundedRegion(pnlPresent, 18);
            SetRoundedRegion(pnlAbsent, 18);

            // Bo góc trực tiếp DataGridView để phần đầu bảng (header) và
            // nền trắng phía dưới được cắt theo cùng một đường viền bo tròn.
            // Gọi lại trong OnResize để Region luôn khớp kích thước bảng.
            SetRoundedRegion(dgvPersonnelReport, 12);
        }

        private static void SetRoundedRegion(Control control, int radius)
        {
            if (control.Width <= 0 || control.Height <= 0) return;
            using (GraphicsPath path = CreateRoundedPath(
                new Rectangle(0, 0, control.Width, control.Height), radius))
            {
                control.Region?.Dispose();
                control.Region = new Region(path);
            }
        }

        private static GraphicsPath CreateRoundedPath(Rectangle b, int radius)
        {
            int d = Math.Max(2, radius * 2);
            GraphicsPath p = new GraphicsPath();
            p.AddArc(b.X, b.Y, d, d, 180, 90);
            p.AddArc(b.Right - d, b.Y, d, d, 270, 90);
            p.AddArc(b.Right - d, b.Bottom - d, d, d, 0, 90);
            p.AddArc(b.X, b.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }

        // Draw the calendar glyph next to the "Ngày báo cáo" label.
        private void lblReportDateIcon_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            Color blue = Color.FromArgb(0, 91, 170);
            using (Pen pen = new Pen(blue, 2.1F))
            using (SolidBrush brush = new SolidBrush(blue))
            {
                // Calendar body and top binding rings.
                g.DrawRectangle(pen, 6, 11, 18, 19);
                g.DrawLine(pen, 6, 16, 24, 16);
                g.DrawLine(pen, 11, 8, 11, 14);
                g.DrawLine(pen, 19, 8, 19, 14);
                // Small date cells.
                g.FillRectangle(brush, 10, 19, 2.5F, 2.5F);
                g.FillRectangle(brush, 15, 19, 2.5F, 2.5F);
                g.FillRectangle(brush, 20, 19, 2.5F, 2.5F);
                g.FillRectangle(brush, 10, 24, 2.5F, 2.5F);
                g.FillRectangle(brush, 15, 24, 2.5F, 2.5F);
            }
        }

        // Paint handlers được khai báo tường minh để Designer không tham chiếu
        // đến các phương thức chưa tồn tại.
        private void pnlFilter_Paint(object sender, PaintEventArgs e)
        {
            // The outer strip intentionally shows the form background as the
            // spacing around pnlFilterCard.
        }

        private void pnlFilterCard_Paint(object sender, PaintEventArgs e)
        {
            if (!(sender is Panel panel) || panel.Width <= 0 || panel.Height <= 0)
                return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            Rectangle bounds = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
            using (GraphicsPath card = CreateRoundedPath(bounds, 14))
            using (Pen border = new Pen(Color.FromArgb(225, 237, 250), 1F))
            {
                e.Graphics.DrawPath(border, card);
            }
        }

        private void pnlDatePicker_Paint(object sender, PaintEventArgs e)
        {
            if (!(sender is Panel panel) || panel.Width <= 0 || panel.Height <= 0)
                return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            Rectangle bounds = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
            using (GraphicsPath rounded = CreateRoundedPath(bounds, 13))
            using (SolidBrush background = new SolidBrush(Color.White))
            using (Pen border = new Pen(Color.FromArgb(190, 213, 242), 1F))
            {
                e.Graphics.FillPath(background, rounded);
                e.Graphics.DrawPath(border, rounded);
            }
        }

        private void pnlHeader_Paint(object sender, PaintEventArgs e)
        {
            if (!(sender is Panel panel) || panel.Width <= 0 || panel.Height <= 0)
                return;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            Rectangle bounds = panel.ClientRectangle;
            int w = bounds.Width;
            int h = bounds.Height;
            if (w <= 0 || h <= 0) return;

            // Bo góc toàn bộ banner giống mẫu thiết kế.
            using (GraphicsPath rounded = CreateRoundedPath(
                new Rectangle(0, 0, w - 1, h - 1), 14))
            {
                GraphicsState clipState = g.Save();
                g.SetClip(rounded);

                // Nền xanh navy chuyển sắc nhẹ, không tạo dải sáng ngang gắt.
                using (LinearGradientBrush background = new LinearGradientBrush(
                    new Rectangle(0, 0, w, h),
                    Color.FromArgb(8, 83, 157),
                    Color.FromArgb(3, 58, 116),
                    LinearGradientMode.Horizontal))
                {
                    background.SetBlendTriangularShape(0.42F, 0.18F);
                    g.FillPath(background, rounded);
                }

                // Ánh xanh nhẹ ở góc trái, hòa dần vào nền.
                using (LinearGradientBrush leftGlow = new LinearGradientBrush(
                    new Rectangle(0, 0, Math.Max(1, (int)(w * 0.62F)), h),
                    Color.FromArgb(35, 27, 125, 211),
                    Color.FromArgb(0, 27, 125, 211),
                    LinearGradientMode.Horizontal))
                {
                    g.FillRectangle(leftGlow, 0, 0, (int)(w * 0.62F), h);
                }

                // Dải sóng lớn phía trên: đường cong rộng, mềm như bản thiết kế.
                using (GraphicsPath upperWave = new GraphicsPath())
                using (SolidBrush upperBrush = new SolidBrush(Color.FromArgb(25, 100, 166, 235)))
                {
                    upperWave.StartFigure();
                    upperWave.AddBezier(w * 0.20F, h * 0.02F,
                        w * 0.43F, h * 0.02F, w * 0.55F, h * 0.76F, w * 0.76F, h * 0.70F);
                    upperWave.AddBezier(w * 0.76F, h * 0.70F,
                        w * 0.88F, h * 0.66F, w * 0.94F, h * 0.18F, w, h * 0.08F);
                    upperWave.AddLine(w, 0, w * 0.20F, 0);
                    upperWave.CloseFigure();
                    g.FillPath(upperBrush, upperWave);
                }

                // Sóng xanh đậm phía dưới, tạo chiều sâu nhưng không lấn nội dung.
                using (GraphicsPath lowerWave = new GraphicsPath())
                using (SolidBrush lowerBrush = new SolidBrush(Color.FromArgb(32, 0, 38, 91)))
                {
                    lowerWave.StartFigure();
                    lowerWave.AddBezier(w * 0.34F, h * 0.76F,
                        w * 0.53F, h * 0.98F, w * 0.75F, h * 0.35F, w, h * 0.18F);
                    lowerWave.AddLine(w, h, w * 0.34F, h);
                    lowerWave.CloseFigure();
                    g.FillPath(lowerBrush, lowerWave);
                }

                // Một đường cong phản sáng rất mảnh theo đường biên của dải sóng.
                using (GraphicsPath waveLine = new GraphicsPath())
                using (Pen wavePen = new Pen(Color.FromArgb(24, 164, 205, 255), 1.1F))
                {
                    waveLine.AddBezier(w * 0.28F, h * 0.80F,
                        w * 0.48F, h * 0.91F, w * 0.72F, h * 0.37F, w * 0.98F, h * 0.15F);
                    g.DrawPath(wavePen, waveLine);
                }

                // Biểu tượng biểu đồ cột: tỷ lệ thấp, rộng và cân đối hơn.
                float iconLeft = w * 0.025F;
                float iconBottom = h * 0.75F;
                using (Pen iconPen = new Pen(Color.White, 2.8F))
                {
                    iconPen.StartCap = LineCap.Square;
                    iconPen.EndCap = LineCap.Square;
                    g.DrawLine(iconPen, iconLeft, h * 0.25F, iconLeft, iconBottom);
                    g.DrawLine(iconPen, iconLeft, iconBottom, iconLeft + 42F, iconBottom);
                    g.DrawRectangle(iconPen, iconLeft + 8F, iconBottom - 19F, 6F, 19F);
                    g.DrawRectangle(iconPen, iconLeft + 20F, iconBottom - 31F, 6F, 31F);
                    g.DrawRectangle(iconPen, iconLeft + 32F, iconBottom - 43F, 6F, 43F);
                }

                // Huy hiệu nguyệt quế hoàn chỉnh ở phía phải banner.
                float emblemX = w * 0.875F;
                float emblemY = h * 0.61F;
                using (Pen branchPen = new Pen(Color.FromArgb(23, 119, 178, 239), 1.8F))
                using (SolidBrush leafBrush = new SolidBrush(Color.FromArgb(34, 111, 174, 235)))
                using (SolidBrush starBrush = new SolidBrush(Color.FromArgb(48, 111, 174, 235)))
                {
                    // Hai thân nguyệt quế đối xứng, ôm quanh ngôi sao.
                    using (GraphicsPath leftBranch = new GraphicsPath())
                    using (GraphicsPath rightBranch = new GraphicsPath())
                    {
                        leftBranch.AddBezier(emblemX - 82F, emblemY + 38F,
                            emblemX - 91F, emblemY + 2F, emblemX - 67F, emblemY - 43F,
                            emblemX - 39F, emblemY - 49F);
                        rightBranch.AddBezier(emblemX + 82F, emblemY + 38F,
                            emblemX + 91F, emblemY + 2F, emblemX + 67F, emblemY - 43F,
                            emblemX + 39F, emblemY - 49F);
                        g.DrawPath(branchPen, leftBranch);
                        g.DrawPath(branchPen, rightBranch);
                    }

                    // Lá được phân bố dọc hai nhánh, không bị dồn lên mép trên.
                    for (int i = 0; i < 9; i++)
                    {
                        float t = 0.08F + i * 0.105F;
                        float oneMinus = 1F - t;
                        float lx = oneMinus * oneMinus * oneMinus * (emblemX - 82F)
                            + 3F * oneMinus * oneMinus * t * (emblemX - 91F)
                            + 3F * oneMinus * t * t * (emblemX - 67F)
                            + t * t * t * (emblemX - 39F);
                        float ly = oneMinus * oneMinus * oneMinus * (emblemY + 38F)
                            + 3F * oneMinus * oneMinus * t * (emblemY + 2F)
                            + 3F * oneMinus * t * t * (emblemY - 43F)
                            + t * t * t * (emblemY - 49F);

                        GraphicsState leafState = g.Save();
                        g.TranslateTransform(lx, ly);
                        g.RotateTransform(-52F + i * 5F);
                        g.FillEllipse(leafBrush, -3.5F, -10F, 7F, 17F);
                        g.Restore(leafState);

                        float rx = 2F * emblemX - lx;
                        GraphicsState rightLeafState = g.Save();
                        g.TranslateTransform(rx, ly);
                        g.RotateTransform(52F - i * 5F);
                        g.FillEllipse(leafBrush, -3.5F, -10F, 7F, 17F);
                        g.Restore(rightLeafState);
                    }

                    // Ngôi sao năm cánh lớn, chìm nhẹ ở trung tâm huy hiệu.
                    PointF[] star = new PointF[10];
                    float outer = 39F;
                    float inner = 17F;
                    for (int i = 0; i < star.Length; i++)
                    {
                        double angle = (-90 + i * 36) * Math.PI / 180.0;
                        float radius = (i % 2 == 0) ? outer : inner;
                        star[i] = new PointF(
                            emblemX + (float)Math.Cos(angle) * radius,
                            emblemY + (float)Math.Sin(angle) * radius);
                    }
                    g.FillPolygon(starBrush, star);
                }

                g.Restore(clipState);
            }
        }

        private void pnlCard_Paint(object sender, PaintEventArgs e)
        {
            if (sender is not Panel panel || panel.Width <= 0 || panel.Height <= 0)
                return;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;

            bool isTotal = panel == pnlTotal;
            bool isPresent = panel == pnlPresent;

            Color topColor = isTotal
                ? Color.FromArgb(225, 239, 255)
                : isPresent
                    ? Color.FromArgb(229, 251, 239)
                    : Color.FromArgb(255, 239, 242);
            Color bottomColor = isTotal
                ? Color.FromArgb(207, 228, 253)
                : isPresent
                    ? Color.FromArgb(207, 246, 225)
                    : Color.FromArgb(255, 222, 229);
            Color accent = isTotal
                ? Color.FromArgb(0, 105, 230)
                : isPresent
                    ? Color.FromArgb(0, 174, 83)
                    : Color.FromArgb(230, 49, 66);

            using (GraphicsPath rounded = CreateRoundedPath(
                new Rectangle(0, 0, panel.Width - 1, panel.Height - 1), 20))
            {
                GraphicsState state = g.Save();
                g.SetClip(rounded);

                // Soft diagonal gradient, matching the three pastel card colors in the reference.
                using (LinearGradientBrush background = new LinearGradientBrush(
                    panel.ClientRectangle, topColor, bottomColor, LinearGradientMode.Vertical))
                {
                    g.FillRectangle(background, panel.ClientRectangle);
                }

                // A subtle glow keeps the center of each card light and clean.
                using (SolidBrush glow = new SolidBrush(Color.FromArgb(22, 255, 255, 255)))
                    g.FillEllipse(glow, panel.Width * 0.30F, -panel.Height * 0.55F,
                        panel.Width * 0.78F, panel.Height * 1.15F);

                // Low-contrast flowing bands along the lower edge.
                using (GraphicsPath lowerWave = new GraphicsPath())
                using (SolidBrush waveBrush = new SolidBrush(Color.FromArgb(25, accent)))
                {
                    lowerWave.StartFigure();
                    lowerWave.AddBezier(
                        panel.Width * 0.34F, panel.Height * 0.91F,
                        panel.Width * 0.53F, panel.Height * 0.70F,
                        panel.Width * 0.73F, panel.Height * 1.17F,
                        panel.Width * 0.91F, panel.Height * 0.91F);
                    lowerWave.AddBezier(
                        panel.Width * 0.91F, panel.Height * 0.91F,
                        panel.Width * 0.97F, panel.Height * 0.80F,
                        panel.Width * 1.02F, panel.Height * 0.84F,
                        panel.Width * 1.05F, panel.Height * 0.88F);
                    lowerWave.AddLine(panel.Width * 1.05F, panel.Height,
                        panel.Width * 0.34F, panel.Height);
                    lowerWave.CloseFigure();
                    g.FillPath(waveBrush, lowerWave);
                }

                using (GraphicsPath upperWave = new GraphicsPath())
                using (SolidBrush waveBrush = new SolidBrush(Color.FromArgb(14, accent)))
                {
                    upperWave.StartFigure();
                    upperWave.AddBezier(
                        panel.Width * 0.52F, panel.Height * 0.98F,
                        panel.Width * 0.70F, panel.Height * 0.84F,
                        panel.Width * 0.82F, panel.Height * 1.08F,
                        panel.Width * 1.02F, panel.Height * 0.96F);
                    upperWave.AddLine(panel.Width * 1.02F, panel.Height,
                        panel.Width * 0.52F, panel.Height);
                    upperWave.CloseFigure();
                    g.FillPath(waveBrush, upperWave);
                }

                // Decorative, translucent people silhouettes at the far right.
                // They remain behind the number and never compete with the main content.
                using (SolidBrush watermark = new SolidBrush(Color.FromArgb(22, accent)))
                {
                    DrawPersonWatermark(g, watermark, panel.Width - 34, panel.Height / 2 + 10, 0.92F);
                    DrawPersonWatermark(g, watermark, panel.Width - 78, panel.Height / 2 + 17, 0.72F);
                }

                // Fine white divider between the description and the numeric value.
                int dividerX = panel.Width - 176;
                using (Pen divider = new Pen(Color.FromArgb(118, 255, 255, 255), 1.4F))
                    g.DrawLine(divider, dividerX, 17, dividerX, panel.Height - 17);

                // Vector icons are drawn directly for consistent rendering on every PC.
                int cx = 72;
                int cy = panel.Height / 2;

                if (isTotal)
                {
                    using (SolidBrush blue = new SolidBrush(accent))
                    {
                        // Three heads.
                        g.FillEllipse(blue, cx - 9, cy - 25, 19, 19);
                        g.FillEllipse(blue, cx - 30, cy - 17, 17, 17);
                        g.FillEllipse(blue, cx + 13, cy - 17, 17, 17);

                        // Three connected shoulders/bodies.
                        g.FillEllipse(blue, cx - 23, cy - 2, 46, 27);
                        g.FillEllipse(blue, cx - 43, cy + 1, 28, 21);
                        g.FillEllipse(blue, cx + 15, cy + 1, 28, 21);
                    }
                }
                else if (isPresent)
                {
                    using (SolidBrush green = new SolidBrush(accent))
                        g.FillEllipse(green, cx - 24, cy - 24, 48, 48);
                    using (Pen check = new Pen(Color.White, 5F))
                    {
                        check.StartCap = LineCap.Round;
                        check.EndCap = LineCap.Round;
                        g.DrawLines(check, new[]
                        {
                            new Point(cx - 12, cy),
                            new Point(cx - 3, cy + 9),
                            new Point(cx + 14, cy - 11)
                        });
                    }
                }
                else
                {
                    using (SolidBrush red = new SolidBrush(accent))
                        g.FillEllipse(red, cx - 24, cy - 24, 48, 48);
                    using (Pen cross = new Pen(Color.White, 5F))
                    {
                        cross.StartCap = LineCap.Round;
                        cross.EndCap = LineCap.Round;
                        g.DrawLine(cross, cx - 10, cy - 10, cx + 10, cy + 10);
                        g.DrawLine(cross, cx + 10, cy - 10, cx - 10, cy + 10);
                    }
                }

                g.Restore(state);
            }
        }

        private static void DrawPersonWatermark(
            Graphics graphics, Brush brush, float centerX, float centerY, float scale)
        {
            float head = 18F * scale;
            float bodyWidth = 25F * scale;
            float bodyHeight = 30F * scale;

            graphics.FillEllipse(
                brush,
                centerX - head / 2F,
                centerY - 27F * scale,
                head,
                head);

            using (GraphicsPath body = new GraphicsPath())
            {
                float x = centerX - bodyWidth / 2F;
                float y = centerY - 5F * scale;
                float radius = 8F * scale;
                body.AddArc(x, y, radius * 2F, radius * 2F, 180, 90);
                body.AddArc(x + bodyWidth - radius * 2F, y,
                    radius * 2F, radius * 2F, 270, 90);
                body.AddLine(x + bodyWidth, y + radius, x + bodyWidth, y + bodyHeight);
                body.AddLine(x + bodyWidth, y + bodyHeight, x, y + bodyHeight);
                body.AddLine(x, y + bodyHeight, x, y + radius);
                body.CloseFigure();
                graphics.FillPath(brush, body);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (btnRefresh != null && btnPreview != null)
                ApplyRoundedCorners();
            LayoutStatisticCards();
        }

        private void LayoutStatisticCards()
        {
            LayoutStatisticCard(pnlTotal, lblTotalTitle, lblTotalCount);
            LayoutStatisticCard(pnlPresent, lblPresentTitle, lblPresentCount);
            LayoutStatisticCard(pnlAbsent, lblAbsentTitle, lblAbsentCount);
        }

        private static void LayoutStatisticCard(Panel panel, Label title, Label count)
        {
            if (panel == null || title == null || count == null || panel.Width <= 0)
                return;

            int dividerX = panel.Width - 176;
            title.Location = new Point(130, Math.Max(0, (panel.Height - 36) / 2));
            title.Size = new Size(Math.Max(100, dividerX - 145), 36);

            count.Location = new Point(
                dividerX + (panel.Width - dividerX - 88) / 2,
                Math.Max(0, (panel.Height - 70) / 2));
            count.Size = new Size(88, 70);
            count.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        }
    }
}