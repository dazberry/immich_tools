using Crank.Result;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace immich_cleaner.CommandLine
{
	public class Config
	{
		[JsonPropertyName("api-key")]
		public string? ApiKey { get; init; }
		[JsonPropertyName("immich-url")]
		public string? ImmichUrl { get; init; }


		public static Result<Config> LoadFromFile(string? filename)
		{
			if (string.IsNullOrEmpty(filename))
				return (false, default!, "No --config path supplied");

			try
			{
				var json = File.ReadAllText(filename);

				var config = JsonSerializer.Deserialize<Config>(json,
					new JsonSerializerOptions
					{
						PropertyNameCaseInsensitive = true,
						PropertyNamingPolicy = JsonNamingPolicy.CamelCase
					});

				return (config != null, config!);
			}
			catch (Exception ex)
			{
				return (false, default!, ex.Message);
			}
		}

		public void UnderrideOptions(Options options)
		{
			static string? ReplaceValue(string? original, string? replacement)
			{
				if (string.IsNullOrEmpty(original) && !string.IsNullOrEmpty(replacement))
					return replacement;
				return original;
			}

			options.ApiKey = ReplaceValue(options.ApiKey, ApiKey);
			options.ImmichUrl = ReplaceValue(options.ImmichUrl, ImmichUrl);
		}
	}
}
