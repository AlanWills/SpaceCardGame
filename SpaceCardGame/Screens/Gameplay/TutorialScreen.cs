using _2DEngine;
using CardGameEngine;
using System.Collections.Generic;

namespace SpaceCardGame
{
    /// <summary>
    /// A battle screen which is specifically designed as a tutorial.
    /// Implements commands and UI to teach the player about the game.
    /// </summary>
    public class TutorialScreen : BattleScreen
    {
        #region Properties and Fields

        /// <summary>
        /// We want to perform specific things based on what turn it is.
        /// </summary>
        private int TurnNumber { get; set; }

        #endregion

        public TutorialScreen(Deck playerChosenDeck, string screenDataAsset = "Screens\\BattleScreen.xml") :
            base(playerChosenDeck, screenDataAsset)
        {
            TurnNumber = 0;

            // We want complete control over what cards our player draws
            Player.CardsToDrawPerTurn = 0;
            Player.OnNewTurn += DrawSpecificCardsForPlayer;
        }

        #region Virtual Functions

        /// <summary>
        /// Add our dialog boxes for this tutorial
        /// </summary>
        protected override void AddInitialCommands()
        {
            List<string> strings = new List<string>()
            {
                "Welcome to the tutorial.",
                "Here you will learn the basics of the game by playing against a simple AI.",
                "At the start of every game, your hand will have five cards.",
                "It can hold a maximum of ten - if your hand is full, you will immediately discard any more you draw.",
                "At the start of your turn, you will draw three cards from your deck.",
                "Lets start with your turn..."
            };

            // Add our opening text info dialog
            TextDialogBoxCommand openingTextDialog = AddTextDialogBoxCommand(strings);

            // Run our command for a new game
            NewTutorialCommand newGameCommand = AddCommand(new NewTutorialCommand(), openingTextDialog);

            strings = new List<string>()
            {
                "Now that you have your starting cards, lets take a closer look.",
                "On the field already, you can see you and your opponent's STATION.",
                "This is the heart of your fleet and if it is destroyed, you will lose this battle.",
                "Similarly, if you destroy your opponent's station you will win.",
                "Hover the mouse over your station now to see more information about it."
            };

            // Add our text box for explaining the role of the station
            TextDialogBoxCommand stationExplanationTextDialog = AddTextDialogBoxCommand(strings, newGameCommand);

            // Wait for the player to hover over the station
            WaitForConditionCommand waitForStationHovered = AddCommand(new WaitForConditionCommand(IsStationHoveredOver), stationExplanationTextDialog);

            // Wait for the player to stop hovering over the station
            WaitForConditionCommand waitForStationNotHovered = AddCommand(new WaitForConditionCommand(IsStationNotHoveredOver), waitForStationHovered);

            strings = new List<string>()
            {
                "You are currently in the 'Card Placement' phase.",
                "During this phase, you can hover the mouse over a card as you have just done to get a better look at it.",
                "The other phase in a turn is the 'Battle' phase.",
                "We will get to that shortly, but during the Battle phase you can do the same thing by holding SHIFT whilst hovering the mouse over the card.",
                "In your hand, by hovering the mouse over each card find the FUEL resource card."
            };

            // Add our text box for explaining the battle phases
            TextDialogBoxCommand phasesExplanationTextDialog = AddTextDialogBoxCommand(strings, waitForStationNotHovered);
            phasesExplanationTextDialog.OnDeathCallback += ActivateUI;

            // Wait for the player to hover over the fuel card
            WaitForConditionCommand waitForFuelCardHovered = AddCommand(new WaitForConditionCommand(IsFuelCardHovered), phasesExplanationTextDialog);

            strings = new List<string>()
            {
                "Resources are what you use to lay cards and come in four types:",
                "CREW",
                "ELECTRONICS",
                "FUEL",
                "METAL",
                "Every card has an associated resource cost at the bottom.",
                "When the card is layed, the resources will be deducted accordingly.",
                "At the beginning of your turn, all of your resources become available to use again.",
                "Cards have an outline around them indicating whether they can be layed - GREEN means they can and WHITE means they cannot.",
                "This usually depends on whether you have enough resources, but some cards also require other conditions too.",
                "Now, click on the fuel resource card and click on your board to lay it."
            };

            // Add our text box for explaining the resourceCards
            TextDialogBoxCommand resourceCardExplanationTextDialog = AddTextDialogBoxCommand(strings, waitForFuelCardHovered);

            // Wait until our fuel resource card has been layed
            WaitForConditionCommand waitForFuelLayed = AddCommand(new WaitForConditionCommand(IsFuelCardPlaced), resourceCardExplanationTextDialog);

            strings = new List<string>()
            {
                "Resource cards have no cost, so it is a good idea to lay them as soon as you can.",
                "Do this now for any other resource cards you have.",
            };

            // Add our text box for prompting the laying of the other resource cards
            TextDialogBoxCommand layOtherResourcesTextDialog = AddTextDialogBoxCommand(strings, waitForFuelLayed);

            // Wait until all of our resources have been layed
            WaitForConditionCommand waitForAllResources = AddCommand(new WaitForConditionCommand(AllResourceCardsLayed), layOtherResourcesTextDialog);

            strings = new List<string>()
            {
                "Now that you have laid some resources, you should be able to build your first ship.",
                "In your hand find the WASP FIGHTER card, select it and click on one of the slots on your side of the board.",
            };

            // Add our text box for ship card information
            TextDialogBoxCommand shipCardExplanationTextDialog = AddTextDialogBoxCommand(strings, waitForAllResources);

            // Wait for the wasp fighter to be added
            WaitForConditionCommand waitForWaspFighterToBeAdded = AddCommand(new WaitForConditionCommand(WaitForWaspFighterToBeAdded), shipCardExplanationTextDialog);
        }

        /// <summary>
        /// Disable our UI section to begin with.
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            Board.PlayerBoardSection.UIBoardSection.ShouldHandleInput.Value = false;
        }

        #endregion

        #region Callbacks

        /// <summary>
        /// We want to be able to draw specific cards for the player to allow the determinism of our tutorial.
        /// A little nasty, but decides on a turn basis.
        /// </summary>
        /// <param name="newActivePlayer"></param>
        private void DrawSpecificCardsForPlayer(GamePlayer newActivePlayer)
        {
            TurnNumber++;

            if (TurnNumber == 1)
            {
                Player.DrawCard("MetalResourceCard");
                Player.DrawCard("CrewResourceCard");
                Player.DrawCard("ElectronicsResourceCard");
            }
        }

        /// <summary>
        /// We want to wait until our station is hovered over
        /// </summary>
        /// <returns></returns>
        private bool IsStationHoveredOver()
        {
            return Board.PlayerBoardSection.GameBoardSection.ShipCardControl.FindChild<CardStationPair>().Card.Collider.IsEntered;
        }

        /// <summary>
        /// We want to wait until our station is no longer hovered over;
        /// </summary>
        /// <returns></returns>
        private bool IsStationNotHoveredOver()
        {
            return Board.PlayerBoardSection.GameBoardSection.ShipCardControl.FindChild<CardStationPair>().Card.Collider.IsExited;
        }

        /// <summary>
        /// Now makes the player's game board section available for input.
        /// </summary>
        private void ActivateUI(Command command)
        {
            Board.PlayerBoardSection.UIBoardSection.ShouldHandleInput.Value = true;
        }

        /// <summary>
        /// We want to wait until our fuel resource is hovered over
        /// </summary>
        /// <returns></returns>
        private bool IsFuelCardHovered()
        {
            return Board.PlayerBoardSection.UIBoardSection.HandUI.FindChild<BaseUICard>(x => (x as BaseUICard).CardData.Type == "Resource").Collider.IsEntered;
        }

        /// <summary>
        /// We want to wait until our fuel resource is placed
        /// </summary>
        /// <returns></returns>
        private bool IsFuelCardPlaced()
        {
            return Player.Resources[(int)ResourceType.Fuel].Count > 0;
        }

        /// <summary>
        /// Waits until we have layed all of our resource cards
        /// </summary>
        /// <returns></returns>
        private bool AllResourceCardsLayed()
        {
            return !Board.PlayerBoardSection.UIBoardSection.HandUI.Exists(x => (x as BaseUICard).CardData.Type == "Resource");
        }

        /// <summary>
        /// Waits until we have added our wasp fighter card
        /// </summary>
        /// <returns></returns>
        private bool WaitForWaspFighterToBeAdded()
        {
            return Board.PlayerBoardSection.GameBoardSection.ShipCardControl.Exists(x => (x as CardShipPair).ShipCard.ShipCardData.CardTypeName == "WaspFighterCard");
        }

        #endregion

        #region Useful Command Functions

        /// <summary>
        /// A wrapper function for adding a text dialog box - just speeds things up a bit
        /// </summary>
        /// <param name="strings"></param>
        /// <param name="previousCommand"></param>
        /// <param name="shouldPauseGame"></param>
        /// <returns></returns>
        private TextDialogBoxCommand AddTextDialogBoxCommand(List<string> strings, Command previousCommand = null, bool shouldPauseGame = true)
        {
            return AddCommand(new TextDialogBoxCommand(strings, shouldPauseGame), previousCommand);
        }

        #endregion
    }
}