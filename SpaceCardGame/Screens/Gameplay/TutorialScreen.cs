using _2DEngine;
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

        private Queue<List<string>> StringSets { get; set; }

        #endregion

        public TutorialScreen(Deck playerChosenDeck, string screenDataAsset = "Screens\\BattleScreen.xml") :
            base(playerChosenDeck, playerChosenDeck, screenDataAsset)
        {
            TurnNumber = 0;

            // We want complete control over what cards our player draws
            Player.CardsToDrawPerTurn = 0;
            Player.OnNewTurn += DrawSpecificCardsForPlayer;
            StringSets = new Queue<List<string>>();

            SetupStringSets();
        }

        /// <summary>
        /// Adds all the strings we will be using in our text dialog boxes to a queue, so we can just pop them off.
        /// Makes manipulating order etc. easier and makes the AddInitialCommands much tidier.
        /// </summary>
        private void SetupStringSets()
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
            StringSets.Enqueue(strings);

            strings = new List<string>()
            {
                "Now that you have your starting cards, lets take a closer look.",
                "On the field already, you can see you and your opponent's STATION.",
                "This is the heart of your fleet and if it is destroyed, you will lose this battle.",
                "Similarly, if you destroy your opponent's station you will win.",
                "Hover the mouse over your station now to see more information about it."
            };
            StringSets.Enqueue(strings);

            strings = new List<string>()
            {
                "You are currently in the 'Card Placement' phase.",
                "During this phase, you can hover the mouse over a card as you have just done to get a better look at it.",
                "The other phase in a turn is the 'Battle' phase.",
                "We will get to that shortly, but during the Battle phase you can do the same thing by holding SHIFT whilst hovering the mouse over the card.",
                "In your hand, by hovering the mouse over each card find the FUEL resource card."
            };
            StringSets.Enqueue(strings);

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
            StringSets.Enqueue(strings);

            strings = new List<string>()
            {
                "Resource cards have no cost, so it is a good idea to lay them as soon as you can.",
                "Do this now for any other resource cards you have.",
            };
            StringSets.Enqueue(strings);

            strings = new List<string>()
            {
                "Now that you have laid some resources, you should be able to build your first ship.",
                "In your hand find the WASP FIGHTER card, select it and click on one of the slots on your side of the board.",
            };
            StringSets.Enqueue(strings);

            strings = new List<string>()
            {
                "Your first ship is deployed, but it will take a turn before it is ready for you to command",
                "When you are ready, press the 'Ready For Battle' button to enter the BATTLE PHASE"
            };
            StringSets.Enqueue(strings);

            strings = new List<string>()
            {
                "This is the Battle Phase",
                "Here you will battle your opponent with the ships you deployed in the Card Placement phase",
                "At any time, you can hover the mouse over a ship and press SHIFT to see it's card, or CTRL to see the ship in all it's full scale glory",
                "The ship we have deployed is not ready yet, so we should end the turn by pressing the 'End Turn' button"
            };
            StringSets.Enqueue(strings);

            strings = new List<string>()
            {
                "It is now the AI's turn",
                "Wait for it to complete it's turn"
            };
            StringSets.Enqueue(strings);

            strings = new List<string>()
            {
                "You have just picked up some more resource cards",
                "If you lay them now we will be able to look at some more card types"
            };
            StringSets.Enqueue(strings);

            strings = new List<string>()
            {
                "Not only can you build ships, you can also improve them with SHIP UPGRADES",
                "Upgrades come in several types, but firstly we will look at WEAPONS",
                "Weapons allow you to upgrade your ship's default weapon system with a custom one instead",
                "They require resources to build, but can offer increased damage or multiple shots",
                "By having multiple shots, your ship can both fire on more than one target and counter attack multiple enemies",
                "Find the VULCAN MISSILE TURRET card in your hand, select it and click anywhere on the screen",
                "You can then select a target to add it to"
            };
            StringSets.Enqueue(strings);

            strings = new List<string>()
            {
                "This weapon actually has an ABILITY",
                "Some cards have passive or active abilities that are either triggered automatically or by the player",
                "The card will appear green if the ability can be used",
                "Once every turn, the vulcan missile turret can fire an extra time if 1 fuel is payed",
                "Activate this ability now by clicking on the turret card next to the ship it is equipped to (it will be GREEN)"
            };
            StringSets.Enqueue(strings);

            strings = new List<string>()
            {
                "Excellent - we will make use of this in the Battle Phase",
                "Now we are going to look at another ship upgrade - SHIELDS",
                "Shields absorb damage that would otherwise be inflicted to the ship they are attached to",
                "They also recharge a certain amount at the start of your turn",
                "Add the PHASE ENERGY SHIELD in the same way as you did the weapon"
            };
            StringSets.Enqueue(strings);

            strings = new List<string>()
            {
                "The final card type is the ABILITY card",
                "These cards work exactly like abilities on other cards except they are stand alone and tend to be more powerful and varied",
                "The ability card in your hand - MISSILE BARRAGE - is extremely useful at clearing your opponent's board of weak ships",
                "Mastering these cards will be key to victory"
            };
            StringSets.Enqueue(strings);

            strings = new List<string>()
            {
                "Lets enter the Battle Phase again to wreak havok with these new upgrades"
            };
            StringSets.Enqueue(strings);

            strings = new List<string>()
            {
                "Click on any ship with a weapon to start an attack order and click on any opponent to complete it",
                "You an always right click to cancel what you are doing",
                "When you are finished with your turn, press the 'End Turn' button to go back to the Main Menu - you have nothing left to learn here"
            };
            StringSets.Enqueue(strings);
        }

        #region Virtual Functions

        /// <summary>
        /// Add our dialog boxes for this tutorial
        /// </summary>
        protected override void AddInitialCommands()
        {
            // Add our opening text info dialog
            AddTextDialogBoxCommand(StringSets.Dequeue()).

            // Run our command for a new game
            NextCommand(new NewTutorialCommand()).

            // Add our text box for explaining the role of the station
            NextCommand(new TextDialogBoxCommand(StringSets.Dequeue())).

            // Wait for the player to hover over the station
            NextCommand(new WaitForConditionCommand(IsStationHoveredOver)).

            // Wait for the player to stop hovering over the station
            NextCommand(new WaitForConditionCommand(IsStationNotHoveredOver)).

            // Add our text box for explaining the battle phases
            NextCommand(new TextDialogBoxCommand(StringSets.Dequeue())).

            NextCommand(new CallbackCommand(ActivateUI)).

            // Wait for the player to hover over the fuel card
            NextCommand(new WaitForConditionCommand(IsFuelCardHovered)).

            // Add our text box for explaining the resourceCards
            NextCommand(new TextDialogBoxCommand(StringSets.Dequeue())).

            // Wait until our fuel resource card has been layed
            NextCommand(new WaitForConditionCommand(IsFuelCardPlaced)).

            // Add our text box for prompting the laying of the other resource cards
            NextCommand(new TextDialogBoxCommand(StringSets.Dequeue())).

            // Wait until all of our resources have been layed
            NextCommand(new WaitForConditionCommand(AllResourceCardsLayed)).

            // Add our text box for ship card information
            NextCommand(new TextDialogBoxCommand(StringSets.Dequeue())).

            // Wait for the wasp fighter to be added
            NextCommand(new WaitForConditionCommand(WaitForWaspFighterToBeAdded)).

            // Add our text box for ending our turn
            NextCommand(new TextDialogBoxCommand(StringSets.Dequeue())).

            // Wait for the Ready For Battle button to be pressed
            NextCommand(new WaitForConditionCommand(WaitForCardPlacementPhaseEnded)).

            // Add our text box for explaining how the battle phase works
            NextCommand(new TextDialogBoxCommand(StringSets.Dequeue())).

            // Wait for the battle phase to be finished
            NextCommand(new WaitForConditionCommand(WaitForBattlePhaseEnded)).

            // Add our text box for explaining the AI turn
            NextCommand(new TextDialogBoxCommand(StringSets.Dequeue())).

            // Wait for the AI turn to be completed
            NextCommand(new WaitForConditionCommand(WaitForAITurnCompleted)).

            NextCommand(new TextDialogBoxCommand(StringSets.Dequeue())).

            // Wait for the second round of resources to be layed
            NextCommand(new WaitForConditionCommand(AllResourceCardsLayed)).

            NextCommand(new TextDialogBoxCommand(StringSets.Dequeue())).

            // Wait for the weapon card to be added to one of our ships
            NextCommand(new WaitForConditionCommand(WaitForWeaponAddedToShip)).

            // Add our text box for explaining active abilities
            NextCommand(new TextDialogBoxCommand(StringSets.Dequeue())).

            // Wait for the vulcan missile turret's card to be activated
            NextCommand(new WaitForConditionCommand(WaitUntilVulcanTurretAbilityActivated)).

            // Add our text box for explaining active abilities
            NextCommand(new TextDialogBoxCommand(StringSets.Dequeue())).

            // Wait for the phase energy shield card to be added to either one of our ships
            NextCommand(new WaitForConditionCommand(WaitForShieldAddedToShip)).

            // Add our text box for explaining ability cards
            NextCommand(new TextDialogBoxCommand(StringSets.Dequeue())).

            // Add our text box for prompting the final battle phase
            NextCommand(new TextDialogBoxCommand(StringSets.Dequeue())).

            NextCommand(new WaitForConditionCommand(WaitForCardPlacementPhaseEnded)).

            // Add our text box for finalising the tutorial
            NextCommand(new TextDialogBoxCommand(StringSets.Dequeue())).

            // Wait for the battle phase to be finished
            NextCommand(new WaitForConditionCommand(WaitForBattlePhaseEnded)).

            // Transition to the main menu after we have pressed the 'End Turn' button
            NextCommand(new CallbackCommand(TransitionToMainMenu));
        }

        /// <summary>
        /// Disable our UI section to begin with.
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            Board.PlayerBoardSection.UIBoardSection.ShouldHandleInput = false;
        }

        #endregion

        #region Callbacks

        /// <summary>
        /// We want to be able to draw specific cards for the player to allow the determinism of our tutorial.
        /// A little nasty, but decides on a turn basis.
        /// </summary>
        /// <param name="newActivePlayer"></param>
        private void DrawSpecificCardsForPlayer(Player newActivePlayer)
        {
            TurnNumber++;

            if (TurnNumber == 1)
            {
                Player.DrawCard("MetalResourceCard");
                Player.DrawCard("CrewResourceCard");
                Player.DrawCard("ElectronicsResourceCard");
            }
            else if (TurnNumber == 2)
            {
                Player.DrawCard("MetalResourceCard");
                Player.DrawCard("FuelResourceCard");
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
        private void ActivateUI()
        {
            Board.PlayerBoardSection.UIBoardSection.ShouldHandleInput = true;
        }

        /// <summary>
        /// We want to wait until our fuel resource is hovered over
        /// </summary>
        /// <returns></returns>
        private bool IsFuelCardHovered()
        {
            return Board.PlayerBoardSection.UIBoardSection.HandUI.FindChild<Card>(x => x is FuelResourceCard).Collider.IsEntered;
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
        /// Waits until we have layed all of our resource cards and none are waiting to be laid
        /// </summary>
        /// <returns></returns>
        private bool AllResourceCardsLayed()
        {
            return !Board.PlayerBoardSection.UIBoardSection.HandUI.Exists(x => x is ResourceCard) && GameMouse.Instance.ChildrenCount == 0;
        }

        /// <summary>
        /// Waits until we have added our wasp fighter card
        /// </summary>
        /// <returns></returns>
        private bool WaitForWaspFighterToBeAdded()
        {
            return Board.PlayerBoardSection.GameBoardSection.ShipCardControl.Exists(x => (x as CardShipPair).ShipCard.GetType() == typeof(WaspFighterShipCard));
        }

        /// <summary>
        /// Wait until we transition to the battle phase
        /// </summary>
        /// <returns></returns>
        private bool WaitForCardPlacementPhaseEnded()
        {
            return TurnState == TurnState.kBattle;
        }

        /// <summary>
        /// Wait until we have finished our turn
        /// </summary>
        /// <returns></returns>
        private bool WaitForBattlePhaseEnded()
        {
            return TurnState == TurnState.kPlaceCards;
        }

        /// <summary>
        /// Wait until the AI has finished it's turn
        /// </summary>
        /// <returns></returns>
        private bool WaitForAITurnCompleted()
        {
            return ActivePlayer == Player;
        }

        /// <summary>
        /// Wait until we have added the Vulcan Missile Turret card to one of our ships.
        /// Have to be a little imaginative with our predicate because all ships have a CardWeaponPair by default for the default turret.
        /// </summary>
        /// <returns></returns>
        private bool WaitForWeaponAddedToShip()
        {
            return Board.PlayerBoardSection.GameBoardSection.ShipCardControl.Exists(x => (x as CardShipPair).FindChild<CardWeaponPair>().WeaponCard.GetType() == typeof(VulcanMissileTurretCard));
        }

        /// <summary>
        /// Wait until we have activated the ability on the Vulcan Missile Turret - we can tell this by one fuel being used up this turn
        /// </summary>
        /// <returns></returns>
        private bool WaitUntilVulcanTurretAbilityActivated()
        {
            return Player.Resources[(int)ResourceType.Fuel].FindAll(x => x.Used == true).Count == 1;
        }

        /// <summary>
        /// Wait until we have added the Phase Energy Shield card to one of our ships
        /// </summary>
        /// <returns></returns>
        private bool WaitForShieldAddedToShip()
        {
            return Board.PlayerBoardSection.GameBoardSection.ShipCardControl.Exists(x => (x as CardShipPair).FindChild<CardShieldPair>() != null);
        }

        /// <summary>
        /// After this tutorial is completed we transition back to the main menu
        /// </summary>
        /// <returns></returns>
        private void TransitionToMainMenu()
        {
            Transition(new MainMenuScreen());
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