using Crank.Result;
using immich_common.Api;
using immich_common.Api.Common;
using immich_common.Extensions;
using immich_uploader.CommandLine;
using immich_uploader.Models;
using immich_uploader.Views;
using Spectre.Console;

namespace immich_uploader
{
	public static class ImmichUploader
	{


		public static async Task<Result> Invoke(Options options)
		{
			string[] imageFileExtensions = [".gif", ".jpg", ".bmp", ".mp4"];

			BannerView.ShowParameters(options);

			using ImmichApiClient client = new(options.ImmichUrl, options.ApiKey);

			var allFiles = Directory.GetFiles(options.Source, "*.*", SearchOption.AllDirectories);

			var imageFiles = allFiles
				.Where(f => imageFileExtensions.Contains(Path.GetExtension(f)))
				.ToArray();


			if (imageFiles.Length == 0)
				return (false, "No image files found.");

			using ImmichApiClient immichApiClient = new ImmichApiClient(options.ImmichUrl, options.ApiKey);

			int remaining = imageFiles.Count();
			int uploaded = 0;
			int skipped = 0;
			int duplicate = 0;
			int replaced = 0;
			int error = 0;

			var (left, top) = Console.GetCursorPosition();
			Console.CursorVisible = false;
			try
			{
				foreach (var imageFile in imageFiles)
				{
					try
					{
						Console.SetCursorPosition(left, top);
						ProgressView.ShowFilename(imageFile);

						var (haveTakeoutMetadata, takeoutMetadata) = await TakeoutMetadata.FindAndLoad(imageFile);

						string filename = Path.GetFileName(imageFile);
						var id = haveTakeoutMetadata && (takeoutMetadata?.TryGetUrlId(out var urlId) ?? false)
							? urlId
							: Guid.NewGuid().ToString("N");

						var assetFileInfo = new AssetFileInfo()
						{
							PathAndFilename = imageFile,
							FileTitle = filename,
							DeviceId = options.DeviceId,
							DeviceAssetId = $"{filename}.{id}",
							Timestamp = haveTakeoutMetadata &&
										(takeoutMetadata?.PhotoTakenTime?.Timestamp?.TryGetDateTime(out DateTime? photoTakenDt) ?? false)
								? photoTakenDt
								: filename.TryGetDateTimeFromFilename(out DateTime? dateTime)
									? dateTime!.Value
									: DateTime.UtcNow
						};

						var (success, uploadResult, _) = await immichApiClient.UploadAsset(assetFileInfo).Deref();
						//Result<UploadAssetResponse> res = (true, new UploadAssetResponse() { Status = "created" });
						//var (success, uploadResult, _) = res.Deref();

						if (success)
						{
							switch (uploadResult.Status)
							{
								case "created":
									uploaded += 1;
									break;
								case "replaced":
									replaced += 1;
									break;
								case "duplicate":
									duplicate += 1;
									break;
								default:
									error += 1;
									break;
							}
						}
						else
							error += 1;
					}
					finally
					{
						remaining -= 1;
						ProgressView.ShowProgress(remaining, uploaded, skipped, replaced, duplicate, error);
					}

				}
			}
			finally
			{
				Console.CursorVisible = true;
			}

			return false;
		}
	}
}
