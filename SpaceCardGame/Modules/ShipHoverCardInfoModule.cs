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
        /// If we mouse over with the left control down we show the ship as a preview.
        /// If we release the left control it goes back to normal.
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            BattleScreen battleScreen = ScreenManager.Instance.CurrentScreen as BattleScreen;

            DebugUtils.AssertNotNull(CardShipPair.Ship.Collider);
            if (!scaled)
            {
                if (IsInputValidToPreviewShip(battleScreen, CardShipPair.Ship.Collider))
                {
                    currentSize = CardShipPair.Ship.Size;
                    CardShipPair.Ship.Resize(CardShipPair.Ship.TextureCentre * 2);
                    CardShipPair.LocalPosition = PreviewPosition;
                    InfoImage.Hide();

                    scaled = true;
                }
            }
            else
            {
                if (!GameKeyboard.IsKeyDown(Keys.LeftControl))
                {
                    CardShipPair.Ship.Resize(currentSize);
                    CardShipPair.LocalPosition = CardControlPosition;
                    InfoImage.Show();

                    scaled = false;
                }
            }
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// A function which returns whether our input is sufficient for us to show the ship preview.
        /// Returns true if we are in the battle screen, the mouse is over our ship and LeftCtrl is held down.
        /// Otherwise returns false.
        /// </summary>
        /// <param name="battlScreen"></param>
        /// <param name="colliderToCheck"></param>
        /// <returns></returns>
        private bool IsInputValidToPreviewShip(BattleScreen battleScreen, Collider colliderToCheck)
        {
            if (battleScreen.TurnState == TurnState.kBattle)
            {
                return colliderToCheck.IsMouseOver && GameKeyboard.IsKeyDown(Keys.LeftControl);
            }

            return false;
        }

        #endregion
    }
}
