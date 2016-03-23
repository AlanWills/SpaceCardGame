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
        /// <summary>
        /// This control is going to be used for one type of card
        /// </summary>
        private Type CardType { get; set; }

        /// <summary>
        /// A flag to indicate whether we need to reposition our elements
        /// </summary>
        private bool NeedsRebuild { get; set; }

        /// <summary>
        /// The number of columns we spread our cards over
        /// </summary>
        private int Columns { get; set; }

        /// <summary>
        /// The number of rows we spread our cards over
        /// </summary>
        private int Rows { get; set; }

        /// <summary>
        /// An array of preset x positions and objects placed there.
        /// We will use to place our objects and update their positions.
        /// </summary>
        private KeyValuePair<float, GameObject>[] Positions { get; set; }

        private float padding = 35f;
        private float columnWidth;

        public GameCardControl(Type cardType, Vector2 size, int columns, int rows, Vector2 localPosition, string dataAsset = AssetManager.EmptyGameObjectDataAsset) :
            base(localPosition, dataAsset)
        {
            CardType = cardType;
            Columns = columns;
            Rows = rows;
            Size = size;

            columnWidth = Size.X / GamePlayer.MaxShipNumber;
            Positions = new KeyValuePair<float, GameObject>[GamePlayer.MaxShipNumber];
            for (int i = 0; i < Positions.Length; i++)
            {
                Positions[i] = new KeyValuePair<float, GameObject>(-Size.X * 0.5f + (i + 0.5f) * columnWidth, null);
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

            // If our mouse has a child that is a card of this type then we can shift UI around depending on where the mouse is
            // If not, then do nothing
        }

        /// <summary>
        /// Rebuilds our UI if necessary
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            // May not actually need to do this
            for (int i = 0; i < Positions.Length; i++)
            {
                // Sets all dead objects in our Positions array to be null instead
                KeyValuePair<float, GameObject> pair = Positions[i];
                if (pair.Value != null && !pair.Value.IsAlive)
                {
                    pair = new KeyValuePair<float, GameObject>(pair.Key, null);
                }
            }

            /*if (NeedsRebuild)
            {
                Rebuild();
                NeedsRebuild = false;
            }*/
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
            Debug.Assert(gameObjectToAdd is GameCard);
            Debug.Assert(gameObjectToAdd.GetType() == CardType);

            NeedsRebuild = true;

            // Check we have room left!
            Debug.Assert(GameObjects.ActiveObjectsCount < Rows * Columns);

            // In here we do the reshuffling of spacing and setting up references to added objects in our map
            float gameMouseXPos = GameMouse.Instance.InGamePosition.X;
            // Find the position closest to our gameMouseXPos
            // Add it
            // Reshuffle
            // What if something is there?
            // Maybe don't allow adding it, or add it to the closest free one?
            // Won't need rebuilding now

            // Can remove this once we fix our sizes!
            return base.AddObject(gameObjectToAdd, load, initialise);
        }

        #endregion

        #region UI Rebuilding

        /// <summary>
        /// Recalculate our spacing
        /// </summary>
        /*private void Rebuild()
        {
            Debug.Assert(NeedsRebuild);

            int index = 0;
            GameObject previous = null;

            foreach (GameObject card in GameObjects)
            {
                Debug.Assert(card is GameCard);
                if (index == 0)
                {
                    card.LocalPosition = new Vector2(0, (-Size.Y + card.Size.Y) * 0.5f + padding);
                }
                else
                {
                    DebugUtils.AssertNotNull(previous);

                    if (index % Columns == 0)
                    {
                        // We should start on a new row
                        card.LocalPosition = new Vector2(0, previous.LocalPosition.Y + card.Size.Y + padding);
                    }
                    else
                    {
                        int side = 2 * (index % Columns) - 1;
                        card.LocalPosition = previous.LocalPosition + new Vector2(side * (card.Size.X + padding), 0);
                    }
                }

                previous = card;
                index++;
            }
        }*/

        #endregion
    }
}
