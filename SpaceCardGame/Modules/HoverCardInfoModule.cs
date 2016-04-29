using _2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A module which creates an enlarged version of a game card in the centre of the screenwhen the mouse is hovered over it.
    /// </summary>
    public class HoverCardInfoModule : BaseObjectModule
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to our our info image - will cache this and then insert/extract it from the current screen as appropriate
        /// </summary>
        protected Image InfoImage { get; private set; }

        /// <summary>
        /// A reference to the attached card we are showing info
        /// </summary>
        private CardObjectPair AttachedCardObjectPair { get; set; }

        private bool cardAdded;

        #endregion

        public HoverCardInfoModule(CardObjectPair cardObjectPair) :
            base()
        {
            AttachedCardObjectPair = cardObjectPair;
        }

        #region Virtual Functions

        /// <summary>
        /// Creates, loads and initialises the info image
        /// </summary>
        public override void Initialise()
        {
            CheckShouldInitialise();

            InfoImage = CreateInfoImageForAttachedCard();
            InfoImage.LoadContent();
            InfoImage.Initialise();

            base.Initialise();
        }

        /// <summary>
        /// Adds or removes the object from the main screen as approprite
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            DebugUtils.AssertNotNull(AttachedCardObjectPair.Card.Collider);
            DebugUtils.AssertNotNull(AttachedCardObjectPair.CardObject.Collider);

            BattleScreen battleScreen = ScreenManager.Instance.CurrentScreen as BattleScreen;
            // Depending on the turn state, show this UI based on the card or the object's collider
            Collider colliderToCheck = battleScreen.TurnState == TurnState.kPlaceCards ? AttachedCardObjectPair.Card.Collider : AttachedCardObjectPair.CardObject.Collider;

            if (!cardAdded)
            {
                if (IsInputValidToShowCard(battleScreen, colliderToCheck))
                {
                    // If we have just entered the object, we add the info image to the screen
                    battleScreen.AddScreenUIObject(InfoImage);
                    cardAdded = true;
                }
            }
            else
            {
                if (!IsInputValidToShowCard(battleScreen, colliderToCheck))
                {
                    // If we have just exited the object, we extract the info image from the screen, but do not kill it
                    // This means we can cache it rather than constantly recreating it
                    battleScreen.ExtractScreenUIObject(InfoImage);
                    cardAdded = false;
                }
            }
        }

        /// <summary>
        /// Really make sure that we kill the info image if this is killed - it could still be inserted in the screen somewhere.
        /// </summary>
        public override void Die()
        {
            base.Die();

            InfoImage.Die();
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Creates a different info image based on the type of the attached card.
        /// </summary>
        /// <returns></returns>
        private Image CreateInfoImageForAttachedCard()
        {
            GameCard card = AttachedCardObjectPair.Card;

            Debug.Assert(card.Size != Vector2.Zero);
            Vector2 size = card.Size * 1.5f;
            Vector2 position = ScreenManager.Instance.ScreenCentre;

            if (AttachedCardObjectPair is CardAbilityPair)
            {
                return new Image(size, position, card.TextureAsset);
            }
            else if (AttachedCardObjectPair is CardResourcePair)
            {
                return new Image(size, position, card.TextureAsset);
            }
            else if (AttachedCardObjectPair is CardShieldPair)
            {
                return new ShieldInfoImage(AttachedCardObjectPair as CardShieldPair, size, position);
            }
            else if (AttachedCardObjectPair is CardShipPair)
            {
                return new ShipInfoImage(AttachedCardObjectPair as CardShipPair, size, position);
            }
            else if (AttachedCardObjectPair is CardWeaponPair)
            {
                return new WeaponInfoImage(AttachedCardObjectPair as CardWeaponPair, size, position);
            }
            else
            {
                Debug.Fail("Unmatched card type in CreateInfoImage");
                return null;
            }
        }

        /// <summary>
        /// A function for determining whether the input in our game is sufficient to add the card info image to the screen.
        /// In card placement this returns true when our mouse is over the object.
        /// In battle phase this returns true when the mouse is over the object and LeftShift is held.
        /// </summary>
        /// <param name="battleScreen"></param>
        /// <param name="colliderToCheck"></param>
        /// <returns></returns>
        private bool IsInputValidToShowCard(BattleScreen battleScreen, Collider colliderToCheck)
        {
            if (battleScreen.TurnState == TurnState.kPlaceCards)
            {
                return colliderToCheck.IsMouseOver;
            }
            else
            {
                return colliderToCheck.IsMouseOver && GameKeyboard.IsKeyDown(Keys.LeftShift);
            }
        }

        #endregion
    }
}