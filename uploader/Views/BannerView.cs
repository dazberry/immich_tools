using immich_uploader.CommandLine;
using Spectre.Console;

namespace immich_uploader.Views
{
	public static class BannerView
	{
		public static void ShowBanner()
		{
			AnsiConsole.MarkupLine("[Yellow]immich uploader[/]");
		}

		public static void ShowParameters(Options options)
		{
			var table = new Table().Border(TableBorder.Minimal);
			table.AddColumns("Param", "Value");
			table.AddRow("Source", options.Source);
			table.AddRow("immich url", options.ImmichUrl);

			AnsiConsole.Write(table);
		}

	}
}
