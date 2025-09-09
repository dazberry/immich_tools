using Crank.Result;
using immich_common.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace immich_uploader.Models
{

	public class TakeoutMetadata
	{
		[JsonPropertyName("title")]
		public string? Title { get; set; }

		[JsonPropertyName("description")]
		public string? Description { get; set; }

		[JsonPropertyName("imageViews")]
		public string? ImageViews { get; set; }

		[JsonPropertyName("creationTime")]
		public TimeInfo? CreationTime { get; set; }

		[JsonPropertyName("photoTakenTime")]
		public TimeInfo? PhotoTakenTime { get; set; }

		[JsonPropertyName("geoData")]
		public GeoData? GeoData { get; set; }

		[JsonPropertyName("geoDataExif")]
		public GeoData? GeoDataExif { get; set; }

		[JsonPropertyName("url")]
		public string? Url { get; set; }

		[JsonPropertyName("googlePhotosOrigin")]
		public GooglePhotosOrigin? GooglePhotosOrigin { get; set; }


		public static async Task<Result<TakeoutMetadata>> Load(string supplimentalFilename)
		{
			if (!File.Exists(supplimentalFilename))
				return (false, default!, "File not found");

			var content = await File.ReadAllTextAsync(supplimentalFilename);

			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			};

			var metadata = JsonSerializer.Deserialize<TakeoutMetadata>(content, options);

			return (metadata != null, metadata!, metadata == null ? "Failed to parse JSON" : null!);
		}
		
		public static async Task<Result<TakeoutMetadata>> FindAndLoad(string imageFilename)
		{
			var path = Path.GetDirectoryName(imageFilename);
			var filename = Path.GetFileName(imageFilename);

			var supplementalFiles = Directory.GetFiles(path, filename + ".supplemental*.json");

			foreach(var supplementalFile in supplementalFiles)
			{
				var loadResult = await Load(supplementalFile);
				if (loadResult.Succeeded)
					return loadResult;
			}

			return (false, default!, "File not found");

		}

		public bool TryGetUrlId(out string? urlId)
		{
			urlId = default;
			if (string.IsNullOrEmpty(Url))
				return false;

			try
			{
				var uri = new Uri(Url);
				urlId = uri.Segments.Last();
				return true;
			}
			catch
			{
			}
			return false;
		}
	}

	public class TimeInfo
	{
		[JsonPropertyName("timestamp")]
		public string? Timestamp { get; set; }


		[JsonPropertyName("formatted")]
		public string? Formatted { get; set; }

		[JsonIgnore]
		public string? FormattedUtcRfc3339 =>
			Timestamp?.TryGetUtcRfc3339(out var value) ?? false
				? value
				: string.Empty;

		[JsonIgnore]
		public DateTime? DateTime
		{
			get
			{
				if (string.IsNullOrEmpty(this.Timestamp))
					return null;
				if (!long.TryParse(this.Timestamp, out var value))
					return null;
				DateTimeOffset dto = Timestamp.Length >= 13
					? DateTimeOffset.FromUnixTimeMilliseconds(value)
					: DateTimeOffset.FromUnixTimeSeconds(value);
				return dto.UtcDateTime;
			}
		}

	}

	public class GeoData
	{
		[JsonPropertyName("latitude")]
		public double Latitude { get; set; }

		[JsonPropertyName("longitude")]
		public double Longitude { get; set; }

		[JsonPropertyName("altitude")]
		public double Altitude { get; set; }

		[JsonPropertyName("latitudeSpan")]
		public double LatitudeSpan { get; set; }

		[JsonPropertyName("longitudeSpan")]
		public double LongitudeSpan { get; set; }
	}

	public class GooglePhotosOrigin
	{
		[JsonPropertyName("mobileUpload")]
		public MobileUpload? MobileUpload { get; set; }
	}

	public class MobileUpload
	{
		[JsonPropertyName("deviceFolder")]
		public DeviceFolder? DeviceFolder { get; set; }

		[JsonPropertyName("deviceType")]
		public string? DeviceType { get; set; }
	}

	public class DeviceFolder
	{
		[JsonPropertyName("localFolderName")]
		public string? LocalFolderName { get; set; }
	}

}
