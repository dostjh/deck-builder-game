using System;
using System.Linq;

namespace DeckBuilderGame
{
	class Program
	{
		static void Main(string[] args)
		{
			var playerCount = Util.GetUserInputInt("How many players are playing?", 2, 4);
			var gameState = new GameState(playerCount);

			Console.WriteLine($"The game has {gameState.Players.Count} players.");
		}
	}
}
