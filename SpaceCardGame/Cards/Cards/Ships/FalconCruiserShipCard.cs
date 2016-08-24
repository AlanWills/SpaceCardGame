using SpaceCardGameData;

namespace SpaceCardGame
{
    public class FalconCruiserShipCard : ShipCard
    {
        public FalconCruiserShipCard(Player player, CardData shipCardData) :
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
