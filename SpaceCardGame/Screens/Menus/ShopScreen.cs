using _2DEngine;
using Microsoft.Xna.Framework;

namespace SpaceCardGame
{
    /// <summary>
    /// A screen where the player can use in game money to buy packs and specific cards.
    /// </summary>
    public class ShopScreen : MenuScreen
    {
        #region Properties and Fields

        /// <summary>
        /// The UI representing our current money
        /// </summary>
        private ImageAndLabel Money { get; set; }

        #endregion

        public ShopScreen(string screenDataAsset = "Screens\\ShopScreen.xml") :
            base(screenDataAsset)
        {

        }

        #region Virtual Functions

        protected override void AddInitialUI()
        {
            base.AddInitialUI();

            Label title = AddScreenUIObject(new Label("Shop", new Vector2(ScreenCentre.X, ScreenDimensions.Y * 0.1f)));
            title.Colour.Value = Color.Green;

            Money = AddScreenUIObject(new ImageAndLabel(PlayerDataRegistry.Instance.PlayerData.CurrentMoney.ToString(), ScreenCentre/*new Vector2(ScreenCentre.X, ScreenDimensions.Y * 0.9f)*/, "UI\\MoneyIcon"));
            Money.Colour.Value = Color.Black;
        }

        /// <summary>
        /// Transition back to the lobby and save the Player's data
        /// </summary>
        protected override void GoToPreviousScreen()
        {
            PlayerDataRegistry.Instance.SaveAssets();
            Transition(new LobbyMenuScreen());
        }

        #endregion
    }
}
