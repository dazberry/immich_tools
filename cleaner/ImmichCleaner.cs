using Crank.Result;
using immich_cleaner.CommandLine;
using immich_cleaner.Views;
using immich_common.Api;
using immich_common.Api.Models;
using immich_common.Interaction;
using Spectre.Console;

namespace immich_cleaner
{
	public static class ImmichCleaner
	{
		public static async Task<Result> Invoke(Options options)
		{
			BannerView.ShowParameters(options);

			using ImmichApiClient client = new(options.ImmichUrl, options.ApiKey);

			var request = new SearchAssetsRequest();
			if (!string.IsNullOrEmpty(options.DeviceId))
				request.DeviceId = options.DeviceId;

			var (success, searchResult, message) = await client.SearchAssets(request).Deref();
			if (!success)
				return (false, $"SearchAssets failed: {message}");

			if (searchResult.Assets.Count == 0)
				return (false, "No matching assets returned from SearchAssets");

			BannerView.ShowPendingDeletions(searchResult.Assets.Count);


			var result = UserInput.SelectOption(1, $"Delete [white bold]{searchResult.Assets.Count}[/] assets:", "YYes", "NNo");
			if (result == -1 || result == 1)
				return false;
			Console.WriteLine();


			while (true)
			{

				var assetIds = searchResult.Assets.Items
					.Select(a => new Guid(a.Id))
					.ToArray();

				var deleteResult = await client.DeleteAssets(assetIds);
				if (deleteResult.Succeeded)
				{
					AnsiConsole.WriteLine("Assets deleted");


					(success, searchResult, message) = await client.SearchAssets(request).Deref();
					if (!success)
						return (false, $"SearchAssets failed: {message}");

					if (searchResult.Assets.Count == 0)
						return (false, "No matching assets returned from SearchAssets");

					continue;
				}


				AnsiConsole.WriteLine($"{deleteResult.Value}: {deleteResult.Message}");
				return false;
			}
		}
	}
}
