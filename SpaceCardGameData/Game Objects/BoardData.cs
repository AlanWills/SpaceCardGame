using _2DEngineData;
using System.Collections.Generic;

namespace SpaceCardGameData
{
    public class BoardData : GameObjectData
    {
        /// <summary>
        /// A list of the texture assets for asteroids we will be adding to the game board (for a bit of extra spice!)
        /// </summary>
        public List<string> AsteroidTextureAssets { get; set; }
    }
}
