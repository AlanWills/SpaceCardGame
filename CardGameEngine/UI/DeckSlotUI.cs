using _2DEngine;
using Microsoft.Xna.Framework;
using System.Diagnostics;

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
        /// A button displaying the deck's name; hidden if no deck exists
        /// </summary>
        private Button DeckNameButton { get; set; }

        /// <summary>
        /// A button for creating a deck for this thumbnail.  
        /// Disabled if we have a valid deck, enabled otherwise
        /// </summary>
        private Button CreateButton { get; set; }

        /// <summary>
        /// A button for editing the cards in a deck.
        /// Disabled if we have no deck, enabled otherwise
        /// </summary>
        public Button EditButton { get; private set; }

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

            DeckNameButton = AddObject(new Button(Deck.Name, new Vector2(0, -(Size.Y * 0.5f + padding))), true, true);
            DeckNameButton.OnLeftClicked += DeckNameButton_OnLeftClicked;

            CreateButton = AddObject(new Button("Create", new Vector2(0, Size.Y * 0.5f + padding)), true, true);
            CreateButton.OnLeftClicked += CreateButton_OnLeftClicked;

            // The edit button is parented to this so we can access this DeckSlotUI from an event callback
            EditButton = AddObject(new Button("Edit", new Vector2(0, CreateButton.LocalPosition.Y + CreateButton.Size.Y + padding)), true, true);
            EditButton.SetParent(this, true);

            DeleteButton = AddObject(new Button("Delete", new Vector2(0, EditButton.Size.Y + padding)), true, true);
            DeleteButton.OnLeftClicked += DeleteButton_OnLeftClicked;
            DeleteButton.SetParent(EditButton, true);

            UpdateUIStatus();
        }

        #endregion

        #region Event Callbacks

        /// <summary>
        /// Brings up a dialog box for editing the deck's name
        /// </summary>
        /// <param name="clickable">The button which fires this event</param>
        private void DeckNameButton_OnLeftClicked(IClickable clickable)
        {
            TextEntryBox deckName = ScreenManager.Instance.CurrentScreen.AddScreenUIObject(new TextEntryBox(Deck.Name, "Deck Name", ScreenManager.Instance.ScreenCentre), true, true);
            TextEntryScript deckNameEntryScript = ScriptManager.Instance.AddObject(new TextEntryScript(deckName), true, true);
            deckName.ConfirmButton.OnLeftClicked += EnterDeckName;
        }

        /// <summary>
        /// Creates a deck and updates UI
        /// </summary>
        /// <param name="image"></param>
        private void CreateButton_OnLeftClicked(IClickable image)
        {
            TextEntryBox deckName = ScreenManager.Instance.CurrentScreen.AddScreenUIObject(new TextEntryBox(Deck.Name, "Deck Name", ScreenManager.Instance.ScreenCentre), true, true);
            TextEntryScript deckNameEntryScript = ScriptManager.Instance.AddObject(new TextEntryScript(deckName), true, true);
            deckName.ConfirmButton.OnLeftClicked += EnterDeckName;

            Deck.Create();

            UpdateUIStatus();
        }

        /// <summary>
        /// Deletes a deck
        /// </summary>
        /// <param name="image"></param>
        private void DeleteButton_OnLeftClicked(IClickable image)
        {
            UpdateUIStatus();
        }

        /// <summary>
        /// Enters the name for the deck.
        /// </summary>
        /// <param name="image"></param>
        private void EnterDeckName(IClickable clickable)
        {
            Debug.Assert(clickable is Button);
            Button button = clickable as Button;

            Debug.Assert(button.GetParent() is TextEntryBox);

            TextEntryBox dialogBox = button.GetParent() as TextEntryBox;
            Deck.Name = dialogBox.Text;
            DeckNameButton.Label.Text = Deck.Name;
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
                DeckNameButton.Enable();
                CreateButton.Disable();
                EditButton.Enable();
                DeleteButton.Enable();
            }
            else
            {
                DeckNameButton.Disable();
                CreateButton.Enable();
                EditButton.Disable();
                DeleteButton.Disable();
            }
        }

        #endregion
    }
}