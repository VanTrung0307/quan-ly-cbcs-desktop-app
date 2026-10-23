using Microsoft.EntityFrameworkCore;
using QuanLyCBCS.Data;
using QuanLyCBCS.Forms.CommonForms;
using QuanLyCBCS.Forms.MissionForms;
using QuanLyCBCS.Forms.PersonnelForms;
using QuanLyCBCS.Forms.UserForms;
using QuanLyCBCS.Helpers;
using QuanLyCBCS.Models;
using QuanLyCBCS.Services;
using System.Drawing.Drawing2D;
using System.IO;

namespace QuanLyCBCS.Forms.Auth
{
    public partial class FrmLogin : Form
    {
        private readonly UserService _userService;

        public FrmLogin()
        {
            InitializeComponent();

            _userService = new UserService();

            BuildModernLoginLayout();

            AcceptButton = btnLogin;
            CancelButton = btnExit;

            LoadRememberedUsername();
        }

        private async void btnLogin_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                string username =
                    txtUsername.Text.Trim();

                string password =
                    txtPassword.Text.Trim();

                if (string.IsNullOrWhiteSpace(username))
                {
                    ShowLoginMessage(
                        LoginMessageType.UsernameRequired,
                        "Tên đăng nhập không được để trống",
                        "Vui lòng nhập tên đăng nhập.");

                    return;
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    ShowLoginMessage(
                        LoginMessageType.PasswordRequired,
                        "Mật khẩu không được để trống",
                        "Vui lòng nhập mật khẩu.");

                    return;
                }
                using ApplicationDbContext db =
                    new ApplicationDbContext();

                User? user =
                    await db.Users
                        .FirstOrDefaultAsync(
                            x =>
                                x.Username ==
                                txtUsername.Text.Trim());

                if (user == null)
                {
                    await AuditHelper.LogAsync(
                        AuditAction.FailedLogin,
                        $"Tài khoản không tồn tại: {username}");

                    ShowLoginMessage(
                        LoginMessageType.InvalidCredentials,
                        "Sai tài khoản hoặc mật khẩu",
                        "Vui lòng kiểm tra lại tên đăng nhập và mật khẩu.");
                    return;
                }

                if (user.LockedUntil != null &&
                        user.LockedUntil > DateTime.Now)
                {
                    ShowLoginMessage(
                        LoginMessageType.AccountLocked,
                        "Tài khoản đang bị khóa",
                        $"Tài khoản đang bị khóa đến: {user.LockedUntil.Value.ToString("dd/MM/yyyy HH:mm:ss")}");

                    return;
                }

                bool isValid =
                    PasswordHelper.VerifyPassword(
                        txtPassword.Text.Trim(),
                        user.Password);

                if (!isValid)
                {
                    await AuditHelper.LogAsync(
                        AuditAction.FailedLogin,
                        $"Sai mật khẩu: {user.Username}");

                    user.FailedLoginCount++;

                    if (user.FailedLoginCount >= 5)
                    {
                        user.LockedUntil =
                            DateTime.Now.AddMinutes(15);

                        user.FailedLoginCount = 0;

                        await db.SaveChangesAsync();

                        await AuditHelper.LogAsync(
                            AuditAction.LockAccount,
                            $"Tài khoản {user.Username} bị khóa 15 phút");

                        ShowLoginMessage(
                            LoginMessageType.AccountLocked,
                            "Tài khoản đang bị khóa",
                            "Tài khoản đã bị khóa 15 phút do đăng nhập sai quá nhiều lần.");

                        return;
                    }

                    await db.SaveChangesAsync();

                    ShowLoginMessage(
                        LoginMessageType.InvalidCredentials,
                        "Sai tài khoản hoặc mật khẩu",
                        $"Sai tài khoản hoặc mật khẩu.\n" +
                        $"Lần sai: {user.FailedLoginCount}/5");

                    return;
                }

                CurrentUser.UserId = user.UserId;
                CurrentUser.Username = user.Username;
                CurrentUser.Role = user.Role;

                PermissionService permissionService = new();

                // Nạp quyền hiệu lực của tài khoản:
                // - Chưa có quyền riêng: lấy quyền theo Role.
                // - Đã có quyền riêng: lấy quyền từ UserPermissions.
                await permissionService.RefreshCurrentUserPermissionsAsync();

                if (user.MustChangePassword)
                {
                    ShowLoginMessage(
                        LoginMessageType.Information,
                        "Thông báo",
                        "Bạn phải đổi mật khẩu trước khi sử dụng hệ thống.");

                    using FrmChangePassword frmChange =
                        new FrmChangePassword();

                    if (frmChange.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    user =
                        await db.Users
                            .FirstOrDefaultAsync(
                                x => x.UserId == user.UserId);

                    if (user == null)
                    {
                        return;
                    }
                }

                user.FailedLoginCount = 0;
                user.LockedUntil = null;

                await db.SaveChangesAsync();

                await AuditHelper.LogAsync(
                    AuditAction.Login,
                    "Đăng nhập hệ thống");

                FrmLoading loading =
                    new FrmLoading();

                loading.Show();

                Application.DoEvents();

                await Task.Delay(300);

                FrmPersonnelList frm =
                    new FrmPersonnelList();

                loading.Hide();

                this.Hide();

                frm.ShowDialog();

                this.Show();

                txtUsername.Clear();

                txtPassword.Clear();

                txtUsername.Focus();
            }
            catch (Exception ex)
            {
                ShowLoginMessage(
                    LoginMessageType.Information,
                    "Lỗi",
                    ex.Message);
            }
        }

        private void btnExit_Click(
            object sender,
            EventArgs e)
        {
            Application.Exit();
        }

        private void BuildModernLoginLayout()
        {
            // Cấu hình cửa sổ.
            Text = "Hệ thống Quản lý CBCS";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            MinimumSize = new Size(1100, 700);
            BackColor = Color.FromArgb(244, 247, 252);
            Font = new Font("Segoe UI", 10F);

            // Loại bỏ bố cục cũ, giữ lại các control và event handler.
            Controls.Clear();

            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Margin = Padding.Empty,
                Padding = Padding.Empty,
                BackColor = Color.White
            };

            mainLayout.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 50F));

            mainLayout.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 50F));

            mainLayout.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100F));

            // Vùng bên trái: ảnh nhận diện CSCĐ.
            var leftPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = Padding.Empty,
                BackColor = Color.FromArgb(8, 45, 91)
            };

            var backgroundImage = new CoverPictureBox
            {
                Dock = DockStyle.Fill,
                Image = Properties.Resources.LoginBackground
            };

            leftPanel.Controls.Add(backgroundImage);

            // Vùng bên phải.
            var rightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = Padding.Empty,
                Padding = new Padding(35),
                BackColor = Color.FromArgb(245, 248, 253)
            };

            var card = new Panel
            {
                Size = new Size(850, 880),
                BackColor = Color.White,
                Padding = new Padding(42, 62, 42, 42),
                Anchor = AnchorStyles.None
            };

            // Tạm thời dùng viền nhẹ để phân biệt thẻ với nền.
            card.Paint += (sender, e) =>
            {
                using var pen = new Pen(
                    Color.FromArgb(231, 237, 246));

                e.Graphics.DrawRectangle(
                    pen, 0, 0,
                    card.Width - 1,
                    card.Height - 1);
            };


            void CenterCard()
            {
                int availableWidth = rightPanel.ClientSize.Width - 48;
                int availableHeight = rightPanel.ClientSize.Height - 48;

                int width = Math.Min(850, availableWidth);
                int height = Math.Min(880, availableHeight);

                // Không ép thẻ lớn hơn không gian khả dụng.
                width = Math.Max(360, width);
                height = Math.Max(560, height);

                card.Size = new Size(width, height);

                card.Left = (rightPanel.ClientSize.Width - card.Width) / 2;
                card.Top = (rightPanel.ClientSize.Height - card.Height) / 2;
            }

            rightPanel.Controls.Add(card);
            rightPanel.Resize += (sender, e) => CenterCard();

            var content = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 15,
                BackColor = Color.White,
                Margin = Padding.Empty,
                Padding = Padding.Empty,
                GrowStyle = TableLayoutPanelGrowStyle.FixedSize
            };

            content.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 100F));

            // Dùng chiều cao cố định cho các thành phần tương tác.
            // Chỉ các hàng đệm mới co giãn, tránh ô nhập bị cắt và nút quá cao.
            content.RowStyles.Add(new RowStyle(SizeType.Absolute, 58F)); // 0 Tiêu đề
            content.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F)); // 1 Tiêu đề phụ
            content.RowStyles.Add(new RowStyle(SizeType.Absolute, 6F));  // 2 Gạch xanh
            content.RowStyles.Add(new RowStyle(SizeType.Percent, 28F));  // 3 Đệm
            content.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F)); // 4 Nhãn tài khoản
            content.RowStyles.Add(new RowStyle(SizeType.Absolute, 68F)); // 5 Ô tài khoản
            content.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));  // 6 Đệm
            content.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F)); // 7 Nhãn mật khẩu
            content.RowStyles.Add(new RowStyle(SizeType.Absolute, 68F)); // 8 Ô mật khẩu
            content.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F)); // 9 Ghi nhớ / quên mật khẩu
            content.RowStyles.Add(new RowStyle(SizeType.Percent, 18F));  // 10 Đệm
            content.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F)); // 11 Nút đăng nhập
            content.RowStyles.Add(new RowStyle(SizeType.Absolute, 76F)); // 12 Nút thoát
            content.RowStyles.Add(new RowStyle(SizeType.Percent, 44F));  // 13 Đệm chân trang
            content.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F)); // 14 Phiên bản

            var title = new Label
            {
                Text = "ĐĂNG NHẬP HỆ THỐNG",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 25F, FontStyle.Bold),
                ForeColor = Color.FromArgb(20, 53, 96),
                AutoEllipsis = true
            };

            var subtitle = new Label
            {
                Text = "Quản lý cán bộ chiến sĩ Cảnh sát cơ động",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 12F),
                ForeColor = Color.FromArgb(105, 122, 148)
            };

            var accent = new Panel
            {
                Size = new Size(90, 4),
                BackColor = Color.FromArgb(0, 103, 230),
                Anchor = AnchorStyles.None
            };

            var usernameLabel = CreateLoginLabel("Tên đăng nhập");

            // Khung nhập tên đăng nhập gồm biểu tượng và ô nhập.
            var usernamePanel = CreateInputPanel(
                "user",
                txtUsername);

            txtUsername.Font = new Font("Segoe UI", 12F);
            txtUsername.PlaceholderText = "Nhập tên đăng nhập...";
            txtUsername.TabIndex = 0;

            var passwordLabel = CreateLoginLabel("Mật khẩu");

            // Khung mật khẩu có biểu tượng ổ khóa.
            var passwordPanel = CreateInputPanel(
                "lock",
                txtPassword,
                includeToggleButton: true);

            txtPassword.Font = new Font("Segoe UI", 12F);
            txtPassword.PlaceholderText = "Nhập mật khẩu...";
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.TabIndex = 1;

            var optionsPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Margin = Padding.Empty,
                BackColor = Color.White
            };

            optionsPanel.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 55F));

            optionsPanel.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 45F));

            var chkRemember = new CheckBox
            {
                Name = "chkRemember",
                Text = "Ghi nhớ đăng nhập",
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                ForeColor = Color.FromArgb(31, 55, 88),
                Font = new Font("Segoe UI", 10F)
            };

            chkRemember.CheckedChanged += (sender, e) =>
            {
                if (!chkRemember.Checked)
                {
                    SaveRememberedUsername(string.Empty);
                }
                else if (!string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    SaveRememberedUsername(txtUsername.Text.Trim());
                }
            };

            txtUsername.Leave += (sender, e) =>
            {
                if (chkRemember.Checked)
                {
                    SaveRememberedUsername(txtUsername.Text.Trim());
                }
            };

            card.Resize += (sender, e) =>
            {
                ApplyRoundedCorners(card, 18);
            };

            ApplyRoundedCorners(card, 18);

            var forgotPassword = new LinkLabel
            {
                Text = "Quên mật khẩu?",
                AutoSize = true,
                Anchor = AnchorStyles.Right,
                TextAlign = ContentAlignment.MiddleRight,
                LinkColor = Color.FromArgb(0, 103, 230),
                Font = new Font("Segoe UI", 10F)
            };

            forgotPassword.LinkClicked += (sender, e) =>
            {
                ShowLoginMessage(
                    LoginMessageType.ForgotPassword,
                    "Quên mật khẩu",
                    "Vui lòng liên hệ quản trị viên để được hỗ trợ đặt lại mật khẩu.");
            };

            optionsPanel.Controls.Add(chkRemember, 0, 0);
            optionsPanel.Controls.Add(forgotPassword, 1, 0);

            // Tận dụng nút đăng nhập hiện có để giữ nguyên sự kiện.
            btnLogin.Dock = DockStyle.Fill;
            btnLogin.AutoSize = false;
            btnLogin.Margin = new Padding(0, 3, 0, 3);
            // Dùng khoảng trắng không ngắt dòng để biểu tượng và chữ luôn nằm cùng hàng.
            btnLogin.Text = "↪\u00A0\u00A0Đăng nhập";
            btnLogin.TextAlign = ContentAlignment.MiddleCenter;
            btnLogin.AutoEllipsis = false;
            btnLogin.UseCompatibleTextRendering = false;
            btnLogin.Padding = Padding.Empty;
            btnLogin.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnLogin.BackColor = Color.FromArgb(0, 103, 230);
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.UseVisualStyleBackColor = false;

            btnExit.Dock = DockStyle.Fill;
            btnExit.AutoSize = false;
            btnExit.Margin = new Padding(0, 3, 0, 3);
            // Không cho phép tách biểu tượng và nhãn nút thành hai dòng.
            btnExit.Text = "⏻\u00A0\u00A0Thoát";
            btnExit.TextAlign = ContentAlignment.MiddleCenter;
            btnExit.AutoEllipsis = false;
            btnExit.UseCompatibleTextRendering = false;
            btnExit.Padding = Padding.Empty;
            btnExit.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            btnExit.BackColor = Color.FromArgb(230, 236, 246);
            btnExit.ForeColor = Color.FromArgb(190, 45, 40);
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.Cursor = Cursors.Hand;
            btnExit.UseVisualStyleBackColor = false;

            btnLogin.Resize += (sender, e) =>
            {
                ApplyRoundedCorners(btnLogin, 12);
            };

            btnExit.Resize += (sender, e) =>
            {
                ApplyRoundedCorners(btnExit, 12);
            };

            ApplyRoundedCorners(btnLogin, 12);
            ApplyRoundedCorners(btnExit, 12);

            var footer = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = Color.White,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };

            footer.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 30F));

            footer.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 40F));

            footer.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 30F));

            footer.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100F));

            var footerLineLeft = new Panel
            {
                Height = 1,
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(220, 229, 242),
                Margin = new Padding(0, 0, 12, 0),
                Anchor = AnchorStyles.Left
            };

            var footerLineRight = new Panel
            {
                Height = 1,
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(220, 229, 242),
                Margin = new Padding(12, 0, 0, 0),
                Anchor = AnchorStyles.Right
            };

            var footerText = new Label
            {
                Text = "Quản lý CBCS v2.0",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(105, 122, 148),
                Font = new Font("Segoe UI", 10F),
                AutoSize = false
            };

            footer.Controls.Add(footerLineLeft, 0, 0);
            footer.Controls.Add(footerText, 1, 0);
            footer.Controls.Add(footerLineRight, 2, 0);

            content.Controls.Add(title, 0, 0);
            content.Controls.Add(subtitle, 0, 1);
            content.Controls.Add(accent, 0, 2);

            content.Controls.Add(usernameLabel, 0, 4);
            content.Controls.Add(usernamePanel, 0, 5);

            content.Controls.Add(passwordLabel, 0, 7);
            content.Controls.Add(passwordPanel, 0, 8);

            content.Controls.Add(optionsPanel, 0, 9);
            content.Controls.Add(btnLogin, 0, 11);
            content.Controls.Add(btnExit, 0, 12);
            content.Controls.Add(footer, 0, 14);

            card.Controls.Add(content);

            mainLayout.Controls.Add(leftPanel, 0, 0);
            mainLayout.Controls.Add(rightPanel, 1, 0);

            Controls.Add(mainLayout);

            // Lưu tham chiếu checkbox để nạp trạng thái sau khi dựng UI.
            _chkRemember = chkRemember;

            CenterCard();
        }

        private Panel CreateInputPanel(
            string iconGlyph,
            TextBox textBox,
            bool includeToggleButton = false)
        {
            var host = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = Padding.Empty,
                Padding = Padding.Empty,
                BackColor = Color.White
            };

            // Bố cục gần bản thiết kế: biểu tượng lớn hơn và có khoảng cách rõ với ô nhập.
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                Margin = Padding.Empty,
                Padding = Padding.Empty,
                BackColor = Color.White
            };

            layout.ColumnStyles.Add(
                new ColumnStyle(SizeType.Absolute, 68F));
            layout.ColumnStyles.Add(
                new ColumnStyle(SizeType.Absolute, 14F));
            layout.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 100F));

            layout.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100F));

            // Khung biểu tượng vuông bo góc, dùng cùng kích thước cho cả hai ô.
            // Vẽ biểu tượng trực tiếp để có hình khối được tô đặc, không phụ thuộc font icon.
            var iconPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = Padding.Empty,
                BackColor = Color.FromArgb(232, 239, 249),
                MinimumSize = new Size(68, 68),
                MaximumSize = new Size(68, 68),
                Tag = iconGlyph
            };

            iconPanel.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                Color iconColor = Color.FromArgb(18, 62, 111);
                float cx = iconPanel.ClientSize.Width / 2F;
                float cy = iconPanel.ClientSize.Height / 2F;

                using (var brush = new SolidBrush(iconColor))
                using (var pen = new Pen(iconColor, 3.0F))
                {
                    if (string.Equals(iconPanel.Tag as string, "user", StringComparison.OrdinalIgnoreCase))
                    {
                        // Đầu và vai người dùng được tô đặc.
                        g.FillEllipse(brush, cx - 7.5F, cy - 15F, 15F, 15F);
                        g.FillEllipse(brush, cx - 14F, cy + 2F, 28F, 19F);
                    }
                    else
                    {
                        // Thân ổ khóa tô đặc và quai khóa rõ nét.
                        g.DrawArc(pen, cx - 9F, cy - 16F, 18F, 21F, 180F, 180F);
                        g.FillRectangle(brush, cx - 12F, cy - 5F, 24F, 23F);
                        g.FillEllipse(Brushes.White, cx - 2.3F, cy + 1F, 4.6F, 4.6F);
                        g.FillRectangle(Brushes.White, cx - 1.2F, cy + 4F, 2.4F, 5.2F);
                    }
                }
            };

            iconPanel.Resize += (s, e) =>
                ApplyRoundedCorners(iconPanel, 10);

            ApplyRoundedCorners(iconPanel, 10);
            layout.Controls.Add(iconPanel, 0, 0);

            // Ô nhập có đường viền bo góc.
            var inputBorder = new RoundedInputPanel
            {
                Dock = DockStyle.Fill,
                Margin = Padding.Empty,
                Padding = new Padding(1),
                BackColor = Color.White,
                BorderColor = Color.FromArgb(215, 225, 240),
                CornerRadius = 8
            };

            var inputLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = includeToggleButton ? 2 : 1,
                RowCount = 1,
                Margin = Padding.Empty,
                Padding = Padding.Empty,
                BackColor = Color.White
            };

            inputLayout.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100F));

            inputLayout.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent,
                    includeToggleButton ? 100F : 100F));

            if (includeToggleButton)
            {
                inputLayout.ColumnStyles.Add(
                    new ColumnStyle(SizeType.Absolute, 40F));
            }

            // Panel đệm giúp TextBox một dòng nằm chính giữa theo chiều dọc.
            // Vẫn dùng chính TextBox hiện tại nên dữ liệu và logic đăng nhập không đổi.
            var textBoxHost = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = Padding.Empty,
                Padding = Padding.Empty,
                BackColor = Color.White
            };

            textBox.Dock = DockStyle.None;
            textBox.Margin = Padding.Empty;
            textBox.BorderStyle = BorderStyle.None;
            textBox.BackColor = Color.White;
            textBox.ForeColor = Color.FromArgb(35, 54, 82);
            textBox.Font = new Font("Segoe UI", 12F);
            textBox.TextAlign = HorizontalAlignment.Left;
            textBox.Multiline = false;
            textBox.AutoSize = true;

            void CenterTextBoxVertically()
            {
                int preferredHeight = TextRenderer.MeasureText(
                    "Ag", textBox.Font).Height + 4;
                textBox.Height = preferredHeight;
                textBox.Left = 10;
                textBox.Top = Math.Max(0, (textBoxHost.ClientSize.Height - textBox.Height) / 2);
                textBox.Width = Math.Max(20, textBoxHost.ClientSize.Width - 20);
            }

            textBoxHost.Resize += (s, e) => CenterTextBoxVertically();
            textBox.FontChanged += (s, e) => CenterTextBoxVertically();
            textBoxHost.Controls.Add(textBox);
            CenterTextBoxVertically();
            inputLayout.Controls.Add(textBoxHost, 0, 0);

            // Nút con mắt nằm bên trong ô mật khẩu.
            if (includeToggleButton)
            {
                var btnTogglePassword = new Button
                {
                    Name = "btnTogglePassword",
                    Dock = DockStyle.Fill,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.White,
                    ForeColor = Color.FromArgb(105, 122, 148),
                    Cursor = Cursors.Hand,
                    TabStop = false,
                    Margin = Padding.Empty,
                    Text = string.Empty
                };

                btnTogglePassword.FlatAppearance.BorderSize = 0;
                btnTogglePassword.FlatAppearance.MouseOverBackColor =
                    Color.FromArgb(245, 248, 253);

                // Tự vẽ biểu tượng mắt, không phụ thuộc font icon.
                btnTogglePassword.Paint += (s, e) =>
                {
                    Graphics g = e.Graphics;
                    g.SmoothingMode =
                        System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    float cx = btnTogglePassword.ClientSize.Width / 2F;
                    float cy = btnTogglePassword.ClientSize.Height / 2F;

                    using (var pen = new Pen(
                        Color.FromArgb(105, 122, 148), 1.4F))
                    using (var brush = new SolidBrush(
                        Color.FromArgb(105, 122, 148)))
                    {
                        // Viền mắt.
                        g.DrawArc(
                            pen,
                            cx - 10, cy - 6,
                            20, 12,
                            200, 140);

                        g.DrawArc(
                            pen,
                            cx - 10, cy - 6,
                            20, 12,
                            20, 140);

                        if (textBox.UseSystemPasswordChar)
                        {
                            // Mắt mở.
                            g.DrawEllipse(
                                pen,
                                cx - 3, cy - 3,
                                6, 6);

                            g.FillEllipse(
                                brush,
                                cx - 1.5F, cy - 1.5F,
                                3, 3);
                        }
                        else
                        {
                            // Mắt đóng: thêm đường chéo.
                            g.DrawLine(
                                pen,
                                cx - 9, cy + 8,
                                cx + 9, cy - 8);
                        }
                    }
                };

                btnTogglePassword.Click += (s, e) =>
                {
                    textBox.UseSystemPasswordChar =
                        !textBox.UseSystemPasswordChar;

                    btnTogglePassword.Invalidate();

                    textBox.Focus();
                    textBox.SelectionStart = textBox.Text.Length;
                };

                inputLayout.Controls.Add(btnTogglePassword, 1, 0);
            }

            inputBorder.Controls.Add(inputLayout);
            layout.Controls.Add(inputBorder, 2, 0);

            host.Controls.Add(layout);
            return host;
        }

        private sealed class RoundedInputPanel : Panel
        {
            public Color BorderColor { get; set; } =
                Color.FromArgb(215, 225, 240);

            public int CornerRadius { get; set; } = 8;

            public RoundedInputPanel()
            {
                SetStyle(
                    ControlStyles.UserPaint |
                    ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.OptimizedDoubleBuffer |
                    ControlStyles.ResizeRedraw,
                    true);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);

                e.Graphics.SmoothingMode =
                    System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                float inset = 0.7F;

                var rect = new RectangleF(
                    inset,
                    inset,
                    Math.Max(0, Width - inset * 2),
                    Math.Max(0, Height - inset * 2));

                float diameter = CornerRadius * 2F;

                using (var path =
                    new System.Drawing.Drawing2D.GraphicsPath())
                using (var pen = new Pen(BorderColor, 1F))
                {
                    path.AddArc(
                        rect.X, rect.Y,
                        diameter, diameter,
                        180, 90);

                    path.AddArc(
                        rect.Right - diameter, rect.Y,
                        diameter, diameter,
                        270, 90);

                    path.AddArc(
                        rect.Right - diameter,
                        rect.Bottom - diameter,
                        diameter, diameter,
                        0, 90);

                    path.AddArc(
                        rect.X,
                        rect.Bottom - diameter,
                        diameter, diameter,
                        90, 90);

                    path.CloseFigure();
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }

        private static Label CreateLoginLabel(string text)
        {
            return new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.BottomLeft,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(20, 53, 96),
                Margin = new Padding(0, 0, 0, 3)
            };
        }

        private CheckBox? _chkRemember;

        private string RememberedUsernamePath =>
            Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                "QuanLyCBCS",
                "remembered_username.txt");

        private void SaveRememberedUsername(string username)
        {
            try
            {
                string path = RememberedUsernamePath;
                string? directory = Path.GetDirectoryName(path);

                if (!string.IsNullOrEmpty(directory))
                    Directory.CreateDirectory(directory);

                if (string.IsNullOrWhiteSpace(username))
                {
                    if (File.Exists(path))
                        File.Delete(path);
                }
                else
                {
                    File.WriteAllText(path, username);
                }
            }
            catch
            {
                // Không để lỗi lưu tùy chọn ảnh hưởng đến đăng nhập.
            }
        }

        private void LoadRememberedUsername()
        {
            try
            {
                string path = RememberedUsernamePath;

                if (!File.Exists(path))
                    return;

                string username = File.ReadAllText(path).Trim();

                txtUsername.Text = username;

                if (_chkRemember != null)
                    _chkRemember.Checked = !string.IsNullOrWhiteSpace(username);

                if (string.IsNullOrWhiteSpace(username))
                    txtUsername.Clear();
                else
                    txtPassword.Focus();
            }
            catch
            {
                // Không để lỗi đọc tùy chọn ảnh hưởng đến đăng nhập.
            }
        }


        internal sealed class CoverPictureBox : PictureBox
        {
            public CoverPictureBox()
            {
                DoubleBuffered = true;
                BackColor = Color.FromArgb(8, 45, 91);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);

                if (Image == null ||
                    ClientSize.Width <= 0 ||
                    ClientSize.Height <= 0)
                {
                    return;
                }

                float scale = Math.Max(
                    (float)ClientSize.Width / Image.Width,
                    (float)ClientSize.Height / Image.Height);

                int width = (int)(Image.Width * scale);
                int height = (int)(Image.Height * scale);

                var destination = new Rectangle(
                    (ClientSize.Width - width) / 2,
                    (ClientSize.Height - height) / 2,
                    width,
                    height);

                e.Graphics.InterpolationMode =
                    InterpolationMode.HighQualityBicubic;

                e.Graphics.DrawImage(Image, destination);
            }
        }

        private static void ApplyRoundedCorners(
            Control control,
            int radius)
        {
            if (control.Width <= 0 || control.Height <= 0)
                return;

            int diameter = Math.Min(
                radius * 2,
                Math.Min(control.Width, control.Height));

            var bounds = new Rectangle(
                0,
                0,
                control.Width,
                control.Height);

            using var path = new GraphicsPath();

            path.AddArc(
                bounds.Left,
                bounds.Top,
                diameter,
                diameter,
                180,
                90);

            path.AddArc(
                bounds.Right - diameter,
                bounds.Top,
                diameter,
                diameter,
                270,
                90);

            path.AddArc(
                bounds.Right - diameter,
                bounds.Bottom - diameter,
                diameter,
                diameter,
                0,
                90);

            path.AddArc(
                bounds.Left,
                bounds.Bottom - diameter,
                diameter,
                diameter,
                90,
                90);

            path.CloseFigure();

            Region? oldRegion = control.Region;

            control.Region = new Region(path);

            oldRegion?.Dispose();
        }

        private void ShowLoginMessage(
            LoginMessageType messageType,
            string title,
            string description)
        {
            using (var message = new FrmLoginMessage(
                messageType,
                title,
                description))
            {
                message.ShowDialog(this);
            }
        }
    }
}
