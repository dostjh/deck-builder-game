using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DeckBuilderGame.GameAtoms
{
	public class Player
	{
		public string Name;
		public int Order;
		public int CurrencyCount;
		public int ActionCount;
		public int ActionMax;
		public int BuyCount;
		public int BuyMax;

		public Dictionary<int, Card> Hand { get; set; }
		public List<Card> DrawPile;
		public List<Card> DiscardPile { get; set; }

		public Player()
		{

		}

		public Player(int order, List<Card> drawPile)
		{
			Order = order;
			Hand = new Dictionary<int, Card>();
			DiscardPile = new List<Card>();

			var tempDiscardPile = new Card[drawPile.Count()];
			drawPile.CopyTo(tempDiscardPile);
			DiscardPile = tempDiscardPile.ToList();

			ShuffleDiscardPileIntoDrawPile();
		}

		public override string ToString()
		{
			return $"Player {Order}: {ActionMax} max actions, {Hand.Count} in hand, {DrawPile.Count} in draw, {DiscardPile.Count} in discard";
		}

		public void StartTurn()
		{
			DrawCards(1);
			Console.WriteLine("Player starts turn.");
		}

		public void EndTurn()
		{
			ReorderHand();
			Console.WriteLine("Player ends turn.");
		}

		public void ShuffleDiscardPileIntoDrawPile()
		{
			// Fisher-Yates shuffle http://en.wikipedia.org/wiki/Fisher-Yates_shuffle
			// https://stackoverflow.com/questions/1150646/card-shuffling-in-c-sharp
			var random = new Random();
			for (var n = DiscardPile.Count - 1; n > 0; n--)
			{
				var k = random.Next(n + 1);
				var temp = DiscardPile[n];
				DiscardPile[n] = DiscardPile[k];
				DiscardPile[k] = temp;
			}

			DrawPile.AddRange(DiscardPile);
			DiscardPile = new List<Card>();

			Console.WriteLine("Player shuffles discard pile into draw pile.");
		}

		public void DrawCards(int amount)
		{
			// Handle not having cards in draw pile. Need to shuffle discard if available, 
			// else set the amount to the amount of cards available in the draw pile.
			if (amount > DrawPile.Count)
			{
				ShuffleDiscardPileIntoDrawPile();

				if (amount > DrawPile.Count)
				{
					amount = DrawPile.Count;
					Util.Error($"Not enough cards after Discard shuffled and added. Will draw {amount}.");
				}
			}

			// Set the start Index to 0 so we can add to the Hand dictionary, 
			// unless we already have a hand, in which case, set it to the last element's key + 1
			var startIndex = Hand.Any() ? Hand.Last().Key + 1: 0;
			for (var i = 0; i < amount; i++)
			{
				Hand.Add(startIndex + i, DrawPile.First());
				DrawPile.RemoveAt(0);
			}
		}

		public void DiscardCards(int amount)
		{
			// TODO, this will need to be a user opt in list for DiscardAndDrawPlayerChoiceAction
			for (var i = 0; i < amount; i++)
			{
				Hand.Remove(i);
			}
			// TODO protect against discarding too many cards; possibly return amount actually discarded
		}

		private void ReorderHand()
		{
			// Reorder the hand
			var tempHand = new Dictionary<int, Card>();
			var currentHand = Hand.Values.ToList();
			for (var i = 0; i < currentHand.Count; i++)
			{
				tempHand.Add(i, currentHand[i]);
			}
			Hand = tempHand;
		}

		public void PlayCard(int cardNumber, GameState gameState)
		{
			Console.WriteLine($"Player at beginning of turn:\n\t{this}");
			Console.WriteLine($"Player {Order} plays {Hand[cardNumber]}.");
			var actions = Hand[cardNumber].Logic;
			DiscardPile.Add(Hand[cardNumber]);
			Hand.Remove(cardNumber);
			ExecuteCardActions(gameState, actions);
			Console.WriteLine($"Player at end of turn:\n\t{this}");
		}

#if (DEBUG)
		internal void PlaySpecificCard(Card card, GameState gameState)
		{
			ExecuteCardActions(gameState, card.Logic);
		}

#endif

		private void ExecuteCardActions(GameState gameState, Dictionary<MethodInfo, object[]> actions)
		{
			foreach (var step in actions)
			{
				// TODO Hack to inject player parameter at runtime
				var methodParameters = step.Key.GetParameters();
				var parameters = new object[methodParameters.Length];
				for (var i = 0; i < methodParameters.Length; i++)
				{
					if (methodParameters[i].Name == "player")
					{
						parameters[i] = Convert.ChangeType(this, methodParameters[i].ParameterType);
					}
					else if (methodParameters[i].Name == "gameState")
					{
						parameters[i] = Convert.ChangeType(gameState, methodParameters[i].ParameterType);
					}
					// In cases where there's a default specifier on the method
					else if (step.Value[i] == null)
					{
						parameters[i] = step.Value[i];
					}
					else
					{
						parameters[i] = Convert.ChangeType(step.Value[i], methodParameters[i].ParameterType);
					}
				}

				step.Key.Invoke(null, parameters);
			}
		}

	}
}