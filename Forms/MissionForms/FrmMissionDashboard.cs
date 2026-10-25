using QuanLyCBCS.Services;
using QuanLyCBCS.DTOs;

namespace QuanLyCBCS.Forms.MissionForms
{
    public partial class FrmMissionDashboard : Form
    {
        private readonly MissionService _missionService = new();
        private Label lblRecentEmptyIcon = null!;
        private Label lblRecentEmptyDetail = null!;

        public FrmMissionDashboard()
        {
            InitializeComponent();
            CreateRecentEmptyState();
            ApplyDashboardStyles();
            LayoutCompactDashboard();
            Resize += (_, _) => LayoutCompactDashboard();
        }

        private async void FrmMissionDashboard_Load(object sender, EventArgs e)
        {
            await LoadDashboardAsync();
        }

        private async Task LoadDashboardAsync()
        {
            try
            {
                btnRefresh.Enabled = false;
                var dashboard = await _missionService.GetDashboardAsync();

                lblTongCongTac.Text = dashboard.TongCongTac.ToString();
                lblDangHoatDong.Text = dashboard.DangHoatDong.ToString();
                lblDaKetThuc.Text = dashboard.DaKetThuc.ToString();

                lblTopPersonnel.Text = string.IsNullOrWhiteSpace(dashboard.TopPersonnelName)
                    ? "Chưa có dữ liệu"
                    : dashboard.TopPersonnelName;
                lblTopPersonnelCount.Text = $"{dashboard.TopPersonnelCount} lần";

                await LoadRecentMissionAsync();
                await LoadTopPersonnelAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Không thể tải dữ liệu bảng điều khiển công tác.\nChi tiết: {ex.Message}",
                    "Lỗi tải dữ liệu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnRefresh.Enabled = true;
            }
        }

        private void CreateRecentEmptyState()
        {
            lblRecentEmptyIcon = new Label
            {
                Name = "lblRecentEmptyIcon",
                Text = "▤",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI Symbol", 28F, FontStyle.Bold),
                ForeColor = Color.FromArgb(119, 166, 242),
                BackColor = Color.FromArgb(232, 240, 255),
                Visible = false
            };
            lblRecentEmptyDetail = new Label
            {
                Name = "lblRecentEmptyDetail",
                Text = "Dữ liệu sẽ hiển thị tại đây khi có công tác được tạo.",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 11F, FontStyle.Regular),
                ForeColor = Color.FromArgb(103, 128, 169),
                BackColor = Color.White,
                Visible = false
            };
            pnlRecent.Controls.Add(lblRecentEmptyIcon);
            pnlRecent.Controls.Add(lblRecentEmptyDetail);
        }

        private void ApplyDashboardStyles()
        {
            StyleGrid(dgvRecentMission);
            StyleGrid(dgvTopPersonnel);
            BackColor = Color.FromArgb(242, 248, 255);
            pnlHeader.Paint -= PnlHeader_Paint;
            pnlHeader.Paint += PnlHeader_Paint;
            lblHeaderIcon.Paint -= LblHeaderIcon_Paint;
            lblHeaderIcon.Paint += LblHeaderIcon_Paint;
            pnlCardTotal.Paint -= PnlCardTotal_Paint;
            pnlCardTotal.Paint += PnlCardTotal_Paint;
            pnlCardActive.Paint -= PnlCardActive_Paint;
            pnlCardActive.Paint += PnlCardActive_Paint;
            pnlCardEnded.Paint -= PnlCardEnded_Paint;
            pnlCardEnded.Paint += PnlCardEnded_Paint;
            pnlCardTop.Paint -= PnlCardTop_Paint;
            pnlCardTop.Paint += PnlCardTop_Paint;
            pnlRecent.Paint -= DashboardPanel_Paint;
            pnlRecent.Paint += DashboardPanel_Paint;
            pnlTopPersonnel.Paint -= DashboardPanel_Paint;
            pnlTopPersonnel.Paint += DashboardPanel_Paint;

            // Keep header text transparent so the gradient is continuous (no dark label rectangles).
            lblTitle.BackColor = Color.Transparent;
            lblSubtitle.BackColor = Color.Transparent;
            lblHeaderIcon.BackColor = Color.Transparent;
            lblTitle.UseCompatibleTextRendering = true;
            lblSubtitle.UseCompatibleTextRendering = true;

            // Softer, rounded-looking dashboard cards while preserving existing icon/logo assets.
            pnlCardTotal.Padding = new Padding(0);
            pnlCardActive.Padding = new Padding(0);
            pnlCardEnded.Padding = new Padding(0);
            pnlCardTop.Padding = new Padding(0);
            // Remove native square borders; the rounded pastel outline is drawn in Paint.
            pnlCardTotal.BorderStyle = BorderStyle.None;
            pnlCardActive.BorderStyle = BorderStyle.None;
            pnlCardEnded.BorderStyle = BorderStyle.None;
            pnlCardTop.BorderStyle = BorderStyle.None;
            pnlRecent.BorderStyle = BorderStyle.None;
            pnlTopPersonnel.BorderStyle = BorderStyle.None;
            lblRecentTitle.Text = "▣  5 công tác gần nhất";
            lblPersonnelTableTitle.Text = "♛  Top 5 CBCS công tác nhiều nhất";

            btnRefresh.Cursor = Cursors.Hand;
            btnClose.Cursor = Cursors.Hand;
            btnRefresh.FlatAppearance.MouseOverBackColor = Color.FromArgb(225, 239, 255);
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(34, 83, 139);
            // Draw the refresh button border ourselves: WinForms' native FlatStyle border
            // can be clipped by a rounded Region, leaving only the top/left edges visible.
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnRefresh.UseVisualStyleBackColor = false;
            btnClose.UseVisualStyleBackColor = false;
            ApplyRoundedRegion(btnRefresh, 9);
            ApplyRoundedRegion(btnClose, 9);

            // Gắn lại sự kiện Paint để đường viền của nút Làm mới luôn được vẽ đủ 4 cạnh.
            // FlatAppearance.BorderSize = 0 tránh viền vuông mặc định của WinForms;
            // đường viền bo góc được tự vẽ trong BtnRefresh_Paint.
            btnRefresh.Paint -= BtnRefresh_Paint;
            btnRefresh.Paint += BtnRefresh_Paint;
            btnRefresh.MouseEnter -= BtnRefresh_MouseEnterLeave;
            btnRefresh.MouseLeave -= BtnRefresh_MouseEnterLeave;
            btnRefresh.MouseEnter += BtnRefresh_MouseEnterLeave;
            btnRefresh.MouseLeave += BtnRefresh_MouseEnterLeave;
        }


        private void BtnRefresh_MouseEnterLeave(object? sender, EventArgs e)
        {
            if (sender is Control control)
                control.Invalidate();
        }

        private void BtnRefresh_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is not Button button || button.ClientSize.Width < 4 || button.ClientSize.Height < 4)
                return;

            // Vẽ đường viền nằm hoàn toàn bên trong vùng client để không bị cắt ở mép.
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            bool hovered = button.ClientRectangle.Contains(button.PointToClient(Cursor.Position));
            Color borderColor = hovered
                ? Color.FromArgb(28, 100, 190)
                : Color.FromArgb(28, 64, 100);

            using var pen = new Pen(borderColor, 1.4f)
            {
                Alignment = System.Drawing.Drawing2D.PenAlignment.Inset,
                LineJoin = System.Drawing.Drawing2D.LineJoin.Round
            };

            int width = button.ClientSize.Width - 2;
            int height = button.ClientSize.Height - 2;
            DrawRoundedRectangle(e.Graphics, pen, new Rectangle(1, 1, width, height), 9);
        }

        private static void StyleGrid(DataGridView grid)
        {
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(19, 65, 115);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            grid.DefaultCellStyle.BackColor = Color.White;
            grid.DefaultCellStyle.ForeColor = Color.FromArgb(24, 48, 83);
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            grid.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(218, 236, 255);
            grid.DefaultCellStyle.SelectionForeColor = Color.FromArgb(15, 53, 105);
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(246, 250, 255);
            grid.GridColor = Color.FromArgb(207, 224, 243);
            grid.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            grid.RowTemplate.Height = 30;
            grid.ColumnHeadersHeight = 44;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.ScrollBars = ScrollBars.None;
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            grid.AutoGenerateColumns = true;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            grid.RowHeadersVisible = false;
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadDashboardAsync();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async Task LoadRecentMissionAsync()
        {
            var missions = await _missionService.GetRecentMissionsAsync();
            dgvRecentMission.DataSource = null;
            dgvRecentMission.DataSource = missions;

            ConfigureVisibleColumns(dgvRecentMission,
                new[] { "MissionName", "MissionType", "AllowOverlap", "StartTime" });
            SetColumnHeader(dgvRecentMission, "MissionName", "Tên công tác");
            SetColumnHeader(dgvRecentMission, "MissionType", "Loại công tác");
            SetColumnHeader(dgvRecentMission, "AllowOverlap", "Cho phép chồng");
            SetColumnHeader(dgvRecentMission, "StartTime", "Thời gian bắt đầu");
            AddOrUpdateIndexColumn(dgvRecentMission);
            SetFillWeight(dgvRecentMission, "STT", 38);
            SetFillWeight(dgvRecentMission, "MissionName", 105);
            SetFillWeight(dgvRecentMission, "MissionType", 98);
            SetFillWeight(dgvRecentMission, "AllowOverlap", 118);
            SetFillWeight(dgvRecentMission, "StartTime", 128);
            SetColumnAlignment(dgvRecentMission, "STT", DataGridViewContentAlignment.MiddleCenter);
            SetColumnAlignment(dgvRecentMission, "AllowOverlap", DataGridViewContentAlignment.MiddleCenter);
            SetColumnAlignment(dgvRecentMission, "StartTime", DataGridViewContentAlignment.MiddleCenter);
            FormatBooleanColumn(dgvRecentMission, "AllowOverlap");

            bool hasNoMissions = dgvRecentMission.Rows.Count == 0;
            lblRecentEmpty.Visible = hasNoMissions;
            lblRecentEmptyIcon.Visible = hasNoMissions;
            lblRecentEmptyDetail.Visible = hasNoMissions;
            if (hasNoMissions)
            {
                lblRecentEmptyIcon.BringToFront();
                lblRecentEmpty.BringToFront();
                lblRecentEmptyDetail.BringToFront();
            }
        }

        private async Task LoadTopPersonnelAsync()
        {
            dgvTopPersonnel.DataSource = null;
            dgvTopPersonnel.DataSource = await _missionService.GetTopPersonnelAsync();

            ConfigureVisibleColumns(dgvTopPersonnel,
                new[] { "SoHieu", "HoTen", "DonVi", "CapBacHam", "SoLanCongTac" });
            SetColumnHeader(dgvTopPersonnel, "SoHieu", "Số hiệu");
            SetColumnHeader(dgvTopPersonnel, "HoTen", "Họ tên");
            SetColumnHeader(dgvTopPersonnel, "DonVi", "Đơn vị");
            SetColumnHeader(dgvTopPersonnel, "CapBacHam", "Cấp bậc hàm");
            SetColumnHeader(dgvTopPersonnel, "SoLanCongTac", "Số lần");
            AddOrUpdateIndexColumn(dgvTopPersonnel);
            SetFillWeight(dgvTopPersonnel, "STT", 42);
            SetFillWeight(dgvTopPersonnel, "SoHieu", 82);
            SetFillWeight(dgvTopPersonnel, "HoTen", 118);
            SetFillWeight(dgvTopPersonnel, "DonVi", 78);
            SetFillWeight(dgvTopPersonnel, "CapBacHam", 104);
            SetFillWeight(dgvTopPersonnel, "SoLanCongTac", 58);
            SetColumnAlignment(dgvTopPersonnel, "STT", DataGridViewContentAlignment.MiddleCenter);
            SetColumnAlignment(dgvTopPersonnel, "SoLanCongTac", DataGridViewContentAlignment.MiddleCenter);
        }

        private void PnlHeader_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is not Panel panel) return;
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                panel.ClientRectangle,
                Color.FromArgb(12, 49, 93),
                Color.FromArgb(47, 126, 194),
                0F);
            g.FillRectangle(brush, panel.ClientRectangle);

            // Subtle flowing bands and a generic, low-opacity seal motif matching the reference.
            using (var wave1 = new SolidBrush(Color.FromArgb(22, Color.White)))
            using (var wave2 = new SolidBrush(Color.FromArgb(13, Color.White)))
            {
                var r = panel.ClientRectangle;
                g.FillEllipse(wave1, r.Width * 0.36f, r.Height * 0.35f, r.Width * 0.78f, r.Height * 1.10f);
                g.FillEllipse(wave2, r.Width * 0.48f, r.Height * 0.52f, r.Width * 0.70f, r.Height * 0.82f);
            }
            DrawHeaderSeal(g, panel.ClientSize.Width - 220, -24, 190);
        }

        private static void DrawHeaderSeal(Graphics g, int x, int y, int size)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var pen = new Pen(Color.FromArgb(24, Color.White), 4f);
            using var penThin = new Pen(Color.FromArgb(20, Color.White), 2f);
            g.DrawEllipse(pen, x, y, size, size);
            g.DrawEllipse(penThin, x + 13, y + 13, size - 26, size - 26);
            var points = new PointF[10];
            float cx = x + size / 2f, cy = y + size / 2f;
            for (int i = 0; i < points.Length; i++)
            {
                double a = -Math.PI / 2 + i * Math.PI / 5;
                float radius = i % 2 == 0 ? size * 0.28f : size * 0.12f;
                points[i] = new PointF(cx + (float)Math.Cos(a) * radius, cy + (float)Math.Sin(a) * radius);
            }
            using var star = new SolidBrush(Color.FromArgb(19, Color.White));
            g.FillPolygon(star, points);
        }

        private void LblHeaderIcon_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is not Label label) return;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            int x = 18, y = 14, w = Math.Min(44, label.Width - 30), h = Math.Min(52, label.Height - 28);
            using var pen = new Pen(Color.White, 3f);
            DrawRoundedRectangle(e.Graphics, pen, new Rectangle(x, y, w, h), 5);
            e.Graphics.DrawLine(pen, x + 9, y + 15, x + w - 8, y + 15);
            e.Graphics.DrawLine(pen, x + 9, y + 25, x + w - 8, y + 25);
            e.Graphics.DrawLine(pen, x + 9, y + 35, x + w - 8, y + 35);
            e.Graphics.DrawLine(pen, x + 13, y - 2, x + w - 13, y - 2);
        }

        private void PnlCardTotal_Paint(object? sender, PaintEventArgs e)
        {
            DrawStatIcon(e.Graphics, pnlCardTotal.ClientRectangle, Color.FromArgb(25, 111, 222), 0);
            DrawStatCardBorder(e.Graphics, pnlCardTotal.ClientRectangle, Color.FromArgb(174, 216, 255));
        }

        private void PnlCardActive_Paint(object? sender, PaintEventArgs e)
        {
            DrawStatIcon(e.Graphics, pnlCardActive.ClientRectangle, Color.FromArgb(23, 173, 83), 1);
            DrawStatCardBorder(e.Graphics, pnlCardActive.ClientRectangle, Color.FromArgb(183, 232, 211));
        }

        private void PnlCardEnded_Paint(object? sender, PaintEventArgs e)
        {
            DrawStatIcon(e.Graphics, pnlCardEnded.ClientRectangle, Color.FromArgb(226, 47, 57), 2);
            DrawStatCardBorder(e.Graphics, pnlCardEnded.ClientRectangle, Color.FromArgb(255, 205, 208));
        }

        private void PnlCardTop_Paint(object? sender, PaintEventArgs e)
        {
            DrawStatIcon(e.Graphics, pnlCardTop.ClientRectangle, Color.FromArgb(91, 72, 225), 3);
            DrawStatCardBorder(e.Graphics, pnlCardTop.ClientRectangle, Color.FromArgb(218, 211, 255));
        }

        private static void DrawStatCardBorder(Graphics g, Rectangle bounds, Color color)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var pen = new Pen(color, 1f);
            DrawRoundedRectangle(g, pen, new Rectangle(1, 1, bounds.Width - 3, bounds.Height - 3), 12);
        }

        private static void DrawStatIcon(Graphics g, Rectangle bounds, Color color, int kind)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            int d = Math.Min(58, Math.Max(46, bounds.Height - 46));
            var circle = new Rectangle(16, Math.Max(12, (bounds.Height - d) / 2), d, d);
            using var pale = new SolidBrush(Color.FromArgb(28, color));
            using var solid = new SolidBrush(color);
            using var white = new SolidBrush(Color.White);
            using var pen = new Pen(color, 2.4f);
            g.FillEllipse(pale, circle);
            var c = new Rectangle(circle.X + 10, circle.Y + 10, circle.Width - 20, circle.Height - 20);
            if (kind == 0)
            {
                PointF[][] layers = {
                    new[] { new PointF(c.X, c.Y + c.Height * .25f), new PointF(c.X + c.Width/2f, c.Y), new PointF(c.Right, c.Y + c.Height*.25f), new PointF(c.X+c.Width/2f, c.Y+c.Height*.5f) },
                    new[] { new PointF(c.X, c.Y + c.Height*.48f), new PointF(c.X+c.Width/2f, c.Y+c.Height*.23f), new PointF(c.Right, c.Y+c.Height*.48f), new PointF(c.X+c.Width/2f, c.Y+c.Height*.73f) },
                    new[] { new PointF(c.X, c.Y + c.Height*.70f), new PointF(c.X+c.Width/2f, c.Y+c.Height*.45f), new PointF(c.Right, c.Y+c.Height*.70f), new PointF(c.X+c.Width/2f, c.Bottom) }
                };
                foreach (var layer in layers) g.FillPolygon(solid, layer);
            }
            else if (kind == 1)
            {
                g.FillEllipse(solid, c);
                var tri = new[] { new PointF(c.X + c.Width * .38f, c.Y + c.Height * .22f), new PointF(c.X + c.Width * .38f, c.Bottom - c.Height * .22f), new PointF(c.Right - c.Width * .20f, c.Y + c.Height / 2f) };
                g.FillPolygon(white, tri);
            }
            else if (kind == 2)
            {
                g.FillEllipse(solid, c);
                g.FillRectangle(white, c.X + c.Width * .30f, c.Y + c.Height * .30f, c.Width * .40f, c.Height * .40f);
            }
            else
            {
                g.FillRectangle(solid, c.X, c.Y + c.Height * .55f, c.Width * .22f, c.Height * .45f);
                g.FillRectangle(solid, c.X + c.Width * .37f, c.Y + c.Height * .32f, c.Width * .22f, c.Height * .68f);
                g.FillRectangle(solid, c.X + c.Width * .74f, c.Y, c.Width * .22f, c.Height);
            }
        }

        private void DashboardPanel_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is not Panel panel) return;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var pen = new Pen(Color.FromArgb(211, 230, 251), 1.2f);
            DrawRoundedRectangle(e.Graphics, pen, new Rectangle(1, 1, panel.Width - 3, panel.Height - 3), 10);
        }

        private void LayoutCompactDashboard()
        {
            int w = ClientSize.Width;
            int h = ClientSize.Height;
            int margin = Math.Max(18, (int)(w * 0.018));
            int gap = 14;
            int headerH = Math.Max(122, (int)(h * 0.17));

            pnlHeader.SetBounds(0, 0, w, headerH);
            lblHeaderIcon.SetBounds(24, 15, 78, headerH - 28);
            lblHeaderIcon.Text = string.Empty;
            lblTitle.SetBounds(118, 12, Math.Max(400, w - 150), 54);
            lblTitle.Font = new Font("Segoe UI", Math.Max(23F, Math.Min(29F, w / 48F)), FontStyle.Bold);
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblSubtitle.SetBounds(121, 68, Math.Max(400, w - 155), 36);
            lblSubtitle.Font = new Font("Segoe UI", 12.5F, FontStyle.Regular);
            lblSubtitle.TextAlign = ContentAlignment.MiddleLeft;

            int buttonY = headerH + 12;
            btnClose.SetBounds(w - margin - 126, buttonY, 126, 42);
            btnRefresh.SetBounds(w - margin - 126 - gap - 142, buttonY, 142, 42);
            ApplyRoundedRegion(btnRefresh, 9);
            ApplyRoundedRegion(btnClose, 9);

            int dashboardY = buttonY + 58;
            int dashboardH = Math.Max(350, h - dashboardY - margin);
            pnlDashboard.SetBounds(margin, dashboardY, w - margin * 2, dashboardH);

            int cardGap = 12;
            int cardH = Math.Max(112, Math.Min(132, (int)(dashboardH * 0.235)));
            int cardW = (pnlDashboard.Width - cardGap * 3) / 4;
            pnlCardTotal.SetBounds(0, 0, cardW, cardH);
            pnlCardActive.SetBounds(cardW + cardGap, 0, cardW, cardH);
            pnlCardEnded.SetBounds((cardW + cardGap) * 2, 0, cardW, cardH);
            pnlCardTop.SetBounds((cardW + cardGap) * 3, 0, pnlDashboard.Width - ((cardW + cardGap) * 3), cardH);
            ApplyRoundedRegion(pnlCardTotal, 12);
            ApplyRoundedRegion(pnlCardActive, 12);
            ApplyRoundedRegion(pnlCardEnded, 12);
            ApplyRoundedRegion(pnlCardTop, 12);
            pnlCardTotal.BorderStyle = BorderStyle.None;
            pnlCardActive.BorderStyle = BorderStyle.None;
            pnlCardEnded.BorderStyle = BorderStyle.None;
            pnlCardTop.BorderStyle = BorderStyle.None;

            LayoutStatCard(pnlCardTotal, lblTongCongTacTitle, lblTongCongTac, lblTotalUnit, cardW);
            LayoutStatCard(pnlCardActive, lblDangHoatDongTitle, lblDangHoatDong, lblActiveUnit, cardW);
            LayoutStatCard(pnlCardEnded, lblDaKetThucTitle, lblDaKetThuc, lblEndedUnit, cardW);
            lblTopPersonnelTitle.SetBounds(82, 14, pnlCardTop.Width - 92, 28);
            lblTopPersonnelTitle.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
            lblTopPersonnel.SetBounds(82, 44, pnlCardTop.Width - 90, 38);
            lblTopPersonnel.Font = new Font("Segoe UI", 14.5F, FontStyle.Bold);
            lblTopPersonnel.AutoEllipsis = false;
            lblTopPersonnelCount.SetBounds(82, 82, pnlCardTop.Width - 90, 24);
            lblTopPersonnelCount.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular);

            int panelsY = cardH + 16;
            int panelH = Math.Max(190, dashboardH - panelsY);
            int panelGap = 14;
            int leftW = (pnlDashboard.Width - panelGap) / 2;
            int rightW = pnlDashboard.Width - panelGap - leftW;
            pnlRecent.SetBounds(0, panelsY, leftW, panelH);
            pnlTopPersonnel.SetBounds(leftW + panelGap, panelsY, rightW, panelH);
            pnlRecent.BorderStyle = BorderStyle.None;
            pnlTopPersonnel.BorderStyle = BorderStyle.None;
            ApplyRoundedRegion(pnlRecent, 12);
            ApplyRoundedRegion(pnlTopPersonnel, 12);

            lblRecentTitle.SetBounds(14, 10, leftW - 28, 34);
            lblPersonnelTableTitle.SetBounds(14, 10, rightW - 28, 34);
            lblRecentTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblPersonnelTableTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblRecentTitle.BackColor = Color.Transparent;
            lblPersonnelTableTitle.BackColor = Color.Transparent;

            int gridTop = 52;
            int gridH = Math.Max(100, panelH - gridTop - 12);
            dgvRecentMission.SetBounds(12, gridTop, leftW - 24, gridH);
            dgvTopPersonnel.SetBounds(12, gridTop, rightW - 24, gridH);
            // Clip the grid to a rounded shape so its dark header corners are rounded too.
            ApplyRoundedRegion(dgvRecentMission, 8);
            ApplyRoundedRegion(dgvTopPersonnel, 8);
            int emptyCenterY = gridTop + Math.Max(20, (gridH - 38) / 2);
            lblRecentEmptyIcon.SetBounds(Math.Max(20, (leftW - 82) / 2), emptyCenterY - 66, 82, 82);
            lblRecentEmptyIcon.Font = new Font("Segoe UI Symbol", 27F, FontStyle.Bold);
            ApplyRoundedRegion(lblRecentEmptyIcon, 41);
            lblRecentEmpty.SetBounds(24, emptyCenterY + 20, leftW - 48, 36);
            lblRecentEmpty.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblRecentEmpty.BackColor = Color.White;
            lblRecentEmptyDetail.SetBounds(24, emptyCenterY + 57, leftW - 48, 30);
            lblRecentEmptyDetail.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular);
            lblRecentEmptyDetail.BackColor = Color.White;

            foreach (Control c in new Control[] { pnlCardTotal, pnlCardActive, pnlCardEnded, pnlCardTop, pnlRecent, pnlTopPersonnel })
                c.Margin = new Padding(0);
        }

        private static void LayoutStatCard(Panel card, Label title, Label value, Label unit, int cardWidth)
        {
            title.SetBounds(88, 17, cardWidth - 100, 28);
            title.Font = new Font("Segoe UI", 12.5F, FontStyle.Bold);
            value.SetBounds(88, 48, 78, 58);
            value.Font = new Font("Segoe UI", 29F, FontStyle.Bold);
            unit.SetBounds(158, 69, Math.Max(42, cardWidth - 170), 28);
            unit.Font = new Font("Segoe UI", 11.5F, FontStyle.Regular);
        }

        private static void ApplyRoundedRegion(Control control, int radius)
        {
            if (control.Width <= 0 || control.Height <= 0) return;
            int d = radius * 2;
            using var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(0, 0, d, d, 180, 90);
            path.AddArc(control.Width - d - 1, 0, d, d, 270, 90);
            path.AddArc(control.Width - d - 1, control.Height - d - 1, d, d, 0, 90);
            path.AddArc(0, control.Height - d - 1, d, d, 90, 90);
            path.CloseFigure();
            var oldRegion = control.Region;
            control.Region = new Region(path);
            oldRegion?.Dispose();
        }

        private static void DrawRoundedRectangle(Graphics graphics, Pen pen, Rectangle bounds, int radius)
        {
            int d = radius * 2;
            using var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            graphics.DrawPath(pen, path);
        }

        private static void ConfigureVisibleColumns(DataGridView grid, string[] allowedNames)
        {
            foreach (DataGridViewColumn column in grid.Columns)
                column.Visible = Array.Exists(allowedNames, name => string.Equals(name, column.Name, StringComparison.OrdinalIgnoreCase));
        }

        private static void AddOrUpdateIndexColumn(DataGridView grid)
        {
            if (!grid.Columns.Contains("STT"))
            {
                var indexColumn = new DataGridViewTextBoxColumn
                {
                    Name = "STT",
                    HeaderText = "STT",
                    ReadOnly = true,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                };
                grid.Columns.Insert(0, indexColumn);
            }
            grid.Columns["STT"].Visible = true;
            grid.Columns["STT"].DisplayIndex = 0;
            grid.Columns["STT"].ReadOnly = true;
            for (int i = 0; i < grid.Rows.Count; i++)
                if (!grid.Rows[i].IsNewRow) grid.Rows[i].Cells["STT"].Value = (i + 1).ToString();
        }

        private static void SetColumnAlignment(DataGridView grid, string name, DataGridViewContentAlignment alignment)
        {
            if (grid.Columns.Contains(name))
            {
                grid.Columns[name].DefaultCellStyle.Alignment = alignment;
                grid.Columns[name].HeaderCell.Style.Alignment = alignment;
            }
        }

        private static void SetFillWeight(DataGridView grid, string name, float weight)
        {
            if (grid.Columns.Contains(name)) grid.Columns[name].FillWeight = weight;
        }

        private static void FormatBooleanColumn(DataGridView grid, string name)
        {
            if (!grid.Columns.Contains(name)) return;
            var column = grid.Columns[name];
            if (column.ValueType == typeof(bool) || column is DataGridViewCheckBoxColumn)
            {
                column.Visible = true;
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            else
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private static void SetColumnVisible(DataGridView grid, string name, bool visible)
        {
            if (grid.Columns.Contains(name))
                grid.Columns[name].Visible = visible;
        }

        private static void SetColumnHeader(DataGridView grid, string name, string header)
        {
            if (grid.Columns.Contains(name))
                grid.Columns[name].HeaderText = header;
        }
    }
}