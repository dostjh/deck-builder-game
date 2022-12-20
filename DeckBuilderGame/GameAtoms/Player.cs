﻿using System;
using System.Collections.Generic;
using System.Linq;

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

		internal Dictionary<int, Card> Hand { get; set; }
		internal List<Card> DrawPile;
		internal List<Card> DiscardPile { get; set; }

		internal Player(int order, List<Card> drawPile)
		{
			Order = order;
			Hand = new Dictionary<int, Card>();
			DiscardPile = new List<Card>();

			var tempDrawPile = new Card[drawPile.Count()];
			drawPile.CopyTo(tempDrawPile);
			DrawPile = tempDrawPile.ToList();

			Shuffle();
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

		public void Shuffle()
		{
			Console.WriteLine("Player shuffles.");
		}

		public void DrawCards(int amount)
		{
			// TODO, handle not having cards in draw pile. Need to shuffle discard if available, else continue.

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
					else if (methodParameters[i].Name == "gamestate")
					{
						parameters[i] = Convert.ChangeType(gameState, methodParameters[i].ParameterType);
					}
					else
					{
						parameters[i] = Convert.ChangeType(step.Value[i], methodParameters[i].ParameterType);
					}
				}

				step.Key.Invoke(null, parameters);
			}
			Console.WriteLine($"Player at end of turn:\n\t{this}");
		}
	}
}