using CardGameEngine;
using CardGameEngineData;

namespace SpaceCardGame
{
    /// <summary>
    /// A class used in our game for doing additional checks involving the resources in the GamePlayer class
    /// </summary>
    public class GameCard : Card
    {
        public GameCard(CardData cardData) :
            base(cardData)
        {

        }

        /// <summary>
        /// Calculates whether the conditions of the game are such that we can lay this card
        /// </summary>
        /// <param name="cardData"></param>
        /// <returns><c>true</c>We can lay this card.<c>false</c>We cannot lay this card</returns>
        public virtual bool CanLay(GamePlayer player)
        {
            for (int i = 0; i < (int)ResourceType.kNumResourceTypes; i++)
            {
                if (player.AvailableResources[i] < CardData.ResourceCosts[i])
                {
                    // We do not have enough of the current resource we are analysing to lay this card so return false
                    return false;
                }
            }

            // We have enough of each resource type for this card
            return true;
        }
    }
}
