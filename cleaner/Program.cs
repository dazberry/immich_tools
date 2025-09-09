using Crank.Result;
using immich_cleaner.CommandLine;
using immich_cleaner.Views;

namespace immich_cleaner
{
	internal class Program
	{
		static async Task<int> Main(string[] args)
		{
			BannerView.ShowBanner();

			var (success, options, message) = Options.Load(args).Deref();
			if (!success)
				return -1;

			if (!string.IsNullOrEmpty(options.Config))
			{
				(success, var config, message) = Config.LoadFromFile(options.Config).Deref();
				if (!success)
				{
					Console.WriteLine($"Failed to load --config file with error: {message}");
					return -1;
				}

				config.UnderrideOptions(options);
			}

			(success, message) = options.Validate();
			if (!success)
			{
				Console.WriteLine(message);
				return -1;
			}

			(success, message) = await ImmichCleaner.Invoke(options);
			if (!success)
			{
				Console.WriteLine(message);
				return -1;
			}

			return 0;
		}
	}
}
