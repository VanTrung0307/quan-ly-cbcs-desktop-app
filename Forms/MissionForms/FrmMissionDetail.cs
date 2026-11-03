using QuanLyCBCS.Helpers;
using QuanLyCBCS.Models;
using QuanLyCBCS.Services;
using QuanLyCBCS.Common;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace QuanLyCBCS.Forms.MissionForms
{
    public partial class FrmMissionDetail : Form
    {
        private readonly MissionService
            _missionService =
                new();

        private Mission? _mission;

        public FrmMissionDetail()
        {
            InitializeComponent();
            ApplyModernWindowStyle();

            SetupMissionType();

            LoadPriority();

            chkIsActive.Checked = true;
        }

        public FrmMissionDetail(
            Mission mission)
        {
            InitializeComponent();
            ApplyModernWindowStyle();

            SetupMissionType();

            LoadPriority();

            _mission = mission;

            LoadMissionData();
        }


        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

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

        private void ApplyModernWindowStyle()
        {
            // Prevent WinForms font-based autoscaling from shrinking the 800x530
            // reference layout on machines with a different default dialog font.
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(920, 570);
            Size = new Size(920, 570);
            MinimumSize = new Size(920, 570);
            MaximumSize = new Size(920, 570);
            DoubleBuffered = true;

            ApplyReferenceDesign();
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;

            pnlHeader.MouseDown += Header_MouseDown;
            lblTitle.MouseDown += Header_MouseDown;
            lblHeaderIcon.MouseDown += Header_MouseDown;
            Resize += (_, _) => ApplyRoundedRegion();
            Shown += (_, _) => ApplyRoundedRegion();
            ApplyRoundedRegion();
        }

        private void ApplyReferenceDesign()
        {
            // Header: blue gradient and a clean briefcase glyph.
            pnlHeader.Paint += (_, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var brush = new LinearGradientBrush(
                    pnlHeader.ClientRectangle,
                    Color.FromArgb(0, 105, 213),
                    Color.FromArgb(0, 72, 157),
                    LinearGradientMode.Horizontal);
                e.Graphics.FillRectangle(brush, pnlHeader.ClientRectangle);
            };

            lblHeaderIcon.Text = string.Empty;
            lblHeaderIcon.BackColor = Color.FromArgb(0, 111, 224);
            lblHeaderIcon.Text = string.Empty;
            lblHeaderIcon.Paint += (_, e) => DrawBriefcaseIcon(e.Graphics, lblHeaderIcon.ClientRectangle);
            lblHeaderIcon.Resize += (_, _) => ApplyRoundedControlRegion(lblHeaderIcon, 7);
            ApplyRoundedControlRegion(lblHeaderIcon, 7);

            lblTitle.BackColor = Color.Transparent;
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitle.Location = new Point(95, 14);

            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.BackColor = Color.Transparent;
            btnClose.Font = new Font("Segoe UI Symbol", 22F, FontStyle.Regular, GraphicsUnit.Point);
            btnClose.Text = "×";

            pnlBody.BackColor = Color.FromArgb(247, 250, 255);
            pnlFooter.BackColor = Color.FromArgb(230, 242, 255);

            StyleRoundedField(txtMissionName, 5);
            StyleRoundedField(txtLocation, 5);
            StyleRoundedField(cboMissionType, 5);
            StyleRoundedField(cboPriority, 5);
            StyleRoundedField(rtbDescription, 7);

            // Custom date/time controls: the rounded outline, text area and calendar
            // button are painted as one surface (no native Windows rectangular frame).
            ConfigureDateTimeField(dtpStartTime);
            ConfigureDateTimeField(dtpEndTime);

            chkIsActive.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            chkIsActive.ForeColor = Color.FromArgb(23, 48, 78);

            ConfigureReferenceLabel(lblMissionName, "pencil");
            ConfigureReferenceLabel(lblMissionType, "layers");
            ConfigureReferenceLabel(lblStart, "calendar");
            ConfigureReferenceLabel(lblEnd, "calendar");
            ConfigureReferenceLabel(lblLocation, "pin");
            ConfigureReferenceLabel(lblDescription, "description");
            ConfigureReferenceLabel(lblPriority, "flag");

            StyleActionButton(btnSave, Color.FromArgb(0, 166, 91), Color.White, 10);
            ConfigureSaveButton(btnSave);
            StyleActionButton(btnCancel, Color.White, Color.FromArgb(225, 35, 48), 10);
        }

        private static void ConfigureDateTimeField(MissionDateTimePicker picker)
        {
            picker.CustomFormat = "dd/MM/yyyy HH:mm";
            picker.Format = DateTimePickerFormat.Custom;
            picker.CalendarMonthBackground = Color.White;
            picker.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
            picker.BorderColor = Color.FromArgb(190, 216, 248);
            picker.BorderRadius = 9;
            picker.ButtonBackColor = Color.FromArgb(232, 242, 255);
            picker.IconColor = Color.FromArgb(0, 103, 210);
        }

        private static void ConfigureReferenceLabel(Label label, string iconName)
        {
            string current = label.Text ?? string.Empty;
            int separator = current.IndexOf("  ", StringComparison.Ordinal);
            if (separator >= 0)
                current = current[(separator + 2)..];
            else if (current.Length > 1 && current[1] == ' ')
                current = current[2..];

            label.Text = string.Empty;
            label.BackColor = Color.Transparent;
            label.Padding = Padding.Empty;
            label.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            label.ForeColor = Color.FromArgb(23, 48, 78);
            label.Paint += (_, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var iconPen = new Pen(Color.FromArgb(0, 103, 210), 1.8F)
                {
                    StartCap = LineCap.Round,
                    EndCap = LineCap.Round,
                    LineJoin = LineJoin.Round
                };
                using var iconBrush = new SolidBrush(Color.FromArgb(0, 103, 210));
                DrawFieldIcon(e.Graphics, iconName, iconPen, iconBrush);

                string plainText = current.EndsWith("*", StringComparison.Ordinal)
                    ? current[..^1]
                    : current;
                using var textBrush = new SolidBrush(Color.FromArgb(23, 48, 78));
                using var textFont = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
                SizeF textSize = e.Graphics.MeasureString(plainText, textFont);
                float textY = Math.Max(0, (label.Height - textSize.Height) / 2F - 1F);
                e.Graphics.DrawString(plainText, textFont, textBrush, new PointF(22, textY));

                if (current.EndsWith("*", StringComparison.Ordinal))
                {
                    using var requiredBrush = new SolidBrush(Color.FromArgb(220, 45, 55));
                    e.Graphics.DrawString("*", textFont, requiredBrush,
                        new PointF(22 + textSize.Width - 1, textY));
                }
            };
            label.Invalidate();
        }

        private static void DrawFieldIcon(Graphics g, string name, Pen pen, Brush brush)
        {
            // All icons are drawn as compact blue vector artwork to match the reference.
            switch (name)
            {
                case "pencil":
                    g.DrawLine(pen, 3, 17, 14, 6);
                    g.DrawLine(pen, 2, 19, 5, 18);
                    g.DrawLine(pen, 13, 5, 16, 8);
                    g.DrawLine(pen, 11, 7, 14, 10);
                    break;

                case "layers":
                    PointF[] layer1 = { new(2, 9), new(10, 4), new(18, 9), new(10, 14) };
                    PointF[] layer2 = { new(2, 13), new(10, 18), new(18, 13) };
                    g.FillPolygon(brush, layer1);
                    g.DrawLines(pen, layer2);
                    break;

                case "calendar":
                    g.DrawRectangle(pen, 2, 5, 16, 13);
                    g.DrawLine(pen, 2, 9, 18, 9);
                    g.DrawLine(pen, 6, 3, 6, 7);
                    g.DrawLine(pen, 14, 3, 14, 7);
                    for (int x = 6; x <= 14; x += 4)
                        for (int y = 12; y <= 16; y += 4)
                            g.FillRectangle(brush, x, y, 1.6F, 1.6F);
                    break;

                case "pin":
                    using (var pinPath = new GraphicsPath())
                    {
                        pinPath.AddEllipse(3, 2, 14, 14);
                        pinPath.AddLine(5, 13, 10, 21);
                        pinPath.AddLine(15, 13, 10, 21);
                        pinPath.CloseFigure();
                        g.FillPath(brush, pinPath);
                    }
                    using (var hole = new SolidBrush(Color.FromArgb(247, 250, 255)))
                        g.FillEllipse(hole, 8, 7, 4, 4);
                    break;

                case "flag":
                    g.DrawLine(pen, 4, 3, 4, 20);
                    PointF[] flag = { new(5, 4), new(17, 6), new(14, 12), new(5, 10) };
                    g.FillPolygon(brush, flag);
                    break;

                case "description":
                    g.DrawRectangle(pen, 3, 3, 14, 17);
                    g.DrawLine(pen, 6, 8, 14, 8);
                    g.DrawLine(pen, 6, 12, 14, 12);
                    g.DrawLine(pen, 6, 16, 12, 16);
                    break;
            }
        }

        private static void ApplyRoundedControlRegion(Control control, int radius)
        {
            if (control.Width <= 0 || control.Height <= 0) return;
            using GraphicsPath path = CreateRoundedPath(
                new Rectangle(0, 0, control.Width, control.Height), radius);
            control.Region?.Dispose();
            control.Region = new Region(path);
        }

        private void StyleRoundedField(Control control, int radius)
        {
            if (control.Parent == null) return;

            Control parent = control.Parent;
            Point originalLocation = control.Location;
            Size originalSize = control.Size;

            var frame = new RoundedFieldPanel
            {
                Location = originalLocation,
                Size = new Size(originalSize.Width + 2, originalSize.Height + 4),
                BackColor = Color.White,
                Radius = radius,
                BorderColor = Color.FromArgb(190, 216, 248),
                BorderThickness = 1
            };

            parent.Controls.Remove(control);
            parent.Controls.Add(frame);
            frame.BringToFront();
            frame.Controls.Add(control);

            control.Location = new Point(5, Math.Max(1, (frame.Height - control.Height) / 2));
            control.Size = new Size(frame.Width - 10, Math.Max(control.Height, frame.Height - 4));

            if (control is TextBox textBox)
            {
                textBox.BorderStyle = BorderStyle.None;
                textBox.BackColor = Color.White;
                textBox.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
            }
            else if (control is ComboBox comboBox)
            {
                comboBox.FlatStyle = FlatStyle.Flat;
                comboBox.BackColor = Color.White;
                comboBox.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
            }
            else if (control is DateTimePicker dateTimePicker)
            {
                dateTimePicker.CalendarMonthBackground = Color.White;
                dateTimePicker.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
                dateTimePicker.Dock = DockStyle.Fill;
                dateTimePicker.Margin = Padding.Empty;
            }
            else if (control is RichTextBox richTextBox)
            {
                richTextBox.BorderStyle = BorderStyle.None;
                richTextBox.BackColor = Color.White;
                richTextBox.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
            }
        }

        private static void StyleActionButton(Button button, Color backColor, Color foreColor, int radius)
        {
            button.BackColor = backColor;
            button.ForeColor = foreColor;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseDownBackColor =
                foreColor == Color.White ? Color.FromArgb(0, 135, 74) : Color.FromArgb(255, 235, 237);
            button.FlatAppearance.MouseOverBackColor =
                foreColor == Color.White ? Color.FromArgb(0, 150, 82) : Color.FromArgb(255, 245, 246);
            button.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            ApplyRoundedControlRegion(button, radius);
            button.Resize += (_, _) => ApplyRoundedControlRegion(button, radius);

            if (foreColor != Color.White)
            {
                button.Paint += (_, e) =>
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    using var path = CreateRoundedPath(
                        new Rectangle(1, 1, button.Width - 3, button.Height - 3), radius);
                    using var borderPen = new Pen(Color.FromArgb(235, 52, 65), 1.25F);
                    e.Graphics.DrawPath(borderPen, path);
                };
            }
        }

        private static void ConfigureSaveButton(Button button)
        {
            // Draw a crisp white floppy-disk glyph instead of the fallback square character.
            button.Text = string.Empty;
            button.AccessibleName = "Lưu";
            button.Paint += (_, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                const float iconWidth = 17F;
                const float iconHeight = 17F;
                using var pen = new Pen(Color.White, 1.8F)
                {
                    StartCap = LineCap.Round,
                    EndCap = LineCap.Round,
                    LineJoin = LineJoin.Round
                };
                using var fill = new SolidBrush(Color.White);
                using var cutout = new SolidBrush(Color.FromArgb(0, 166, 91));

                using var textFont = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
                SizeF textSize = e.Graphics.MeasureString("Lưu", textFont);
                float gap = 8F;
                float totalWidth = iconWidth + gap + textSize.Width;
                float startX = (button.ClientSize.Width - totalWidth) / 2F;
                float top = (button.ClientSize.Height - iconHeight) / 2F;

                RectangleF disk = new(startX, top, iconWidth, iconHeight);
                e.Graphics.DrawRectangle(pen, disk.X + 1, disk.Y + 1, disk.Width - 2, disk.Height - 2);
                e.Graphics.DrawLine(pen, disk.X + 3, disk.Y + 1, disk.X + 3, disk.Y + 6);
                e.Graphics.DrawLine(pen, disk.X + 3, disk.Y + 6, disk.X + 12, disk.Y + 6);
                e.Graphics.DrawRectangle(pen, disk.X + 4, disk.Y + 10, 9, 5);
                e.Graphics.FillRectangle(cutout, disk.X + 5, disk.Y + 11, 7, 3);
                e.Graphics.DrawString("Lưu", textFont, fill,
                    new PointF(startX + iconWidth + gap, (button.ClientSize.Height - textSize.Height) / 2F - 1F));
            };
        }

        private static GraphicsPath CreateRoundedPath(Rectangle bounds, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(bounds.Left, bounds.Top, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Top, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.Left, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static void DrawBriefcaseIcon(Graphics graphics, Rectangle bounds)
        {
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var pen = new Pen(Color.White, 2.4F);
            int x = 9, y = 17, w = bounds.Width - 18, h = 19;
            graphics.DrawArc(pen, x + 10, y - 8, w - 20, 16, 180, 180);
            graphics.DrawLine(pen, x, y, x + w, y);
            graphics.DrawLine(pen, x, y, x, y + h);
            graphics.DrawLine(pen, x + w, y, x + w, y + h);
            graphics.DrawLine(pen, x, y + h, x + w, y + h);
            graphics.DrawRectangle(pen, bounds.Width / 2 - 5, y - 2, 10, 5);
        }

        private void ApplyRoundedRegion()
        {
            if (ClientSize.Width <= 0 || ClientSize.Height <= 0) return;

            const int radius = 16;
            using GraphicsPath path = CreateRoundedPath(
                new Rectangle(0, 0, ClientSize.Width, ClientSize.Height), radius);
            Region?.Dispose();
            Region = new Region(path);
        }

        private void Header_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, (IntPtr)HTCAPTION, IntPtr.Zero);
        }

        private void SetupMissionType()
        {
            cboMissionType.Items.Clear();

            cboMissionType.Items.AddRange(
            [
                MissionTypes.DutyCommander,
                MissionTypes.DutyOfficer,

                MissionTypes.TargetProtection,

                MissionTypes.Patrol,

                MissionTypes.SSCD,

                MissionTypes.Training,

                MissionTypes.Meeting,

                MissionTypes.Other,

                MissionTypes.Guard,

                MissionTypes.Escort,

                MissionTypes.FireSupport,

                MissionTypes.TrafficSupport
            ]);

            cboMissionType.SelectedIndex = 0;
        }

        private void LoadMissionData()
        {
            if (_mission == null)
                return;

            txtMissionName.Text =
                _mission.MissionName;

            cboMissionType.Text =
                _mission.MissionType;

            txtLocation.Text =
                _mission.Location;

            rtbDescription.Text =
                _mission.Description;

            dtpStartTime.Value =
                _mission.StartTime;

            dtpEndTime.Value =
                _mission.EndTime;

            chkIsActive.Checked =
                _mission.IsActive;

            cboPriority.SelectedValue =
                _mission.Priority;
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(
                txtMissionName.Text))
            {
                MessageBox.Show(
                    "Vui lòng nhập tên công tác.");

                txtMissionName.Focus();

                return false;
            }

            if (dtpEndTime.Value
                <= dtpStartTime.Value)
            {
                MessageBox.Show(
                    "Thời gian kết thúc phải lớn hơn thời gian bắt đầu.");

                dtpEndTime.Focus();

                return false;
            }

            return true;
        }

        private async Task SaveAsync()
        {
            if (!ValidateInput())
                return;

            if (_mission == null)
            {
                Mission mission =
                    new Mission();

                mission.MissionName =
                    txtMissionName.Text.Trim();

                mission.MissionType =
                    cboMissionType.Text;

                mission.Location =
                    txtLocation.Text.Trim();

                mission.Description =
                    rtbDescription.Text.Trim();

                mission.StartTime =
                    dtpStartTime.Value;

                mission.EndTime =
                    dtpEndTime.Value;

                mission.IsActive =
                    chkIsActive.Checked;

                mission.Priority =
                    GetSelectedPriority();

                await _missionService
                    .AddAsync(mission);

                await AuditHelper.LogAsync(
                    AuditAction.AddMission,
                    $"Thêm công tác '{mission.MissionName}' tại '{mission.Location}'.");
            }
            else
            {
                _mission.MissionName =
                    txtMissionName.Text.Trim();

                _mission.MissionType =
                    cboMissionType.Text;

                _mission.Location =
                    txtLocation.Text.Trim();

                _mission.Description =
                    rtbDescription.Text.Trim();

                _mission.StartTime =
                    dtpStartTime.Value;

                _mission.EndTime =
                    dtpEndTime.Value;

                _mission.IsActive =
                    chkIsActive.Checked;

                _mission.Priority =
                    GetSelectedPriority();

                await _missionService
                    .UpdateAsync(_mission);

                await AuditHelper.LogAsync(
                    AuditAction.UpdateMission,
                    $"Cập nhật công tác '{_mission.MissionName}' tại '{_mission.Location}'.");
            }

            DialogResult =
                DialogResult.OK;
        }

        private async void btnSave_Click(
            object sender,
            EventArgs e)
        {
            await SaveAsync();
        }

        private void btnCancel_Click(
            object sender,
            EventArgs e)
        {
            Close();
        }

        public class PriorityItem
        {
            public int Value { get; set; }

            public string Text { get; set; } = "";

            public override string ToString()
            {
                return Text;
            }
        }

        private void LoadPriority()
        {
            cboPriority.DataSource = new List<PriorityItem>
            {
                new PriorityItem
                {
                    Value = 1,
                    Text = "1 - Thấp"
                },

                new PriorityItem
                {
                    Value = 2,
                    Text = "2 - Bình thường"
                },

                new PriorityItem
                {
                    Value = 3,
                    Text = "3 - Cao"
                },

                new PriorityItem
                {
                    Value = 4,
                    Text = "4 - Khẩn"
                }
            };

            cboPriority.DisplayMember = nameof(PriorityItem.Text);
            cboPriority.ValueMember = nameof(PriorityItem.Value);

            cboPriority.SelectedValue = 2;
        }

        private int GetSelectedPriority()
        {
            if (cboPriority.SelectedValue is int priority)
                return priority;

            return 2;
        }

    }

    internal sealed class RoundedFieldPanel : Panel
    {
        public int Radius { get; set; } = 6;
        public Color BorderColor { get; set; } = Color.FromArgb(190, 216, 248);
        public float BorderThickness { get; set; } = 1F;

        public RoundedFieldPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw, true);
            Padding = new Padding(2);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var rect = new RectangleF(
                BorderThickness / 2F,
                BorderThickness / 2F,
                Width - BorderThickness,
                Height - BorderThickness);
            using var path = new GraphicsPath();
            float d = Radius * 2F;
            path.AddArc(rect.Left, rect.Top, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Top, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.Left, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            using var fill = new SolidBrush(Color.White);
            using var pen = new Pen(BorderColor, BorderThickness);
            e.Graphics.FillPath(fill, path);
            e.Graphics.DrawPath(pen, path);
        }
    }
}


/// <summary>
/// Native DateTimePicker with its square client-edge removed.
/// The surrounding RoundedFieldPanel supplies the rounded outline while
/// the native drop-down calendar and keyboard behaviour remain intact.
/// </summary>
namespace QuanLyCBCS.Forms.MissionForms
{
    public class RoundedDateTimePicker : DateTimePicker
    {
        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_EX_CLIENTEDGE = 0x00000200;
                CreateParams cp = base.CreateParams;
                cp.ExStyle &= ~WS_EX_CLIENTEDGE;
                return cp;
            }
        }
    }
}


namespace QuanLyCBCS.Forms.MissionForms
{
    /// <summary>
    /// Custom rounded date/time input matching the mission form reference design.
    /// Uses a text field and a dedicated calendar button instead of the native
    /// DateTimePicker chrome, which otherwise draws a rectangular inner border.
    /// </summary>
    public sealed class MissionDateTimePicker : UserControl
    {
        private readonly TextBox _textBox;
        private readonly Button _calendarButton;
        private readonly MonthCalendar _calendar;
        private readonly ToolStripDropDown _calendarPopup;
        private DateTime _value = DateTime.Now;
        private string _customFormat = "dd/MM/yyyy HH:mm";
        private DateTimePickerFormat _format = DateTimePickerFormat.Custom;
        private Color _calendarMonthBackground = Color.White;

        public event EventHandler? ValueChanged;

        public Color BorderColor { get; set; } = Color.FromArgb(190, 216, 248);
        public int BorderRadius { get; set; } = 9;
        public Color ButtonBackColor { get; set; } = Color.FromArgb(232, 242, 255);
        public Color IconColor { get; set; } = Color.FromArgb(0, 103, 210);

        public string CustomFormat
        {
            get => _customFormat;
            set { _customFormat = string.IsNullOrWhiteSpace(value) ? "dd/MM/yyyy HH:mm" : value; UpdateText(); }
        }

        public DateTimePickerFormat Format
        {
            get => _format;
            set { _format = value; UpdateText(); }
        }

        public Color CalendarMonthBackground
        {
            get => _calendarMonthBackground;
            set { _calendarMonthBackground = value; _calendar.BackColor = value; }
        }

        public DateTime Value
        {
            get => _value;
            set
            {
                DateTime normalized = value;
                if (_value == normalized) { UpdateText(); return; }
                _value = normalized;
                UpdateText();
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public override Color BackColor
        {
            get => base.BackColor;
            set => base.BackColor = value;
        }

        public MissionDateTimePicker()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            BackColor = Color.White;
            Size = new Size(194, 36);
            MinimumSize = new Size(150, 32);
            TabStop = true;

            _textBox = new TextBox
            {
                BorderStyle = BorderStyle.None,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(25, 45, 70),
                Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point),
                Location = new Point(9, 8),
                Width = Width - 55,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                TabStop = true
            };
            _textBox.Leave += (_, _) => CommitText();
            _textBox.KeyDown += TextBox_KeyDown;
            Controls.Add(_textBox);

            _calendarButton = new Button
            {
                FlatStyle = FlatStyle.Flat,
                BackColor = ButtonBackColor,
                ForeColor = IconColor,
                Size = new Size(39, 28),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Cursor = Cursors.Hand,
                TabStop = false,
                Text = string.Empty
            };
            _calendarButton.FlatAppearance.BorderSize = 0;
            _calendarButton.Location = new Point(Width - 43, 4);
            _calendarButton.Paint += CalendarButton_Paint;
            _calendarButton.Click += (_, _) => ShowCalendar();
            Controls.Add(_calendarButton);

            _calendar = new MonthCalendar
            {
                MaxSelectionCount = 1,
                BackColor = _calendarMonthBackground,
                Font = new Font("Segoe UI", 9F)
            };
            _calendar.DateSelected += (_, e) =>
            {
                Value = e.Start.Date.Add(_value.TimeOfDay);
                _calendarPopup?.Close();
            };
            _calendarPopup = new ToolStripDropDown { Padding = Padding.Empty, AutoClose = true };
            _calendarPopup.Items.Add(new ToolStripControlHost(_calendar)
            {
                Margin = Padding.Empty,
                Padding = new Padding(4),
                AutoSize = false,
                Size = new Size(_calendar.Width + 8, _calendar.Height + 8)
            });

            Resize += (_, _) => LayoutChildren();
            FontChanged += (_, _) => _textBox.Font = Font;
            UpdateText();
            LayoutChildren();
        }

        private void LayoutChildren()
        {
            if (_textBox == null || _calendarButton == null) return;
            _calendarButton.Size = new Size(39, Math.Max(26, Height - 8));
            _calendarButton.Location = new Point(Width - 43, 4);
            _calendarButton.Region?.Dispose();
            using (var buttonPath = RoundedPath(new Rectangle(0, 0, _calendarButton.Width, _calendarButton.Height), 7))
                _calendarButton.Region = new Region(buttonPath);
            _textBox.Location = new Point(10, Math.Max(5, (Height - _textBox.PreferredHeight) / 2));
            _textBox.Width = Math.Max(30, _calendarButton.Left - _textBox.Left - 7);
            Invalidate();
        }

        private void UpdateText()
        {
            if (_textBox == null) return;
            string format = _format == DateTimePickerFormat.Short ? "d" :
                            _format == DateTimePickerFormat.Time ? "t" : _customFormat;
            _textBox.Text = _value.ToString(format, System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));
        }

        private void CommitText()
        {
            if (DateTime.TryParseExact(_textBox.Text.Trim(),
                new[] { _customFormat, "dd/MM/yyyy HH:mm", "d/M/yyyy H:mm", "dd/MM/yyyy" },
                System.Globalization.CultureInfo.GetCultureInfo("vi-VN"),
                System.Globalization.DateTimeStyles.None, out DateTime parsed))
            {
                if (_textBox.Text.Trim().Length <= 10)
                    parsed = parsed.Date.Add(_value.TimeOfDay);
                Value = parsed;
            }
            else UpdateText();
        }

        private void TextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CommitText();
                e.SuppressKeyPress = true;
                Parent?.SelectNextControl(this, true, true, true, true);
            }
            else if (e.KeyCode == Keys.Escape) UpdateText();
        }

        private void ShowCalendar()
        {
            _calendar.SetDate(_value.Date);
            Point screen = _calendarButton.PointToScreen(new Point(0, _calendarButton.Height));
            _calendarPopup.Show(screen);
        }

        private void CalendarButton_Paint(object? sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var pen = new Pen(IconColor, 1.8F) { StartCap = LineCap.Round, EndCap = LineCap.Round };
            using var fill = new SolidBrush(IconColor);
            int cx = _calendarButton.Width / 2;
            int cy = _calendarButton.Height / 2;
            var rect = new Rectangle(cx - 8, cy - 7, 16, 15);
            e.Graphics.DrawRectangle(pen, rect);
            e.Graphics.DrawLine(pen, rect.Left, cy - 2, rect.Right, cy - 2);
            e.Graphics.DrawLine(pen, cx - 4, cy - 10, cx - 4, cy - 5);
            e.Graphics.DrawLine(pen, cx + 4, cy - 10, cx + 4, cy - 5);
            using var gridPen = new Pen(IconColor, 1.1F);
            e.Graphics.DrawLine(gridPen, cx - 4, cy + 2, cx + 4, cy + 2);
            e.Graphics.DrawLine(gridPen, cx - 4, cy + 5, cx + 4, cy + 5);
        }

        private static GraphicsPath RoundedPath(Rectangle bounds, int radius)
        {
            var path = new GraphicsPath();
            int d = Math.Max(2, radius * 2);
            path.AddArc(bounds.Left, bounds.Top, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Top, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.Left, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            float inset = 0.8F;
            var bounds = new RectangleF(inset, inset, Width - 1.6F, Height - 1.6F);
            float diameter = BorderRadius * 2F;
            using var path = new GraphicsPath();
            path.AddArc(bounds.Left, bounds.Top, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Top, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.Left, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            using var border = new Pen(BorderColor, 1.3F);
            e.Graphics.DrawPath(border, path);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _calendarPopup?.Dispose();
                _calendar?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}