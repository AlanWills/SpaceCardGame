using CelesteEngine;
using Microsoft.Xna.Framework;
using SpaceCardGameData;
using System.Collections.Generic;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A custom UI window that is displayed when the player receives rewards from the game.
    /// This is typically after completing a match.
    /// </summary>
    public class PlayerVictoryUI : UIObject
    {
        #region Properties and Fields

        /// <summary>
        /// The data structure that holds the actual information on what the player has won.
        /// </summary>
        private RewardData RewardData { get; set; }

        /// <summary>
        /// A button used to navigate to the previous reward in the UI
        /// </summary>
        private Button PreviousRewardButton { get; set; }

        /// <summary>
        /// A button used to navigate tothe next reward in the UI
        /// </summary>
        private Button NextRewardButton { get; set; }

        /// <summary>
        /// A list containing all of the reward UI that we will be wanting to navigate between using the buttons
        /// </summary>
        private List<UIObject> RewardUI { get; set; }

        /// <summary>
        /// Index of the current reward UI we are showing
        /// </summary>
        private int CurrentRewardUI { get; set; }

        #endregion

        public PlayerVictoryUI(RewardData rewardData, string textureAsset = AssetManager.DefaultMenuTextureAsset) :
            base(new Vector2(ScreenManager.Instance.ScreenDimensions.X * 0.25f, ScreenManager.Instance.ScreenDimensions.Y * 0.5f), ScreenManager.Instance.ScreenCentre, textureAsset)
        {
            RewardData = rewardData;
            RewardUI = new List<UIObject>();
            UsesCollider = false;
        }

        #region Virtual Functions

        /// <summary>
        /// Set up all of the UI for this window from the RewardData
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            PreviousRewardButton = AddChild(new Button("Previous", Anchor.kCentreLeft, 1, AssetManager.DefaultNarrowButtonTextureAsset, AssetManager.DefaultNarrowButtonHighlightedTextureAsset));
            PreviousRewardButton.ClickableModule.OnLeftClicked += GoToPreviousRewardUI;

            NextRewardButton = AddChild(new Button("Next", Anchor.kCentreRight, 1, AssetManager.DefaultNarrowButtonTextureAsset, AssetManager.DefaultNarrowButtonHighlightedTextureAsset));
            NextRewardButton.ClickableModule.OnLeftClicked += GoToNextRewardUI;

            Button doneButton = AddChild(new Button("Done", Anchor.kBottomCentre, 0, AssetManager.DefaultNarrowButtonTextureAsset, AssetManager.DefaultNarrowButtonHighlightedTextureAsset));
            doneButton.ClickableModule.OnLeftClicked += AddRewards;

            // Create the reward UI here

            // Money earnt
            Image moneyImage = AddChild(new Image(Vector2.Zero, "UI\\MoneyIcon"));
            Label moneyEarntExplanation = moneyImage.AddChild(new Label("Money Earnt", Anchor.kTopCentre, 4));
            moneyEarntExplanation.Colour = Color.White;
            Label moneyEarntValue = moneyImage.AddChild(new Label(RewardData.Money.ToString(), Anchor.kBottomCentre, 4));
            moneyEarntValue.Colour = Color.White;
            RewardUI.Add(moneyImage);

            // Cards earnt
            foreach (string cardDataAsset in RewardData.CardDataAssets)
            {
                CardData cardData = AssetManager.GetData<CardData>("Cards\\" + cardDataAsset);
                Image cardImage = AddChild(new Image(Vector2.Zero, cardData.TextureAsset));
                Label cardWonExplanation = cardImage.AddChild(new Label("Card Won", Anchor.kTopCentre, 2));
                cardWonExplanation.Colour = Color.White;
                Label cardWonName = cardImage.AddChild(new Label(cardData.DisplayName, Anchor.kBottomCentre, 2));
                cardWonName.Colour = Color.White;
                RewardUI.Add(cardImage);
            }

            // Packs won
            Image packsWonImage = AddChild(new Image(Vector2.Zero, Card.CardBackTextureAsset));
            Label packsWonExplanation = packsWonImage.AddChild(new Label("Packs Won", Anchor.kTopCentre, 2));
            packsWonExplanation.Colour = Color.White;
            Label packsWonValue = packsWonImage.AddChild(new Label(RewardData.CardPacks.ToString(), Anchor.kBottomCentre, 2));
            packsWonValue.Colour = Color.White;
            RewardUI.Add(packsWonImage);

            // We have set the current UI to be 0, so make sure the UI is updated to reflect this
            RefreshUI();

            base.LoadContent();
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Uses the current value of the CurrentRewardUI to update the UI.
        /// Hides all the reward UI except for the current one and updates the navigation buttons so that they are only displayed if navigation is possible.
        /// </summary>
        private void RefreshUI()
        {
            // Hide all the reward UI except the current UI

            if (CurrentRewardUI == 0)
            {
                // Hide the previous reward button if we have reached the first reward UI
                PreviousRewardButton.Hide();
                NextRewardButton.Show();
            }
            else if (CurrentRewardUI == RewardUI.Count - 1)
            {
                PreviousRewardButton.Show();
                NextRewardButton.Hide();
            }
            else
            {
                PreviousRewardButton.Show();
                NextRewardButton.Show();
            }

            for (int i = 0; i < RewardUI.Count; i++)
            {
                if (i == CurrentRewardUI)
                {
                    RewardUI[i].Show();
                }
                else
                {
                    RewardUI[i].Hide();
                }
            }
        }

        #endregion

        #region Callbacks

        /// <summary>
        /// Hides the current UI and shows previous UI in the list.
        /// Increments the CurrentRewardUI index by one and if it is the first reward UI, we disable this button.
        /// </summary>
        /// <param name="clickedObject"></param>
        private void GoToPreviousRewardUI(BaseObject clickedObject)
        {
            // Should not be able to click this if this is not true
            Debug.Assert(CurrentRewardUI > 0 && CurrentRewardUI < RewardUI.Count );
            CurrentRewardUI--;

            RefreshUI();
        }
        
        /// <summary>
        /// Hides the current UI and shows the next UI in the list.
        /// Increments the CurrentRewardUI index by one and if it is the last reward UI, we disable this button
        /// </summary>
        /// <param name="clickedObject"></param>
        private void GoToNextRewardUI(BaseObject clickedObject)
        {
            // Should not be able to click this if this is not true
            Debug.Assert(CurrentRewardUI >= 0 && CurrentRewardUI < RewardUI.Count - 1);
            CurrentRewardUI++;

            RefreshUI();
        }

        /// <summary>
        /// When the player quits the dialog, we add the rewards to their account and then go back to the mission screen.
        /// </summary>
        /// <param name="clickedObject"></param>
        private void AddRewards(BaseObject clickedObject)
        {
            PlayerDataRegistry.Instance.PlayerData.CardDataAssets.AddRange(RewardData.CardDataAssets);
            PlayerDataRegistry.Instance.PlayerData.AvailablePacksToOpen += RewardData.CardPacks;

            ScreenManager.Instance.CurrentScreen.Transition(new CampaignMapScreen());
        }

        #endregion
    }
}
