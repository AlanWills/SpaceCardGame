using _2DEngine;

namespace SpaceCardGame
{
    /// <summary>
    /// The beam turret fires beams and implements the creation of them when firing.
    /// </summary>
    public class BeamTurret : Turret
    {
        public BeamTurret(string turretDataAsset) :
            base(turretDataAsset)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// Create a beam for firing.
        /// </summary>
        protected override void CreateBullet(GameObject target)
        {
            
        }

        #endregion
    }
}
