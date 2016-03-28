using _2DEngine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A game object container for the two player game board sections in our game
    /// They perform most of the logic, but this is a nice grouping class to keep things componentised
    /// </summary>
    public class Board : GameObject
    {
        /// <summary>
        /// A local reference to the battle screen just for ease
        /// </summary>
        private BattleScreen BattleScreen { get; set; }

        /// <summary>
        /// A reference to the player's board section
        /// </summary>
        public PlayerBoardSection PlayerBoardSection { get; private set; }

        /// <summary>
        /// A reference to the opponent's board section
        /// </summary>
        public PlayerBoardSection OpponentBoardSection { get; private set; }

        /// <summary>
        /// Returns the appropriate board section based on the active player
        /// </summary>
        public PlayerBoardSection CurrentActivePlayerBoardSection
        {
            get
            {
                if (BattleScreen.ActivePlayer == BattleScreen.Player)
                {
                    return PlayerBoardSection;
                }
                else
                {
                    return OpponentBoardSection;
                }
            }
        }

        public Board(Vector2 localPosition, string dataAsset = AssetManager.EmptyGameObjectDataAsset) :
            base(localPosition, dataAsset)
        {
            Size = ScreenManager.Instance.ScreenDimensions;

            Debug.Assert(ScreenManager.Instance.CurrentScreen is BattleScreen);
            BattleScreen = ScreenManager.Instance.CurrentScreen as BattleScreen;

            // Add the objects to the screen
            PlayerBoardSection = BattleScreen.AddGameObject(new PlayerBoardSection(BattleScreen.Player, new Vector2(0, Size.Y * 0.25f)), true, true);
            OpponentBoardSection = BattleScreen.AddGameObject(new PlayerBoardSection(BattleScreen.Opponent, new Vector2(0, Size.Y * 0.25f)), true, true);
            OpponentBoardSection.LocalRotation = MathHelper.Pi;     // Rotate the opponent's board screen so that it's facing down screen

            // But set the parent to this object, so we can rotate both by just rotating this
            PlayerBoardSection.SetParent(this);
            OpponentBoardSection.SetParent(this);
        }
    }
}
