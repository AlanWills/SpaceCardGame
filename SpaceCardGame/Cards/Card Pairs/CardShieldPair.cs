﻿using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A class which uses the CardObjectPair class but is specifically used with Defence objects.
    /// </summary>
    public class CardShieldPair : CardObjectPair
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the stored card as a DefenceCard (saves casting elsewhere)
        /// </summary>
        public ShieldCard ShieldCard { get; private set; }

        /// <summary>
        /// A reference to the stored game object as a Shield (saves casting elsewhere)
        /// </summary>
        public Shield Shield { get; private set; }

        #endregion

        public CardShieldPair(ShieldCard defenceCard) :
            base(defenceCard)
        {
            Shield = AddChild(new Shield(defenceCard.CardData.ObjectDataAsset));
            CardObject = Shield;

            Debug.Assert(Card is ShieldCard);
            ShieldCard = Card as ShieldCard;
        }

        #region Virtual Functions

        /// <summary>
        /// Adds a shield to a ship
        /// </summary>
        /// <param name="cardShipPair"></param>
        public override void AddToCardShipPair(CardShipPair cardShipPair)
        {
            // Reparent under the ship card
            ReparentTo(cardShipPair);

            // Change the size and position of the card so it appears to the top right of the ship card
            ShieldCard.Size = cardShipPair.Card.Size / 3;
            LocalPosition = new Vector2((cardShipPair.Card.Size.X + ShieldCard.Size.X) * 0.5f, (ShieldCard.Size.Y - cardShipPair.Card.Size.Y) * 0.5f);

            // Set up the reference to this shield on the inputted ship
            cardShipPair.Ship.Shield = Shield;

            // Set our Shield's position so that it will be centred at the centre of the ship
            Shield.LocalPosition = cardShipPair.WorldPosition - WorldPosition;

            // Change our Shield size to wrap around the ship
            float maxDimension = MathHelper.Max(cardShipPair.Ship.Size.X, cardShipPair.Ship.Size.Y);
            float maxPadding = MathHelper.Max(30, 0.85f * maxDimension);        // Either add an absolute or relative amount depending on which is bigger (first for small ships, second for large ships)
            Shield.Size = new Vector2(maxPadding + maxDimension);               // Add a little extra padding for safety
        }

        #endregion
    }
}