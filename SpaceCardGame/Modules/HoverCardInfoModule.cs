using _2DEngine;
using CardGameEngine;
using Microsoft.Xna.Framework;
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
        private Image InfoImage { get; set; }

        /// <summary>
        /// A reference to the attached card we are showing info
        /// </summary>
        private CardObjectPair AttachedCardObjectPair { get; set; }

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
            DebugUtils.AssertNotNull(ScreenManager.Instance.CurrentScreen);
            Debug.Assert(ScreenManager.Instance.CurrentScreen is BattleScreen);

            BattleScreen battleScreen = ScreenManager.Instance.CurrentScreen as BattleScreen;
            // Depending on the turn state, show this UI based on the card or the object's collider
            Collider colliderToCheck = battleScreen.TurnState == TurnState.kPlaceCards ? AttachedCardObjectPair.Card.Collider : AttachedCardObjectPair.CardObject.Collider;

            if (colliderToCheck.IsEntered)
            {
                // If we have just entered the object, we add the info image to the screen
                battleScreen.AddScreenUIObject(InfoImage);
            }
            
            if (colliderToCheck.IsExited)
            {
                // If we have just exited the object, we extract the info image from the screen, but do not kill it
                // This means we can cache it rather than constantly recreating it
                battleScreen.ExtractScreenUIObject(InfoImage);
            }
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
        /// Really make sure that we kill the info image if this is killed - it could still be inserted in the screen somewhere.
        /// </summary>
        public override void Die()
        {
            base.Die();

            InfoImage.Die();
        }

        #endregion
    }
}