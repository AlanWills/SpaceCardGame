namespace SpaceCardGame
{
    public class VenatorCruiserShipCard : ShipCard
    {
        public VenatorCruiserShipCard(CardData shipCardData) :
            base(shipCardData)
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
