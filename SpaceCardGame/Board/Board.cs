using _2DEngine;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using _2DEngineData;
using SpaceCardGameData;

namespace SpaceCardGame
{
    /// <summary>
    /// A game object container for the two player game board sections in our game
    /// They perform most of the logic, but this is a nice grouping class to keep things componentised
    /// </summary>
    public class Board : Image
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
        public OpponentBoardSection OpponentBoardSection { get; private set; }

        /// <summary>
        /// Returns the appropriate board section based on the active player
        /// </summary>
        public PlayerBoardSection ActivePlayerBoardSection
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

        /// <summary>
        /// Returns the appropriate board section based on the non active player
        /// </summary>
        public PlayerBoardSection NonActivePlayerBoardSection
        {
            get
            {
                if (BattleScreen.ActivePlayer == BattleScreen.Player)
                {
                    return OpponentBoardSection;
                }
                else
                {
                    return PlayerBoardSection;
                }
            }
        }

        public Board(Vector2 localPosition, string backgroundTextureAsset = "Backgrounds\\Background-1") :
            base(localPosition, backgroundTextureAsset)
        {
            Size = ScreenManager.Instance.ScreenDimensions;
            UsesCollider = false;

            BattleScreen = ScreenManager.Instance.GetCurrentScreenAs<BattleScreen>();
        }

        #region Virtual Functions

        /// <summary>
        /// Create the board sections.
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            // Add the board sections to this
            PlayerBoardSection = AddChild(new PlayerBoardSection(BattleScreen.Player, new Vector2(0, Size.Y * 0.25f)));
            OpponentBoardSection = AddChild(new OpponentBoardSection(BattleScreen.Opponent, new Vector2(0, Size.Y * 0.25f)));

            base.LoadContent();
        }

        #endregion
    }
}