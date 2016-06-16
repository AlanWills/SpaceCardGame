namespace SpaceCardGame
{
    public class BastionShipyardCard : StationCard
    {
        public BastionShipyardCard(Player player, CardData stationCardData) :
            base(player, stationCardData)
        {

        }

        #region Virtual Functions

        public override AICardWorthMetric CalculateAIMetric(GameBoardSection aiGameBoardSection, GameBoardSection otherGameBoardSection)
        {
            return AICardWorthMetric.kShouldNotPlayAtAll;
        }

        #endregion
    }
}
