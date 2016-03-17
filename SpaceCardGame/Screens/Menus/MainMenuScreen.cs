using _2DEngine;
using CardGameEngine;
using Microsoft.Xna.Framework;
using System.IO;

namespace SpaceCardGame
{
    public class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen(string screenDataAsset = "Content\\Data\\Screens\\MainMenuScreen.xml") :
            base(screenDataAsset)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// Add Buttons to our MainMenuScreen
        /// </summary>
        protected override void AddInitialUI()
        {
            base.AddInitialUI();

            float padding = ScreenDimensions.Y * 0.1f;
            BaseObject parent = null;

            Button newGameButton = AddScreenUIObject(new Button("New Game", new Vector2(ScreenDimensions.X * 0.5f, ScreenDimensions.Y * 0.35f)));
            newGameButton.OnLeftClicked += OnNewGameButtonLeftClicked;
            parent = newGameButton;

            Button continueGameButton = AddScreenUIObject(new Button("Continue", new Vector2(0, padding)));
            continueGameButton.SetParent(parent);
            continueGameButton.OnLeftClicked += OnContinueButtonLeftClicked;
            parent = continueGameButton;

            // Disable the continue button if we have no saved data file
            if (!File.Exists(ScreenManager.Instance.Content.RootDirectory + PlayerCardRegistry.playerCardRegistryDataAsset))
            {
                continueGameButton.Disable();
            }

            Button optionsButton = AddScreenUIObject(new Button("Options", new Vector2(0, padding)));
            optionsButton.SetParent(parent);
            optionsButton.OnLeftClicked += OnOptionsButtonClicked;
            parent = optionsButton;

            Button exitGameButton = AddScreenUIObject(new Button("Exit", new Vector2(0, padding)));
            exitGameButton.SetParent(parent);
            exitGameButton.OnLeftClicked += OnExitGameButtonClicked;
            parent = exitGameButton;
        }

        #endregion

        #region Event callbacks for main menu screen buttons

        /// <summary>
        /// The callback to execute when we press the 'Play' button
        /// </summary>
        /// <param name="image">The image that was clicked</param>
        private void OnNewGameButtonLeftClicked(IClickable image)
        {
            // Need to load assets before we transition to the next screen
            PlayerCardRegistry.Instance.LoadAssets(PlayerCardRegistry.startingCardRegistryDataAsset);
            Transition(new LobbyMenuScreen());
        }

        /// <summary>
        /// The callback to execute when we press the 'Continue' button
        /// </summary>
        /// <param name="image"></param>
        private void OnContinueButtonLeftClicked(IClickable image)
        {
            // Need to load assets before we transition to the next screen
            PlayerCardRegistry.Instance.LoadAssets(PlayerCardRegistry.playerCardRegistryDataAsset);
            Transition(new LobbyMenuScreen());
        }

        /// <summary>
        /// The callback to execute when we press the 'Options' button
        /// </summary>
        /// <param name="image">The image that was clicked</param>
        private void OnOptionsButtonClicked(IClickable image)
        {
            Transition(new GameOptionsScreen());
        }

        /// <summary>
        /// The callback to execute when we press the 'Exit' button
        /// </summary>
        /// <param name="image">Unused</param>
        private void OnExitGameButtonClicked(IClickable image)
        {
            ScreenManager.Instance.EndGame();
        }

        #endregion
    }
}
