using _2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A module which creates an enlarged version of a game card in the centre of the screenwhen the mouse is hovered over it.
    /// </summary>
    public class CardHoverInfoModule : ToolTipModule
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the attached card we are showing info
        /// </summary>
        private CardObjectPair AttachedCardObjectPair { get; set; }

        protected override Vector2 ToolTipPosition
        {
            get
            {
                return ScreenManager.Instance.ScreenCentre;
            }
        }

        #endregion

        /// <summary>
        /// Pass in the card object pair here because the attached object will not be set in the constructor.
        /// Also provides a way of limiting what objects can use this module.
        /// </summary>
        /// <param name="cardObjectPair"></param>
        public CardHoverInfoModule(CardObjectPair cardObjectPair)
        {
            AttachedCardObjectPair = cardObjectPair;

            ToolTip = CreateInfoImageForAttachedCard();
        }

        #region Utility Functions

        /// <summary>
        /// Creates a different info image based on the type of the attached card.
        /// </summary>
        /// <returns></returns>
        private Image CreateInfoImageForAttachedCard()
        {
            Card card = AttachedCardObjectPair.Card;

            Debug.Assert(card.Size != Vector2.Zero);
            Vector2 size = card.Size * 1.5f;
            Vector2 position = ScreenManager.Instance.ScreenCentre;
            
            return new Image(size, position, card.TextureAsset);
        }

        /// <summary>
        /// A function for determining whether the input in our game is sufficient to add the card info image to the screen.
        /// In card placement this returns true when our mouse is over the object.
        /// In battle phase this returns true when the mouse is over the object and LeftShift is held.
        /// </summary>
        /// <param name="battleScreen"></param>
        /// <param name="colliderToCheck"></param>
        /// <returns></returns>
        protected override bool IsInputValidToShowToolTip()
        {
            DebugUtils.AssertNotNull(AttachedCardObjectPair.Card.Collider);
            DebugUtils.AssertNotNull(AttachedCardObjectPair.CardObject.Collider);

            BattleScreen battleScreen = ScreenManager.Instance.GetCurrentScreenAs<BattleScreen>();

            // Depending on the turn state, show this UI based on the card or the object's collider
            if (battleScreen.TurnState == TurnState.kPlaceCards)
            {
                return AttachedCardObjectPair.Card.Collider.IsMouseOver;
            }
            else
            {
                return AttachedCardObjectPair.CardObject.Collider.IsMouseOver && GameKeyboard.IsKeyDown(Keys.LeftShift);
            }
        }

        #endregion
    }
}