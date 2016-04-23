using CardGameEngine;

namespace SpaceCardGame
{
    /// <summary>
    /// A wrapper class to allow us to transition between screens
    /// </summary>
    public class GameDeckEditingScreen : DeckEditingScreen
    {
        public GameDeckEditingScreen(Deck deck, string dataAsset = "Screens\\DeckEditingScreen.xml") :
            base(deck, dataAsset)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// Want to return to our deck manager screen
        /// </summary>
        /// <returns></returns>
        protected override void GoToPreviousScreen()
        {
            PlayerCardRegistry.Instance.SaveAssets();
            Transition(new GameDeckManagerScreen());
        }

        #endregion
    }
}
