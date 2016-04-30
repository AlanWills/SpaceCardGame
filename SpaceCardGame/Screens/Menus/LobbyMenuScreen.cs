using _2DEngine;
using CardGameEngine;
using Microsoft.Xna.Framework;
using System.Diagnostics;

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

            GridControl buttonGridControl = AddScreenUIObject(new GridControl(1, 3, ScreenCentre));
            buttonGridControl.BorderPadding = new Vector2(100, 50);

            Button playGameButton = buttonGridControl.AddChild(new Button("Play", Vector2.Zero));
            playGameButton.ClickableModule.OnLeftClicked += OnPlayGameButtonLeftClicked;

            // Disable the play button if we have no decks to choose from
            if (PlayerCardRegistry.Instance.AvailableDecks == 0)
            {
                playGameButton.Disable();
            }

            Button deckManagerButton = buttonGridControl.AddChild(new Button("Decks", Vector2.Zero));
            deckManagerButton.ClickableModule.OnLeftClicked += OnDeckManagerButtonClicked;

            Button openPacksButton = buttonGridControl.AddChild(new Button("Open Packs", Vector2.Zero));
            openPacksButton.ClickableModule.OnLeftClicked += OnOpenPacksButtonLeftClicked;

            if (PlayerCardRegistry.Instance.AvailablePacksToOpen <= 0)
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
            Transition(new ChooseDeckMenuScreen());
        }

        /// <summary>
        /// The callback to execute when we press the 'Decks' button
        /// </summary>
        /// <param name="image"></param>
        private void OnDeckManagerButtonClicked(BaseObject baseObject)
        {
            Transition(new GameDeckManagerScreen());
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