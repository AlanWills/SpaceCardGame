using _2DEngine;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

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
    public class CardObjectPair : GameObjectContainer
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the card part of our pair
        /// </summary>
        private GameCard Card { get; set; }

        /// <summary>
        /// A reference to the object part of our pair
        /// </summary>
        private GameObject CardObject { get; set; }

        #endregion

        // This class is a substitute for a game card, so again we do not need to input the position as this will be sorted out by the screen
        public CardObjectPair(GameCard gameCard, GameObject cardObject) :
            base(Vector2.Zero, AssetManager.EmptyGameObjectDataAsset)
        {
            Card = gameCard;

            // Add the card object and make sure it's local position is zero - we will altering it's position by changing this class' local position
            CardObject = AddObject(cardObject);
            CardObject.LocalPosition = Vector2.Zero;
        }

        #region Virtual Functions

        /// <summary>
        /// Add the card - it will have already been loaded and initialised by this point
        /// </summary>
        public override void Initialise()
        {
            CheckShouldInitialise();

            base.Initialise();

            // Add the card and make sure it's local position is zero - we will be altering it's position by changing this class' local position
            AddObject(Card, false, false);
            Card.LocalPosition = Vector2.Zero;
        }

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
