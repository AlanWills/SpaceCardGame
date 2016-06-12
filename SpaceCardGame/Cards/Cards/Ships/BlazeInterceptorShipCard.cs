namespace SpaceCardGame
{
    public class BlazeInterceptorShipCard : ShipCard
    {
        public BlazeInterceptorShipCard(ShipCardData shipCardData) :
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
