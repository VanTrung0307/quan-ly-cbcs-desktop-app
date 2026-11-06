using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace QuanLyCBCS.Forms.CommonForms
{
    public partial class FrnUpComing : Form
    {
        // Lưu Padding ban đầu của pnlContent để màn hình này có thể tràn kín vùng nội dung,
        // sau đó khôi phục khi người dùng chuyển sang tab khác.
        private Panel? _contentHost;
        private Padding _originalHostPadding;

        public FrnUpComing()
        {
            InitializeComponent();
            ConfigureUpcomingScreen();
        }

        /// <summary>
        /// Cấu hình màn hình chờ để có thể nhúng trực tiếp vào pnlContent của FrmHome.
        /// </summary>
        private void ConfigureUpcomingScreen()
        {
            FormBorderStyle = FormBorderStyle.None;
            TopLevel = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(239, 247, 255);

            Padding = Padding.Empty;
            Margin = Padding.Empty;

            picUpcoming.Dock = DockStyle.Fill;
            picUpcoming.Margin = Padding.Empty;
            // StretchImage giúp ảnh phủ kín toàn bộ vùng nội dung, không còn viền trống
            // do chênh lệch tỷ lệ giữa ảnh 16:9 và kích thước pnlContent.
            picUpcoming.SizeMode = PictureBoxSizeMode.StretchImage;
            picUpcoming.BackColor = BackColor;
            picUpcoming.Image = LoadUpcomingDesign();

            // Tránh để hai tab Nghỉ phép/Cập nhật ứng dụng hiển thị một vùng trắng
            // nếu ảnh chưa được copy vào thư mục chạy ứng dụng.
            if (picUpcoming.Image == null)
            {
                picUpcoming.Visible = false;
                var lblFallback = new Label
                {
                    Name = "lblUpcomingImageMissing",
                    Dock = DockStyle.Fill,
                    Text = "KHÔNG TÌM THẤY ẢNH UpcomingScreen.png\r\n\r\n" +
                           "Vui lòng kiểm tra file trong thư mục assets của dự án.",
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.FromArgb(10, 43, 105),
                    BackColor = BackColor,
                    Font = new Font("Segoe UI", 16F, FontStyle.Bold)
                };
                Controls.Add(lblFallback);
                lblFallback.BringToFront();
            }
        }

        /// <summary>
        /// Khi được nhúng vào pnlContent, bỏ Padding 10px của vùng chứa để ảnh phủ kín
        /// toàn bộ khu vực làm việc. Khi tháo Form khỏi pnlContent, khôi phục Padding cũ.
        /// </summary>
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            if (_contentHost != null && !ReferenceEquals(_contentHost, Parent))
            {
                if (!_contentHost.IsDisposed)
                    _contentHost.Padding = _originalHostPadding;

                _contentHost = null;
            }

            if (_contentHost == null && Parent is Panel hostPanel)
            {
                _contentHost = hostPanel;
                _originalHostPadding = hostPanel.Padding;
                hostPanel.Padding = Padding.Empty;
            }
        }

        /// <summary>
        /// Tìm ảnh trong assets ở thư mục chạy ứng dụng hoặc thư mục dự án.
        /// Cách tìm ngược lên các thư mục cha giúp chạy được cả khi Visual Studio
        /// chưa copy Content vào bin\Debug.
        /// </summary>
        private Image? LoadUpcomingDesign()
        {
            string? imagePath = FindUpcomingImagePath();
            if (string.IsNullOrWhiteSpace(imagePath) || !File.Exists(imagePath))
                return null;

            try
            {
                // Đọc qua MemoryStream để không khóa file ảnh trên ổ đĩa.
                byte[] imageBytes = File.ReadAllBytes(imagePath);
                using (var stream = new MemoryStream(imageBytes))
                using (var source = Image.FromStream(stream))
                {
                    return new Bitmap(source);
                }
            }
            catch (Exception)
            {
                // Không để ảnh thiếu/hỏng làm ứng dụng chính bị dừng.
                return null;
            }
        }

        private string? FindUpcomingImagePath()
        {
            const string imageFileName = "UpcomingScreen.png";
            string[] startingDirectories =
            {
                AppDomain.CurrentDomain.BaseDirectory,
                Application.StartupPath,
                Environment.CurrentDirectory
            };

            foreach (string startDirectory in startingDirectories)
            {
                if (string.IsNullOrWhiteSpace(startDirectory))
                    continue;

                DirectoryInfo? directory = new DirectoryInfo(startDirectory);
                // Tìm tối đa 6 cấp thư mục cha, bao gồm thư mục chạy hiện tại.
                for (int level = 0; directory != null && level <= 6; level++, directory = directory.Parent)
                {
                    string lowerCasePath = Path.Combine(directory.FullName, "assets", imageFileName);
                    if (File.Exists(lowerCasePath))
                        return lowerCasePath;

                    // Hỗ trợ trường hợp thư mục được đặt tên Assets viết hoa.
                    string upperCasePath = Path.Combine(directory.FullName, "Assets", imageFileName);
                    if (File.Exists(upperCasePath))
                        return upperCasePath;
                }
            }

            return null;
        }
    }
}
