using immich_cleaner.CommandLine;
using Spectre.Console;

namespace immich_cleaner.Views
{
	public static class BannerView
	{
		public static void ShowBanner()
		{
			AnsiConsole.MarkupLine("[Yellow]immich cleaner[/]");
		}

		public static void ShowParameters(Options options)
		{
			bool haveDeviceAssetId = !string.IsNullOrEmpty(options.DeviceId);

			if (!haveDeviceAssetId)
				return;

			var table = new Table().Border(TableBorder.Minimal);
			table.AddColumns("Param", "Value");
			table.AddRow("deviceAssetId", options.DeviceId);

			AnsiConsole.Write(table);
		}

		public static void ShowPendingDeletions(int deletionCount)
		{
			AnsiConsole.MarkupLine($"Warning: [Red]Pending deletion: {deletionCount}[/]");
		}
	}
}
