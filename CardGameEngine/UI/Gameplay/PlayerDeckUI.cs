﻿using _2DEngine;
using CardGameEngineData;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CardGameEngine
{
    /// <summary>
    /// This class is really just UI sugar.
    /// Will be placed in our game screen to give information about the number of cards left etc.
    /// </summary>
    public class PlayerDeckUI : UIObject
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to our player
        /// </summary>
        private Player Player { get; set; }

        /// <summary>
        /// A label which shows how many cards are left in the player's deck - will be shown when our mouse is over the deck.
        /// </summary>
        public Label DeckCountLabel { get; private set; }

        /// <summary>
        /// A list of references to the images that have been created when we draw a card
        /// </summary>
        private List<UIObject> CardImagesList { get; set; }

        // A timer used for some UI sugar, nothing more
        private float cardLifeTime = 0.15f;
        private const string cardsLeftString = "Cards Left: ";
        private float padding = 5;

        #endregion

        public PlayerDeckUI(Player player, Vector2 localPosition, string textureAsset = BaseUICard.CardBackTextureAsset) :
            base(localPosition, textureAsset)
        {
            Player = player;

            CardImagesList = new List<UIObject>();
        }

        #region Virtual Functions

        /// <summary>
        /// Set up our label for deck count
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            DeckCountLabel = AddChild(new Label(cardsLeftString + Player.CardsLeftInDeck.ToString(), Vector2.Zero));
            DeckCountLabel.Hide();

            base.LoadContent();
        }

        /// <summary>
        /// Set up our event callbacks
        /// </summary>
        public override void Initialise()
        {
            CheckShouldInitialise();

            // Set up our event callbacks for when a card is drawn
            Player.OnCardDraw += SpawnCardUIWhenCardDrawn;
            Player.OnCardDraw += UpdateDeckUI;

            base.Initialise();
        }

        /// <summary>
        /// For the active objects we do some simple animation to show they are being drawn
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            CardImagesList.RemoveAll(x => !x.IsAlive);

            foreach (UIObject uiObject in CardImagesList)
            {
                uiObject.LocalPosition -= new Vector2(2f, 0);
            }

            // Update the visibility of our label count based on whether the mouse is over the collider
            // Would normally do this in the HandleInput, but our DeckUI can be disabled if it's an opponent
            DebugUtils.AssertNotNull(Collider);
            if (Collider.IsMouseOver)
            {
                DeckCountLabel.Show();
            }
            else
            {
                DeckCountLabel.Hide();
            }
        }

        #endregion

        /// <summary>
        /// A callback when we draw a card to spawn some UI from our deck, showing a card has been drawn.
        /// </summary>
        /// <param name="cardData"></param>
        private void SpawnCardUIWhenCardDrawn(CardData cardData)
        {
            // Image will be automatically parented under this
            Image cardImage = AddChild(new Image(Size, Vector2.Zero, BaseUICard.CardBackTextureAsset), true, true);
            cardImage.LifeTime = cardLifeTime;

            CardImagesList.Add(cardImage);
        }

        /// <summary>
        /// A callback for updating our UI when we draw a new card.
        /// </summary>
        /// <param name="cardData"></param>
        private void UpdateDeckUI(CardData cardData)
        {
            DeckCountLabel.Text = cardsLeftString + Player.CardsLeftInDeck.ToString();

            if (Player.CardsLeftInDeck == 0)
            {
                // If we have no cards left, hide the main deck UI
                Hide();
            }
            else
            {
                // Otherwise make it visible
                Show();
            }
        }
    }
}