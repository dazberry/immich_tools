namespace immich_common.Extensions
{

	public static class TimeInfoExtensions
	{
		// Converts Google Takeout "timestamp" (usually seconds) to RFC3339 UTC.
		public static bool TryGetUtcRfc3339(this string timestamp, out string? iso)
		{
			iso = null;
			if (string.IsNullOrWhiteSpace(timestamp))
				return false;

			if (!long.TryParse(timestamp, out var raw))
				return false;

			// Handle seconds vs milliseconds just in case
			DateTimeOffset dto = timestamp.Length >= 13
				? DateTimeOffset.FromUnixTimeMilliseconds(raw)
				: DateTimeOffset.FromUnixTimeSeconds(raw);

			iso = dto.UtcDateTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
			return true;
		}

		public static bool TryGetDateTime(this string timestamp, out DateTime? dateTime)
		{
			dateTime = null;

			if (string.IsNullOrWhiteSpace(timestamp))
				return false;

			if (!long.TryParse(timestamp, out var raw))
				return false;

			try
			{
				// Handle seconds vs milliseconds
				DateTimeOffset dto = timestamp.Length >= 13
					? DateTimeOffset.FromUnixTimeMilliseconds(raw)
					: DateTimeOffset.FromUnixTimeSeconds(raw);

				dateTime = dto.UtcDateTime;
				return true;
			}
			catch
			{
				return false;
			}
		}


		public static bool TryGetUtcRfc3339(this DateTime? dateTime, out string? iso)
		{
			iso = null;
			if (dateTime == null) return false;

			DateTimeOffset dto = new DateTimeOffset(dateTime.Value);
			iso = dto.UtcDateTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
			return true;

		}
	}

}
