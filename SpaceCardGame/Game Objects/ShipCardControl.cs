using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A card control specifically for ship cards.
    /// Also implements functions for setting up our station too.
    /// </summary>
    public class ShipCardControl : CardControl
    {
        #region Properties and Fields

        /// <summary>
        /// The preset position for our station
        /// </summary>
        public Vector2 StationPosition { get; set; }

        #endregion

        public ShipCardControl(Vector2 size, Vector2 localPosition) :
            base(typeof(ShipCard), Player.MaxShipNumber, 1, size, localPosition)
        {
            StationPosition = new Vector2(0, Size.Y * 0.75f);
        }

        /// <summary>
        /// A function called right at the start of the game to add our player's station to this control.
        /// We load and initialise the station too and handle all parenting problems.
        /// We have a separate function, because the station is a special card and we do no want it to go through the same pipeline as the normal CardShipPairs (i.e. AddChild)
        /// </summary>
        /// <param name="stationCardPair"></param>
        public void AddStation(CardObjectPair stationCardPair)
        {
            Debug.Assert(stationCardPair is CardStationPair);

            // Do a shallow reparent - we do not want to go through AddChild because it will do unnecessary things to this station
            // Instead we have to manually do a bit of hacking
            bool dontWantToBeAddedViaAddChild = false;
            stationCardPair.ReparentTo(this, dontWantToBeAddedViaAddChild);       // This does set up our new parent however
            Children.AddChild(stationCardPair, true, true);

            stationCardPair.LocalPosition = StationPosition;
        }
    }
}
