
using System;
using System.Text.Json.Serialization;

namespace QuanLyCBCS.Services.Update
{
    public sealed class UpdateManifest
    {
        [JsonPropertyName("version")]
        public string Version { get; set; } = string.Empty;

        [JsonPropertyName("packageFileName")]
        public string PackageFileName { get; set; } = string.Empty;

        [JsonPropertyName("sha256")]
        public string Sha256 { get; set; } = string.Empty;

        [JsonPropertyName("releaseNotes")]
        public string ReleaseNotes { get; set; } = string.Empty;

        [JsonPropertyName("publishedAtUtc")]
        public DateTimeOffset PublishedAtUtc { get; set; }

        [JsonPropertyName("mandatory")]
        public bool Mandatory { get; set; }
    }
}
