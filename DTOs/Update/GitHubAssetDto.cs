using System;
using System.Text.Json.Serialization;

namespace QuanLyCBCS.DTOs.Update
{
    /// <summary>
    /// Đại diện cho một file đính kèm (asset) trong GitHub Release.
    /// Ánh xạ theo cấu trúc GitHub REST API: GET /repos/{owner}/{repo}/releases/latest.
    /// </summary>
    internal sealed class GitHubAssetDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("label")]
        public string? Label { get; set; }

        [JsonPropertyName("content_type")]
        public string ContentType { get; set; } = string.Empty;

        [JsonPropertyName("state")]
        public string State { get; set; } = string.Empty;

        [JsonPropertyName("size")]
        public long Size { get; set; }

        [JsonPropertyName("download_count")]
        public long DownloadCount { get; set; }

        [JsonPropertyName("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTimeOffset? UpdatedAt { get; set; }

        [JsonPropertyName("browser_download_url")]
        public string BrowserDownloadUrl { get; set; } = string.Empty;

        /// <summary>
        /// API URL của asset trên GitHub; không dùng làm URL tải trực tiếp.
        /// </summary>
        [JsonPropertyName("url")]
        public string ApiUrl { get; set; } = string.Empty;
    }
}
