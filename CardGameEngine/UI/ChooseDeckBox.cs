using _2DEngine;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Input;

namespace CardGameEngine
{
    /// <summary>
    /// A class which has a grid control for our decks as the main UI element.
    /// Each element in the grid control will represent one of our player's decks which they will choose when beginning a match.
    /// </summary>
    public class ChooseDeckGridControl : GridControl, IClickable
    {
        #region Properties and Fields

        /// <summary>
        /// IClickable properties
        /// </summary>
        public Keys LeftClickAccelerator { get; set; }
        public Keys MiddleClickAccelerator { get; set; }
        public Keys RightClickAccelerator { get; set; }
        public ClickState ClickState { get; }
        public event OnClicked OnLeftClicked;
        public event OnClicked OnMiddleClicked;
        public event OnClicked OnRightClicked;

        #endregion

        public ChooseDeckGridControl(int rows, int columns, Vector2 localPosition, string textureAsset = AssetManager.DefaultMenuTextureAsset) :
            this(rows, columns, Vector2.Zero, localPosition, textureAsset)
        {
            
        }

        public ChooseDeckGridControl(int rows, int columns, Vector2 size, Vector2 localPosition, string textureAsset = AssetManager.DefaultMenuTextureAsset)
            : base(rows, columns, size, localPosition, textureAsset)
        {
            
        }

        #region Virtual Functions

        /// <summary>
        /// Add our final deck images - do this here so that all the sizes are calculated
        /// </summary>
        public override void Begin()
        {
            foreach (Deck deck in Array.FindAll(PlayerCardRegistry.Instance.Decks, x => x.IsCreated))
            {
                ClickableImage deckUI = AddChild(new ClickableImage(ElementSize * 0.75f, Vector2.Zero, BaseUICard.CardBackTextureAsset), true, true);
                deckUI.StoredObject = deck;

                // A lot of these will be unused but it will avoid bugs down the road if we just do this in case they are used
                deckUI.ClickableModule.OnLeftClicked += OnLeftClicked;
                deckUI.ClickableModule.OnMiddleClicked += OnMiddleClicked;
                deckUI.ClickableModule.OnRightClicked += OnRightClicked;
                deckUI.ClickableModule.LeftClickAccelerator = LeftClickAccelerator;
                deckUI.ClickableModule.MiddleClickAccelerator = MiddleClickAccelerator;
                deckUI.ClickableModule.RightClickAccelerator = RightClickAccelerator;
            }

            base.Begin();
        }

        #endregion
    }
}
