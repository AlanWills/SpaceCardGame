namespace SpaceCardGame
{
    public class BlazeInterceptorShipCard : ShipCard
    {
        public BlazeInterceptorShipCard(CardData shipCardData) :
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
