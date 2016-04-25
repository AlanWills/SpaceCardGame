using _2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceCardGame
{
    /// <summary>
    /// An extension of the hover card info module designed for use with ships.
    /// As well as displaying the standard card info, if shift is held whilst the mouse is over, the full scale ship will be shown.
    /// </summary>
    public class ShipHoverCardInfoModule : HoverCardInfoModule
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the ship we want to preview.
        /// </summary>
        private CardShipPair CardShipPair { get; set; }

        /// <summary>
        /// The position for our ship in the ShipCardControl.
        /// </summary>
        private Vector2 CardControlPosition { get; set; }

        /// <summary>
        /// The position for our ship in preview.
        /// </summary>
        private Vector2 PreviewPosition { get; set; }

        private Vector2 currentSize;
        private bool scaled;

        #endregion

        public ShipHoverCardInfoModule(CardShipPair cardShipPair) :
            base(cardShipPair)
        {
            CardShipPair = cardShipPair;
        }

        #region Properties and Fields

        /// <summary>
        /// Set up our card control position when our ship has been added to the ship control.
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            CardControlPosition = CardShipPair.LocalPosition;
            PreviewPosition = ScreenManager.Instance.ScreenCentre - CardShipPair.Parent.WorldPosition;
        }

        /// <summary>
        /// If we mouse over with the left shift down we show the ship as a preview.
        /// If we release the left shift it goes back to normal.
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            DebugUtils.AssertNotNull(CardShipPair.Ship.Collider);
            if (!scaled && CardShipPair.Ship.Collider.IsMouseOver && GameKeyboard.IsKeyDown(Keys.LeftShift))
            {
                currentSize = CardShipPair.Ship.Size;
                CardShipPair.Ship.ApplyScaling(CardShipPair.Ship.TextureCentre * 2);
                CardShipPair.LocalPosition = PreviewPosition;
                InfoImage.Hide();

                scaled = true;
            }
            else if (scaled && !GameKeyboard.IsKeyDown(Keys.LeftShift))
            {
                CardShipPair.Ship.ApplyScaling(currentSize);
                CardShipPair.LocalPosition = CardControlPosition;
                InfoImage.Show();

                scaled = false;
            }
        }

        #endregion
    }
}
