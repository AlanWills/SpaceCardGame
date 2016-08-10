using System.Collections.Generic;

namespace SpaceCardGameData
{
    /// <summary>
    /// A class which holds the names of all the cards the player has unlocked, as well as information about their current decks, available money and unlocked levels.
    /// </summary>
    public class PlayerDataRegistryData : CardRegistryData
    {
        /// <summary>
        /// A list of cards representing a user deck
        /// </summary>
        public List<DeckData> Decks { get; set; }

        /// <summary>
        /// One based index of the lowest level we have not completed
        /// </summary>
        public int CurrentLevel { get; set; }

        /// <summary>
        /// The in-game currency the player has to spend
        /// </summary>
        public int CurrentMoney { get; set; }

        /// <summary>
        /// The number of packs this player has left to open
        /// </summary>
        public int AvailablePacksToOpen { get; set; }
    }
}
