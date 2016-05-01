using _2DEngine;

namespace SpaceCardGame
{
    public class KineticTurret : Turret
    {
        /// <summary>
        /// The kinetic turret fires projectiles and implements the creation of them when firing.
        /// </summary>
        public KineticTurret(string turretDataAsset) :
            base(turretDataAsset)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// Create a missile for firing.
        /// </summary>
        protected override void CreateBullet(GameObject target)
        {
            ScreenManager.Instance.CurrentScreen.AddInGameUIObject(new Bullet(target, WorldPosition, BulletData), true, true);
        }

        #endregion
    }
}
