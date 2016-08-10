using _2DEngine;
using Microsoft.Xna.Framework;
using SpaceCardGameData;

namespace SpaceCardGame
{
    /// <summary>
    /// A class representing the screen after our MainMenuScreen, where players can edit their decks and play games
    /// </summary>
    public class LobbyMenuScreen : MenuScreen
    {
        public LobbyMenuScreen(string screenDataAsset = "Screens\\LobbyMenuScreen.xml") :
            base(screenDataAsset)
        {

        }

        #region Properties and Fields

        /// <summary>
        /// Adds our buttons for playing or managing decks
        /// </summary>
        protected override void AddInitialUI()
        {
            base.AddInitialUI();

            GridControl buttonGridControl = AddScreenUIObject(new GridControl(1, 4, ScreenCentre));
            buttonGridControl.BorderPadding = new Vector2(100, 50);

            Button playGameButton = buttonGridControl.AddChild(new Button("Play", Vector2.Zero));
            playGameButton.ClickableModule.OnLeftClicked += OnPlayGameButtonLeftClicked;

            Button tutorialButton = buttonGridControl.AddChild(new Button("Tutorial", Vector2.Zero));
            tutorialButton.ClickableModule.OnLeftClicked += OnTutorialButtonLeftClicked;

            // Disable the play button if we have no decks to choose from
            if (PlayerDataRegistry.Instance.AvailableDecks == 0)
            {
                playGameButton.Disable();
            }

            Button deckManagerButton = buttonGridControl.AddChild(new Button("Decks", Vector2.Zero));
            deckManagerButton.ClickableModule.OnLeftClicked += OnDeckManagerButtonClicked;

            Button openPacksButton = buttonGridControl.AddChild(new Button("Open Packs", Vector2.Zero));
            openPacksButton.ClickableModule.OnLeftClicked += OnOpenPacksButtonLeftClicked;

            if (PlayerDataRegistry.Instance.PlayerData.AvailablePacksToOpen <= 0)
            {
                openPacksButton.Disable();
            }
        }

        /// <summary>
        /// Transition back to our main menu screen
        /// </summary>
        protected override void GoToPreviousScreen()
        {
            Transition(new MainMenuScreen());
        }

        #endregion

        #region Click Callbacks

        /// <summary>
        /// The callback to execute when we press the 'Play' button
        /// </summary>
        /// <param name="baseObject"></param>
        private void OnPlayGameButtonLeftClicked(BaseObject baseObject)
        {
            Transition(new CampaignMapScreen());
        }

        /// <summary>
        /// The callback to execute when we press the 'Tutorial' button
        /// </summary>
        /// <param name="baseObject"></param>
        private void OnTutorialButtonLeftClicked(BaseObject baseObject)
        {
            PlayerDataRegistryData startingRegistryData = AssetManager.GetData<PlayerDataRegistryData>(PlayerDataRegistry.startingDataRegistryDataAsset);
            Deck tutorialDeck = new Deck();
            tutorialDeck.Create(startingRegistryData.Decks[0].CardDataAssets);

            Transition(new TutorialScreen(tutorialDeck));
        }

        /// <summary>
        /// The callback to execute when we press the 'Decks' button
        /// </summary>
        /// <param name="image"></param>
        private void OnDeckManagerButtonClicked(BaseObject baseObject)
        {
            Transition(new DeckManagerScreen());
        }

        /// <summary>
        /// The callback to execute when we press the 'Open Packs' button
        /// </summary>
        /// <param name=""></param>
        private void OnOpenPacksButtonLeftClicked(BaseObject baseObject)
        {
            Transition(new OpenCardPacksScreen());
        }

        #endregion
    }
}