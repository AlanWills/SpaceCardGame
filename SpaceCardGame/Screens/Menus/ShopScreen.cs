using _2DEngine;
using Microsoft.Xna.Framework;
using System.Diagnostics;

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

        /// <summary>
        /// A reference to the button we use to buy packs
        /// </summary>
        private Button BuyPackButton { get; set; }

        /// <summary>
        /// Shortcut to accessing the player's money
        /// </summary>
        private int PlayerMoney
        {
            get
            {
                return PlayerDataRegistry.Instance.PlayerData.CurrentMoney;
            }
            set
            {
                PlayerDataRegistry.Instance.PlayerData.CurrentMoney = value;
            }
        }

        private const int packPrice = 200;

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

            Money = AddScreenUIObject(new ImageAndLabel(PlayerDataRegistry.Instance.PlayerData.CurrentMoney.ToString(), new Vector2(ScreenCentre.X, ScreenDimensions.Y * 0.9f), "UI\\MoneyIcon"));
            Money.Colour.Value = Color.Black;

            BuyPackButton = AddScreenUIObject(new Button("Click to Buy Packs (" + packPrice.ToString() + ")", new Vector2(ScreenCentre.X * 0.5f, ScreenCentre.Y), Card.CardBackTextureAsset, Card.CardBackTextureAsset));
            BuyPackButton.AddModule(new ToolTipModule("Contains 5 cards, one of which is at least rare."));
            BuyPackButton.ClickableModule.OnLeftClicked += PurchasePack;

            // Create a new piece of UI that is a tab control with different sections for each card type in an inputted list.
            // Then the DeckCardTypeControl can override that using some custom functionality for clicking, but leave the UI layout to the base class
            // We can also then plonk it in here to do something similar
            TabControl cardTabControl = AddScreenUIObject(new TabControl(new Vector2(ScreenDimensions.X * 0.4f, ScreenDimensions.Y * 0.8f), new Vector2(ScreenDimensions.X * 0.75f, ScreenCentre.Y)));
            
            foreach (string cardType in CentralCardRegistry.CardTypes)
            {
                
            }

            RefreshUI();
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

        #region Utility Functions

        /// <summary>
        /// Updates all of the UI on the screen to adjust for the current state, including Player's money, new cards, invalid options (e.g. buying stuff they can't afford).
        /// </summary>
        private void RefreshUI()
        {
            Money.Label.Text = PlayerMoney.ToString();

            if (PlayerMoney < packPrice)
            {
                BuyPackButton.Disable();
            }
        }

        #endregion

        #region Click Callbacks

        private void PurchasePack(BaseObject clickedObject)
        {
            Debug.Assert(PlayerMoney >= packPrice);
            PlayerMoney -= packPrice;

            RefreshUI();
        }

        #endregion
    }
}
