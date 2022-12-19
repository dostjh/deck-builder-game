using System;
using System.Collections.Generic;
using System.Linq;

namespace DeckBuilderGame.GameAtoms
{
	public class Player
	{
		public string Name;
		public int Order;
		public int ActionsTaken;
		public int ActionMax;

		internal Dictionary<int, Card> Hand { get; set; }

		private List<Card> _drawPile;

		internal List<Card> GetDrawPile()
		{
			return _drawPile;
		}

		void SetDrawPile(List<Card> value)
		{
			_drawPile = value;
		}

		internal List<Card> DiscardPile { get; set; }

		internal Player(int order, List<Card> drawPile)
		{
			Order = order;
			Hand = new Dictionary<int, Card>();
			SetDrawPile(drawPile);
			DiscardPile = new List<Card>();

			Shuffle();
		}

		public void StartTurn()
		{
			// TODO Actually draw cards from draw pile
			Hand.Add(0, _drawPile.First());
			_drawPile.RemoveAt(0);
			Console.WriteLine("Player starts turn.");
		}

		public void EndTurn()
		{
			// Reorder the hand
			var tempHand = new Dictionary<int, Card>();
			var currentHand = Hand.Values.ToList();
			for(var i = 0; i < currentHand.Count; i++)
			{
				tempHand.Add(i, currentHand[i]);
			}
			Hand = tempHand;

			Console.WriteLine("Player ends turn.");
		}

		public void Shuffle()
		{
			Console.WriteLine("Player shuffles.");
		}

		public void PlayCard(int cardNumber)
		{
			Hand[cardNumber].Play();
			DiscardPile.Add(Hand[cardNumber]);
			Hand.Remove(cardNumber);
		}
	}
}