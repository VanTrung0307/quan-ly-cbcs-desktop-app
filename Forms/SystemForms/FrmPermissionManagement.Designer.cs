namespace QuanLyCBCS.Forms.SystemForms
{
    partial class FrmPermissionManagement
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pnlRoot = new TableLayoutPanel();
            pnlBanner = new Panel();
            lblBannerIcon = new Label();
            lblBannerTitle = new Label();
            lblBannerSubtitle = new Label();
            pnlBody = new TableLayoutPanel();
            pnlAccountCard = new Panel();
            pnlAccountHeader = new Panel();
            lblAccountHeaderIcon = new Label();
            lblAccountHeader = new Label();
            lblRole = new Label();
            pnlAccountSelector = new Panel();
            lblAccountUserIcon = new Label();
            cboRole = new ComboBox();
            pnlAccountIllustration = new Panel();
            lblAccountHint = new Label();
            pnlPermissionCard = new Panel();
            tlpPermissionLayout = new TableLayoutPanel();
            pnlPermissionHeader = new Panel();
            lblPermissionHeaderIcon = new Label();
            lblPermissionHeader = new Label();
            btnUndo = new Button();
            btnRedo = new Button();
            pnlPermissionList = new Panel();
            clbPermission = new PermissionCheckedListBox();
            pnlFooter = new Panel();
            btnSave = new Button();
            btnRefresh = new Button();

            pnlRoot.SuspendLayout();
            pnlBanner.SuspendLayout();
            pnlBody.SuspendLayout();
            pnlAccountCard.SuspendLayout();
            pnlAccountHeader.SuspendLayout();
            pnlAccountSelector.SuspendLayout();
            pnlPermissionCard.SuspendLayout();
            tlpPermissionLayout.SuspendLayout();
            pnlPermissionHeader.SuspendLayout();
            pnlFooter.SuspendLayout();
            SuspendLayout();

            // pnlRoot
            pnlRoot.BackColor = Color.FromArgb(241, 247, 255);
            pnlRoot.ColumnCount = 1;
            pnlRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            pnlRoot.Controls.Add(pnlBanner, 0, 0);
            pnlRoot.Controls.Add(pnlBody, 0, 1);
            pnlRoot.Dock = DockStyle.Fill;
            pnlRoot.Padding = new Padding(8);
            pnlRoot.RowCount = 2;
            pnlRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 94F));
            pnlRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // pnlBanner: banner gradient, bo góc và họa tiết vẽ trực tiếp để sắc nét ở mọi DPI.
            pnlBanner.BackColor = Color.Transparent;
            pnlBanner.Controls.Add(lblBannerSubtitle);
            pnlBanner.Controls.Add(lblBannerTitle);
            pnlBanner.Controls.Add(lblBannerIcon);
            pnlBanner.Dock = DockStyle.Fill;
            pnlBanner.Margin = new Padding(0, 0, 0, 14);
            pnlBanner.Paint += (sender, e) =>
            {
                System.Drawing.Graphics g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                Rectangle bannerRect = new Rectangle(0, 0, pnlBanner.Width - 1, pnlBanner.Height - 1);
                using (System.Drawing.Drawing2D.GraphicsPath rounded = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    int radius = 13;
                    rounded.AddArc(bannerRect.Left, bannerRect.Top, radius * 2, radius * 2, 180, 90);
                    rounded.AddArc(bannerRect.Right - radius * 2, bannerRect.Top, radius * 2, radius * 2, 270, 90);
                    rounded.AddArc(bannerRect.Right - radius * 2, bannerRect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
                    rounded.AddArc(bannerRect.Left, bannerRect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
                    rounded.CloseFigure();
                    using (System.Drawing.Drawing2D.LinearGradientBrush gradient = new System.Drawing.Drawing2D.LinearGradientBrush(
                        bannerRect, Color.FromArgb(8, 111, 204), Color.FromArgb(0, 76, 151), 0F))
                    {
                        g.FillPath(gradient, rounded);
                    }
                }

                // Biểu tượng khiên + dấu kiểm + bánh răng: tăng độ dày nét
                // và kích thước để gần với bản thiết kế, vẫn giữ nét vector sắc ở DPI cao.
                using (Pen shieldPen = new Pen(Color.White, 3.8F))
                using (Pen checkPen = new Pen(Color.White, 4.2F))
                using (Pen gearPen = new Pen(Color.White, 3.0F))
                using (SolidBrush gearCenterBrush = new SolidBrush(Color.FromArgb(8, 111, 204)))
                {
                    shieldPen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                    shieldPen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                    shieldPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                    checkPen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                    checkPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                    gearPen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                    gearPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;

                    Point[] shield = {
                        new Point(42, 21), new Point(52, 26), new Point(63, 21),
                        new Point(70, 25), new Point(69, 42), new Point(64, 55),
                        new Point(52, 64), new Point(40, 55), new Point(35, 42),
                        new Point(34, 25)
                    };
                    g.DrawPolygon(shieldPen, shield);

                    // Dấu kiểm lớn, nét dày và đầu nét bo tròn.
                    g.DrawLines(checkPen, new Point[] {
                        new Point(42, 42), new Point(49, 49), new Point(62, 34)
                    });

                    // Bánh răng đặt chồng nhẹ ở góc dưới bên phải của khiên.
                    const float gearCx = 72.5F;
                    const float gearCy = 51.5F;
                    const float outerRadius = 11.5F;
                    const float innerRadius = 8.0F;
                    PointF[] gearTeeth = new PointF[16];
                    for (int i = 0; i < gearTeeth.Length; i++)
                    {
                        double angle = -Math.PI / 2 + i * Math.PI / 8.0;
                        float radius = (i % 2 == 0) ? outerRadius : innerRadius;
                        gearTeeth[i] = new PointF(
                            gearCx + (float)Math.Cos(angle) * radius,
                            gearCy + (float)Math.Sin(angle) * radius);
                    }
                    g.DrawPolygon(gearPen, gearTeeth);
                    g.FillEllipse(gearCenterBrush, gearCx - 3.0F, gearCy - 3.0F, 6.0F, 6.0F);
                    g.DrawEllipse(gearPen, gearCx - 3.0F, gearCy - 3.0F, 6.0F, 6.0F);
                }

                // Họa tiết ngôi sao và vòng nguyệt quế mờ ở phía bên phải.
                int cx = pnlBanner.Width - 82;
                int cy = pnlBanner.Height / 2;
                using (SolidBrush watermark = new SolidBrush(Color.FromArgb(48, 225, 244, 255)))
                using (Pen wreathPen = new Pen(Color.FromArgb(58, 225, 244, 255), 2.0F))
                {
                    PointF[] star = new PointF[10];
                    for (int i = 0; i < star.Length; i++)
                    {
                        double a = -Math.PI / 2 + i * Math.PI / 5;
                        float r = i % 2 == 0 ? 22F : 9F;
                        star[i] = new PointF(cx + (float)Math.Cos(a) * r, cy + (float)Math.Sin(a) * r);
                    }
                    g.FillPolygon(watermark, star);
                    g.DrawBezier(wreathPen, cx - 24, cy + 27, cx - 56, cy + 10, cx - 53, cy - 22, cx - 34, cy - 34);
                    g.DrawBezier(wreathPen, cx + 24, cy + 27, cx + 56, cy + 10, cx + 53, cy - 22, cx + 34, cy - 34);
                    for (int i = 0; i < 5; i++)
                    {
                        float y = cy - 24 + i * 11;
                        g.FillEllipse(watermark, cx - 48 + i * 3, y, 12, 5);
                        g.FillEllipse(watermark, cx + 36 - i * 3, y, 12, 5);
                    }
                }
            };

            // lblBannerIcon: phần biểu tượng được vẽ vector trong sự kiện Paint của banner.
            lblBannerIcon.Dock = DockStyle.Left;
            lblBannerIcon.BackColor = Color.Transparent;
            lblBannerIcon.Width = 94;
            lblBannerIcon.Text = string.Empty;

            // lblBannerTitle
            lblBannerTitle.AutoSize = true;
            lblBannerTitle.BackColor = Color.Transparent;
            lblBannerTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblBannerTitle.ForeColor = Color.White;
            lblBannerTitle.Location = new Point(102, 10);
            lblBannerTitle.Text = "PHÂN QUYỀN";

            // lblBannerSubtitle
            lblBannerSubtitle.AutoSize = true;
            lblBannerSubtitle.BackColor = Color.Transparent;
            lblBannerSubtitle.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular);
            lblBannerSubtitle.ForeColor = Color.FromArgb(242, 248, 255);
            lblBannerSubtitle.Location = new Point(104, 49);
            lblBannerSubtitle.Text = "Thiết lập và quản lý quyền truy cập, chức năng của người dùng";

            // pnlBody
            pnlBody.BackColor = Color.Transparent;
            pnlBody.ColumnCount = 2;
            pnlBody.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 29F));
            pnlBody.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 71F));
            pnlBody.Controls.Add(pnlAccountCard, 0, 0);
            pnlBody.Controls.Add(pnlPermissionCard, 1, 0);
            pnlBody.Dock = DockStyle.Fill;
            pnlBody.RowCount = 1;
            pnlBody.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            pnlBody.Padding = new Padding(2, 2, 2, 0);

            // pnlAccountCard
            pnlAccountCard.BackColor = Color.FromArgb(246, 250, 255);
            pnlAccountCard.BorderStyle = BorderStyle.None;
            pnlAccountCard.Paint += (sender, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                Rectangle r = new Rectangle(0, 0, pnlAccountCard.Width - 1, pnlAccountCard.Height - 1);
                using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    int radius = 12;
                    path.AddArc(r.Left, r.Top, radius * 2, radius * 2, 180, 90);
                    path.AddArc(r.Right - radius * 2, r.Top, radius * 2, radius * 2, 270, 90);
                    path.AddArc(r.Right - radius * 2, r.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
                    path.AddArc(r.Left, r.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
                    path.CloseFigure();
                    using (var fill = new System.Drawing.Drawing2D.LinearGradientBrush(
                        r, Color.FromArgb(250, 252, 255), Color.FromArgb(231, 242, 255), 90F))
                        e.Graphics.FillPath(fill, path);

                    // Hai dải sóng trang trí chạy hết chiều rộng đáy thẻ,
                    // thay vì chỉ nằm trong panel minh họa hẹp ở giữa.
                    var state = e.Graphics.Save();
                    e.Graphics.SetClip(path);
                    int cardW = pnlAccountCard.ClientSize.Width;
                    int cardH = pnlAccountCard.ClientSize.Height;
                    using (var wave1 = new SolidBrush(Color.FromArgb(38, 112, 177, 247)))
                    using (var wave2 = new SolidBrush(Color.FromArgb(25, 91, 161, 235)))
                    {
                        using (var wavePath1 = new System.Drawing.Drawing2D.GraphicsPath())
                        {
                            wavePath1.AddBezier(0, cardH - 92, cardW * 0.22F, cardH - 145, cardW * 0.43F, cardH - 45, cardW * 0.66F, cardH - 88);
                            wavePath1.AddBezier(cardW * 0.66F, cardH - 88, cardW * 0.82F, cardH - 118, cardW * 0.92F, cardH - 42, cardW, cardH - 72);
                            wavePath1.AddLine(cardW, cardH, 0, cardH);
                            wavePath1.CloseFigure();
                            e.Graphics.FillPath(wave1, wavePath1);
                        }
                        using (var wavePath2 = new System.Drawing.Drawing2D.GraphicsPath())
                        {
                            wavePath2.AddBezier(0, cardH - 46, cardW * 0.27F, cardH - 91, cardW * 0.56F, cardH - 15, cardW, cardH - 54);
                            wavePath2.AddLine(cardW, cardH, 0, cardH);
                            wavePath2.CloseFigure();
                            e.Graphics.FillPath(wave2, wavePath2);
                        }
                    }
                    e.Graphics.Restore(state);

                    using (var border = new Pen(Color.FromArgb(222, 234, 249), 1F))
                        e.Graphics.DrawPath(border, path);
                }
            };
            pnlAccountCard.Controls.Add(pnlAccountIllustration);
            pnlAccountCard.Controls.Add(lblAccountHint);
            pnlAccountCard.Controls.Add(pnlAccountSelector);
            pnlAccountCard.Controls.Add(lblRole);
            pnlAccountCard.Controls.Add(pnlAccountHeader);
            pnlAccountCard.Dock = DockStyle.Fill;
            pnlAccountCard.Margin = new Padding(0, 0, 12, 0);
            pnlAccountCard.Padding = new Padding(6);
            pnlAccountCard.Resize += (sender, e) =>
            {
                int cardWidth = pnlAccountCard.ClientSize.Width;

                // Header phủ sát mép trong của thẻ; các nội dung còn lại vẫn giữ padding 6 px.
                pnlAccountHeader.Dock = DockStyle.None;
                pnlAccountHeader.Location = new Point(1, 1);
                pnlAccountHeader.Size = new Size(Math.Max(0, cardWidth - 2), 46);
                pnlAccountHeader.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

                // Ô chọn tài khoản có chiều rộng giới hạn, không kéo dài hết thẻ.
                int selectorWidth = Math.Min(270, Math.Max(190, cardWidth - 36));
                pnlAccountSelector.Width = selectorWidth;
                pnlAccountSelector.Left = Math.Max(8, (cardWidth - selectorWidth) / 2);
                lblRole.Left = pnlAccountSelector.Left;

                // Minh họa được căn giữa theo chính chiều rộng thẻ.
                int illustrationWidth = Math.Min(340, Math.Max(250, cardWidth - 20));
                pnlAccountIllustration.Width = illustrationWidth;
                pnlAccountIllustration.Left = Math.Max(0, (cardWidth - illustrationWidth) / 2);
            };

            // pnlAccountHeader
            pnlAccountHeader.BackColor = Color.FromArgb(218, 234, 255);
            pnlAccountHeader.Paint += (sender, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                Rectangle r = new Rectangle(0, 0, pnlAccountHeader.Width - 1, pnlAccountHeader.Height - 1);
                using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    int radius = 10;
                    path.AddArc(r.Left, r.Top, radius * 2, radius * 2, 180, 90);
                    path.AddArc(r.Right - radius * 2, r.Top, radius * 2, radius * 2, 270, 90);
                    path.AddArc(r.Right - radius * 2, r.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
                    path.AddArc(r.Left, r.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
                    path.CloseFigure();
                    using (var b = new System.Drawing.Drawing2D.LinearGradientBrush(
                        r, Color.FromArgb(220, 235, 255), Color.FromArgb(211, 229, 252), 90F))
                        e.Graphics.FillPath(b, path);

                    // Clip cả nền và các control con theo đúng đường bo góc.
                    pnlAccountHeader.Region?.Dispose();
                    pnlAccountHeader.Region = new System.Drawing.Region(path);
                }
            };
            pnlAccountHeader.Location = new Point(1, 1);
            pnlAccountHeader.Size = new Size(Math.Max(0, pnlAccountCard.ClientSize.Width - 2), 46);
            pnlAccountHeader.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlAccountHeader.Controls.Add(lblAccountHeader);
            pnlAccountHeader.Controls.Add(lblAccountHeaderIcon);
            pnlAccountHeader.Dock = DockStyle.None;
            pnlAccountHeader.Height = 46;

            // lblAccountHeaderIcon
            lblAccountHeaderIcon.Dock = DockStyle.Left;
            lblAccountHeaderIcon.Font = new Font("Segoe MDL2 Assets", 19F, FontStyle.Regular);
            lblAccountHeaderIcon.ForeColor = Color.FromArgb(0, 91, 181);
            lblAccountHeaderIcon.Text = "\uE716";
            lblAccountHeaderIcon.TextAlign = ContentAlignment.MiddleCenter;
            lblAccountHeaderIcon.Width = 48;

            // lblAccountHeader
            lblAccountHeader.Dock = DockStyle.Fill;
            lblAccountHeader.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblAccountHeader.ForeColor = Color.FromArgb(15, 43, 78);
            lblAccountHeader.Text = "CHỌN TÀI KHOẢN";
            lblAccountHeader.TextAlign = ContentAlignment.MiddleLeft;

            // lblRole
            lblRole.AutoSize = true;
            lblRole.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblRole.ForeColor = Color.FromArgb(25, 48, 78);
            lblRole.Location = new Point(18, 68);
            lblRole.Text = "Tài khoản:";

            // pnlAccountSelector: hộp chọn tài khoản có biểu tượng người dùng.
            pnlAccountSelector.Anchor = AnchorStyles.Top;
            pnlAccountSelector.BackColor = Color.White;
            pnlAccountSelector.Location = new Point(0, 96); // căn giữa trong pnlAccountCard ở sự kiện Resize
            pnlAccountSelector.Name = "pnlAccountSelector";
            pnlAccountSelector.Size = new Size(270, 34);
            pnlAccountSelector.Padding = new Padding(5, 2, 5, 2);
            pnlAccountSelector.Paint += (sender, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                Rectangle r = new Rectangle(0, 0, pnlAccountSelector.Width - 1, pnlAccountSelector.Height - 1);
                using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    int radius = 6;
                    path.AddArc(r.Left, r.Top, radius * 2, radius * 2, 180, 90);
                    path.AddArc(r.Right - radius * 2, r.Top, radius * 2, radius * 2, 270, 90);
                    path.AddArc(r.Right - radius * 2, r.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
                    path.AddArc(r.Left, r.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
                    path.CloseFigure();
                    using (var pen = new Pen(Color.FromArgb(194, 208, 226), 1F))
                        e.Graphics.DrawPath(pen, path);
                }
            };

            // lblAccountUserIcon
            lblAccountUserIcon.Dock = DockStyle.Left;
            lblAccountUserIcon.Width = 30;
            lblAccountUserIcon.Font = new Font("Segoe MDL2 Assets", 13F, FontStyle.Regular);
            lblAccountUserIcon.ForeColor = Color.FromArgb(31, 67, 108);
            lblAccountUserIcon.Text = "\uE77B";
            lblAccountUserIcon.TextAlign = ContentAlignment.MiddleCenter;

            // cboRole
            cboRole.Dock = DockStyle.Fill;
            cboRole.BackColor = Color.White;
            // ComboBox chỉ là nguồn dữ liệu ẩn; popup tìm kiếm được dựng tùy chỉnh trong code-behind.
            cboRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cboRole.AutoCompleteMode = AutoCompleteMode.None;
            cboRole.AutoCompleteSource = AutoCompleteSource.None;
            cboRole.Visible = false;
            cboRole.MaxDropDownItems = 8;
            cboRole.IntegralHeight = true;
            cboRole.FlatStyle = FlatStyle.Flat;
            cboRole.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            cboRole.ForeColor = Color.FromArgb(31, 52, 79);
            cboRole.FormattingEnabled = true;
            cboRole.Margin = new Padding(0);
            cboRole.Name = "cboRole";
            cboRole.TabIndex = 1;

            pnlAccountSelector.Controls.Add(cboRole);
            pnlAccountSelector.Controls.Add(lblAccountUserIcon);

            // pnlAccountIllustration: minh họa vector khiên bảo mật, ổ khóa,
            // bánh răng, bảng danh sách quyền và nền sóng theo bản thiết kế.
            pnlAccountIllustration.Anchor = AnchorStyles.Top;
            pnlAccountIllustration.BackColor = Color.Transparent;
            pnlAccountIllustration.Location = new Point(0, 158); // căn giữa theo chiều ngang trong pnlAccountCard
            pnlAccountIllustration.Name = "pnlAccountIllustration";
            pnlAccountIllustration.Size = new Size(340, 260);
            pnlAccountIllustration.Paint += (sender, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                int w = pnlAccountIllustration.ClientSize.Width;
                int h = pnlAccountIllustration.ClientSize.Height;

                // Quầng sáng trung tâm.
                Rectangle glow = new Rectangle(w / 2 - 104, 28, 208, 208);
                using (var gp = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    gp.AddEllipse(glow);
                    using (var radial = new System.Drawing.Drawing2D.PathGradientBrush(gp))
                    {
                        radial.CenterColor = Color.FromArgb(230, 255, 255, 255);
                        radial.SurroundColors = new[] { Color.FromArgb(0, 108, 177, 255) };
                        g.FillEllipse(radial, glow);
                    }
                }

                // Khiên xanh chuyển sắc.
                PointF[] shield = {
                    new PointF(w/2, 49), new PointF(w/2 + 57, 70),
                    new PointF(w/2 + 54, 134), new PointF(w/2 + 39, 154),
                    new PointF(w/2, 174), new PointF(w/2 - 39, 154),
                    new PointF(w/2 - 54, 134), new PointF(w/2 - 57, 70)
                };
                using (var shieldPath = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    shieldPath.AddPolygon(shield);
                    using (var shieldBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
                        new Rectangle(w / 2 - 58, 49, 116, 126),
                        Color.FromArgb(67, 166, 255), Color.FromArgb(0, 91, 202), 90F))
                        g.FillPath(shieldBrush, shieldPath);
                    using (var shieldPen = new Pen(Color.FromArgb(103, 188, 255), 2.2F))
                        g.DrawPath(shieldPen, shieldPath);
                }

                // Ổ khóa trắng nằm trên khiên.
                using (var whitePen = new Pen(Color.White, 8F))
                using (var whiteBrush = new SolidBrush(Color.White))
                using (var blueBrush = new SolidBrush(Color.FromArgb(43, 132, 226)))
                {
                    whitePen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                    whitePen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                    g.DrawArc(whitePen, w / 2 - 20, 88, 40, 43, 180, 180);
                    g.FillRectangle(whiteBrush, w / 2 - 28, 113, 56, 51);
                    g.FillEllipse(blueBrush, w / 2 - 4, 129, 8, 8);
                    g.FillRectangle(blueBrush, w / 2 - 2, 135, 4, 10);
                }

                // Bảng danh sách quyền ở phía dưới.
                Rectangle board = new Rectangle(w / 2 - 68, 145, 136, 91);
                using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    int radius = 9;
                    path.AddArc(board.Left, board.Top, radius * 2, radius * 2, 180, 90);
                    path.AddArc(board.Right - radius * 2, board.Top, radius * 2, radius * 2, 270, 90);
                    path.AddArc(board.Right - radius * 2, board.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
                    path.AddArc(board.Left, board.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
                    path.CloseFigure();
                    using (var shadow = new SolidBrush(Color.FromArgb(30, 25, 95, 170)))
                        g.FillPath(shadow, path);
                    using (var fill = new SolidBrush(Color.White))
                        g.FillPath(fill, path);
                    using (var border = new Pen(Color.FromArgb(139, 190, 246), 2F))
                        g.DrawPath(border, path);
                }
                for (int i = 0; i < 3; i++)
                {
                    int yy = 160 + i * 23;
                    using (var checkBrush = new SolidBrush(Color.FromArgb(67, 158, 244)))
                        g.FillEllipse(checkBrush, w / 2 - 55, yy, 18, 18);
                    using (var checkPen = new Pen(Color.White, 2.3F))
                    {
                        checkPen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                        checkPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                        g.DrawLines(checkPen, new[] {
                            new Point(w/2 - 51, yy + 9), new Point(w/2 - 47, yy + 13),
                            new Point(w/2 - 40, yy + 5)
                        });
                    }
                    using (var lineBrush = new SolidBrush(Color.FromArgb(191, 218, 248)))
                    {
                        g.FillRectangle(lineBrush, w / 2 - 29, yy + 4, 72, 5);
                        g.FillRectangle(lineBrush, w / 2 - 29, yy + 12, 50, 3);
                    }
                }

                // Bánh răng xanh đặt chồng lên góc phải bảng.
                float gx = w / 2 + 64, gy = 151;
                PointF[] teeth = new PointF[24];
                for (int i = 0; i < teeth.Length; i++)
                {
                    double a = -Math.PI / 2 + i * Math.PI / 12;
                    float rr = (i % 2 == 0) ? 28F : 22F;
                    teeth[i] = new PointF(gx + (float)Math.Cos(a) * rr, gy + (float)Math.Sin(a) * rr);
                }
                using (var gearBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
                    new Rectangle((int)gx - 29, (int)gy - 29, 58, 58),
                    Color.FromArgb(112, 194, 255), Color.FromArgb(0, 104, 211), 45F))
                using (var gearPen = new Pen(Color.FromArgb(106, 178, 245), 2F))
                using (var holeBrush = new SolidBrush(Color.FromArgb(240, 248, 255)))
                {
                    g.FillPolygon(gearBrush, teeth);
                    g.DrawPolygon(gearPen, teeth);
                    g.FillEllipse(holeBrush, gx - 10, gy - 10, 20, 20);
                }

            };

            // lblAccountHint
            lblAccountHint.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblAccountHint.BackColor = Color.Transparent;
            lblAccountHint.Font = new Font("Segoe UI", 9.5F, FontStyle.Italic);
            lblAccountHint.ForeColor = Color.FromArgb(104, 128, 158);
            lblAccountHint.Location = new Point(18, 540);
            lblAccountHint.Size = new Size(320, 44);
            lblAccountHint.Text = "Chọn tài khoản để xem và thiết lập\r\ncác quyền truy cập tương ứng.";
            lblAccountHint.TextAlign = ContentAlignment.MiddleCenter;
            lblAccountHint.Visible = false;

            // pnlPermissionCard
            pnlPermissionCard.BackColor = Color.FromArgb(247, 250, 255);
            pnlPermissionCard.BorderStyle = BorderStyle.None;
            pnlPermissionCard.Paint += (sender, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                Rectangle r = new Rectangle(0, 0, pnlPermissionCard.Width - 1, pnlPermissionCard.Height - 1);
                using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    int radius = 14;
                    path.AddArc(r.Left, r.Top, radius * 2, radius * 2, 180, 90);
                    path.AddArc(r.Right - radius * 2, r.Top, radius * 2, radius * 2, 270, 90);
                    path.AddArc(r.Right - radius * 2, r.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
                    path.AddArc(r.Left, r.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
                    path.CloseFigure();
                    using (var brush = new SolidBrush(Color.FromArgb(247, 250, 255))) e.Graphics.FillPath(brush, path);
                    using (var pen = new Pen(Color.FromArgb(211, 226, 246), 1F)) e.Graphics.DrawPath(pen, path);
                }
            };
            pnlPermissionCard.Controls.Add(tlpPermissionLayout);
            pnlPermissionCard.Dock = DockStyle.Fill;
            pnlPermissionCard.Margin = new Padding(0);
            pnlPermissionCard.Padding = new Padding(12);

            // tlpPermissionLayout: chia cố định Header / vùng danh sách / Footer.
            // Cách bố trí này bảo đảm danh sách không đè lên các nút phía dưới.
            tlpPermissionLayout.BackColor = Color.FromArgb(247, 250, 255);
            tlpPermissionLayout.ColumnCount = 1;
            tlpPermissionLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpPermissionLayout.RowCount = 3;
            tlpPermissionLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            tlpPermissionLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpPermissionLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));
            tlpPermissionLayout.Controls.Add(pnlPermissionHeader, 0, 0);
            tlpPermissionLayout.Controls.Add(pnlPermissionList, 0, 1);
            tlpPermissionLayout.Controls.Add(pnlFooter, 0, 2);
            tlpPermissionLayout.Dock = DockStyle.Fill;
            tlpPermissionLayout.Margin = new Padding(0);
            tlpPermissionLayout.Padding = new Padding(0);

            // pnlPermissionHeader
            pnlPermissionHeader.BackColor = Color.Transparent;
            pnlPermissionHeader.Paint += (sender, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                Rectangle r = new Rectangle(0, 0, pnlPermissionHeader.Width - 1, pnlPermissionHeader.Height - 1);
                using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    int radius = 12;
                    path.AddArc(r.Left, r.Top, radius * 2, radius * 2, 180, 90);
                    path.AddArc(r.Right - radius * 2, r.Top, radius * 2, radius * 2, 270, 90);
                    path.AddArc(r.Right - radius * 2, r.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
                    path.AddArc(r.Left, r.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
                    path.CloseFigure();
                    using (var brush = new SolidBrush(Color.FromArgb(220, 235, 255))) e.Graphics.FillPath(brush, path);
                }
            };
            pnlPermissionHeader.Controls.Add(btnRedo);
            pnlPermissionHeader.Controls.Add(btnUndo);
            pnlPermissionHeader.Controls.Add(lblPermissionHeader);
            pnlPermissionHeader.Controls.Add(lblPermissionHeaderIcon);
            pnlPermissionHeader.Dock = DockStyle.Fill;
            pnlPermissionHeader.Margin = new Padding(0);
            pnlPermissionHeader.Height = 52;
            pnlPermissionHeader.Resize += pnlPermissionHeader_Resize;

            // lblPermissionHeaderIcon
            lblPermissionHeaderIcon.Dock = DockStyle.Left;
            lblPermissionHeaderIcon.Font = new Font("Segoe UI Symbol", 23F, FontStyle.Bold);
            lblPermissionHeaderIcon.ForeColor = Color.FromArgb(0, 91, 181);
            lblPermissionHeaderIcon.Text = string.Empty;
            lblPermissionHeaderIcon.TextAlign = ContentAlignment.MiddleCenter;
            lblPermissionHeaderIcon.Width = 52;

            // lblPermissionHeader
            lblPermissionHeader.Dock = DockStyle.None;
            lblPermissionHeader.Location = new Point(54, 0);
            lblPermissionHeader.Size = new Size(430, 52);
            lblPermissionHeader.Font = new Font("Segoe UI", 15.5F, FontStyle.Bold);
            lblPermissionHeader.ForeColor = Color.FromArgb(15, 43, 78);
            lblPermissionHeader.Text = "DANH SÁCH QUYỀN";
            lblPermissionHeader.TextAlign = ContentAlignment.MiddleLeft;

            // btnUndo
            btnUndo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnUndo.BackColor = Color.White;
            btnUndo.FlatAppearance.BorderColor = Color.FromArgb(90, 150, 215);
            btnUndo.FlatAppearance.BorderSize = 0;
            btnUndo.FlatAppearance.BorderColor = Color.FromArgb(90, 150, 215);
            btnUndo.FlatStyle = FlatStyle.Flat;
            btnUndo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnUndo.ForeColor = Color.FromArgb(0, 91, 181);
            btnUndo.Location = new Point(650, 8);
            btnUndo.Name = "btnUndo";
            btnUndo.Size = new Size(96, 36);
            btnUndo.Text = "↶  Undo";
            btnUndo.UseVisualStyleBackColor = false;
            btnUndo.Click += btnUndo_Click;

            // btnRedo
            btnRedo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRedo.BackColor = Color.White;
            btnRedo.FlatAppearance.BorderColor = Color.FromArgb(90, 150, 215);
            btnRedo.FlatAppearance.BorderSize = 0;
            btnRedo.FlatAppearance.BorderColor = Color.FromArgb(145, 165, 190);
            btnRedo.FlatStyle = FlatStyle.Flat;
            btnRedo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnRedo.ForeColor = Color.FromArgb(45, 65, 90);
            btnRedo.Location = new Point(756, 8);
            btnRedo.Name = "btnRedo";
            btnRedo.Size = new Size(96, 36);
            btnRedo.Text = "↷  Redo";
            btnRedo.UseVisualStyleBackColor = false;
            btnRedo.Click += btnRedo_Click;

            // pnlPermissionList: khung danh sách bo góc, viền xanh xám mảnh.
            pnlPermissionList.BackColor = Color.White;
            pnlPermissionList.Dock = DockStyle.Fill;
            pnlPermissionList.Margin = new Padding(10, 8, 10, 8);
            pnlPermissionList.Padding = new Padding(1, 17, 1, 1); // Viền 1 px + đệm trong 16 px phía trên.
            pnlPermissionList.Paint += (sender, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                Rectangle r = new Rectangle(0, 0, pnlPermissionList.Width - 1, pnlPermissionList.Height - 1);
                using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    int radius = 9;
                    path.AddArc(r.Left, r.Top, radius * 2, radius * 2, 180, 90);
                    path.AddArc(r.Right - radius * 2, r.Top, radius * 2, radius * 2, 270, 90);
                    path.AddArc(r.Right - radius * 2, r.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
                    path.AddArc(r.Left, r.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
                    path.CloseFigure();
                    using (var pen = new Pen(Color.FromArgb(213, 225, 239), 1F)) e.Graphics.DrawPath(pen, path);
                }
            };
            pnlPermissionList.Controls.Add(clbPermission);

            // clbPermission
            clbPermission.Dock = DockStyle.Fill;
            clbPermission.Margin = Padding.Empty;
            clbPermission.BackColor = Color.White;
            clbPermission.BorderStyle = BorderStyle.None;
            clbPermission.CheckOnClick = true;
            clbPermission.DrawMode = DrawMode.OwnerDrawFixed;
            clbPermission.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular);
            clbPermission.ForeColor = Color.FromArgb(28, 48, 74);
            clbPermission.FormattingEnabled = true;
            clbPermission.IntegralHeight = false;
            clbPermission.ItemHeight = 30;
            clbPermission.Location = Point.Empty;
            clbPermission.Name = "clbPermission";
            clbPermission.Size = new Size(875, 420);
            clbPermission.TabIndex = 2;
            clbPermission.ItemCheck += clbPermission_ItemCheck;
            clbPermission.DrawItem += clbPermission_DrawItem;

            // pnlFooter
            pnlFooter.BackColor = Color.Transparent;
            pnlFooter.Paint += (sender, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                Rectangle r = new Rectangle(0, 0, pnlFooter.Width - 1, pnlFooter.Height - 1);
                using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    int radius = 11;
                    path.AddArc(r.Left, r.Top, radius * 2, radius * 2, 180, 90);
                    path.AddArc(r.Right - radius * 2, r.Top, radius * 2, radius * 2, 270, 90);
                    path.AddArc(r.Right - radius * 2, r.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
                    path.AddArc(r.Left, r.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
                    path.CloseFigure();
                    using (var brush = new SolidBrush(Color.FromArgb(231, 241, 255))) e.Graphics.FillPath(brush, path);
                }
            };
            pnlFooter.Controls.Add(btnRefresh);
            pnlFooter.Controls.Add(btnSave);
            pnlFooter.Dock = DockStyle.Fill;
            pnlFooter.Margin = new Padding(0, 10, 0, 0);
            pnlFooter.Height = 66;
            pnlFooter.Padding = new Padding(10, 4, 10, 4);
            pnlFooter.Resize += pnlFooter_Resize;

            // btnSave
            btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSave.BackColor = Color.FromArgb(220, 224, 230);
            btnSave.Enabled = false;
            btnSave.FlatAppearance.BorderColor = Color.FromArgb(190, 199, 210);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(0, 10);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(124, 40);
            btnSave.TabIndex = 3;
            btnSave.Text = "💾  Lưu";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;

            // btnRefresh
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.BackColor = Color.White;
            btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(0, 91, 181);
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnRefresh.ForeColor = Color.FromArgb(0, 91, 181);
            btnRefresh.Location = new Point(0, 10);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(124, 40);
            btnRefresh.TabIndex = 4;
            btnRefresh.Text = "⟳  Làm mới";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;


            // FrmPermissionManagement
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(241, 247, 255);
            ClientSize = new Size(1380, 740);
            Controls.Add(pnlRoot);
            Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(0);
            MinimumSize = new Size(1050, 620);
            Name = "FrmPermissionManagement";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Quản lý phân quyền";
            Load += FrmPermissionManagement_Load;

            pnlRoot.ResumeLayout(false);
            pnlBanner.ResumeLayout(false);
            pnlBanner.PerformLayout();
            pnlBody.ResumeLayout(false);
            pnlAccountCard.ResumeLayout(false);
            pnlAccountCard.PerformLayout();
            pnlAccountHeader.ResumeLayout(false);
            pnlAccountSelector.ResumeLayout(false);
            pnlPermissionCard.ResumeLayout(false);
            tlpPermissionLayout.ResumeLayout(false);
            pnlPermissionHeader.ResumeLayout(false);
            pnlPermissionList.ResumeLayout(false);
            pnlFooter.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel pnlRoot;
        private Panel pnlBanner;
        private Label lblBannerIcon;
        private Label lblBannerTitle;
        private Label lblBannerSubtitle;
        private TableLayoutPanel pnlBody;
        private Panel pnlAccountCard;
        private Panel pnlAccountHeader;
        private Label lblAccountHeaderIcon;
        private Label lblAccountHeader;
        private Label lblRole;
        private Panel pnlAccountSelector;
        private Label lblAccountUserIcon;
        private ComboBox cboRole;
        private Panel pnlAccountIllustration;
        private Label lblAccountHint;
        private Panel pnlPermissionCard;
        private TableLayoutPanel tlpPermissionLayout;
        private Panel pnlPermissionHeader;
        private Label lblPermissionHeaderIcon;
        private Label lblPermissionHeader;
        private Panel pnlPermissionList;
        private PermissionCheckedListBox clbPermission;
        private Panel pnlFooter;
        private Button btnSave;
        private Button btnRefresh;
        private Button btnRedo;
        private Button btnUndo;
    }
}