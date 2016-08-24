using SpaceCardGameData;

namespace SpaceCardGame
{
    public class Y_AkaiBomberShipCard : ShipCard
    {
        public Y_AkaiBomberShipCard(Player player, CardData shipCardData) :
            base(player, shipCardData)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// If the opponent has few ships deployed this is a good card to play.
        /// If they have an average number, this is an average card to play.
        /// If they have a lot deployed, it is a bad card to play.
        /// </summary>
        /// <param name="aiGameBoardSection"></param>
        /// <param name="otherGameBoardSection"></param>
        /// <returns></returns>
        public override AICardWorthMetric CalculateAIMetric(GameBoardSection aiGameBoardSection, GameBoardSection otherGameBoardSection)
        {
            int numberOfShipsDeployed = otherGameBoardSection.ShipCardControl.ChildrenCount;
            if (numberOfShipsDeployed <= 2)
            {
                return AICardWorthMetric.kGoodCardToPlay;
            }
            else if (numberOfShipsDeployed <= 3)
            {
                return AICardWorthMetric.kAverageCardToPlay;
            }
            else
            {
                return AICardWorthMetric.kBadCardToPlay;
            }
        }

        #endregion
    }
}