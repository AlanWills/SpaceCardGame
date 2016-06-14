namespace SpaceCardGame
{
    public class VenatorCruiserShipCard : ShipCard
    {
        public VenatorCruiserShipCard(CardData shipCardData) :
            base(shipCardData)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// In general this is a good card to play
        /// </summary>
        /// <param name="aiGameBoardSection"></param>
        /// <param name="otherGameBoardSection"></param>
        /// <returns></returns>
        public override AICardWorthMetric CalculateAIMetric(GameBoardSection aiGameBoardSection, GameBoardSection otherGameBoardSection)
        {
            return AICardWorthMetric.kGoodCardToPlay;
        }

        #endregion
    }
}
