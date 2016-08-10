using _2DEngine;
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

            // When not in debug, we should only have 1 rows of four buttons, but when in debug we will need 2 rows
            int rows = 1;
#if DEBUG
            rows++;
#endif

            GridControl buttonControl = AddScreenUIObject(new GridControl(rows, 4, ScreenCentre));

            Button newGameButton = buttonControl.AddChild(new Button("New Game", Vector2.Zero));
            newGameButton.ClickableModule.OnLeftClicked += OnNewGameButtonLeftClicked;

            Button continueGameButton = buttonControl.AddChild(new Button("Continue", Vector2.Zero));
            continueGameButton.ClickableModule.OnLeftClicked += OnContinueButtonLeftClicked;

            Button optionsButton = buttonControl.AddChild(new Button("Options", Vector2.Zero));
            optionsButton.ClickableModule.OnLeftClicked += OnOptionsButtonClicked;

#if DEBUG
            // If in debug add the hardpoint screen option
            Button hardpointButton = buttonControl.AddChild(new Button("Hardpoint Calculator", Vector2.Zero));
            hardpointButton.ClickableModule.OnLeftClicked += OnHardpointButtonClicked;
#endif

            Button exitGameButton = buttonControl.AddChild(new Button("Exit", Vector2.Zero));
            exitGameButton.ClickableModule.OnLeftClicked += OnExitGameButtonClicked;
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
            PlayerDataRegistry.Instance.LoadAssets(PlayerDataRegistry.startingCardRegistryDataAsset);

            // Reset the player's current level to 1
            PlayerDataRegistry.Instance.PlayerData.CurrentLevel = 1;
            Transition(new LobbyMenuScreen());
        }

        /// <summary>
        /// The callback to execute when we press the 'Continue' button
        /// </summary>
        /// <param name="baseObject"></param>
        private void OnContinueButtonLeftClicked(BaseObject baseObject)
        {
            // Need to load assets before we transition to the next screen
            PlayerDataRegistry.Instance.LoadAssets(PlayerDataRegistry.playerCardRegistryDataAsset);
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

#if DEBUG

        /// <summary>
        /// The callback to execute where we transition to the hardpoint screen
        /// </summary>
        /// <param name="baseObject"></param>
        private void OnHardpointButtonClicked(BaseObject baseObject)
        {
            Transition(new HardpointScreen());
        }

#endif

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
