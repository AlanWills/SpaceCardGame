namespace SpaceCardGame
{
    public class MunitionsLoaderShipCard : ShipCard
    {
        public MunitionsLoaderShipCard(Player player, CardData shipCardData) :
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
