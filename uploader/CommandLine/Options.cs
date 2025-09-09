using CommandLine;
using Crank.Result;

namespace immich_uploader.CommandLine
{
	public class Options
	{

		[Option('a', "api-key", Required = false, HelpText = "Immich API key")]
		public string? ApiKey { get; set; }

		[Option('i', "immich-url", Required = false, HelpText = "Immich Url")]
		public string? ImmichUrl { get; set; }

		[Option('s', "takeout-folder", Required = false, HelpText = "Source files extract from google takeout zip file")]
		public string? Source { get; set; }

		[Option('c', "config", Required = false, HelpText = "Config file, underride command line settings")]
		public string? Config { get; set; }

		[Option('d', "device-id", Required = false, HelpText = "Device Id")]
		public string? DeviceId { get; set; } = "GoogleTakeout";


		public static Result<Options> Load(string[] args)
		{
			var parser = new Parser(with =>
			{
				with.HelpWriter = Console.Out;
				with.AutoHelp = true;
				with.AutoVersion = false;
				with.CaseInsensitiveEnumValues = true;
			});
			var result = parser.ParseArguments<Options>(args);

			if (result.Tag == ParserResultType.Parsed)
			{
				var options = ((Parsed<Options>)result).Value;
				return options;
			}

			return (false, default!);
		}

		public Result Validate()
		{
			if (string.IsNullOrEmpty(ApiKey))
				return (false, "Missing api-key value");
			if (string.IsNullOrEmpty(ImmichUrl))
				return (false, "Missing immich-url value");
			if (string.IsNullOrEmpty(Source))
				return (false, "Missing takeout-folder value");

			if (!Path.Exists(Source))
				return (false, "Invalid takeout-folder path");

			return true;
		}
	}
}
