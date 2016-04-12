using _2DEngine;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// This script just draws out the player's beign given their initial hands a bit so we can see some cool animations.
    /// WHen completed, it begins a new turn.
    /// </summary>
    public class NewGameScript : Script
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the battle screen, for ease
        /// </summary>
        private BattleScreen BattleScreen { get; set; }

        private float currentTimeBetweenDraws = 0;
        private const float delay = 0.35f;

        #endregion

        public NewGameScript() :
            base()
        {
            Debug.Assert(ScreenManager.Instance.CurrentScreen is BattleScreen);
            BattleScreen = ScreenManager.Instance.CurrentScreen as BattleScreen;
            BattleScreen.ShouldHandleInput.Value = false;
        }

        #region Virtual Functions

        /// <summary>
        /// Draws cards first for the player, then the opponent, then finishes
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

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
                    // Draw a card for the oppoent if they do not have enough
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

        /// <summary>
        /// When this script dies it restores input to the battle screen and triggers a new turn
        /// </summary>
        public override void Die()
        {
            base.Die();

            BattleScreen.ShouldHandleInput.Value = true;
            BattleScreen.ProgressTurnButton.ClickableModule.ForceClick();
        }

        #endregion
    }
}
