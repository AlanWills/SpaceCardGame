using CardGameEngine;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// The data class for a shield card
    /// </summary>
    public class ShieldCardData : CardData
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
            GamePlayer gamePlayer = player as GamePlayer;
            bool hasEnoughResources = gamePlayer.HasSufficientResources(this, ref error);

            return hasEnoughResources && gamePlayer.CurrentShipsPlaced > 0;
        }

        #endregion
    }
}
