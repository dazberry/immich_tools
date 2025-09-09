using System.Text.Json.Serialization;

namespace immich_common.Api.Common
{
    public class SearchMetadataResponse
    {
        [JsonPropertyName("albums")]
        public AlbumContainer Albums { get; set; }

        [JsonPropertyName("assets")]
        public AssetContainer Assets { get; set; }
    }

    public class AlbumContainer
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("items")]
        public List<object> Items { get; set; }  // albums are empty in your example; you can create a real Album model if needed

        [JsonPropertyName("facets")]
        public List<object> Facets { get; set; }
    }

    public class AssetContainer
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("items")]
        public List<AssetItem> Items { get; set; }

        [JsonPropertyName("facets")]
        public List<object> Facets { get; set; }
    }

    public class AssetItem
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("deviceAssetId")]
        public string? DeviceAssetId { get; set; }

        [JsonPropertyName("ownerId")]
        public string? OwnerId { get; set; }

        [JsonPropertyName("deviceId")]
        public string? DeviceId { get; set; }

        [JsonPropertyName("libraryId")]
        public string? LibraryId { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("originalPath")]
        public string? OriginalPath { get; set; }

        [JsonPropertyName("originalFileName")]
        public string? OriginalFileName { get; set; }

        [JsonPropertyName("originalMimeType")]
        public string? OriginalMimeType { get; set; }

        [JsonPropertyName("thumbhash")]
        public string? Thumbhash { get; set; }

        [JsonPropertyName("fileCreatedAt")]
        public DateTime FileCreatedAt { get; set; }

        [JsonPropertyName("fileModifiedAt")]
        public DateTime FileModifiedAt { get; set; }

        [JsonPropertyName("localDateTime")]
        public DateTime LocalDateTime { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("isFavorite")]
        public bool IsFavorite { get; set; }

        [JsonPropertyName("isArchived")]
        public bool IsArchived { get; set; }

        [JsonPropertyName("isTrashed")]
        public bool IsTrashed { get; set; }

        [JsonPropertyName("visibility")]
        public string? Visibility { get; set; }

        [JsonPropertyName("duration")]
        public string? Duration { get; set; }

        [JsonPropertyName("livePhotoVideoId")]
        public string? LivePhotoVideoId { get; set; }

        [JsonPropertyName("people")]
        public List<object> People { get; set; }

        [JsonPropertyName("checksum")]
        public string? Checksum { get; set; }

        [JsonPropertyName("isOffline")]
        public bool IsOffline { get; set; }

        [JsonPropertyName("hasMetadata")]
        public bool HasMetadata { get; set; }

        [JsonPropertyName("duplicateId")]
        public string? DuplicateId { get; set; }

        [JsonPropertyName("resized")]
        public bool Resized { get; set; }
    }
}
