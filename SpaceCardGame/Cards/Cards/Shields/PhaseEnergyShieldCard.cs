namespace SpaceCardGame
{
    /// <summary>
    /// Class for the phase energy shield card
    /// </summary>
    public class PhaseEnergyShieldCard : ShieldCard
    {
        public PhaseEnergyShieldCard(Player player, CardData shieldCardData) :
            base(player, shieldCardData)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// If our opponent has a ship as well as it's station, or it's station has a turret then this is a good card to play.
        /// Otherwise this is an average card to play.
        /// </summary>
        /// <param name="aiGameBoardSection"></param>
        /// <param name="otherGameBoardSection"></param>
        /// <returns></returns>
        public override AICardWorthMetric CalculateAIMetric(GameBoardSection aiGameBoardSection, GameBoardSection otherGameBoardSection)
        {
            if (otherGameBoardSection.ShipCardControl.ChildrenCount > 1 || !(otherGameBoardSection.ShipCardControl.FirstChild() as CardShipPair).Ship.Turret.IsDefaultTurret)
            {
                return AICardWorthMetric.kGoodCardToPlay;
            }

            return AICardWorthMetric.kAverageCardToPlay;
        }

        #endregion
    }
}
