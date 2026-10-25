using QuanLyCBCS.Data;
using QuanLyCBCS.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCBCS.Forms.PersonnelForms
{
    public partial class FrmAdminDashboard : Form
    {

        private Label _lblPersonnelValue = null!;
        private Label _lblSoldierValue = null!;
        private Label _lblOfficerValue = null!;

        private Label _lblUsersValue = null!;
        private Label _lblAdminValue = null!;
        private Label _lblUserValue = null!;
        private Label _lblLockedValue = null!;

        private Label _lblMissionValue = null!;
        private Label _lblActiveMissionValue = null!;
        private Label _lblEndingMissionValue = null!;
        private Label _lblFinishedMissionValue = null!;

        private Label _lblLoginValue = null!;
        private Label _lblFailedLoginValue = null!;
        private Label _lblLockValue = null!;
        private Label _lblResetValue = null!;
        private Label _lblUnlockValue = null!;
        private Label _lblChangePasswordValue = null!;

        private Panel _dashboardRoot = null!;
        private DataGridView _auditGrid = null!;

        private static readonly Color Navy = Color.FromArgb(19, 65, 117);
        private static readonly Color DeepNavy = Color.FromArgb(13, 51, 96);
        private static readonly Color Blue = Color.FromArgb(0, 102, 204);
        private static readonly Color TextColor = Color.FromArgb(25, 48, 82);
        private static readonly Color BorderColor = Color.FromArgb(213, 228, 245);
        private static readonly Color Surface = Color.White;
        private static readonly Color Background = Color.FromArgb(242, 248, 254);

        public FrmAdminDashboard()
        {
            InitializeComponent();
            BuildModernDashboard();
        }

        private void BuildModernDashboard()
        {
            // Keep the existing data-bound controls and event handlers, but replace
            // the old default WinForms layout with the new dashboard composition.
            Controls.Clear();

            Text = "Bảng điều khiển quản trị";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(1280, 760);
            WindowState = FormWindowState.Maximized;
            BackColor = Background;
            Font = new Font("Segoe UI", 10F, FontStyle.Regular);

            _dashboardRoot = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Background,
                Padding = new Padding(0)
            };
            Controls.Add(_dashboardRoot);

            var rootLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Background,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 112F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 350F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _dashboardRoot.Controls.Add(rootLayout);

            // Header banner
            var header = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Navy,
                Padding = new Padding(34, 17, 30, 12),
                Margin = new Padding(0)
            };
            header.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                    header.ClientRectangle, Color.FromArgb(8, 55, 111), Color.FromArgb(53, 123, 185), 0F))
                    e.Graphics.FillRectangle(brush, header.ClientRectangle);

                // Subtle official-style watermark on the right, matching the supplied design.
                int diameter = Math.Min(220, header.Height + 95);
                var emblem = new Rectangle(header.Width - diameter + 16, -22, diameter, diameter);
                using (var pen = new Pen(Color.FromArgb(28, Color.White), 3F))
                {
                    e.Graphics.DrawEllipse(pen, emblem);
                    e.Graphics.DrawEllipse(pen, Rectangle.Inflate(emblem, -13, -13));
                }
                using (var brush = new SolidBrush(Color.FromArgb(22, Color.White)))
                {
                    PointF[] star = new PointF[10];
                    float cx = emblem.Left + emblem.Width / 2F;
                    float cy = emblem.Top + emblem.Height / 2F;
                    float outer = diameter * 0.31F;
                    float inner = outer * 0.42F;
                    for (int i = 0; i < star.Length; i++)
                    {
                        double a = -Math.PI / 2 + i * Math.PI / 5;
                        float r = i % 2 == 0 ? outer : inner;
                        star[i] = new PointF(cx + (float)Math.Cos(a) * r, cy + (float)Math.Sin(a) * r);
                    }
                    e.Graphics.FillPolygon(brush, star);
                }
            };

            var headerIcon = new Label
            {
                Text = "⚙",
                Font = new Font("Segoe UI Symbol", 34F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(28, 4),
                Size = new Size(78, 75),
                BackColor = Color.Transparent
            };
            var headerTitle = new Label
            {
                Text = "BẢNG ĐIỀU KHIỂN QUẢN TRỊ",
                Font = new Font("Segoe UI", 27F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(110, 12),
                BackColor = Color.Transparent
            };
            var headerSubtitle = new Label
            {
                Text = "Theo dõi tổng quan hệ thống, người dùng, công tác và bảo mật",
                Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                ForeColor = Color.FromArgb(225, 238, 252),
                AutoSize = false,
                Location = new Point(112, 65),
                Size = new Size(760, 28),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent
            };
            header.Controls.Add(headerIcon);
            header.Controls.Add(headerTitle);
            header.Controls.Add(headerSubtitle);
            rootLayout.Controls.Add(header, 0, 0);

            // Four summary cards
            var cards = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1,
                Padding = new Padding(22, 18, 22, 14),
                BackColor = Background,
                Margin = new Padding(0)
            };
            for (int i = 0; i < 4; i++)
                cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            var personnelCard = CreateCard("CBCS", "\uE716", Color.FromArgb(232, 243, 255), Color.FromArgb(0, 91, 190));
            var usersCard = CreateCard("Người dùng", "\uE77B", Color.FromArgb(235, 249, 241), Color.FromArgb(0, 123, 65));
            var missionCard = CreateCard("Công tác", "\uE70B", Color.FromArgb(255, 247, 232), Color.FromArgb(218, 91, 0));
            var securityCard = CreateCard("Bảo mật", "\uE72E", Color.FromArgb(255, 239, 240), Color.FromArgb(198, 35, 35));

            _lblPersonnelValue = AddMetric(personnelCard, "Tổng CBCS:", "0", 100);
            _lblSoldierValue = AddMetric(personnelCard, "Chiến sĩ:", "0", 164);
            _lblOfficerValue = AddMetric(personnelCard, "Cán bộ:", "0", 228);

            _lblUsersValue = AddMetric(usersCard, "Tổng tài khoản:", "0", 94);
            _lblAdminValue = AddMetric(usersCard, "Admin:", "0", 142);
            _lblUserValue = AddMetric(usersCard, "User:", "0", 190);
            _lblLockedValue = AddMetric(usersCard, "Đang khóa:", "0", 238);

            _lblMissionValue = AddMetric(missionCard, "Tổng công tác:", "0", 94);
            _lblActiveMissionValue = AddMetric(missionCard, "Đang diễn ra:", "0", 142);
            _lblEndingMissionValue = AddMetric(missionCard, "Sắp kết thúc:", "0", 190);
            _lblFinishedMissionValue = AddMetric(missionCard, "Đã kết thúc:", "0", 238);

            // Six security metrics are shown in compact rows so the last row remains
            // fully inside the card and does not overlap or clip the lower border.
            _lblLoginValue = AddCompactMetric(securityCard, "LOGIN:", "0", 82);
            _lblFailedLoginValue = AddCompactMetric(securityCard, "FAILED LOGIN:", "0", 116);
            _lblLockValue = AddCompactMetric(securityCard, "LOCK ACCOUNT:", "0", 150);
            _lblResetValue = AddCompactMetric(securityCard, "RESET PASSWORD:", "0", 184);
            _lblUnlockValue = AddCompactMetric(securityCard, "UNLOCK ACCOUNT:", "0", 218);
            _lblChangePasswordValue = AddCompactMetric(securityCard, "CHANGE PASSWORD:", "0", 252);

            cards.Controls.Add(personnelCard, 0, 0);
            cards.Controls.Add(usersCard, 1, 0);
            cards.Controls.Add(missionCard, 2, 0);
            cards.Controls.Add(securityCard, 3, 0);
            rootLayout.Controls.Add(cards, 0, 1);

            // Audit log section
            var auditPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Surface,
                Padding = new Padding(18, 12, 18, 14),
                Margin = new Padding(22, 0, 22, 18)
            };
            auditPanel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (var path = RoundedRectangle(new Rectangle(0, 0, auditPanel.Width - 1, auditPanel.Height - 1), 12))
                using (var pen = new Pen(BorderColor, 1F))
                    e.Graphics.DrawPath(pen, path);
            };

            var auditHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 54,
                BackColor = Surface
            };
            var auditTitle = new Label
            {
                Text = "▤  NHẬT KÝ HỆ THỐNG (AUDIT LOG)",
                Dock = DockStyle.Left,
                Width = 600,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = DeepNavy
            };
            btnRefresh.Text = "⟳  Làm mới";
            btnRefresh.Size = new Size(142, 38);
            btnRefresh.Dock = DockStyle.Right;
            btnRefresh.Margin = new Padding(0, 4, 0, 4);
            btnRefresh.MinimumSize = new Size(142, 38);
            btnRefresh.MaximumSize = new Size(142, 38);
            btnRefresh.Resize -= BtnRefresh_Resize;
            btnRefresh.Resize += BtnRefresh_Resize;
            BtnRefresh_Resize(btnRefresh, EventArgs.Empty);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatAppearance.MouseOverBackColor = Color.FromArgb(31, 91, 151);
            btnRefresh.FlatAppearance.MouseDownBackColor = DeepNavy;
            btnRefresh.BackColor = Navy;
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnRefresh.Cursor = Cursors.Hand;
            auditHeader.Controls.Add(btnRefresh);
            auditHeader.Controls.Add(auditTitle);

            // Rounded host keeps the table visually inside the rounded audit section.
            var gridHost = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Surface,
                Padding = new Padding(1),
                Margin = new Padding(0, 2, 0, 0)
            };
            gridHost.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (var path = RoundedRectangle(new Rectangle(0, 0, gridHost.Width - 1, gridHost.Height - 1), 8))
                using (var pen = new Pen(BorderColor, 1F))
                    e.Graphics.DrawPath(pen, path);
            };
            gridHost.Resize += (s, e) => ApplyRoundedRegion(gridHost, 8);

            _auditGrid = dgvRecentLogs;
            _auditGrid.Dock = DockStyle.Fill;
            _auditGrid.Margin = new Padding(0);
            _auditGrid.ScrollBars = ScrollBars.Vertical;
            _auditGrid.VirtualMode = false;
            _auditGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            _auditGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _auditGrid.ColumnHeadersVisible = true;
            _auditGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            _auditGrid.AllowUserToResizeRows = false;
            _auditGrid.BackgroundColor = Surface;
            _auditGrid.BorderStyle = BorderStyle.None;
            _auditGrid.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            _auditGrid.GridColor = BorderColor;
            _auditGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            _auditGrid.EnableHeadersVisualStyles = false;
            _auditGrid.ColumnHeadersDefaultCellStyle.BackColor = Navy;
            _auditGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            _auditGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _auditGrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _auditGrid.DefaultCellStyle.BackColor = Color.White;
            _auditGrid.DefaultCellStyle.ForeColor = TextColor;
            _auditGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(133, 210, 245);
            _auditGrid.DefaultCellStyle.SelectionForeColor = DeepNavy;
            _auditGrid.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            _auditGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 249, 253);
            _auditGrid.RowTemplate.Height = 35;
            _auditGrid.ColumnHeadersHeight = 38;
            _auditGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _auditGrid.AllowUserToAddRows = false;
            _auditGrid.AllowUserToDeleteRows = false;
            _auditGrid.ReadOnly = true;
            _auditGrid.RowHeadersVisible = false;
            _auditGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _auditGrid.MultiSelect = false;
            _auditGrid.DataBindingComplete -= AuditGrid_DataBindingComplete;
            _auditGrid.DataBindingComplete += AuditGrid_DataBindingComplete;

            gridHost.Controls.Add(_auditGrid);
            auditPanel.Controls.Add(gridHost);
            auditPanel.Controls.Add(auditHeader);
            rootLayout.Controls.Add(auditPanel, 0, 2);
        }

        private void BtnRefresh_Resize(object? sender, EventArgs e)
        {
            if (btnRefresh.Width <= 0 || btnRefresh.Height <= 0)
                return;

            using (var path = RoundedRectangle(new Rectangle(0, 0, btnRefresh.Width - 1, btnRefresh.Height - 1), 8))
                btnRefresh.Region = new Region(path);
        }

        private static void ApplyRoundedRegion(Control control, int radius)
        {
            if (control.Width <= 0 || control.Height <= 0)
                return;

            using (var path = RoundedRectangle(new Rectangle(0, 0, control.Width - 1, control.Height - 1), radius))
                control.Region = new Region(path);
        }

        private Panel CreateCard(string title, string icon, Color backColor, Color accent)
        {
            var card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = backColor,
                Margin = new Padding(7),
                Padding = new Padding(15, 12, 15, 12)
            };
            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                    card.ClientRectangle, Color.White, backColor, 90F))
                using (var path = RoundedRectangle(new Rectangle(0, 0, card.Width - 1, card.Height - 1), 12))
                using (var pen = new Pen(Color.FromArgb(215, accent), 1F))
                {
                    e.Graphics.FillPath(brush, path);
                    e.Graphics.DrawPath(pen, path);
                }

                using (var brush = new SolidBrush(Color.FromArgb(24, accent)))
                    e.Graphics.FillEllipse(brush, new Rectangle(20, 10, 58, 58));
                using (var pen = new Pen(Color.FromArgb(55, accent), 1F))
                    e.Graphics.DrawLine(pen, 24, 79, card.Width - 24, 79);
            };

            var iconLabel = new Label
            {
                Text = icon,
                Font = new Font("Segoe MDL2 Assets", 24F, FontStyle.Regular),
                ForeColor = accent,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(58, 58),
                Location = new Point(20, 10)
            };
            var titleLabel = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = accent,
                AutoSize = false,
                Location = new Point(94, 12),
                Size = new Size(220, 52),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent
            };
            card.Controls.Add(iconLabel);
            card.Controls.Add(titleLabel);
            return card;
        }

        private static System.Drawing.Drawing2D.GraphicsPath RoundedRectangle(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void AuditGrid_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (_auditGrid.Columns.Contains("colRowNumber"))
                _auditGrid.Columns.Remove("colRowNumber");

            var rowNumber = new DataGridViewTextBoxColumn
            {
                Name = "colRowNumber",
                HeaderText = "STT",
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                FillWeight = 8,
                MinimumWidth = 55
            };
            _auditGrid.Columns.Insert(0, rowNumber);
            for (int i = 0; i < _auditGrid.Rows.Count; i++)
            {
                if (!_auditGrid.Rows[i].IsNewRow)
                    _auditGrid.Rows[i].Cells["colRowNumber"].Value = i + 1;
            }
            // Vietnamese column captions for the audit log.
            SetAuditColumnHeader("AuditLogId", "Mã nhật ký");
            SetAuditColumnHeader("Username", "Tài khoản");
            SetAuditColumnHeader("Action", "Hành động");
            SetAuditColumnHeader("Description", "Mô tả");
            SetAuditColumnHeader("CreatedAt", "Thời gian");

            if (_auditGrid.Columns.Contains("AuditLogId"))
                _auditGrid.Columns["AuditLogId"].FillWeight = 14;
            if (_auditGrid.Columns.Contains("Username"))
                _auditGrid.Columns["Username"].FillWeight = 14;
            if (_auditGrid.Columns.Contains("Action"))
                _auditGrid.Columns["Action"].FillWeight = 12;
            if (_auditGrid.Columns.Contains("Description"))
                _auditGrid.Columns["Description"].FillWeight = 42;
            if (_auditGrid.Columns.Contains("CreatedAt"))
                _auditGrid.Columns["CreatedAt"].FillWeight = 22;
        }

        private void SetAuditColumnHeader(string columnName, string headerText)
        {
            if (_auditGrid.Columns.Contains(columnName))
                _auditGrid.Columns[columnName].HeaderText = headerText;
        }

        private Label AddCompactMetric(Panel card, string caption, string value, int top)
        {
            var captionLabel = new Label
            {
                Text = caption,
                AutoSize = false,
                Location = new Point(17, top),
                Size = new Size(205, 27),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                ForeColor = TextColor,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent
            };
            var valueLabel = new Label
            {
                Text = value,
                AutoSize = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(Math.Max(175, card.Width - 62), top),
                Size = new Size(48, 27),
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Blue,
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = Color.Transparent
            };
            card.Controls.Add(captionLabel);
            card.Controls.Add(valueLabel);
            card.Resize += (s, e) =>
            {
                valueLabel.Left = Math.Max(175, card.ClientSize.Width - valueLabel.Width - 15);
                captionLabel.Width = Math.Max(90, valueLabel.Left - captionLabel.Left - 8);
            };
            return valueLabel;
        }

        private Label AddMetric(Panel card, string caption, string value, int top)
        {
            var captionLabel = new Label
            {
                Text = caption,
                AutoSize = false,
                Location = new Point(17, top),
                Size = new Size(205, 32),
                Font = new Font("Segoe UI", 11F),
                ForeColor = TextColor,
                TextAlign = ContentAlignment.MiddleLeft
            };
            var valueLabel = new Label
            {
                Text = value,
                AutoSize = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(Math.Max(175, card.Width - 62), top),
                Size = new Size(48, 32),
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 91, 190),
                TextAlign = ContentAlignment.MiddleRight
            };
            card.Controls.Add(captionLabel);
            card.Controls.Add(valueLabel);
            card.Resize += (s, e) =>
            {
                valueLabel.Left = Math.Max(175, card.ClientSize.Width - valueLabel.Width - 15);
                captionLabel.Width = Math.Max(100, valueLabel.Left - captionLabel.Left - 8);
            };
            return valueLabel;
        }

        // Data access is kept in this file so this is the only dashboard code file required.
        private sealed class DashboardSnapshot
        {
            public int TotalPersonnel { get; set; }
            public int SoldierCount { get; set; }
            public int OfficerCount { get; set; }
            public int TotalUsers { get; set; }
            public int AdminCount { get; set; }
            public int UserCount { get; set; }
            public int LockedUsers { get; set; }
            public int TotalMission { get; set; }
            public int ActiveMission { get; set; }
            public int EndingMission { get; set; }
            public int FinishedMission { get; set; }
            public int LoginToday { get; set; }
            public int FailedLoginToday { get; set; }
            public int LockToday { get; set; }
            public int ResetToday { get; set; }
            public int UnlockToday { get; set; }
            public int ChangePasswordToday { get; set; }
        }

        private async Task<DashboardSnapshot> GetDashboardDataAsync()
        {
            using (var db = new ApplicationDbContext())
            {
                var data = new DashboardSnapshot();
                data.TotalPersonnel = await db.Personnel.CountAsync();
                data.SoldierCount = await db.Personnel.CountAsync(x =>
                    (x.CapBacHam ?? "") == "Binh nhì" ||
                    (x.CapBacHam ?? "") == "Binh nhất" ||
                    (x.CapBacHam ?? "") == "Hạ sĩ");
                data.OfficerCount = data.TotalPersonnel - data.SoldierCount;

                data.TotalUsers = await db.Users.CountAsync();
                data.AdminCount = await db.Users.CountAsync(x => x.Role == "Admin");
                data.UserCount = await db.Users.CountAsync(x => x.Role == "User");
                data.LockedUsers = await db.Users.CountAsync(x =>
                    x.LockedUntil != null && x.LockedUntil > DateTime.Now);

                data.TotalMission = await db.Missions.CountAsync();
                data.ActiveMission = await db.Missions.CountAsync(x =>
                    x.StartTime <= DateTime.Now && x.EndTime >= DateTime.Now);
                data.FinishedMission = await db.Missions.CountAsync(x => x.EndTime < DateTime.Now);
                data.EndingMission = await db.Missions.CountAsync(x =>
                    EF.Functions.DateDiffDay(DateTime.Now, x.EndTime) <= 3 && x.EndTime >= DateTime.Now);

                DateTime today = DateTime.Today;
                data.LoginToday = await db.AuditLogs.CountAsync(x => x.Action == "LOGIN" && x.CreatedAt.Date == today);
                data.FailedLoginToday = await db.AuditLogs.CountAsync(x => x.Action == "FAILED_LOGIN" && x.CreatedAt.Date == today);
                data.LockToday = await db.AuditLogs.CountAsync(x => x.Action == "LOCK_ACCOUNT" && x.CreatedAt.Date == today);
                data.ResetToday = await db.AuditLogs.CountAsync(x => x.Action == "RESET_PASSWORD" && x.CreatedAt.Date == today);
                data.UnlockToday = await db.AuditLogs.CountAsync(x => x.Action == "UNLOCK_ACCOUNT" && x.CreatedAt.Date == today);
                data.ChangePasswordToday = await db.AuditLogs.CountAsync(x => x.Action == "CHANGE_PASSWORD" && x.CreatedAt.Date == today);
                return data;
            }
        }

        private async Task<List<AuditLog>> GetAllAuditLogsAsync()
        {
            using (var db = new ApplicationDbContext())
            {
                // Do not limit the query to the latest 10 rows; the grid scrolls through the full history.
                return await db.AuditLogs
                    .OrderByDescending(x => x.CreatedAt)
                    .ThenByDescending(x => x.AuditLogId)
                    .ToListAsync();
            }
        }

        private async Task LoadDashboardAsync()
        {
            var data = await GetDashboardDataAsync();

            // Keep the original labels in sync as well, in case other code relies on them.
            lblTotalPersonnel.Text = $"Tổng CBCS: {data.TotalPersonnel}";
            lblSoldierCount.Text = $"Chiến sĩ: {data.SoldierCount}";
            lblOfficerCount.Text = $"Cán bộ: {data.OfficerCount}";
            lblTotalUsers.Text = $"Tổng TK: {data.TotalUsers}";
            lblAdminCount.Text = $"Admin: {data.AdminCount}";
            lblUserCount.Text = $"User: {data.UserCount}";
            lblLockedUsers.Text = $"Đang khóa: {data.LockedUsers}";
            lblTotalMission.Text = $"Tổng CT: {data.TotalMission}";
            lblActiveMission.Text = $"Đang diễn ra: {data.ActiveMission}";
            lblEndingMission.Text = $"Sắp kết thúc: {data.EndingMission}";
            lblFinishedMission.Text = $"Đã kết thúc: {data.FinishedMission}";
            lblLoginToday.Text = $"LOGIN: {data.LoginToday}";
            lblFailedLoginToday.Text = $"FAILED LOGIN: {data.FailedLoginToday}";
            lblLockToday.Text = $"LOCK ACCOUNT: {data.LockToday}";
            lblResetToday.Text = $"RESET PASSWORD: {data.ResetToday}";
            lblUnlockToday.Text = $"UNLOCK ACCOUNT: {data.UnlockToday}";
            lblChangePasswordToday.Text = $"CHANGE PASSWORD: {data.ChangePasswordToday}";

            _lblPersonnelValue.Text = data.TotalPersonnel.ToString();
            _lblSoldierValue.Text = data.SoldierCount.ToString();
            _lblOfficerValue.Text = data.OfficerCount.ToString();
            _lblUsersValue.Text = data.TotalUsers.ToString();
            _lblAdminValue.Text = data.AdminCount.ToString();
            _lblUserValue.Text = data.UserCount.ToString();
            _lblLockedValue.Text = data.LockedUsers.ToString();
            _lblMissionValue.Text = data.TotalMission.ToString();
            _lblActiveMissionValue.Text = data.ActiveMission.ToString();
            _lblEndingMissionValue.Text = data.EndingMission.ToString();
            _lblFinishedMissionValue.Text = data.FinishedMission.ToString();
            _lblLoginValue.Text = data.LoginToday.ToString();
            _lblFailedLoginValue.Text = data.FailedLoginToday.ToString();
            _lblLockValue.Text = data.LockToday.ToString();
            _lblResetValue.Text = data.ResetToday.ToString();
            _lblUnlockValue.Text = data.UnlockToday.ToString();
            _lblChangePasswordValue.Text = data.ChangePasswordToday.ToString();
        }

        private async void FrmAdminDashboard_Load(object sender, EventArgs e)
        {
            await LoadDashboardAsync();
            dgvRecentLogs.DataSource = await GetAllAuditLogsAsync();
            dgvRecentLogs.ScrollBars = ScrollBars.Vertical;
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            btnRefresh.Enabled = false;
            try
            {
                await LoadDashboardAsync();
                dgvRecentLogs.DataSource = await GetAllAuditLogsAsync();
                dgvRecentLogs.ScrollBars = ScrollBars.Vertical;
            }
            finally
            {
                btnRefresh.Enabled = true;
            }
        }
    }
}