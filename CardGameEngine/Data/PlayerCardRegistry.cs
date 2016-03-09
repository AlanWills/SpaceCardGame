using System.Collections.Generic;

namespace CardGameEngine
{
    /// <summary>
    /// A class for the player's cards and decks.
    /// Singleton.
    /// </summary>
    public class PlayerCardRegistry
    {
        #region Properties and Fields

        /// <summary>
        /// A singleton for the PlayerCardRegistry
        /// </summary>
        private static PlayerCardRegistry instance = new PlayerCardRegistry();
        public static PlayerCardRegistry Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// A list of all the available cards that the player has unlocked
        /// </summary>
        public List<string> AvailableCards { get; private set; }

        /// <summary>
        /// A data structure for all of the player's decks the player has created
        /// </summary>
        public Deck[] Decks { get; private set; }

        public const int maxDeckNumber = 1;

        #endregion

        public PlayerCardRegistry()
        {
            AvailableCards = new List<string>();
            Decks = new Deck[maxDeckNumber];
        }
    }
}
