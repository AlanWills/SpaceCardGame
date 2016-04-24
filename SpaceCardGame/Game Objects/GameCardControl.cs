using _2DEngine;
using CardGameEngine;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A general class for managing cards in our screen.
    /// Handles dynamic moving of cards to show the user what they would look like if a card is laid.
    /// Specific to one type of card - others will not be able to be laid.
    /// Also allows us to restrict the maximum number of elements we can add.
    /// </summary>
    public class GameCardControl : GameObject
    {
        private enum Space
        {
            kWorldSpace,
            kLocalSpace
        }

        private enum PositionIndex
        {
            kInvalid = -1
        }

        /// <summary>
        /// The card data type we will be restricting cards to have before they can be added to this.
        /// </summary>
        private Type FilterType { get; set; }

        /// <summary>
        /// The number of columns we spread our cards over
        /// </summary>
        private int Columns { get; set; }

        /// <summary>
        /// The number of rows we spread our cards over
        /// </summary>
        private int Rows { get; set; }

        /// <summary>
        /// A cached array of the fixed positions of this card control
        /// </summary>
        private float[] LocalXPositions { get; set; }
        
        /// <summary>
        /// A cached array of stored cards
        /// </summary>
        private BaseObject[] StoredCards { get; set; }

        private float columnWidth;

        public GameCardControl(Type filterType, int columns, int rows, Vector2 size, Vector2 localPosition) :
            base(localPosition, AssetManager.EmptyGameObjectDataAsset)
        {
            FilterType = filterType;
            Columns = columns;
            Rows = rows;
            Size = size;

            columnWidth = Size.X / Columns;
            LocalXPositions = new float[Columns];
            StoredCards = new GameObject[Columns];

            for (int i = 0; i < Columns; i++)
            {
                LocalXPositions[i] = -Size.X * 0.5f + (i + 0.5f) * columnWidth;
                StoredCards[i] = null;
            }
        }

        #region Virtual Functions

        /// <summary>
        /// Set up some event call backs from the main screen
        /// </summary>
        public override void Initialise()
        {
            CheckShouldInitialise();

            Debug.Assert(ScreenManager.Instance.CurrentScreen is BattleScreen);
            BattleScreen battleScreen = ScreenManager.Instance.CurrentScreen as BattleScreen;

            battleScreen.OnCardPlacementStateStarted += SetUpGameObjectsForCardPlacement;
            battleScreen.OnBattleStateStarted += SetUpGameObjectsForBattle;
            battleScreen.OnTurnEnd += GameObjectsOnTurnEnd;

            base.Initialise();
        }

        /// <summary>
        /// Shifts cards if we are looking to lay one of the type represented by this control.
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            GameMouse gameMouse = GameMouse.Instance;
            if (Collider.CheckIntersects(gameMouse.InGameWorldPosition))
            {
                BaseObject baseobject = gameMouse.FindChild<BaseObject>(x => x is BaseUICard && (x as BaseUICard).CardData.GetType() == FilterType);
                if (baseobject != null)
                {
                    float gameMouseLocalYPos = gameMouse.InGameWorldPosition.Y - WorldPosition.Y;
                    int positionIndex = GetPositionIndex(GameMouse.Instance.WorldPosition.X, Space.kWorldSpace);
                    float localXPos = LocalXPositions[positionIndex];

                    // Might need to change this at some point
                    gameMouse.LocalPosition = new Vector2(WorldPosition.X + localXPos, WorldPosition.Y);
                }
            }
        }

        /// <summary>
        /// Removes references to dead cards
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            DebugUtils.AssertNotNull(StoredCards);
            for (int i = 0; i < StoredCards.Length; i++)
            {
                if (StoredCards[i] != null && !StoredCards[i].IsAlive)
                {
                    StoredCards[i] = null;
                }
            }
        }

        /// <summary>
        /// Add a game card to this control if it is of the corresponding type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObjectToAdd"></param>
        /// <param name="load"></param>
        /// <param name="initialise"></param>
        /// <returns></returns>
        public override T AddChild<T>(T gameObjectToAdd, bool load = false, bool initialise = false)
        {
            // Check we have room left!
            Debug.Assert(ChildrenCount < Rows * Columns);

            // In our handle input we do shifting so that by the time we add a card the space under it should be empty
            // Need to get the mouse position and work out the appropriate section we are in
            int pairIndex = GetPositionIndex(gameObjectToAdd.WorldPosition.X, Space.kLocalSpace);
            if (pairIndex == (int)PositionIndex.kInvalid)
            {
                // Our current position is not valid, so just find an empty one
                pairIndex = GetEmptySlotIndex();
            }

            Debug.Assert(pairIndex >= 0 && pairIndex < LocalXPositions.Length);

            // Set game object's local position
            gameObjectToAdd.LocalPosition = new Vector2(LocalXPositions[pairIndex], 0);
            StoredCards[pairIndex] = gameObjectToAdd;

            return base.AddChild(gameObjectToAdd, load, initialise);
        }

        #endregion

        #region Event Callbacks

        /// <summary>
        /// An event called when we begin card placement.
        /// Sets all the card object pairs to be showing their cards.
        /// </summary>
        private void SetUpGameObjectsForCardPlacement()
        {
            foreach (CardObjectPair cardPair in GetChildrenOfType<CardObjectPair>())
            {
                cardPair.MakeReadyForCardPlacement();
            }
        }

        /// <summary>
        /// An event called when we begin battle.
        /// Sets all the card object pairs to be showing their card objects.
        /// </summary>
        private void SetUpGameObjectsForBattle()
        {
            foreach (CardObjectPair cardPair in GetChildrenOfType<CardObjectPair>())
            {
                cardPair.MakeReadyForBattle();
            }
        }

        /// <summary>
        /// An event called before our turn ends and the opponent's begins.
        /// Makes all the cards we placed this turn ready.
        /// </summary>
        private void GameObjectsOnTurnEnd()
        {
            foreach (CardObjectPair cardPair in GetChildrenOfType<CardObjectPair>())
            {
                cardPair.OnTurnEnd();
            }
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Returns an empty position index in this card control.
        /// Should only be used in a context where there is room
        /// </summary>
        /// <returns></returns>
        private int GetEmptySlotIndex()
        {
            Debug.Assert(ChildrenCount < LocalXPositions.Length);

            for (int i = 0; i < StoredCards.Length; i++)
            {
                if (StoredCards[i] == null)
                {
                    return i;
                }
            }

            Debug.Fail("No empty slot could be found");
            return -1;
        }

        #endregion

        #region UI Rebuilding

        /// <summary>
        /// Get the appropriate position pair
        /// </summary>
        /// <param name="positionX"></param>
        /// <returns></returns>
        private int GetPositionIndex(float positionX, Space inputPositionSpace)
        {
            Debug.Assert(LocalXPositions.Length > 0);

            if (inputPositionSpace == Space.kWorldSpace)
            {
                // Convert position to local space
                positionX -= WorldPosition.X;
            }

            for (int i = 0; i < LocalXPositions.Length; i++)
            {
                float halfWidth = columnWidth * 0.5f;

                if (positionX >= LocalXPositions[i] - halfWidth && positionX <= LocalXPositions[i] + halfWidth)
                {
                    return i;
                }
            }

            return (int)PositionIndex.kInvalid;
        }

        #endregion
    }
}
