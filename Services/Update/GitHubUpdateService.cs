
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using QuanLyCBCS.DTOs.Update;

namespace QuanLyCBCS.Services.Update
{
    public sealed class GitHubUpdateService
    {
        private const string Owner = "VanTrung0307";
        private const string Repository = "quan-ly-cbcs-desktop-app";

        private const string ApiUrl =
            "https://api.github.com/repos/" +
            Owner + "/" + Repository + "/releases/latest";

        private const long MaxPackageSize = 500L * 1024 * 1024;
        private const long MaxManifestSize = 1024L * 1024;

        private static readonly HttpClient HttpClient =
            CreateHttpClient();

        private static HttpClient CreateHttpClient()
        {
            var client = new HttpClient
            {
                Timeout = Timeout.InfiniteTimeSpan
            };

            client.DefaultRequestHeaders.UserAgent.ParseAdd(
                "QuanLyCBCS-Desktop-Updater/1.0");

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(
                    "application/vnd.github+json"));

            client.DefaultRequestHeaders.Add(
                "X-GitHub-Api-Version",
                "2022-11-28");

            return client;
        }

        // =====================================================
        // 1. LẤY PHIÊN BẢN HIỆN TẠI
        // =====================================================

        public static string GetCurrentVersion()
        {
            string? version = Assembly.GetEntryAssembly()?
                .GetCustomAttribute<
                    AssemblyInformationalVersionAttribute>()?
                .InformationalVersion;

            if (string.IsNullOrWhiteSpace(version))
                version = ApplicationVersionFallback();

            return version.Split('+')[0];
        }

        private static string ApplicationVersionFallback()
        {
            return System.Windows.Forms.Application.ProductVersion
                   ?? "0.0.0";
        }

        private static bool TryParseVersion(
            string value,
            out Version version)
        {
            value = value.Trim();

            if (value.StartsWith(
                "v",
                StringComparison.OrdinalIgnoreCase))
            {
                value = value.Substring(1);
            }

            return Version.TryParse(value, out version!);
        }

        // =====================================================
        // 2. KIỂM TRA CẬP NHẬT TỪ GITHUB RELEASES
        // =====================================================

        public async Task<UpdateCheckResult> CheckForUpdateAsync(
            string? currentVersion = null,
            CancellationToken cancellationToken = default)
        {
            currentVersion ??= GetCurrentVersion();

            var result = new UpdateCheckResult
            {
                CurrentVersion = currentVersion
            };

            try
            {
                using var timeoutCts =
                    CancellationTokenSource.CreateLinkedTokenSource(
                        cancellationToken);

                timeoutCts.CancelAfter(TimeSpan.FromSeconds(30));

                CancellationToken token = timeoutCts.Token;

                // 1. Lấy GitHub Release mới nhất và ánh xạ sang DTO.
                using var response = await HttpClient.GetAsync(
                    ApiUrl,
                    HttpCompletionOption.ResponseHeadersRead,
                    token);

                response.EnsureSuccessStatusCode();

                if (response.Content.Headers.ContentLength is long releaseLength &&
                    releaseLength > MaxManifestSize * 5)
                {
                    throw new InvalidDataException(
                        "Dữ liệu GitHub Release vượt quá giới hạn cho phép.");
                }

                string releaseJson =
                    await response.Content.ReadAsStringAsync(token);

                GitHubReleaseDto? release =
                    JsonSerializer.Deserialize<GitHubReleaseDto>(
                        releaseJson);

                if (release == null)
                {
                    throw new InvalidDataException(
                        "Không đọc được thông tin GitHub Release.");
                }

                if (release.Draft || release.Prerelease)
                {
                    throw new InvalidDataException(
                        "GitHub Release không hợp lệ.");
                }

                if (string.IsNullOrWhiteSpace(release.TagName))
                {
                    throw new InvalidDataException(
                        "GitHub Release không có tag phiên bản.");
                }

                if (release.Assets == null || release.Assets.Count == 0)
                {
                    throw new InvalidDataException(
                        "GitHub Release không có file đính kèm.");
                }

                // 2. Tìm duy nhất latest.json trong danh sách Assets.
                GitHubAssetDto manifestAsset =
                    FindUniqueReleaseAsset(
                        release,
                        "latest.json");

                ValidateReleaseAsset(manifestAsset);

                if (manifestAsset.Size > MaxManifestSize)
                {
                    throw new InvalidDataException(
                        "File latest.json vượt quá giới hạn 1 MB.");
                }

                // 3. Tải và deserialize Manifest.
                using var manifestResponse = await HttpClient.GetAsync(
                    manifestAsset.BrowserDownloadUrl,
                    HttpCompletionOption.ResponseHeadersRead,
                    token);

                manifestResponse.EnsureSuccessStatusCode();

                if (manifestResponse.Content.Headers.ContentLength is long manifestLength &&
                    manifestLength > MaxManifestSize)
                {
                    throw new InvalidDataException(
                        "File latest.json vượt quá giới hạn 1 MB.");
                }

                string manifestJson =
                    await manifestResponse.Content.ReadAsStringAsync(token);

                if (System.Text.Encoding.UTF8.GetByteCount(manifestJson) >
                    MaxManifestSize)
                {
                    throw new InvalidDataException(
                        "File latest.json vượt quá giới hạn 1 MB.");
                }

                UpdateManifest? manifest =
                    JsonSerializer.Deserialize<UpdateManifest>(
                        manifestJson,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                if (manifest == null)
                {
                    throw new InvalidDataException(
                        "Không đọc được thông tin cập nhật.");
                }

                // 4. Kiểm tra tính hợp lệ và đồng nhất phiên bản.
                if (!TryParseVersion(
                    manifest.Version,
                    out Version latestVersion))
                {
                    throw new InvalidDataException(
                        "Phiên bản trong manifest không hợp lệ.");
                }

                if (!TryParseVersion(
                    currentVersion,
                    out Version installedVersion))
                {
                    throw new InvalidDataException(
                        "Phiên bản hiện tại không hợp lệ.");
                }

                if (!TryParseVersion(
                    release.TagName,
                    out Version releaseVersion) ||
                    latestVersion != releaseVersion)
                {
                    throw new InvalidDataException(
                        "Phiên bản manifest không khớp Release.");
                }

                // 5. Kiểm tra tên file ZIP trong Manifest.
                if (string.IsNullOrWhiteSpace(manifest.PackageFileName) ||
                    Path.GetFileName(manifest.PackageFileName) !=
                        manifest.PackageFileName ||
                    !manifest.PackageFileName.EndsWith(
                        ".zip",
                        StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidDataException(
                        "Tên file cập nhật không hợp lệ.");
                }

                // 6. Kiểm tra mã SHA-256 trong Manifest.
                if (!Regex.IsMatch(
                    manifest.Sha256 ?? string.Empty,
                    @"\A[0-9a-fA-F]{64}\z"))
                {
                    throw new InvalidDataException(
                        "Mã SHA-256 không hợp lệ.");
                }

                // 7. Tìm duy nhất gói ZIP có tên khớp Manifest.
                GitHubAssetDto packageAsset =
                    FindUniqueReleaseAsset(
                        release,
                        manifest.PackageFileName);

                ValidateReleaseAsset(packageAsset);

                if (packageAsset.Size <= 0 ||
                    packageAsset.Size > MaxPackageSize)
                {
                    throw new InvalidDataException(
                        "Dung lượng gói cập nhật trên GitHub không hợp lệ.");
                }

                // 8. Trả kết quả.
                result.Success = true;
                result.LatestVersion = manifest.Version;

                result.IsUpdateAvailable =
                    latestVersion > installedVersion;

                result.ReleaseNotes = manifest.ReleaseNotes;
                result.PackageFileName = manifest.PackageFileName;
                result.DownloadUrl = packageAsset.BrowserDownloadUrl;
                result.Sha256 = manifest.Sha256 ?? string.Empty;
                result.IsMandatory = manifest.Mandatory;

                return result;
            }
            catch (OperationCanceledException)
            {
                result.Success = false;
                result.ErrorMessage =
                    cancellationToken.IsCancellationRequested
                    ? "Đã hủy quá trình kiểm tra cập nhật."
                    : "Quá thời gian kiểm tra kết nối GitHub.";

                return result;
            }
            catch (HttpRequestException ex)
            {
                result.Success = false;
                result.ErrorMessage =
                    "Không thể kết nối GitHub: " + ex.Message;

                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage =
                    "Lỗi kiểm tra cập nhật: " + ex.Message;

                return result;
            }
        }

        // =====================================================
        // 3. TẢI GÓI CẬP NHẬT
        // =====================================================

        public async Task<UpdateDownloadResult>
            DownloadUpdatePackageAsync(
                UpdateCheckResult update,
                IProgress<UpdateDownloadProgress>? progress = null,
                CancellationToken cancellationToken = default)
        {
            var result = new UpdateDownloadResult();

            string? temporaryPath = null;

            try
            {
                // 1. Kiểm tra kết quả cập nhật.
                if (update == null ||
                    !update.Success ||
                    !update.IsUpdateAvailable)
                {
                    throw new InvalidOperationException(
                        "Không có gói cập nhật hợp lệ để tải.");
                }

                if (string.IsNullOrWhiteSpace(update.DownloadUrl))
                {
                    throw new InvalidDataException(
                        "Không có đường dẫn tải xuống.");
                }

                ValidateReleaseAssetUrl(update.DownloadUrl);

                // 2. Kiểm tra tên file.
                if (string.IsNullOrWhiteSpace(
                    update.PackageFileName) ||
                    Path.GetFileName(update.PackageFileName)
                        != update.PackageFileName ||
                    !update.PackageFileName.EndsWith(
                        ".zip",
                        StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidDataException(
                        "Tên gói cập nhật không hợp lệ.");
                }

                // 3. Kiểm tra SHA-256.
                if (!Regex.IsMatch(
                    update.Sha256 ?? string.Empty,
                    @"\A[0-9a-fA-F]{64}\z"))
                {
                    throw new InvalidDataException(
                        "SHA-256 trong manifest không hợp lệ.");
                }

                if (!TryParseVersion(
                    update.LatestVersion,
                    out Version packageVersion))
                {
                    throw new InvalidDataException(
                        "Phiên bản gói cập nhật không hợp lệ.");
                }

                // 4. Xác định thư mục lưu trữ.
                string updateDirectory = Path.Combine(
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.LocalApplicationData),
                    "QuanLyCBCS",
                    "Updates",
                    packageVersion.ToString());

                Directory.CreateDirectory(updateDirectory);

                string finalPath = Path.Combine(
                    updateDirectory,
                    update.PackageFileName);

                temporaryPath = Path.Combine(
                    updateDirectory,
                    Guid.NewGuid().ToString("N") + ".download");

                // 5. Gửi yêu cầu tải file.
                using var request = new HttpRequestMessage(
                    HttpMethod.Get,
                    update.DownloadUrl);

                using var response = await HttpClient.SendAsync(
                    request,
                    HttpCompletionOption.ResponseHeadersRead,
                    cancellationToken);

                response.EnsureSuccessStatusCode();

                long totalBytes =
                    response.Content.Headers.ContentLength ?? -1;

                if (totalBytes > MaxPackageSize)
                {
                    throw new InvalidDataException(
                        "Gói cập nhật vượt quá giới hạn 500 MB.");
                }

                // 6. Tải và tính SHA-256 đồng thời.
                using Stream inputStream =
                    await response.Content.ReadAsStreamAsync(
                        cancellationToken);

                using var outputStream = new FileStream(
                    temporaryPath,
                    FileMode.CreateNew,
                    FileAccess.Write,
                    FileShare.None,
                    81920,
                    FileOptions.Asynchronous |
                    FileOptions.SequentialScan);

                using var hashAlgorithm =
                    IncrementalHash.CreateHash(
                        HashAlgorithmName.SHA256);

                byte[] buffer = new byte[81920];

                long downloadedBytes = 0;

                int bytesRead;

                while ((bytesRead = await inputStream.ReadAsync(
                    buffer.AsMemory(0, buffer.Length),
                    cancellationToken)) > 0)
                {
                    downloadedBytes += bytesRead;

                    // Kiểm tra giới hạn dung lượng thực tế.
                    if (downloadedBytes > MaxPackageSize)
                    {
                        throw new InvalidDataException(
                            "Gói cập nhật vượt quá giới hạn 500 MB.");
                    }

                    hashAlgorithm.AppendData(
                        buffer,
                        0,
                        bytesRead);

                    await outputStream.WriteAsync(
                        buffer.AsMemory(0, bytesRead),
                        cancellationToken);

                    int percentage = totalBytes > 0
                        ? (int)Math.Min(
                            100,
                            downloadedBytes * 100.0 / totalBytes)
                        : 0;

                    progress?.Report(
                        new UpdateDownloadProgress
                        {
                            BytesReceived = downloadedBytes,
                            TotalBytes = Math.Max(0, totalBytes),
                            Percentage = percentage,
                            StatusMessage = "Đang tải gói cập nhật..."
                        });
                }

                await outputStream.FlushAsync(cancellationToken);

                // 7. Kiểm tra kích thước.
                if (downloadedBytes <= 0)
                {
                    throw new InvalidDataException(
                        "Gói cập nhật tải xuống bị rỗng.");
                }

                if (totalBytes >= 0 &&
                    downloadedBytes != totalBytes)
                {
                    throw new InvalidDataException(
                        "Dung lượng file tải xuống không khớp.");
                }

                // 8. Tính SHA-256 thực tế.
                string actualSha256 = Convert.ToHexString(
                    hashAlgorithm.GetHashAndReset());

                string expectedSha256 = (update.Sha256 ?? string.Empty)
                    .Trim()
                    .ToUpperInvariant();

                if (!CryptographicOperations.FixedTimeEquals(
                    Convert.FromHexString(actualSha256),
                    Convert.FromHexString(expectedSha256)))
                {
                    throw new InvalidDataException(
                        "SHA-256 không khớp. Gói cập nhật có thể bị lỗi hoặc đã bị thay đổi.");
                }

                // 9. Chỉ chuyển file sau khi xác minh thành công.
                outputStream.Close();

                File.Move(
                    temporaryPath,
                    finalPath,
                    true);

                temporaryPath = null;

                progress?.Report(
                    new UpdateDownloadProgress
                    {
                        BytesReceived = downloadedBytes,
                        TotalBytes = downloadedBytes,
                        Percentage = 100,
                        StatusMessage = "Tải và xác minh thành công."
                    });

                // 10. Trả kết quả.
                result.Success = true;
                result.FilePath = finalPath;
                result.FileSize = downloadedBytes;
                result.Sha256 = actualSha256;

                return result;
            }
            catch (OperationCanceledException)
            {
                result.Success = false;
                result.Cancelled = true;
                result.ErrorMessage =
                    "Đã hủy quá trình tải cập nhật.";

                return result;
            }
            catch (HttpRequestException ex)
            {
                result.Success = false;
                result.ErrorMessage =
                    "Lỗi kết nối khi tải cập nhật: " + ex.Message;

                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage =
                    "Lỗi tải gói cập nhật: " + ex.Message;

                return result;
            }
            finally
            {
                // Xóa file tải dở hoặc file không hợp lệ.
                if (!string.IsNullOrWhiteSpace(temporaryPath) &&
                    File.Exists(temporaryPath))
                {
                    try
                    {
                        File.Delete(temporaryPath);
                    }
                    catch
                    {
                        // Không làm mất kết quả lỗi ban đầu.
                    }
                }
            }
        }

        // =====================================================
        // 4. KIỂM TRA ĐƯỜNG DẪN GITHUB
        // =====================================================

        /// <summary>
        /// Tìm đúng một asset theo tên trong GitHub Release.
        /// Không chấp nhận trường hợp thiếu hoặc trùng tên asset.
        /// </summary>
        private static GitHubAssetDto FindUniqueReleaseAsset(
            GitHubReleaseDto release,
            string assetName)
        {
            var matches = release.Assets.FindAll(asset =>
                asset != null &&
                string.Equals(
                    asset.Name,
                    assetName,
                    StringComparison.Ordinal));

            if (matches.Count == 0)
            {
                throw new InvalidDataException(
                    $"GitHub Release không có file {assetName}.");
            }

            if (matches.Count > 1)
            {
                throw new InvalidDataException(
                    $"GitHub Release có nhiều file trùng tên {assetName}.");
            }

            return matches[0];
        }

        /// <summary>
        /// Kiểm tra trạng thái và URL của asset trước khi sử dụng.
        /// </summary>
        private static void ValidateReleaseAsset(GitHubAssetDto asset)
        {
            if (asset == null)
            {
                throw new InvalidDataException(
                    "Thông tin file đính kèm GitHub không hợp lệ.");
            }

            if (!string.IsNullOrWhiteSpace(asset.State) &&
                !string.Equals(
                    asset.State,
                    "uploaded",
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidDataException(
                    $"File {asset.Name} chưa được tải lên hoàn chỉnh.");
            }

            if (string.IsNullOrWhiteSpace(asset.BrowserDownloadUrl))
            {
                throw new InvalidDataException(
                    $"File {asset.Name} không có đường dẫn tải xuống.");
            }

            ValidateReleaseAssetUrl(asset.BrowserDownloadUrl);
        }

        private static void ValidateReleaseAssetUrl(string url)
        {
            if (!Uri.TryCreate(
                url,
                UriKind.Absolute,
                out Uri? uri))
            {
                throw new InvalidDataException(
                    "Đường dẫn tải xuống không hợp lệ.");
            }

            if (uri.Scheme != Uri.UriSchemeHttps ||
                !string.Equals(
                    uri.Host,
                    "github.com",
                    StringComparison.OrdinalIgnoreCase) ||
                !uri.AbsolutePath.Contains(
                    "/releases/download/",
                    StringComparison.Ordinal))
            {
                throw new InvalidDataException(
                    "Nguồn tải xuống không được phép.");
            }

            if (uri.AbsolutePath.Contains(".."))
            {
                throw new InvalidDataException(
                    "Đường dẫn tải xuống không hợp lệ.");
            }
        }
    }
}
