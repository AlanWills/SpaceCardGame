using _2DEngine;
using Microsoft.Xna.Framework;

namespace SpaceCardGame
{
    /// <summary>
    /// A game object container for the cards in our game - will have slots for them to be laid in etc.
    /// </summary>
    public class GameBoard : GameObjectContainer
    {
        /// <summary>
        /// A reference to the player's game board section
        /// </summary>
        public PlayerGameBoardSection PlayerGameBoardSection { get; private set; }

        public GameBoard(Vector2 localPosition, string dataAsset = AssetManager.EmptyGameObjectDataAsset) :
            base(localPosition, dataAsset)
        {
            Size = ScreenManager.Instance.ScreenDimensions;
        }

        #region Virtual Functions

        /// <summary>
        /// Set up our game board section for each player
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            PlayerGameBoardSection = AddObject(new PlayerGameBoardSection(new Vector2(0, ScreenManager.Instance.ScreenDimensions.Y * 0.25f)));

            base.LoadContent();
        }

        #endregion
    }
}
