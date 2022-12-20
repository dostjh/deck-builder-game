using DeckBuilderGame.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static DeckBuilderGame.GameAtoms.Player;

namespace DeckBuilderGame.GameAtoms
{
	internal class Card
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

		public void Play(Player player)
		{
			foreach (var step in Logic)
			{
				// TODO Hack to inject player parameter at runtime
				var methodParameters = step.Key.GetParameters();
				var parameters = new object[methodParameters.Length];
				for (var i = 0; i < methodParameters.Length; i++)
				{
					if (methodParameters[i].Name == "player")
					{
						parameters[i] = Convert.ChangeType(player, typeof(Player));
					}
					else
					{
						parameters[i] = Convert.ChangeType(step.Value[i], step.Value[i].GetType());
					}
				}
				
				step.Key.Invoke(typeof(Actions), parameters);
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

		public override string ToString()
		{
			return $"Type:{Type.ToString().ToUpper()}, Name:{Name},{(Description != null ? $" Description:{Description}," : "")} Cost:{Cost}, Value:{Value}";
		}

		
	}

	public enum CardType
	{
		Action,
		Victory,
		Currency
	}
}
