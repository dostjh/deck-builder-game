using DeckBuilderGame.Cards;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

			SetLogic(card);
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

		public override bool Equals(object obj)
		{
			var result = false;
			
			var tempObject = (Card)obj;
			if (tempObject != null)
			{
				result = Name == tempObject.Name;
			}

			return result;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Name);
		}

		void SetLogic(CardSerializable card)
		{
			Logic = new Dictionary<MethodInfo, object[]>();
			foreach (var action in card.Logic)
			{
				var methodInfo = typeof(Actions).GetMethod(action.Name);
				var parameterDict = action.Parameters.ToDictionary(p => p.Name, p => p.Value);
				var methodInfoParameters = methodInfo.GetParameters();
				var logicParameters = new object[methodInfoParameters.Length];
				for (var i = 0; i < methodInfoParameters.Length; i++)
				{
					var methodInfoParameter = methodInfoParameters[i];


					if (parameterDict.ContainsKey(methodInfoParameter.Name))
					{
						// If we've got the parameter in our dictionary from the XML file, then 
						// load that value in.
						var parameterDictValue = parameterDict[methodInfoParameter.Name];
						if (methodInfoParameter.ParameterType == typeof(CardType))
						{
							// Handle CardType parameter specially
							var cardTypeValue = Enum.Parse(typeof(CardType), parameterDictValue);
							logicParameters[i] = Convert.ChangeType(cardTypeValue, methodInfoParameter.ParameterType);
						}
						else
						{
							logicParameters[i] = Convert.ChangeType(parameterDictValue, methodInfoParameter.ParameterType);
						}
					}
					else
					{
						// Otherwise, let's set the value to null and we'll set it at runtime (This is for player, gameState, etc.)
						logicParameters[i] = null;
					}
				}

				Logic.Add(methodInfo, logicParameters);
			}
		}
	}

	public enum CardType
	{
		[Description("Action")]
		Action,
		[Description("Action - Attack")]
		ActionAttack,
		[Description("Action - Reaction")]
		ActionReaction,
		[Description("Victory")]
		Victory,
		[Description("Currency")]
		Currency,
		[Description("None")]
		Default
	}
}
