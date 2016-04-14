﻿using _2DEngine;
using CardGameEngine;
using Microsoft.Xna.Framework;

namespace SpaceCardGame
{
    /// <summary>
    /// A class used in our game for doing additional checks involving the resources in the GamePlayer class
    /// </summary>
    public abstract class GameCard : BaseGameCard
    {
        #region Properties and Fields

        /// <summary>
        /// A highlight module we will use to highlight the card
        /// </summary>
        protected HighlightOnHoverModule HighlightModule { get; private set; }

        #endregion

        public GameCard(CardData cardData) :
            base(cardData)
        {
            HighlightModule = AddModule(new HighlightOnHoverModule(Color.White, Color.LightGreen, BlendMode.kBinary));
        }

        #region Virtual Functions

        /// <summary>
        /// Calculates whether the conditions of the player are such that we can lay this card
        /// </summary>
        /// <param name="cardData"></param>
        /// <returns><c>true</c>We can lay this card.<c>false</c>We cannot lay this card</returns>
        public virtual bool CanLay(GamePlayer player, ref string error)
        {
            return player.HasSufficientResources(CardData, ref error);
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

        #endregion
    }
}
