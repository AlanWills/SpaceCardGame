using _2DEngine;
using CardGameEngine;

namespace SpaceCardGame
{
    /// <summary>
    /// The screen where our main gameplay will take place between a player and an opponent
    /// </summary>
    public class BattleScreen : GameplayScreen
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to our human playable character
        /// </summary>
        private Player Player { get; set; }

        /// <summary>
        /// A reference to our HUD
        /// </summary>
        private BattleScreenHUD HUD { get; set; }

        private float timer = 0;
        private int index = 0;

        #endregion

        public BattleScreen(Deck playerChosenDeck, string screenDataAsset) :
            base(screenDataAsset)
        {
            Player = new Player(playerChosenDeck);
        }

        #region Virtual Functions

        /// <summary>
        /// Set up our game HUD.
        /// </summary>
        protected override void AddInitialUI()
        {
            base.AddInitialUI();

            HUD = AddScreenUIObject(new BattleScreenHUD(Player, AssetManager.DefaultEmptyPanelTextureAsset));
        }

        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            timer += elapsedGameTime;
            if (timer > 0.5f && index < 6)
            {
                Player.Draw();
                timer = 0;
                index++;
            }
        }

        #endregion
    }
}