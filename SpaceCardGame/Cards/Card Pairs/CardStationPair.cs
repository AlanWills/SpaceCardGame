using _2DEngine;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A specific implentation of a ship for our singular station object that both players will have.
    /// This is the win condition - if you destroy your opponents' you win.
    /// </summary>
    public class CardStationPair : CardShipPair
    {
        public CardStationPair(ShipCardData shipCardData) :
            base(shipCardData)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// When we add the station to the game board, we have a specific place for it.
        /// We wish to place it there and add it to the ShipCardControl so that it can attack/be attacked etc.
        /// </summary>
        /// <param name="gameBoard"></param>
        /// <param name="player"></param>
        public override void WhenAddedToGameBoard(GameBoardSection gameBoard, GamePlayer player)
        {
            // Need to extract from game board section so that we can be added to the ShipCardControl
            gameBoard.ExtractChild(this);
            gameBoard.ShipCardControl.AddStation(this);         // Call the specific function we have for stations - this is for positioning purposes mainly
        }

        #endregion
    }
}
