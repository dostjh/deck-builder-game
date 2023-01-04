using DeckBuilderGame.GameAtoms;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DeckBuilderGame.SerializableClasses
{
	[XmlRoot("CardDefinitions")]
	public class CardDefinitionsSerializable
	{
		[XmlElement("CardDefinition")]
		public List<CardDefinitionSerializable> CardDefinitions;
	}

	public class CardDefinitionSerializable
	{
		public string Name;
		public string Description;
		public int Cost;
		public int Value;
		public CardType CardType;
		public List<Step> Logic;
	}
}