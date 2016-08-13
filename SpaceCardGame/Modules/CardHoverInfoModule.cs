using _2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A module which creates an enlarged version of a game card in the centre of the screen when the mouse is hovered over it.
    /// </summary>
    public class CardHoverInfoModule : ToolTipModule
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the attached card we are showing info
        /// </summary>
        private Card AttachedCard { get; set; }

        protected override Vector2 ToolTipPosition
        {
            get
            {
                return ScreenManager.Instance.ScreenCentre;
            }
        }

        #endregion

        /// <summary>
        /// Pass in the card here because the attached object will not be set in the constructor.
        /// Also provides a way of limiting what objects can use this module.
        /// </summary>
        /// <param name="card"></param>
        public CardHoverInfoModule(Card card)
        {
            AttachedCard = card;

            ToolTip = CreateInfoImageForAttachedCard();
        }

        /// <summary>
        /// Pass in the card object pair here because the attached object will not be set in the constructor.
        /// Also provides a way of limiting what objects can use this module.
        /// </summary>
        /// <param name="cardObjectPair"></param>
        public CardHoverInfoModule(CardObjectPair cardObjectPair) :
            this(cardObjectPair.Card)
        {
            
        }

        #region Utility Functions

        /// <summary>
        /// Creates a different info image based on the type of the attached card.
        /// </summary>
        /// <returns></returns>
        private Image CreateInfoImageForAttachedCard()
        {
            Debug.Assert(AttachedCard.Size != Vector2.Zero);
            Vector2 size = AttachedCard.Size * 2f;
            Vector2 position = ScreenManager.Instance.ScreenCentre;
            
            return new Image(size, position, AttachedCard.TextureAsset);
        }

        /// <summary>
        /// A function for determining whether the input in our game is sufficient to add the card info image to the screen.
        /// In the battle screen we have two possibilities:
        ///     In card placement this returns true when our mouse is over the object.
        ///     In battle phase this returns true when the mouse is over the object and LeftShift is held.
        ///     
        /// In a menu screen, we just check whether the mouse is over the card
        /// </summary>
        /// <param name="battleScreen"></param>
        /// <param name="colliderToCheck"></param>
        /// <returns></returns>
        protected override bool IsInputValidToShowToolTip()
        {
            DebugUtils.AssertNotNull(AttachedCard.Collider);

            BattleScreen battleScreen = ScreenManager.Instance.CurrentScreen as BattleScreen;

            if (battleScreen != null)
            {
                // Depending on the turn state, show this UI based on the card or the object's collider
                if (battleScreen.TurnState == TurnState.kPlaceCards)
                {
                    return AttachedCard.Collider.IsMouseOver;
                }
                else
                {
                    DebugUtils.AssertNotNull(AttachedCard.GetCardObjectPair<CardObjectPair>().CardObject.Collider);
                    return GameKeyboard.IsKeyDown(Keys.LeftShift) && AttachedCard.GetCardObjectPair<CardObjectPair>().CardObject.Collider.IsMouseOver;
                }
            }
            else
            {
                return AttachedCard.Collider.IsMouseOver;
            }
        }

        #endregion
    }
}