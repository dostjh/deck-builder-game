using System;
using System.Collections.Generic;
using System.Linq;

namespace DeckBuilderGame
{
	public static class Util
	{
		public static string GetUserInputOption(string prompt, IEnumerable<string> options)
		{
			var result = string.Empty;
			var success = false;

			while (!success)
			{
				Console.WriteLine(prompt);
				Console.WriteLine($"Options: {string.Join(", ", options)}");
				result = Console.ReadLine();

				success = options.Select(o => o.ToLower()).Any(o => o == result.ToLower().Trim());
			}

			return result;
		}

		public static int GetUserInputInt(string prompt)
		{
			var result = -1;
			var success = false;

			while (!success)
			{
				Console.WriteLine(prompt);
				var input = Console.ReadLine();

				success = int.TryParse(input, out result);
			}

			return result;
		}

		public static int GetUserInputInt(string prompt, int min, int max)
		{
			var result = -1;
			var success = false;
			prompt = $"{prompt} (Choose a number between {min} and {max}.)";

			while (!success)
			{
				result = GetUserInputInt(prompt);
				success = IsValueInRange(result, min, max);
			}

			return result;
		}

		public static bool IsValueInRange(int value, int min, int max)
		{
			return value >= min && value <= max;
		}

		public static void Error(string message)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(message);
			Console.ResetColor();
		}
	}
}