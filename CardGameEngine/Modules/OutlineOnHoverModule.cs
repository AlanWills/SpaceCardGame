using _2DEngine;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace CardGameEngine
{
    public delegate void CustomOutlineColourHandler(Image cardOutlineImage);

    /// <summary>
    /// Shows an outline around the card when the mouse is over
    /// </summary>
    public class OutlineOnHoverModule : BaseObjectModule
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to our card outline image
        /// </summary>
        private Image CardOutlineImage { get; set; }

        /// <summary>
        /// A reference to the attached base object as a BaseGameCard
        /// </summary>
        private BaseCard gameCard;
        private BaseCard GameCard
        {
            get
            {
                if (gameCard == null)
                {
                    Debug.Assert(AttachedBaseObject is BaseCard);
                    gameCard = AttachedBaseObject as BaseCard;
                }

                return gameCard;
            }
        }

        /// <summary>
        /// An event we can override to add custom behaviour to our outline colour
        /// </summary>
        public event CustomOutlineColourHandler CustomOutlineColour;

        private static string cardOutlineImageTextureAsset = "Cards\\CardOutline";

        private static Color validColour = Color.LightGreen;
        private static Color invalidColour = new Color(1, 0.75f, 0.75f, 1);

        #endregion

        public OutlineOnHoverModule() :
            base()
        {

        }

        #region Virtual Functions

        /// <summary>
        /// Create our card outline image
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            CardOutlineImage = AttachedBaseObject.AddChild(new Image(Vector2.Zero, cardOutlineImageTextureAsset), true);

            base.LoadContent();
        }

        /// <summary>
        /// Updates the visibility and size of our outline image
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            DebugUtils.AssertNotNull(AttachedBaseObject);
            DebugUtils.AssertNotNull(AttachedBaseObject.Collider);

            CardOutlineImage.ShouldDraw.Value = AttachedBaseObject.Collider.IsMouseOver;
            CardOutlineImage.Colour.Value = validColour;

            if (GameCard is BaseUICard)
            {
                CardOutlineImage.Size = (AttachedBaseObject as BaseUICard).DrawingSize;
            }

            // Call our custom event if it is set up
            if (CustomOutlineColour != null)
            {
                CustomOutlineColour(CardOutlineImage);
            }
        }

        #endregion
    }
}
