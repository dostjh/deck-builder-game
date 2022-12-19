using System;

namespace DeckBuilderGame
{
	public static class Util
	{
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
	}
}