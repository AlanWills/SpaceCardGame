using _2DEngine;
using Microsoft.Xna.Framework;
using System.Collections;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A script which uses a targeting line and loops through a collection of CardShipPair
    /// to find a valid target for an inputted card.
    /// Marked as abstract because we need to specifically implement this for either friendly or enemy ships,
    /// by setting the ShipPairsToLookThrough.
    /// </summary>
    public abstract class ChooseShipCommand : Command
    {
        #region Properties and Fields

        /// <summary>
        /// A stored reference to our battle screen (which will be our parent).
        /// Saves us having to cast the ParentScreen every frame
        /// </summary>
        protected BattleScreen BattleScreen { get; private set; }

        /// <summary>
        /// A reference to the card object pair we need to choose a target for.
        /// </summary>
        protected CardObjectPair CardToChooseTargetFor { get; private set; }

        /// <summary>
        /// A reference to the line we use to indicate we're selecting
        /// </summary>
        protected TargetingLine SelectingLine { get; private set; }

        /// <summary>
        /// A reference to a card ship pair which we are currently targeting
        /// </summary>
        protected CardShipPair Target { get; private set; }

        /// <summary>
        /// The game object container we will look through to find a valid target
        /// </summary>
        protected IEnumerable ContainerToLookThrough { private get; set; }

        #endregion

        public ChooseShipCommand(CardObjectPair cardObjectPair) :
            base()
        {
            CardToChooseTargetFor = cardObjectPair;
        }

        #region Virtual Functions

        /// <summary>
        /// Creates a reference to the battle screen and sets up our targeting line
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            Debug.Assert(ParentScreen is BattleScreen);
            BattleScreen = ParentScreen as BattleScreen;

            SelectingLine = BattleScreen.AddInGameUIObject(new TargetingLine(CardToChooseTargetFor.WorldPosition), true, true);
        }

        /// <summary>
        /// Loops through our friendly ships and attempts to select one that passes the validity check for our card.
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            DebugUtils.AssertNotNull(ContainerToLookThrough);

            // If we right click, or left click on empty space we cancel the script
            if (GameMouse.Instance.IsClicked(MouseButton.kRightButton))
            {
                Die();
            }

            Target = null;
            foreach (CardShipPair pair in ContainerToLookThrough)
            {
                // We check intersection with mouse position here, because the object may not have actually had it's HandleInput called yet
                // Could do this stuff in the Update loop, but this is really what this function is for so do this CheckIntersects instead for clarity
                DebugUtils.AssertNotNull(pair.Card.Collider);
                if (pair != CardToChooseTargetFor && (pair.Card.Collider.CheckIntersects(mousePosition) || pair.CardObject.Collider.CheckIntersects(mousePosition)))
                {
                    // Check to see whether this current object is a valid match for the card we want to find a target for
                    if (CardToChooseTargetFor.Card.CanUseOn(pair))
                    {
                        Target = pair;
                        break;
                    }
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

            // Set the target position of the attacking line
            if (Target != null)
            {
                SelectingLine.TargetPosition = Target.WorldPosition;
            }
            else
            {
                SelectingLine.TargetPosition = GameMouse.Instance.InGamePosition;
            }
        }

        /// <summary>
        /// Kills our attack line image
        /// </summary>
        public override void Die()
        {
            base.Die();

            SelectingLine.Die();
        }

        #endregion
    }
}
