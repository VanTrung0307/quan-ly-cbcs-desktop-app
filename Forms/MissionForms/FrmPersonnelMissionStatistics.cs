using QuanLyCBCS.Forms.Auth;
using QuanLyCBCS.DTOs;
using QuanLyCBCS.Services;
using QuanLyCBCS.Helpers;
using System.Drawing.Drawing2D;

namespace QuanLyCBCS.Forms.MissionForms;

public partial class FrmPersonnelMissionStatistics : Form
{
    private readonly MissionPersonnelService
        _missionPersonnelService = new();

    private List<PersonnelMissionStatisticDto>
        _statistics = new();

    private readonly
        PersonnelMissionStatisticExportService
            _exportService =
                new();

    private Label? _personnelValueLabel;
    private Label? _missionValueLabel;
    private Label? _footerLabel;

    private static readonly Color Navy = Color.FromArgb(13, 43, 91);
    private static readonly Color Blue = Color.FromArgb(0, 105, 220);
    private static readonly Color HeaderBlue = Color.FromArgb(0, 83, 166);
    private static readonly Color BorderBlue = Color.FromArgb(204, 225, 255);

    public FrmPersonnelMissionStatistics()
    {
        InitializeComponent();
        BuildModernLayout();
    }

    private void BuildModernLayout()
    {
        SuspendLayout();
        Controls.Clear();
        BackColor = Color.FromArgb(242, 248, 255);
        Font = new Font("Segoe UI", 10F);
        MinimumSize = new Size(900, 560);
        FormBorderStyle = FormBorderStyle.None;
        Dock = DockStyle.Fill;

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            BackColor = BackColor,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 108));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 69));
        Controls.Add(root);

        // Banner tiêu đề: gradient xanh, bo góc và huy hiệu chìm bên phải.
        pnlHeader.Dock = DockStyle.Fill;
        pnlHeader.Margin = new Padding(0, 6, 0, 0);
        pnlHeader.Padding = Padding.Empty;
        pnlHeader.BackColor = HeaderBlue;
        pnlHeader.Controls.Clear();
        pnlHeader.Paint += (_, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var brush = new LinearGradientBrush(
                pnlHeader.ClientRectangle,
                Color.FromArgb(0, 119, 226),
                Color.FromArgb(0, 65, 139), 0f);
            e.Graphics.FillRectangle(brush, pnlHeader.ClientRectangle);

            // Ngôi sao và vòng nguyệt quế cân đối, nét mảnh như bản thiết kế.
            float cx = pnlHeader.Width - 105F;
            float cy = 46F;
            using var watermark = new SolidBrush(Color.FromArgb(34, Color.White));
            using var starPen = new Pen(Color.FromArgb(48, Color.White), 1.6F);
            using var laurelPen = new Pen(Color.FromArgb(54, Color.White), 1.8F)
            {
                LineJoin = LineJoin.Round,
                StartCap = LineCap.Round,
                EndCap = LineCap.Round
            };
            using var leafBrush = new SolidBrush(Color.FromArgb(43, Color.White));
            PointF[] star = CreateStarPoints(cx, cy, 37F, 16F, 5);
            e.Graphics.FillPolygon(watermark, star);
            e.Graphics.DrawPolygon(starPen, star);

            using (var leftBranch = new GraphicsPath())
            using (var rightBranch = new GraphicsPath())
            {
                leftBranch.AddBezier(cx - 21, cy + 42, cx - 77, cy + 28, cx - 92, cy - 25, cx - 66, cy - 43);
                rightBranch.AddBezier(cx + 21, cy + 42, cx + 77, cy + 28, cx + 92, cy - 25, cx + 66, cy - 43);
                e.Graphics.DrawPath(laurelPen, leftBranch);
                e.Graphics.DrawPath(laurelPen, rightBranch);
            }
            for (int i = 0; i < 9; i++)
            {
                float t = i / 8F;
                float y = cy + 31F - i * 8.2F;
                float lx = cx - 32F - (float)Math.Sin(t * Math.PI) * 43F;
                float rx = cx + 32F + (float)Math.Sin(t * Math.PI) * 43F;
                GraphicsState stateL = e.Graphics.Save();
                e.Graphics.TranslateTransform(lx, y);
                e.Graphics.RotateTransform(-38F + i * 2.4F);
                e.Graphics.FillEllipse(leafBrush, -4.1F, -8.2F, 8.2F, 16.4F);
                e.Graphics.Restore(stateL);
                GraphicsState stateR = e.Graphics.Save();
                e.Graphics.TranslateTransform(rx, y);
                e.Graphics.RotateTransform(38F - i * 2.4F);
                e.Graphics.FillEllipse(leafBrush, -4.1F, -8.2F, 8.2F, 16.4F);
                e.Graphics.Restore(stateR);
            }
        };
        ApplyRoundedRegion(pnlHeader, 13);

        var headerIcon = MakeDrawingIcon("banner-chart", Color.White);
        headerIcon.SetBounds(29, 25, 48, 48);
        headerIcon.TextAlign = ContentAlignment.MiddleCenter;
        lblTitle.Dock = DockStyle.None;
        lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
        lblTitle.ForeColor = Color.White;
        lblTitle.Text = "THỐNG KÊ CÔNG TÁC CBCS";
        lblTitle.TextAlign = ContentAlignment.MiddleLeft;
        lblTitle.SetBounds(116, 10, 700, 43);
        var subtitle = MakeLabel("Tra cứu, tổng hợp và thống kê công tác CBCS", 11F, FontStyle.Regular, Color.White);
        subtitle.SetBounds(114, 53, 650, 27);
        pnlHeader.Controls.Add(headerIcon);
        pnlHeader.Controls.Add(lblTitle);
        pnlHeader.Controls.Add(subtitle);
        root.Controls.Add(pnlHeader, 0, 0);

        // Hai cột nội dung: tăng bề rộng cột trái đúng tỷ lệ bản thiết kế.
        var body = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = Color.Transparent,
            Padding = new Padding(7, 14, 15, 10),
            Margin = Padding.Empty
        };
        body.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 364));
        body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        body.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.Controls.Add(body, 0, 1);

        // Thẻ tổng hợp bên trái.
        var summary = CreateCard();
        summary.Dock = DockStyle.Top;
        summary.Height = 294;
        summary.Margin = new Padding(0, 3, 17, 0);
        summary.Padding = Padding.Empty;
        body.Controls.Add(summary, 0, 0);

        var summaryHeader = new Panel
        {
            Location = new Point(0, 0),
            Size = new Size(summary.ClientSize.Width, 42),
            Dock = DockStyle.Top,
            BackColor = Color.FromArgb(218, 237, 255),
            Margin = Padding.Empty
        };
        var summaryHeaderIcon = MakeDrawingIcon("chart-small", Blue);
        summaryHeaderIcon.SetBounds(20, 5, 30, 31);
        var summaryTitle = MakeLabel("THÔNG TIN TỔNG HỢP", 11.5F, FontStyle.Bold, Navy);
        summaryTitle.SetBounds(56, 0, 260, 42);
        summaryTitle.TextAlign = ContentAlignment.MiddleLeft;
        summaryHeader.Controls.Add(summaryHeaderIcon);
        summaryHeader.Controls.Add(summaryTitle);
        summary.Controls.Add(summaryHeader);

        var stat1 = CreateStatCard("people", "Tổng CBCS", out _personnelValueLabel);
        stat1.Location = new Point(13, 58);
        stat1.Size = new Size(322, 56);
        stat1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        summary.Controls.Add(stat1);

        var stat2 = CreateStatCard("clipboard", "Tổng lượt công tác", out _missionValueLabel);
        stat2.Location = new Point(13, 124);
        stat2.Size = new Size(322, 56);
        stat2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        summary.Controls.Add(stat2);

        var separator = new Panel
        {
            Location = new Point(14, 209),
            Size = new Size(320, 1),
            BackColor = BorderBlue,
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };
        summary.Controls.Add(separator);

        // Slightly wider pair with Segoe Fluent Icons, centered inside the summary card.
        const int summaryButtonsWidth = 280;
        const int summaryButtonsHeight = 36;
        var summaryButtons = new TableLayoutPanel
        {
            Location = new Point(
                Math.Max(0, (summary.ClientSize.Width - summaryButtonsWidth) / 2), 228),
            Size = new Size(summaryButtonsWidth, summaryButtonsHeight),
            Anchor = AnchorStyles.Top,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = Color.Transparent,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        summaryButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        summaryButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        summaryButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        // Owner-drawn buttons using Segoe Fluent Icons glyphs (Trophy and History).
        var top10Surface = CreateActionSurface(
            "\uE7C0", "Top 10", false, 8, 16, 10.5F,
            (_, _) => btnTop10_Click(btnTop10, EventArgs.Empty));
        top10Surface.Margin = new Padding(0, 0, 5, 0);
        var historySurface = CreateActionSurface(
            "\uE81C", "Lịch sử", false, 8, 16, 10.5F,
            (_, _) => btnHistory_Click(btnHistory, EventArgs.Empty));
        historySurface.Margin = new Padding(5, 0, 0, 0);

        summaryButtons.Controls.Add(top10Surface, 0, 0);
        summaryButtons.Controls.Add(historySurface, 1, 0);
        summary.Controls.Add(summaryButtons);
        summary.Resize += (_, _) =>
        {
            // Đồng bộ chiều rộng theo kích thước thực tế của thẻ tổng hợp.
            // Cột trái có thể bị thu hẹp theo DPI/kích thước cửa sổ; không giữ
            // chiều rộng cố định 322 px vì sẽ làm tràn và cắt phần số liệu.
            int contentWidth = Math.Max(120, summary.ClientSize.Width - 26);
            summaryHeader.Width = summary.ClientSize.Width;
            summaryTitle.Width = Math.Max(120, summaryHeader.ClientSize.Width - summaryTitle.Left - 12);
            stat1.Width = contentWidth;
            stat2.Width = contentWidth;
            separator.Width = Math.Max(80, summary.ClientSize.Width - 28);
            summaryButtons.Left = Math.Max(0, (summary.ClientSize.Width - summaryButtonsWidth) / 2);
        };

        // Áp dụng ngay cả khi thẻ đã có kích thước trước khi đăng ký Resize.
        int initialContentWidth = Math.Max(120, summary.ClientSize.Width - 26);
        stat1.Width = initialContentWidth;
        stat2.Width = initialContentWidth;
        separator.Width = Math.Max(80, summary.ClientSize.Width - 28);

        // Thanh công cụ và bảng dữ liệu bên phải.
        var right = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            BackColor = Color.Transparent,
            Margin = Padding.Empty
        };
        right.RowStyles.Add(new RowStyle(SizeType.Absolute, 62));
        right.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        body.Controls.Add(right, 1, 0);

        var toolbar = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 4,
            RowCount = 1,
            BackColor = Color.Transparent,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        // Search field: tăng rõ thêm 30 px so với V9, giữ dáng thanh mảnh.
        // Cột toolbar được tăng đồng bộ để không cắt phần bên phải ô tìm kiếm.
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 390));
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 136));
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 126));
        right.Controls.Add(toolbar, 0, 0);

        var searchHost = new Panel
        {
            // Kích thước cố định, tăng nhẹ chiều rộng và chiều cao; nằm trọn phía trên DataGridView.
            Dock = DockStyle.None,
            Location = new Point(0, 14),
            Size = new Size(390, 36),
            BackColor = Color.White,
            Margin = Padding.Empty,
            Padding = Padding.Empty,
            Anchor = AnchorStyles.Top | AnchorStyles.Left
        };
        ApplyRoundedRegion(searchHost, 18);
        searchHost.Paint += (_, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var pen = new Pen(Color.FromArgb(198, 220, 250), 1F);
            using var path = RoundedPath(
                new Rectangle(1, 1, Math.Max(1, searchHost.Width - 3), Math.Max(1, searchHost.Height - 3)),
                Math.Max(10, (searchHost.Height - 3) / 2));
            e.Graphics.DrawPath(pen, path);
        };

        // Vẽ kính lúp bằng GDI+ thay cho glyph font để không bị cắt nét bên phải.
        var searchIcon = new Panel
        {
            Location = new Point(9, 6),
            Size = new Size(22, 22),
            BackColor = Color.Transparent,
            Margin = Padding.Empty
        };
        searchIcon.Paint += (_, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var lensPen = new Pen(Color.FromArgb(0, 92, 190), 1.65F)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round,
                LineJoin = LineJoin.Round
            };
            e.Graphics.DrawEllipse(lensPen, 1.4F, 1.2F, 13.2F, 13.2F);
            e.Graphics.DrawLine(lensPen, 12.2F, 12.2F, 20.2F, 20.2F);
        };
        txtSearch.Dock = DockStyle.None;
        txtSearch.SetBounds(36, 6, 122, 23);
        txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtSearch.Font = new Font("Segoe UI", 9.5F);
        txtSearch.BackColor = Color.White;
        txtSearch.ForeColor = Navy;
        txtSearch.BorderStyle = BorderStyle.None;
        txtSearch.PlaceholderText = "Tìm theo họ tên, số hiệu, đơn vị...";
        searchHost.Controls.Add(searchIcon);
        searchHost.Controls.Add(txtSearch);
        searchHost.Resize += (_, _) =>
        {
            searchIcon.Left = 9;
            searchIcon.Top = Math.Max(0, (searchHost.ClientSize.Height - searchIcon.Height) / 2);
            searchIcon.Invalidate();
            txtSearch.Left = 36;
            txtSearch.Top = Math.Max(0, (searchHost.ClientSize.Height - txtSearch.Height) / 2);
            txtSearch.Width = Math.Max(60, searchHost.ClientSize.Width - 44);
        };
        toolbar.Controls.Add(searchHost, 0, 0);

        // Khoảng đệm giữa ô tìm kiếm và cụm nút, theo đúng bố cục tham chiếu.
        toolbar.Controls.Add(new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.Transparent,
            Margin = Padding.Empty
        }, 1, 0);

        var refreshSurface = CreateActionSurface(
            "\uE72C", "Làm mới", false, 9, 18, 10.2F,
            (_, _) => btnRefresh_Click(btnRefresh, EventArgs.Empty));
        refreshSurface.Dock = DockStyle.None;
        refreshSurface.Size = new Size(128, 35);
        refreshSurface.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        refreshSurface.Margin = new Padding(0, 10, 8, 0);
        toolbar.Controls.Add(refreshSurface, 2, 0);

        var excelSurface = CreateActionSurface(
            "\uE9F9", "Excel", true, 9, 18, 10.2F,
            (_, _) => btnExportExcel_Click(btnExportExcel, EventArgs.Empty));
        excelSurface.Dock = DockStyle.None;
        excelSurface.Size = new Size(118, 35);
        excelSurface.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        excelSurface.Margin = new Padding(0, 10, 14, 0);
        toolbar.Controls.Add(excelSurface, 3, 0);

        var gridCard = CreateCard();
        gridCard.Dock = DockStyle.Fill;
        gridCard.Padding = new Padding(1);
        gridCard.Margin = new Padding(0, 0, 0, 8);
        right.Controls.Add(gridCard, 0, 1);

        dgvStatistics.Dock = DockStyle.Fill;
        dgvStatistics.Margin = Padding.Empty;
        dgvStatistics.BackgroundColor = Color.White;
        dgvStatistics.BorderStyle = BorderStyle.None;
        dgvStatistics.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        dgvStatistics.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.None;
        dgvStatistics.GridColor = Color.FromArgb(215, 229, 246);
        dgvStatistics.EnableHeadersVisualStyles = false;
        dgvStatistics.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 83, 166);
        dgvStatistics.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgvStatistics.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        dgvStatistics.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dgvStatistics.DefaultCellStyle.Font = new Font("Segoe UI", 10.5F);
        dgvStatistics.DefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
        dgvStatistics.DefaultCellStyle.ForeColor = Color.FromArgb(17, 38, 75);
        dgvStatistics.DefaultCellStyle.SelectionBackColor = Color.FromArgb(167, 220, 255);
        dgvStatistics.DefaultCellStyle.SelectionForeColor = Color.FromArgb(17, 38, 75);
        dgvStatistics.RowTemplate.Height = 46;
        dgvStatistics.ColumnHeadersHeight = 46;
        dgvStatistics.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgvStatistics.CellBorderStyle = DataGridViewCellBorderStyle.Single;
        dgvStatistics.DataBindingComplete += (_, _) => ApplyRowHighlights();
        dgvStatistics.CellPainting += DgvStatistics_CellPainting;
        gridCard.Controls.Add(dgvStatistics);

        // Thanh thông tin cuối trang.
        var footer = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(218, 237, 255),
            Margin = new Padding(15, 0, 15, 15),
            Padding = new Padding(14, 0, 10, 0)
        };
        ApplyRoundedRegion(footer, 10);
        var footerIcon = new Label { BackColor = Color.Transparent };
        footerIcon.SetBounds(8, 0, 36, 46);
        footerIcon.Paint += (_, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var infoPen = new Pen(Blue, 1.8F);
            using var infoFont = new Font("Segoe UI", 12F, FontStyle.Bold);
            e.Graphics.DrawEllipse(infoPen, 5, 11, 24, 24);
            TextRenderer.DrawText(e.Graphics, "i", infoFont,
                new Rectangle(5, 10, 24, 25), Blue,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
        };
        _footerLabel = MakeLabel("Hiển thị: 0 CBCS     |     Tổng lượt công tác: 0", 10F, FontStyle.Bold, Navy);
        _footerLabel.SetBounds(49, 0, 700, 46);
        _footerLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _footerLabel.TextAlign = ContentAlignment.MiddleLeft;
        footer.Controls.Add(footerIcon);
        footer.Controls.Add(_footerLabel);
        root.Controls.Add(footer, 0, 2);

        btnClose.Visible = false;
        lblTotalPersonnel.Visible = false;
        lblTotalMissionCount.Visible = false;
        ResumeLayout(true);
    }

    private static Label MakeLabel(string text, float size, FontStyle style, Color color)
        => new Label
        {
            Text = text,
            Font = new Font("Segoe UI", size, style),
            ForeColor = color,
            BackColor = Color.Transparent,
            AutoEllipsis = true
        };

    private static Label MakeIconLabel(string glyph, float size, Color color)
        => new Label
        {
            Text = glyph,
            Font = new Font("Segoe MDL2 Assets", size, FontStyle.Regular),
            ForeColor = color,
            BackColor = Color.Transparent,
            AutoEllipsis = false,
            TextAlign = ContentAlignment.MiddleCenter
        };

    private static Panel CreateCard()
    {
        var panel = new Panel { BackColor = Color.White, BorderStyle = BorderStyle.None };
        ApplyRoundedRegion(panel, 11);
        panel.Paint += (_, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var pen = new Pen(BorderBlue, 1F);
            using var path = RoundedPath(new Rectangle(0, 0, panel.Width - 1, panel.Height - 1), 11);
            e.Graphics.DrawPath(pen, path);
        };
        return panel;
    }

    private static Panel CreateStatCard(string glyph, string caption, out Label valueLabel)
    {
        var panel = new Panel
        {
            BackColor = Color.FromArgb(244, 249, 255),
            Padding = new Padding(8, 4, 8, 4)
        };
        ApplyRoundedRegion(panel, 9);
        var icon = MakeDrawingIcon(glyph, Blue);
        icon.SetBounds(10, 5, 40, 46);
        var title = MakeLabel(caption, 10.5F, FontStyle.Bold, Navy);
        title.SetBounds(61, 3, 205, 50);
        title.TextAlign = ContentAlignment.MiddleLeft;
        valueLabel = MakeLabel("0", 20F, FontStyle.Bold, Navy);
        // Chừa vùng đủ rộng cho số liệu 2–4 chữ số khi thẻ bị thu hẹp theo DPI.
        valueLabel.SetBounds(240, 3, 66, 50);
        valueLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        valueLabel.TextAlign = ContentAlignment.MiddleRight;
        panel.Controls.Add(icon);
        panel.Controls.Add(title);
        panel.Controls.Add(valueLabel);

        Label valueLabelForResize = valueLabel;
        panel.Resize += (_, _) =>
        {
            // Căn số liệu theo mép phải thực tế của thẻ, không dùng tọa độ cố định.
            valueLabelForResize.Left = Math.Max(150, panel.ClientSize.Width - valueLabelForResize.Width - 10);
            title.Width = Math.Max(90, valueLabelForResize.Left - title.Left - 8);
        };

        // Căn lại ngay sau khi tạo, không phụ thuộc lần phát sinh Resize đầu tiên.
        valueLabelForResize.Left = Math.Max(150, panel.ClientSize.Width - valueLabelForResize.Width - 10);
        title.Width = Math.Max(90, valueLabelForResize.Left - title.Left - 8);
        panel.Paint += (_, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var pen = new Pen(Color.FromArgb(225, 237, 251), 1F);
            using var path = RoundedPath(new Rectangle(0, 0, panel.Width - 1, panel.Height - 1), 9);
            e.Graphics.DrawPath(pen, path);
        };
        return panel;
    }

    private static Label MakeDrawingIcon(string kind, Color color)
    {
        var icon = new Label
        {
            BackColor = Color.Transparent,
            ForeColor = color,
            Text = string.Empty
        };
        icon.Paint += (_, e) => DrawStatisticsIcon(e.Graphics, icon.ClientRectangle, kind, color);
        return icon;
    }

    private static void DrawStatisticsIcon(Graphics graphics, Rectangle bounds, string kind, Color color)
    {
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        float scale = Math.Min(bounds.Width, bounds.Height) / 32f;
        float ox = (bounds.Width - 32 * scale) / 2f;
        float oy = (bounds.Height - 32 * scale) / 2f;
        using var pen = new Pen(color, kind == "banner-chart" ? Math.Max(1.25f, 1.45f * scale) : (kind == "refresh" || kind == "excel" ? Math.Max(1.5f, 2.2f * scale) : (kind == "trophy" || kind == "history" ? Math.Max(1.8f, 2.25f * scale) : Math.Max(1.5f, 1.8f * scale)))) { LineJoin = LineJoin.Round, StartCap = LineCap.Round, EndCap = LineCap.Round };
        using var fill = new SolidBrush(color);
        using var path = new GraphicsPath();
        GraphicsState state = graphics.Save();
        graphics.TranslateTransform(ox, oy);
        graphics.ScaleTransform(scale, scale);

        if (kind == "banner-chart")
        {
            graphics.DrawLine(pen, 3, 29, 3, 3);
            graphics.DrawLine(pen, 3, 29, 30, 29);
            graphics.DrawRectangle(pen, 7, 18, 5, 10);
            graphics.DrawRectangle(pen, 15, 13, 5, 15);
            graphics.DrawRectangle(pen, 23, 7, 5, 21);
            graphics.DrawLines(pen, new[] { new PointF(5, 15), new PointF(12, 10), new PointF(18, 12), new PointF(27, 1.5F) });
            graphics.DrawLine(pen, 22, 1.5F, 27, 1.5F);
            graphics.DrawLine(pen, 27, 1.5F, 27, 6.5F);
        }
        else if (kind == "chart" || kind == "chart-small")
        {
            graphics.DrawLine(pen, 3, 28, 3, 4);
            graphics.DrawLine(pen, 3, 28, 30, 28);
            graphics.DrawRectangle(pen, 7, 17, 5, 10);
            graphics.DrawRectangle(pen, 15, 11, 5, 16);
            graphics.DrawRectangle(pen, 23, 5, 5, 22);
            graphics.DrawLines(pen, new[] { new PointF(6, 13), new PointF(13, 8), new PointF(19, 10), new PointF(27, 2) });
            graphics.DrawLine(pen, 22, 2, 27, 2);
            graphics.DrawLine(pen, 27, 2, 27, 7);
        }
        else if (kind == "people")
        {
            graphics.FillEllipse(fill, 12, 2, 9, 9);
            graphics.FillEllipse(fill, 2, 8, 8, 8);
            graphics.FillEllipse(fill, 22, 8, 8, 8);
            graphics.DrawArc(pen, 7, 12, 19, 18, 180, 180);
            graphics.DrawArc(pen, -2, 17, 16, 13, 190, 155);
            graphics.DrawArc(pen, 18, 17, 16, 13, 195, 155);
        }
        else if (kind == "clipboard")
        {
            graphics.DrawRectangle(pen, 6, 5, 21, 25);
            graphics.DrawRectangle(pen, 11, 2, 11, 7);
            graphics.DrawLine(pen, 11, 14, 22, 14);
            graphics.DrawLine(pen, 11, 19, 22, 19);
            graphics.DrawLine(pen, 11, 24, 19, 24);
        }
        else if (kind == "trophy")
        {
            graphics.DrawLine(pen, 10, 5, 22, 5);
            graphics.DrawLine(pen, 11, 5, 12, 16);
            graphics.DrawLine(pen, 21, 5, 20, 16);
            graphics.DrawArc(pen, 11, 10, 10, 12, 0, 180);
            graphics.DrawArc(pen, 3, 6, 10, 11, 260, 190);
            graphics.DrawArc(pen, 19, 6, 10, 11, 90, 190);
            graphics.DrawLine(pen, 16, 22, 16, 27);
            graphics.DrawLine(pen, 10, 28, 22, 28);
        }
        else if (kind == "history")
        {
            graphics.DrawArc(pen, 4, 4, 24, 24, 45, 300);
            graphics.DrawLine(pen, 4, 4, 4, 11);
            graphics.DrawLine(pen, 4, 4, 11, 4);
            graphics.DrawLine(pen, 16, 9, 16, 16);
            graphics.DrawLine(pen, 16, 16, 22, 19);
        }
        else if (kind == "refresh")
        {
            // Clear refresh glyph: one continuous circular arrow with a defined arrowhead.
            graphics.DrawArc(pen, 5.5F, 5.5F, 21, 21, 42, 286);
            graphics.DrawLines(pen, new[]
            {
                new PointF(22.5F, 4.5F),
                new PointF(28.5F, 5.0F),
                new PointF(27.0F, 11.0F)
            });
        }
        else if (kind == "excel")
        {
            // Compact Excel-style document mark, centered in the icon box.
            using var pageFill = new SolidBrush(color);
            using var pagePath = new GraphicsPath();
            pagePath.AddRectangle(new RectangleF(10, 3.5F, 18, 25));
            graphics.FillPath(pageFill, pagePath);
            graphics.DrawRectangle(pen, 10, 3.5F, 18, 25);
            graphics.DrawLine(pen, 21, 3.5F, 21, 9.5F);
            graphics.DrawLine(pen, 21, 9.5F, 28, 9.5F);
            graphics.DrawLine(pen, 15, 14, 24, 14);
            graphics.DrawLine(pen, 15, 19, 24, 19);
            graphics.DrawLine(pen, 15, 24, 24, 24);

            // White Excel tile with a green X, matching the reference toolbar icon.
            using var xTile = new SolidBrush(Color.White);
            graphics.FillRectangle(xTile, 2.5F, 10.5F, 13, 14);
            using var xPen = new Pen(Color.FromArgb(16, 177, 91), Math.Max(1.8F, 2.3F * scale))
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round
            };
            graphics.DrawLine(xPen, 5.5F, 14, 12.5F, 21);
            graphics.DrawLine(xPen, 12.5F, 14, 5.5F, 21);
        }
        graphics.Restore(state);
    }

    private static PointF[] CreateStarPoints(float cx, float cy, float outer, float inner, int points)
    {
        var result = new PointF[points * 2];
        for (int i = 0; i < result.Length; i++)
        {
            double angle = -Math.PI / 2 + i * Math.PI / points;
            float radius = i % 2 == 0 ? outer : inner;
            result[i] = new PointF(cx + (float)Math.Cos(angle) * radius, cy + (float)Math.Sin(angle) * radius);
        }
        return result;
    }

    private static Panel CreateActionSurface(
        string glyph,
        string caption,
        bool filled,
        int radius,
        int iconSize,
        float fontSize,
        EventHandler onClick)
    {
        var surface = new Panel
        {
            BackColor = Color.White,
            Cursor = Cursors.Hand,
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
            Padding = Padding.Empty,
            TabStop = true
        };

        Color borderColor = Color.FromArgb(0, 111, 235);
        Color fillColor = Color.FromArgb(16, 177, 91);
        Color foreground = filled ? Color.White : Color.FromArgb(0, 82, 174);

        surface.Paint += (_, e) =>
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            Rectangle face = new Rectangle(
                1, 1,
                Math.Max(1, surface.ClientSize.Width - 3),
                Math.Max(1, surface.ClientSize.Height - 3));
            using var facePath = RoundedPath(face, radius);
            using var background = new SolidBrush(filled ? fillColor : Color.White);
            using var border = new Pen(filled ? fillColor : borderColor, 1.15F);
            g.FillPath(background, facePath);
            g.DrawPath(border, facePath);

            using var textFont = new Font("Segoe UI", fontSize, FontStyle.Bold);

            // Measure and draw with the same GDI+ typographic metrics so the
            // icon + caption are centered as one group at every DPI setting.
            using var textFormat = (StringFormat)StringFormat.GenericTypographic.Clone();
            textFormat.FormatFlags |= StringFormatFlags.NoWrap;
            textFormat.Alignment = StringAlignment.Near;
            textFormat.LineAlignment = StringAlignment.Center;

            // Measure the caption at the origin. Passing int.MaxValue as a
            // layout width can produce an unusably large measurement on some
            // GDI+ configurations, pushing the icon to the left edge.
            SizeF measuredText = g.MeasureString(
                caption, textFont, PointF.Empty, textFormat);

            bool showIcon = !string.Equals(glyph, "none", StringComparison.OrdinalIgnoreCase);
            float gap = showIcon ? 5F : 0F;
            float glyphWidth = showIcon ? Math.Max(14F, iconSize) : 0F;
            float groupWidth = measuredText.Width + (showIcon ? glyphWidth + gap : 0F);
            float startX = Math.Max(0F, (surface.ClientSize.Width - groupWidth) / 2F);

            if (showIcon)
            {
                // Segoe Fluent Icons glyphs are drawn as a font glyph, not rebuilt
                // with GDI+ paths. The full-height rectangle vertically centers it
                // on the same line as the caption.
                using var fluentIconFont = new Font("Segoe Fluent Icons", 12F, FontStyle.Regular, GraphicsUnit.Point);
                using var iconBrush = new SolidBrush(foreground);
                using var iconFormat = (StringFormat)StringFormat.GenericTypographic.Clone();
                iconFormat.Alignment = StringAlignment.Center;
                iconFormat.LineAlignment = StringAlignment.Center;
                iconFormat.FormatFlags |= StringFormatFlags.NoWrap;
                var iconRect = new RectangleF(startX, 0F, glyphWidth, surface.ClientSize.Height);
                g.DrawString(glyph, fluentIconFont, iconBrush, iconRect, iconFormat);
            }

            var textRect = new RectangleF(
                startX + (showIcon ? glyphWidth + gap : 0F),
                0F,
                Math.Max(1F, measuredText.Width + 2F),
                surface.ClientSize.Height);

            using var textBrush = new SolidBrush(foreground);
            g.DrawString(caption, textFont, textBrush, textRect, textFormat);
        };

        surface.Click += onClick;
        surface.MouseEnter += (_, _) => surface.Invalidate();
        surface.MouseLeave += (_, _) => surface.Invalidate();
        ApplyRoundedRegion(surface, radius);
        return surface;
    }

    private static void SetButtonIconText(Button button, string glyph, string caption)
    {
        // Paint the icon and caption as one centered horizontal group.
        // Text is not delegated to the native Button renderer, avoiding clipping
        // and the left-aligned MDL2 glyph issue seen on different DPI settings.
        button.Text = string.Empty;
        button.Image = null;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 0;
        button.UseVisualStyleBackColor = false;
        button.Cursor = Cursors.Hand;
        button.Padding = Padding.Empty;
        button.Margin = Padding.Empty;
        button.MinimumSize = Size.Empty;
        button.AutoSize = false;
        button.TabStop = true;

        bool filled = glyph == "excel";
        int radius = glyph == "trophy" || glyph == "history" ? 9 : 8;
        Color borderColor = Color.FromArgb(0, 111, 235);
        Color fillColor = Color.FromArgb(16, 177, 91);
        Color foreground = filled ? Color.White : Color.FromArgb(0, 82, 174);
        float captionFontSize = glyph == "trophy" || glyph == "history" ? 10.5F : 9.5F;
        int iconSize = glyph == "trophy" || glyph == "history" ? 19 : 17;
        int gap = glyph == "trophy" || glyph == "history" ? 7 : 6;

        button.Paint += (_, e) =>
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            Rectangle face = new Rectangle(1, 1,
                Math.Max(1, button.ClientSize.Width - 3),
                Math.Max(1, button.ClientSize.Height - 3));

            using var facePath = RoundedPath(face, radius);
            using var background = new SolidBrush(filled ? fillColor : Color.White);
            g.FillPath(background, facePath);

            using (var border = new Pen(filled ? fillColor : borderColor, 1.15F))
                g.DrawPath(border, facePath);

            using var textFont = new Font("Segoe UI", captionFontSize, FontStyle.Bold);
            Size textSize = TextRenderer.MeasureText(
                g, caption, textFont, Size.Empty,
                TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);

            int totalWidth = iconSize + gap + textSize.Width;
            int x = Math.Max(3, (button.ClientSize.Width - totalWidth) / 2);
            int iconY = (button.ClientSize.Height - iconSize) / 2;
            Rectangle iconRect = new Rectangle(x, iconY, iconSize, iconSize);

            DrawStatisticsIcon(g, iconRect, glyph, foreground);

            Rectangle textRect = new Rectangle(
                x + iconSize + gap,
                0,
                Math.Max(1, textSize.Width + 3),
                button.ClientSize.Height);

            TextRenderer.DrawText(
                g, caption, textFont, textRect, foreground,
                TextFormatFlags.Left |
                TextFormatFlags.VerticalCenter |
                TextFormatFlags.NoPadding |
                TextFormatFlags.SingleLine);
        };

        ApplyRoundedRegion(button, radius);
    }

    private static void StyleOutlineButton(Button button)
    {
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 0; // Border is painted as one continuous rounded path.
        button.BackColor = Color.White;
        button.ForeColor = Color.FromArgb(0, 82, 174);
        button.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        button.Cursor = Cursors.Hand;
        button.UseVisualStyleBackColor = false;
    }

    private static void ApplyRoundedRegion(Control control, int radius)
    {
        void UpdateRegion(object? sender, EventArgs e)
        {
            if (control.Width < 2 || control.Height < 2) return;
            using var path = RoundedPath(new Rectangle(0, 0, control.Width, control.Height), radius);
            control.Region = new Region(path);
        }
        control.Resize += UpdateRegion;
        UpdateRegion(control, EventArgs.Empty);
    }

    private static GraphicsPath RoundedPath(Rectangle bounds, int radius)
    {
        int d = Math.Min(radius * 2, Math.Min(bounds.Width, bounds.Height));
        var path = new GraphicsPath();
        path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
        path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
        path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
        path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }

    private async Task LoadStatisticsAsync()
    {
        _statistics =
            await _missionPersonnelService
                .GetPersonnelMissionStatisticsAsync();

        lblTotalPersonnel.Text =
            $"Tổng CBCS: {_statistics.Count}";

        lblTotalMissionCount.Text =
            $"Tổng lượt công tác: {_statistics.Sum(x => x.SoLanCongTac)}";

        if (_personnelValueLabel != null) _personnelValueLabel.Text = _statistics.Count.ToString();
        if (_missionValueLabel != null) _missionValueLabel.Text = _statistics.Sum(x => x.SoLanCongTac).ToString();
        if (_footerLabel != null) _footerLabel.Text = $"Hiển thị: {_statistics.Count} CBCS     |     Tổng lượt công tác: {_statistics.Sum(x => x.SoLanCongTac)}";

        dgvStatistics.DataSource = null;
        dgvStatistics.DataSource = _statistics;

        for (int i = 0; i < dgvStatistics.Rows.Count; i++)
        {
            dgvStatistics.Rows[i]
                .Cells["colSTT"]
                .Value = i + 1;
        }
        ApplyRowHighlights();
    }

    private void DgvStatistics_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
    {
        if (e.RowIndex != -1 || e.ColumnIndex < 0)
            return;

        Graphics? graphics = e.Graphics;
        if (graphics == null)
            return;

        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle bounds = e.CellBounds;
        bounds.Width -= 1;
        bounds.Height -= 1;
        bool first = e.ColumnIndex == 0;
        bool last = e.ColumnIndex == dgvStatistics.Columns.Count - 1;
        using var path = HeaderCellPath(bounds, 8, first, last);
        using var brush = new SolidBrush(HeaderBlue);
        graphics.FillPath(brush, path);

        using var separatorPen = new Pen(Color.FromArgb(205, 226, 248), 1F);
        if (!last)
            graphics.DrawLine(separatorPen, e.CellBounds.Right - 1, e.CellBounds.Top + 1,
                e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);

        string caption = Convert.ToString(e.FormattedValue) ?? string.Empty;
        Font font = e.CellStyle?.Font ?? dgvStatistics.Font;
        TextRenderer.DrawText(graphics, caption, font,
            Rectangle.Inflate(e.CellBounds, -3, 0), Color.White,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
            TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
        e.Handled = true;
    }

    private static GraphicsPath HeaderCellPath(Rectangle r, int radius, bool roundLeft, bool roundRight)
    {
        var path = new GraphicsPath();
        int d = Math.Min(radius * 2, Math.Min(r.Width, r.Height));
        if (roundLeft)
            path.AddArc(r.Left, r.Top, d, d, 180, 90);
        else
            path.AddLine(r.Left, r.Top, r.Left, r.Top);

        path.AddLine(r.Left + (roundLeft ? radius : 0), r.Top,
            r.Right - (roundRight ? radius : 0), r.Top);
        if (roundRight)
            path.AddArc(r.Right - d, r.Top, d, d, 270, 90);
        else
            path.AddLine(r.Right, r.Top, r.Right, r.Top);
        path.AddLine(r.Right, r.Bottom, r.Left, r.Bottom);
        path.CloseFigure();
        return path;
    }

    private void ApplyRowHighlights()
    {
        for (int i = 0; i < dgvStatistics.Rows.Count; i++)
        {
            dgvStatistics.Rows[i].DefaultCellStyle.BackColor =
                i == 0 ? Color.FromArgb(157, 218, 255) :
                i == 2 ? Color.FromArgb(255, 246, 220) : Color.White;
        }
    }

    private void CustomizeGrid()
    {
        dgvStatistics.AutoGenerateColumns = false;

        dgvStatistics.AllowUserToAddRows = false;
        dgvStatistics.AllowUserToDeleteRows = false;

        dgvStatistics.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect;

        dgvStatistics.MultiSelect = false;

        dgvStatistics.ReadOnly = true;

        dgvStatistics.RowHeadersVisible = false;

        dgvStatistics.AutoSizeColumnsMode =
            DataGridViewAutoSizeColumnsMode.Fill;

        dgvStatistics.Columns["colSTT"].FillWeight = 50;

        dgvStatistics.Columns["colSoLanCongTac"]
            .DefaultCellStyle.Alignment =
            DataGridViewContentAlignment.MiddleCenter;

        dgvStatistics.RowHeadersVisible = false;

        dgvStatistics.AllowUserToAddRows = false;

        dgvStatistics.AllowUserToDeleteRows = false;

        dgvStatistics.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect;

        dgvStatistics.MultiSelect = false;

        dgvStatistics.ReadOnly = true;

        dgvStatistics.AutoSizeColumnsMode =
            DataGridViewAutoSizeColumnsMode.Fill;

        dgvStatistics.ColumnHeadersHeight = 46;
        dgvStatistics.RowTemplate.Height = 46;
        dgvStatistics.Columns["colSTT"].FillWeight = 58;
        dgvStatistics.Columns["colSoHieu"].FillWeight = 82;
        dgvStatistics.Columns["colHoTen"].FillWeight = 160;
        dgvStatistics.Columns["colDonVi"].FillWeight = 138;
        dgvStatistics.Columns["colCapBac"].FillWeight = 165;
        dgvStatistics.Columns["colSoLanCongTac"].FillWeight = 170;
        foreach (DataGridViewColumn column in dgvStatistics.Columns)
        {
            column.SortMode = DataGridViewColumnSortMode.NotSortable;
            column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
        dgvStatistics.Columns["colSTT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dgvStatistics.Columns["colHoTen"].DefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        dgvStatistics.Columns["colSoHieu"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dgvStatistics.Columns["colDonVi"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dgvStatistics.Columns["colCapBac"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        dgvStatistics.Columns["colSoLanCongTac"]
            .DefaultCellStyle.Alignment =
            DataGridViewContentAlignment.MiddleCenter;
    }

    private async void
    FrmPersonnelMissionStatistics_Load(
        object sender,
        EventArgs e)
    {
        CustomizeGrid();

        await LoadStatisticsAsync();
    }

    private async void
    btnRefresh_Click(
        object sender,
        EventArgs e)
    {
        try
        {
            PermissionHelper.Require("Mission.View");
        }
        catch (UnauthorizedAccessException ex)
        {
            MessageBox.Show(
                ex.Message,
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            Close();
            return;
        }

        await LoadStatisticsAsync();
    }

    private void
    btnClose_Click(
        object sender,
        EventArgs e)
    {
        Close();
    }

    private void btnExportExcel_Click(
        object sender,
        EventArgs e)
    {
        SaveFileDialog saveFileDialog =
            new();

        saveFileDialog.Filter =
            "Excel File|*.xlsx";

        saveFileDialog.FileName =
            $"ThongKeCongTac_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

        if (saveFileDialog.ShowDialog()
            != DialogResult.OK)
            return;

        try
        {
            _exportService.Export(
                _statistics,
                saveFileDialog.FileName);

            FrmLoginMessage.ShowMessage(
                this,
                LoginMessageType.Success,
                "Thông báo",
                "Xuất Excel thành công.");
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Lỗi",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void txtSearch_TextChanged(
        object sender,
        EventArgs e)
    {
        string keyword =
            txtSearch.Text
                .Trim()
                .ToLower();

        var filtered =
            _statistics
                .Where(x =>
                    (x.HoTen ?? string.Empty).ToLower().Contains(keyword)
                    || (x.SoHieu ?? string.Empty).ToLower().Contains(keyword)
                    || (x.DonVi ?? string.Empty).ToLower().Contains(keyword))
                .ToList();

        dgvStatistics.DataSource = null;
        dgvStatistics.DataSource = filtered;

        for (int i = 0; i < dgvStatistics.Rows.Count; i++)
        {
            dgvStatistics.Rows[i]
                .Cells["colSTT"]
                .Value = i + 1;
        }
        UpdateVisibleCount();
    }

    private void UpdateVisibleCount()
    {
        if (_footerLabel != null)
            _footerLabel.Text = $"Hiển thị: {dgvStatistics.Rows.Count} CBCS     |     Tổng lượt công tác: {_statistics.Sum(x => x.SoLanCongTac)}";
    }

    private void btnTop10_Click(
        object sender,
        EventArgs e)
    {
        dgvStatistics.DataSource = null;

        dgvStatistics.DataSource =
            _statistics
                .OrderByDescending(x =>
                    x.SoLanCongTac)
                .Take(10)
                .ToList();
        UpdateVisibleCount();
    }

    private void btnHistory_Click(
        object sender,
        EventArgs e)
    {
        if (dgvStatistics.CurrentRow == null)
        {
            MessageBox.Show(
                "Vui lòng chọn CBCS!",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return;
        }

        var selected =
            dgvStatistics.CurrentRow
            .DataBoundItem
            as PersonnelMissionStatisticDto;

        if (selected == null)
            return;

        FrmPersonnelMissionHistory frm =
            new FrmPersonnelMissionHistory(
                selected.PersonnelId,
                selected.HoTen);

        frm.ShowDialog();
    }
}