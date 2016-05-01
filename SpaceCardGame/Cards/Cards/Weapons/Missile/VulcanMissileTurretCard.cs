namespace SpaceCardGame
{
    /// <summary>
    /// The Vulcan missile turret fires missiles.
    /// Also, if the player pays one fuel it can fire one more time.
    /// </summary>
    public class VulcanMissileTurretCard : WeaponCard
    {
        public VulcanMissileTurretCard(WeaponCardData weaponCardData) :
            base(weaponCardData)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// Creates a missile turret.
        /// </summary>
        /// <param name="weaponObjectDataAsset"></param>
        /// <returns></returns>
        public override Turret CreateTurret(string weaponObjectDataAsset)
        {
            return new MissileTurret(weaponObjectDataAsset);
        }

        #endregion
    }
}
