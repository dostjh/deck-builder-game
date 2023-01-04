using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeckBuilderGame.GameAtoms
{
	static public class Actions
	{
		static public void IncrementActionMaxAction(int amount, Player player)
		{
			player.ActionMax += amount;
		}

		static public void DrawCardsAction(int amount, Player player)
		{
			player.DrawCards(amount);
		}

		static public void IncrementBuyMaxAction(int amount, Player player)
		{
			player.BuyMax += amount;
		}

		static public void IncrementCurrencyAction(int amount, Player player)
		{
			player.CurrencyCount += amount;
		}

		public static void AddCardFromPoolAction(int maxCost, Player player, GameState gameState, CardType cardType = CardType.Default)
		{
			var successfulDraw = false;
			var userSelection = string.Empty;
			while (!successfulDraw)
			{
				var options = gameState.GetDrawableCardPool(maxCost);
				userSelection = Util.GetUserInputOption("Which card do you want to draw?", options.Select(o => o.Name));
				successfulDraw = gameState.DrawCardFromPool(userSelection);
			}

			player.DiscardPile.Add(gameState.CardDefinitions[userSelection]);
		}

		static public void AddCardFromPoolWithCostBasedMaxAction(int costBasedMax, Player player, GameState gameState, CardType cardType = CardType.Default)
		{
			throw new NotImplementedException();
		}

		static public void ForceOpponentDiscardAction(int amount, GameState gameState)
		{
			throw new NotImplementedException();
			// to do, will need to see if other players have option to ActionReaction (e.g. Moat)
		}

		static public void TrashCardAction(int amount, Player player, GameState gameState, CardType cardType = CardType.Default)
		{
			throw new NotImplementedException();
		}

		static public void DiscardAndDrawPlayerChoiceAction(Player player)
		{
			// TODO, better prompt
			// TODO Need to present list of cards to discard and allow user selection of arbitrary number
			var amount = Util.GetUserInputInt("How many cards would you like to discard?");
			player.DiscardCards(amount);
			player.DrawCards(amount);
		}
	}
}
