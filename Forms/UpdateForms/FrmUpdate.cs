using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyCBCS.Services.Update;

namespace QuanLyCBCS.Forms.UpdateForms
{
    public partial class FrmUpdate : Form
    {
        private readonly GitHubUpdateService _updateService = new GitHubUpdateService();
        private readonly CancellationTokenSource _formCts = new CancellationTokenSource();
        private UpdateCheckResult? _latestUpdate;
        private UpdateDownloadResult? _downloadResult;
        private bool _hasAutoChecked;

        public FrmUpdate()
        {
            InitializeComponent();

            lblCurrentVersion.Text = "v" + GitHubUpdateService.GetCurrentVersion();
            SetCurrentStatus("● Chưa kiểm tra cập nhật", Color.FromArgb(238, 243, 249), Color.FromArgb(90, 105, 125));
            lblSystemVersionValue.Text = "v" + GitHubUpdateService.GetCurrentVersion();
            lblAppSizeValue.Text = GetApplicationSizeText();
            lblOsValue.Text = Environment.OSVersion.Platform + " " +
                              Environment.OSVersion.Version;

            // FrmUpdate được nhúng trong pnlContent của FrmHome nên phải
            // tự co giãn theo kích thước thực tế của vùng nội dung.
            Load += FrmUpdate_LayoutChanged;
            Resize += FrmUpdate_LayoutChanged;
            flowActions.Resize += FrmUpdate_LayoutChanged;
            Shown += FrmUpdate_Shown;
            FormClosing += FrmUpdate_FormClosing;
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
                        MessageBox.Show(this, result.ErrorMessage, "Kiểm tra cập nhật",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        MessageBox.Show(this,
                            "Đã tìm thấy phiên bản mới v" + result.LatestVersion + ".",
                            "Có bản cập nhật mới", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    SetCurrentStatus("✓ Đang là phiên bản mới nhất", Color.FromArgb(218, 246, 224), Color.FromArgb(0, 105, 42));
                    btnDownloadUpdate.Enabled = false;
                    btnDownloadUpdate.Text = "Đã là phiên bản mới nhất";
                    if (showMessage)
                        MessageBox.Show(this, "Phần mềm đang ở phiên bản mới nhất.",
                            "Kiểm tra cập nhật", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    MessageBox.Show(this, ex.Message, "Lỗi cập nhật",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show(this, "Vui lòng kiểm tra cập nhật trước khi tải.",
                    "Cập nhật ứng dụng", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult confirm = MessageBox.Show(this,
                "Tải gói cập nhật phiên bản v" + _latestUpdate.LatestVersion +
                " về máy?\r\n\r\nGói tải xuống sẽ được kiểm tra SHA-256 trước khi sử dụng.",
                "Xác nhận tải cập nhật",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            await DownloadUpdateAsync();
        }

        private async Task DownloadUpdateAsync()
        {
            btnDownloadUpdate.Enabled = false;
            btnCheckUpdate.Enabled = false;
            pnlDownload.Visible = true;
            btnOpenDownloadFolder.Visible = false;
            progressDownload.Value = 0;
            lblDownloadPercent.Text = "0%";
            lblDownloadStatus.Text = "Đang chuẩn bị tải gói cập nhật...";

            try
            {
                var progress = new Progress<UpdateDownloadProgress>(p =>
                {
                    progressDownload.Value = Math.Max(0, Math.Min(100, p.Percentage));
                    lblDownloadPercent.Text = p.Percentage + "%";
                    lblDownloadStatus.Text = p.StatusMessage +
                        (p.TotalBytes > 0
                            ? "  (" + FormatBytes(p.BytesReceived) + " / " +
                              FormatBytes(p.TotalBytes) + ")"
                            : "  (" + FormatBytes(p.BytesReceived) + ")");
                });

                _downloadResult = await _updateService.DownloadUpdatePackageAsync(
                    _latestUpdate!,
                    progress,
                    _formCts.Token);

                if (!_downloadResult.Success)
                {
                    lblDownloadStatus.Text = _downloadResult.ErrorMessage;
                    MessageBox.Show(this, _downloadResult.ErrorMessage,
                        "Tải cập nhật thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                progressDownload.Value = 100;
                lblDownloadPercent.Text = "100%";
                lblDownloadStatus.Text = "Đã tải và xác minh SHA-256 thành công. " +
                                         FormatBytes(_downloadResult.FileSize);
                btnOpenDownloadFolder.Visible = true;

                lstUpdateHistory.Items.Insert(0,
                    DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") +
                    "  |  Tải gói  |  v" + _latestUpdate!.LatestVersion + " (SHA-256 OK)");

                MessageBox.Show(this,
                    "Gói cập nhật đã được tải và xác minh thành công.\r\n\r\n" +
                    "Đường dẫn:\r\n" + _downloadResult.FilePath +
                    "\r\n\r\nBước cài đặt tự động sẽ được triển khai ở phần tiếp theo. Hiện tại chưa thay thế file ứng dụng.",
                    "Tải cập nhật hoàn tất",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (OperationCanceledException)
            {
                lblDownloadStatus.Text = "Đã hủy tải gói cập nhật.";
            }
            catch (Exception ex)
            {
                lblDownloadStatus.Text = "Lỗi tải cập nhật: " + ex.Message;
                MessageBox.Show(this, ex.Message, "Lỗi tải cập nhật",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnCheckUpdate.Enabled = true;
                btnDownloadUpdate.Enabled = _latestUpdate?.IsUpdateAvailable == true;
            }
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