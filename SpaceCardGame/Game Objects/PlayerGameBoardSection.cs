using _2DEngine;
using CardGameEngine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;

namespace SpaceCardGame
{
    public delegate void AfterCardPlacedHandler(Card card);

    /// <summary>
    /// A class to organise the cards in the game board for one player
    /// </summary>
    public class PlayerGameBoardSection : GameObjectContainer
    {
        /// <summary>
        /// A list of the player's currently laid resource cards indexed by resource type.
        /// Useful easy access for changing their appearance based on what has happened in the game.
        /// </summary>
        public List<Card>[] ResourceCards { get; private set; }

        /// <summary>
        /// A reference to the human player
        /// </summary>
        private GamePlayer Player { get; set; }

        /// <summary>
        /// An event that will be triggered after we have finished adding a card to the board.
        /// </summary>
        public event AfterCardPlacedHandler AfterCardPlaced;

        public PlayerGameBoardSection(Vector2 localPosition, string dataAsset = AssetManager.EmptyGameObjectDataAsset) :
            base(localPosition, dataAsset)
        {
            Player = BattleScreen.Player;
            Size = new Vector2(ScreenManager.Instance.ScreenDimensions.X, ScreenManager.Instance.ScreenDimensions.Y * 0.5f);

            // Create an array with a list for each resource type
            ResourceCards = new List<Card>[(int)ResourceType.kNumResourceTypes];
            for (int type = 0; type < (int)ResourceType.kNumResourceTypes; type++)
            {
                ResourceCards[type] = new List<Card>();
            }
        }

        #region Virtual Functions

        /// <summary>
        /// Hook into the screen events
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            // Hook into the screen events
            Debug.Assert(ScreenManager.Instance.CurrentScreen is BattleScreen);
            (ScreenManager.Instance.CurrentScreen as BattleScreen).OnNewTurn += UpdateResourceCardsAtTurnStart;
        }

        /// <summary>
        /// Adds our card to the section, but calls a particular function based on it's type to perform extra stuff like adding a reference to it to a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObjectToAdd"></param>
        /// <param name="load"></param>
        /// <param name="initialise"></param>
        /// <returns></returns>
        public override T AddObject<T>(T gameObjectToAdd, bool load = false, bool initialise = false)
        {
            Debug.Assert(gameObjectToAdd is Card);
            Card card = gameObjectToAdd as Card;

            if (card is ResourceCard)
            {
                AddResourceCard(card as ResourceCard);
            }

            base.AddObject(gameObjectToAdd, load, initialise);

            if (AfterCardPlaced != null)
            {
                AfterCardPlaced(card);
            }

            return gameObjectToAdd;
        }

        #endregion

        #region Specific Functions for when adding card types

        /// <summary>
        /// A function which will be called when we add a resource card to this section.
        /// </summary>
        private void AddResourceCard(ResourceCard resourceCard)
        {
            ResourceCards[(int)resourceCard.ResourceType].Add(resourceCard);

            float xPos = ((float)resourceCard.ResourceType + 0.5f) * (Size.X / (int)ResourceType.kNumResourceTypes) - Size.X * 0.5f;

            resourceCard.LocalPosition = new Vector2(xPos, 0);
        }

        #endregion

        #region Event Callbacks

        /// <summary>
        /// A callback for when we begin a new turn - updates the resource cards so that they are all face up and active again.
        /// </summary>
        /// <param name="newActivePlayer"></param>
        private void UpdateResourceCardsAtTurnStart(GamePlayer newActivePlayer)
        {
            for (int type = 0; type < (int)ResourceType.kNumResourceTypes; type++)
            {
                foreach (ResourceCard resourceCard in ResourceCards[type])
                {
                    resourceCard.Flip(CardFlipState.kFaceUp);
                }
            }
        }

        #endregion
    }
}