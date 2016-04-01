using _2DEngineData;

namespace SpaceCardGameData
{
    public class ShipData : GameObjectData
    {
        /// <summary>
        /// The attack value of this ship
        /// </summary>
        public int Attack { get; set; }
        
        /// <summary>
        /// The defence value of this ship
        /// </summary>
        public int Defence { get; set; }

        /// <summary>
        /// The speed value of this ship
        /// </summary>
        public int Speed { get; set; }
    }
}
