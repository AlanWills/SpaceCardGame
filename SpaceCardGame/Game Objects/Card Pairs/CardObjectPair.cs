using _2DEngine;
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
        public GameCard Card { get; private set; }

        /// <summary>
        /// A reference to the object part of our pair
        /// </summary>
        public GameObject CardObject { get; private set; }

        #endregion

        public CardObjectPair(GameCard gameCard, GameObject cardObject) :
            base(gameCard.WorldPosition, AssetManager.EmptyGameObjectDataAsset)
        {
            Card = gameCard;
            Card.Reparent(this);
            Card.LocalPosition = Vector2.Zero;

            // Add the card object and make sure it's local position is zero - we will altering it's position by changing this class' local position
            CardObject = AddChild(cardObject);
            CardObject.LocalPosition = Vector2.Zero;
        }

        #region Virtual Functions

        /// <summary>
        /// Calls Die explicitly on our card and card object if they have not had Die called.
        /// </summary>
        public override void Die()
        {
            base.Die();

            if (Card.IsAlive)
            {
                Card.Die();
            }

            if (CardObject.IsAlive)
            {
                CardObject.Die();
            }
        }

        /// <summary>
        /// A function called when this card is added to a card ship pair.
        /// This occurs with abilities, weapons etc.
        /// Allows us to apply custom effects based on the card type.
        /// </summary>
        /// <param name="cardShipPair"></param>
        public abstract void AddToCardShipPair(CardShipPair cardShipPair);

        #endregion

        #region Utility Functions

        /// <summary>
        /// A function used to switch the active object that this pair represents.
        /// Only one can be active at any one time.
        /// </summary>
        /// <param name="cardOrObject"></param>
        public void SetActiveObject(CardOrObject cardOrObject)
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
