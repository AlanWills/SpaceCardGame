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

            float tabControlHeight = ScreenDimensions.Y * 0.035f;

            TabControl tabControl = AddScreenUIObject(new TabControl(new Vector2(ScreenDimensions.X, tabControlHeight), new Vector2(ScreenCentre.X, tabControlHeight * 0.5f))) as TabControl;

            // Add a DeckCardTypeControl for Resource type
            tabControl.AddObject(new DeckCardTypeControl(Deck, "Resource", new Vector2(ScreenDimensions.X, ScreenDimensions.Y - tabControlHeight), new Vector2(0, ScreenCentre.Y)));
        }

        #endregion
    }
}
