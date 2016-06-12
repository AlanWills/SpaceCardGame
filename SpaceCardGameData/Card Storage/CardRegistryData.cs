using _2DEngineData;
using System.Collections.Generic;

namespace SpaceCardGameData
{
    public class CardRegistryData : BaseData
    {
        /// <summary>
        /// The central list of all the available ability cards in the game.
        /// Do not need to add 'Cards\' on the front, e.g. an item in the XML may be <Item>Abilities\MissileBarrageAbility\MissileBarrageAbility.xml</Item>
        /// </summary>
        public List<string> AbilityCardDataAssets { get; set; }

        /// <summary>
        /// The central list of all the available defence cards in the game.
        /// Do not need to add 'Cards\' on the front, e.g. an item in the XML may be <Item>Shields\PhaseEnergyShield\PhaseEnergyShield.xml</Item>
        /// </summary>
        public List<string> ShieldCardDataAssets { get; set; }

        /// <summary>
        /// The central list of all the available resource cards in the game.
        /// Do not need to add 'Cards\' on the front, e.g. an item in the XML may be <Item>Resources\Crew\CrewResourceCard.xml</Item>
        /// </summary>
        public List<string> ResourceCardDataAssets { get; set; }

        /// <summary>
        /// The central list of all the available ship cards in the game.
        /// Do not need to add 'Cards\' on the front, e.g. an item in the XML may be <Item>Ships\BlazeInterceptor\BlazeInterceptor.xml</Item>
        /// </summary>
        public List<string> ShipCardDataAssets { get; set; }

        /// <summary>
        /// The central list of all the available weapon cards in the game.
        /// Do not need to add 'Cards\' on the front, e.g. an item in the XML may be <Item>Weapons\Beam\LaserBeamTurret\LaserBeamTurret.xml</Item>
        /// </summary>
        public List<string> WeaponCardDataAssets { get; set; }
    }
}
