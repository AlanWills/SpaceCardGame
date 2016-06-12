namespace SpaceCardGame
{
    public class BastionShipyardCard : StationCard
    {
        public BastionShipyardCard(CardData stationCardData) :
            base(stationCardData)
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
