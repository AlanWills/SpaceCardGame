using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A card control specifically for ship cards.
    /// Also implements functions for setting up our station too.
    /// </summary>
    public class ShipCardControl : GameCardControl
    {
        #region Properties and Fields

        /// <summary>
        /// The preset position for our station
        /// </summary>
        public Vector2 StationPosition { get; set; }

        #endregion

        public ShipCardControl(Vector2 size, Vector2 localPosition) :
            base(typeof(ShipCardData), GamePlayer.MaxShipNumber, 1, size, localPosition)
        {
            StationPosition = new Vector2(0, Size.Y * 0.75f);
        }

        /// <summary>
        /// A function called right at the start of the game to add our player's station to this control.
        /// We load and initialise the station too and handle all parenting problems.
        /// We have a separate function, because the station is a special card and we do no want it to go through the same pipeline as the normal CardShipPairs (i.e. AddChild)
        /// </summary>
        /// <param name="stationCard"></param>
        public void AddStation(CardObjectPair stationCard)
        {
            Debug.Assert(stationCard is CardStationPair);

            // Set the parent to be this
            stationCard.Reparent(this);

            Children.AddChild(stationCard, true, true);

            stationCard.LocalPosition = StationPosition;
            stationCard.Card.Size *= 0.5f;
        }
    }
}
