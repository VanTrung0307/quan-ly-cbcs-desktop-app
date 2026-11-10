using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyCBCS.Services.Update;
using QuanLyCBCS.Forms.Auth;

namespace QuanLyCBCS.Forms.UpdateForms
{
    public partial class FrmUpdate : Form
    {
        private readonly GitHubUpdateService _updateService = new GitHubUpdateService();
        private readonly CancellationTokenSource _formCts = new CancellationTokenSource();
        private UpdateCheckResult? _latestUpdate;
        private UpdateDownloadResult? _downloadResult;
        private bool _hasAutoChecked;
        private Bitmap? _downloadStateIcon;
        private Bitmap? _successStateIcon;
        private Bitmap? _folderButtonIcon;
        private PictureBox? _downloadStatePicture;
        private bool _downloadSucceeded;

        public FrmUpdate()
        {
            InitializeComponent();

            lblCurrentVersion.Text = "v" + GitHubUpdateService.GetCurrentVersion();
            SetCurrentStatus("● Chưa kiểm tra cập nhật", Color.FromArgb(238, 243, 249), Color.FromArgb(90, 105, 125));
            lblSystemVersionValue.Text = "v" + GitHubUpdateService.GetCurrentVersion();
            lblAppSizeValue.Text = GetApplicationSizeText();
            lblOsValue.Text = Environment.OSVersion.Platform + " " +
                              Environment.OSVersion.Version;

            ConfigureDownloadPanel();

            // FrmUpdate được nhúng trong pnlContent của FrmHome nên phải
            // tự co giãn theo kích thước thực tế của vùng nội dung.
            Load += FrmUpdate_LayoutChanged;
            Resize += FrmUpdate_LayoutChanged;
            flowActions.Resize += FrmUpdate_LayoutChanged;
            Shown += FrmUpdate_Shown;
            FormClosing += FrmUpdate_FormClosing;
            FormClosed += (s, e) => DisposeDownloadIcons();
        }


        private void FrmUpdate_LayoutChanged(object? sender, EventArgs e)
        {
            ApplyHostedLayout();
        }

        private void ApplyHostedLayout()
        {
            if (flowActions == null || pnlLeft == null)
                return;

            int availableWidth = flowActions.ClientSize.Width
                                 - flowActions.Padding.Left
                                 - flowActions.Padding.Right;
            if (availableWidth <= 0)
                return;

            const int gap = 12;
            const int horizontalPadding = 12;
            int buttonWidth = Math.Max(90,
                (flowActions.ClientSize.Width - horizontalPadding * 2 - gap * 2) / 3);

            flowActions.SuspendLayout();
            flowActions.Padding = new Padding(horizontalPadding, 8, horizontalPadding, 8);

            ConfigureHostedActionButton(
                btnCheckUpdate, "⟳  Kiểm tra cập nhật", buttonWidth, true);
            ConfigureHostedActionButton(
                btnUpdateOptions, "⚙  Tùy chọn cập nhật", buttonWidth, false);
            ConfigureHostedActionButton(
                btnUpdateHistory, "◷  Lịch sử cập nhật", buttonWidth, false);

            btnCheckUpdate.Margin = new Padding(0, 0, gap, 0);
            btnUpdateOptions.Margin = new Padding(0, 0, gap, 0);
            btnUpdateHistory.Margin = new Padding(0);

            flowActions.ResumeLayout(true);
        }

        private void ConfigureHostedActionButton(
            Button button, string text, int width, bool primary)
        {
            button.Width = width;
            button.Height = 42;
            button.Margin = Padding.Empty;
            button.Text = text;
            button.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            button.FlatStyle = FlatStyle.Flat;
            // Viền phụ được tự vẽ theo GraphicsPath để không bị Region bo góc cắt mất.
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.BorderColor = Color.FromArgb(155, 190, 240);
            button.Paint -= DrawActionButtonBorder;
            if (!primary)
                button.Paint += DrawActionButtonBorder;
            button.BackColor = primary
                ? Color.FromArgb(0, 91, 220)
                : Color.FromArgb(247, 250, 255);
            button.ForeColor = primary
                ? Color.White
                : Color.FromArgb(8, 36, 125);
            button.Cursor = Cursors.Hand;
            button.FlatAppearance.MouseOverBackColor = primary
                ? Color.FromArgb(0, 105, 235)
                : Color.FromArgb(235, 244, 255);
            button.FlatAppearance.MouseDownBackColor = primary
                ? Color.FromArgb(0, 78, 190)
                : Color.FromArgb(224, 237, 255);
        }

        private async void FrmUpdate_Shown(object? sender, EventArgs e)
        {
            if (_hasAutoChecked || !chkAutoCheck.Checked)
                return;

            _hasAutoChecked = true;
            await CheckForUpdatesAsync(showMessage: false);
        }

        private void FrmUpdate_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (!_formCts.IsCancellationRequested)
                _formCts.Cancel();
        }

        private async void btnCheckUpdate_Click(object? sender, EventArgs e)
        {
            await CheckForUpdatesAsync(showMessage: true);
        }

        private async Task CheckForUpdatesAsync(bool showMessage)
        {
            if (btnCheckUpdate.Enabled == false)
                return;

            SetCheckingState(true);
            SetCurrentStatus("● Đang kiểm tra cập nhật...", Color.FromArgb(225, 239, 255), Color.FromArgb(0, 91, 190));
            txtReleaseNotes.Text = "Đang tải thông tin phiên bản từ GitHub Releases...";
            _latestUpdate = null;
            btnDownloadUpdate.Enabled = false;

            try
            {
                UpdateCheckResult result = await _updateService.CheckForUpdateAsync(
                    GitHubUpdateService.GetCurrentVersion(),
                    _formCts.Token);

                lblLastCheck.Text = "▦  Cập nhật lần cuối: " +
                                    DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                if (!result.Success)
                {
                    SetCurrentStatus("● Không thể kiểm tra cập nhật", Color.FromArgb(255, 235, 235), Color.Firebrick);
                    tabRelease.Text = "Phiên bản mới";
                    txtReleaseNotes.Text = result.ErrorMessage;

                    if (showMessage)
                        ShowUpdateMessage(LoginMessageType.Warning,
                            "Kiểm tra cập nhật", result.ErrorMessage);
                    return;
                }

                _latestUpdate = result;
                lblLatestVersion.Text = "v" + result.LatestVersion;
                tabRelease.Text = "Phiên bản mới (v" + result.LatestVersion + ")";
                lblReleaseType.Text = result.IsUpdateAvailable
                    ? "● Bản nâng cấp"
                    : "● Đang sử dụng bản mới nhất";
                lblReleaseType.ForeColor = Color.FromArgb(0, 125, 48);
                lblReleaseType.BackColor = Color.FromArgb(218, 246, 224);
                // API hiện tại chưa cung cấp thời điểm phát hành và kích thước asset,
                // vì vậy không hiển thị nhầm phiên bản/tên file như dữ liệu ngày hoặc dung lượng.
                lblReleaseDate.Text = "▦  Ngày phát hành: —";
                lblPackageSize.Text = "⇩  Dung lượng: —";
                txtReleaseNotes.Text = string.IsNullOrWhiteSpace(result.ReleaseNotes)
                    ? "Nhà phát hành chưa cung cấp nội dung thay đổi."
                    : result.ReleaseNotes;

                if (result.IsUpdateAvailable)
                {
                    SetCurrentStatus("● Có phiên bản mới: v" + result.LatestVersion, Color.FromArgb(226, 247, 232), Color.FromArgb(0, 115, 48));
                    btnDownloadUpdate.Enabled = true;
                    btnDownloadUpdate.Text = "⇩  Tải bản cập nhật v" + result.LatestVersion;
                    if (showMessage && chkNotify.Checked)
                        ShowUpdateMessage(LoginMessageType.Information,
                            "Có bản cập nhật mới",
                            "Đã tìm thấy phiên bản mới v" + result.LatestVersion + ".");
                }
                else
                {
                    SetCurrentStatus("✓ Đang là phiên bản mới nhất", Color.FromArgb(218, 246, 224), Color.FromArgb(0, 105, 42));
                    btnDownloadUpdate.Enabled = false;
                    btnDownloadUpdate.Text = "Đã là phiên bản mới nhất";
                    if (showMessage)
                        ShowUpdateMessage(LoginMessageType.Success,
                            "Kiểm tra cập nhật", "Phần mềm đang ở phiên bản mới nhất.");
                }

                lstUpdateHistory.Items.Insert(0,
                    DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") +
                    "  |  Kiểm tra  |  " +
                    (result.IsUpdateAvailable ? "Có phiên bản " + result.LatestVersion : "Đã mới nhất"));
            }
            catch (OperationCanceledException)
            {
                SetCurrentStatus("● Đã hủy kiểm tra cập nhật", Color.FromArgb(255, 244, 220), Color.DarkOrange);
            }
            catch (Exception ex)
            {
                SetCurrentStatus("● Có lỗi khi kiểm tra cập nhật", Color.FromArgb(255, 235, 235), Color.Firebrick);
                txtReleaseNotes.Text = ex.Message;
                if (showMessage)
                    ShowUpdateMessage(LoginMessageType.Warning, "Lỗi cập nhật", ex.Message);
            }
            finally
            {
                SetCheckingState(false);
            }
        }

        private async void btnDownloadUpdate_Click(object? sender, EventArgs e)
        {
            if (_latestUpdate == null || !_latestUpdate.Success ||
                !_latestUpdate.IsUpdateAvailable)
            {
                ShowUpdateMessage(LoginMessageType.Information,
                    "Cập nhật ứng dụng", "Vui lòng kiểm tra cập nhật trước khi tải.");
                return;
            }

            DialogResult confirm = FrmLoginMessage.ShowConfirmation(
                this,
                "Xác nhận tải cập nhật",
                "Tải gói cập nhật phiên bản v" + _latestUpdate.LatestVersion +
                " về máy?\r\n\r\nGói tải xuống sẽ được kiểm tra SHA-256 trước khi sử dụng.");

            if (confirm != DialogResult.Yes)
                return;

            await DownloadUpdateAsync();
        }

        private async Task DownloadUpdateAsync()
        {
            btnDownloadUpdate.Enabled = false;
            btnCheckUpdate.Enabled = false;
            pnlDownload.Visible = true;
            _downloadResult = null;
            _downloadSucceeded = false;
            btnOpenDownloadFolder.Visible = false;
            progressDownload.Value = 0;
            progressDownload.ForeColor = Color.FromArgb(0, 180, 55);
            lblDownloadPercent.Text = "0%";
            SetDownloadStatus("Đang chuẩn bị tải gói cập nhật...", false);
            lblDownloadStatus.ForeColor = Color.FromArgb(12, 43, 92);
            pnlDownload.BackColor = Color.FromArgb(239, 246, 255);
            pnlDownload.Invalidate();
            LayoutDownloadPanel();

            try
            {
                var progress = new Progress<UpdateDownloadProgress>(p =>
                {
                    // Progress<T> có thể đưa callback vào hàng đợi UI và callback
                    // cuối cùng vẫn chạy sau khi await tải xuống đã hoàn tất.
                    // Không cho callback đến muộn ghi đè trạng thái thành công.
                    if (_downloadSucceeded || _downloadResult?.Success == true)
                        return;

                    // Tính phần trăm trực tiếp từ số byte thực tế để tránh
                    // trường hợp Percentage từ service bị giữ ở giá trị cũ.
                    int actualPercentage = p.TotalBytes > 0
                        ? (int)Math.Round(
                            Math.Min(100d, p.BytesReceived * 100d / p.TotalBytes),
                            MidpointRounding.AwayFromZero)
                        : p.Percentage;

                    actualPercentage = Math.Max(0, Math.Min(100, actualPercentage));
                    progressDownload.Value = actualPercentage;
                    lblDownloadPercent.Text = actualPercentage.ToString() + "%";

                    string sizeText = p.TotalBytes > 0
                        ? " (" + FormatBytes(p.BytesReceived) + " / " + FormatBytes(p.TotalBytes) + ")"
                        : " (" + FormatBytes(p.BytesReceived) + ")";
                    SetDownloadStatus(p.StatusMessage + sizeText, false);
                    LayoutDownloadPanel();
                });

                _downloadResult = await _updateService.DownloadUpdatePackageAsync(
                    _latestUpdate!,
                    progress,
                    _formCts.Token);

                if (!_downloadResult.Success)
                {
                    SetDownloadStatus(_downloadResult.Cancelled
                        ? "Đã hủy tải gói cập nhật."
                        : "Tải cập nhật thất bại: " + _downloadResult.ErrorMessage, false);
                    lblDownloadStatus.ForeColor = Color.FromArgb(180, 45, 45);
                    pnlDownload.BackColor = Color.FromArgb(255, 242, 242);
                    pnlDownload.Invalidate();

                    if (!_downloadResult.Cancelled)
                    {
                        ShowUpdateMessage(LoginMessageType.Warning,
                            "Tải cập nhật thất bại", _downloadResult.ErrorMessage);
                    }
                    else
                    {
                        ShowUpdateMessage(LoginMessageType.Information,
                            "Đã hủy tải cập nhật", "Quá trình tải gói cập nhật đã được hủy.");
                    }
                    return;
                }

                progressDownload.Value = 100;
                lblDownloadPercent.Text = "100%";
                SetDownloadStatus("Tải và xác minh thành công. (" +
                                  FormatBytes(_downloadResult.FileSize) + " / " +
                                  FormatBytes(_downloadResult.FileSize) + ")", true);
                lblDownloadStatus.ForeColor = Color.FromArgb(0, 91, 48);
                pnlDownload.BackColor = Color.FromArgb(239, 250, 241);
                btnOpenDownloadFolder.Visible = true;
                LayoutDownloadPanel();

                lstUpdateHistory.Items.Insert(0,
                    DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") +
                    "  |  Tải gói  |  v" + _latestUpdate!.LatestVersion + " (SHA-256 OK)");

                // Không bật lại nút tải sau khi đã tải thành công để tránh tải trùng.
                btnDownloadUpdate.Text = "Đã tải gói cập nhật v" + _latestUpdate.LatestVersion;
                ShowUpdateMessage(LoginMessageType.Success,
                    "Tải cập nhật hoàn tất",
                    "Gói cập nhật v" + _latestUpdate.LatestVersion +
                    " đã được tải xuống và xác minh SHA-256 thành công.\r\n\r\n" +
                    "Bạn có thể mở thư mục tải bằng nút ở thanh trạng thái bên dưới.");
            }
            catch (OperationCanceledException)
            {
                SetDownloadStatus("Đã hủy tải gói cập nhật.", false);
                lblDownloadStatus.ForeColor = Color.FromArgb(180, 105, 0);
                pnlDownload.BackColor = Color.FromArgb(255, 248, 232);
                pnlDownload.Invalidate();
            }
            catch (Exception ex)
            {
                SetDownloadStatus("Lỗi tải cập nhật: " + ex.Message, false);
                lblDownloadStatus.ForeColor = Color.FromArgb(180, 45, 45);
                pnlDownload.BackColor = Color.FromArgb(255, 242, 242);
                pnlDownload.Invalidate();
                ShowUpdateMessage(LoginMessageType.Warning, "Lỗi tải cập nhật", ex.Message);
            }
            finally
            {
                btnCheckUpdate.Enabled = true;
                btnDownloadUpdate.Enabled =
                    _latestUpdate?.IsUpdateAvailable == true &&
                    _downloadResult?.Success != true;
                LayoutDownloadPanel();
            }
        }

        private void ConfigureDownloadPanel()
        {
            // Tạo hai trạng thái trực quan: đang tải và tải/xác minh thành công.
            pnlDownload.Height = 86;
            pnlDownload.Padding = new Padding(14, 7, 14, 5);
            pnlDownload.BackColor = Color.FromArgb(239, 246, 255);
            pnlDownload.Resize += (s, e) =>
            {
                ApplyRoundedRegion(pnlDownload, 15);
                LayoutDownloadPanel();
            };
            pnlDownload.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                Color border = btnOpenDownloadFolder.Visible
                    ? Color.FromArgb(194, 230, 201)
                    : Color.FromArgb(202, 220, 248);
                using (GraphicsPath path = CreateRoundedRectangle(
                    new Rectangle(0, 0, Math.Max(1, pnlDownload.Width - 1),
                        Math.Max(1, pnlDownload.Height - 1)), 15))
                using (Pen pen = new Pen(border, 1F))
                    e.Graphics.DrawPath(pen, path);

                if (btnOpenDownloadFolder.Visible)
                {
                    int dividerX = pnlDownload.Width - 18 - 90 - 12;
                    using (Pen divider = new Pen(Color.FromArgb(175, 190, 190), 1F))
                        e.Graphics.DrawLine(divider, dividerX, 12, dividerX, 39);
                }
            };

            lblDownloadStatus.Dock = DockStyle.None;
            lblDownloadPercent.Dock = DockStyle.None;
            progressDownload.Dock = DockStyle.None;
            btnOpenDownloadFolder.Dock = DockStyle.None;

            lblDownloadStatus.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblDownloadStatus.TextAlign = ContentAlignment.MiddleLeft;
            lblDownloadStatus.Image = null;
            lblDownloadStatus.Padding = Padding.Empty;
            lblDownloadStatus.AutoEllipsis = true;

            lblDownloadPercent.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblDownloadPercent.ForeColor = Color.FromArgb(8, 36, 125);
            lblDownloadPercent.TextAlign = ContentAlignment.MiddleRight;
            lblDownloadPercent.AutoSize = false;

            progressDownload.Style = ProgressBarStyle.Continuous;
            progressDownload.ForeColor = Color.FromArgb(0, 180, 55);
            progressDownload.Minimum = 0;
            progressDownload.Maximum = 100;
            progressDownload.Value = 0;

            _downloadStateIcon = CreateDownloadStateIcon(false);
            _successStateIcon = CreateDownloadStateIcon(true);
            _folderButtonIcon = CreateFolderIcon();

            // Dùng PictureBox riêng thay vì vẽ trong Panel.Paint:
            // bảo đảm icon không bị nền các control con che mất.
            _downloadStatePicture = new PictureBox
            {
                Name = "picDownloadState",
                Size = new Size(30, 30),
                Location = new Point(15, 7),
                SizeMode = PictureBoxSizeMode.CenterImage,
                BackColor = Color.Transparent,
                Image = _downloadStateIcon,
                TabStop = false
            };
            pnlDownload.Controls.Add(_downloadStatePicture);
            _downloadStatePicture.BringToFront();

            btnOpenDownloadFolder.Text = "Mở thư mục tải";
            btnOpenDownloadFolder.Image = _folderButtonIcon;
            btnOpenDownloadFolder.ImageAlign = ContentAlignment.MiddleLeft;
            btnOpenDownloadFolder.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnOpenDownloadFolder.TextAlign = ContentAlignment.MiddleCenter;
            btnOpenDownloadFolder.Padding = new Padding(12, 0, 12, 0);
            btnOpenDownloadFolder.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnOpenDownloadFolder.FlatStyle = FlatStyle.Flat;
            btnOpenDownloadFolder.FlatAppearance.BorderSize = 0;
            btnOpenDownloadFolder.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath borderPath = CreateRoundedRectangle(
                    new Rectangle(1, 1, Math.Max(1, btnOpenDownloadFolder.Width - 3),
                        Math.Max(1, btnOpenDownloadFolder.Height - 3)), 8))
                using (Pen borderPen = new Pen(Color.FromArgb(0, 91, 220), 1.2F))
                    e.Graphics.DrawPath(borderPen, borderPath);
            };
            btnOpenDownloadFolder.BackColor = Color.White;
            btnOpenDownloadFolder.ForeColor = Color.FromArgb(8, 36, 125);
            btnOpenDownloadFolder.Cursor = Cursors.Hand;
            btnOpenDownloadFolder.Visible = false;

            ApplyRoundedRegion(pnlDownload, 15);
            SetDownloadStatus("Đang chuẩn bị tải gói cập nhật...", false);
            LayoutDownloadPanel();
        }

        private void LayoutDownloadPanel()
        {
            if (pnlDownload == null || lblDownloadStatus == null ||
                lblDownloadPercent == null || progressDownload == null ||
                btnOpenDownloadFolder == null)
                return;

            int width = pnlDownload.ClientSize.Width;
            if (width <= 0) return;

            const int left = 48; // Chừa vùng riêng cho biểu tượng trạng thái.
            const int right = 20;
            const int percentWidth = 90; // Đủ chỗ hiển thị cả "100%".
            const int buttonWidth = 190;
            const int buttonGap = 24;

            if (_downloadStatePicture != null)
            {
                _downloadStatePicture.Location = new Point(15, 7);
                _downloadStatePicture.Size = new Size(30, 30);
                _downloadStatePicture.BringToFront();
            }

            lblDownloadStatus.Location = new Point(left, 7);
            lblDownloadStatus.Size = new Size(
                Math.Max(120, width - left - right - percentWidth -
                    (btnOpenDownloadFolder.Visible ? buttonWidth + buttonGap : 0) - 12), 30);

            lblDownloadPercent.Location = new Point(width - right - percentWidth, 6);
            lblDownloadPercent.Size = new Size(percentWidth, 32);

            if (btnOpenDownloadFolder.Visible)
            {
                btnOpenDownloadFolder.Location = new Point(
                    width - right - percentWidth - buttonGap - buttonWidth, 5);
                btnOpenDownloadFolder.Size = new Size(buttonWidth, 34);
            }

            progressDownload.Location = new Point(left, 48);
            progressDownload.Size = new Size(Math.Max(100, width - left - right), 20);
            progressDownload.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            ApplyRoundedRegion(progressDownload, 10);
            pnlDownload.Invalidate();
        }

        private void SetDownloadStatus(string message, bool success)
        {
            lblDownloadStatus.Text = message;

            // Chỉ chuyển sang trạng thái thành công tại đây; không tự động
            // chuyển ngược về đang tải bởi callback tiến trình đến muộn.
            // Khi bắt đầu một lượt tải mới, DownloadUpdateAsync đặt lại cờ
            // _downloadSucceeded = false một cách tường minh.
            if (success)
                _downloadSucceeded = true;

            if (_downloadStatePicture != null)
            {
                _downloadStatePicture.Image = _downloadSucceeded
                    ? _successStateIcon
                    : _downloadStateIcon;
                _downloadStatePicture.Invalidate();
                _downloadStatePicture.BringToFront();
            }

            pnlDownload?.Invalidate();
        }

        private static Bitmap CreateDownloadStateIcon(bool success)
        {
            var bitmap = new Bitmap(30, 30);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                if (success)
                {
                    using (var brush = new SolidBrush(Color.FromArgb(20, 145, 70)))
                        g.FillEllipse(brush, 2, 2, 26, 26);
                    using (var pen = new Pen(Color.White, 2.8F))
                    {
                        pen.StartCap = LineCap.Round;
                        pen.EndCap = LineCap.Round;
                        g.DrawLines(pen, new[]
                        {
                            new Point(8, 15), new Point(13, 20), new Point(22, 10)
                        });
                    }
                }
                else
                {
                    using (var pen = new Pen(Color.FromArgb(42, 105, 190), 3.2F))
                    {
                        pen.StartCap = LineCap.Round;
                        pen.EndCap = LineCap.Round;
                        g.DrawLine(pen, 15, 3, 15, 19);
                        g.DrawLines(pen, new[]
                        {
                            new Point(8, 13), new Point(15, 20), new Point(22, 13)
                        });
                        g.DrawLine(pen, 6, 24, 24, 24);
                    }
                }
            }
            return bitmap;
        }

        private static Bitmap CreateFolderIcon()
        {
            var bitmap = new Bitmap(22, 18);
            using (Graphics g = Graphics.FromImage(bitmap))
            using (var pen = new Pen(Color.FromArgb(35, 105, 190), 1.8F))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawLines(pen, new[]
                {
                    new Point(2, 5), new Point(2, 3), new Point(9, 3),
                    new Point(12, 6), new Point(20, 6), new Point(20, 15),
                    new Point(2, 15), new Point(2, 5)
                });
                g.DrawLine(pen, 2, 7, 20, 7);
            }
            return bitmap;
        }

        private void DisposeDownloadIcons()
        {
            if (_downloadStatePicture != null)
            {
                _downloadStatePicture.Image = null;
                _downloadStatePicture.Dispose();
                _downloadStatePicture = null;
            }

            _downloadStateIcon?.Dispose();
            _successStateIcon?.Dispose();
            _folderButtonIcon?.Dispose();
            _downloadStateIcon = null;
            _successStateIcon = null;
            _folderButtonIcon = null;
        }

        private static void ApplyRoundedRegion(Control control, int radius)
        {
            if (control == null || control.Width <= 0 || control.Height <= 0)
                return;

            using (GraphicsPath path = CreateRoundedRectangle(
                new Rectangle(0, 0, control.Width, control.Height), radius))
            {
                Region? oldRegion = control.Region;
                control.Region = new Region(path);
                oldRegion?.Dispose();
            }
        }

        private static GraphicsPath CreateRoundedRectangle(Rectangle bounds, int radius)
        {
            int diameter = Math.Max(1, radius * 2);
            diameter = Math.Min(diameter, Math.Min(bounds.Width, bounds.Height));
            var path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter,
                diameter, diameter, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - diameter,
                diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void ShowUpdateMessage(
            LoginMessageType type, string title, string description)
        {
            FrmLoginMessage.ShowMessage(this, type, title, description);
        }

        private void btnUpdateOptions_Click(object? sender, EventArgs e)
        {
            pnlOptions.Focus();
            pnlOptions.BackColor = Color.FromArgb(242, 248, 255);
            pnlOptions.BorderStyle = BorderStyle.Fixed3D;
            var timer = new System.Windows.Forms.Timer { Interval = 700 };
            timer.Tick += (s, args) =>
            {
                timer.Stop();
                timer.Dispose();
                pnlOptions.BackColor = Color.White;
                pnlOptions.BorderStyle = BorderStyle.FixedSingle;
            };
            timer.Start();
        }

        private void btnUpdateHistory_Click(object? sender, EventArgs e)
        {
            tabVersion.SelectedTab = tabHistory;
            lstUpdateHistory.Focus();
        }

        private void btnViewDetails_Click(object? sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/VanTrung0307/quan-ly-cbcs-desktop-app/releases",
                UseShellExecute = true
            });
        }

        private void btnOpenDownloadFolder_Click(object? sender, EventArgs e)
        {
            if (_downloadResult == null || string.IsNullOrWhiteSpace(_downloadResult.FilePath))
                return;

            string? directory = Path.GetDirectoryName(_downloadResult.FilePath);
            if (!string.IsNullOrWhiteSpace(directory) && Directory.Exists(directory))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = directory,
                    UseShellExecute = true
                });
            }
            else
            {
                ShowUpdateMessage(LoginMessageType.Warning,
                    "Không tìm thấy thư mục tải", "Thư mục chứa gói cập nhật không còn tồn tại.");
            }
        }


        private void SetCurrentStatus(string text, Color background, Color foreground)
        {
            lblCurrentStatus.Text = text;
            lblCurrentStatus.BackColor = background;
            lblCurrentStatus.ForeColor = foreground;
            lblCurrentStatus.Region?.Dispose();
            lblCurrentStatus.Region = new Region(RoundedRect(
                new Rectangle(0, 0, lblCurrentStatus.Width, lblCurrentStatus.Height), 8));
            lblCurrentStatus.Invalidate();
        }

        private void SetCheckingState(bool isChecking)
        {
            btnCheckUpdate.Enabled = !isChecking;
            btnCheckUpdate.Text = isChecking ? "Đang kiểm tra..." : "⟳  Kiểm tra cập nhật";
        }

        private static string GetApplicationSizeText()
        {
            try
            {
                string? directory = AppContext.BaseDirectory;
                if (string.IsNullOrWhiteSpace(directory) || !Directory.Exists(directory))
                    return "Không xác định";

                long total = 0;
                foreach (string file in Directory.EnumerateFiles(
                    directory, "*", SearchOption.AllDirectories))
                {
                    try { total += new FileInfo(file).Length; }
                    catch { /* Bỏ qua file đang được hệ thống sử dụng. */ }
                }
                return FormatBytes(total);
            }
            catch
            {
                return "Không xác định";
            }
        }

        private static string FormatBytes(long bytes)
        {
            string[] units = { "B", "KB", "MB", "GB" };
            double size = bytes;
            int unit = 0;
            while (size >= 1024 && unit < units.Length - 1)
            {
                size /= 1024;
                unit++;
            }
            return size.ToString("0.##") + " " + units[unit];
        }
    }
}