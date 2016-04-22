using _2DEngine;
using CardGameEngine;
using Microsoft.Xna.Framework;
using System.Diagnostics;

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
    public abstract class CardObjectPair : GameObject
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the card part of our pair
        /// </summary>
        public GameCard Card { get; protected set; }

        /// <summary>
        /// A reference to the object part of our pair
        /// </summary>
        public GameObject CardObject { get; protected set; }

        /// <summary>
        /// Some cards need to wait one turn before they can be interacted with (i.e. ships need to wait a turn before they can attack).
        /// This bool property indicates whether this condition has been satisfied.
        /// </summary>
        public Property<bool> IsReady { get; private set; }

        #endregion

        public CardObjectPair(GameCardData cardData) :
            base(Vector2.Zero, AssetManager.EmptyGameObjectDataAsset)
        {
            Card = AddChild(cardData.CreateCard());

            UsesCollider = false;

            IsReady = new Property<bool>(false);
        }

        #region Virtual Functions

        /// <summary>
        /// Just checks we have set up references correctly and sets the active object to the card
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            // We should have set these references by now
            DebugUtils.AssertNotNull(Card);
            DebugUtils.AssertNotNull(CardObject);

            SetActiveObject(CardOrObject.kCard);
        }

        /// <summary>
        /// An abstract function used to perform custom setup when adding to the game board
        /// </summary>
        /// <param name="gameBoard"></param>
        /// <param name="player"></param>
        public abstract void WhenAddedToGameBoard(GameBoardSection gameBoard, GamePlayer player);

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
        public virtual void MakeReadyForCardPlacement()
        {
            SetActiveObject(CardOrObject.kCard);

            // Cards will be placed after this function is called, meaning that when this function is called on this instance, it will have been a turn since it was laid.
            IsReady.Value = true;
        }

        /// <summary>
        /// A function we will call when the game turn state changes to the battle phase.
        /// This makes the object the active object.
        /// Can override for custom cards to change what they do when we change turn state.
        /// </summary>
        public virtual void MakeReadyForBattle()
        {
            SetActiveObject(CardOrObject.kObject);
        }

        /// <summary>
        /// A function we will call when our turn ends, but before the next player's turn begins.
        /// Updates the IsReady flag to be true.
        /// </summary>
        public virtual void OnTurnEnd()
        {
            IsReady.Value = true;
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// A function used to switch the active object that this pair represents.
        /// Only one can be active at any one time.
        /// </summary>
        /// <param name="cardOrObject"></param>
        private void SetActiveObject(CardOrObject cardOrObject)
        {
            switch (cardOrObject)
            {
                case CardOrObject.kCard:
                    {
                        Card.Show();
                        CardObject.Hide();
                        break;
                    }

                case CardOrObject.kObject:
                    {
                        Card.Hide();
                        CardObject.Show();
                        break;
                    }

                default:
                    {
                        Debug.Fail("");
                        break;
                    }
            }
        }

        #endregion
    }
}
