namespace SpaceCardGameData
{
    public class TurretData : ShipAddOnData
    {
        /// <summary>
        /// The data asset for our projectiles
        /// </summary>
        public string ProjectileDataAsset { get; set; }

        /// <summary>
        /// The number of shots our turret can fire each turn
        /// </summary>
        public int ShotsPerTurn { get; set; }
    }
}
