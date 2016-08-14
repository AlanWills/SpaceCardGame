using _2DEngine;
using Microsoft.Xna.Framework;

namespace SpaceCardGame
{
    /// <summary>
    /// An image used for our card outline which we can animate and update accordingly
    /// </summary>
    public class CardOutline : Image
    {
        #region Properties and Fields

        /// <summary>
        /// A flag to indicate whether the card is valid or not - will dictate the colour we are.
        /// </summary>
        public bool Valid
        {
            get;
            set;
        }

        #endregion

        public CardOutline(Vector2 size) :
            this(size, Vector2.Zero)
        {
            
        }

        public CardOutline(Vector2 size, Vector2 localPosition) :
            base(size, localPosition, "Cards\\CardOutline")
        {
            Valid = true;
        }


        #region Virtual Functions

        /// <summary>
        /// Updates the colour of this outline
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            Colour = Valid ? Color.Green : Color.Gray;
        }

        #endregion
    }
}