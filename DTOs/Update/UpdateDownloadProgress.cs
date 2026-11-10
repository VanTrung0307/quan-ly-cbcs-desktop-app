
namespace QuanLyCBCS.Services.Update
{
    /// <summary>
    /// Thông tin tiến trình tải gói cập nhật.
    /// </summary>
    public sealed class UpdateDownloadProgress
    {
        public long BytesReceived { get; set; }

        public long TotalBytes { get; set; }

        public int Percentage { get; set; }

        public string StatusMessage { get; set; } = string.Empty;
    }
}
