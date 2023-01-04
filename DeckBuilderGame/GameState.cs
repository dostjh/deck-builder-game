using DeckBuilderGame.SerializableClasses;
using DeckBuilderGame.GameAtoms;
using System.Collections.Generic;
using System.Linq;

namespace DeckBuilderGame
{
	public class GameState
	{
		public Dictionary<int, Player> Players;
		internal List<Card> Trash { get; set; }
		public int Round;
		public int CurrentPlayer;
		public int ActionMax;
		public int DrawCount;
		internal Dictionary<string, int?> CardPool { get; set; }
		internal Dictionary<string, Card> CardDefinitions { get; set; }

		internal GameState(int playerCount, IEnumerable<Card> cards, GameDataSerializable gameData)
		{
			Players = new Dictionary<int, Player>();
			for (var i = 0; i < playerCount; i++)
			{
				Players.Add(i, new Player(i, cards.ToList()));
			}
			
			Trash = new List<Card>();
			Round = 0;
			CurrentPlayer = 0;
			ActionMax = gameData.Rules.MaxAction;
			DrawCount = gameData.Rules.DrawCount;
			CardDefinitions = cards.ToDictionary(c => c.Name, c => c);

			CardPool = GetCardPool(gameData.Cards);
		}

		private Dictionary<string, int?> GetCardPool(IEnumerable<CardSerializable> cards)
		{
			var tempCardPool = new Dictionary<string, int?>();
			
			foreach (var card in cards)
			{
				if (card.NoLimit)
				{
					tempCardPool.Add(card.Name, null);
				}
				else
				{
					tempCardPool.Add(card.Name, card.Pool);
				}
			}

			return tempCardPool;
		}

		internal bool DrawCardFromPool(string cardName)
		{
			var result = false;
			var cardPoolState = CardPool[cardName];

			if (cardPoolState == null || cardPoolState > 0)
			{
				result = true;
				if (cardPoolState != null)
				{ 
					CardPool[cardName]--; 
				}
			}

			return result;
		}

		internal IEnumerable<Card> GetDrawableCardPool(int maxCost)
		{
			var drawableCards = CardPool.Where(c => c.Value > 0 || c.Value == null).Select(cp => cp.Key);
			return CardDefinitions.Values.Where(cd => drawableCards.Contains(cd.Name) || cd.Cost <= maxCost);
		}
	}
}
