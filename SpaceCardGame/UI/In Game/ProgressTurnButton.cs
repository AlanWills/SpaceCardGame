using CelesteEngine;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A wrapper around an ordinary button for our BattleScreen.
    /// It has some extra logic for disabling itself when we are still resolving bullets etc.
    /// </summary>
    public class ProgressTurnButton : Button
    {
        #region Properties and Fields

        /// <summary>
        /// A flag to indicate whether are able to progress to the next phase of the turn
        /// </summary>
        public bool CanProgress { get; private set; }

        #endregion

        public ProgressTurnButton(Vector2 localPosition, string textureAsset = AssetManager.DefaultButtonTextureAsset) :
            base("Start Battle", localPosition, textureAsset)
        {
            OnPressedSFXAsset = "UI\\ProgressTurnButtonPressedSound";
        }

        #region Virtual Functions

        /// <summary>
        /// Set up the click callback to update the text.
        /// We do this here so it is definitely set up last after any others
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            ClickableModule.OnLeftClicked += OnLeftClickUpdateText;
        }

        /// <summary>
        /// Disables our button if we are in the Battle phase and still have unresolved bullets.
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            // See if we have any explosion animations left to finish in our battle screen
            // Loop through both player boards and check to see if any turret has unresolved bullets
            // If one exists, disable this button

            BattleScreen battleScreen = ScreenManager.Instance.GetCurrentScreenAs<BattleScreen>();

            if (battleScreen.FindInGameUIObject<Explosion>(x => x is Explosion) != null)
            {
                CanProgress = false;
                return;
            }

            foreach (CardShipPair shipPair in battleScreen.Board.NonActivePlayerBoardSection.GameBoardSection.ShipCardControl)
            {
                if (shipPair.Ship.Turret.ChildrenCount > 0)
                {
                    CanProgress = false;
                    return;
                }
            }

            foreach (CardShipPair shipPair in battleScreen.Board.ActivePlayerBoardSection.GameBoardSection.ShipCardControl)
            {
                if (shipPair.Ship.Turret.ChildrenCount > 0)
                {
                    CanProgress = false;
                    return;
                }
            }

            CanProgress = true;
        }

        #endregion

        #region Click Callbacks

        /// <summary>
        /// A callback for updating our text when we are left clicked
        /// </summary>
        /// <param name="baseObject"></param>
        private void OnLeftClickUpdateText(BaseObject baseObject)
        {
            Label.Text = GetTurnStateButtonText();
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Gets the appropriate text for the turn state button based on the current turn state
        /// </summary>
        /// <returns></returns>
        private string GetTurnStateButtonText()
        {
            BattleScreen battleScreen = ScreenManager.Instance.GetCurrentScreenAs<BattleScreen>();

            switch (battleScreen.TurnState)
            {
                case TurnState.kPlaceCards:
                    return "Start Battle";

                case TurnState.kBattle:
                    return "End Turn";

                default:
                    Debug.Fail("");
                    return "";
            }
        }

        #endregion
    }
}
