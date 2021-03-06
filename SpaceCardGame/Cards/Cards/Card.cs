﻿using CelesteEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using SpaceCardGameData;

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
        /// The player who owns this card
        /// </summary>
        public Player Player { get; private set; }

        /// <summary>
        /// The card data for this card
        /// </summary>
        public CardData CardData { get; private set; }

        /// <summary>
        /// A reference to the clickable module attached to this card.
        /// </summary>
        public ClickableCardModule ClickableModule { get; private set; }

        /// <summary>
        /// Performs animation whilst the card is in our hand, but is removed during the OnLay function
        /// </summary>
        public CardHandAnimationModule HandAnimationModule { get; private set; }

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

        public Card(Player player, CardData cardData) :
            base(Vector2.Zero, "")
        {
            DebugUtils.AssertNotNull(cardData);

            Player = player;
            CardData = cardData;
            TextureAsset = cardData.TextureAsset;
            FlipState = CardFlipState.kFaceUp;
            UsesCollider = true;                    // Explicitly state this because Image does not use a collider

            ClickableModule = AddModule(new ClickableCardModule());       // Add our clickable module
            ClickableModule.OnLeftClicked += ClickableObjectModule.EmptyClick;

            HandAnimationModule = AddModule(new CardHandAnimationModule());

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

            // Hard coded asset size correction
            Scale(0.5f);
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

            if (IsPlaced)
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
            if (IsPlaced || !HandAnimationModule.IsAlive)
            {
                base.UpdateCollider(ref position, ref size);
            }
            else
            {
                position = WorldPosition;
                size = new Vector2(HandAnimationModule.DrawingSize.X, HandAnimationModule.DrawingSize.Y + Math.Abs(HandAnimationModule.RestingPosition.Y - LocalPosition.Y));   // Not perfect, but better.  Over estimates the top
            }
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

                    // Scale the card and all of it's children up to the drawing size
                    Scale(Vector2.Divide(HandAnimationModule.DrawingSize, currentSize));

                    base.Draw(spriteBatch);

                    // And then reset the size
                    Scale(Vector2.Divide(currentSize, HandAnimationModule.DrawingSize));
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
                    Colour * Opacity,
                    SpriteEffect,
                    0);
            }
        }

        /// <summary>
        /// Calls the OnDeath event if it is hooked up.
        /// If we are parented under a CardObjectPair we call die on the parent triggering this and the CardObject to die too.
        /// </summary>
        public override void Die()
        {
            OnDie();

            base.Die();

            if (Parent is CardObjectPair && Parent.IsAlive)
            {
                Parent.Die();
            }

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
        /// Templated function for accessing our parent as a specific CardObjectPair
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetCardObjectPair<T>() where T : CardObjectPair
        {
            Debug.Assert(Parent is T);
            return Parent as T;
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
        /// Removes the hand animation module.
        /// If a target has to be chosen etc. then run a script in here to perform that.
        /// </summary>
        public virtual void OnLay()
        {
            Player.AlterResources(this, ChargeType.kCharge);
            Player.CurrentHand.Remove(this);

            // This should clean up the HandAnimationModule
            HandAnimationModule.Die();
            HandAnimationModule = null;
        }

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

        #endregion
    }
}
