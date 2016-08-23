using CelesteEngine;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// This command just draws out the player's beign given their initial hands a bit so we can see some cool animations.
    /// WHen completed, it begins a new turn.
    /// </summary>
    public class NewGameCommand : Command
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the battle screen, for ease
        /// </summary>
        private BattleScreen BattleScreen { get; set; }

        private float currentTimeBetweenDraws = 0;
        private const float delay = 0.35f;

        #endregion

        public NewGameCommand() :
            base()
        {
            Debug.Assert(ParentScreen is BattleScreen);
            BattleScreen = ParentScreen as BattleScreen;
            GameHandleInput = false;
        }

        #region Virtual Functions

        /// <summary>
        /// Draws cards first for the player, then the opponent, then finish
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            AddInitialCards(elapsedGameTime);
        }

        /// <summary>
        /// When this script dies it restores input to the battle screen and triggers a new turn
        /// </summary>
        public override void Die()
        {
            base.Die();

            BattleScreen.ShouldHandleInput = true;
            BattleScreen.ProgressTurnButton.ClickableModule.ForceClick();
            BattleScreen.ProgressTurnButton.Show();
        }

        /// <summary>
        /// A virtual function for adding cards for the players hands at the beginning of the game
        /// </summary>
        protected virtual void AddInitialCards(float elapsedGameTime)
        {
            // Check to see if enough time has passed since the last card draw
            currentTimeBetweenDraws += elapsedGameTime;
            if (currentTimeBetweenDraws > delay)
            {
                if (BattleScreen.Player.CurrentHand.Count < 6)
                {
                    // Draw a card for the player if they do not have enough
                    BattleScreen.Player.DrawCard();
                    currentTimeBetweenDraws = 0;
                }
                else if (BattleScreen.Opponent.CurrentHand.Count < 6)
                {
                    // Draw a card for the opponent if they do not have enough
                    BattleScreen.Opponent.DrawCard();
                    currentTimeBetweenDraws = 0;
                }
                else
                {
                    // Both players have enough so we can finish
                    Die();
                }
            }
        }

        #endregion
    }
}
