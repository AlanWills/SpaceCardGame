using _2DEngineData;

namespace SpaceCardGameData
{
    public class TurretData : GameObjectData
    {
        /// <summary>
        /// The data asset for our bullets
        /// </summary>
        public string BulletDataAsset { get; set; }

        /// <summary>
        /// The number of shots our turret can fire each turn
        /// </summary>
        public int Shots { get; set; }
    }
}
