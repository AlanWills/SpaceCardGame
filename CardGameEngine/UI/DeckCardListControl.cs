using _2DEngine;
using CardGameEngineData;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace CardGameEngine
{
    /// <summary>
    /// A list control which displays images of the cards in our deck sorted by the order they were added to the deck.
    /// </summary>
    public class DeckCardListControl : ListControl
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the deck this list control is representing.
        /// </summary>
        private Deck Deck { get; set; }

        /// <summary>
        /// A counter for the number of elements in this control
        /// </summary>
        private int ElementCount { get; set; }

        // Private fields just to help for UI calculations
        private Vector2 elementSize;
        private int Columns { get; set; }
        //ClickableImage previousCard;

        #endregion

        public DeckCardListControl(Deck deck, int columns, Vector2 localPosition, string textureAsset = AssetManager.DefaultMenuTextureAsset) :
            this(deck, columns, Vector2.Zero, localPosition, textureAsset)
        {

        }

        public DeckCardListControl(Deck deck, int columns, Vector2 size, Vector2 localPosition, string textureAsset = AssetManager.DefaultMenuTextureAsset) :
            base(size, localPosition, textureAsset)
        {
            Deck = deck;
            Columns = columns;
        }

        #region Properties and Fields

        /// <summary>
        /// Add the current cards in the deck to this list control
        /// </summary>
        public override void Initialise()
        {
            CheckShouldInitialise();

            base.Initialise();

            // Use the x dimension because y will be whole screen height.
            // Assume we have square elements and let the image code deal with the rest
            elementSize = new Vector2(Size.X / Columns, Size.X / Columns);

            foreach (CardData cardData in Deck)
            {
                AddCard(cardData, false);
            }
        }

        /// <summary>
        /// Rebuild our UI for our newly added card images
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            // Rebuild our UI when we add our objects in Begin()
            RebuildList();
        }

#endregion

#region Utility Functions

        /// <summary>
        /// A utility function for adding a card to this list control after the last one.
        /// </summary>
        /// <param name="cardData">The data of the card</param>
        /// <param name="rebuild">A flag we can use to skip rebuilding the UI</param>
        public void AddCard(CardData cardData, bool rebuild = true)
        {
            // Calculate the next position for our card image based on the number of elements in our list
            ElementCount++;

            // Maybe don't do this? Maybe do if it's horrendous
            /*Vector2 position = new Vector2();

            if (previousCard == null)
            {
                // If we are adding our first card, do the position manually
                position = new Vector2((-Size.X + elementSize.X) * 0.5f, (-Size.Y + elementSize.Y) * 0.5f);
            }
            else
            {
                // Add relative to the position of the last card
                int column = (ElementCount - 1) % Columns;
                if (column == 0)
                {
                    // We add to new row in first column
                    position = new Vector2((-Size.X + elementSize.X) * 0.5f, previousCard.LocalPosition.Y + elementSize.Y);
                }
                else
                {
                    // Add to next column along on same row
                    position = previousCard.LocalPosition + new Vector2(elementSize.X, 0);
                }

                // Check we are indeed creating in a different place
                Debug.Assert(position != previousCard.LocalPosition);
            }
            
            previousCard = AddObject(new ClickableImage(elementSize, position, cardData.TextureAsset), true, true) as ClickableImage;
            previousCard.StoredObject = cardData;
            previousCard.OnRightClicked += CardImage_OnRightClicked;*/

            ClickableImage image = AddObject(new ClickableImage(elementSize, Vector2.Zero, cardData.TextureAsset), true, true) as ClickableImage;
            image.StoredObject = cardData;
            image.OnRightClicked += CardImage_OnRightClicked;

            if (rebuild)
            {
                RebuildList();
            }
        }

        /// <summary>
        /// Loops through all the alive elements in this container and updates their positions based on their order.
        /// Gets more costly the more elements we have so should not be done lightly.
        /// </summary>
        private void RebuildList()
        {
            UIObject previous = null;

            foreach (UIObject uiObject in this)
            {
                // If our object is dead it will be cleared up so should have it's position recalculated
                if (!uiObject.IsAlive.Value) { continue; }

                if (previous == null)
                {
                    uiObject.LocalPosition = new Vector2((-Size.X + elementSize.X) * 0.5f, (-Size.Y + elementSize.Y) * 0.5f);
                }
                else
                {
                    // Add relative to the position of the last card
                    int column = (ElementCount - 1) % Columns;
                    if (column == 0)
                    {
                        // We add to new row in first column
                        uiObject.LocalPosition = new Vector2((-Size.X + elementSize.X) * 0.5f, previous.LocalPosition.Y + elementSize.Y);
                    }
                    else
                    {
                        // Add to next column along on same row
                        uiObject.LocalPosition = previous.LocalPosition + new Vector2(elementSize.X, 0);
                    }

                    // Check we are indeed creating in a different place
                    Debug.Assert(uiObject.LocalPosition != previous.LocalPosition);
                }

                previous = uiObject;
            }
        }

#endregion

#region Card Image Callbacks

        /// <summary>
        /// The function to call when one of our cards is clicked on - removes it from the deck and this UI and rebuilds our list.
        /// </summary>
        /// <param name="image">The image we clicked on</param>
        private void CardImage_OnRightClicked(IClickable clickable)
        {
            UIObject image = clickable as UIObject;
            DebugUtils.AssertNotNull(image);

            DebugUtils.AssertNotNull(image.StoredObject);
            CardData cardData = image.StoredObject as CardData;
            DebugUtils.AssertNotNull(cardData);

            Deck.Remove(cardData);

            image.Die();

            RebuildList();
        }

#endregion
    }
}
