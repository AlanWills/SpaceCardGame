using _2DEngine;
using Microsoft.Xna.Framework;

namespace CardGameEngine
{
    public abstract class DeckEditingScreen : MenuScreen
    {
        #region Properties and Fields

        /// <summary>
        /// The deck we will be editing.
        /// </summary>
        private Deck Deck { get; set; }

        #endregion

        public DeckEditingScreen(Deck deck, string dataAsset = "Screens\\DeckEditingScreen.xml") :
            base(dataAsset)
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

            TabControl tabControl = AddScreenUIObject(new TabControl(new Vector2(ScreenDimensions.X, tabControlHeight), new Vector2(ScreenCentre.X, tabControlHeight * 0.5f)));

            // Add a DeckCardTypeControl for each resource type
            foreach (string cardType in CentralCardRegistry.CardTypes)
            {
                tabControl.AddChild(new DeckCardTypeControl(Deck, cardType, new Vector2(ScreenDimensions.X, ScreenDimensions.Y - tabControlHeight), new Vector2(0, ScreenCentre.Y)));
            }
        }

        #endregion
    }
}
