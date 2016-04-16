using _2DEngine;
using CardGameEngine;
using Microsoft.Xna.Framework;

namespace SpaceCardGame
{
    /// <summary>
    /// A module which creates an enlarged version of a game card in the centre of the screenwhen the mouse is hovered over it.
    /// </summary>
    public class HoverCardInfoModule : BaseObjectModule
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to our our image - will cache this and then insert/extract it from the current screen as appropriate
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
            AttachedCard = card;
        }

        public HoverCardInfoModule(CardObjectPair cardPair) :
            this(cardPair.Card)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// Creates and loads the info image 
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            InfoImage = new Image(ScreenManager.Instance.ScreenCentre, AttachedCard.CardData.TextureAsset);
            InfoImage.LoadContent();

            base.LoadContent();
        }

        /// <summary>
        /// Initialises the info image
        /// </summary>
        public override void Initialise()
        {
            CheckShouldInitialise();

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
    }
}