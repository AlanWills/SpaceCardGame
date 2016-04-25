using _2DEngine;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;

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

            // Only add a hover info module if this is not the default turret we add for all ships
            AddHoverInfoModule = !Turret.DefaultTurret;
        }

        #region Virtual Functions

        /// <summary>
        /// Add a script to choose a ship to add this weapon to
        /// </summary>
        /// <param name="gameBoard"></param>
        /// <param name="player"></param>
        public override void WhenAddedToGameBoard(GameBoardSection gameBoard, GamePlayer player)
        {
            
        }

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

            // If our turret is more than 3 times the size of our ship, we want to scale it down
            Vector2 potentialScaling = Vector2.Divide(cardShipPair.Card.Size, Turret.Size * 3);
            if (potentialScaling.X < 1 || potentialScaling.Y < 1)
            {
                // We need to scale the turret down
                Turret.Size *= (Math.Min(potentialScaling.X, potentialScaling.Y) / 3.0f);
            }

            // Set up the reference to this turret on the inputted ship
            cardShipPair.Ship.Turret = Turret;

            // Set up the reference to the CardShipPair
            CardShipPair = cardShipPair;
        }

        /// <summary>
        /// Reloads our turret when we start our card placement turn again and changes our position to be to the side of the ship card
        /// </summary>
        public override void MakeReadyForCardPlacement()
        {
            base.MakeReadyForCardPlacement();

            Colour.Value = Color.White;

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
            Turret.LocalRotation = 0;
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
