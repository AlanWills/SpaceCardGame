using _2DEngine;
using Microsoft.Xna.Framework;

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

        public CardWeaponPair(WeaponCardData weaponCardData) :
            base(weaponCardData)
        {
            WeaponCard = AddChild(new WeaponCard(weaponCardData));
            Turret = AddChild(new Turret(weaponCardData.ObjectDataAsset));

            Card = WeaponCard;
            CardObject = Turret;
        }

        #region Virtual Functions

        /// <summary>
        /// Adds a weapon to our ship
        /// </summary>
        /// <param name="cardShipPair"></param>
        public override void AddToCardShipPair(CardShipPair cardShipPair)
        {
            // Reparent under the ship card
            Reparent(cardShipPair);

            // Change the size and position of the card so it appears to the top right of the ship card
            WeaponCard.Size = cardShipPair.Card.Size / 3;
            WeaponCard.EnlargeOnHover = false;
            LocalPosition = new Vector2((cardShipPair.Card.Size.X + WeaponCard.Size.X) * 0.5f, (3 * WeaponCard.Size.Y - cardShipPair.Card.Size.Y) * 0.5f);

            // Set up the reference to this shield on the inputted ship
            cardShipPair.Ship.Turret = Turret;

            // Set our Shield's position so that it will be centred at the centre of the ship
            Turret.LocalPosition = cardShipPair.WorldPosition - WorldPosition;
        }

        /// <summary>
        /// Reloads our turret when we start our card placement turn again
        /// </summary>
        public override void MakeReadyForCardPlacement()
        {
            base.MakeReadyForCardPlacement();

            DebugUtils.AssertNotNull(Turret);
            Turret.Reload();
        }

        #endregion
    }
}
