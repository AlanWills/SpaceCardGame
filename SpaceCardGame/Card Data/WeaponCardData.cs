using CardGameEngine;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// The data class for a weapon card
    /// </summary>
    public class WeaponCardData : CardData
    {
        #region Virtual Functions

        /// <summary>
        /// Only can lay this weapon if we have sufficient resources
        /// </summary>
        /// <param name="player"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public override bool CanLay(Player player, ref string error)
        {
            Debug.Assert(player is GamePlayer);
            return (player as GamePlayer).HasSufficientResources(this, ref error);
        }

        #endregion
    }
}
