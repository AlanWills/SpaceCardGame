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

        /// <summary>
        /// An array of references to the asteroids in our scene - this will be fixed size based on the density and the number of different asteroid textures we are using.
        /// The asteroids will be constantly recycled - when they leave the right hand side of the screen they will be reinserted into the left hand side with a new rigid body setup and position.
        /// </summary>
        private Asteroid[] Asteroids { get; set; }

        private const int asteroidDensity = 5;

        public Board(Vector2 localPosition, string dataAsset = "GameObjects\\Board.xml") :
            base(localPosition, dataAsset)
        {
            Size = ScreenManager.Instance.ScreenDimensions;
            UsesCollider = false;

            BattleScreen = ScreenManager.Instance.GetCurrentScreenAs<BattleScreen>();
        }

        #region Virtual Functions

        /// <summary>
        /// Create the asteroids and board sections.
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            // Add any extra decals first so that they appear underneath our board sections
            // This will prevent anything obstructing our cards and card objects
            Debug.Assert(Data is BoardData);
            BoardData data = Data as BoardData;

            // Create the fixed size array of asteroids
            Asteroids = new Asteroid[data.AsteroidTextureAssets.Count * asteroidDensity];

            int asteroidTypeIndex = 0;
            foreach (string asteroidTextureAsset in data.AsteroidTextureAssets)
            {
                for (int i = 0; i < asteroidDensity; ++i)
                {
                    float xPos = MathUtils.GenerateFloat(-Size.X * 0.5f, Size.X * 0.4f);
                    float yPos = MathUtils.GenerateFloat(-Size.Y * 0.5f, Size.Y * 0.5f);
                    Asteroids[asteroidTypeIndex * asteroidDensity + i] = AddChild(new Asteroid(new Vector2(xPos, yPos), asteroidTextureAsset));
                }

                asteroidTypeIndex++;
            }

            // Add the board sections to this
            PlayerBoardSection = AddChild(new PlayerBoardSection(BattleScreen.Player, new Vector2(0, Size.Y * 0.25f)));
            OpponentBoardSection = AddChild(new OpponentBoardSection(BattleScreen.Opponent, new Vector2(0, Size.Y * 0.25f)));

            base.LoadContent();
        }

        /// <summary>
        /// Loops through our asteroids and moves any that have gone off the side of the RHS of the board back over onto the LHS.
        /// Then gives the asteroid a new y position and speed (to simulate more is happening than actually is).
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            foreach (Asteroid asteroid in Asteroids)
            {
                if (asteroid.LocalPosition.X > (Size.X + asteroid.Size.X) * 0.5f)
                {
                    float yPos = MathUtils.GenerateFloat(-Size.Y * 0.5f, Size.Y * 0.5f);
                    float speed = MathUtils.GenerateFloat(1, 20);

                    asteroid.LocalPosition = new Vector2(-asteroid.LocalPosition.X, yPos);
                    asteroid.RigidBody.LinearVelocity = new Vector2(speed, 0);
                }
            }
        }

        #endregion
    }
}