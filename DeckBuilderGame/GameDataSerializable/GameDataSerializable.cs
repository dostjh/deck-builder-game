using DeckBuilderGame.GameAtoms;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DeckBuilderGame.Cards
{

	[Serializable]
	[XmlRoot("Game")]
	public class GameDataSerializable
	{
		[XmlArrayItem("Card")]
		public List<CardSerializable> Cards;
		public Rules Rules;
	}

	[Serializable]
	public class Rules
	{
		public int MaxAction;
		public int DrawCount;
	}

	[Serializable]
	[XmlRoot("Card")]
	public class CardSerializable
	{
		public string Name;
		public string Description;
		public int Cost;
		public int Value;
		public CardType CardType;
		public List<Step> Logic;
	}

	[Serializable]
	public class Step
	{
		[XmlAttribute]
		public string Name;
		public List<Parameter> Parameters;
	}

	[Serializable]
	public class Parameter
	{
		[XmlAttribute]
		public string Name;
		[XmlAttribute]
		public string Value;
	}
}
