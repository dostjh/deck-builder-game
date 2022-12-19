using DeckBuilderGame.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DeckBuilderGame.GameAtoms
{
	class Card
	{
		public string Name;
		public string Description;
		public CardType Type;
		public int Cost;
		public int Value;
		public Dictionary<MethodInfo, object[]> Logic;

		public Card(string name, string description, CardType cardType, int cost, int value)
		{
			Name = name;
			Description = description;
			Type = cardType;
			Cost = cost;
			Value = value;
		}

		public Card(CardSerializable card)
		{
			Name = card.Name;
			Description = card.Description;
			Type = card.CardType;
			Cost = card.Cost;
			Value = card.Value;
			Logic = new Dictionary<MethodInfo, object[]>();
			foreach (var action in card.Logic)
			{
				var methodInfo = typeof(Actions).GetMethod(action.Name);
				var parameterDict = action.Parameters.ToDictionary(p => p.Name, p => p.Value);
				var parameters = methodInfo.GetParameters().Select(p => Convert.ChangeType(parameterDict[p.Name], p.ParameterType)).ToArray();
				Logic.Add(methodInfo, parameters);
			}
		}

		public void Play()
		{
			foreach (var step in Logic)
			{
				step.Key.Invoke(typeof(Actions), step.Value);
			}
		}

		public void WriteToConsole()
		{
			Console.WriteLine("=========================");
			Console.WriteLine(Name);
			Console.WriteLine(Description);
			switch (Type)
			{
				case CardType.Action:
					Console.ForegroundColor = ConsoleColor.Blue;
					break;
				case CardType.Currency:
					Console.ForegroundColor = ConsoleColor.Yellow;
					break;
				case CardType.Victory:
					Console.ForegroundColor = ConsoleColor.Green;
					break;
			}
			Console.WriteLine($"Type: {Type}");
			Console.ResetColor();
			Console.WriteLine($"Cost: {Cost} | Value: {Value}");
			Console.WriteLine("=========================");
		}
	}

	static class Actions
	{
		static public void IncrementActionMax(int amount)
		{
			Console.WriteLine($"Would have incremented action max by {amount}");
		}

		static public void DrawCards(int amount)
		{
			Console.WriteLine($"Would have drawn {amount} cards.");
		}
	}

	public enum CardType
	{
		Action,
		Victory,
		Currency
	}
}
