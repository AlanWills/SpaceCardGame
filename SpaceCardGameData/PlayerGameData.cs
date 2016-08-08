using _2DEngineData;

namespace SpaceCardGameData
{
    /// <summary>
    /// Holds information about the player's game session.
    /// </summary>
    public class PlayerGameData : BaseData
    {
        /// <summary>
        /// One based index of the lowest level we have not completed
        /// </summary>
        public int CurrentLevel { get; set; }
    }
}
