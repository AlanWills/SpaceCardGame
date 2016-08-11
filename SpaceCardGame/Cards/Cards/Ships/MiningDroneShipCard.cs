namespace SpaceCardGame
{
    public class MiningDroneShipCard : ShipCard
    {
        public MiningDroneShipCard(Player player, CardData shipCardData) :
            base(player, shipCardData)
        {

        }

        #region Virtual Functions

        public override AICardWorthMetric CalculateAIMetric(GameBoardSection aiGameBoardSection, GameBoardSection otherGameBoardSection)
        {
            return AICardWorthMetric.kAverageCardToPlay;
        }

        #endregion
    }
}
