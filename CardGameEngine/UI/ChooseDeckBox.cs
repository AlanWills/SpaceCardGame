using _2DEngine;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Input;

namespace CardGameEngine
{
    /// <summary>
    /// A wrapper class around the UIObjectBox which has a grid control for our decks as the main UI element.
    /// Each element in the grid control will represent one of our player's decks which they will choose when beginning a match.
    /// </summary>
    public class ChooseDeckBox : UIObjectBox, IClickable
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the main UI in this dialog box - 
        /// </summary>
        private GridControl DeckGridControl { get; set; }

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

        public ChooseDeckBox(string title, Vector2 localPosition, string textureAsset = AssetManager.DefaultTextBoxTextureAsset, float lifeTime = float.MaxValue) :
            base(title, localPosition, textureAsset, lifeTime)
        {
            DeckGridControl = new GridControl(1, new Vector2(300, 350), Vector2.Zero);
            UIObject = DeckGridControl;
        }

        #region Virtual Functions

        /// <summary>
        /// Fix up the size of this box and the positions of the UI in it
        /// </summary>
        public override void Initialise()
        {
            CheckShouldInitialise();

            base.Initialise();

            Size = new Vector2(Math.Max(DeckGridControl.Size.X, Title.Size.X) + 2 * xPadding, 3 * yPadding + DeckGridControl.Size.Y + Title.Size.Y);
            Title.LocalPosition = new Vector2(0, (-Size.Y + Title.Size.Y) * 0.5f + yPadding);
            DeckGridControl.LocalPosition = Title.LocalPosition + new Vector2(0, (Title.Size.Y + DeckGridControl.Size.Y) * 0.5f + yPadding);
        }

        /// <summary>
        /// Add our final deck images - do this here so that all the sizes are calculated
        /// </summary>
        public override void Begin()
        {
            foreach (Deck deck in Array.FindAll(PlayerCardRegistry.Instance.Decks, x => x.IsCreated))
            {
                ClickableImage deckUI = DeckGridControl.AddObject(new ClickableImage(DeckGridControl.ElementSize, Vector2.Zero, Card.CardBackTextureAsset), true, true) as ClickableImage;

                // A lot of these will be unused but it will avoid bugs down the road if we just do this in case they are used
                deckUI.OnLeftClicked += OnLeftClicked;
                deckUI.OnMiddleClicked += OnMiddleClicked;
                deckUI.OnRightClicked += OnRightClicked;
                deckUI.LeftClickAccelerator = LeftClickAccelerator;
                deckUI.MiddleClickAccelerator = MiddleClickAccelerator;
                deckUI.RightClickAccelerator = RightClickAccelerator;
            }

            base.Begin();
        }

        #endregion
    }
}
