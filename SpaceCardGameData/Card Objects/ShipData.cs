using _2DEngineData;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

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
        /// A list of the turret hardpoints for this ship.
        /// It's size will be Attack
        /// </summary>
        public List<Vector2> TurretHardpoints { get; set; }

        /// <summary>
        /// A list of the engine hardpoints for this ship.
        /// It's size will be arbritrary
        /// </summary>
        public List<Vector2> EngineHardpoints { get; set; }

        /// <summary>
        /// A list of the damage hardpoints for this ship.
        /// It's size will be Defence - 1
        /// </summary>
        public List<Vector2> DamageHardpoints { get; set; }
    }
}
