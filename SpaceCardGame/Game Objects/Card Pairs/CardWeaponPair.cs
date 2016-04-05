using _2DEngine;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A class which uses the CardObjectPair class but is specifically used with Weapons.
    /// </summary>
    public class CardWeaponPair : CardObjectPair
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the stored card as a WeaponCard (saves casting elsewhere)
        /// </summary>
        public WeaponCard WeaponCard { get; private set; }

        /// <summary>
        /// A reference to the stored game object as a Turret (saves casting elsewhere)
        /// </summary>
        public Turret Turret { get; private set; }

        #endregion

        public CardWeaponPair(WeaponCard weaponCard, Turret turret) :
            base(weaponCard, turret)
        {
            DebugUtils.AssertNotNull(weaponCard);
            DebugUtils.AssertNotNull(turret);

            WeaponCard = weaponCard;
            Turret = turret;
        }

        #region Virtual Functions

        /// <summary>
        /// Adds a weapon to our ship
        /// </summary>
        /// <param name="cardShipPair"></param>
        public override void AddToCardShipPair(CardShipPair cardShipPair)
        {
            Debug.Fail("TODO");
        }

        #endregion
    }
}
