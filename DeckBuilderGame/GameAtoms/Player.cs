using System.Collections.Generic;

namespace DeckBuilderGame.GameAtoms
{
	public class Player
	{
		public string Name;
		public int Order;
		public int ActionsTaken;
		public int ActionMax;

		internal Dictionary<int, ICard> Hand;
		internal List<ICard> DrawPile;
		internal List<ICard> DiscardPile;

		public Player()
		{
			Hand = new Dictionary<int, ICard>();
			DrawPile = new List<ICard>();
			DiscardPile = new List<ICard>();
		}
	}
}