using _2DEngine;
using Microsoft.Xna.Framework;

namespace CardGameEngine
{
    /// <summary>
    /// A screen for managing the player's current decks
    /// </summary>
    public abstract class DeckManagerScreen : MenuScreen
    {
        #region Properties and Fields

        #endregion

        public DeckManagerScreen(string screenDataAsset = "Screens\\DeckManagerScreen.xml") :
            base(screenDataAsset)
        {
            
        }

        #region Virtual Functions

        protected override void AddInitialUI()
        {
            base.AddInitialUI();

            // We fake the UI by pretending there are six columns but with the centre of the first and last at 0 and ScreenWidth
            int columns = 4;
            Vector2 padding = ScreenDimensions * 0.05f;
            Vector2 slotSize = new Vector2(200, 300);

            for (int i = 0; i < PlayerCardRegistry.maxDeckNumber; ++i)
            {
                int row = i / columns;
                int column = (i % columns) + 1;

                Vector2 position = new Vector2(column * ScreenDimensions.X / (columns + 2), ScreenDimensions.Y * (0.2f + row * 0.5f));

                DebugUtils.AssertNotNull(PlayerCardRegistry.Instance.Decks[i]);
                DeckSlotUI deckSlotUI = AddScreenUIObject(new DeckSlotUI(PlayerCardRegistry.Instance.Decks[i], slotSize, position));
                deckSlotUI.StoredObject = PlayerCardRegistry.Instance.Decks[i];
            }
        }

        #endregion
    }
}