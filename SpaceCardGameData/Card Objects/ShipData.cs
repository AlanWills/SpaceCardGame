using _2DEngineData;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Xml.Serialization;

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

        /// <summary>
        /// A list of the damage hardpoints for this ship.
        /// It's size will be Defence - 1
        /// </summary>
        [XmlArrayAttribute(ElementName = "Hardpoint")]
        public List<Vector2> DamageHardpoints { get; set; }
    }
}
