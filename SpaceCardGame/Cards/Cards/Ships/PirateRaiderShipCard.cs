using SpaceCardGameData;

namespace SpaceCardGame
{
    public class PirateRaiderShipCard : ShipCard
    {
        public PirateRaiderShipCard(Player player, CardData shipCardData) :
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
