using _2DEngine;
using CardGameEngine;
using Microsoft.Xna.Framework;
using System.IO;

namespace SpaceCardGame
{
    public class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen(string screenDataAsset = "Screens\\MainMenuScreen.xml") :
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
            newGameButton.ClickableModule.OnLeftClicked += OnNewGameButtonLeftClicked;
            parent = newGameButton;

            Button continueGameButton = parent.AddChild(new Button("Continue", new Vector2(0, padding)));
            continueGameButton.ClickableModule.OnLeftClicked += OnContinueButtonLeftClicked;
            parent = continueGameButton;

            // Disable the continue button if we have no saved data file
            if (!File.Exists(ScreenManager.Instance.Content.RootDirectory + "\\Data\\" + PlayerCardRegistry.playerCardRegistryDataAsset))
            {
                continueGameButton.Disable();
            }

            Button optionsButton = parent.AddChild(new Button("Options", new Vector2(0, padding)));
            optionsButton.ClickableModule.OnLeftClicked += OnOptionsButtonClicked;
            parent = optionsButton;

            Button exitGameButton = parent.AddChild(new Button("Exit", new Vector2(0, padding)));
            exitGameButton.ClickableModule.OnLeftClicked += OnExitGameButtonClicked;
            parent = exitGameButton;
        }

        #endregion

        #region Event callbacks for main menu screen buttons

        /// <summary>
        /// The callback to execute when we press the 'Play' button
        /// </summary>
        /// <param name="baseObject">The baseObject that was clicked</param>
        private void OnNewGameButtonLeftClicked(BaseObject baseObject)
        {
            // Need to load assets before we transition to the next screen
            PlayerCardRegistry.Instance.LoadAssets(PlayerCardRegistry.startingCardRegistryDataAsset);
            Transition(new LobbyMenuScreen());
        }

        /// <summary>
        /// The callback to execute when we press the 'Continue' button
        /// </summary>
        /// <param name="baseObject"></param>
        private void OnContinueButtonLeftClicked(BaseObject baseObject)
        {
            // Need to load assets before we transition to the next screen
            PlayerCardRegistry.Instance.LoadAssets(PlayerCardRegistry.playerCardRegistryDataAsset);
            Transition(new LobbyMenuScreen());
        }

        /// <summary>
        /// The callback to execute when we press the 'Options' button
        /// </summary>
        /// <param name="baseObject">The image that was clicked</param>
        private void OnOptionsButtonClicked(BaseObject baseObject)
        {
            Transition(new GameOptionsScreen());
        }

        /// <summary>
        /// The callback to execute when we press the 'Exit' button
        /// </summary>
        /// <param name="baseObject">Unused</param>
        private void OnExitGameButtonClicked(BaseObject baseObject)
        {
            ScreenManager.Instance.EndGame();
        }

        #endregion
    }
}
