using System.Globalization;
using System.Text.RegularExpressions;

namespace immich_common.Extensions
{
	public static class FilenameExtensions
	{
		public static bool TryGetDateTimeFromFilename(this string filename, out DateTime? dateTime)
		{
			dateTime = null;

			if (string.IsNullOrWhiteSpace(filename))
				return false;

			// Extract just the file name (without path)
			string name = Path.GetFileNameWithoutExtension(filename);

			// Match pattern: IMG_YYYYMMDD_HHMMSS...
			var match = Regex.Match(name, @"(\d{8})_(\d{6})");

			if (match.Success)
			{
				string datePart = match.Groups[1].Value; // YYYYMMDD
				string timePart = match.Groups[2].Value; // HHMMSS

				if (DateTime.TryParseExact(
					datePart + timePart,
					"yyyyMMddHHmmss",
					CultureInfo.InvariantCulture,
					DateTimeStyles.None,
					out DateTime parsed))
				{
					dateTime = parsed;
					return true;
				}
			}

			return false;
		}

	}
}
