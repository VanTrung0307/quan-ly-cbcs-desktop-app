using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace QuanLyCBCS.DTOs.Update
{
    /// <summary>
    /// Đại diện cho một bản phát hành GitHub Release.
    /// Ánh xạ theo cấu trúc GitHub REST API: GET /repos/{owner}/{repo}/releases/latest.
    /// </summary>
    internal sealed class GitHubReleaseDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("tag_name")]
        public string TagName { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("body")]
        public string Body { get; set; } = string.Empty;

        [JsonPropertyName("draft")]
        public bool Draft { get; set; }

        [JsonPropertyName("prerelease")]
        public bool Prerelease { get; set; }

        [JsonPropertyName("target_commitish")]
        public string TargetCommitish { get; set; } = string.Empty;

        [JsonPropertyName("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        [JsonPropertyName("published_at")]
        public DateTimeOffset? PublishedAt { get; set; }

        [JsonPropertyName("html_url")]
        public string HtmlUrl { get; set; } = string.Empty;

        [JsonPropertyName("assets")]
        public List<GitHubAssetDto> Assets { get; set; } = new();
    }
}
