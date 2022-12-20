using DeckBuilderGame.Cards;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace DeckBuilderGame
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var playerCount = Util.GetUserInputInt("How many players are playing?", 2, 4);

			var cards = new List<GameAtoms.Card>();

			var gameData = ImportGameData(@".\..\..\..\config\Dominion.xml");
			foreach (var card in gameData.Cards)
			{
				cards.Add(new GameAtoms.Card(card));
			}

			var gameState = new GameState(playerCount, cards, gameData);
			foreach (var player in gameState.Players.Values)
			{
				player.StartTurn();
				player.EndTurn();
				player.Shuffle();
				player.PlayCard(0);
			}

			Console.WriteLine($"The game has {gameState.Players.Count} players.");
			foreach (var player in gameState.Players)
			{
				Console.WriteLine($"Player {player.Key}: {player.Value}");
			}
			Console.ReadLine();
		}

		private static GameDataSerializable ImportGameData(string filePath)
		{
			var serializer = new XmlSerializer(typeof(GameDataSerializable));

			GameDataSerializable cardData;

			using (var reader = new FileStream(filePath, FileMode.Open))
			{
				cardData = (GameDataSerializable)serializer.Deserialize(reader);
			}

			return cardData;
		}
	}
}
