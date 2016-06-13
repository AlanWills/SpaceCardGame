using _2DEngine;
using Microsoft.Xna.Framework;

namespace SpaceCardGame
{
    /// <summary>
    /// An enum we will use to set the active object
    /// </summary>
    public enum CardOrObject
    {
        kCard,
        kObject
    }

    /// <summary>
    /// Some cards will be replaced by an object during the battle phase.
    /// This class handles which one is present based on the phase of the turn.
    /// </summary>
    public abstract class CardObjectPair : GameObject, ICardObjectElement
    {
        #region Properties and Fields

        /// <summary>
        /// The player who owns this card object pair
        /// </summary>
        public Player Player { get; private set; }

        /// <summary>
        /// A reference to the card part of our pair
        /// </summary>
        public Card Card { get; protected set; }

        /// <summary>
        /// A reference to the object part of our pair
        /// </summary>
        public GameObject CardObject { get; protected set; }

        /// <summary>
        /// Some cards need to wait one turn before they can be interacted with (i.e. ships need to wait a turn before they can attack).
        /// This bool property indicates whether this condition has been satisfied.
        /// </summary>
        public Property<bool> IsReady { get; protected set; }

        /// <summary>
        /// A flag to indicate whether we should create a hover info module.
        /// Some cards may not make use of it (i.e. resource card or default turret).
        /// </summary>
        protected bool AddHoverInfoModule { get; set; }

        /// <summary>
        /// The hover info module for this CardObjectPair.
        /// </summary>
        protected CardHoverInfoModule HoverInfoModule { get; set; }

        #endregion

        public CardObjectPair(Card card) :
            base(Vector2.Zero, AssetManager.EmptyGameObjectDataAsset)
        {
            Card = card;
            Card.LocalPosition = Vector2.Zero;
            Card.Flip(CardFlipState.kFaceUp);

            UsesCollider = false;
            AddHoverInfoModule = true;

            IsReady = new Property<bool>(false);

            // We get the player who owns this card by using the active player - this card is created on the owning player's turn
            BattleScreen battleScreen = ScreenManager.Instance.CurrentScreen as BattleScreen;
            Player = battleScreen.ActivePlayer;
        }

        #region Virtual Functions

        /// <summary>
        /// Add our HoverCardInfo module to the card object pair so that it appears in both the card place stage and the battle stage.
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            // Check to see whether our HoverInfoModule has been created already - some derived classes create their own custom info image.
            if (AddHoverInfoModule && HoverInfoModule == null)
            {
                HoverInfoModule = AddModule(new CardHoverInfoModule(this));
            }

            base.LoadContent();
        }

        /// <summary>
        /// Just checks we have set up references correctly and sets the active object to the card
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            if (Card.Parent != null)
            {
                // Add the card as a child HERE so we do not call LoadContent or Initialise on it - this will be done right at the start of the BattleScreen in the DeckInstance
                Card.ReparentTo(this);
            }
            else
            {
                // If the card had no parent (Station) then we just add it normally
                AddChild(Card);
            }


            // We should have set these references by now
            DebugUtils.AssertNotNull(Card);
            DebugUtils.AssertNotNull(CardObject);

            Card.Colour.Connect(Colour);
            CardObject.Colour.Connect(Colour);
        }

        /// <summary>
        /// An abstract function used to perform custom fixup when adding to the game board.
        /// Does not trigger any card behaviours, but is more used for shuffling objects around the scene and fixing up sizes.
        /// </summary>
        /// <param name="gameBoard"></param>
        /// <param name="player"></param>
        public abstract void WhenAddedToGameBoard(GameBoardSection gameBoard);

        /// <summary>
        /// A function called when this card is added to a card ship pair.
        /// This occurs with abilities, weapons etc.
        /// Allows us to apply custom effects based on the card type.
        /// </summary>
        /// <param name="cardShipPair"></param>
        public abstract void AddToCardShipPair(CardShipPair cardShipPair);

        /// <summary>
        /// A function we will call when the game turn state changes to placing cards.
        /// This makes the card the active object.
        /// Can override for custom cards to change what they do when we change turn state.
        /// </summary>
        public virtual void OnTurnBegin()
        {
            foreach (BaseObject child in Children)
            {
                if (child is ICardObjectElement)
                {
                    (child as ICardObjectElement).OnTurnBegin();
                }
            }
        }

        /// <summary>
        /// A function we will call when the game turn state changes to the battle phase.
        /// This makes the object the active object.
        /// Can override for custom cards to change what they do when we change turn state.
        /// </summary>
        public virtual void MakeReadyForBattle()
        {
            foreach (BaseObject child in Children)
            {
                if (child is ICardObjectElement)
                {
                    (child as ICardObjectElement).MakeReadyForBattle();
                }
            }
        }

        /// <summary>
        /// A function we will call when our turn ends, but before the next player's turn begins.
        /// Updates the IsReady flag to be true.
        /// </summary>
        public virtual void OnTurnEnd()
        {
            IsReady.Value = true;

            foreach (BaseObject child in Children)
            {
                if (child is ICardObjectElement)
                {
                    (child as ICardObjectElement).OnTurnEnd();
                }
            }
        }

        #endregion
    }
}
