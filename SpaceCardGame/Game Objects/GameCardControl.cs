using _2DEngine;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A general class for managing cards in our screen.
    /// Handles spacing and repositioning after insertions.
    /// Mimics the grid control.
    /// Also allows us to restrict the maximum number of elements we can add.
    /// </summary>
    public class GameCardControl : GameObjectContainer
    {
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

        private float padding = 35f;

        public GameCardControl(Vector2 size, int columns, int rows, Vector2 localPosition, string dataAsset = AssetManager.EmptyGameObjectDataAsset) :
            base(localPosition, dataAsset)
        {
            Columns = columns;
            Rows = rows;
            Size = size;
        }

        #region Virtual Functions

        /// <summary>
        /// Rebuilds our UI if necessary
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            if (NeedsRebuild)
            {
                Rebuild();
                NeedsRebuild = false;
            }
        }

        /// <summary>
        /// Add a game card to this control.
        /// Also, indicates that our cards need rebuilding.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObjectToAdd"></param>
        /// <param name="load"></param>
        /// <param name="initialise"></param>
        /// <returns></returns>
        public override T AddObject<T>(T gameObjectToAdd, bool load = false, bool initialise = false)
        {
            Debug.Assert(gameObjectToAdd is GameCard);
            NeedsRebuild = true;

            // Check we have room left!
            Debug.Assert(GameObjects.ActiveObjectsCount < Rows * Columns);

            // Can remove this once we fix our sizes!
            return base.AddObject(gameObjectToAdd, load, initialise);
        }

        #endregion

        #region UI Rebuilding

        /// <summary>
        /// Recalculate our spacing
        /// </summary>
        private void Rebuild()
        {
            Debug.Assert(NeedsRebuild);

            int index = 0;
            GameObject previous = null;

            foreach (GameObject card in GameObjects)
            {
                Debug.Assert(card is GameCard);
                if (index == 0)
                {
                    card.LocalPosition = new Vector2((-Size.X + card.Size.X) * 0.5f + padding, (-Size.Y + card.Size.Y) * 0.5f + padding);
                }
                else
                {
                    DebugUtils.AssertNotNull(previous);

                    if (index % Columns == 0)
                    {
                        // We should start on a new row
                        card.LocalPosition = new Vector2((-Size.X + card.Size.X) * 0.5f + padding, previous.LocalPosition.Y + card.Size.Y + padding);
                    }
                    else
                    {
                        card.LocalPosition = previous.LocalPosition + new Vector2(card.Size.X + padding, 0);
                    }
                }

                previous = card;
                index++;
            }
        }

        #endregion
    }
}
