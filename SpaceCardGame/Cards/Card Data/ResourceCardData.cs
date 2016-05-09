﻿using CardGameEngine;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// The data class for a resource card
    /// </summary>
    public class ResourceCardData : GameCardData
    {
        #region Virtual Functions

        /// <summary>
        /// Only lay this resource card if we have laid less than our quote of resource cards this turn
        /// </summary>
        /// <param name="player"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public override bool CanLay(Player player, ref string error)
        {
            Debug.Assert(player is GamePlayer);

            // Check to make sure we haven't laid 2 resource cards already
            Debug.Assert(Type == "Resource");
            if ((player as GamePlayer).ResourceCardsPlacedThisTurn >= GamePlayer.ResourceCardsCanLay)
            {
                error = "Already laid two resource cards this turn";
                return false;
            }

            return true;
        }

        #endregion
    }
}
