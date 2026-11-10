
namespace QuanLyCBCS.Services.Update
{
    public sealed class UpdateCheckResult
    {
        public bool Success { get; set; }

        public bool IsUpdateAvailable { get; set; }

        public string CurrentVersion { get; set; } = string.Empty;

        public string LatestVersion { get; set; } = string.Empty;

        public string ReleaseNotes { get; set; } = string.Empty;

        public string PackageFileName { get; set; } = string.Empty;

        public string DownloadUrl { get; set; } = string.Empty;

        public string Sha256 { get; set; } = string.Empty;

        public bool IsMandatory { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;
    }
}
