using CardGameEngine;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// The data class for a ship card 
    /// </summary>
    public class ShipCardData : GameCardData
    {
        #region Virtual Functions

        /// <summary>
        /// Can only lay this ship card if we have sufficient resources
        /// </summary>
        /// <param name="player"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public override bool CanLay(Player player, ref string error)
        {
            Debug.Assert(player is GamePlayer);
            return (player as GamePlayer).HaveSufficientResources(this, ref error);
        }

        #endregion
    }
}
