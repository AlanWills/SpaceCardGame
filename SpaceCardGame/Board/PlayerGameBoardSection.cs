﻿using _2DEngine;
using CardGameEngine;
using Microsoft.Xna.Framework;
using SpaceCardGameData;
using System.Collections.Generic;
using System.Diagnostics;

namespace SpaceCardGame
{
    public delegate void AfterCardPlacedHandler(GameCard card);

    /// <summary>
    /// A class to handle the game objects in the game board for one player
    /// </summary>
    public class PlayerGameBoardSection : GameObject
    {
        /// <summary>
        /// A list of the player's currently laid resource cards indexed by resource type.
        /// Useful easy access for changing their appearance based on what has happened in the game.
        /// </summary>
        private List<CardResourcePair>[] ResourceCards { get; set; }

        /// <summary>
        /// A list of references to the ship cards that have been added.
        /// We will change the active object when we change turn phase.
        /// </summary>
        private List<CardShipPair> Ships { get; set; }

        /// <summary>
        /// A container to group our ships together and automatically space them.
        /// </summary>
        public GameCardControl PlayerShipCardControl { get; private set; }

        /// <summary>
        /// A reference to the human player
        /// </summary>
        private GamePlayer Player { get; set; }

        /// <summary>
        /// A reference to the battle screen for convenience
        /// </summary>
        private BattleScreen BattleScreen { get; set; }

        /// <summary>
        /// An event that will be triggered after we have finished adding a card to the board.
        /// </summary>
        public event AfterCardPlacedHandler AfterCardPlaced;

        public PlayerGameBoardSection(GamePlayer player, Vector2 localPosition, string dataAsset = AssetManager.EmptyGameObjectDataAsset) :
            base(localPosition, dataAsset)
        {
            Player = player;
            Size = new Vector2(ScreenManager.Instance.ScreenDimensions.X, ScreenManager.Instance.ScreenDimensions.Y * 0.5f);

            // Create an array with a list for each resource type
            ResourceCards = new List<CardResourcePair>[(int)ResourceType.kNumResourceTypes];
            for (int type = 0; type < (int)ResourceType.kNumResourceTypes; type++)
            {
                ResourceCards[type] = new List<CardResourcePair>();
            }

            PlayerShipCardControl = AddChild(new GameCardControl(typeof(ShipCardData), new Vector2(Size.X * 0.8f, Size.Y * 0.5f), GamePlayer.MaxShipNumber, 1, new Vector2(0, - Size.Y * 0.25f), "Sprites\\Backgrounds\\TileableNebula"));

            Ships = new List<CardShipPair>();

            // Set up events
            AfterCardPlaced += UseResourcesToLayCard;
            Player.OnNewTurn += FlipResourceCardsFaceUp;

            Debug.Assert(ScreenManager.Instance.CurrentScreen is BattleScreen);
            BattleScreen = ScreenManager.Instance.CurrentScreen as BattleScreen;
            BattleScreen.OnCardPlacementStateStarted += SetUpGameObjectsForCardPlacement;
            BattleScreen.OnBattleStateStarted += SetUpGameObjectsForBattle;
        }

        #region Specific Functions for adding card types

        /// <summary>
        /// Adds our card to the section, but calls a particular function based on it's type to perform extra stuff like adding a reference to a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObjectToAdd"></param>
        /// <param name="load"></param>
        /// <param name="initialise"></param>
        /// <returns></returns>
        public void AddCard(CardData cardData)
        {
            CardObjectPair pair = null;
            if (cardData is AbilityCardData)
            {

            }
            else if (cardData is DefenceCardData)
            {
                pair = AddDefenceCard(cardData as DefenceCardData);
            }
            else if (cardData is ResourceCardData)
            {
                pair = AddResourceCard(cardData as ResourceCardData);
            }
            else if (cardData is ShipCardData)
            {
                pair = AddShipCard(cardData as ShipCardData);
            }
            else if (cardData is WeaponCardData)
            {

            }
            else
            {
                Debug.Fail("Adding an unregistered card to game board");
            }

            pair.Card.IsPlaced = true;

            if (AfterCardPlaced != null)
            {
                AfterCardPlaced(pair.Card);
            }
        }

        /// <summary>
        /// A function which will be called when we add resource card data to this section.
        /// Adds a resource card object pair to this game board section and edits the available resource cards.
        /// </summary>
        private CardResourcePair AddResourceCard(ResourceCardData resourceCardData)
        {
            Debug.Assert(Player.ResourceCardsPlacedThisTurn < GamePlayer.ResourceCardsCanLay);

            float padding = 10;

            // Will always need to load and initialise this new card object pair
            CardResourcePair resource = base.AddChild(new CardResourcePair(resourceCardData), true, true);

            int typeIndex = (int)resource.ResourceCard.ResourceType;
            int cardCount = ResourceCards[typeIndex].Count;

            resource.ResourceCard.Size *= 0.35f;
            resource.ResourceCard.OnFlip += OnResourceCardFlip;

            if (cardCount == 0)
            {
                // We are adding the first resource card of this type
                resource.LocalPosition = new Vector2((-Size.X + resource.ResourceCard.Size.X) * 0.5f + padding, Size.Y * 0.5f - resource.ResourceCard.Size.Y * 0.5f - padding - typeIndex * (resource.ResourceCard.Size.Y + padding));
            }
            else
            {
                // We are adding another resource card, so overlay it on top and slightly to the side of the previous one
                resource.LocalPosition = ResourceCards[typeIndex][cardCount - 1].LocalPosition + new Vector2(resource.ResourceCard.Size.X * 0.15f, 0);
            }

            // We do this update because of the order in which events occur.  We have changed local position and reparented, but since we were parented to the mouse our collider has yet to be updated.
            // Therefore the card will show it's info image for one frame, before the update collider function is called automatically.
            // By updating the collider automatically here, we avoid this problem.
            resource.ResourceCard.Collider.Update();

            ResourceCards[typeIndex].Add(resource);

            Player.AvailableResources[typeIndex]++;
            Player.ResourceCardsPlacedThisTurn++;

            return resource;
        }

        /// <summary>
        /// Adds a ship card object pair to our ship control and increments the player's total number of ships placed.
        /// </summary>
        /// <param name="shipCard"></param>
        private CardShipPair AddShipCard(ShipCardData shipCardData)
        {
            Debug.Assert(Player.CurrentShipsPlaced < GamePlayer.MaxShipNumber);

            // Will always need to load and initialise this new card object pair
            CardShipPair cardShipPair = new CardShipPair(shipCardData);
            cardShipPair.LocalPosition = GameMouse.Instance.InGamePosition;         // Do this before we add it to the control because we use the position to place it in the correct spot
            PlayerShipCardControl.AddChild(cardShipPair, true, true);

            cardShipPair.Card.Size *= 0.75f;
            cardShipPair.SetActiveObject(CardOrObject.kCard);

            // Set up an event for syncing the player's total ships when this card dies.
            cardShipPair.ShipCard.OnDeath += SyncPlayerShipsPlaced;

            Player.CurrentShipsPlaced++;

            Ships.Add(cardShipPair);

            return cardShipPair;
        }

        /// <summary>
        /// Adds a script to choose a ship to add the defence card to.
        /// </summary>
        /// <param name="defenceCard"></param>
        private CardDefencePair AddDefenceCard(DefenceCardData defenceCardData)
        {
            CardDefencePair cardObjectPair = new CardDefencePair(defenceCardData);

            ScriptManager.Instance.AddChild(new ChooseFriendlyShipScript(cardObjectPair), true, true);

            return cardObjectPair;
        }

        /// <summary>
        /// Removes resources from the player when they lay a card and updates the resource cards by flipping them face down
        /// </summary>
        /// <param name="card"></param>
        private void UseResourcesToLayCard(GameCard card)
        {
            for (int typeIndex = 0; typeIndex < (int)ResourceType.kNumResourceTypes; typeIndex++)
            {
                int numResourceCardsTotal = ResourceCards[typeIndex].Count;
                int numAvailableResourceCards = Player.AvailableResources[typeIndex];
                Debug.Assert(numAvailableResourceCards >= card.CardData.ResourceCosts[typeIndex]);

                for (int cost = 0; cost < card.CardData.ResourceCosts[typeIndex]; cost++)
                {
                    // Flip our bottom most available card face down
                    Debug.Assert(numResourceCardsTotal - numAvailableResourceCards >= 0);
                    ResourceCards[typeIndex][numResourceCardsTotal - numAvailableResourceCards].Card.Flip(CardFlipState.kFaceDown);
                }
            }
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Flips the resource cards back face up for the inputted cardData.
        /// This triggers the refunding to the player of their resources.
        /// </summary>
        /// <param name="cardData"></param>
        public void RefundCardResources(CardData cardData)
        {
            for (int resourceIndex = 0; resourceIndex < (int)ResourceType.kNumResourceTypes; resourceIndex++)
            {
                // Loop through all the costs of the card we are sending back and flip the resource cards face up
                for (int cost = 0; cost < cardData.ResourceCosts[resourceIndex]; cost++)
                {
                    // Find a face down resource card and flip it face up
                    Debug.Assert(ResourceCards[resourceIndex].Exists(x => x.Card.FlipState == CardFlipState.kFaceDown));
                    ResourceCards[resourceIndex].Find(x => x.Card.FlipState == CardFlipState.kFaceDown).Card.Flip(CardFlipState.kFaceUp);
                }
            }
        }

        #endregion

        #region Event Callbacks

        /// <summary>
        /// A callback for when this player begins a new turn - updates the resource cards so that they are all face up and active again.
        /// </summary>
        /// <param name="newActivePlayer"></param>
        private void FlipResourceCardsFaceUp(GamePlayer newActivePlayer)
        {
            for (int type = 0; type < (int)ResourceType.kNumResourceTypes; type++)
            {
                foreach (CardResourcePair resourceCardPair in ResourceCards[type])
                {
                    resourceCardPair.Card.Flip(CardFlipState.kFaceUp);
                }
            }
        }

        /// <summary>
        /// A callback for when a resource card is flipped.
        /// Adds or subtracts from the player's available resources depending on whether the card's flip state has changed and is face up or face down.
        /// </summary>
        /// <param name="newActivePlayer"></param>
        private void OnResourceCardFlip(BaseGameCard baseGameCard, CardFlipState newFlipState, CardFlipState oldFlipState)
        {
            // If our flip state has not changed, then do nothing
            if (newFlipState == oldFlipState)
            {
                return;
            }

            Debug.Assert(baseGameCard is ResourceCard);
            ResourceCard resourceCard = baseGameCard as ResourceCard;

            if (newFlipState == CardFlipState.kFaceDown)
            {
                Debug.Assert(Player.AvailableResources[(int)resourceCard.ResourceType] > 0);
                Player.AvailableResources[(int)resourceCard.ResourceType]--;
            }
            else
            {
                Player.AvailableResources[(int)resourceCard.ResourceType]++;
            }
        }

        /// <summary>
        /// An event called when we begin card placement.
        /// Sets all the card object pairs to be showing their cards.
        /// </summary>
        private void SetUpGameObjectsForCardPlacement()
        {
            // Show all the cards
            foreach (CardShipPair cardPair in Ships)
            {
                cardPair.SetActiveObject(CardOrObject.kCard);
            }
        }

        /// <summary>
        /// An event called when we begin battle.
        /// Sets all the card object pairs to be showing their card objects.
        /// </summary>
        private void SetUpGameObjectsForBattle()
        {
            foreach (CardShipPair cardPair in Ships)
            {
                cardPair.SetActiveObject(CardOrObject.kObject);

                Debug.Assert(cardPair.CardObject is Ship);
                Ship ship = cardPair.CardObject as Ship;

                if (Player != BattleScreen.ActivePlayer)
                {
                    ship.Turret.Colour = Color.White;
                }
                else if (ship.Turret.ShotsLeft > 0)
                {
                    // If it is our turn, we set the turrets with available shots to be highlighted green
                    ship.Turret.Colour = Color.Green;
                }
            }
        }

        /// <summary>
        /// An event which is called when a ship card dies.
        /// Will subtract one from the player's running total of the number of ships they have placed.
        /// </summary>
        /// <param name="gameCard"></param>
        private void SyncPlayerShipsPlaced(BaseGameCard gameCard)
        {
            Debug.Assert(Player.CurrentShipsPlaced > 0);
            Player.CurrentShipsPlaced--;
        }

        #endregion
    }
}