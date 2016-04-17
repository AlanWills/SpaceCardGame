using CardGameEngine;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// The data class for a weapon card
    /// </summary>
    public class WeaponCardData : CardData
    {
        #region Properties and Fields

        // The path to a default weapon we will use to create a weapon for each ship initially
        public const string defaultWeaponCardDataAsset = "Cards\\Weapons\\DefaultTurretCard.xml";

        #endregion

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
            GamePlayer gamePlayer = player as GamePlayer;
            bool hasEnoughResources = gamePlayer.HasSufficientResources(this, ref error);

            return hasEnoughResources && gamePlayer.CurrentShipsPlaced > 0;
        }

        #endregion
    }
}
