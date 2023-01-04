using DeckBuilderGame.GameAtoms;
using System.Collections.Generic;
using Xunit;

namespace DeckBuilderGameXUnitTests.GameAtomsTests
{
	public class ActionsTests
	{
		[Fact]
		public void IncrementActionMaxAction_IsIncremented()
		{
			var incrementAmount = 1;
			var player = new Player
			{
				ActionMax = 1
			};

			Actions.IncrementActionMaxAction(incrementAmount, player);

			Assert.True(player.ActionMax == 2, "Player.ActionMax should be 2");
		}

		[Fact]
		public void DrawCardsAction_CardsDrawn()
		{
			var drawAmount = 1;
			var player = new Player()
			{
				DrawPile = new List<Card>() { new Card(), new Card(), new Card() },
				Hand = new Dictionary<int, Card>()
			};

			Actions.DrawCardsAction(drawAmount, player);

			Assert.True(player.Hand.Count == 1, "Player.Hand.Count should be 1");
		}

		[Fact]
		public void DrawCardsAction_CardsDrawnWhenDrawEmpties()
		{
			var drawAmount = 1;
			var player = new Player
			{
				DrawPile = new List<Card>(),
				DiscardPile = new List<Card>() { new Card(), new Card(), new Card() },
				Hand = new Dictionary<int, Card>()
			};

			Actions.DrawCardsAction(drawAmount, player);

			Assert.True(player.Hand.Count == 1 && player.DrawPile.Count == 2 && player.DiscardPile.Count == 0, "Player.Hand.Count should be 1; Player.DrawPile.Count should 2; Player.DiscardPile.Count should be 0");
		}
	}
}
