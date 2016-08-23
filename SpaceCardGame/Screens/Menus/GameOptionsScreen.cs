using CelesteEngine;

namespace SpaceCardGame
{
    /// <summary>
    /// An override to enable us to go back to previous screens
    /// </summary>
    public class GameOptionsScreen : OptionsScreen
    {
        #region Virtual Functions

        /// <summary>
        /// Go back to our main menu screen
        /// </summary>
        protected override void GoToPreviousScreen()
        {
            Transition(new MainMenuScreen());
        }

        #endregion
    }
}
