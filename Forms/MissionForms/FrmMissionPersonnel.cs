using QuanLyCBCS.DTOs;
using QuanLyCBCS.Helpers;
using QuanLyCBCS.Models;
using QuanLyCBCS.Services;

namespace QuanLyCBCS.Forms.MissionForms
{
    public partial class FrmMissionPersonnel : Form
    {
        private readonly int _missionId;

        private readonly MissionPersonnelService
            _missionPersonnelService =
                new();

        private List<MissionPersonnelDto>
            _availablePersonnel =
                new();

        private List<MissionPersonnelDto>
            _assignedPersonnel =
                new();

        public FrmMissionPersonnel(
            int missionId,
            string missionName)
        {
            InitializeComponent();

            CustomizeGrid();

            _missionId = missionId;

            lblMissionName.Text =
                $"Công tác: {missionName}";
        }

        private async Task LoadPersonnelAsync()
        {
            _availablePersonnel =
                await _missionPersonnelService
                    .GetAvailablePersonnelDtoAsync(
                        _missionId);

            _assignedPersonnel =
                await _missionPersonnelService
                    .GetAssignedPersonnelDtoAsync(
                        _missionId);

            dgvAvailable.DataSource = _availablePersonnel;
            dgvAssigned.DataSource = _assignedPersonnel;
            pnlAssignedEmpty.Visible = _assignedPersonnel.Count == 0;

            dgvAssigned.Columns[
                "colNgayPhanCongAssigned"]
                .DefaultCellStyle.Format =
                    "dd/MM/yyyy HH:mm";

            for (int i = 0; i < dgvAvailable.Rows.Count; i++)
            {
                dgvAvailable.Rows[i]
                    .Cells["colSttAvailable"]
                    .Value = i + 1;
            }

            for (int i = 0; i < dgvAssigned.Rows.Count; i++)
            {
                dgvAssigned.Rows[i]
                    .Cells["colSttAssigned"]
                    .Value = i + 1;
            }

            UpdateCounts();
        }

        private void UpdateCounts()
        {
            lblAvailableCount.Text =
                $"ⓘ  Chưa phân công: {_availablePersonnel.Count} CBCS";

            lblAssignedCount.Text =
                $"ⓘ  Đã phân công: {_assignedPersonnel.Count} CBCS";

            lblAvailableTitle.Text = $"CBCS CHƯA PHÂN CÔNG ({_availablePersonnel.Count})";
            lblAssignedTitle.Text = $"CBCS ĐÃ PHÂN CÔNG ({_assignedPersonnel.Count})";
        }

        private async void
        FrmMissionPersonnel_Load(
            object? sender,
            EventArgs e)
        {
            await LoadPersonnelAsync();
        }

        private void CustomizeGrid()
        {
            dgvAvailable.AutoGenerateColumns = false;
            dgvAssigned.AutoGenerateColumns = false;

            dgvAvailable.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvAssigned.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvAvailable.MultiSelect = false;
            dgvAssigned.MultiSelect = false;

            dgvAvailable.ReadOnly = true;
            dgvAssigned.ReadOnly = true;

            dgvAvailable.AllowUserToAddRows = false;
            dgvAssigned.AllowUserToAddRows = false;

            dgvAvailable.RowHeadersVisible = false;
            dgvAssigned.RowHeadersVisible = false;

            // Explicitly disable the native DataGridView frame. The rounded
            // RoundedPanel hosts provide the only visible outer border.
            dgvAvailable.BorderStyle = BorderStyle.None;
            dgvAssigned.BorderStyle = BorderStyle.None;
            dgvAvailable.Margin = Padding.Empty;
            dgvAssigned.Margin = Padding.Empty;
            dgvAvailable.BackgroundColor = Color.White;
            dgvAssigned.BackgroundColor = Color.White;
            dgvAvailable.Margin = Padding.Empty;
            dgvAssigned.Margin = Padding.Empty;
        }

        private async void btnAssign_Click(
            object sender,
            EventArgs e)
        {
            await AssignSelectedPersonnelAsync();
        }

        private async void btnRemove_Click(
            object sender,
            EventArgs e)
        {
            await RemoveSelectedPersonnelAsync();
        }

        private void txtSearch_TextChanged(
            object sender,
            EventArgs e)
        {
            string keyword =
                VietnameseHelper
                    .RemoveVietnameseSigns(
                        txtSearch.Text.Trim())
                    .ToLower();

            dgvAvailable.DataSource =
                _availablePersonnel
                .Where(x =>
                    VietnameseHelper.RemoveVietnameseSigns(x.HoTen)
                        .ToLower()
                        .Contains(keyword)
                    ||
                    VietnameseHelper.RemoveVietnameseSigns(x.SoHieu)
                        .ToLower()
                        .Contains(keyword)
                    ||
                    VietnameseHelper.RemoveVietnameseSigns(x.DonVi)
                        .ToLower()
                        .Contains(keyword)
                )
                .ToList();

            for (int i = 0; i < dgvAvailable.Rows.Count; i++)
            {
                dgvAvailable.Rows[i]
                    .Cells["colSttAvailable"]
                    .Value = i + 1;
            }
        }

        private async void
        dgvAvailable_CellDoubleClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            await AssignSelectedPersonnelAsync();
        }

        private async Task
        AssignSelectedPersonnelAsync()
        {
            if (dgvAvailable.CurrentRow?.DataBoundItem
                is not MissionPersonnelDto selected)
            {
                return;
            }

            try
            {
                await _missionPersonnelService
                    .AddAsync(
                        _missionId,
                        selected.PersonnelId);

                await AuditHelper.LogAsync(
                    AuditAction.AssignMission,
                    $"Phân công CBCS '{selected.HoTen}' ({selected.SoHieu}) vào {lblMissionName.Text}.");

                await LoadPersonnelAsync();
            }
            catch (Exception ex)
            {
                if (IsMissionEndedException(ex))
                {
                    using var dialog = new MissionEndedNotificationForm();
                    dialog.ShowDialog(this);
                }
                else if (IsPersonnelConflictException(ex))
                {
                    // Dùng cùng hộp thoại cảnh báo custom, giữ nguyên nghiệp vụ kiểm tra
                    // trùng lịch trong MissionPersonnelService.
                    using var dialog = new MissionEndedNotificationForm(
                        "CBCS đang tham gia công tác khác trong cùng thời gian.");
                    dialog.ShowDialog(this);
                }
                else
                {
                    MessageBox.Show(
                        ex.Message,
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
        }

        /// <summary>
        /// Nhận diện lỗi nghiệp vụ khi công tác đã kết thúc.
        /// Không thay đổi điều kiện phân công; chỉ chọn giao diện thông báo phù hợp.
        /// </summary>
        private static bool IsMissionEndedException(Exception exception)
        {
            for (Exception? current = exception; current != null; current = current.InnerException)
            {
                string message = current.Message ?? string.Empty;

                if (message.Contains("đã kết thúc", StringComparison.OrdinalIgnoreCase)
                    || message.Contains("không thể phân công", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Nhận diện lỗi nghiệp vụ khi CBCS đã được phân công vào công tác khác
        /// có thời gian giao nhau với công tác hiện tại. Chỉ đổi cách hiển thị.
        /// </summary>
        private static bool IsPersonnelConflictException(Exception exception)
        {
            for (Exception? current = exception; current != null; current = current.InnerException)
            {
                string message = current.Message ?? string.Empty;

                if (message.Contains("đang tham gia", StringComparison.OrdinalIgnoreCase)
                    || message.Contains("công tác khác", StringComparison.OrdinalIgnoreCase)
                    || message.Contains("trùng thời gian", StringComparison.OrdinalIgnoreCase)
                    || message.Contains("trùng khoảng thời gian", StringComparison.OrdinalIgnoreCase)
                    || message.Contains("chồng lấn", StringComparison.OrdinalIgnoreCase)
                    || message.Contains("thời gian bị trùng", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private async Task
        RemoveSelectedPersonnelAsync()
        {
            if (dgvAssigned.CurrentRow?
                    .DataBoundItem
                is not MissionPersonnelDto selected)
            {
                return;
            }

            await _missionPersonnelService
                .RemoveAsync(
                    _missionId,
                    selected.PersonnelId);

            await AuditHelper.LogAsync(
                AuditAction.RemoveMissionAssignment,
                $"Hủy phân công CBCS '{selected.HoTen}' ({selected.SoHieu}) khỏi {lblMissionName.Text}.");

            await LoadPersonnelAsync();
        }

        private async void
        dgvAssigned_CellDoubleClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            await RemoveSelectedPersonnelAsync();
        }

        private void
        btnClose_Click(
            object sender,
            EventArgs e)
        {
            Close();
        }
    }


    /// <summary>
    /// Hộp thoại cảnh báo theo thiết kế riêng cho trường hợp công tác đã kết thúc.
    /// </summary>
    internal sealed class MissionEndedNotificationForm : Form
    {
        private readonly Button _btnOk;
        private readonly Button _btnClose;
        private readonly Label _lblMessage;
        private readonly Panel _warningIcon;

        private const int CornerRadius = 14;
        private const int HeaderHeight = 54;
        private const int FooterHeight = 52;

        public MissionEndedNotificationForm()
            : this("Công tác đã kết thúc, không thể phân công.")
        {
        }

        public MissionEndedNotificationForm(string message)
        {
            Text = "Thông báo";
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            TopMost = false;
            ClientSize = new Size(500, 218);
            MinimumSize = new Size(500, 218);
            MaximumSize = new Size(500, 218);
            BackColor = Color.White;
            Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            DoubleBuffered = true;
            MinimizeBox = false;
            MaximizeBox = false;
            KeyPreview = true;

            // Nút đóng tự vẽ bằng Graphics để tránh lỗi font/ký tự × bị thay thế
            // thành glyph khác trên một số máy Windows hoặc cấu hình DPI.
            _btnClose = new Button
            {
                Text = string.Empty,
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                UseVisualStyleBackColor = false,
                TabStop = false,
                Size = new Size(36, 36),
                Location = new Point(ClientSize.Width - 45, 8),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            _btnClose.FlatAppearance.BorderSize = 0;
            _btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(35, 255, 255, 255);
            _btnClose.FlatAppearance.MouseDownBackColor = Color.FromArgb(65, 255, 255, 255);
            _btnClose.Paint += PaintCloseButton;
            _btnClose.Click += (_, _) => Close();
            Controls.Add(_btnClose);
            _btnClose.BringToFront();

            // Warning icon is painted in its own 56x56 coordinate space (no transform).
            _warningIcon = new Panel
            {
                Location = new Point(27, 83),
                Size = new Size(56, 56),
                BackColor = Color.Transparent
            };
            _warningIcon.Paint += PaintWarningIcon;
            Controls.Add(_warningIcon);

            _lblMessage = new Label
            {
                Text = message,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 12.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 39, 75),
                Location = new Point(116, 76),
                Size = new Size(356, 76),
                BackColor = Color.Transparent
            };
            Controls.Add(_lblMessage);

            _btnOk = new Button
            {
                Text = "✓  OK",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(0, 105, 215),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(118, 38),
                Location = new Point(ClientSize.Width - 137, ClientSize.Height - 46),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.OK
            };
            _btnOk.FlatAppearance.BorderColor = Color.White;
            _btnOk.FlatAppearance.BorderSize = 1;
            _btnOk.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 91, 190);
            _btnOk.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 78, 165);
            _btnOk.Resize += (_, _) => ApplyRoundedButtonRegion(_btnOk, 10);
            Controls.Add(_btnOk);

            AcceptButton = _btnOk;
            CancelButton = _btnClose;
            KeyDown += (_, e) =>
            {
                if (e.KeyCode == Keys.Escape)
                    Close();
            };
        }

        protected override CreateParams CreateParams
        {
            get
            {
                const int CS_DROPSHADOW = 0x00020000;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            ApplyWindowRegion();
            ApplyRoundedButtonRegion(_btnOk, 10);
            _btnClose.BringToFront();
            _btnOk.Focus();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ApplyWindowRegion();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            Rectangle bounds = new Rectangle(0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
            using (var windowPath = RoundedRectangle(bounds, CornerRadius))
            using (var whiteBrush = new SolidBrush(Color.White))
            using (var borderPen = new Pen(Color.FromArgb(130, 174, 225), 1F))
            {
                g.FillPath(whiteBrush, windowPath);
                g.DrawPath(borderPen, windowPath);
            }

            // Header: vẽ trực tiếp theo kích thước thật của cửa sổ.
            Rectangle headerRect = new Rectangle(1, 1, ClientSize.Width - 2, HeaderHeight + 8);
            using (var headerPath = TopRoundedRectangle(headerRect, CornerRadius))
            using (var headerBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
                headerRect,
                Color.FromArgb(0, 96, 190),
                Color.FromArgb(0, 75, 158),
                0F))
            {
                g.FillPath(headerBrush, headerPath);
            }

            // Icon thông tin: vòng tròn và chữ i cùng một tâm hình học.
            const int infoSize = 32;
            Rectangle infoRect = new Rectangle(21, 11, infoSize, infoSize);
            using (var infoBrush = new SolidBrush(Color.White))
            using (var infoFont = new Font("Segoe UI", 20F, FontStyle.Bold))
            using (var infoTextBrush = new SolidBrush(Color.FromArgb(0, 86, 174)))
            using (var sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            })
            {
                g.FillEllipse(infoBrush, infoRect);
                g.DrawString("i", infoFont, infoTextBrush,
                    new RectangleF(infoRect.X, infoRect.Y - 1, infoRect.Width, infoRect.Height), sf);
            }

            using (var titleFont = new Font("Segoe UI", 19F, FontStyle.Bold))
            using (var titleBrush = new SolidBrush(Color.White))
            {
                g.DrawString("Thông báo", titleFont, titleBrush, new PointF(66, 12));
            }

            // Footer xanh nhạt, nằm đúng trong 52 px cuối của form.
            Rectangle footerRect = new Rectangle(1, ClientSize.Height - FooterHeight - 1,
                ClientSize.Width - 2, FooterHeight);
            using (var footerPath = BottomRoundedRectangle(footerRect, CornerRadius))
            using (var footerBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
                footerRect,
                Color.FromArgb(237, 246, 255),
                Color.FromArgb(222, 238, 255),
                90F))
            {
                g.FillPath(footerBrush, footerPath);
            }

            // Divider canh giữa chiều cao vùng nội dung và thẳng hàng với icon cảnh báo.
            using (var separatorPen = new Pen(Color.FromArgb(0, 112, 225), 1.2F))
            {
                g.DrawLine(separatorPen, 101, 78, 101, 151);
            }

            using (var borderPath = RoundedRectangle(bounds, CornerRadius))
            using (var borderPen = new Pen(Color.FromArgb(130, 174, 225), 1F))
            {
                g.DrawPath(borderPen, borderPath);
            }
        }

        private void PaintCloseButton(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            // Hai nét chéo cân đối tạo thành dấu X, luôn nằm chính giữa nút.
            float centerX = _btnClose.ClientSize.Width / 2F;
            float centerY = _btnClose.ClientSize.Height / 2F;
            const float halfLength = 8F;
            using (var pen = new Pen(Color.White, 2.8F))
            {
                pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                g.DrawLine(pen, centerX - halfLength, centerY - halfLength,
                    centerX + halfLength, centerY + halfLength);
                g.DrawLine(pen, centerX + halfLength, centerY - halfLength,
                    centerX - halfLength, centerY + halfLength);
            }
        }

        private void PaintWarningIcon(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            // Tam giác cân được căn chính giữa panel 56x56.
            PointF[] triangle =
            {
                new PointF(28, 3),
                new PointF(53, 51),
                new PointF(3, 51)
            };

            using (var path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                path.AddPolygon(triangle);
                using (var fill = new System.Drawing.Drawing2D.LinearGradientBrush(
                    new Rectangle(3, 3, 50, 48),
                    Color.FromArgb(255, 220, 0),
                    Color.FromArgb(255, 166, 0),
                    90F))
                using (var outline = new Pen(Color.FromArgb(230, 158, 0), 2F))
                {
                    g.FillPath(fill, path);
                    g.DrawPath(outline, path);
                }
            }

            using (var exclamationFont = new Font("Segoe UI", 23F, FontStyle.Bold))
            using (var exclamationBrush = new SolidBrush(Color.FromArgb(35, 35, 25)))
            using (var sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            })
            {
                g.DrawString("!", exclamationFont, exclamationBrush,
                    new RectangleF(15, 8, 26, 42), sf);
            }
        }

        private void ApplyWindowRegion()
        {
            if (ClientSize.Width <= 0 || ClientSize.Height <= 0)
                return;

            using (var path = RoundedRectangle(
                new Rectangle(0, 0, ClientSize.Width, ClientSize.Height), CornerRadius))
            {
                Region = new Region(path);
            }
        }

        private static void ApplyRoundedButtonRegion(Button button, int radius)
        {
            if (button.Width <= 0 || button.Height <= 0)
                return;

            using (var path = RoundedRectangle(
                new Rectangle(0, 0, button.Width, button.Height), radius))
            {
                button.Region = new Region(path);
            }
        }

        private static System.Drawing.Drawing2D.GraphicsPath RoundedRectangle(Rectangle rect, int radius)
        {
            int d = radius * 2;
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static System.Drawing.Drawing2D.GraphicsPath TopRoundedRectangle(Rectangle rect, int radius)
        {
            int d = radius * 2;
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddLine(rect.Right, rect.Bottom, rect.X, rect.Bottom);
            path.CloseFigure();
            return path;
        }

        private static System.Drawing.Drawing2D.GraphicsPath BottomRoundedRectangle(Rectangle rect, int radius)
        {
            int d = radius * 2;
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddLine(rect.X, rect.Y, rect.Right, rect.Y);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

}
