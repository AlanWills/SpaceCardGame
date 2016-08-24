using SpaceCardGameData;

namespace SpaceCardGame
{
    public class AzmodaeusSupercruiserStationCard : StationCard
    {
        public AzmodaeusSupercruiserStationCard(Player player, CardData stationCardData) :
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
