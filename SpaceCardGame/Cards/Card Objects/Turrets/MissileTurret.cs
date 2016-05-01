using _2DEngine;

namespace SpaceCardGame
{
    /// <summary>
    /// The missile turret fires missiles and implements the creation of them when firing.
    /// </summary>
    public class MissileTurret : Turret
    {
        public MissileTurret(string turretDataAsset) :
            base(turretDataAsset)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// Create a missile for firing.
        /// </summary>
        protected override void CreateBullet(GameObject target)
        {
            ScreenManager.Instance.CurrentScreen.AddInGameUIObject(new Missile(target, WorldPosition, BulletData), true, true);
        }

        #endregion
    }
}
