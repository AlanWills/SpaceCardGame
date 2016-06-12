namespace SpaceCardGame
{
    public class WaspFighterShipCard : ShipCard
    {
        public WaspFighterShipCard(CardData shipCardData) :
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