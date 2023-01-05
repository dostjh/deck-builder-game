# Fully configurable Dominion-style deck building games for CLI
## Description
This is a personal project to build a fully configurable deck building card game engine based on the mechanics of the game Dominion. Users of the project will be able to specify game rules, victory conditions, card properties, and card effects via XML configuration files. The engine will process this configuration and allow users to play the specified game. (Out of the box, the game is configured to run the Dominion introductory game.)

> **Note**  
> This is a work in progress and the following features are not yet implemented:
> * Games are not playable as such. (Early development focused on getting the data model and reflection down.)
> * Several actions will throw NotImplementedException()
> * Several tests have not been implemented
> * Multiplayer support has not been implemented
> * It's not pretty... yet.

## Getting started
TODO

## Configuring games
Games are configured in `config\Games.xml`. Each game should specify a game name, max number of players, and default values for max buy, max action, and max draw per turn. The game should also specify the cards used in the game's common pool as well as how many cards for each. If a card is unlimited, you can instead specify `<NoLimit>true</NoLimit>`. Listed cards need to be configured in `config\CardLibrary.xml`.

Here is a simplified game definition.

```xml
<Game>
  <Name>Dominion - Base</Name>
  <Rules>
    <MaxAction>1</MaxAction>
    <MaxBuy>1</MaxBuy>
    <TurnDraw>3</TurnDraw>
  </Rules>
  <Cards>
    <Card>
      <Name>Village</Name>
      <Pool>10</Pool>
    </Card>
    <Card>
      <Name>Gold</Name>
      <NoLimit>true</NoLimit>
    </Card>
  </Cards>
</Game>
```

## Configuring cards
Cards are specified in `config\CardLibrary.xml` separately from the game configurations to allow cards to be specified for several games without duplicating them. Cards define the following values:
* Name
* Description
* Cost
* Value (for victory cards)
* CardType (Action, Victory, Currency)
* Logic for the cards actions in the order they should be applied

When specifying the logic for the card, you should include the step name as the method name to be called from `DeckBuilderGame.GameAtoms.Actions.cs`. You must specify all parameters the method specifies with the following exceptions:
* Target player (This is injected at runtime)
* Game state (This is injected at runtime)
* Parameters that have defaults specified (The default will be passed as null at runtime)

Here is an example card configuration.

```xml
<CardDefinition>
  <Name>Village</Name>
  <Description>+1 Action; +2 Cards</Description>
  <Cost>3</Cost>
  <Value>0</Value>
  <CardType>Action</CardType>
  <Logic>
    <Step Name="IncrementActionMaxAction">
      <Parameters>
        <Parameter Name="amount" Value="1"/>
      </Parameters>
    </Step>
    <Step Name="DrawCardsAction">
      <Parameters>
        <Parameter Name="amount" Value="2"/>
      </Parameters>
    </Step>
  </Logic>
</CardDefinition>
```

## Writing new actions
Actions are written in C# in `DeckBuilderGame.GameAtoms.Actions.cs`. Some things to keep in mind:
* Actions can specify the target player (`player`) and the game state (`gameState`) as parameters without further modification of any additional classes. These do not need to specified in the configuration files. These are injected at runtime when the method is called.
* When defaults are specified in the method declaration, these do not need to be specified in the configuration files. These will be passed to the method as null and receive the default value.

Here is an example action that uses both the player and the game state parameters as well as a default.
```csharp
public static void AddCardFromPoolAction(int maxCost, Player player, GameState gameState, CardTypecardType = CardType.Default)
{
	var successfulDraw = false;
	var userSelection = string.Empty;
	while (!successfulDraw)
	{
		var options = gameState.GetDrawableCardPool(maxCost);
		userSelection = Util.GetUserInputOption("Which card do you want to draw?", options.Select(o=>o.Name));
		successfulDraw = gameState.DrawCardFromPool(userSelection);
	}

	player.DiscardPile.Add(gameState.CardDefinitions[userSelection]);
}
```

## Frameworks
* .NET Core
* xUnit

## Inspiration
My inspiration for this project was taking my own advice often proffered to aspiring video game developers--find a physical game you enjoy playing and make it into a video game. This is a useful exercise since the incredibly difficult part of finding the fun and creating the mechanics is already accomplished, allowing the aspiring developer to focus solely on the implementation. I'm a history major by training, and have only learned to program opportunistically. Rarely do I get a chance to build something complex from scratch.

## Goals
As a personal project, I hope to learn or improve skills in the following areas:
* (Represented) Git source control
* (Represented) Reflection
* (Represented) Serialization
* (Represented) Unit testing implementation
* (Pending) Multiplayer networking