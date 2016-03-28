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
        public PlayerGameBoardSection PlayerGameBoardSection { get; private set; }

        /// <summary>
        /// A reference to this player's section of the game board that deals with UIObjects
        /// </summary>
        public PlayerUIBoardSection PlayerUIBoardSection { get; private set; }

        #endregion

        public PlayerBoardSection(GamePlayer player, Vector2 localPosition, string dataAsset = AssetManager.EmptyGameObjectDataAsset) :
            base(localPosition, dataAsset)
        {
            Size = new Vector2(ScreenManager.Instance.ScreenDimensions.X, ScreenManager.Instance.ScreenDimensions.Y * 0.5f);

            Debug.Assert(ScreenManager.Instance.CurrentScreen is BattleScreen);
            BattleScreen battleScreen = ScreenManager.Instance.CurrentScreen as BattleScreen;

            // Add the objects to the screen
            PlayerGameBoardSection = battleScreen.AddGameObject(new PlayerGameBoardSection(player, Vector2.Zero), true, true);
            PlayerUIBoardSection = battleScreen.AddScreenUIObject(new PlayerUIBoardSection(player, Vector2.Zero), true, true);

            // But set the parent to this object, so we can rotate both by just rotating this
            PlayerGameBoardSection.SetParent(this);
            PlayerUIBoardSection.SetParent(this);
        }
    }
}
