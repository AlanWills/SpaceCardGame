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

            float padding = ScreenDimensions.Y * 0.1f;
            UIObject parent = null;

            Button playGameButton = AddScreenUIObject(new Button("Play", new Vector2(ScreenDimensions.X * 0.5f, ScreenDimensions.Y * 0.40f)));
            playGameButton.ClickableModule.OnLeftClicked += OnPlayGameButtonLeftClicked;
            parent = playGameButton;

            // Disable the play button if we have no decks to choose from
            if (PlayerCardRegistry.Instance.AvailableDecks == 0)
            {
                playGameButton.Disable();
            }

            Button deckManagerButton = parent.AddChild(new Button("Decks", new Vector2(0, padding)));
            deckManagerButton.ClickableModule.OnLeftClicked += OnDeckManagerButtonClicked;
            parent = deckManagerButton;

            Button openPacksButton = parent.AddChild(new Button("Open Packs", new Vector2(0, padding)));
            openPacksButton.ClickableModule.OnLeftClicked += OnOpenPacksButtonLeftClicked;
            parent = openPacksButton;

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
            // Have to do this separately so we get the callbacks added to our objects during load
            ChooseDeckBox chooseDeckBox = AddScreenUIObject(new ChooseDeckBox("Choose Deck", ScreenCentre), true, true);
            chooseDeckBox.OnLeftClicked += ChooseDeckBoxClicked;
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

        /// <summary>
        /// The callback to execute when we choose our deck
        /// </summary>
        /// <param name="image"></param>
        private void ChooseDeckBoxClicked(BaseObject baseObject)
        {
            Debug.Assert(baseObject is UIObject);
            UIObject clickableImage = baseObject as UIObject;

            DebugUtils.AssertNotNull(clickableImage.StoredObject);
            Debug.Assert(clickableImage.StoredObject is Deck);

            Transition(new BattleScreen(clickableImage.StoredObject as Deck, "Screens\\BattleScreen.xml"));
        }

        #endregion
    }
}