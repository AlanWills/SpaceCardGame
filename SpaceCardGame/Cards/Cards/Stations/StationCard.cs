namespace SpaceCardGame
{
    /// <summary>
    /// A general class to represent a station, which is a unique form of Ship
    /// </summary>
    public abstract class StationCard : ShipCard
    {
        public StationCard(Player player, CardData stationCardData) :
            base(player, stationCardData)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// Station cards create a CardStationPair
        /// </summary>
        /// <returns></returns>
        public override CardObjectPair CreateCardObjectPair()
        {
            return new CardStationPair(this);
        }

        #endregion
    }
}
