using SpaceCardGameData;

namespace SpaceCardGame
{
    /// <summary>
    /// The laser beam turret fires beams at ships.
    /// </summary>
    public class LaserBeamTurretCard : WeaponCard
    {
        public LaserBeamTurretCard(Player player, CardData weaponCardData) :
            base(player, weaponCardData)
        {
            
        }

        #region Virtual Functions

        /// <summary>
        /// Creates a beam turret
        /// </summary>
        /// <param name="weaponObjectDataAsset"></param>
        /// <returns></returns>
        public override Turret CreateTurret(string weaponObjectDataAsset)
        {
            return new BeamTurret(weaponObjectDataAsset);
        }

        #endregion
    }
}
