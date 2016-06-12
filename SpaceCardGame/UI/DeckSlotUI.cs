using _2DEngine;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A class containing UI used in our DeckManager screen for a deck slot.
    /// Represents a thumbnail indicating whether our deck has been created and contains buttons to create/edit/delete where appropriate.
    /// </summary>
    public class DeckSlotUI : UIObject
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
            UsesCollider = false;
        }

        #region Virtual Functions

        /// <summary>
        /// Adds all the buttons we use to interact with our deck
        /// </summary>
        public override void Initialise()
        {
            CheckShouldInitialise();

            base.Initialise();

            // Use this to change the initial state of our UI
            float padding = 5;

            Vector2 buttonSize = new Vector2(Size.X, Size.Y * 0.1f);

            DeckNameButton = AddChild(new Button(Deck.Name, buttonSize, new Vector2(0, -((Size.Y - buttonSize.Y) * 0.5f - padding)), AssetManager.DefaultNarrowButtonTextureAsset, AssetManager.DefaultNarrowButtonHighlightedTextureAsset), true, true);
            DeckNameButton.ClickableModule.OnLeftClicked += DeckNameButton_OnLeftClicked;

            CreateButton = AddChild(new Button("Create", buttonSize, new Vector2(0, Size.Y * 0.5f - padding - buttonSize.Y), AssetManager.DefaultNarrowButtonTextureAsset, AssetManager.DefaultNarrowButtonHighlightedTextureAsset), true, true);
            CreateButton.ClickableModule.OnLeftClicked += CreateButton_OnLeftClicked;

            // The edit button is parented to this so we can access this DeckSlotUI from an event callback
            EditButton = AddChild(new Button("Edit", buttonSize, new Vector2(0, CreateButton.LocalPosition.Y + buttonSize.Y + padding), AssetManager.DefaultNarrowButtonTextureAsset, AssetManager.DefaultNarrowButtonHighlightedTextureAsset), true, true);

            DeleteButton = EditButton.AddChild(new Button("Delete", buttonSize, new Vector2(0, EditButton.Size.Y + padding), AssetManager.DefaultNarrowButtonTextureAsset, AssetManager.DefaultNarrowButtonHighlightedTextureAsset), true, true);
            DeleteButton.ClickableModule.OnLeftClicked += DeleteButton_OnLeftClicked;

            UpdateUIStatus();
        }

        #endregion

        #region Event Callbacks

        /// <summary>
        /// Brings up a dialog box for editing the deck's name
        /// </summary>
        /// <param name="baseObject">The button which fires this event</param>
        private void DeckNameButton_OnLeftClicked(BaseObject baseObject)
        {
            TextEntryBox deckName = ScreenManager.Instance.CurrentScreen.AddScreenUIObject(new TextEntryBox(Deck.Name, "Deck Name", ScreenManager.Instance.ScreenCentre), true, true);
            TextEntryCommand deckNameEntryScript = CommandManager.Instance.AddChild(new TextEntryCommand(deckName), true, true);
            deckName.ConfirmButton.ClickableModule.OnLeftClicked += EnterDeckName;
        }

        /// <summary>
        /// Creates a deck and updates UI
        /// </summary>
        /// <param name="image"></param>
        private void CreateButton_OnLeftClicked(BaseObject baseObject)
        {
            TextEntryBox deckName = ScreenManager.Instance.CurrentScreen.AddScreenUIObject(new TextEntryBox(Deck.Name, "Deck Name", ScreenManager.Instance.ScreenCentre), true, true);
            TextEntryCommand deckNameEntryScript = CommandManager.Instance.AddChild(new TextEntryCommand(deckName), true, true);
            deckName.ConfirmButton.ClickableModule.OnLeftClicked += EnterDeckName;

            Deck.Create();

            UpdateUIStatus();
        }

        /// <summary>
        /// Deletes a deck
        /// </summary>
        /// <param name="baseObject"></param>
        private void DeleteButton_OnLeftClicked(BaseObject baseObject)
        {
            // Add a dialog box here and then properly delete it
            Deck.Delete();

            UpdateUIStatus();
        }

        /// <summary>
        /// Enters the name for the deck.
        /// </summary>
        /// <param name="baseObject"></param>
        private void EnterDeckName(BaseObject baseObject)
        {
            Debug.Assert(baseObject is Button);
            Button button = baseObject as Button;

            Debug.Assert(button.Parent is TextEntryBox);

            TextEntryBox dialogBox = button.Parent as TextEntryBox;
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