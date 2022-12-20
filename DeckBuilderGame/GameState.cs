using DeckBuilderGame.Cards;
using DeckBuilderGame.GameAtoms;
using System.Collections.Generic;

namespace DeckBuilderGame
{
	internal class GameState
	{
		public Dictionary<int, Player> Players;
		public List<Card> Trash;
		public int Round;
		public int CurrentPlayer;
		public int ActionMax;
		public int DrawCount;

		public GameState()
		{

		}

		public GameState(int playerCount, List<Card> cards, GameDataSerializable gameData)
		{
			Players = new Dictionary<int, Player>();
			for (var i = 0; i < playerCount; i++)
			{
				Players.Add(i, new Player(i, cards));
			}
			
			Trash = new List<Card>();
			Round = 0;
			CurrentPlayer = 0;
			ActionMax = gameData.Rules.MaxAction;
			DrawCount = gameData.Rules.DrawCount;
		}
	}
}
