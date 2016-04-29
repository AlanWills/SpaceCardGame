using _2DEngine;
using Microsoft.Xna.Framework;

namespace SpaceCardGame
{
    /// <summary>
    /// A screen used in debug to calculate hard points easily for ships
    /// </summary>
    public class HardpointScreen : MenuScreen
    {
        #region Properties and Fields

        /// <summary>
        /// The image of the ship
        /// </summary>
        private Image ShipImage { get; set; }

        /// <summary>
        /// The label for our local position
        /// </summary>
        private Label Label { get; set; }

        const string type = "Stations";
        const string shipTexture = "BastionShipyard";

        #endregion

        public HardpointScreen(string hardpointScreen = "Screens\\HardpointScreen.xml") :
            base(hardpointScreen)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// Sets all the UI up
        /// </summary>
        protected override void AddInitialUI()
        {
            base.AddInitialUI();

            ShipImage = AddScreenUIObject(new Image(ScreenCentre, "Cards\\" + type + "\\" + shipTexture + "\\" + shipTexture + "Object"));
            Label = AddScreenUIObject(new Label((GameMouse.Instance.WorldPosition - ScreenCentre).ToPoint().ToString(), new Vector2(100, ScreenCentre.Y)));
        }

        /// <summary>
        /// Updates our label based on the mouse position
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            Label.Text = (GameMouse.Instance.WorldPosition - ScreenCentre).ToPoint().ToString();
            //Label.Colour.Value = Color.Black;
        }

        #endregion
    }
}
