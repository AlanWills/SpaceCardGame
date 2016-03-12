using _2DEngine;
using Microsoft.Xna.Framework;

namespace CardGameEngine
{
    /// <summary>
    /// A class containing UI used in our DeckManager screen for a deck slot.
    /// Represents a thumbnail indicating whether our deck has been created and contains buttons to create/edit/delete where appropriate.
    /// </summary>
    public class DeckSlotUI : UIObjectContainer
    {
        #region Properties and Fields

        /// <summary>
        /// A label displaying the deck's name, or empty if no deck exists
        /// </summary>
        private Label DeckName { get; set; }

        /// <summary>
        /// A button for creating a deck for this thumbnail.  
        /// Disabled if we have a valid deck, enabled otherwise
        /// </summary>
        private Button CreateButton { get; set; }

        /// <summary>
        /// A button for editing the cards in a deck.
        /// Disabled if we have no deck, enabled otherwise
        /// </summary>
        private Button EditButton { get; set; }

        /// <summary>
        /// A button for deleting a deck.
        /// Disabled if we have no deck, enabled otherwise
        /// </summary>
        private Button DeleteButton { get; set; }

        /// <summary>
        /// The Deck in our PlayerCardRegistry this slot represents (could be initially null).
        /// </summary>
        private Deck Deck { get; set; }

        #endregion

        public DeckSlotUI(Deck deck, Vector2 localPosition, string textureAsset = Card.CardBackTextureAsset) :
            this(deck, Vector2.Zero, localPosition, textureAsset)
        {
            
        }

        public DeckSlotUI(Deck deck, Vector2 size, Vector2 localPosition, string textureAsset = Card.CardBackTextureAsset) :
            base(size, localPosition, textureAsset)
        {
            Deck = deck;
        }

        #region Virtual Functions

        public override void Initialise()
        {
            CheckShouldInitialise();

            base.Initialise();

            // Use this to change the initial state of our UI
            float padding = 5;

            string deckName = Deck.IsCreated ? Deck.Name : "";
            DeckName = AddObject(new Label(deckName, new Vector2(0, -(Size.Y * 0.5f + padding))), true, true) as Label;

            CreateButton = AddObject(new Button("Create", new Vector2(0, Size.Y * 0.5f + padding)), true, true) as Button;
            CreateButton.OnLeftClicked += CreateButton_OnLeftClicked;

            EditButton = AddObject(new Button("Edit", new Vector2(0, CreateButton.Size.Y + padding)), true, true) as Button;
            EditButton.OnLeftClicked += EditButton_OnLeftClicked;
            EditButton.SetParent(CreateButton, true);

            DeleteButton = AddObject(new Button("Delete", new Vector2(0, EditButton.Size.Y + padding)), true, true) as Button;
            DeleteButton.OnLeftClicked += DeleteButton_OnLeftClicked;
            DeleteButton.SetParent(EditButton, true);

            if (Deck.IsCreated)
            {
                // If we have a deck then disable our create button - a deck already exists!
                CreateButton.Disable();
            }
            else
            {
                // If we have no deck, disable our edit and delete buttons - we have nothing to edit or delete!
                EditButton.Disable();
                DeleteButton.Disable();
            }
        }

        #endregion

        #region Event Callbacks

        /// <summary>
        /// Creates a deck and updates UI
        /// </summary>
        /// <param name="image"></param>
        private void CreateButton_OnLeftClicked(IClickable image)
        {
            TextEntryDialogBox deckName = ScreenManager.Instance.CurrentScreen.AddScreenUIObject(new TextEntryDialogBox(Deck.Name, "Deck Name", ScreenManager.Instance.ScreenCentre), true, true) as TextEntryDialogBox;
            TextEntryScript deckNameEntryScript = ScriptManager.Instance.AddObject(new TextEntryScript(deckName), true, true) as TextEntryScript;

            Deck.Create();

            UpdateUIStatus();
        }

        /// <summary>
        /// Transitions to a new deck editting screen
        /// </summary>
        /// <param name="image"></param>
        private void EditButton_OnLeftClicked(IClickable image)
        {
            DebugUtils.AssertNotNull(Deck);
            DebugUtils.AssertNotNull(ScreenManager.Instance.CurrentScreen as MenuScreen);

            ScreenManager.Instance.Transition(new DeckEditingScreen(Deck, ScreenManager.Instance.CurrentScreen as MenuScreen));
        }

        /// <summary>
        /// Deletes a deck
        /// </summary>
        /// <param name="image"></param>
        private void DeleteButton_OnLeftClicked(IClickable image)
        {
            UpdateUIStatus();
        }

        #endregion

        #region Utility Function

        /// <summary>
        /// Updates each element of our UI based on whether the deck has been created or not.
        /// </summary>
        private void UpdateUIStatus()
        {
            if (Deck.IsCreated)
            {
                CreateButton.Disable();
                EditButton.Enable();
                DeleteButton.Enable();
            }
            else
            {
                CreateButton.Enable();
                EditButton.Disable();
                DeleteButton.Disable();
            }
        }

        #endregion
    }
}