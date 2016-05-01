namespace SpaceCardGame
{
    /// <summary>
    /// The weapon card for the default turret every ship has when first created.
    /// Creates a kinetic turret.
    /// </summary>
    public class DefaultTurretCard : WeaponCard
    {
        public DefaultTurretCard(WeaponCardData weaponCardData) :
            base(weaponCardData)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// Creates a kinetic turret
        /// </summary>
        /// <param name="weaponObjectDataAsset"></param>
        /// <returns></returns>
        public override Turret CreateTurret(string weaponObjectDataAsset)
        {
            return new KineticTurret(weaponObjectDataAsset);
        }

        #endregion
    }
}
