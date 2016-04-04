using _2DEngine;
using Microsoft.Xna.Framework;

namespace SpaceCardGame
{
    /// <summary>
    /// A class used to indicate a line between one object and a target.
    /// Automatically adjusts the size and position based on the target position.
    /// </summary>
    public class TargetingLine : Image
    {
        #region Properties and Fields

        /// <summary>
        /// The position of the fixed end of our line
        /// </summary>
        private Vector2 FixedPosition { get; set; }

        /// <summary>
        /// The position of the variavle end of the line.
        /// When set, recalculates the angle, size and position of this targeting line.
        /// </summary>
        private Vector2 targetPosition;
        public Vector2 TargetPosition
        {
            get { return targetPosition; }
            set
            {
                targetPosition = value;

                CalculatePositionAndAngle();
            }
        }

        #endregion

        public TargetingLine(Vector2 fixedPosition) :
            base(Vector2.Zero, "Sprites\\UI\\AttackLine")
        {
            FixedPosition = fixedPosition;
        }

        #region Utility Functions

        /// <summary>
        /// Uses our fixed and target position to recalculat the angle, position and size of this line.
        /// This will draw the line between the fixed and target positions.
        /// </summary>
        private void CalculatePositionAndAngle()
        {
            float targetAngle = MathUtils.AngleBetweenPoints(FixedPosition, TargetPosition);

            LocalRotation = targetAngle;
            LocalPosition = (FixedPosition + TargetPosition) * 0.5f;
            Size = new Vector2(Size.X, (FixedPosition - TargetPosition).Length());
        }

        #endregion
    }
}
