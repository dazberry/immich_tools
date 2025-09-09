using Crank.Result;
using immich_uploader.CommandLine;
using immich_uploader.Views;

namespace immich_uploader
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


			(success, message) = await ImmichUploader.Invoke(options);
			if (!success)
			{
				Console.WriteLine(message);
				return -1;
			}

			return 0;
		}
	}
}
