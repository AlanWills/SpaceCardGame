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
        /// Override this function to perform custom behaviour when first layed.
        /// Called as soon as we add the object to the scene and right after WhenAddedToGameBoard.
        /// If a target has to be chosen etc. then run a script in here to perform that.
        /// </summary>
        public virtual void OnLay() { }

        /// <summary>
        /// Override this function to perform custom behaviour when our turn begins.
        /// </summary>
        public virtual void OnTurnBegin() { }

        /// <summary>
        /// Override this function to perform custom behaviour when this card dies.
        /// </summary>
        public virtual void OnDie() { }

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
        /// When we call Die on this card, calls Die on the parent too, to trigger killing the card object too
        /// </summary>
        public override void Die()
        {
            OnDie();

            base.Die();

            Debug.Assert(Parent is CardObjectPair);
            Parent.Die();
        }

        #endregion
    }
}
