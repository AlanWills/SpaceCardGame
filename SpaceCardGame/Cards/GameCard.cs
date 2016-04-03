using CardGameEngine;
using CardGameEngineData;

namespace SpaceCardGame
{
    /// <summary>
    /// A class used in our game for doing additional checks involving the resources in the GamePlayer class
    /// </summary>
    public class GameCard : BaseGameCard
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
        public virtual bool CanLay(GamePlayer player, ref string error)
        {
            return player.HaveSufficientResources(CardData, ref error);
        }
    }
}
