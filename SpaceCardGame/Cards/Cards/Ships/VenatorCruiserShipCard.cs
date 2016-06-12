namespace SpaceCardGame
{
    public class VenatorCruiserShipCard : ShipCard
    {
        public VenatorCruiserShipCard(ShipCardData shipCardData) :
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
