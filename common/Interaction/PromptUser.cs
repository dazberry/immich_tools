using Crank.Result;
using Spectre.Console;

namespace immich_common.Interaction
{


	public static class UserInput
	{
		public static int SelectOption(
			int defaultIndex,
			string prefixMarkup,
			params string[] options)
		{
			var index = defaultIndex;
			var count = options.Length;
			var optionKeys = options.Select(o => o[0]).ToArray();

			while (true)
			{
				var optionsWithMarkup = string.Join(", ", options.Select((opt, idx) =>
				{
					var optionKey = opt[0];
					var text = opt[1..];

					var indexOfOptionsKey = text.IndexOf(optionKey);
					if (indexOfOptionsKey == -1) indexOfOptionsKey = 0;

					var beforeOptionsKey = text[..indexOfOptionsKey];
					var afterOptionsKey = text[(indexOfOptionsKey + 1)..];
					var markup = idx == index
						? $"{beforeOptionsKey}[yellow on blue][[{optionKey}]][/]{afterOptionsKey}"
						: $"{beforeOptionsKey}[[{optionKey}]]{afterOptionsKey}";

					return markup;
				})) + ":";

				Console.SetCursorPosition(0, Console.CursorTop);
				Console.Write(new string(' ', Console.WindowWidth - 1)); // clear line
				Console.SetCursorPosition(0, Console.CursorTop);

				AnsiConsole.Markup(prefixMarkup + optionsWithMarkup);

				var key = Console.ReadKey(intercept: true);
				if (key.Key == ConsoleKey.LeftArrow)
				{
					index -= 1;
					if (index < 0) index = 0;
					continue;
				}
				if (key.Key == ConsoleKey.RightArrow)
				{
					index += 1;
					if (index >= count) index = count - 1;
					continue;
				}
				if (key.Key == ConsoleKey.Home)
				{
					index = 0;
					continue;
				}
				if (key.Key == ConsoleKey.End)
				{
					index = count - 1;
					continue;
				}
				if (key.Key == ConsoleKey.Enter)
				{
					Console.Write(optionKeys[index]);
					return index;
				}
				if (key.Key == ConsoleKey.Escape)
				{
					return -1;
				}

				var keyIndex = Array.IndexOf(optionKeys, key.KeyChar);
				if (keyIndex != -1)
					return keyIndex;

			}
		}


	}
}
