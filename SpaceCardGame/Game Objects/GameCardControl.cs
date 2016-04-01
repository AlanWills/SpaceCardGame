using _2DEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A general class for managing cards in our screen.
    /// Handles dynamic moving of cards to show the user what they would look like if a card is laid.
    /// Specific to one type of card - others will not be able to be laid.
    /// Also allows us to restrict the maximum number of elements we can add.
    /// </summary>
    public class GameCardControl : GameObjectContainer
    {
        private enum Space
        {
            kWorldSpace,
            kLocalSpace
        }

        /// <summary>
        /// This control is going to be used for one type of card
        /// </summary>
        private Type CardType { get; set; }

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
        private GameObject[] StoredCards { get; set; }

        private float columnWidth;

        public GameCardControl(Type cardType, Vector2 size, int columns, int rows, Vector2 localPosition, string textureAsset) :
            base(localPosition, AssetManager.EmptyGameObjectDataAsset)
        {
            CardType = cardType;
            Columns = columns;
            Rows = rows;
            Size = size;
            TextureAsset = textureAsset;

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
        /// Shifts cards if we are looking to lay one of the type represented by this control.
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            GameMouse gameMouse = GameMouse.Instance;
            if (Collider.CheckIntersects(gameMouse.InGamePosition))
            {
                if (gameMouse.Children.Exists(x => x is GameCard && x.GetType() == CardType))
                {
                    float gameMouseLocalYPos = gameMouse.InGamePosition.Y - WorldPosition.Y;
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
        public override T AddObject<T>(T gameObjectToAdd, bool load = false, bool initialise = false)
        {
            // Check we have room left!
            Debug.Assert(GameObjects.ActiveObjectsCount < Rows * Columns);

            // In our handle input we do shifting so that by the time we add a card the space under it should be empty
            // Need to get the mouse position and work out the appropriate section we are in
            int pairIndex = GetPositionIndex(gameObjectToAdd.WorldPosition.X, Space.kWorldSpace);
            Debug.Assert(pairIndex >= 0 && pairIndex < LocalXPositions.Length);

            // Set game object's local position
            gameObjectToAdd.LocalPosition = new Vector2(LocalXPositions[pairIndex], 0);
            StoredCards[pairIndex] = gameObjectToAdd;

            // Can remove this once we fix our sizes!
            return base.AddObject(gameObjectToAdd, load, initialise);
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Returns an empty position in this card control.
        /// Should only be used in a context where there is room
        /// </summary>
        /// <returns></returns>
        public Vector2 GetEmptySlot()
        {
            Debug.Assert(GameObjects.ActiveObjectsCount < LocalXPositions.Length);

            for (int i = 0; i < StoredCards.Length; i++)
            {
                if (StoredCards[i] == null)
                {
                    return new Vector2(LocalXPositions[i], 0);
                }
            }

            Debug.Fail("No empty slot could be found");
            return Vector2.Zero;
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

            Debug.Fail("Position not registered");
            return -1;
        }

        #endregion
    }
}
