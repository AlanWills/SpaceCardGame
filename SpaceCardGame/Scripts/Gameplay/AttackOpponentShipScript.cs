using _2DEngine;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A script that is triggerd when we click on one of the ships during the battle phase.
    /// Adds appropriate UI and constantly checks for selection of opponent ships.
    /// Then handles the firing of the ship's turret.
    /// This script can be cancelled by right clicking anywhere, or left clicking on empty space.
    /// </summary>
    public class AttackOpponentShipScript : Script
    {
        #region Properties and Fields

        /// <summary>
        /// The selected ship which will attack.
        /// </summary>
        private Ship AttackingShip { get; set; }

        /// <summary>
        /// A stored reference to our battle screen (which will be our parent).
        /// Saves us having to cast the ParentScreen every frame
        /// </summary>
        private BattleScreen BattleScreen { get; set; }

        /// <summary>
        /// A reference to the image we use to indicate we're attacking
        /// </summary>
        private Image AttackLineImage { get; set; }

        /// <summary>
        /// A reference to a game object which we are currently targeting
        /// </summary>
        private GameObject Target { get; set; }

        #endregion

        public AttackOpponentShipScript(Ship attackingShip) :
            base()
        {
            AttackingShip = attackingShip;
        }

        #region Virtual Functions

        /// <summary>
        /// Creates a reference to the battle screen for this class
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            Debug.Assert(ParentScreen is BattleScreen);
            BattleScreen = ParentScreen as BattleScreen;

            AttackLineImage = BattleScreen.AddInGameUIObject(new Image(Vector2.Zero, "Sprites\\UI\\AttackLine"), true, true);
            AttackLineImage.Colour = Color.Red;
        }

        /// <summary>
        /// Handles the selection of the target ship as well as the cancelling of the attack script.
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            // If we right click, or left click on empty space we cancel the script
            if (GameMouse.Instance.IsClicked(MouseButton.kRightButton))
            {
                Die();
            }

            Target = null;
            foreach (CardObjectPair pair in BattleScreen.Board.NonActivePlayerBoardSection.PlayerGameBoardSection.PlayerShipCardControl)
            {
                // We check intersection with mouse position here, because the object may not have actually had it's HandleInput called yet
                // Could do this stuff in the Update loop, but this is really what this function is for so do this CheckIntersects instead for clarity 
                if (pair.CardObject != AttackingShip && pair.CardObject.Collider.CheckIntersects(mousePosition))
                {
                    Target = pair.CardObject;
                    break;
                }
            }

            // We handle what happens if we have left clicked on our mouse
            if (GameMouse.Instance.IsClicked(MouseButton.kLeftButton))
            {
                if (Target != null)
                {
                    // We have left clicked on a ship, so attack it
                    AttackingShip.Turret.Attack(Target);
                }
                else
                {
                    // We have left clicked on empty space so we should just kill this script as the player has cancelled the attack
                    Die();
                }
            }
        }

        /// <summary>
        /// Rotates the turrets on the attacking ship to point at the mouse position.
        /// Nothing huge but a nice piece of UI.
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            Vector2 attackingShipWorldPos = AttackingShip.WorldPosition;
            Vector2 mousePosition = GameMouse.Instance.InGamePosition;

            // Rotate the attacking ship's turrets to be pointing at the mouse whilst we are hovering
            float mouseAngle = MathUtils.AngleBetweenPoints(attackingShipWorldPos, mousePosition);
            AttackingShip.Turret.LocalRotation = mouseAngle - AttackingShip.WorldRotation;

            if (Target != null)
            {
                Vector2 targetWorldPos = Target.WorldPosition;
                float targetAngle = MathUtils.AngleBetweenPoints(attackingShipWorldPos, targetWorldPos);

                AttackLineImage.LocalRotation = targetAngle;
                AttackLineImage.LocalPosition = (attackingShipWorldPos + targetWorldPos) * 0.5f;
                AttackLineImage.Size = new Vector2(AttackLineImage.Size.X, (attackingShipWorldPos - targetWorldPos).Length());
            }
            else
            {
                AttackLineImage.LocalRotation = mouseAngle;
                AttackLineImage.LocalPosition = (attackingShipWorldPos + mousePosition) * 0.5f;
                AttackLineImage.Size = new Vector2(AttackLineImage.Size.X, (attackingShipWorldPos - mousePosition).Length());
            }

            // If our attacking ship has no shots left then we just kill this script as our ship will not be able to attack any more
            if (AttackingShip.Turret.ShotsLeft <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// Kills our attack line image
        /// </summary>
        public override void Die()
        {
            base.Die();

            AttackLineImage.Die();
        }

        #endregion
    }
}
