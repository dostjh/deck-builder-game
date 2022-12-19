using DeckBuilderGame.GameAtoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeckBuilderGame
{
	class GameState
	{
		public List<Player> Players;
		public List<ICard> Trash;

		public GameState()
		{

		}

		public GameState(int playerCount)
		{
			Players = new List<Player>();
			for (var i = 0; i < playerCount; i++)
			{
				Players.Add(new Player());
			}
		}
	}
}
