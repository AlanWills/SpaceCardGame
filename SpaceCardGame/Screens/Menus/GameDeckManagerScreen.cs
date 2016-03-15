using _2DEngine;
using CardGameEngine;
using System.Collections.Generic;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A wrapper class to allow us to traverse back to the main menu screen
    /// </summary>
    public class GameDeckManagerScreen : DeckManagerScreen
    {
        #region Virtual Functions

        /// <summary>
        /// Set up our screen transition functions
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            List<DeckSlotUI> deckSlotUIs = ScreenUIObjects.GetObjectsOfType<DeckSlotUI>();
            Debug.Assert(deckSlotUIs.Count > 0);

            foreach (DeckSlotUI deckSlotUI in deckSlotUIs)
            {
                deckSlotUI.EditButton.OnLeftClicked += EditButton_OnLeftClicked;
            }
        }

        /// <summary>
        /// Transition back to our main menu screen
        /// </summary>
        protected override void GoToPreviousScreen()
        {
            Transition(new LobbyMenuScreen());
        }

        /// <summary>
        /// Transitions to a new deck editing screen
        /// </summary>
        /// <param name="image"></param>
        private void EditButton_OnLeftClicked(IClickable image)
        {
            Debug.Assert(image is Button);
            Button editButton = (image as Button);

            DebugUtils.AssertNotNull(editButton.GetParent());
            Debug.Assert(editButton.GetParent() is DeckSlotUI);
            DeckSlotUI deckSlotUI = editButton.GetParent() as DeckSlotUI;

            DebugUtils.AssertNotNull(deckSlotUI.StoredObject);
            Debug.Assert(deckSlotUI.StoredObject is Deck);
            Deck deck = deckSlotUI.StoredObject as Deck;

            DebugUtils.AssertNotNull(deck);

            ScreenManager.Instance.Transition(new GameDeckEditingScreen(deck));
        }

        #endregion
    }
}
