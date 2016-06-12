namespace SpaceCardGame
{
    public class VenatorCruiserShipCard : ShipCard
    {
        public VenatorCruiserShipCard(CardData shipCardData) :
            base(shipCardData)
        {

        }

        #region Virtual Functions

        public override AICardWorthMetric CalculateAIMetric()
        {
            return AICardWorthMetric.kShouldNotPlayAtAll;
        }

        #endregion
    }
}
