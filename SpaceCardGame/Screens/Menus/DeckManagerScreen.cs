using _2DEngine;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A wrapper class to allow us to traverse back to the main menu screen
    /// </summary>
    public class DeckManagerScreen : MenuScreen
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

        /// <summary>
        /// Set up our screen transition functions
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            Debug.Assert(DeckSlotUIGridControl.ChildrenCount > 0);

            foreach (DeckSlotUI deckSlotUI in DeckSlotUIGridControl)
            {
                deckSlotUI.EditButton.ClickableModule.OnLeftClicked += EditButton_OnLeftClicked;
            }
        }

        /// <summary>
        /// Transition back to our main menu screen
        /// </summary>
        protected override void GoToPreviousScreen()
        {
            PlayerCardRegistry.Instance.SaveAssets();
            Transition(new LobbyMenuScreen());
        }

        /// <summary>
        /// Transitions to a new deck editing screen
        /// </summary>
        /// <param name="baseObject"></param>
        private void EditButton_OnLeftClicked(BaseObject baseObject)
        {
            Debug.Assert(baseObject is Button);
            Button editButton = (baseObject as Button);

            DebugUtils.AssertNotNull(editButton.Parent);
            Debug.Assert(editButton.Parent is DeckSlotUI);
            DeckSlotUI deckSlotUI = editButton.Parent as DeckSlotUI;

            DebugUtils.AssertNotNull(deckSlotUI.StoredObject);
            Debug.Assert(deckSlotUI.StoredObject is Deck);
            Deck deck = deckSlotUI.StoredObject as Deck;

            DebugUtils.AssertNotNull(deck);

            ScreenManager.Instance.Transition(new DeckEditingScreen(deck));
        }

        #endregion
    }
}
