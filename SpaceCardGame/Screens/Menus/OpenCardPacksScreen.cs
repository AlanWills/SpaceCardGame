using _2DEngine;

namespace SpaceCardGame
{
    /// <summary>
    /// A screen where the player opens packs which they have earnt.
    /// Not really anything fancy in terms of gameplay; just fancy UI.
    /// </summary>
    public class OpenCardPacksScreen : MenuScreen
    {
        public OpenCardPacksScreen(string screenDataAsset = "Content\\Data\\Screens\\OpenCardPacksScreen.xml") :
            base(screenDataAsset)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// Transition back to our lobby screen if we press esc
        /// </summary>
        protected override void GoToPreviousScreen()
        {
            Transition(new LobbyMenuScreen());
        }

        #endregion
    }
}
