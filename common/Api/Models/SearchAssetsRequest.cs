namespace immich_common.Api.Models
{
	public class SearchAssetsRequest
	{
		public List<Guid>? AlbumIds { get; set; }
		public string? Checksum { get; set; }
		public string? City { get; set; }
		public string? Country { get; set; }
		public DateTime? CreatedAfter { get; set; }
		public DateTime? CreatedBefore { get; set; }
		public string? Description { get; set; }
		public string? DeviceAssetId { get; set; }
		public string? DeviceId { get; set; }
		public string? EncodedVideoPath { get; set; }
		public Guid? Id { get; set; }
		public bool? IsEncoded { get; set; }
		public bool? IsFavorite { get; set; }
		public bool? IsMotion { get; set; }
		public bool? IsNotInAlbum { get; set; }
		public bool? IsOffline { get; set; }
		public string? LensModel { get; set; }
		public Guid? LibraryId { get; set; }
		public string? Make { get; set; }
		public string? Model { get; set; }
		public string? Order { get; set; }
		public string? OriginalFileName { get; set; }
		public string? OriginalPath { get; set; }
		public int? Page { get; set; }
		public List<Guid>? PersonIds { get; set; }
		public string? PreviewPath { get; set; }
		public int? Rating { get; set; }
		public long? Size { get; set; }
		public string? State { get; set; }
		public List<Guid>? TagIds { get; set; }
		public DateTime? TakenAfter { get; set; }
		public DateTime? TakenBefore { get; set; }
		public string? ThumbnailPath { get; set; }
		public DateTime? TrashedAfter { get; set; }
		public DateTime? TrashedBefore { get; set; }
		public string? Type { get; set; }
		public DateTime? UpdatedAfter { get; set; }
		public DateTime? UpdatedBefore { get; set; }
		public string? Visibility { get; set; }
		public bool? WithDeleted { get; set; }
		public bool? WithExif { get; set; }
		public bool? WithPeople { get; set; }
		public bool? WithStacked { get; set; }
	}
}
