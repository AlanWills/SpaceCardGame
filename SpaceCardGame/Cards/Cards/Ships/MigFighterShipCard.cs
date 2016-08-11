namespace SpaceCardGame
{
    public class MigFighterShipCard : ShipCard
    {
        public MigFighterShipCard(Player player, CardData shipCardData) :
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
