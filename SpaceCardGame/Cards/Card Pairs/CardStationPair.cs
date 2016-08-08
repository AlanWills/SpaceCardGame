namespace SpaceCardGame
{
    /// <summary>
    /// Delegate specification for a function called when the station is destroyed.
    /// Will allow us to perform custom end-game behaviour when either the player or the opponent's station is destroyed.
    /// </summary>
    public delegate void OnStationDestroyedHandler();

    /// <summary>
    /// A specific implentation of a ship for our singular station object that both players will have.
    /// This is the win condition - if you destroy your opponents' you win.
    /// </summary>
    public class CardStationPair : CardShipPair
    {
        #region Properties and Fields

        /// <summary>
        /// This will fire when the station is destroyed.
        /// Can subscribe to this event to add a victory/defeat screen or other end game behaviour when a station is destroyed.
        /// </summary>
        public event OnStationDestroyedHandler OnStationDestroyed;

        #endregion

        public CardStationPair(ShipCard shipCard) :
            base(shipCard)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// When we add the station to the game board, we have a specific place for it.
        /// We wish to place it there and add it to the ShipCardControl so that it can attack/be attacked etc.
        /// </summary>
        /// <param name="gameBoard"></param>
        /// <param name="player"></param>
        public override void WhenAddedToGameBoard(GameBoardSection gameBoard)
        {
            // Do not call through to base implementation, because our station is added differently to our normal ships
            Card.OnLay();

            // This function will handle parenting
            gameBoard.ShipCardControl.AddStation(this);         // Call the specific function we have for stations - this is for positioning purposes mainly
        }

        /// <summary>
        /// Make sure we hide our engine for the station - we do not want them to appear for now
        /// </summary>
        public override void MakeReadyForBattle()
        {
            base.MakeReadyForBattle();

            // Hide our default turret
            if (Ship.Turret.IsDefaultTurret)
            {
                Ship.Turret.Hide();
            }

            // Always hide our engines
            for (int i = 0; i < Ship.Engines.Length; ++i)
            {
                Ship.Engines[i].Hide();
            }
        }

        /// <summary>
        /// Invokes the OnStationDestroyed event.
        /// </summary>
        public override void Die()
        {
            base.Die();

            OnStationDestroyed?.Invoke();
        }

        #endregion
    }
}
