using _2DEngineData;
using System.Collections.Generic;

namespace SpaceCardGameData
{
    public class CardRegistryData : BaseData
    {
        /// <summary>
        /// The central list of all the available cards in the game.
        /// Do not need to add 'Cards\' on the front, e.g. an item in the XML may be <Item>Abilities\MissileBarrageAbility\MissileBarrageAbility.xml</Item>
        /// </summary>
        public List<string> CardDataAssets { get; set; }
    }
}
