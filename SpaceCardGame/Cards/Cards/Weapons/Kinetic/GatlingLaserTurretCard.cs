namespace SpaceCardGame
{
    /// <summary>
    /// The gatling laser turret creates a kinetic turret.
    /// </summary>
    public class GatlingLaserTurretCard : WeaponCard
    {
        public GatlingLaserTurretCard(CardData weaponCardData) :
            base(weaponCardData)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// Create a kinetic turret
        /// </summary>
        /// <param name="weaponObjectDataAsset"></param>
        /// <returns></returns>
        public override Turret CreateTurret(string weaponObjectDataAsset)
        {
            return new KineticTurret(weaponObjectDataAsset);
        }

        public override AICardWorthMetric CalculateAIMetric(GameBoardSection aiGameBoardSection, GameBoardSection otherGameBoardSection)
        {
            return AICardWorthMetric.kShouldNotPlayAtAll;
        }

        #endregion
    }
}
