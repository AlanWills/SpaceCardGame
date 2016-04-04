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

        #region Virtual Functions

        /// <summary>
        /// Calculates whether the conditions of the player are such that we can lay this card
        /// </summary>
        /// <param name="cardData"></param>
        /// <returns><c>true</c>We can lay this card.<c>false</c>We cannot lay this card</returns>
        public virtual bool CanLay(GamePlayer player, ref string error)
        {
            return player.HaveSufficientResources(CardData, ref error);
        }

        /// <summary>
        /// Some cards require us to choose a target before laying them, or for an ability.
        /// This is a virtual function which can be used to work out whether the inputted proposed target is a valid one for this card.
        /// </summary>
        /// <param name="pair"></param>
        /// <returns></returns>
        public virtual bool IsValidTargetForInput(CardObjectPair pairToValidate)
        {
            return true;
        }

        #endregion
    }
}
