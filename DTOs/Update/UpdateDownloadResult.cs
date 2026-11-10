
namespace QuanLyCBCS.Services.Update
{
    /// <summary>
    /// Kết quả tải và xác minh gói cập nhật.
    /// </summary>
    public sealed class UpdateDownloadResult
    {
        public bool Success { get; set; }

        public bool Cancelled { get; set; }

        public string FilePath { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public string Sha256 { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } = string.Empty;
    }
}
