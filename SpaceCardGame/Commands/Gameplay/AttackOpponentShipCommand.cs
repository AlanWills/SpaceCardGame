using _2DEngine;
using Microsoft.Xna.Framework;

namespace SpaceCardGame
{
    /// <summary>
    /// A command that is triggerd when we click on one of the ships during the battle phase.
    /// Adds appropriate UI and constantly checks for selection of opponent ships.
    /// Then handles the firing of the ship's turret.
    /// This command can be cancelled by right clicking anywhere, or left clicking on empty space.
    /// </summary>
    public class AttackOpponentShipCommand : ChooseShipCommand
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the ship card pair that is attacking
        /// </summary>
        private CardShipPair AttackingShipCardPair { get; set; }

        #endregion

        public AttackOpponentShipCommand(CardShipPair attackingShipPair) :
            base(attackingShipPair.ShipCard)
        {
            AttackingShipCardPair = attackingShipPair;
            ValidTargetFunction += AlwaysValid;         // All the opponent ships will be valid for now
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
                    AttackingShipCardPair.Ship.Turret.Attack(Target.Ship, false);
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
            AttackingShipCardPair.Ship.Turret.LocalRotation = SelectingLine.WorldRotation - AttackingShipCardPair.WorldRotation;

            // If our attacking ship is dead we kill this script
            // If our attacking ship has no shots left then we kill this script as our ship will not be able to attack any more
            if (!AttackingShipCardPair.IsAlive || !AttackingShipCardPair.Ship.Turret.CanFire)
            {
                Die();
            }
        }

        #endregion
    }
}
