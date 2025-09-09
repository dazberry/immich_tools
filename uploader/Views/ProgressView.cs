using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace immich_uploader.Views
{
	public static class ProgressView
	{
		public static void ShowFilename(string filename)
		{
			var filePanel = new Panel(new TextPath(filename)).BorderStyle(Style.Plain).Expand();
			AnsiConsole.Write(filePanel);
		}

		public static void ShowProgress(int remaining, int uploaded, int skipped, int replaced, int duplicate, int error)
		{
			var progressText = $"count: {remaining} up: {uploaded}, skipped: {skipped}, replaced: {replaced}, dup: {duplicate}, err: {error}";
			AnsiConsole.Write(progressText);
		}
	}
}
