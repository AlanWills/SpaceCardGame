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

        /// <summary>
        /// The grid control which holds our DeckSlotUI
        /// </summary>
        protected GridControl DeckSlotUIGridControl { get; private set; }

        #endregion

        public DeckManagerScreen(string screenDataAsset = "Screens\\DeckManagerScreen.xml") :
            base(screenDataAsset)
        {
            
        }

        #region Virtual Functions

        /// <summary>
        /// Adds our DeckSlotUI to a grid control in our screen
        /// </summary>
        protected override void AddInitialUI()
        {
            base.AddInitialUI();

            Vector2 slotSize = new Vector2(200, 300);

            DeckSlotUIGridControl = AddScreenUIObject(new GridControl(2, 4, ScreenDimensions, ScreenCentre));
            DeckSlotUIGridControl.BorderPadding = new Vector2(ScreenDimensions.X * 0.1f, ScreenDimensions.Y * 0.05f);

            for (int i = 0; i < PlayerCardRegistry.maxDeckNumber; ++i)
            {
                DebugUtils.AssertNotNull(PlayerCardRegistry.Instance.Decks[i]);
                DeckSlotUI deckSlotUI = DeckSlotUIGridControl.AddChild(new DeckSlotUI(PlayerCardRegistry.Instance.Decks[i], slotSize, Vector2.Zero));
                deckSlotUI.StoredObject = PlayerCardRegistry.Instance.Decks[i];
            }
        }

        #endregion
    }
}