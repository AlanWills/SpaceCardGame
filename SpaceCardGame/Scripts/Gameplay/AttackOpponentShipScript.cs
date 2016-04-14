using _2DEngine;
using Microsoft.Xna.Framework;

namespace SpaceCardGame
{
    /// <summary>
    /// A script that is triggerd when we click on one of the ships during the battle phase.
    /// Adds appropriate UI and constantly checks for selection of opponent ships.
    /// Then handles the firing of the ship's turret.
    /// This script can be cancelled by right clicking anywhere, or left clicking on empty space.
    /// </summary>
    public class AttackOpponentShipScript : ChooseShipScript
    {
        #region Properties and Fields

        /// <summary>
        /// The selected ship which will attack.
        /// </summary>
        private CardShipPair AttackingShipPair { get; set; }

        #endregion

        public AttackOpponentShipScript(CardShipPair attackingShipPair) :
            base(attackingShipPair)
        {
            AttackingShipPair = attackingShipPair;
        }

        #region Virtual Functions

        /// <summary>
        /// Creates a reference to the battle screen for this class
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            ContainerToLookThrough = BattleScreen.Board.NonActivePlayerBoardSection.GameBoardSection.ShipCardControl;
            SelectingLine.Colour.Value = Color.Red;
        }

        /// <summary>
        /// Handles the selection of the target ship as well as the cancelling of the attack script.
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);
            
            // We handle what happens if we have left clicked on our mouse
            if (GameMouse.Instance.IsClicked(MouseButton.kLeftButton))
            {
                if (Target != null)
                {
                    // We have left clicked on a ship, so attack it
                    AttackingShipPair.Ship.Turret.Attack(Target.Ship);
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

            // Then use the rotation of the line to set the turret rotation - this is more efficient that calling RotateToTarget and will give a better result
            AttackingShipPair.Ship.Turret.LocalRotation = SelectingLine.WorldRotation - AttackingShipPair.WorldRotation;

            // If our attacking ship has no shots left then we just kill this script as our ship will not be able to attack any more
            if (!AttackingShipPair.Ship.Turret.CanFire)
            {
                Die();
            }
        }

        #endregion
    }
}
