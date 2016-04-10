namespace SpaceCardGameData
{
    public class ShieldData : ShipAddOnData
    {
        /// <summary>
        /// An int value representing the maximum strength of our shield
        /// </summary>
        public int MaxShieldStrength { get; set; }

        /// <summary>
        /// An int value representing the amount our shield recharges inately per turn
        /// </summary>
        public int ShieldRechargePerTurn { get; set; }
    }
}
