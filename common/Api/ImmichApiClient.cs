using Crank.Result;
using immich_common.Api.Common;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using immich_common.Extensions;
using immich_common.Api.Models;
using System.Text.Json.Serialization;

namespace immich_common.Api
{
	public class ImmichApiClient : IDisposable
	{
		private HttpClient? _httpClient = null;
		private Uri _baseUrl;
		public ImmichApiClient(string? baseUrl, string? ApiKey)
		{
			ArgumentNullException.ThrowIfNull(baseUrl);


			_baseUrl = new Uri(baseUrl);

			_httpClient = new HttpClient();
			_httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-api-key", ApiKey);
			_httpClient.DefaultRequestHeaders.Accept.Add(
				new MediaTypeWithQualityHeaderValue("application/json"));
		}


		private string BuildUrl(string path) =>
			new Uri(_baseUrl, path).ToString();

		public async Task<Result<string[]>> GetAllUserAssetsByDeviceId(string deviceId)
		{
			try
			{
				var url = BuildUrl($"api/assets/device/{deviceId}");
				var result = await _httpClient!.GetAsync(url);
				var content = await result.Content.ReadAsStringAsync();

				var ids = JsonSerializer.Deserialize<string[]>(content);

				return (ids != null, ids!);

			}
			catch (Exception ex)
			{
				return (false, default!, ex.Message);
			}
		}

		public async Task<Result<SearchMetadataResponse>> SearchAssets(
			SearchAssetsRequest searchAssetsRequest)
		{

			try
			{
				var url = BuildUrl($"api/search/metadata");

				var options = new JsonSerializerOptions
				{
					DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
					PropertyNamingPolicy = JsonNamingPolicy.CamelCase
				};

				var content = JsonSerializer.Serialize(searchAssetsRequest, options);
				var request = new HttpRequestMessage
				{
					Method = HttpMethod.Post,
					RequestUri = new Uri(url),
					Content = new StringContent(content, Encoding.UTF8, "application/json")
				};

				using var response = await _httpClient.SendAsync(request);
				var responseBody = await response.Content.ReadAsStringAsync();
				var metadataResponse = JsonSerializer.Deserialize<SearchMetadataResponse>(responseBody);

				return (response.IsSuccessStatusCode, metadataResponse!);
			}
			catch (Exception ex)
			{
				return (false, default!, ex.Message);
			}
		}

		public async Task<Result<int>> DeleteAssets(params Guid[] assetIds)
		{
			var url = BuildUrl($"api/assets");
			var requestBody = new
			{
				force = true,
				ids = assetIds
			};
			var content = JsonSerializer.Serialize(requestBody);

			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Delete,
				RequestUri = new Uri(url),
				Content = new StringContent(content, Encoding.UTF8, "application/json")
			};

			using var response = await _httpClient.SendAsync(request);
			string responseBody = await response.Content.ReadAsStringAsync();
			return (response.IsSuccessStatusCode, (int)response.StatusCode, responseBody);

		}


		private bool TryGetDateTimeFromFilename(string filename, out DateTime? dateTime)
		{
			var match = Regex.Match(filename, @"(?<date>\d{8})_(?<time>\d{6})(?<ms>\d{3})?");
			if (match.Success)
			{
				string datePart = match.Groups["date"].Value; // 20190108
				string timePart = match.Groups["time"].Value; // 212655
				string msPart = match.Groups["ms"].Success ? match.Groups["ms"].Value : "000"; // 060

				string fullTimestamp = $"{datePart}{timePart}{msPart}";

				var result = DateTime.TryParseExact(fullTimestamp, "yyyyMMddHHmmssfff",
										CultureInfo.InvariantCulture, DateTimeStyles.None,
										out DateTime dt);
				if (result)
				{
					dateTime = dt;
					return true;
				}
			}
			dateTime = default;
			return false;
		}


		public async Task<Result<UploadAssetResponse>> UploadAsset(AssetFileInfo assetFileInfo)
		{
			var url = BuildUrl($"api/assets");

			//string fileTitle = takeoutMetadata?.Title ?? string.Empty;
			//if (string.IsNullOrEmpty(fileTitle))
			//	fileTitle = Path.GetFileName(filename);


			//DateTime photoTakenDate =
			//	takeoutMetadata?.PhotoTakenTime?.DateTime
			//	?? takeoutMetadata?.CreationTime?.DateTime
			//	?? new FileInfo(filename).CreationTime;
			//if (photoTakenDate == DateTime.MinValue)
			//	photoTakenDate = DateTime.UtcNow;

			var timestamp = $"{new DateTimeOffset(assetFileInfo.Timestamp.Value).ToUnixTimeSeconds()}";

			var filename = Path.GetFileName(assetFileInfo.PathAndFilename);

			using var form = new MultipartFormDataContent
			{
				{ new StringContent(assetFileInfo.DeviceId), "deviceId" },
				{ new StringContent(assetFileInfo.DeviceAssetId), "deviceAssetId" },
				{ new StringContent(filename), "filename" },
				{ new StringContent(assetFileInfo.FileTitle), "title" }
			};

			if (assetFileInfo.Timestamp.TryGetUtcRfc3339(out string value))
			{
				form.Add(new StringContent(value), "fileModifiedAt");
				form.Add(new StringContent(value), "fileCreatedAt");
			}

			using var stream = File.OpenRead(assetFileInfo.PathAndFilename);
			form.Add(new StreamContent(stream), "assetData", Path.GetFileName(filename));

			try
			{
				using var response = await _httpClient.PostAsync(url, form);
				string responseBody = await response.Content.ReadAsStringAsync();

				if (response.IsSuccessStatusCode)
				{
					var uploadAssetResponse = JsonSerializer.Deserialize<UploadAssetResponse>(responseBody);
					return (uploadAssetResponse != null, uploadAssetResponse);
				}

				return (false, default!, responseBody);
			}
			catch (Exception ex)
			{
				return (false, default!, ex.Message);
			}
		}

		public void Dispose()
		{
			_httpClient?.Dispose();
			_httpClient = null;
		}

	}
}
