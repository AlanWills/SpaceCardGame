using _2DEngine;
using Microsoft.Xna.Framework;

namespace CardGameEngine
{
   public class DeckEditingScreen : MenuScreen
    {
        #region Properties and Fields

        /// <summary>
        /// The deck we will be editing.
        /// </summary>
        private Deck Deck { get; set; }

        #endregion

        public DeckEditingScreen(Deck deck, MenuScreen previousMenuScreen, string dataAsset = "Content\\Data\\Screens\\DeckEditingScreen.xml") :
            base(previousMenuScreen, dataAsset)
        {
            Deck = deck;
        }

        #region Virtual Functions

        /// <summary>
        /// Adds the UI for editing our deck
        /// </summary>
        protected override void AddInitialUI()
        {
            base.AddInitialUI();

            foreach (string cardAsset in PlayerCardRegistry.Instance.AvailableCards)
            {
                Deck.Add(CentralCardRegistry.CardData[cardAsset]);
            }

            AddScreenUIObject(new DeckCardListControl(Deck, 4, new Vector2(ScreenDimensions.X * 0.25f, ScreenDimensions.Y), new Vector2(ScreenDimensions.X * 0.875f, ScreenCentre.Y)));
        }

        #endregion
    }
}
