using CelesteEngine;
using Microsoft.Xna.Framework;
using SpaceCardGameData;
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
        /// Card type tab control which holds all of the images in the 
        /// </summary>
        private CardTypesTabControl CardTabControl { get; set; }

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

        public ShopScreen() :
            base("ShopScreen")
        {

        }

        #region Virtual Functions

        protected override void AddInitialUI()
        {
            base.AddInitialUI();

            Label title = AddScreenUIObject(new Label("Shop", new Vector2(ScreenCentre.X, ScreenDimensions.Y * 0.1f)));
            title.Colour = Color.Green;

            Money = AddScreenUIObject(new ImageAndLabel(PlayerDataRegistry.Instance.PlayerData.CurrentMoney.ToString(), new Vector2(ScreenCentre.X, ScreenDimensions.Y * 0.9f), "UI\\MoneyIcon"));
            Money.Colour = Color.Black;

            BuyPackButton = AddScreenUIObject(new Button("Click to Buy Packs (" + packPrice.ToString() + ")", new Vector2(ScreenDimensions.X * 0.1f, ScreenCentre.Y), Card.CardBackTextureAsset, Card.CardBackTextureAsset));
            BuyPackButton.AddModule(new ToolTipModule("Contains 5 cards, one of which is at least rare."));
            BuyPackButton.ClickableModule.OnLeftClicked += PurchasePack;

            // Add a card tab control for all of the cards registered with the registry
            CardTabControl = AddScreenUIObject(new CardTypesTabControl(CentralCardRegistry.CardData, 
                                                                                       new Vector2(ScreenDimensions.X * 0.8f, ScreenDimensions.Y * 0.05f), 
                                                                                       new Vector2(ScreenDimensions.X * 0.8f, ScreenDimensions.Y * 0.7f), 
                                                                                       new Vector2(ScreenDimensions.X * 0.6f, ScreenDimensions.Y * 0.2f), 
                                                                                       PurchaseCard));
        }

        /// <summary>
        /// Do a UI refresh once everything is set up so we know all the cards have been added etc.
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            RefreshUI();

            foreach (CardGridControl gridControl in CardTabControl.TabbedObjects)
            {
                foreach (Card card in gridControl)
                {
                    card.AddModule(new ToolTipModule("Price: " + card.CardData.Price.ToString()), true, true);
                }
            }
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

            foreach (CardGridControl gridControl in CardTabControl.TabbedObjects)
            {
                foreach (Card card in gridControl)
                {
                    // If card price is greater than money, turn it grey and disable the click module so we cannot buy it
                    if (card.CardData.Price > PlayerMoney)
                    {
                        card.Colour = Color.Red;
                        card.ClickableModule.Hide();        // This will disable the clicking
                    }
                    else
                    {
                        card.Colour = Color.White;
                        card.ClickableModule.Show();        // Enable the card for clicking
                    }
                }
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

        private void PurchaseCard(BaseObject clickedObject)
        {
            // The clicked object has the card data stored (this is set up by the CardGridControl)
            Debug.Assert(clickedObject is Card);
            CardData cardData = (clickedObject as Card).CardData;

            Debug.Assert(PlayerMoney >= cardData.Price);
            PlayerMoney -= cardData.Price;

            PlayerDataRegistry.Instance.PlayerData.CardDataAssets.Add(CentralCardRegistry.FindCardDataAsset(cardData));
            Debug.Fail("Add UI for cards we already own/don't own");

            RefreshUI();
        }

        #endregion
    }
}
