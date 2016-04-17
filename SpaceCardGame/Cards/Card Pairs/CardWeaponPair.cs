using _2DEngine;
using Microsoft.Xna.Framework;
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
        /// A reference to our parent CardShipPair - for convenience
        /// </summary>
        private CardShipPair CardShipPair { get; set; }

        /// <summary>
        /// A reference to the stored game object as a Turret (saves casting elsewhere)
        /// </summary>
        public Turret Turret { get; private set; }

        #endregion

        public CardWeaponPair(WeaponCardData weaponCardData) :
            base(weaponCardData)
        {
            Turret = AddChild(new Turret(weaponCardData.ObjectDataAsset));
            CardObject = Turret;

            Debug.Assert(Card is WeaponCard);
            WeaponCard = Card as WeaponCard;
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
            LocalPosition = new Vector2((cardShipPair.Card.Size.X + WeaponCard.Size.X) * 0.5f, (3 * WeaponCard.Size.Y - cardShipPair.Card.Size.Y) * 0.5f);

            // Set up the reference to this shield on the inputted ship
            cardShipPair.Ship.Turret = Turret;

            // Set up the reference to the CardShipPair
            CardShipPair = cardShipPair;

            // Connect the turret's colour to the ship - this is for highlighting purposes
            Turret.Colour.Connect(CardShipPair.Ship.Colour);
        }

        /// <summary>
        /// Reloads our turret when we start our card placement turn again and changes our position to be to the side of the ship card
        /// </summary>
        public override void MakeReadyForCardPlacement()
        {
            base.MakeReadyForCardPlacement();

            DebugUtils.AssertNotNull(Turret);
            Turret.Reload();

            LocalPosition = new Vector2((CardShipPair.Card.Size.X + WeaponCard.Size.X) * 0.5f, (3 * WeaponCard.Size.Y - CardShipPair.Card.Size.Y) * 0.5f);
        }

        /// <summary>
        /// Move this pair so that it is directly over the ship - this is because the turret is going to rotate and this card pair must be in the same place as the turret
        /// </summary>
        public override void MakeReadyForBattle()
        {
            base.MakeReadyForBattle();

            LocalPosition = Vector2.Zero;
        }

        /// <summary>
        /// Reloads our turret ready for our opponent's battle phase.
        /// </summary>
        public override void OnTurnEnd()
        {
            base.OnTurnEnd();

            DebugUtils.AssertNotNull(Turret);
            Turret.Reload();
        }

        #endregion
    }
}
