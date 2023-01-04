using System.Collections.Generic;
using System.Xml.Serialization;

namespace DeckBuilderGame.SerializableClasses
{
	[XmlRoot("Games")]
	public class GamesDataSerializable
	{
		[XmlElement("Game")]
		public List<GameDataSerializable> Games;
	}
	
	
	public class GameDataSerializable
	{
		public string Name;
		[XmlElement("Card")]
		public List<CardSerializable> Cards;
		public Rules Rules;
	}

	public class Rules
	{
		public int MaxAction;
		public int MaxBuy;
		public int DrawCount;
	}

	public class CardSerializable
	{
		public string Name;
		public int Pool;
		public bool NoLimit;
	}

	public class Step
	{
		[XmlAttribute]
		public string Name;
		public List<Parameter> Parameters;
	}

	public class Parameter
	{
		[XmlAttribute]
		public string Name;
		[XmlAttribute]
		public string Value;
	}
}