using _2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace SpaceCardGame
{
    /// <summary>
    /// A list control which displays images of the inputted cards.
    /// Can specify a predicate to only include certain cards, but by default adds all contained in the inputted list
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

        public CardGridControl(List<CardData> cardList, int columns, Vector2 localPosition, string textureAsset = AssetManager.DefaultEmptyPanelTextureAsset) :
            this(cardList, columns, Vector2.Zero, localPosition, textureAsset)
        {

        }

        public CardGridControl(List<CardData> cardList, int columns, Vector2 size, Vector2 localPosition, string textureAsset = AssetManager.DefaultEmptyPanelTextureAsset) :
            base(columns, size, localPosition, textureAsset)
        {
            CardList = cardList;
            IncludePredicate = new Predicate<CardData>(x => true);  // By default all the cards we input will be added
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
            Card card = AddChild(cardData.CreateCard(null));
            card.HandAnimationModule.Die();     // Don't need animation or card outlines for the shop screen
            card.CardOutline.Die();
            card.AddModule(new CardHoverInfoModule(card), true, true);

            // These are probably not going to be used, but set them up anyway
            card.ClickableModule.LeftClickAccelerator = LeftClickAccelerator;
            card.ClickableModule.MiddleClickAccelerator = MiddleClickAccelerator;
            card.ClickableModule.RightClickAccelerator = RightClickAccelerator;

            // Add the click callbacks if they exist
            if (OnLeftClicked != null)
            {
                card.ClickableModule.OnLeftClicked += OnLeftClicked;
                card.ClickableModule.OnLeftClicked += Rebuild_Callback;
            }

            if (OnMiddleClicked != null)
            {
                card.ClickableModule.OnMiddleClicked += OnMiddleClicked;
                card.ClickableModule.OnMiddleClicked += Rebuild_Callback;
            }

            if (OnRightClicked != null)
            {
                card.ClickableModule.OnRightClicked += OnRightClicked;
                card.ClickableModule.OnRightClicked += Rebuild_Callback;
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
