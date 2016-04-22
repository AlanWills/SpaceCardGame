using _2DEngine;
using CardGameEngine;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A class used in our game for doing additional checks involving the resources in the GamePlayer class
    /// </summary>
    public abstract class GameCard : BaseGameCard
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to our parent as a CardObjectPair
        /// </summary>
        private CardObjectPair cardPair;
        private CardObjectPair CardPair
        {
            get
            {
                if (cardPair == null)
                {
                    DebugUtils.AssertNotNull(Parent);
                    Debug.Assert(Parent is CardObjectPair);
                    cardPair = Parent as CardObjectPair;
                }

                return cardPair;
            }
        }

        #endregion

        public GameCard(CardData cardData) :
            base(cardData)
        {
            AddModule(new HoverCardInfoModule(this));
        }

        #region Virtual Functions

        /// <summary>
        /// Calculates whether the conditions of the player are such that we can lay this card.
        /// Passes an error string for output error message.
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
        public virtual bool CanUseOn(CardObjectPair pairToValidate)
        {
            return true;
        }

        /// <summary>
        /// A function which will be called when we place this card into the game.
        /// Sets the IsPlaced parameter to true and charges the resources to the player
        /// </summary>
        public virtual void OnLay(Board board, GamePlayer player)
        {
            IsPlaced = true;

            // Charge the resources from the player
            bool charge = true;
            player.AlterResources(CardData, charge);
        }

        #endregion
    }
}
