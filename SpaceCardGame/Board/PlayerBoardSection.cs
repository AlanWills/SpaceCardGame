using _2DEngine;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A simple class designed just to group the PlayerGameBoardSection and PlayerUIBoardSection in one class
    /// </summary>
    public class PlayerBoardSection : GameObject
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to this player's section of the game board that deals with GameObjects
        /// </summary>
        public GameBoardSection GameBoardSection { get; private set; }

        /// <summary>
        /// A reference to this player's section of the game board that deals with UIObjects
        /// </summary>
        public UIBoardSection UIBoardSection { get; private set; }

        #endregion

        public PlayerBoardSection(GamePlayer player, Vector2 localPosition, string dataAsset = AssetManager.EmptyGameObjectDataAsset) :
            base(localPosition, dataAsset)
        {
            Size = new Vector2(ScreenManager.Instance.ScreenDimensions.X, ScreenManager.Instance.ScreenDimensions.Y * 0.5f);

            Debug.Assert(ScreenManager.Instance.CurrentScreen is BattleScreen);
            BattleScreen battleScreen = ScreenManager.Instance.CurrentScreen as BattleScreen;

            // Add the objects to the screen
            GameBoardSection = AddChild(new GameBoardSection(player, Vector2.Zero));
            UIBoardSection = AddChild(new UIBoardSection(player, Vector2.Zero));
        }
    }
}
