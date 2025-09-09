using System.Text.Json.Serialization;

namespace immich_common.Api.Common
{
	public class UploadAssetResponse
	{
		[JsonPropertyName("id")]
		public string? Id { get; set; }
		[JsonPropertyName("status")]
		public string? Status { get; set; }
	}
}
