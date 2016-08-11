namespace SpaceCardGame
{
    public class HulkDreadnoughtStationCard : StationCard
    {
        public HulkDreadnoughtStationCard(Player player, CardData stationCardData) :
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
