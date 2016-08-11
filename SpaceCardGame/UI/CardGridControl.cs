using _2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace SpaceCardGame
{
    /// <summary>
    /// A list control which displays images of the inputted cards
    /// </summary>
    public class CardGridControl : GridControl, IClickable
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the list of cards this control is representing (could be a deck).
        /// </summary>
        private List<CardData> CardList { get; set; }

        /// <summary>
        /// A predicate which can be used to indicate whether a card should be added or not.
        /// </summary>
        public Predicate<CardData> IncludePredicate { get; set; }

        /// <summary>
        /// Optional keyboard keys to trigger the click events if set
        /// </summary>
        public Keys LeftClickAccelerator { get; set; }
        public Keys MiddleClickAccelerator { get; set; }
        public Keys RightClickAccelerator { get; set; }

        // Events we will use for our cards when clicked on
        public event OnClicked OnLeftClicked;
        public event OnClicked OnMiddleClicked;
        public event OnClicked OnRightClicked;

        // Unused
        public ClickState ClickState { get { throw new NotImplementedException("Not used you walnut"); } }

        #endregion

        #region Constructors

        public CardGridControl(List<CardData> cardList, int rows, int columns, Vector2 localPosition, string textureAsset = AssetManager.DefaultMenuTextureAsset) :
            this(cardList, rows, columns, Vector2.Zero, localPosition, textureAsset)
        {

        }

        public CardGridControl(List<CardData> cardList, int rows, int columns, Vector2 size, Vector2 localPosition, string textureAsset = AssetManager.DefaultMenuTextureAsset) :
            base(rows, columns, size, localPosition, textureAsset)
        {
            CardList = cardList;
            
        }

        #endregion

        #region Properties and Fields

        /// <summary>
        /// Add the current cards in the list to this list control
        /// </summary>
        public override void Initialise()
        {
            CheckShouldInitialise();

            base.Initialise();

            DebugUtils.AssertNotNull(IncludePredicate);
            List<CardData> cardsToAdd = CardList.FindAll(IncludePredicate);

            foreach (CardData cardData in cardsToAdd)
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
            NeedsRebuild = true;
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

            ClickableImage image = AddChild(new ClickableImage(ElementSize, Vector2.Zero, cardData.TextureAsset), true, true);
            image.StoredObject = cardData;

            // These are probably not going to be used, but set them up anyway
            image.ClickableModule.LeftClickAccelerator = LeftClickAccelerator;
            image.ClickableModule.MiddleClickAccelerator = MiddleClickAccelerator;
            image.ClickableModule.RightClickAccelerator = RightClickAccelerator;

            // Add the click callbacks if they exist
            if (OnLeftClicked != null)
            {
                image.ClickableModule.OnLeftClicked += OnLeftClicked;
                image.ClickableModule.OnLeftClicked += Rebuild_Callback;
            }

            if (OnMiddleClicked != null)
            {
                image.ClickableModule.OnMiddleClicked += OnMiddleClicked;
                image.ClickableModule.OnMiddleClicked += Rebuild_Callback;
            }

            if (OnRightClicked != null)
            {
                image.ClickableModule.OnRightClicked += OnRightClicked;
                image.ClickableModule.OnRightClicked += Rebuild_Callback;
            }

            // If we have already been marked as needing rebuild, we should do it already, otherwise only do it if indicated
            NeedsRebuild = NeedsRebuild || rebuild;
        }

        #endregion

        #region Card Image Callbacks

        /// <summary>
        /// A callback we will always add if we add a click image
        /// </summary>
        /// <param name="clickable"></param>
        private void Rebuild_Callback(BaseObject baseObject)
        {
            NeedsRebuild = true;
        }

        #endregion
    }
}
