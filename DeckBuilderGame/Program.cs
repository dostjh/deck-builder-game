using DeckBuilderGame.GameAtoms;
using DeckBuilderGame.SerializableClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace DeckBuilderGame
{
	internal class Program
	{
		private static void Main()
		{
			
			var games = ImportGames(@".\..\..\..\config\Games.xml");
			var cardLibrary = ImportCardLibrary(@".\..\..\..\config\CardLibrary.xml");

			//var userGameSelection = Util.GetUserInputOption("What game are we playing?", games.Games.Select(g => g.Name));
			var userGameSelection = "Dominion - Base";
			var game = games.Games.First(g => g.Name == userGameSelection);
			var playerCount = Util.GetUserInputInt("How many players are playing?", 2, 4);

			var gameState = new GameState(	playerCount, 
											cardLibrary.CardDefinitions.Select(cd => new Card(cd)), 
											game);

			foreach (var player in gameState.Players.Values)
			{
				player.StartTurn();
				player.PlayCard(0, gameState);
				player.EndTurn();
				player.ShuffleDiscardPileIntoDrawPile();
			}

			Console.WriteLine($"The game has {gameState.Players.Count} players.");
			foreach (var player in gameState.Players)
			{
				Console.WriteLine($"Player {player.Key}: {player.Value}");
			}
			Console.WriteLine("Game over. Press any key to end.");
			Console.ReadLine();
		}

		private static GamesDataSerializable ImportGames(string gameDefinitionFilePath)
		{
			// Import games
			var serializer = new XmlSerializer(typeof(GamesDataSerializable));
			GamesDataSerializable games;
			using (var reader = new FileStream(gameDefinitionFilePath, FileMode.Open))
			{
				games = (GamesDataSerializable)serializer.Deserialize(reader);
			}

			return games;
		}

		private static CardDefinitionsSerializable ImportCardLibrary(string cardLibraryFilePath)
		{
			// Import card library
			var serializer = new XmlSerializer(typeof(CardDefinitionsSerializable));
			CardDefinitionsSerializable cardDefinitionLibrary;
			using (var reader = new FileStream(cardLibraryFilePath, FileMode.Open))
			{
				cardDefinitionLibrary = (CardDefinitionsSerializable)serializer.Deserialize(reader);
			}

			return cardDefinitionLibrary;
		}
	}
}
