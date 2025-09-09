using CommandLine;
using Crank.Result;
using System.ComponentModel.DataAnnotations;

namespace immich_cleaner.CommandLine
{
	public enum ApiMode
	{
		Full,
		Partial,
		Test
	};

	public class Options
	{

		[Option('a', "api-key", Required = false, HelpText = "Immich API key")]
		public string? ApiKey { get; set; }

		[Option('i', "immich-url", Required = false, HelpText = "Immich Url")]
		public string? ImmichUrl { get; set; }


		[Option('d', "device-id", Required = false, HelpText = "Device Id")]
		public string? DeviceId { get; set; }


		[Option('c', "config", Required = false, HelpText = "Config file, underride command line settings")]
		public string? Config { get; set; }



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

			return true;
		}
	}
}
