using _2DEngine;
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
    public class PlayerGameBoardSection : GameObjectContainer
    {
        /// <summary>
        /// A list of the player's currently laid resource cards indexed by resource type.
        /// Useful easy access for changing their appearance based on what has happened in the game.
        /// </summary>
        private List<GameCard>[] ResourceCards { get; set; }

        /// <summary>
        /// A list of references to the ship cards that have been added.
        /// We will change the active object when we change turn phase.
        /// </summary>
        private List<CardObjectPair> Ships { get; set; }

        /// <summary>
        /// A container to group our ships together and automatically space them.
        /// </summary>
        public GameCardControl PlayerShipCardControl { get; private set; }

        /// <summary>
        /// A reference to the human player
        /// </summary>
        private GamePlayer Player { get; set; }

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
            ResourceCards = new List<GameCard>[(int)ResourceType.kNumResourceTypes];
            for (int type = 0; type < (int)ResourceType.kNumResourceTypes; type++)
            {
                ResourceCards[type] = new List<GameCard>();
            }

            PlayerShipCardControl = AddObject(new GameCardControl(typeof(ShipCard), new Vector2(Size.X * 0.8f, Size.Y * 0.5f), GamePlayer.MaxShipNumber, 1, new Vector2(0, - Size.Y * 0.25f), "Sprites\\Backgrounds\\TileableNebula"));

            Ships = new List<CardObjectPair>();

            // Set up events
            AfterCardPlaced += UseResourcesToLayCard;
            Player.OnNewTurn += FlipResourceCardsFaceUp;
            Player.OnNewTurn += GivePlayerFullResources;

            Debug.Assert(ScreenManager.Instance.CurrentScreen is BattleScreen);
            BattleScreen battleScreen = ScreenManager.Instance.CurrentScreen as BattleScreen;
            battleScreen.OnCardPlacementStateStarted += ShowCardsForCardObjectPairs;
            battleScreen.OnBattleStateStarted += ShowObjectsForCardObjectPairs;
        }

        #region Virtual Functions

        /// <summary>
        /// Adds our card to the section, but calls a particular function based on it's type to perform extra stuff like adding a reference to a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObjectToAdd"></param>
        /// <param name="load"></param>
        /// <param name="initialise"></param>
        /// <returns></returns>
        public override T AddObject<T>(T gameObjectToAdd, bool load = false, bool initialise = false)
        {
            // If we are adding a card, deal with special cases here
            GameCard card = gameObjectToAdd as GameCard;
            if (card != null)
            {
                if (card is AbilityCard)
                {

                }
                else if (card is DefenceCard)
                {

                }
                else if (card is ResourceCard)
                {
                    AddResourceCard(card as ResourceCard, load, initialise);
                }
                else if (card is ShipCard)
                {
                    AddShipCard(card as ShipCard);
                }
                else if (card is WeaponCard)
                {

                }
                else
                {
                    Debug.Fail("Adding an unregistered card to game board");
                }

                card.IsPlaced = true;

                if (AfterCardPlaced != null)
                {
                    AfterCardPlaced(card);
                }
            }
            else
            {
                // Otherwise just add the object as normal
                base.AddObject(gameObjectToAdd, load, initialise);
            }

            return gameObjectToAdd;
        }

        #endregion

        #region Specific Functions when adding card types

        /// <summary>
        /// A function which will be called when we add a resource card to this section.
        /// Adds the resource card to this game board section and edits the available resource cards.
        /// </summary>
        private void AddResourceCard(ResourceCard resourceCard, bool load, bool initialise)
        {
            Debug.Assert(Player.ResourceCardsPlacedThisTurn < GamePlayer.ResourceCardsCanLay);

            float padding = 10;
            int typeIndex = (int)resourceCard.ResourceType;
            int cardCount = ResourceCards[typeIndex].Count;

            resourceCard.Size *= 0.7f;

            if (cardCount == 0)
            {
                // We are adding the first resource card of this type
                resourceCard.LocalPosition = new Vector2((-Size.X + resourceCard.Size.X) * 0.5f + padding, Size.Y * 0.5f - resourceCard.Size.Y * 0.5f - padding - typeIndex * (resourceCard.Size.Y + padding));
            }
            else
            {
                // We are adding another resource card, so overlay it on top and slightly to the side of the previous one
                resourceCard.LocalPosition = ResourceCards[typeIndex][cardCount - 1].LocalPosition + new Vector2(resourceCard.Size.X * 0.15f, 0);
            }

            // We do this update because of the order in which events occur.  We have changed local position and reparented, but since we were parented to the mouse our collider has yet to be updated.
            // Therefore the card will show it's info image for one frame, before the update collider function is called automatically.
            // By updating the collider automatically here, we avoid this problem.
            resourceCard.Collider.Update();

            ResourceCards[typeIndex].Add(resourceCard);

            Player.AvailableResources[typeIndex]++;
            Player.ResourceCardsPlacedThisTurn++;

            base.AddObject(resourceCard, load, initialise);
        }

        /// <summary>
        /// Adds a ship card object pair to our ship control and increments the player's total number of ships placed.
        /// </summary>
        /// <param name="shipCard"></param>
        private void AddShipCard(ShipCard shipCard)
        {
            Debug.Assert(Player.CurrentShipsPlaced < GamePlayer.MaxShipNumber);
            Debug.Assert(shipCard.CardData is ShipCardData);

            // Set up an event for syncing the player's total ships when this card dies.
            shipCard.OnDeath += SyncPlayerShipsPlaced;

            Ship ship = new Ship(shipCard.CardData as ShipCardData);

            // Will always need to load and initialise this new card object pair
            CardObjectPair shipCardAndShip = PlayerShipCardControl.AddObject(new CardObjectPair(shipCard, ship), true, true);
            Player.CurrentShipsPlaced++;

            Ships.Add(shipCardAndShip);
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
                    ResourceCards[typeIndex][numResourceCardsTotal - numAvailableResourceCards].Flip(CardFlipState.kFaceDown);

                    // Remove one from our player's available resources.
                    Player.AvailableResources[typeIndex]--;
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
                foreach (ResourceCard resourceCard in ResourceCards[type])
                {
                    resourceCard.Flip(CardFlipState.kFaceUp);
                }

                Player.AvailableResources[type] = ResourceCards[type].Count;
            }
        }

        /// <summary>
        /// A callback for when this player begins a new turn - resets all of our player's available resources.
        /// </summary>
        /// <param name="newActivePlayer"></param>
        private void GivePlayerFullResources(GamePlayer newActivePlayer)
        {
            for (int type = 0; type < (int)ResourceType.kNumResourceTypes; type++)
            {
                Player.AvailableResources[type] = ResourceCards[type].Count;
            }
        }

        /// <summary>
        /// An event which sets all the card object pairs to be showing their card
        /// </summary>
        /// <param name="turnState"></param>
        private void ShowCardsForCardObjectPairs()
        {
            foreach (CardObjectPair cardPair in Ships)
            {
                cardPair.SetActiveObject(CardOrObject.kCard);
            }
        }

        /// <summary>
        /// An event which sets all the card object pairs to be showing their object
        /// </summary>
        /// <param name="turnState"></param>
        private void ShowObjectsForCardObjectPairs()
        {
            foreach (CardObjectPair cardPair in Ships)
            {
                cardPair.SetActiveObject(CardOrObject.kObject);
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