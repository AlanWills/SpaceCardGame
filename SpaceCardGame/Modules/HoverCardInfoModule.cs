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
        private BaseCard AttachedCard { get; set; }

        #endregion

        public HoverCardInfoModule(GameCard card) :
            base()
        {
            DebugUtils.AssertNotNull(card);
            AttachedCard = card;
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

            DebugUtils.AssertNotNull(AttachedBaseObject.Collider);
            DebugUtils.AssertNotNull(ScreenManager.Instance.CurrentScreen);

            if (AttachedBaseObject.Collider.IsEntered)
            {
                // If we have just entered the object, we add the info image to the screen
                ScreenManager.Instance.CurrentScreen.AddScreenUIObject(InfoImage);
            }
            
            if (AttachedBaseObject.Collider.IsExited)
            {
                // If we have just exited the object, we extract the info image from the screen, but do not kill it
                // This means we can cache it rather than constantly recreating it
                ScreenManager.Instance.CurrentScreen.ExtractScreenUIObject(InfoImage);
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
            Vector2 size = AttachedBaseObject.Size * 1.5f;
            Vector2 position = ScreenManager.Instance.ScreenCentre;

            if (AttachedCard is AbilityCard)
            {
                return new Image(size, position, AttachedCard.TextureAsset);
            }
            else if (AttachedCard is ResourceCard)
            {
                return new Image(size, position, AttachedCard.TextureAsset);
            }
            else if (AttachedCard is ShieldCard)
            {
                return new ShieldInfoImage(size, position, AttachedCard.TextureAsset);
            }
            else if (AttachedCard is ShipCard)
            {
                return new ShipInfoImage(AttachedCard.Parent as CardShipPair, size, position);
            }
            else if (AttachedCard is WeaponCard)
            {
                return new WeaponInfoImage(AttachedCard.Parent as CardWeaponPair, size, position);
            }
            else
            {
                Debug.Fail("Unmatched card type in CreateInfoImage");
                return null;
            }
        }

        #endregion
    }
}