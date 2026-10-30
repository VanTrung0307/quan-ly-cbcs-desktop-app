using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using QuanLyCBCS.Enums;
using QuanLyCBCS.Helpers;
using QuanLyCBCS.Models;
using QuanLyCBCS.Services;

namespace QuanLyCBCS.Forms.PersonnelForms
{
    public partial class FrmPersonnelDetail : Form
    {
        private readonly PersonnelService _service;

        private readonly FormMode _mode;

        private readonly int? _personnelId;

        private readonly PersonnelHistoryService _historyService =
            new PersonnelHistoryService();

        // UI-only fields used to match the approved basic-information mockup.
        // Quê quán is synchronized with the existing persisted QueQuanMoi field.
        private TextBox? _txtBasicQueQuan;
        // Retained as nullable compatibility fields for existing load/reset code;
        // the approved UI no longer creates email or gender radio controls.
        private TextBox? _txtBasicEmail;
        private Panel? _headerIconPanel;
        private Panel? _otherTabCanvas;
        private readonly Dictionary<Control, RoundedInputBorderWindow> _roundedInputBorderWindows = new Dictionary<Control, RoundedInputBorderWindow>();
        private readonly Dictionary<Control, Label> _validationLabels = new Dictionary<Control, Label>();
        private readonly Dictionary<Control, string> _validationMessages = new Dictionary<Control, string>();
        private readonly HashSet<Control> _invalidControls = new HashSet<Control>();

        public FrmPersonnelDetail(
            FormMode mode,
            int personnelId = 0)
        {
            InitializeComponent();

            ApplyBasicTabDesign();
            ApplyAddressTabDesign();
            ApplyWorkTabDesign();
            ApplyEducationTabDesign();
            ApplyPartyTabDesign();
            ApplyOtherTabDesign();

            // Keep the form at a practical dialog size; the maximize button is disabled.
            AutoSize = false;
            WindowState = FormWindowState.Normal;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Shown += (_, _) =>
            {
                UpdateTabLayout();
                UpdateAddressTabLayout();
                UpdateWorkTabLayout();
                UpdateEducationTabLayout();
                UpdatePartyTabLayout();
                UpdateOtherTabLayout();
                ApplyRoundedInputs();
                btnResetBasicTabInvalidate();
            };
            Resize += (_, _) => { UpdateTabLayout(); UpdateAddressTabLayout(); UpdateWorkTabLayout(); UpdateEducationTabLayout(); UpdatePartyTabLayout(); UpdateOtherTabLayout(); };

            epValidation.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            _service = new PersonnelService();

            _mode = mode;

            _personnelId = personnelId;

            cboPersonnelType.SelectedIndex = 0;

            dtNgaySinh.Format =
                DateTimePickerFormat.Custom;

            dtNgaySinh.CustomFormat =
                "dd/MM/yyyy";

            StartPosition =
                FormStartPosition.CenterScreen;

            txtSoHieu.TextChanged +=
                (_, _) => ClearError(txtSoHieu);

            txtHoTen.TextChanged +=
                (_, _) => ClearError(txtHoTen);

            txtCCCD.TextChanged +=
                (_, _) => ClearError(txtCCCD);

            txtSoDienThoai.TextChanged +=
                (_, _) => ClearError(txtSoDienThoai);

            // Keep ErrorProvider feedback from covering the input area. Validation text
            // remains available through the provider tooltip while field borders stay intact.
            foreach (Control validationControl in new Control[] { txtSoHieu, txtHoTen, txtCCCD, txtSoDienThoai, txtHeSoLuong, cboGioiTinh, cboPersonnelType, dtNgaySinh })
            {
                epValidation.SetIconAlignment(validationControl, ErrorIconAlignment.MiddleLeft);
                epValidation.SetIconPadding(validationControl, -1000);
            }

            txtCCCD.KeyPress += TxtNumberOnly_KeyPress;

            txtSoDienThoai.KeyPress += TxtNumberOnly_KeyPress;

            txtHeSoLuong.KeyPress += TxtHeSoLuong_KeyPress;

            txtHeSoLuong.Leave += TxtHeSoLuong_Leave;

            ToolTipHelper
                .SetupPersonnelDetailToolTips(
                    this,
                    toolTipMain);

            FormNavigationHelper
                .SetupEnterNavigation(
                    this);

            txtHoTen.Leave += txtHoTen_Leave;
            txtSoHieu.Leave += txtSoHieu_Leave;
            txtNoiSinh.Leave += txtNoiSinh_Leave;
            txtQueQuanMoi.Leave += Address_Leave;
            txtQueQuanCu.Leave += Address_Leave;
            txtTinhMoi.Leave += Address_Leave;
            txtTinhCu.Leave += Address_Leave;

            dtNgaySinh.MaxDate = DateTime.Today;

            dtNgaySinh.ValueChanged += (_, _) => UpdateAge();
        }


        // Reorganize the existing controls to match the approved first-tab mockup.
        // Existing data-bound controls and save/validation handlers are preserved.
        private void ApplyBasicTabDesign()
        {
            SuspendLayout();
            tabPage1.SuspendLayout();

            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = false;
            Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            BackColor = Color.FromArgb(242, 248, 255);
            ClientSize = new Size(1480, 790);
            MinimumSize = new Size(1280, 700);
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            WindowState = FormWindowState.Normal;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Thông tin CBCS";

            // Banner: same proportions and typography as the reference design.
            tabPage1.Controls.Remove(pnlHeader);
            Controls.Add(pnlHeader);
            pnlHeader.Dock = DockStyle.None;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Size = new Size(ClientSize.Width, 126);
            pnlHeader.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlHeader.BackColor = Color.FromArgb(18, 70, 132);
            pnlHeader.Paint -= PnlHeader_Paint;
            pnlHeader.Paint += PnlHeader_Paint;

            lblTitle.Dock = DockStyle.None;
            lblTitle.Location = new Point(170, 20);
            lblTitle.Size = new Size(760, 52);
            lblTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Text = "THÔNG TIN CBCS";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;

            lblSubTitle.Dock = DockStyle.None;
            lblSubTitle.Location = new Point(172, 72);
            lblSubTitle.Size = new Size(800, 30);
            lblSubTitle.Font = new Font("Segoe UI", 13F, FontStyle.Regular);
            lblSubTitle.ForeColor = Color.FromArgb(226, 239, 255);
            lblSubTitle.Text = "Quản lý, cập nhật thông tin cán bộ, chiến sĩ";
            lblSubTitle.TextAlign = ContentAlignment.MiddleLeft;

            if (_headerIconPanel == null)
            {
                _headerIconPanel = new Panel { Name = "pnlHeaderIcon" };
                pnlHeader.Controls.Add(_headerIconPanel);
                _headerIconPanel.Paint += HeaderIconPanel_Paint;
            }
            _headerIconPanel.BackColor = Color.Transparent;
            _headerIconPanel.Location = new Point(34, 17);
            _headerIconPanel.Size = new Size(96, 96);
            _headerIconPanel.BringToFront();
            lblTitle.BringToFront();
            lblSubTitle.BringToFront();

            // Main tab strip: owner-drawn to avoid the legacy Windows tab appearance.
            tabMain.Dock = DockStyle.None;
            tabMain.Location = new Point(28, 138);
            tabMain.Size = new Size(ClientSize.Width - 56, ClientSize.Height - 244);
            tabMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabMain.Appearance = TabAppearance.FlatButtons;
            tabMain.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabMain.SizeMode = TabSizeMode.Fixed;
            // Reserve space for the native TabControl header margins and DPI scaling.
            // UpdateTabLayout recalculates this after the control has its final size.
            tabMain.ItemSize = new Size(
                Math.Max(100, (tabMain.ClientSize.Width - 120) / Math.Max(1, tabMain.TabPages.Count)),
                42);
            tabMain.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            tabMain.BackColor = BackColor;
            tabMain.Padding = new Point(0, 0);
            tabMain.BackColor = Color.FromArgb(242, 248, 255);
            tabMain.DrawItem -= TabMain_DrawItem;
            tabMain.DrawItem += TabMain_DrawItem;

            // Save/cancel actions stay available at the bottom of every tab.
            tabPage5.Controls.Remove(btnSave);
            tabPage5.Controls.Remove(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            btnSave.Text = "✓  Lưu";
            btnSave.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnSave.BackColor = Color.FromArgb(0, 103, 235);
            btnSave.ForeColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Size = new Size(154, 48);
            btnSave.Location = new Point(ClientSize.Width - 350, ClientSize.Height - 78);
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSave.UseVisualStyleBackColor = false;

            btnCancel.Text = "✕  Hủy";
            btnCancel.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnCancel.BackColor = Color.FromArgb(242, 53, 53);
            btnCancel.ForeColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Size = new Size(154, 48);
            btnCancel.Location = new Point(ClientSize.Width - 180, ClientSize.Height - 78);
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click -= btnCancel_Click_Layout;
            btnCancel.Click += btnCancel_Click_Layout;

            var btnReset = Controls.Find("btnResetBasicTab", false).FirstOrDefault() as Button;
            if (btnReset == null)
            {
                btnReset = new Button { Name = "btnResetBasicTab" };
                Controls.Add(btnReset);
            }
            btnReset.Text = "⟳  Làm mới";
            btnReset.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnReset.ForeColor = Color.FromArgb(17, 66, 135);
            btnReset.BackColor = Color.White;
            btnReset.FlatStyle = FlatStyle.Flat;
            btnReset.FlatAppearance.BorderColor = Color.FromArgb(153, 183, 224);
            btnReset.FlatAppearance.BorderSize = 1;
            btnReset.FlatAppearance.MouseOverBackColor = Color.FromArgb(242, 248, 255);
            btnReset.Size = new Size(172, 48);
            btnReset.Location = new Point(32, ClientSize.Height - 78);
            btnReset.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnReset.UseVisualStyleBackColor = false;
            btnReset.Click -= btnResetBasicTab_Click;
            btnReset.Click += btnResetBasicTab_Click;

            // Basic-information card with a soft blue rounded outline.
            tabPage1.BackColor = BackColor;
            tabPage1.Padding = new Padding(0);
            foreach (TabPage page in tabMain.TabPages)
            {
                page.BackColor = BackColor;
                page.Padding = new Padding(0);
            }
            tabPage1.Controls.Remove(lblTitle);
            tabPage1.Controls.Remove(lblSubTitle);

            var card = Controls.Find("pnlBasicInfoCard", true).FirstOrDefault() as Panel;
            if (card == null)
            {
                card = new Panel { Name = "pnlBasicInfoCard" };
                tabPage1.Controls.Add(card);
            }
            card.BackColor = Color.FromArgb(247, 251, 255);
            card.BorderStyle = BorderStyle.None;
            // The card itself draws only the rounded blue outline; no native rectangular border.
            // Fill the tab page's display area so the native grey page frame is not visible
            // around the rounded information card.
            card.Location = new Point(0, 0);
            card.Size = new Size(tabPage1.ClientSize.Width, tabPage1.ClientSize.Height);
            card.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            card.Paint -= BasicInfoCard_Paint;
            card.Paint += BasicInfoCard_Paint;
            card.SendToBack();

            int leftLabel = 28, leftInput = 190, leftWidth = 330;
            int rightLabel = 535, rightInput = 690, rightWidth = 330;
            int[] ys = { 48, 108, 168, 228, 288 };

            void PlaceLabel(Label label, int x, int y, string text)
            {
                label.Text = text;
                label.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular);
                label.ForeColor = Color.FromArgb(20, 45, 92);
                label.BackColor = Color.Transparent;
                label.AutoSize = false;
                label.TextAlign = ContentAlignment.MiddleLeft;
                label.Location = new Point(x, y + 1);
                label.Size = new Size(150, 38);
                label.BringToFront();
            }

            void PlaceInput(Control input, int x, int y, int width, int height = 51)
            {
                input.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular);
                input.Location = new Point(x, y);
                input.Size = new Size(width, height == 51 ? 38 : height);
                input.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                if (input is TextBox tb)
                {
                    tb.BorderStyle = BorderStyle.FixedSingle;
                    tb.BackColor = Color.White;
                    tb.ForeColor = Color.FromArgb(35, 45, 60);
                }
                else if (input is ComboBox cb)
                {
                    cb.DropDownStyle = ComboBoxStyle.DropDownList;
                    cb.FlatStyle = FlatStyle.Flat;
                    cb.BackColor = Color.White;
                }
                input.BringToFront();
            }

            PlaceLabel(lblSoHieu, leftLabel, ys[0], "Số hiệu");
            PlaceInput(txtSoHieu, leftInput, ys[0], leftWidth);
            PlaceLabel(lblHoTen, leftLabel, ys[1], "Họ và tên");
            PlaceInput(txtHoTen, leftInput, ys[1], leftWidth);
            PlaceLabel(lblType, leftLabel, ys[2], "Loại CBCS");
            PlaceInput(cboPersonnelType, leftInput, ys[2], 220);
            PlaceLabel(lblGioiTinh, leftLabel, ys[3], "Giới tính");
            PlaceInput(cboGioiTinh, leftInput, ys[3], 220);
            cboGioiTinh.Visible = true;
            PlaceLabel(lblNgaySinh, leftLabel, ys[4], "Ngày sinh");
            PlaceInput(dtNgaySinh, leftInput, ys[4], 220, 38);
            var lblBasicNgaySinhRequired = Controls.Find("lblBasicNgaySinhRequired", true).FirstOrDefault() as Label;
            if (lblBasicNgaySinhRequired == null)
            {
                lblBasicNgaySinhRequired = new Label { Name = "lblBasicNgaySinhRequired", Text = "*", ForeColor = Color.Red, BackColor = Color.Transparent };
                tabPage1.Controls.Add(lblBasicNgaySinhRequired);
            }
            lblBasicNgaySinhRequired.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblBasicNgaySinhRequired.Location = new Point(leftLabel + 80, ys[4] + 4);
            lblBasicNgaySinhRequired.Size = new Size(18, 28);
            lblBasicNgaySinhRequired.BringToFront();

            PlaceLabel(lblNoiSinh, rightLabel, ys[0], "Nơi sinh");
            PlaceInput(txtNoiSinh, rightInput, ys[0], rightWidth);
            PlaceLabel(lblCCCD, rightLabel, ys[2], "CCCD");
            PlaceInput(txtCCCD, rightInput, ys[2], rightWidth);
            PlaceLabel(lblNgayCap, rightLabel, ys[3], "Ngày cấp");
            PlaceInput(dtNgayCapCCCD, rightInput, ys[3], 220, 38);
            PlaceLabel(lblSoDienThoai, rightLabel, ys[4], "Số điện thoại");
            PlaceInput(txtSoDienThoai, rightInput, ys[4], rightWidth);

            // Quê quán reuses the existing persisted field in the Address tab.
            if (_txtBasicQueQuan == null)
            {
                _txtBasicQueQuan = new TextBox { Name = "txtBasicQueQuan" };
                tabPage1.Controls.Add(_txtBasicQueQuan);
                _txtBasicQueQuan.TextChanged += (_, _) =>
                {
                    if (txtQueQuanMoi.Text != _txtBasicQueQuan.Text)
                        txtQueQuanMoi.Text = _txtBasicQueQuan.Text;
                };
                txtQueQuanMoi.TextChanged += (_, _) =>
                {
                    if (_txtBasicQueQuan.Text != txtQueQuanMoi.Text)
                        _txtBasicQueQuan.Text = txtQueQuanMoi.Text;
                };
            }
            var lblBasicQueQuan = Controls.Find("lblBasicQueQuan", true).FirstOrDefault() as Label;
            if (lblBasicQueQuan == null)
            {
                lblBasicQueQuan = new Label { Name = "lblBasicQueQuan" };
                tabPage1.Controls.Add(lblBasicQueQuan);
            }
            PlaceLabel(lblBasicQueQuan, rightLabel, ys[1], "Quê quán");
            PlaceInput(_txtBasicQueQuan, rightInput, ys[1], rightWidth);

            // Email is intentionally omitted from this form.
            var lblBasicEmail = Controls.Find("lblBasicEmail", true).FirstOrDefault() as Label;
            if (lblBasicEmail != null) { tabPage1.Controls.Remove(lblBasicEmail); lblBasicEmail.Dispose(); }
            if (_txtBasicEmail != null) { tabPage1.Controls.Remove(_txtBasicEmail); _txtBasicEmail.Dispose(); _txtBasicEmail = null; }

            // Required-field markers remain aligned with the fields they annotate.
            lblVaSoHieu.Location = new Point(leftLabel + 72, ys[0] + 5);
            lblVaHoTen.Location = new Point(leftLabel + 100, ys[1] + 5);
            lblVaType.Location = new Point(leftLabel + 88, ys[2] + 4);
            lblVaGioiTinh.Location = new Point(leftLabel + 88, ys[3] + 4);
            lblVaCCCD.Location = new Point(rightLabel + 52, ys[2] + 4);
            lblVaSDT.Location = new Point(rightLabel + 115, ys[4] + 4);
            foreach (var star in new[] { lblVaSoHieu, lblVaHoTen, lblVaType, lblVaGioiTinh, lblVaCCCD, lblVaSDT })
            {
                star.BackColor = Color.Transparent;
                star.BringToFront();
            }

            lblTuoi.Text = "Tuổi: " + Math.Max(0, DateTime.Today.Year - dtNgaySinh.Value.Year).ToString();
            lblTuoi.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTuoi.ForeColor = Color.FromArgb(0, 103, 235);
            lblTuoi.BackColor = Color.Transparent;
            lblTuoi.Location = new Point(leftInput + 230, ys[4] + 2);
            lblTuoi.Size = new Size(100, 36);
            lblTuoi.BringToFront();

            // Keep the personnel type on the Basic Information tab, directly below name.
            tabPage6.Controls.Remove(lblType);
            tabPage6.Controls.Remove(cboPersonnelType);
            tabPage6.Controls.Remove(lblVaType);
            if (!tabPage1.Controls.Contains(lblType)) tabPage1.Controls.Add(lblType);
            if (!tabPage1.Controls.Contains(cboPersonnelType)) tabPage1.Controls.Add(cboPersonnelType);
            if (!tabPage1.Controls.Contains(lblVaType)) tabPage1.Controls.Add(lblVaType);
            lblVaType.BringToFront();

            // Photo card with a person silhouette, matching the reference more closely.
            var photoPanel = Controls.Find("pnlPhotoPlaceholder", true).FirstOrDefault() as Panel;
            if (photoPanel == null)
            {
                photoPanel = new Panel { Name = "pnlPhotoPlaceholder" };
                tabPage1.Controls.Add(photoPanel);
            }
            photoPanel.Controls.Clear();
            photoPanel.BackColor = Color.FromArgb(247, 251, 255);
            photoPanel.BorderStyle = BorderStyle.None;
            photoPanel.Location = new Point(Math.Max(0, tabPage1.ClientSize.Width - 205), 28);
            photoPanel.Size = new Size(180, 220);
            photoPanel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            photoPanel.Paint -= PhotoPanel_Paint;
            photoPanel.Paint += PhotoPanel_Paint;

            // Use fixed, non-overlapping bounds instead of Dock=Fill; the previous
            // dock order caused the avatar and file-format hint to be clipped.
            var avatarCanvas = new Panel
            {
                Name = "pnlAvatarCanvas",
                Location = new Point(0, 8),
                Size = new Size(180, 116),
                BackColor = Color.Transparent
            };
            avatarCanvas.Paint += AvatarCanvas_Paint;
            var choose = new Label
            {
                Name = "lblChoosePhoto",
                Text = "Chọn ảnh",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(17, 50, 112),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(4, 126),
                Size = new Size(172, 30),
                BackColor = Color.Transparent
            };
            var photoHint = new Label
            {
                Name = "lblPhotoHint",
                Text = "JPG, PNG (tối đa 2MB)",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(82, 105, 150),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(4, 158),
                Size = new Size(172, 30),
                BackColor = Color.Transparent
            };
            photoPanel.Controls.Add(avatarCanvas);
            photoPanel.Controls.Add(choose);
            photoPanel.Controls.Add(photoHint);
            photoPanel.BringToFront();

            // Ensure title/footer controls stay above the tab control.
            UpdateTabLayout();
            pnlHeader.BringToFront();
            btnReset.BringToFront();
            btnSave.BringToFront();
            btnCancel.BringToFront();

            tabPage1.ResumeLayout(true);
            ResumeLayout(true);
        }

        // Restyle only the Address tab to match the approved reference image.
        // Existing TextBox instances and their data-binding/save logic are preserved.
        private void ApplyAddressTabDesign()
        {
            tabPage2.BackColor = BackColor;
            tabPage2.Padding = new Padding(0);

            var card = tabPage2.Controls.Find("pnlAddressCard", false).FirstOrDefault() as Panel;
            if (card == null)
            {
                card = new Panel { Name = "pnlAddressCard", BackColor = Color.FromArgb(247, 251, 255) };
                tabPage2.Controls.Add(card);
            }
            card.BorderStyle = BorderStyle.None;
            card.Paint -= AddressCard_Paint;
            card.Paint += AddressCard_Paint;
            card.SendToBack();

            void StyleAddressLabel(Label label, string text, bool heading = false)
            {
                label.Text = text;
                label.AutoSize = false;
                label.Font = heading
                    ? new Font("Segoe UI", 14F, FontStyle.Bold)
                    : new Font("Segoe UI", 10.5F, FontStyle.Regular);
                label.ForeColor = Color.FromArgb(20, 45, 92);
                label.BackColor = Color.Transparent;
                label.TextAlign = ContentAlignment.MiddleLeft;
            }

            StyleAddressLabel(lblQueQuanMoi, "Quê quán hiện nay", true);
            StyleAddressLabel(lblQueQuanCu, "Quê quán trước sáp nhập", true);
            StyleAddressLabel(lblTinhMoi, "Tỉnh/thành hiện nay");
            StyleAddressLabel(lblTinhCu, "Tỉnh/thành trước sáp nhập");
            StyleAddressLabel(lblHKTTMoi, "HKTT hiện nay", true);
            StyleAddressLabel(lblHKTTCu, "HKTT trước sáp nhập", true);

            void StyleAddressTextBox(TextBox textBox, string placeholder, bool multiline)
            {
                textBox.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
                textBox.BackColor = Color.White;
                textBox.ForeColor = Color.FromArgb(35, 45, 60);
                textBox.BorderStyle = BorderStyle.FixedSingle;
                textBox.PlaceholderText = placeholder;
                textBox.Multiline = multiline;
                textBox.ScrollBars = multiline ? ScrollBars.Vertical : ScrollBars.None;
                textBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            }

            StyleAddressTextBox(txtQueQuanMoi, "Nhập thông tin quê quán hiện nay...", true);
            StyleAddressTextBox(txtQueQuanCu, "Nhập thông tin quê quán trước sáp nhập...", true);
            StyleAddressTextBox(txtTinhMoi, "Nhập tỉnh/thành hiện nay...", false);
            StyleAddressTextBox(txtTinhCu, "Nhập tỉnh/thành trước sáp nhập...", false);
            StyleAddressTextBox(txtHKTTMoi, "Nhập thông tin hộ khẩu thường trú hiện nay...", true);
            StyleAddressTextBox(txtHKTTCu, "Nhập thông tin hộ khẩu thường trú trước sáp nhập...", true);

            AddAddressIcon("pnlAddressIconCurrent", "\uE80F", tabPage2);
            AddAddressIcon("pnlAddressIconPrevious", "\uE81C", tabPage2);
            AddAddressIcon("pnlAddressIconHKTTCurrent", "\uE707", tabPage2);
            AddAddressIcon("pnlAddressIconHKTTPrevious", "\uE707", tabPage2);

            UpdateAddressTabLayout();
        }

        // Restyle only the Work tab to match the approved third-tab mockup.
        // Existing controls are reused so persistence, validation and event handlers remain intact.
        private void ApplyWorkTabDesign()
        {
            if (tabPage3 == null) return;
            tabPage3.BackColor = BackColor;
            tabPage3.Padding = new Padding(0);

            var card = tabPage3.Controls.Find("pnlWorkCard", false).FirstOrDefault() as Panel;
            if (card == null)
            {
                card = new Panel { Name = "pnlWorkCard", BackColor = Color.FromArgb(247, 251, 255) };
                tabPage3.Controls.Add(card);
            }
            card.BorderStyle = BorderStyle.None;
            card.Paint -= AddressCard_Paint;
            card.Paint += AddressCard_Paint;
            card.SendToBack();

            void StyleWorkLabel(Label label, string text)
            {
                label.Text = text;
                label.AutoSize = false;
                label.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                label.ForeColor = Color.FromArgb(20, 45, 105);
                label.BackColor = Color.Transparent;
                label.TextAlign = ContentAlignment.MiddleLeft;
            }

            StyleWorkLabel(lblDonVi, "Đơn vị");
            StyleWorkLabel(lblViTriCongTac, "Vị trí công tác");
            StyleWorkLabel(lblCapBacHam, "Cấp bậc hàm");
            StyleWorkLabel(lblNgayNhanCapBac, "Ngày nhận cấp bậc");
            StyleWorkLabel(lblThangNamVaoNganh, "Tháng năm vào ngành");
            StyleWorkLabel(lblThangNamNhanCongTac, "Tháng năm nhận công tác");
            StyleWorkLabel(lblChucVu, "Chức vụ");
            StyleWorkLabel(lblNgayBoNhiem, "Ngày bổ nhiệm");
            StyleWorkLabel(lblHeSoLuong, "Hệ số lương");

            void StyleWorkTextBox(TextBox textBox, string placeholder)
            {
                textBox.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
                textBox.BackColor = Color.White;
                textBox.ForeColor = Color.FromArgb(35, 45, 60);
                textBox.BorderStyle = BorderStyle.FixedSingle;
                textBox.PlaceholderText = placeholder;
                textBox.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            }
            StyleWorkTextBox(txtDonVi, "Nhập tên đơn vị...");
            StyleWorkTextBox(txtViTriCongTac, "Nhập nơi công tác...");
            StyleWorkTextBox(txtChucVu, "Nhập chức vụ hiện tại...");
            StyleWorkTextBox(txtHeSoLuong, "Nhập hệ số lương...");

            cboCapBacHam.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            cboCapBacHam.BackColor = Color.White;
            cboCapBacHam.ForeColor = Color.FromArgb(35, 45, 60);
            cboCapBacHam.DropDownStyle = ComboBoxStyle.DropDownList;

            foreach (var picker in new[] { dtNgayNhanCapBac, dtNgayBoNhiem })
            {
                picker.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
                picker.Format = DateTimePickerFormat.Custom;
                picker.CustomFormat = "dd/MM/yyyy";
            }
            foreach (var picker in new[] { dtThangNamVaoNganh, dtThangNamNhanCongTac })
            {
                picker.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
                picker.Format = DateTimePickerFormat.Custom;
                picker.CustomFormat = "MM/yyyy";
                picker.ShowUpDown = false;
            }

            // Dedicated vector icons matching the approved Work tab mockup.
            // The address tab keeps its existing Segoe MDL2 glyphs unchanged.
            AddAddressIcon("pnlWorkIconUnit", "work:building", tabPage3);
            AddAddressIcon("pnlWorkIconPosition", "work:map-pin", tabPage3);
            AddAddressIcon("pnlWorkIconRank", "work:layers", tabPage3);
            AddAddressIcon("pnlWorkIconRankDate", "work:calendar", tabPage3);
            AddAddressIcon("pnlWorkIconIndustryDate", "work:briefcase", tabPage3);
            AddAddressIcon("pnlWorkIconAssignmentDate", "work:calendar", tabPage3);
            AddAddressIcon("pnlWorkIconTitle", "work:gear", tabPage3);
            AddAddressIcon("pnlWorkIconAppointment", "work:calendar", tabPage3);
            AddAddressIcon("pnlWorkIconSalary", "work:users", tabPage3);

            UpdateWorkTabLayout();
        }

        private void UpdateWorkTabLayout()
        {
            if (IsDisposed || tabPage3 == null || tabPage3.IsDisposed || tabPage3.ClientSize.Width < 100)
                return;

            int w = tabPage3.ClientSize.Width;
            int h = tabPage3.ClientSize.Height;
            var card = tabPage3.Controls.Find("pnlWorkCard", false).FirstOrDefault() as Panel;
            if (card != null)
            {
                card.Location = Point.Empty;
                card.Size = new Size(w, h);
                card.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                card.SendToBack();
                card.Invalidate();
            }

            int margin = 30;
            int columnGap = 46;
            int colWidth = Math.Max(300, (w - margin * 2 - columnGap) / 2);
            int leftX = margin;
            int rightX = margin + colWidth + columnGap;
            int iconSize = 54;
            int iconLabelGap = 24;
            int labelWidth = 205;
            int inputGap = 18;
            int rowTop = 34;
            int rowStep = Math.Max(72, Math.Min(84, (h - 55) / 5));
            int inputXLeft = leftX + iconSize + iconLabelGap + labelWidth + inputGap;
            int inputXRight = rightX + iconSize + iconLabelGap + labelWidth + inputGap;
            int inputWidthLeft = Math.Max(140, leftX + colWidth - inputXLeft - 12);
            int inputWidthRight = Math.Max(140, rightX + colWidth - inputXRight - 12);

            void PositionLabel(Label label, int x, int y, int width, int height = 54)
            {
                label.Location = new Point(x, y);
                label.Size = new Size(width, height);
                label.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                label.BringToFront();
            }
            void PositionControl(Control control, int x, int y, int width, int height = 50)
            {
                control.Location = new Point(x, y + (iconSize - height) / 2);
                control.Size = new Size(width, height);
                control.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                control.BringToFront();
            }
            void PositionIcon(string name, int x, int y)
            {
                var icon = tabPage3.Controls.Find(name, false).FirstOrDefault() as Panel;
                if (icon == null) return;
                icon.Location = new Point(x, y);
                icon.Size = new Size(iconSize, iconSize);
                ApplyAddressIconRegion(icon);
                icon.Invalidate();
                icon.BringToFront();
            }
            void PlaceRow(string iconName, int left, int y, Label label, Control control, int inputWidth, string? glyph = null)
            {
                PositionIcon(iconName, left, y);
                PositionLabel(label, left + iconSize + iconLabelGap, y, labelWidth);
                PositionControl(control, left + iconSize + iconLabelGap + labelWidth + inputGap, y, inputWidth);
            }

            PlaceRow("pnlWorkIconUnit", leftX, rowTop, lblDonVi, txtDonVi, inputWidthLeft);
            PlaceRow("pnlWorkIconPosition", leftX, rowTop + rowStep, lblViTriCongTac, txtViTriCongTac, inputWidthLeft);
            PlaceRow("pnlWorkIconRank", leftX, rowTop + rowStep * 2, lblCapBacHam, cboCapBacHam, inputWidthLeft);
            PlaceRow("pnlWorkIconRankDate", leftX, rowTop + rowStep * 3, lblNgayNhanCapBac, dtNgayNhanCapBac, inputWidthLeft);
            PlaceRow("pnlWorkIconIndustryDate", leftX, rowTop + rowStep * 4, lblThangNamVaoNganh, dtThangNamVaoNganh, inputWidthLeft);

            PlaceRow("pnlWorkIconAssignmentDate", rightX, rowTop, lblThangNamNhanCongTac, dtThangNamNhanCongTac, inputWidthRight);
            PlaceRow("pnlWorkIconTitle", rightX, rowTop + rowStep, lblChucVu, txtChucVu, inputWidthRight);
            PlaceRow("pnlWorkIconAppointment", rightX, rowTop + rowStep * 2, lblNgayBoNhiem, dtNgayBoNhiem, inputWidthRight);
            PlaceRow("pnlWorkIconSalary", rightX, rowTop + rowStep * 3, lblHeSoLuong, txtHeSoLuong, inputWidthRight);
        }

        // Education tab: reproduce the approved single-column design while reusing
        // the existing data-bound controls and preserving all save/load behavior.
        private void ApplyEducationTabDesign()
        {
            if (tabPage4 == null) return;
            tabPage4.BackColor = BackColor;
            tabPage4.Padding = new Padding(0);

            var card = tabPage4.Controls.Find("pnlEducationCard", false).FirstOrDefault() as Panel;
            if (card == null)
            {
                card = new Panel { Name = "pnlEducationCard", BackColor = Color.FromArgb(247, 251, 255) };
                tabPage4.Controls.Add(card);
            }
            card.BorderStyle = BorderStyle.None;
            card.Paint -= AddressCard_Paint;
            card.Paint += AddressCard_Paint;
            card.SendToBack();

            void StyleEducationLabel(Label label, string text)
            {
                label.Text = text;
                label.AutoSize = false;
                label.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                label.ForeColor = Color.FromArgb(20, 45, 105);
                label.BackColor = Color.Transparent;
                label.TextAlign = ContentAlignment.MiddleLeft;
            }

            StyleEducationLabel(lblTrinhDoTinHoc, "Trình độ tin học");
            StyleEducationLabel(lblTrinhDoNgoaiNgu, "Trình độ ngoại ngữ");
            StyleEducationLabel(lblTrinhDoVanHoa, "Trình độ văn hoá");
            StyleEducationLabel(lblTrinhDoChuyenMon, "Chuyên môn");
            StyleEducationLabel(lblChuyenNganhDaoTao, "Chuyên ngành đào tạo");
            StyleEducationLabel(lblThangNamTotNghiep, "Tháng năm tốt nghiệp");
            StyleEducationLabel(lblTrinhDoLyLuanChinhTri, "Lý luận chính trị");

            void StyleEducationTextBox(TextBox textBox, string placeholder)
            {
                textBox.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
                textBox.BackColor = Color.White;
                textBox.ForeColor = Color.FromArgb(35, 45, 60);
                textBox.BorderStyle = BorderStyle.FixedSingle;
                textBox.PlaceholderText = placeholder;
                textBox.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            }
            StyleEducationTextBox(txtTrinhDoTinHoc, "Nhập trình độ tin học hiện có của CBCS");
            StyleEducationTextBox(txtTrinhDoNgoaiNgu, "Nhập trình độ ngoại ngữ hiện có của CBCS");
            StyleEducationTextBox(txtTrinhDoVanHoa, "Ví dụ: 12/12, Trung cấp, Cao đẳng, Đại học...");
            StyleEducationTextBox(txtTrinhDoChuyenMon, "Nhập chuyên môn được đào tạo");
            StyleEducationTextBox(txtChuyenNganhDaoTao, "Nhập chuyên ngành đào tạo cụ thể");
            StyleEducationTextBox(txtTrinhDoLyLuanChinhTri, "Ví dụ: Sơ cấp, Trung cấp, Cao cấp, Cử nhân...");

            dtThangNamTotNghiep.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            dtThangNamTotNghiep.Format = DateTimePickerFormat.Custom;
            dtThangNamTotNghiep.CustomFormat = "MM/yyyy";
            dtThangNamTotNghiep.ShowUpDown = false;

            AddAddressIcon("pnlEducationIconComputer", "education:book", tabPage4);
            AddAddressIcon("pnlEducationIconLanguage", "education:globe", tabPage4);
            AddAddressIcon("pnlEducationIconCulture", "education:university", tabPage4);
            AddAddressIcon("pnlEducationIconSpecialty", "education:document", tabPage4);
            AddAddressIcon("pnlEducationIconMajor", "work:gear", tabPage4);
            AddAddressIcon("pnlEducationIconGraduation", "education:edu-calendar", tabPage4);
            AddAddressIcon("pnlEducationIconPolitics", "education:chart", tabPage4);

            UpdateEducationTabLayout();
        }

        private void UpdateEducationTabLayout()
        {
            if (IsDisposed || tabPage4 == null || tabPage4.IsDisposed || tabPage4.ClientSize.Width < 100)
                return;

            int w = tabPage4.ClientSize.Width;
            int h = tabPage4.ClientSize.Height;
            var card = tabPage4.Controls.Find("pnlEducationCard", false).FirstOrDefault() as Panel;
            if (card != null)
            {
                card.Location = Point.Empty;
                card.Size = new Size(w, h);
                card.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                card.SendToBack();
                card.Invalidate();
            }

            int marginLeft = Math.Max(28, (int)(w * 0.023));
            int iconSize = Math.Max(52, Math.Min(64, (int)(h * 0.105)));
            int labelX = marginLeft + iconSize + Math.Max(28, (int)(w * 0.024));
            int labelWidth = Math.Max(220, Math.Min(250, (int)(w * 0.17)));
            int inputX = labelX + labelWidth + Math.Max(22, (int)(w * 0.025));
            int inputWidth = Math.Max(260, w - inputX - marginLeft - 20);
            int top = Math.Max(16, (int)(h * 0.025));
            int rowStep = Math.Max(58, Math.Min(70, (h - top * 2 - iconSize) / 6));
            int inputHeight = Math.Max(42, Math.Min(50, iconSize - 4));

            void PositionLabel(Label label, int y)
            {
                label.Location = new Point(labelX, y);
                label.Size = new Size(labelWidth, iconSize);
                label.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                label.BringToFront();
            }
            void PositionControl(Control control, int y)
            {
                control.Location = new Point(inputX, y + (iconSize - inputHeight) / 2);
                control.Size = new Size(inputWidth, inputHeight);
                control.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                control.BringToFront();
            }
            void PositionIcon(string name, int y)
            {
                var icon = tabPage4.Controls.Find(name, false).FirstOrDefault() as Panel;
                if (icon == null) return;
                icon.Location = new Point(marginLeft, y);
                icon.Size = new Size(iconSize, iconSize);
                ApplyAddressIconRegion(icon);
                icon.Invalidate();
                icon.BringToFront();
            }
            void PlaceRow(string iconName, int row, Label label, Control control)
            {
                int y = top + row * rowStep;
                PositionIcon(iconName, y);
                PositionLabel(label, y);
                PositionControl(control, y);
            }

            PlaceRow("pnlEducationIconComputer", 0, lblTrinhDoTinHoc, txtTrinhDoTinHoc);
            PlaceRow("pnlEducationIconLanguage", 1, lblTrinhDoNgoaiNgu, txtTrinhDoNgoaiNgu);
            PlaceRow("pnlEducationIconCulture", 2, lblTrinhDoVanHoa, txtTrinhDoVanHoa);
            PlaceRow("pnlEducationIconSpecialty", 3, lblTrinhDoChuyenMon, txtTrinhDoChuyenMon);
            PlaceRow("pnlEducationIconMajor", 4, lblChuyenNganhDaoTao, txtChuyenNganhDaoTao);
            PlaceRow("pnlEducationIconGraduation", 5, lblThangNamTotNghiep, dtThangNamTotNghiep);
            PlaceRow("pnlEducationIconPolitics", 6, lblTrinhDoLyLuanChinhTri, txtTrinhDoLyLuanChinhTri);
        }

        // Party tab: match the approved three-row design while retaining the existing
        // checkbox/date controls and all original persistence/event handlers.
        // The approved "Khác" tab is a coming-soon placeholder. It is drawn on a
        // dedicated canvas so the existing form controls and data handlers are not
        // deleted or renamed; the canvas only changes what is visible on this tab.
        private void ApplyOtherTabDesign()
        {
            if (tabPage6 == null) return;
            tabPage6.BackColor = Color.FromArgb(244, 249, 255);
            tabPage6.Padding = new Padding(0);

            if (_otherTabCanvas == null || _otherTabCanvas.IsDisposed)
            {
                _otherTabCanvas = new Panel
                {
                    Name = "pnlOtherComingSoonCanvas",
                    BackColor = Color.FromArgb(244, 249, 255),
                    BorderStyle = BorderStyle.None,
                    Dock = DockStyle.Fill,
                    Margin = Padding.Empty,
                    TabStop = false
                };
                _otherTabCanvas.Paint += OtherTabCanvas_Paint;
                tabPage6.Controls.Add(_otherTabCanvas);
            }
            _otherTabCanvas.Dock = DockStyle.Fill;
            _otherTabCanvas.BringToFront();
            UpdateOtherTabLayout();
        }

        private void UpdateOtherTabLayout()
        {
            if (IsDisposed || tabPage6 == null || tabPage6.IsDisposed) return;
            if (_otherTabCanvas == null || _otherTabCanvas.IsDisposed) return;
            _otherTabCanvas.Bounds = tabPage6.ClientRectangle;
            _otherTabCanvas.Invalidate();
        }

        private void OtherTabCanvas_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is not Panel canvas || canvas.Width < 100 || canvas.Height < 100) return;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            int w = canvas.ClientSize.Width;
            int h = canvas.ClientSize.Height;

            // Pale blue surface and the fine rounded outline from the design.
            using (var background = new LinearGradientBrush(new Rectangle(0, 0, w, h),
                Color.FromArgb(248, 252, 255), Color.FromArgb(239, 247, 255), 90F))
                g.FillRectangle(background, 0, 0, w, h);
            using (var borderPath = RoundedRectangle(new Rectangle(1, 1, w - 3, h - 3), 12))
            using (var borderPen = new Pen(Color.FromArgb(190, 216, 250), 1.4F))
                g.DrawPath(borderPen, borderPath);

            // Scale the illustration as a single centered composition.
            float scale = Math.Min(1F, Math.Min(w / 1120F, h / 560F));
            scale = Math.Max(0.58F, scale);
            float cx = w / 2F;
            float illustrationTop = h * 0.19F;
            float iconW = 440F * scale;
            float iconH = 220F * scale;
            float left = cx - iconW / 2F;
            float top = illustrationTop;
            GraphicsState state = g.Save();
            g.TranslateTransform(left, top);
            g.ScaleTransform(scale, scale);
            DrawComingSoonIllustration(g);
            g.Restore(state);

            // Blue pill banner with clock and exact label.
            float pillW = 480F * scale;
            float pillH = 82F * scale;
            float pillX = cx - pillW / 2F;
            float pillY = top + iconH + 18F * scale;
            using (var pillPath = RoundedRectangle(new Rectangle((int)pillX, (int)pillY, (int)pillW, (int)pillH), (int)(pillH / 2F)))
            using (var pillFill = new LinearGradientBrush(new Rectangle((int)pillX, (int)pillY, (int)pillW, (int)pillH),
                Color.FromArgb(222, 237, 255), Color.FromArgb(209, 229, 255), 0F))
            using (var pillPen = new Pen(Color.FromArgb(190, 208, 241), 1.4F))
            {
                g.FillPath(pillFill, pillPath);
                g.DrawPath(pillPen, pillPath);
            }

            float clockD = 43F * scale;
            float clockX = pillX + 52F * scale;
            float clockY = pillY + (pillH - clockD) / 2F;
            using (var clockPen = new Pen(Color.FromArgb(0, 57, 169), 3.5F * scale))
            {
                g.DrawEllipse(clockPen, clockX, clockY, clockD, clockD);
                g.DrawLine(clockPen, clockX + clockD * .5F, clockY + clockD * .22F,
                    clockX + clockD * .5F, clockY + clockD * .52F);
                g.DrawLine(clockPen, clockX + clockD * .5F, clockY + clockD * .52F,
                    clockX + clockD * .72F, clockY + clockD * .52F);
            }
            using (var textFont = new Font("Segoe UI", 25F * scale, FontStyle.Bold, GraphicsUnit.Pixel))
            using (var textBrush = new SolidBrush(Color.FromArgb(13, 47, 151)))
            {
                var textRect = new RectangleF(clockX + clockD + 18F * scale, pillY, pillX + pillW - (clockX + clockD + 28F * scale), pillH);
                using var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.NoWrap };
                g.DrawString("Sẽ cập nhật sớm", textFont, textBrush, textRect, format);
            }

            // Small blue emphasis strokes at both sides of the banner.
            using (var rayPen = new Pen(Color.FromArgb(13, 47, 170), 3.2F * scale))
            {
                rayPen.StartCap = LineCap.Round; rayPen.EndCap = LineCap.Round;
                float yMid = pillY + pillH * .46F;
                g.DrawLine(rayPen, pillX - 24F * scale, yMid - 22F * scale, pillX - 42F * scale, yMid - 36F * scale);
                g.DrawLine(rayPen, pillX - 30F * scale, yMid, pillX - 52F * scale, yMid - 4F * scale);
                g.DrawLine(rayPen, pillX - 24F * scale, yMid + 20F * scale, pillX - 43F * scale, yMid + 30F * scale);
                g.DrawLine(rayPen, pillX + pillW + 24F * scale, yMid - 22F * scale, pillX + pillW + 42F * scale, yMid - 36F * scale);
                g.DrawLine(rayPen, pillX + pillW + 30F * scale, yMid, pillX + pillW + 52F * scale, yMid - 4F * scale);
                g.DrawLine(rayPen, pillX + pillW + 24F * scale, yMid + 20F * scale, pillX + pillW + 43F * scale, yMid + 30F * scale);
            }
        }

        private static void DrawComingSoonIllustration(Graphics g)
        {
            // Soft blue cloud behind the clipboard.
            using (var cloud = new SolidBrush(Color.FromArgb(218, 235, 255)))
            {
                g.FillEllipse(cloud, 28, 100, 145, 105);
                g.FillEllipse(cloud, 90, 48, 170, 150);
                g.FillEllipse(cloud, 225, 75, 145, 130);
                g.FillEllipse(cloud, 300, 112, 100, 88);
                g.FillRectangle(cloud, 75, 120, 275, 78);
            }

            // Decorative gear on the right.
            using (var gearBrush = new SolidBrush(Color.FromArgb(137, 187, 246)))
            using (var gearHole = new SolidBrush(Color.FromArgb(222, 238, 255)))
            {
                float gx = 310, gy = 98, r = 47;
                for (int i = 0; i < 8; i++)
                {
                    GraphicsState gearState = g.Save();
                    g.TranslateTransform(gx, gy);
                    g.RotateTransform(i * 45F);
                    FillRoundedRectangleCompat(g, gearBrush, new RectangleF(-10, -r - 9, 20, 28), 4);
                    g.Restore(gearState);
                }
                g.FillEllipse(gearBrush, gx - r + 4, gy - r + 4, (r - 4) * 2, (r - 4) * 2);
                g.FillEllipse(gearHole, gx - 17, gy - 17, 34, 34);
            }

            // Clipboard body and blue outline.
            using (var boardPath = RoundedRectangle(new Rectangle(120, 22, 128, 164), 13))
            using (var boardFill = new SolidBrush(Color.FromArgb(249, 252, 255)))
            using (var boardPen = new Pen(Color.FromArgb(71, 139, 225), 8F))
            {
                g.FillPath(boardFill, boardPath);
                g.DrawPath(boardPen, boardPath);
            }
            using (var clipPath = RoundedRectangle(new Rectangle(157, 11, 58, 25), 6))
            using (var clipFill = new SolidBrush(Color.FromArgb(43, 103, 181)))
                g.FillPath(clipFill, clipPath);
            using (var clipHole = new SolidBrush(Color.FromArgb(245, 250, 255)))
                g.FillEllipse(clipHole, 179, 3, 13, 13);

            // Three checked rows.
            using (var checkFill = new SolidBrush(Color.FromArgb(173, 208, 250)))
            using (var lineFill = new SolidBrush(Color.FromArgb(115, 169, 237)))
            using (var checkPen = new Pen(Color.FromArgb(248, 252, 255), 3.2F))
            {
                for (int i = 0; i < 3; i++)
                {
                    float y = 65 + i * 35;
                    FillRoundedRectangleCompat(g, checkFill, new RectangleF(139, y, 21, 21), 4);
                    g.DrawLine(checkPen, 144, y + 10, 149, y + 15);
                    g.DrawLine(checkPen, 149, y + 15, 157, y + 5);
                    FillRoundedRectangleCompat(g, lineFill, new RectangleF(170, y + 2, 55, 5), 2);
                    FillRoundedRectangleCompat(g, lineFill, new RectangleF(170, y + 12, 42, 4), 2);
                }
            }

            // Golden hard hat in front of the clipboard.
            using (var gold = new SolidBrush(Color.FromArgb(255, 174, 20)))
            using (var goldLight = new SolidBrush(Color.FromArgb(255, 195, 51)))
            {
                g.FillEllipse(goldLight, 190, 98, 100, 86);
                g.FillRectangle(goldLight, 195, 130, 90, 38);
                g.FillEllipse(gold, 190, 145, 102, 22);
                FillRoundedRectangleCompat(g, gold, new RectangleF(173, 162, 136, 22), 9);
                g.FillPolygon(gold, new[] { new PointF(224, 100), new PointF(239, 105), new PointF(239, 158), new PointF(224, 158) });
                g.FillPolygon(gold, new[] { new PointF(203, 111), new PointF(215, 119), new PointF(215, 158), new PointF(203, 158) });
                g.FillPolygon(gold, new[] { new PointF(250, 111), new PointF(262, 119), new PointF(262, 158), new PointF(250, 158) });
            }

            // Tiny sparkle accents.
            using (var sparkle = new SolidBrush(Color.FromArgb(255, 192, 54)))
            {
                g.FillPolygon(sparkle, new[] { new PointF(35, 95), new PointF(41, 108), new PointF(54, 114), new PointF(41, 120), new PointF(35, 133), new PointF(29, 120), new PointF(16, 114), new PointF(29, 108) });
            }
            using (var paleSparkle = new SolidBrush(Color.FromArgb(194, 220, 252)))
                g.FillPolygon(paleSparkle, new[] { new PointF(72, 77), new PointF(80, 86), new PointF(72, 95), new PointF(64, 86) });
        }

        private void ApplyPartyTabDesign()
        {
            if (tabPage5 == null) return;
            tabPage5.BackColor = BackColor;
            tabPage5.Padding = new Padding(0);

            var card = tabPage5.Controls.Find("pnlPartyCard", false).FirstOrDefault() as Panel;
            if (card == null)
            {
                card = new Panel { Name = "pnlPartyCard", BackColor = Color.FromArgb(247, 251, 255) };
                tabPage5.Controls.Add(card);
            }
            card.BorderStyle = BorderStyle.None;
            card.Paint -= AddressCard_Paint;
            card.Paint += AddressCard_Paint;
            card.SendToBack();

            StylePartyLabel(lblDangDuBi, "Đảng viên dự bị");
            StylePartyLabel(lblDangChinhThuc, "Đảng viên chính thức");
            chkDangVien.Text = "Đảng viên";
            chkDangVien.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            chkDangVien.ForeColor = Color.FromArgb(20, 45, 105);
            chkDangVien.BackColor = Color.Transparent;
            chkDangVien.AutoSize = true;
            chkDangVien.UseVisualStyleBackColor = false;

            foreach (DateTimePicker picker in new[] { dtDangDuBi, dtDangChinhThuc })
            {
                picker.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
                picker.Format = DateTimePickerFormat.Custom;
                picker.CustomFormat = "dd/MM/yyyy";
                picker.CalendarForeColor = Color.FromArgb(35, 45, 60);
                picker.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            }

            AddAddressIcon("pnlPartyIconMember", "party:people", tabPage5);
            AddAddressIcon("pnlPartyIconProvisional", "party:calendar-provisional", tabPage5);
            AddAddressIcon("pnlPartyIconOfficial", "party:calendar-check", tabPage5);
            UpdatePartyTabLayout();
        }

        private static void StylePartyLabel(Label label, string text)
        {
            label.Text = text;
            label.AutoSize = false;
            label.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            label.ForeColor = Color.FromArgb(20, 45, 105);
            label.BackColor = Color.Transparent;
            label.TextAlign = ContentAlignment.MiddleLeft;
        }

        private void UpdatePartyTabLayout()
        {
            if (IsDisposed || tabPage5 == null || tabPage5.IsDisposed || tabPage5.ClientSize.Width < 100)
                return;

            int w = tabPage5.ClientSize.Width;
            int h = tabPage5.ClientSize.Height;
            var card = tabPage5.Controls.Find("pnlPartyCard", false).FirstOrDefault() as Panel;
            if (card != null)
            {
                card.Location = Point.Empty;
                card.Size = new Size(w, h);
                card.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                card.SendToBack();
                card.Invalidate();
            }

            int marginLeft = Math.Max(30, (int)(w * 0.023));
            int iconSize = Math.Max(62, Math.Min(76, (int)(h * 0.145)));
            int top = Math.Max(34, (int)(h * 0.125));
            int rowStep = Math.Max(86, Math.Min(106, (int)(h * 0.20)));
            int labelX = marginLeft + iconSize + Math.Max(34, (int)(w * 0.034));
            int labelWidth = Math.Max(245, Math.Min(285, (int)(w * 0.18)));
            int inputX = labelX + labelWidth + Math.Max(22, (int)(w * 0.018));
            int inputWidth = Math.Max(270, Math.Min(326, w - inputX - marginLeft - 20));
            int inputHeight = Math.Max(42, Math.Min(58, iconSize - 12));

            void PlaceIcon(string name, int y)
            {
                var icon = tabPage5.Controls.Find(name, false).FirstOrDefault() as Panel;
                if (icon == null) return;
                icon.Location = new Point(marginLeft, y);
                icon.Size = new Size(iconSize, iconSize);
                ApplyAddressIconRegion(icon);
                icon.Invalidate();
                icon.BringToFront();
            }
            void PlaceLabel(Label label, int y)
            {
                label.Location = new Point(labelX, y);
                label.Size = new Size(labelWidth, iconSize);
                label.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                label.BringToFront();
            }
            void PlaceDate(DateTimePicker picker, int y)
            {
                picker.Location = new Point(inputX, y + (iconSize - inputHeight) / 2);
                picker.Size = new Size(inputWidth, inputHeight);
                picker.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                picker.BringToFront();
            }

            PlaceIcon("pnlPartyIconMember", top);
            chkDangVien.Location = new Point(labelX, top + (iconSize - chkDangVien.PreferredSize.Height) / 2);
            chkDangVien.BringToFront();

            int provisionalY = top + rowStep;
            PlaceIcon("pnlPartyIconProvisional", provisionalY);
            PlaceLabel(lblDangDuBi, provisionalY);
            PlaceDate(dtDangDuBi, provisionalY);

            int officialY = top + rowStep * 2;
            PlaceIcon("pnlPartyIconOfficial", officialY);
            PlaceLabel(lblDangChinhThuc, officialY);
            PlaceDate(dtDangChinhThuc, officialY);
        }

        private void AddAddressIcon(string name, string glyph, Control parent)
        {
            var iconPanel = parent.Controls.Find(name, false).FirstOrDefault() as Panel;
            if (iconPanel == null)
            {
                iconPanel = new Panel { Name = name, Tag = glyph };
                parent.Controls.Add(iconPanel);
            }
            // Update Tag even when the icon panel was created by the Designer earlier.
            iconPanel.Tag = glyph;
            // Keep the panel itself transparent. Otherwise WinForms paints its
            // rectangular BackColor behind the rounded icon tile, leaving square
            // corners visible outside the rounded fill.
            iconPanel.BackColor = Color.Transparent;
            iconPanel.BorderStyle = BorderStyle.None;
            iconPanel.Paint -= AddressIcon_Paint;
            iconPanel.Paint += AddressIcon_Paint;
            iconPanel.SizeChanged -= AddressIcon_SizeChanged;
            iconPanel.SizeChanged += AddressIcon_SizeChanged;
            ApplyAddressIconRegion(iconPanel);
            iconPanel.BringToFront();
        }

        private void UpdateAddressTabLayout()
        {
            if (IsDisposed || tabPage2 == null || tabPage2.IsDisposed || tabPage2.ClientSize.Width < 100)
                return;

            int w = tabPage2.ClientSize.Width;
            int h = tabPage2.ClientSize.Height;
            var card = tabPage2.Controls.Find("pnlAddressCard", false).FirstOrDefault() as Panel;
            if (card != null)
            {
                card.Location = new Point(0, 0);
                card.Size = new Size(w, h);
                card.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                card.SendToBack();
                card.Invalidate();
            }

            int margin = 24;
            int gap = 42;
            int colWidth = Math.Max(250, (w - margin * 2 - gap) / 2);
            int leftX = margin;
            int rightX = margin + colWidth + gap;
            int iconSize = 46;
            int headingXOffset = 64;
            int headingY = 16;
            int textareaY = 68;
            int textareaH = Math.Max(95, Math.Min(122, (h - 250) / 2));
            int provinceLabelY = textareaY + textareaH + 10;
            int provinceInputY = provinceLabelY + 28;
            int secondHeadingY = Math.Max(provinceInputY + 58, h - 204);
            int secondTextareaY = secondHeadingY + 54;
            int secondTextareaH = Math.Max(75, h - secondTextareaY - 20);

            void PositionLabel(Label label, int x, int y, int width, int height)
            {
                label.Location = new Point(x, y);
                label.Size = new Size(width, height);
                label.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                label.BringToFront();
            }
            void PositionText(TextBox textBox, int x, int y, int width, int height)
            {
                textBox.Location = new Point(x, y);
                textBox.Size = new Size(width, height);
                textBox.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                textBox.BringToFront();
            }
            void PositionIcon(string name, int x, int y)
            {
                var icon = tabPage2.Controls.Find(name, false).FirstOrDefault() as Panel;
                if (icon == null) return;
                icon.Location = new Point(x, y);
                icon.Size = new Size(iconSize, iconSize);
                ApplyAddressIconRegion(icon);
                icon.Invalidate();
                icon.BringToFront();
            }

            PositionIcon("pnlAddressIconCurrent", leftX, headingY);
            PositionIcon("pnlAddressIconPrevious", rightX, headingY);
            PositionLabel(lblQueQuanMoi, leftX + headingXOffset, headingY, colWidth - headingXOffset, iconSize);
            PositionLabel(lblQueQuanCu, rightX + headingXOffset, headingY, colWidth - headingXOffset, iconSize);
            PositionText(txtQueQuanMoi, leftX + headingXOffset, textareaY, colWidth - headingXOffset, textareaH);
            PositionText(txtQueQuanCu, rightX + headingXOffset, textareaY, colWidth - headingXOffset, textareaH);

            PositionLabel(lblTinhMoi, leftX + headingXOffset, provinceLabelY, colWidth - headingXOffset, 25);
            PositionLabel(lblTinhCu, rightX + headingXOffset, provinceLabelY, colWidth - headingXOffset, 25);
            PositionText(txtTinhMoi, leftX + headingXOffset, provinceInputY, colWidth - headingXOffset, 38);
            PositionText(txtTinhCu, rightX + headingXOffset, provinceInputY, colWidth - headingXOffset, 38);

            PositionIcon("pnlAddressIconHKTTCurrent", leftX, secondHeadingY);
            PositionIcon("pnlAddressIconHKTTPrevious", rightX, secondHeadingY);
            PositionLabel(lblHKTTMoi, leftX + headingXOffset, secondHeadingY, colWidth - headingXOffset, iconSize);
            PositionLabel(lblHKTTCu, rightX + headingXOffset, secondHeadingY, colWidth - headingXOffset, iconSize);
            PositionText(txtHKTTMoi, leftX + headingXOffset, secondTextareaY, colWidth - headingXOffset, secondTextareaH);
            PositionText(txtHKTTCu, rightX + headingXOffset, secondTextareaY, colWidth - headingXOffset, secondTextareaH);
        }

        private void AddressCard_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is not Panel panel || panel.Width < 4 || panel.Height < 4) return;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var path = RoundedRectangle(new Rectangle(1, 1, panel.Width - 3, panel.Height - 3), 10);
            using var pen = new Pen(Color.FromArgb(190, 211, 244), 1.2F);
            e.Graphics.DrawPath(pen, path);
        }

        private void AddressIcon_SizeChanged(object? sender, EventArgs e)
        {
            if (sender is Panel panel)
                ApplyAddressIconRegion(panel);
        }

        private static void ApplyAddressIconRegion(Panel panel)
        {
            if (panel.Width < 4 || panel.Height < 4) return;
            // Clip the native control to the same rounded shape we paint. This
            // removes any square panel pixels that can otherwise show at corners.
            using var regionPath = RoundedRectangle(
                new Rectangle(0, 0, panel.Width - 1, panel.Height - 1), 8);
            var newRegion = new Region(regionPath);
            var oldRegion = panel.Region;
            panel.Region = newRegion;
            oldRegion?.Dispose();
        }

        private void AddressIcon_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is not Panel panel) return;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            using var path = RoundedRectangle(new Rectangle(1, 1, panel.Width - 3, panel.Height - 3), 8);
            using var fill = new SolidBrush(Color.FromArgb(222, 238, 255));
            using var border = new Pen(Color.FromArgb(190, 218, 250), 1F);
            e.Graphics.FillPath(fill, path);
            e.Graphics.DrawPath(border, path);
            string glyph = panel.Tag as string ?? "\uE707";
            if (glyph.StartsWith("work:", StringComparison.Ordinal))
            {
                DrawWorkTabIcon(e.Graphics, glyph.Substring(5), panel.ClientRectangle);
                return;
            }
            if (glyph.StartsWith("education:", StringComparison.Ordinal))
            {
                DrawWorkTabIcon(e.Graphics, glyph.Substring(10), panel.ClientRectangle);
                return;
            }
            if (glyph.StartsWith("party:", StringComparison.Ordinal))
            {
                DrawWorkTabIcon(e.Graphics, glyph.Substring(6), panel.ClientRectangle);
                return;
            }

            using var font = new Font("Segoe MDL2 Assets", 22F, FontStyle.Regular);
            using var brush = new SolidBrush(Color.FromArgb(0, 75, 170));
            var size = e.Graphics.MeasureString(glyph, font);
            e.Graphics.DrawString(glyph, font, brush,
                (panel.Width - size.Width) / 2F, (panel.Height - size.Height) / 2F + 1F);
        }

        private static void DrawWorkTabIcon(Graphics g, string kind, Rectangle bounds)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            Color blue = Color.FromArgb(0, 91, 190);
            Color white = Color.White;
            using var blueBrush = new SolidBrush(blue);
            using var whiteBrush = new SolidBrush(white);
            using var bluePen = new Pen(blue, 2.2F) { LineJoin = LineJoin.Round, StartCap = LineCap.Round, EndCap = LineCap.Round };
            using var whitePen = new Pen(white, 1.8F) { LineJoin = LineJoin.Round, StartCap = LineCap.Round, EndCap = LineCap.Round };
            float s = Math.Min(bounds.Width, bounds.Height) / 54F;
            g.TranslateTransform(bounds.Left + bounds.Width / 2F, bounds.Top + bounds.Height / 2F);
            g.ScaleTransform(s, s);

            switch (kind)
            {
                case "book":
                    using (var bookPen = new Pen(blue, 2.6F) { LineJoin = LineJoin.Round })
                    {
                        g.DrawRectangle(bookPen, -15, -17, 13, 31);
                        g.DrawRectangle(bookPen, 2, -17, 13, 31);
                        g.DrawLine(bookPen, 0, -14, 0, 15);
                        g.DrawLine(bookPen, -11, -9, -6, -9);
                        g.DrawLine(bookPen, 6, -9, 11, -9);
                    }
                    break;
                case "globe":
                    g.DrawEllipse(bluePen, -19, -19, 38, 38);
                    g.DrawEllipse(bluePen, -9, -19, 18, 38);
                    g.DrawEllipse(bluePen, -18, -8, 36, 16);
                    g.DrawLine(bluePen, -18, 0, 18, 0);
                    break;
                case "university":
                    g.FillPolygon(blueBrush, new[] { new PointF(0, -21), new PointF(20, -10), new PointF(-20, -10) });
                    g.DrawLine(bluePen, -20, -7, 20, -7);
                    for (int x = -14; x <= 14; x += 9) g.DrawLine(bluePen, x, -5, x, 13);
                    g.DrawLine(bluePen, -21, 15, 21, 15);
                    g.DrawLine(bluePen, -23, 20, 23, 20);
                    break;
                case "document":
                    using (var docPath = new GraphicsPath())
                    {
                        docPath.AddRectangle(new Rectangle(-15, -20, 30, 40));
                        g.DrawPath(bluePen, docPath);
                    }
                    g.DrawLine(bluePen, -8, -9, 8, -9);
                    g.DrawLine(bluePen, -8, -1, 8, -1);
                    g.DrawLine(bluePen, -8, 7, 4, 7);
                    break;
                case "edu-calendar":
                    g.DrawRectangle(bluePen, -17, -15, 34, 32);
                    g.DrawLine(bluePen, -17, -7, 17, -7);
                    g.DrawLine(bluePen, -9, -20, -9, -11);
                    g.DrawLine(bluePen, 9, -20, 9, -11);
                    break;
                case "chart":
                    g.DrawLine(bluePen, -18, 18, -18, -17);
                    g.DrawLine(bluePen, -18, 18, 20, 18);
                    g.DrawLines(bluePen, new[] { new Point(-13, 9), new Point(-4, 1), new Point(3, 6), new Point(14, -9) });
                    g.DrawLine(bluePen, 7, -9, 14, -9);
                    g.DrawLine(bluePen, 14, -9, 14, -2);
                    break;
                case "building":
                    // Solid office building with a stepped roof and white windows.
                    g.FillRectangle(blueBrush, -13, -17, 26, 34);
                    g.FillPolygon(blueBrush, new[] { new PointF(-16, -17), new PointF(0, -23), new PointF(16, -17) });
                    for (int row = 0; row < 3; row++)
                        for (int col = 0; col < 3; col++)
                            g.FillRectangle(whiteBrush, -8 + col * 7, -12 + row * 8, 3, 4);
                    g.FillRectangle(whiteBrush, -3, 10, 6, 7);
                    break;
                case "map-pin":
                    using (var pin = new GraphicsPath())
                    {
                        pin.AddArc(-12, -21, 24, 24, 205, 310);
                        pin.AddLine(pin.GetLastPoint(), new PointF(0, 22));
                        pin.CloseFigure();
                        g.FillPath(blueBrush, pin);
                    }
                    g.FillEllipse(whiteBrush, -5, -15, 10, 10);
                    break;
                case "layers":
                    g.FillPolygon(blueBrush, new[] { new PointF(0, -20), new PointF(17, -10), new PointF(0, 0), new PointF(-17, -10) });
                    g.FillPolygon(blueBrush, new[] { new PointF(-16, -2), new PointF(0, 8), new PointF(16, -2), new PointF(16, 4), new PointF(0, 14), new PointF(-16, 4) });
                    g.FillPolygon(blueBrush, new[] { new PointF(-13, 9), new PointF(0, 17), new PointF(13, 9), new PointF(13, 14), new PointF(0, 22), new PointF(-13, 14) });
                    break;
                case "calendar":
                    g.FillRectangle(blueBrush, -16, -15, 32, 31);
                    g.FillRectangle(whiteBrush, -11, -7, 22, 18);
                    g.DrawLine(whitePen, -10, -10, 10, -10);
                    g.DrawLine(whitePen, -9, -20, -9, -12);
                    g.DrawLine(whitePen, 9, -20, 9, -12);
                    using (var gridPen = new Pen(blue, 1.5F))
                    {
                        for (int x = -5; x <= 5; x += 5) g.DrawLine(gridPen, x, -4, x, 8);
                        for (int y = 0; y <= 6; y += 6) g.DrawLine(gridPen, -9, y, 9, y);
                    }
                    break;
                case "briefcase":
                    g.FillRectangle(blueBrush, -18, -10, 36, 25);
                    g.FillRectangle(blueBrush, -8, -17, 16, 8);
                    g.FillRectangle(whiteBrush, -4, -14, 8, 3);
                    g.FillRectangle(whiteBrush, -18, -2, 36, 3);
                    g.FillRectangle(whiteBrush, -2, -3, 4, 7);
                    break;
                case "gear":
                    // Filled cog, white hub, visually matching the solid settings icon in the mockup.
                    for (int i = 0; i < 8; i++)
                    {
                        g.ResetTransform();
                        g.TranslateTransform(bounds.Left + bounds.Width / 2F, bounds.Top + bounds.Height / 2F);
                        g.ScaleTransform(s, s);
                        g.RotateTransform(i * 45F);
                        g.FillRectangle(blueBrush, -4.2F, -21F, 8.4F, 10F);
                    }
                    g.ResetTransform();
                    g.TranslateTransform(bounds.Left + bounds.Width / 2F, bounds.Top + bounds.Height / 2F);
                    g.ScaleTransform(s, s);
                    g.FillEllipse(blueBrush, -15, -15, 30, 30);
                    g.FillEllipse(whiteBrush, -6, -6, 12, 12);
                    break;
                case "users":
                    g.FillEllipse(blueBrush, -6, -19, 12, 12);
                    g.FillEllipse(blueBrush, -18, -13, 10, 10);
                    g.FillEllipse(blueBrush, 8, -13, 10, 10);
                    using (var body = RoundedRectangle(new Rectangle(-13, -4, 26, 18), 5)) g.FillPath(blueBrush, body);
                    using (var leftBody = RoundedRectangle(new Rectangle(-23, -2, 13, 14), 4)) g.FillPath(blueBrush, leftBody);
                    using (var rightBody = RoundedRectangle(new Rectangle(10, -2, 13, 14), 4)) g.FillPath(blueBrush, rightBody);
                    break;
                case "people":
                    // Three-person Party member symbol, matching the reference icon.
                    g.FillEllipse(blueBrush, -6, -17, 12, 12);
                    g.FillEllipse(blueBrush, -19, -11, 10, 10);
                    g.FillEllipse(blueBrush, 9, -11, 10, 10);
                    using (var centerBody = RoundedRectangle(new Rectangle(-13, -3, 26, 17), 5)) g.FillPath(blueBrush, centerBody);
                    using (var leftBody = RoundedRectangle(new Rectangle(-24, 0, 14, 12), 4)) g.FillPath(blueBrush, leftBody);
                    using (var rightBody = RoundedRectangle(new Rectangle(10, 0, 14, 12), 4)) g.FillPath(blueBrush, rightBody);
                    break;
                case "calendar-provisional":
                case "calendar-check":
                    g.DrawRectangle(bluePen, -17, -14, 34, 31);
                    g.DrawLine(bluePen, -17, -6, 17, -6);
                    g.DrawLine(bluePen, -9, -20, -9, -11);
                    g.DrawLine(bluePen, 9, -20, 9, -11);
                    if (kind == "calendar-check")
                    {
                        g.DrawLines(bluePen, new[] { new Point(-9, 4), new Point(-3, 10), new Point(10, -3) });
                    }
                    else
                    {
                        g.DrawRectangle(bluePen, -7, -2, 14, 12);
                        g.DrawLine(bluePen, -4, 4, -1, 7);
                        g.DrawLine(bluePen, -1, 7, 4, 1);
                    }
                    break;
            }
            g.ResetTransform();
        }

        private void UpdateTabLayout()
        {
            if (IsDisposed || tabMain == null || tabMain.TabPages.Count == 0)
                return;

            // Expand each tab slightly and evenly while preserving a small native
            // header margin so the left/right overflow buttons remain hidden.
            int tabCount = tabMain.TabPages.Count;
            int availableWidth = Math.Max(tabCount * 100, tabMain.ClientSize.Width - 72);
            int tabWidth = Math.Max(100, availableWidth / tabCount);
            if (tabMain.SizeMode != TabSizeMode.Fixed)
                tabMain.SizeMode = TabSizeMode.Fixed;
            if (tabMain.DrawMode != TabDrawMode.OwnerDrawFixed)
                tabMain.DrawMode = TabDrawMode.OwnerDrawFixed;
            if (tabMain.Appearance != TabAppearance.FlatButtons)
                tabMain.Appearance = TabAppearance.FlatButtons;
            if (tabMain.ItemSize.Width != tabWidth || tabMain.ItemSize.Height != 42)
                tabMain.ItemSize = new Size(tabWidth, 42);
            if (tabMain.Font.Size != 9F || tabMain.Font.Name != "Segoe UI")
                tabMain.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            tabMain.Padding = new Point(0, 0);

            // Keep the rounded card and photo placeholder aligned after DPI/layout changes.
            var card = Controls.Find("pnlBasicInfoCard", true).FirstOrDefault() as Panel;
            if (card != null)
                card.Size = new Size(Math.Max(100, tabPage1.ClientSize.Width), Math.Max(100, tabPage1.ClientSize.Height));
            var photoPanel = Controls.Find("pnlPhotoPlaceholder", true).FirstOrDefault() as Panel;
            if (photoPanel != null)
                photoPanel.Location = new Point(Math.Max(0, tabPage1.ClientSize.Width - photoPanel.Width - 14), 28);
            tabMain.Invalidate();
        }

        private void PnlHeader_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var ringPen = new Pen(Color.FromArgb(45, 160, 215, 255), 2F);
            g.DrawEllipse(ringPen, pnlHeader.Width - 230, -95, 300, 300);
            g.DrawEllipse(ringPen, pnlHeader.Width - 210, -75, 260, 260);
            using var starBrush = new SolidBrush(Color.FromArgb(35, 225, 241, 255));
            PointF[] star = new PointF[10];
            float cx = pnlHeader.Width - 100, cy = 60, outer = 48, inner = 22;
            for (int i = 0; i < 10; i++)
            {
                double angle = -Math.PI / 2 + i * Math.PI / 5;
                float radius = i % 2 == 0 ? outer : inner;
                star[i] = new PointF(cx + (float)Math.Cos(angle) * radius, cy + (float)Math.Sin(angle) * radius);
            }
            g.FillPolygon(starBrush, star);
        }

        private void HeaderIconPanel_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var white = new SolidBrush(Color.FromArgb(248, 252, 255));
            using var blue = new SolidBrush(Color.FromArgb(22, 66, 124));
            using var shadow = new SolidBrush(Color.FromArgb(60, 130, 180, 235));
            using (var shadowPath = RoundedRectangle(new Rectangle(2, 7, 104, 98), 12)) g.FillPath(shadow, shadowPath);
            using (var whitePath = RoundedRectangle(new Rectangle(7, 2, 96, 100), 12)) g.FillPath(white, whitePath);
            g.FillEllipse(blue, 40, 22, 20, 20);
            using (var bodyPath = RoundedRectangle(new Rectangle(27, 46, 46, 30), 8)) g.FillPath(blue, bodyPath);
            using (var linePath = RoundedRectangle(new Rectangle(24, 80, 52, 7), 3)) g.FillPath(blue, linePath);
        }

        private void BasicInfoCard_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is not Panel panel) return;
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var path = RoundedRectangle(new Rectangle(1, 1, panel.Width - 3, panel.Height - 3), 12);
            using var pen = new Pen(Color.FromArgb(190, 211, 244), 1.3F);
            g.DrawPath(pen, path);
        }

        private void PhotoPanel_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is not Panel panel) return;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var path = RoundedRectangle(new Rectangle(1, 1, panel.Width - 3, panel.Height - 3), 10);
            using var pen = new Pen(Color.FromArgb(153, 194, 247), 1.2F) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };
            e.Graphics.DrawPath(pen, path);
        }

        private void AvatarCanvas_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var pale = new SolidBrush(Color.FromArgb(219, 235, 255));
            using var blue = new SolidBrush(Color.FromArgb(91, 159, 245));
            // Center a 100px avatar inside the 180x116 canvas without clipping.
            g.FillEllipse(pale, 40, 4, 100, 100);
            g.FillEllipse(blue, 73, 20, 34, 34);
            using (var bodyPath = RoundedRectangle(new Rectangle(61, 57, 58, 42), 21)) g.FillPath(blue, bodyPath);
        }

        private void TabMain_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= tabMain.TabPages.Count) return;
            bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            Rectangle r = e.Bounds;
            using var bg = new SolidBrush(selected ? Color.FromArgb(0, 103, 235) : Color.FromArgb(242, 248, 255));
            using var border = new Pen(Color.FromArgb(190, 211, 244), 1F);
            using var tabPath = RoundedRectangle(new Rectangle(r.X + 1, r.Y + 1, r.Width - 3, r.Height - 3), selected ? 10 : 4);
            e.Graphics.FillPath(bg, tabPath);
            e.Graphics.DrawPath(border, tabPath);
            string[] icons = { "\uE946", "\uE80F", "\uE821", "\uE7BE", "\uE8D7", "\uE8FD" };
            string[] names = { "Thông tin cơ bản", "Địa chỉ", "Công tác", "Đào tạo", "Đảng", "Khác" };
            Color fore = selected ? Color.White : Color.FromArgb(17, 48, 112);
            using var iconFont = new Font("Segoe MDL2 Assets", 14F, FontStyle.Regular);
            using var textFont = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            using var brush = new SolidBrush(fore);
            var iconSize = e.Graphics.MeasureString(icons[e.Index], iconFont);
            float textWidth = e.Graphics.MeasureString(names[e.Index], textFont).Width;
            float totalWidth = iconSize.Width + 10 + textWidth;
            float startX = r.X + (r.Width - totalWidth) / 2;
            float y = r.Y + (r.Height - 24) / 2;
            e.Graphics.DrawString(icons[e.Index], iconFont, brush, startX, y);
            e.Graphics.DrawString(names[e.Index], textFont, brush, startX + iconSize.Width + 6, y + 3);
        }

        private static void FillRoundedRectangleCompat(Graphics g, Brush brush, RectangleF bounds, float radius)
        {
            float d = radius * 2F;
            using var path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            g.FillPath(brush, path);
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
        private void btnCancel_Click_Layout(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnResetBasicTab_Click(object? sender, EventArgs e)
        {
            if (_mode == FormMode.Add)
            {
                txtSoHieu.Clear();
                txtHoTen.Clear();
                txtNoiSinh.Clear();
                txtCCCD.Clear();
                txtSoDienThoai.Clear();
                cboGioiTinh.SelectedIndex = -1;
                cboPersonnelType.SelectedIndex = 0;
                dtNgaySinh.Value = DateTime.Today.AddYears(-18);
                dtNgayCapCCCD.Value = DateTime.Today;
                epValidation.Clear();
                UpdateAge();
            }
            else
            {
                MessageBox.Show("Chức năng Làm mới ở chế độ chỉnh sửa không xóa dữ liệu đã lưu.", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void chkDangVien_CheckedChanged(
            object sender,
            EventArgs e)
        {
            dtDangDuBi.Enabled =
                chkDangVien.Checked;

            dtDangChinhThuc.Enabled =
                chkDangVien.Checked;

            bool enabled =
                chkDangVien.Checked;

            dtDangDuBi.Enabled =
                enabled;

            dtDangChinhThuc.Enabled =
                enabled;

            lblDangDuBi.Enabled =
                enabled;

            lblDangChinhThuc.Enabled =
                enabled;
        }

        private async Task<bool> ValidateInputAsync()
        {
            bool soHieuExists =
                await _service
                    .IsSoHieuExistsAsync(
                        txtSoHieu.Text);

            if (_mode == FormMode.Add
                && soHieuExists)
            {
                SetValidationError(
                    txtSoHieu,
                    "Số hiệu đã tồn tại");

                SyncValidationUi();
                FocusControl(txtSoHieu);

                return false;
            }

            bool cccdExists =
                await _service
                    .IsCCCDExistsAsync(
                        txtCCCD.Text.Trim(),
                        _mode == FormMode.Edit
                            ? _personnelId
                            : null);

            if (cccdExists)
            {
                SetValidationError(
                    txtCCCD,
                    "CCCD đã tồn tại");

                SyncValidationUi();
                FocusControl(txtCCCD);

                return false;
            }

            bool phoneExists =
                await _service
                    .IsPhoneExistsAsync(
                        txtSoDienThoai.Text.Trim(),
                        _mode == FormMode.Edit
                            ? _personnelId
                            : null);

            if (phoneExists)
            {
                SetValidationError(
                    txtSoDienThoai,
                    "Số điện thoại đã tồn tại");

                SyncValidationUi();
                FocusControl(txtSoDienThoai);

                return false;
            }

            if (dtNgaySinh.Value > DateTime.Today)
            {
                SetValidationError(
                    dtNgaySinh,
                    "Ngày sinh không hợp lệ");
                SyncValidationUi();

                return false;
            }

            return true;
        }

        private void FillPersonnel(Personnel p)
        {
            // =====================
            // BASIC
            // =====================

            p.PersonnelType =
                cboPersonnelType
                .SelectedIndex + 1;

            p.SoHieu =
                txtSoHieu.Text.Trim();

            p.HoTen =
                txtHoTen.Text.Trim();

            p.DonVi =
                txtDonVi.Text.Trim();

            p.GioiTinh =
                cboGioiTinh.Text;

            p.NgaySinh =
                dtNgaySinh.Value;

            p.NoiSinh =
                txtNoiSinh.Text.Trim();

            // =====================
            // ADDRESS
            // =====================

            p.QueQuanMoi =
                txtQueQuanMoi.Text.Trim();

            p.QueQuanCu =
                txtQueQuanCu.Text.Trim();

            p.TinhMoi =
                txtTinhMoi.Text.Trim();

            p.TinhCu =
                txtTinhCu.Text.Trim();

            p.HKTTMoi =
                txtHKTTMoi.Text.Trim();

            p.HKTTCu =
                txtHKTTCu.Text.Trim();

            // =====================
            // WORK
            // =====================

            p.ViTriCongTac =
                txtViTriCongTac.Text.Trim();

            p.CapBacHam =
                cboCapBacHam.Text.Trim();

            p.NgayNhanCapBac =
                dtNgayNhanCapBac.Value;

            p.ThangNamVaoNganh =
                dtThangNamVaoNganh.Value;

            p.ThangNamNhanCongTac =
                dtThangNamNhanCongTac.Value;

            p.ChucVu =
                txtChucVu.Text.Trim();

            p.NgayBoNhiem =
                dtNgayBoNhiem.Value;

            // Hệ số lương

            if (decimal.TryParse(
                txtHeSoLuong.Text,
                out decimal heSoLuong))
            {
                p.HeSoLuong =
                    heSoLuong;
            }

            // =====================
            // EDUCATION
            // =====================

            p.TrinhDoTinHoc =
                txtTrinhDoTinHoc.Text.Trim();

            p.TrinhDoNgoaiNgu =
                txtTrinhDoNgoaiNgu.Text.Trim();

            p.TrinhDoVanHoa =
                txtTrinhDoVanHoa.Text.Trim();

            p.TrinhDoChuyenMon =
                txtTrinhDoChuyenMon.Text.Trim();

            p.ChuyenNganhDaoTao =
                txtChuyenNganhDaoTao.Text.Trim();

            p.ThangNamTotNghiep =
                dtThangNamTotNghiep.Value;

            p.TrinhDoLyLuanChinhTri =
                txtTrinhDoLyLuanChinhTri.Text.Trim();

            // =====================
            // PARTY
            // =====================

            p.DangVien =
                chkDangVien.Checked;

            if (chkDangVien.Checked)
            {
                p.NgayVaoDangDuBi =
                    dtDangDuBi.Value;

                p.NgayVaoDangChinhThuc =
                    dtDangChinhThuc.Value;
            }

            // =====================
            // OTHER
            // =====================

            p.CCCD =
                txtCCCD.Text.Trim();

            p.NgayCapCCCD =
                dtNgayCapCCCD.Value;

            p.SoDienThoai =
                txtSoDienThoai.Text.Trim();
        }

        // Hộp thoại thành công theo bản thiết kế: gọn, cân đối, biểu tượng check
        // xanh có các tia nhấn hai bên, tiêu đề xanh đậm và nút OK bo góc.
        private void ShowSuccessDialog(string message)
        {
            using (Form dialog = new Form())
            {
                dialog.Name = "dlgSuccess";
                dialog.Text = string.Empty;
                dialog.FormBorderStyle = FormBorderStyle.None;
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.ClientSize = new Size(210, 174);
                dialog.BackColor = Color.White;
                dialog.ShowInTaskbar = false;
                dialog.MinimizeBox = false;
                dialog.MaximizeBox = false;
                dialog.ControlBox = false;
                dialog.KeyPreview = true;
                dialog.Padding = Padding.Empty;

                dialog.Region = new Region(CreateRoundedRectangle(
                    new Rectangle(0, 0, dialog.ClientSize.Width, dialog.ClientSize.Height), 11));
                dialog.Paint += (sender, e) =>
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    using (Pen borderPen = new Pen(Color.FromArgb(222, 228, 237), 1F))
                    using (GraphicsPath borderPath = CreateRoundedRectangle(
                        new Rectangle(0, 0, dialog.ClientSize.Width - 1, dialog.ClientSize.Height - 1), 11))
                    {
                        e.Graphics.DrawPath(borderPen, borderPath);
                    }
                };

                // Nút đóng tự vẽ để dấu X luôn hiển thị đầy đủ ở các mức DPI.
                Button btnClose = new Button
                {
                    Name = "btnCloseSuccess",
                    Text = string.Empty,
                    ForeColor = Color.FromArgb(55, 55, 55),
                    BackColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Size = new Size(27, 27),
                    Location = new Point(dialog.ClientSize.Width - 32, 3),
                    Cursor = Cursors.Hand,
                    TabStop = false,
                    UseVisualStyleBackColor = false
                };
                btnClose.FlatAppearance.BorderSize = 0;
                btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(245, 247, 250);
                btnClose.Paint += (sender, e) =>
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    using (Pen xPen = new Pen(Color.FromArgb(55, 55, 55), 1.6F))
                    {
                        xPen.StartCap = LineCap.Round;
                        xPen.EndCap = LineCap.Round;
                        e.Graphics.DrawLine(xPen, 9, 9, 17, 17);
                        e.Graphics.DrawLine(xPen, 17, 9, 9, 17);
                    }
                };
                btnClose.Click += (sender, e) => dialog.Close();
                dialog.Controls.Add(btnClose);

                // Biểu tượng check và các tia xanh hai bên giống bản thiết kế.
                Panel pnlCheck = new Panel
                {
                    Name = "pnlSuccessIcon",
                    Size = new Size(70, 58),
                    Location = new Point(70, 20),
                    BackColor = Color.White
                };
                pnlCheck.Paint += (sender, e) =>
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    Color green = Color.FromArgb(39, 174, 78);
                    using (Pen rayPen = new Pen(green, 2.2F))
                    {
                        rayPen.StartCap = LineCap.Round;
                        rayPen.EndCap = LineCap.Round;
                        e.Graphics.DrawLine(rayPen, 7, 20, 13, 24);
                        e.Graphics.DrawLine(rayPen, 4, 29, 11, 29);
                        e.Graphics.DrawLine(rayPen, 7, 38, 13, 34);
                        e.Graphics.DrawLine(rayPen, 57, 24, 63, 20);
                        e.Graphics.DrawLine(rayPen, 59, 29, 66, 29);
                        e.Graphics.DrawLine(rayPen, 57, 34, 63, 38);
                    }

                    using (SolidBrush haloBrush = new SolidBrush(Color.FromArgb(225, 246, 231)))
                        e.Graphics.FillEllipse(haloBrush, 15, 2, 40, 40);
                    using (SolidBrush greenBrush = new SolidBrush(green))
                        e.Graphics.FillEllipse(greenBrush, 19, 6, 32, 32);
                    using (Pen checkPen = new Pen(Color.White, 3.4F))
                    {
                        checkPen.StartCap = LineCap.Round;
                        checkPen.EndCap = LineCap.Round;
                        e.Graphics.DrawLines(checkPen, new[]
                        {
                            new Point(27, 22), new Point(32, 27), new Point(43, 15)
                        });
                    }
                };
                dialog.Controls.Add(pnlCheck);

                Label lblMessage = new Label
                {
                    Name = "lblSuccessMessage",
                    Text = message,
                    Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(17, 48, 111),
                    TextAlign = ContentAlignment.MiddleCenter,
                    AutoSize = false,
                    Size = new Size(194, 27),
                    Location = new Point(8, 83),
                    BackColor = Color.Transparent,
                    AutoEllipsis = false
                };
                dialog.Controls.Add(lblMessage);

                Button btnOk = new Button
                {
                    Name = "btnSuccessOK",
                    Text = "OK",
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(0, 105, 238),
                    FlatStyle = FlatStyle.Flat,
                    Size = new Size(108, 37),
                    Location = new Point(51, 127),
                    Cursor = Cursors.Hand,
                    DialogResult = DialogResult.OK,
                    UseVisualStyleBackColor = false
                };
                btnOk.FlatAppearance.BorderSize = 0;
                btnOk.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 91, 214);
                btnOk.Region = new Region(CreateRoundedRectangle(
                    new Rectangle(0, 0, btnOk.Width, btnOk.Height), 7));
                btnOk.Click += (sender, e) => dialog.Close();
                dialog.Controls.Add(btnOk);

                dialog.AcceptButton = btnOk;
                dialog.CancelButton = btnClose;
                dialog.KeyDown += (sender, e) =>
                {
                    if (e.KeyCode == Keys.Escape)
                    {
                        dialog.Close();
                        e.Handled = true;
                    }
                };

                dialog.ShowDialog(this);
            }
        }

        private static GraphicsPath CreateRoundedRectangle(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }

        private async Task SaveAddAsync()
        {
            bool isValid =
                await ValidateInputAsync();

            if (!isValid)
                return;

            Personnel p =
                new Personnel();

            FillPersonnel(p);

            await _service.AddAsync(p);

            await AuditHelper.LogAsync(
                AuditAction.AddPersonnel,
                $"Thêm CBCS '{p.HoTen}' (Số hiệu: {p.SoHieu})");

            await _historyService
                .AddHistoryAsync(
                    p.PersonnelID,
                    p.HoTen,
                    "CREATE_PERSONNEL",
                    $"Thêm mới CBCS '{p.HoTen}'");

            ShowSuccessDialog("Thêm CBCS thành công");

            DialogResult =
                DialogResult.OK;

            Close();
        }

        private async void btnSave_Click(
            object sender,
            EventArgs e)
        {
            if (!ValidateForm())
                return;

            if (_mode == FormMode.Add)
            {
                await SaveAddAsync();
            }
            else
            {
                await SaveEditAsync();
            }
        }

        private async Task LoadPersonnelAsync()
        {

            if (!_personnelId.HasValue)
            {
                MessageBox.Show(
                    "Không xác định CBCS.");

                return;
            }

            var p =
                await _service
                    .GetByIdAsync(
                        _personnelId.Value);

            if (p == null)
            {
                MessageBox.Show("Không tìm thấy dữ liệu");
                return;
            }

            cboPersonnelType.SelectedIndex =
                p.PersonnelType - 1;

            txtSoHieu.Text =
                p.SoHieu;

            txtHoTen.Text =
                p.HoTen;

            txtDonVi.Text =
                p.DonVi;

            cboGioiTinh.SelectedItem = p.GioiTinh;
            if (cboGioiTinh.SelectedIndex < 0)
                cboGioiTinh.Text = p.GioiTinh;

            dtNgaySinh.Value =
                p.NgaySinh ?? DateTime.Now;

            txtNoiSinh.Text =
                p.NoiSinh;

            txtCCCD.Text =
                p.CCCD;

            txtSoDienThoai.Text =
                p.SoDienThoai;

            chkDangVien.Checked =
                p.DangVien;

            // ADDRESS

            txtQueQuanMoi.Text =
                p.QueQuanMoi;

            if (_txtBasicQueQuan != null)
                _txtBasicQueQuan.Text = p.QueQuanMoi;

            txtQueQuanCu.Text =
                p.QueQuanCu;

            txtTinhMoi.Text =
                p.TinhMoi;

            txtTinhCu.Text =
                p.TinhCu;

            txtHKTTMoi.Text =
                p.HKTTMoi;

            txtHKTTCu.Text =
                p.HKTTCu;

            // WORK

            txtViTriCongTac.Text =
                p.ViTriCongTac;

            cboCapBacHam.Text =
                p.CapBacHam;

            txtChucVu.Text =
                p.ChucVu;

            txtHeSoLuong.Text =
                p.HeSoLuong.ToString();

            dtNgayNhanCapBac.Value =
                p.NgayNhanCapBac ?? DateTime.Now;

            dtThangNamVaoNganh.Value =
                p.ThangNamVaoNganh ?? DateTime.Now;

            dtThangNamNhanCongTac.Value =
                p.ThangNamNhanCongTac ?? DateTime.Now;

            dtNgayBoNhiem.Value =
                p.NgayBoNhiem ?? DateTime.Now;

            // EDUCATION

            txtTrinhDoTinHoc.Text =
                p.TrinhDoTinHoc;

            txtTrinhDoNgoaiNgu.Text =
                p.TrinhDoNgoaiNgu;

            txtTrinhDoVanHoa.Text =
                p.TrinhDoVanHoa;

            txtTrinhDoChuyenMon.Text =
                p.TrinhDoChuyenMon;

            txtChuyenNganhDaoTao.Text =
                p.ChuyenNganhDaoTao;

            txtTrinhDoLyLuanChinhTri.Text =
                p.TrinhDoLyLuanChinhTri;

            dtThangNamTotNghiep.Value =
                p.ThangNamTotNghiep ?? DateTime.Now;

            // PARTY

            if (p.DangVien)
            {
                dtDangDuBi.Value =
                    p.NgayVaoDangDuBi ?? DateTime.Now;

                dtDangChinhThuc.Value =
                    p.NgayVaoDangChinhThuc ?? DateTime.Now;
            }

            // OTHER

            dtNgayCapCCCD.Value =
                p.NgayCapCCCD ?? DateTime.Now;
        }

        private async void FrmPersonnelDetail_Load(
            object sender,
            EventArgs e)
        {
            SetupButtons();

            SetupTabControl();

            SetupTextBoxes();

            SetupHeader();

            if (_mode == FormMode.Edit)
            {
                await LoadPersonnelAsync();
            }

            UpdateAge();
        }

        private async Task SaveEditAsync()
        {
            if (!_personnelId.HasValue)
            {
                MessageBox.Show(
                    "Không xác định CBCS.");

                return;
            }

            bool isValid =
                await ValidateInputAsync();

            if (!isValid)
                return;

            var p =
                await _service
                .GetByIdAsync(_personnelId.Value);

            if (p == null)
                return;

            FillPersonnel(p);

            await _service.UpdateAsync(p);

            await AuditHelper.LogAsync(
                AuditAction.UpdatePersonnel,
                $"Cập nhật hồ sơ CBCS '{p.HoTen}' (Số hiệu: {p.SoHieu})");

            await _historyService
                .AddHistoryAsync(
                    p.PersonnelID,
                    p.HoTen,
                    "UPDATE_PERSONNEL",
                    $"Cập nhật hồ sơ CBCS '{p.HoTen}'");

            ShowSuccessDialog("Cập nhật thành công");

            DialogResult =
                DialogResult.OK;

            Close();
        }


        private static IEnumerable<Control> GetAllControls(Control parent)
        {
            foreach (Control child in parent.Controls)
            {
                yield return child;
                foreach (Control descendant in GetAllControls(child))
                    yield return descendant;
            }
        }


        private void btnResetBasicTabInvalidate()
        {
            var reset = Controls.Find("btnResetBasicTab", false).FirstOrDefault() as Button;
            if (reset == null) return;
            reset.FlatStyle = FlatStyle.Flat;
            reset.FlatAppearance.BorderSize = 0;
            reset.BackColor = Color.White;
            RoundedControlHelper.SetRounded(reset, 9);
            reset.Paint -= ResetButton_PaintRoundedBorder;
            reset.Paint += ResetButton_PaintRoundedBorder;
            reset.Invalidate();
        }

        private void ResetButton_PaintRoundedBorder(object? sender, PaintEventArgs e)
        {
            if (sender is not Button button || button.Width < 4 || button.Height < 4)
                return;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var path = RoundedRectangle(
                new Rectangle(1, 1, button.Width - 3, button.Height - 3), 9);
            using var pen = new Pen(Color.FromArgb(153, 183, 224), 1.2F);
            e.Graphics.DrawPath(pen, path);
        }

        private void ApplyRoundedInputs()
        {
            // IMPORTANT: this method only styles input controls. It does not change
            // tab widths, tab layout, form dimensions, or any form behaviour.
            foreach (Control control in GetAllControls(this).Where(c => c is Button && c.Name != "btnResetBasicTab"))
            {
                try { RoundedControlHelper.SetRounded(control, 9); }
                catch { /* Keep the native button appearance if a region cannot be applied. */ }
            }

            foreach (ComboBox combo in GetAllControls(this).OfType<ComboBox>())
            {
                // Flat rendering prevents the native square 3D edge from competing
                // with the rounded outline drawn by the parent container.
                combo.FlatStyle = FlatStyle.Flat;
                combo.BackColor = Color.White;
            }

            // Apply a rounded clipping region to every input, including controls
            // created dynamically on the basic-information tab. Rebuild the region
            // after resizing so the corner radius stays consistent and the outline
            // is not clipped using stale dimensions.
            foreach (Control input in GetAllControls(this).Where(c =>
                c is TextBox || c is ComboBox || c is DateTimePicker ||
                c is MaskedTextBox || c is NumericUpDown))
            {
                input.Resize -= InputControl_Resize;
                input.Resize += InputControl_Resize;
                ApplyRoundedInputRegion(input);

                // Draw the outline on the input's own native window after native painting.
                // Parent painting is covered by child controls and can leave the border missing.
                if (!_roundedInputBorderWindows.ContainsKey(input))
                {
                    var borderWindow = new RoundedInputBorderWindow(input);
                    _roundedInputBorderWindows.Add(input, borderWindow);
                    borderWindow.Attach();
                }

                // As soon as the user edits an invalid field, remove the inline error
                // and restore the normal outline. Validation is re-run when saving.
                input.TextChanged -= InputValidation_TextChanged;
                input.TextChanged += InputValidation_TextChanged;
                input.Enter -= InputValidation_Enter;
                input.Enter += InputValidation_Enter;
                if (input is ComboBox combo)
                {
                    combo.SelectedIndexChanged -= InputValidation_SelectionChanged;
                    combo.SelectedIndexChanged += InputValidation_SelectionChanged;
                }
                if (input is DateTimePicker datePicker)
                {
                    datePicker.ValueChanged -= InputValidation_ValueChanged;
                    datePicker.ValueChanged += InputValidation_ValueChanged;
                }
            }

            foreach (Control control in _validationMessages.Keys.ToList())
                UpdateValidationPresentation(control);
        }

        private void InputValidation_TextChanged(object? sender, EventArgs e)
        {
            if (sender is Control control && _invalidControls.Contains(control))
                ClearError(control);
        }

        private void InputValidation_Enter(object? sender, EventArgs e)
        {
            if (sender is Control control && _validationLabels.TryGetValue(control, out Label? label))
                label.BringToFront();
        }

        private void InputValidation_SelectionChanged(object? sender, EventArgs e)
        {
            if (sender is Control control && _invalidControls.Contains(control))
                ClearError(control);
        }

        private void InputValidation_ValueChanged(object? sender, EventArgs e)
        {
            if (sender is Control control && _invalidControls.Contains(control))
                ClearError(control);
        }

        private void InputControl_Resize(object? sender, EventArgs e)
        {
            if (sender is Control input)
                ApplyRoundedInputRegion(input);
        }

        private static void ApplyRoundedInputRegion(Control input)
        {
            if (input.IsDisposed || input.Width < 8 || input.Height < 8)
                return;

            try
            {
                using var path = RoundedRectangle(
                    new Rectangle(0, 0, input.Width - 1, input.Height - 1), 7);
                var newRegion = new Region(path);
                var oldRegion = input.Region;
                input.Region = newRegion;
                oldRegion?.Dispose();
            }
            catch
            {
                // Keep the input usable if Windows rejects a region for a native control.
            }
        }

        private void StyleActionButton(
            Button button,
            Color color)
        {
            button.FlatStyle =
                FlatStyle.Flat;

            button.FlatAppearance.BorderSize = 0;

            button.BackColor =
                color;

            button.ForeColor =
                Color.White;

            button.Font =
                new Font(
                    "Segoe UI",
                    13F,
                    FontStyle.Bold);

            RoundedControlHelper
                .SetRounded(
                    button,
                    12);
        }

        private void SetupButtons()
        {
            StyleActionButton(
                btnSave,
                Color.FromArgb(0, 103, 235));

            StyleActionButton(
                btnCancel,
                Color.FromArgb(242, 53, 53));
        }

        private void SetupTabControl()
        {
            // Keep the owner-drawn, full-width tab strip configured in
            // ApplyBasicTabDesign; the old 120x35 setting caused the overlap.
            tabMain.Appearance = TabAppearance.FlatButtons;
            tabMain.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabMain.SizeMode = TabSizeMode.Fixed;
            tabMain.Padding = new Point(1, 1);
            tabMain.BackColor = BackColor;
            UpdateTabLayout();
        }

        private void StyleTextBox(
            TextBox txt)
        {
            txt.Font =
                new Font(
                    "Segoe UI",
                    12F);

            txt.BorderStyle =
                BorderStyle.None;
        }

        private void SetupTextBoxes()
        {
            StyleTextBox(txtSoHieu);

            StyleTextBox(txtHoTen);

            StyleTextBox(txtNoiSinh);

            StyleTextBox(txtCCCD);

            StyleTextBox(txtSoDienThoai);

            StyleTextBox(txtChucVu);

            StyleTextBox(txtDonVi);
        }

        private void SetupHeader()
        {
            // Use the exact heading/subheading shown in the approved mockup
            // for both add and edit modes.
            lblTitle.Text = "THÔNG TIN CBCS";
            lblSubTitle.Text = "Quản lý, cập nhật thông tin cán bộ, chiến sĩ";
        }

        private void ClearValidationErrors()
        {
            epValidation.Clear();
            _validationMessages.Clear();
            _invalidControls.Clear();

            foreach (var pair in _validationLabels)
                pair.Value.Visible = false;

            foreach (var pair in _roundedInputBorderWindows)
                pair.Value.IsInvalid = false;
        }

        private void SetValidationError(Control control, string message)
        {
            if (control == null) return;

            // Keep ErrorProvider's message empty so its red X icon is never displayed.
            epValidation.SetIconPadding(control, -1000);
            epValidation.SetError(control, string.Empty);

            if (string.IsNullOrWhiteSpace(message))
            {
                _validationMessages.Remove(control);
                _invalidControls.Remove(control);
                if (_validationLabels.TryGetValue(control, out Label? oldLabel))
                    oldLabel.Visible = false;
                if (_roundedInputBorderWindows.TryGetValue(control, out RoundedInputBorderWindow? oldBorder))
                    oldBorder.IsInvalid = false;
                return;
            }

            _validationMessages[control] = message;
            _invalidControls.Add(control);
            EnsureValidationLabel(control);
            UpdateValidationPresentation(control);
        }

        private void SyncValidationUi()
        {
            // ValidationHelper writes into ErrorProvider; transfer those messages to
            // the inline label + red rounded border, then clear native error icons.
            foreach (Control control in GetAllControls(this).Where(c =>
                c is TextBox || c is ComboBox || c is DateTimePicker ||
                c is MaskedTextBox || c is NumericUpDown))
            {
                string message = epValidation.GetError(control);
                if (!string.IsNullOrWhiteSpace(message))
                    SetValidationError(control, message);
                else if (!_validationMessages.ContainsKey(control))
                    epValidation.SetError(control, string.Empty);
            }
        }

        private void EnsureValidationLabel(Control control)
        {
            if (_validationLabels.ContainsKey(control) || control.Parent == null)
                return;

            var label = new Label
            {
                Name = "inlineValidation_" + control.Name,
                AutoSize = false,
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(220, 35, 35),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(5, 0, 2, 0),
                Visible = false,
                Cursor = Cursors.IBeam,
                Tag = control
            };

            // Keep the message inside the input bounds. Clicking it focuses the input,
            // so the user can immediately correct the value.
            label.Click += (_, _) =>
            {
                if (control.CanFocus) control.Focus();
            };
            control.Parent.Controls.Add(label);
            label.BringToFront();
            _validationLabels[control] = label;
            control.LocationChanged += (_, _) => PositionValidationLabel(control);
            control.SizeChanged += (_, _) => PositionValidationLabel(control);
            control.ParentChanged += (_, _) => PositionValidationLabel(control);
        }

        private void PositionValidationLabel(Control control)
        {
            if (!_validationLabels.TryGetValue(control, out Label? label) || control.Parent == null)
                return;

            label.Parent = control.Parent;
            label.SetBounds(control.Left + 5, control.Top + 1,
                Math.Max(0, control.Width - 12), Math.Max(0, control.Height - 2));
            label.BringToFront();
        }

        private void UpdateValidationPresentation(Control control)
        {
            EnsureValidationLabel(control);
            if (!_validationLabels.TryGetValue(control, out Label? label)) return;

            string message = _validationMessages.TryGetValue(control, out string? value) ? value : string.Empty;
            label.Text = message;
            bool empty = IsInputEmpty(control);
            // Render the validation message inside the field for every invalid state.
            // Editing the field clears the message and restores visibility of the value.
            label.Visible = !string.IsNullOrWhiteSpace(message);
            PositionValidationLabel(control);

            if (_roundedInputBorderWindows.TryGetValue(control, out RoundedInputBorderWindow? border))
                border.IsInvalid = !string.IsNullOrWhiteSpace(message);

            if (!string.IsNullOrWhiteSpace(message) && !empty && toolTipMain != null)
                toolTipMain.SetToolTip(control, message);
        }

        private static bool IsInputEmpty(Control control)
        {
            if (control is TextBoxBase textBox) return string.IsNullOrWhiteSpace(textBox.Text);
            if (control is ComboBox combo) return combo.SelectedIndex < 0 || string.IsNullOrWhiteSpace(combo.Text);
            if (control is NumericUpDown numeric) return numeric.Value == 0;
            return false;
        }

        private bool ValidateForm()
        {
            ClearValidationErrors();

            Control? firstErrorControl = null;

            bool isValid = true;

            // Validate directly into the inline-message system. Do not use the
            // ErrorProvider for these fields, because it draws the red X icon.
            string soHieu = txtSoHieu.Text.Trim();
            if (string.IsNullOrWhiteSpace(soHieu))
            {
                SetValidationError(txtSoHieu, "Nhập số hiệu");
                firstErrorControl ??= txtSoHieu;
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtHoTen.Text.Trim()))
            {
                SetValidationError(txtHoTen, "Nhập họ tên");
                firstErrorControl ??= txtHoTen;
                isValid = false;
            }

            string cccd = txtCCCD.Text.Trim();
            if (string.IsNullOrWhiteSpace(cccd))
            {
                SetValidationError(txtCCCD, "Nhập CCCD");
                firstErrorControl ??= txtCCCD;
                isValid = false;
            }
            else if (cccd.Length != 12 || !cccd.All(char.IsDigit))
            {
                SetValidationError(txtCCCD, "CCCD phải gồm 12 số");
                firstErrorControl ??= txtCCCD;
                isValid = false;
            }

            string phone = txtSoDienThoai.Text.Trim();
            if (string.IsNullOrWhiteSpace(phone))
            {
                SetValidationError(txtSoDienThoai, "Nhập số điện thoại");
                firstErrorControl ??= txtSoDienThoai;
                isValid = false;
            }
            else if (!phone.StartsWith("0", StringComparison.Ordinal))
            {
                SetValidationError(txtSoDienThoai, "Số điện thoại không hợp lệ");
                firstErrorControl ??= txtSoDienThoai;
                isValid = false;
            }
            else if (phone.Length != 10 || !phone.All(char.IsDigit))
            {
                SetValidationError(txtSoDienThoai, "Số điện thoại chỉ bao gồm 10 số");
                firstErrorControl ??= txtSoDienThoai;
                isValid = false;
            }

            if (!string.IsNullOrWhiteSpace(
                    txtHeSoLuong.Text)
                &&
                !ValidationHelper.IsHeSoLuong(
                    txtHeSoLuong.Text))
            {
                SetValidationError(
                    txtHeSoLuong,
                    "Hệ số lương không hợp lệ");

                firstErrorControl ??=
                    txtHeSoLuong;

                isValid = false;
            }

            if (cboGioiTinh.SelectedIndex < 0)
            {
                SetValidationError(
                    cboGioiTinh,
                    "Chọn giới tính");

                firstErrorControl ??= cboGioiTinh;

                isValid = false;
            }

            if (cboPersonnelType.SelectedIndex < 0)
            {
                SetValidationError(
                    cboPersonnelType,
                    "Chọn loại CBCS");

                firstErrorControl ??= cboPersonnelType;

                isValid = false;
            }

            SyncValidationUi();

            if (firstErrorControl != null)
            {
                FocusControl(
                    firstErrorControl);
            }

            if (!isValid)
            {
                FocusControl(firstErrorControl!);

                return false;
            }

            return isValid;
        }

        private void txtHoTen_TextChanged(
            object sender,
            EventArgs e)
        {
            ClearError(txtHoTen);
        }

        private void txtCCCD_TextChanged(
            object sender,
            EventArgs e)
        {
            ClearError(txtCCCD);
        }

        private void txtSoHieu_TextChanged(
            object sender,
            EventArgs e)
        {
            ClearError(txtSoHieu);
        }

        private void ClearError(
            Control control)
        {
            SetValidationError(control, string.Empty);
        }

        private void FocusControl(
            Control control)
        {
            Control? current = control;

            while (current != null)
            {
                if (current is TabPage page)
                {
                    if (page.Parent is TabControl tabControl)
                    {
                        tabControl.SelectedTab = page;
                    }

                    break;
                }

                current = current.Parent;
            }

            control.Focus();
        }

        private void TxtNumberOnly_KeyPress(
            object? sender,
            KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TxtHeSoLuong_KeyPress(
            object? sender,
            KeyPressEventArgs e)
        {
            TextBox txt =
                (TextBox)sender!;

            // Backspace

            if (char.IsControl(e.KeyChar))
                return;

            // Số

            if (char.IsDigit(e.KeyChar))
            {
                string futureText =
                    txt.Text.Insert(
                        txt.SelectionStart,
                        e.KeyChar.ToString());

                int dotIndex =
                    futureText.IndexOf('.');

                if (dotIndex >= 0)
                {
                    string decimalPart =
                        futureText[(dotIndex + 1)..];

                    if (decimalPart.Length > 1)
                    {
                        e.Handled = true;
                    }
                }

                return;
            }

            // Dấu chấm

            if (e.KeyChar == '.')
            {
                if (txt.Text.Contains('.'))
                {
                    e.Handled = true;
                }

                return;
            }

            e.Handled = true;
        }

        private void TxtHeSoLuong_Leave(
            object? sender,
            EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(
                txtHeSoLuong.Text))
                return;

            if (decimal.TryParse(
                txtHeSoLuong.Text,
                out decimal value))
            {
                txtHeSoLuong.Text =
                    value.ToString("0.0");
            }
            else
            {
                txtHeSoLuong.Text = "";
            }
        }

        private void txtHoTen_Leave(
            object? sender,
            EventArgs e)
        {
            txtHoTen.Text =
                ValidationHelper
                    .ToTitleCase(
                        txtHoTen.Text);
        }

        private void txtSoHieu_Leave(
            object? sender,
            EventArgs e)
        {
            txtSoHieu.Text =
                txtSoHieu.Text
                    .Trim()
                    .ToUpper();
        }

        private void txtNoiSinh_Leave(
            object? sender,
            EventArgs e)
        {
            txtNoiSinh.Text =
                ValidationHelper
                    .ToTitleCase(
                        txtNoiSinh.Text);
        }

        private void Address_Leave(
            object? sender,
            EventArgs e)
        {
            TextBox txt =
                (TextBox)sender!;

            txt.Text =
                ValidationHelper
                    .ToTitleCase(
                        txt.Text);
        }

        private void UpdateAge()
        {
            int age =
                DateTime.Today.Year
                - dtNgaySinh.Value.Year;

            if (dtNgaySinh.Value.Date >
                DateTime.Today.AddYears(-age))
            {
                age--;
            }

            lblTuoi.Text =
                $"Tuổi: {age}";
        }
    }


    // Removes the native rectangular frame around the tab strip/content area.
    // Owner-drawn tabs and the rounded information card provide the visible borders.
    internal class BorderlessTabControl : TabControl
    {
        private const int WM_NCPAINT = 0x0085;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                const int WS_BORDER = 0x00800000;
                const int WS_EX_CLIENTEDGE = 0x00000200;
                cp.Style &= ~WS_BORDER;
                cp.ExStyle &= ~WS_EX_CLIENTEDGE;
                return cp;
            }
        }

        // Suppress the native non-client frame which otherwise leaves a gray
        // rectangular outline around the tab strip and page area on Windows.
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_NCPAINT)
            {
                m.Result = IntPtr.Zero;
                return;
            }

            base.WndProc(ref m);
        }
    }


    /// <summary>Draws a complete rounded outline on top of native input controls.</summary>
    internal sealed class RoundedInputBorderWindow : NativeWindow
    {
        private const int WM_PAINT = 0x000F;
        private const int WM_NCPAINT = 0x0085;
        private const int WM_SIZE = 0x0005;
        private readonly Control _control;
        private bool _attached;
        private bool _isInvalid;

        public bool IsInvalid
        {
            get => _isInvalid;
            set
            {
                if (_isInvalid == value) return;
                _isInvalid = value;
                if (_control.IsHandleCreated) _control.Invalidate();
                DrawRoundedBorder();
            }
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        public RoundedInputBorderWindow(Control control)
        {
            _control = control;
            _control.HandleCreated += Control_HandleCreated;
            _control.HandleDestroyed += Control_HandleDestroyed;
        }

        public void Attach()
        {
            if (_control.IsHandleCreated && !_attached)
            {
                AssignHandle(_control.Handle);
                _attached = true;
                _control.Invalidate();
            }
        }

        private void Control_HandleCreated(object? sender, EventArgs e) => Attach();

        private void Control_HandleDestroyed(object? sender, EventArgs e)
        {
            if (_attached)
            {
                ReleaseHandle();
                _attached = false;
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_PAINT || m.Msg == WM_NCPAINT || m.Msg == WM_SIZE)
                DrawRoundedBorder();
        }

        private void DrawRoundedBorder()
        {
            if (_control.IsDisposed || !_control.IsHandleCreated ||
                _control.Width < 8 || _control.Height < 8)
                return;

            IntPtr hdc = GetWindowDC(_control.Handle);
            if (hdc == IntPtr.Zero) return;

            try
            {
                using Graphics graphics = Graphics.FromHdc(hdc);
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.PixelOffsetMode = PixelOffsetMode.Half;
                using var path = CreateRoundedPath(
                    new RectangleF(0.65F, 0.65F, _control.Width - 1.3F, _control.Height - 1.3F), 8F);
                Color borderColor = _isInvalid
                    ? Color.FromArgb(220, 35, 35)
                    : Color.FromArgb(125, 135, 150);
                using var pen = new Pen(borderColor, _isInvalid ? 1.8F : 1.2F);
                graphics.DrawPath(pen, path);
            }
            catch
            {
                // Do not interrupt normal input behaviour if drawing fails for a frame.
            }
            finally
            {
                ReleaseDC(_control.Handle, hdc);
            }
        }

        private static GraphicsPath CreateRoundedPath(RectangleF bounds, float radius)
        {
            float d = Math.Min(radius * 2F, Math.Min(bounds.Width, bounds.Height));
            var path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}