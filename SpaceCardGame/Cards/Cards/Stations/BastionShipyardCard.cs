namespace SpaceCardGame
{
    public class BastionShipyardCard : StationCard
    {
        public BastionShipyardCard(CardData stationCardData) :
            base(stationCardData)
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
