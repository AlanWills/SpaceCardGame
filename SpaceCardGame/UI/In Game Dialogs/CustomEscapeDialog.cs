using CelesteEngine;

namespace SpaceCardGame
{
    /// <summary>
    /// The custom escape dialog for our game which will implement going to the Lobby Screen
    /// </summary>
    public class CustomEscapeDialog : InGameEscapeDialog
    {
        #region Virtual Functions

        /// <summary>
        /// When the quit to lobbby button is clicked, we quit to the lobby screen.
        /// </summary>
        /// <param name="clickedObject"></param>
        protected override void QuitToLobby(BaseObject clickedObject)
        {
            ScreenManager.Instance.Transition(new LobbyMenuScreen());
        }

        #endregion
    }
}