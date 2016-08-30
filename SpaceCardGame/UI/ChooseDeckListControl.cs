using CelesteEngine;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Input;

namespace SpaceCardGame
{
    /// <summary>
    /// A class which has a list control for our decks as the main UI element.
    /// Each element in the list control will represent one of our player's decks which they will choose when beginning a match.
    /// </summary>
    public class ChooseDeckListControl : ListControl, IClickable
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

        public ChooseDeckListControl(Vector2 localPosition, string textureAsset = AssetManager.DefaultMenuTextureAsset) :
            this(Vector2.Zero, localPosition, textureAsset)
        {
            
        }

        public ChooseDeckListControl(Vector2 size, Vector2 localPosition, string textureAsset = AssetManager.DefaultMenuTextureAsset)
            : base(size, localPosition, textureAsset)
        {
            
        }

        #region Virtual Functions

        /// <summary>
        /// Add our final deck images - do this here so that all the sizes are calculated
        /// </summary>
        public override void Begin()
        {
            foreach (Deck deck in Array.FindAll(PlayerDataRegistry.Instance.Decks, x => x.IsCreated))
            {
                ClickableImage deckUI = AddChild(new ClickableImage(Vector2.Zero, Card.CardBackTextureAsset), true, true);
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

        /// <summary>
        /// Kill this object when we press escape
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            if (GameKeyboard.Instance.IsKeyPressed(Keys.Escape))
            {
                Die();
            }
        }

        #endregion
    }
}
