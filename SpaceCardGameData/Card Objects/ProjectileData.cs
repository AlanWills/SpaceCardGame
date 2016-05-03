using _2DEngineData;

namespace SpaceCardGameData
{
    public class ProjectileData : GameObjectData
    {
        /// <summary>
        /// The damage this bullet does per shot
        /// </summary>
        public float Damage { get; set; }

        /// <summary>
        /// The asset path to the SFX we will trigger when firing this turret
        /// </summary>
        public string FiringSFXAsset { get; set; }

        /// <summary>
        /// The asset path to the SFX we will trigger when this collides with a damageable target
        /// </summary>
        public string ExplosionSFXAsset { get; set; }
    }
}