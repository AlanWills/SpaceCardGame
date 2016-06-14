using _2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A metric we can use to provide a hint to the AI as to how effective this card will be if played in the current board state.
    /// </summary>
    public enum AICardWorthMetric
    {
        kShouldNotPlayAtAll,
        kBadCardToPlay,
        kAverageCardToPlay,
        kGoodCardToPlay,
        kShouldDefinitelyPlay,
    }

    public enum CardFlipState
    {
        kFaceUp,
        kFaceDown,
    }

    public delegate void OnCardFlippedHandler(Card baseCard, CardFlipState newFlipState, CardFlipState oldFlipState);
    public delegate void OnCardDeathHandler(Card baseCard);

    /// <summary>
    /// A class used in our game for doing additional checks involving the resources in the GamePlayer class
    /// </summary>
    public abstract class Card : Image, ICardObjectElement
    {
        #region Properties and Fields

        /// <summary>
        /// The card data for this card
        /// </summary>
        public CardData CardData { get; private set; }

        /// <summary>
        /// A reference to the clickable module attached to this UI card.
        /// </summary>
        public ClickableObjectModule ClickableModule { get; private set; }

        /// <summary>
        /// A flag to indicate whether we wish the card to increase in size whilst our mouse is over it.
        /// True by default
        /// </summary>
        public bool EnlargeOnHover { get; set; }

        /// <summary>
        /// A reference to our size we will use to alter the size of this card if hovered over.
        /// This size really drives the size of the card
        /// </summary>
        private Vector2 DrawingSize { get; set; }

        /// <summary>
        /// Used for some effects - our card if the mouse is over will move up the screen slightly
        /// </summary>
        private Vector2 RestingPosition { get; set; }
        private Vector2 HighlightedPosition { get; set; }

        /// <summary>
        /// A vector property that is a local offset from the position of this card to it's highlighted position
        /// </summary>
        public Vector2 OffsetToHighlightedPosition { get; set; }

        /// <summary>
        /// An image which shows an outline round our card - we can update it with colours to show validity etc.
        /// </summary>
        public CardOutline CardOutline { get; private set; }

        /// <summary>
        /// The current flip state of this card
        /// </summary>
        public CardFlipState FlipState { get; private set; }

        /// <summary>
        /// We are placed on the board when we have been added to a suitable CardObjectPair 
        /// </summary>
        public bool IsPlaced { get { return Parent is CardObjectPair; } }

        /// <summary>
        /// An event which is called when the card flip state is changed.
        /// Passes the new flip state.
        /// </summary>
        public event OnCardFlippedHandler OnFlip;

        /// <summary>
        /// An event that will be called after this object dies
        /// </summary>
        public OnCardDeathHandler OnDeath;

        public const string CardBackTextureAsset = "Cards\\CardBack";
        public static Texture2D CardBackTexture;

        /// <summary>
        /// A reference to our parent as a CardObjectPair
        /// </summary>
        private CardObjectPair cardObjectPair;
        protected CardObjectPair CardObjectPair
        {
            get
            {
                if (cardObjectPair == null)
                {
                    DebugUtils.AssertNotNull(Parent);
                    Debug.Assert(Parent is CardObjectPair);
                    cardObjectPair = Parent as CardObjectPair;
                }

                return cardObjectPair;
            }
        }

        #endregion

        public Card(CardData cardData) :
            base(Vector2.Zero, "")
        {
            DebugUtils.AssertNotNull(cardData);

            CardData = cardData;
            TextureAsset = cardData.TextureAsset;
            FlipState = CardFlipState.kFaceUp;
            UsesCollider = true;                    // Explicitly state this because Image does not use a collider

            EnlargeOnHover = true;
            OffsetToHighlightedPosition = new Vector2(0, -140);
            ClickableModule = AddModule(new ClickableObjectModule());       // Add our clickable module
            ClickableModule.OnLeftClicked += ClickableObjectModule.EmptyClick;

            CardOutline = AddChild(new CardOutline(Vector2.Zero));
        }

        #region Virtual Functions

        /// <summary>
        /// Perform some fixup of Sizes and make sure the CardOutline is inline with the size of the card.
        /// </summary>
        public override void Initialise()
        {
            CheckShouldInitialise();

            base.Initialise();

            Size *= 0.5f;       // Size correction
            CardOutline.Size = Size;
        }

        /// <summary>
        /// Set up some constants for our animation effects here.
        /// By now the local position should be set.
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            Debug.Assert(Size != Vector2.Zero);
            DrawingSize = Size;

            UpdatePositions(LocalPosition);

            if (!IsPlaced)
            {
                // Experimental - have the card drop down to it's resting position
                LocalPosition = HighlightedPosition;
            }
        }

        /// <summary>
        /// If the mouse is over the card we show the info image, otherwise we hide it.
        /// Also, performs animation if the card is in our hand.
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            if (!IsPlaced)
            {
                DebugUtils.AssertNotNull(Collider);
                if (Collider.IsMouseOver)
                {
                    // We are sufficiently far away from the end position
                    if (LocalPosition.Y - HighlightedPosition.Y > 2)
                    {
                        // Move upwards slightly if we are hovering over
                        LocalPosition = Vector2.Lerp(LocalPosition, HighlightedPosition, elapsedGameTime * 5);
                    }
                    else
                    {
                        // We are close enough to be at the end position
                        LocalPosition = HighlightedPosition;
                    }

                    if (EnlargeOnHover && FlipState == CardFlipState.kFaceUp)
                    {
                        // If our card is face up and the mouse has no attached children (like other cards we want to place), increase the size
                        DrawingSize = Size * 2;
                    }
                    else
                    {
                        // If the mouse is not over the card, it's size should go back to normal
                        DrawingSize = Size;
                    }
                }
                else
                {
                    // We are sufficiently far away from the initial position
                    if (RestingPosition.Y - LocalPosition.Y > 2)
                    {
                        // Otherwise move back down to initial position
                        LocalPosition = Vector2.Lerp(LocalPosition, RestingPosition, elapsedGameTime * 5);
                    }
                    else
                    {
                        // We are close enough to be at the initial position
                        LocalPosition = RestingPosition;
                    }

                    // If the mouse is not over the card, it's size should go back to normal
                    DrawingSize = Size;
                }

                CardOutline.Size = DrawingSize;
            }
            else
            {
                // We always do this so that the outline is always inline with the card size after we have placed it.
                // If we have added a turret to a ship for example, the size will be smaller and we do not want to have to explicitly remember to fix up the size
                CardOutline.Size = Size;
            }
        }

        /// <summary>
        /// Updates the collider using the drawing size rather than the base size so that we don't get horrible flickering between the two
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        public override void UpdateCollider(ref Vector2 position, ref Vector2 size)
        {
            position = WorldPosition;
            size = new Vector2(DrawingSize.X, DrawingSize.Y + Math.Abs(RestingPosition.Y - LocalPosition.Y));   // Not perfect, but better.  Over estimates the top
        }

        /// <summary>
        /// Either draw our normal card if we are face up, or the back of the card if we are face down
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (FlipState == CardFlipState.kFaceUp)
            {
                if (!IsPlaced)
                {
                    // Store the size of the card, but set the Size property to the DrawingSize for drawing ONLY
                    Vector2 currentSize = Size;
                    Size = DrawingSize;

                    base.Draw(spriteBatch);

                    // Set the Size back to it's original value
                    Size = currentSize;
                }
                else
                {
                    // If we are placed, we do not want to be changing the size to do the animation used for the hand
                    base.Draw(spriteBatch);
                }
            }
            else
            {
                DebugUtils.AssertNotNull(CardBackTexture);
                spriteBatch.Draw(
                    CardBackTexture,
                    WorldPosition,
                    null,
                    SourceRectangle,
                    TextureCentre,
                    WorldRotation,
                    Vector2.Divide(Size, new Vector2(CardBackTexture.Width, CardBackTexture.Height)),
                    Colour.Value * Opacity,
                    SpriteEffect,
                    0);
            }
        }

        /// <summary>
        /// Calls the OnDeath event if it is hooked up.
        /// We call die on the parent triggering this and the CardObject to die too.
        /// </summary>
        public override void Die()
        {
            OnDie();

            base.Die();

            Debug.Assert(Parent is CardObjectPair);
            Parent.Die();

            OnDeath?.Invoke(this);
        }

        #endregion

        #region Card Specific Virtual Functions

        /// <summary>
        /// Creates a card object pair using this card data
        /// </summary>
        /// <returns></returns>
        public virtual CardObjectPair CreateCardObjectPair()
        {
            Debug.Fail("Handled by derived classes");
            return null;
        }

        /// <summary>
        /// A function that we will override with specific implementations for each card type.
        /// A function which can be used to determine whether the player can lay a card - could be used for resources, limiting a specific number of cards per turn etc.
        /// By default, checks the resource costs of this card against the inputted player's resources
        /// </summary>
        /// <param name="player">The player attempting to lay the card</param>
        /// <param name="error">An error string which is returned for displaying error UI</param>
        /// <returns></returns>
        public virtual bool CanLay(Player player, ref string error)
        {
            return player.HaveSufficientResources(this, ref error);
        }

        /// <summary>
        /// Override this function to perform custom behaviour when first layed.
        /// Called as soon as we add the object to the scene and right after WhenAddedToGameBoard.
        /// If a target has to be chosen etc. then run a script in here to perform that.
        /// </summary>
        public virtual void OnLay() { }

        /// <summary>
        /// Override this function to perform custom behaviour when our turn begins.
        /// </summary>
        public virtual void OnTurnBegin()
        {
            Show();
        }

        /// <summary>
        /// Override this function to perform custom behaviour when we begin the battle phase.
        /// </summary>
        public virtual void MakeReadyForBattle()
        {
            Hide();
        }

        /// <summary>
        /// Override this function to perform custom behaviour when our turn ends.
        /// </summary>
        public virtual void OnTurnEnd() { }

        /// <summary>
        /// Override this function to perform custom behaviour when this card dies.
        /// </summary>
        public virtual void OnDie() { }

        /// <summary>
        /// Some cards require us to choose a target before laying them, or for an ability.
        /// This is a virtual function which can be used to work out whether the inputted proposed target is a valid one for this card.
        /// </summary>
        /// <param name="pair"></param>
        /// <returns></returns>
        public virtual bool CanUseOn(CardObjectPair pairToValidate)
        {
            return true;
        }

        /// <summary>
        /// When our AI is analysing the cards it has in it's hands, it needs to work out the best cards to lay.
        /// By analysing the current board set up, other cards the AI has, cards the opponent has down etc. we 
        /// can create a value for how good a choice for the AI laying this card will be.
        /// This will also only be called if our card can actually be laid so we do not need to perform that validation here.
        /// </summary>
        public abstract AICardWorthMetric CalculateAIMetric(GameBoardSection aiGameBoardSection, GameBoardSection otherGameBoardSection);

        #endregion

        #region Utility Functions

        /// <summary>
        /// Flips our card state so that we are now facing according to the inputted flip state
        /// </summary>
        public void Flip(CardFlipState flipState)
        {
            CardFlipState oldFlipState = FlipState;
            FlipState = flipState;

            // Call our on flip event if it's not null
            OnFlip?.Invoke(this, FlipState, oldFlipState);
        }

        /// <summary>
        /// Recalculates the extra positions we use for our lerping effect
        /// </summary>
        /// <param name="newLocalPosition"></param>
        public void UpdatePositions(Vector2 newLocalPosition)
        {
            LocalPosition = newLocalPosition;
            RestingPosition = newLocalPosition;
            HighlightedPosition = newLocalPosition + OffsetToHighlightedPosition;
        }

        #endregion
    }
}
