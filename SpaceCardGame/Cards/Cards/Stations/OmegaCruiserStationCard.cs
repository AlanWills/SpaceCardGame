using SpaceCardGameData;

namespace SpaceCardGame
{
    public class OmegaCruiserStationCard : StationCard
    {
        public OmegaCruiserStationCard(Player player, CardData stationCardData) :
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
