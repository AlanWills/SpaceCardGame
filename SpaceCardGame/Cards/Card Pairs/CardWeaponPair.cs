using Microsoft.Xna.Framework;
using System;
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

        public CardWeaponPair(WeaponCard weaponCard) :
            base(weaponCard)
        {
            Debug.Assert(Card is WeaponCard);
            WeaponCard = Card as WeaponCard;

            Turret = AddChild(WeaponCard.CreateTurret(weaponCard.CardData.ObjectDataAsset));
            CardObject = Turret;

            // Only add a hover info module if this is not the default turret we add for all ships
            AddHoverInfoModule = !Turret.IsDefaultTurret;
        }

        #region Virtual Functions

        /// <summary>
        /// Adds a weapon to our ship
        /// </summary>
        /// <param name="cardShipPair"></param>
        public override void AddToCardShipPair(CardShipPair cardShipPair)
        {
            // Reparent under the ship card
            ReparentTo(cardShipPair);

            // Set up the reference to the CardShipPair
            CardShipPair = cardShipPair;

            // Change the size and position of the card so it appears to the top right of the ship card
            WeaponCard.Size = cardShipPair.Card.Size / 3;
            LocalPosition = new Vector2((cardShipPair.Card.Size.X + WeaponCard.Size.X) * 0.5f, (3 * WeaponCard.Size.Y - cardShipPair.Card.Size.Y) * 0.5f);

            // Finally, scale the turret down by the same scaling as the ship
            float yScale = CardShipPair.CardObject.Size.Y / (2 * CardShipPair.CardObject.TextureCentre.Y);
            Turret.Scale(yScale);

            // Do scaling if we are not the default turret
            if (!Turret.IsDefaultTurret)
            {
                float scalingAmount = 2;

                // If our turret is more than 3 times the size of our ship, we want to scale it down
                Vector2 potentialScaling = Vector2.Divide(cardShipPair.Card.Size, Turret.Size * scalingAmount);
                if (potentialScaling.X < 1 || potentialScaling.Y < 1)
                {
                    // We need to scale the turret down
                    Turret.Size *= (Math.Min(potentialScaling.X, potentialScaling.Y) / scalingAmount);
                }
            }

            // Set up the reference to this turret on the inputted ship
            cardShipPair.Ship.Turret = Turret;
        }

        /// <summary>
        /// Reloads our turret when we start our card placement turn again and changes our position to be to the side of the ship card
        /// </summary>
        public override void OnTurnBegin()
        {
            base.OnTurnBegin();

            Colour = Color.White;

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
        
        #endregion
    }
}
