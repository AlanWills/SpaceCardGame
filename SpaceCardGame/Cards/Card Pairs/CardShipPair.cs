using _2DEngine;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;

namespace SpaceCardGame
{
    /// <summary>
    /// A class which uses the CardObjectPair class but is specifically used with Ships.
    /// </summary>
    public class CardShipPair : CardObjectPair
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the stored card as a ShipCard (saves casting elsewhere)
        /// </summary>
        public ShipCard ShipCard { get; private set; }

        /// <summary>
        /// A reference to the stored game object as a Ship (saves casting elsewhere)
        /// </summary>
        public Ship Ship { get; private set; }

        #endregion

        public CardShipPair(ShipCardData shipCardData) :
            base(shipCardData)
        {
            Ship = AddChild(new Ship(shipCardData.ObjectDataAsset));
            CardObject = Ship;

            Debug.Assert(Card is ShipCard);
            ShipCard = Card as ShipCard;

            AddDefaultWeapon();
        }

        #region Virtual Functions

        /// <summary>
        /// When we add a ship to the game board.
        /// Want to update the player's ships placed and set up event callbacks for when this ship dies
        /// </summary>
        /// <param name="gameBoard"></param>
        /// <param name="player"></param>
        public override void WhenAddedToGameBoard(GameBoardSection gameBoard, GamePlayer player)
        {
            Debug.Assert(player.CurrentShipsPlaced < GamePlayer.MaxShipNumber);

            LocalPosition = GameMouse.Instance.InGamePosition;         // Do this before we add it to the control because we use the position to place it in the correct spot
            Reparent(gameBoard.ShipCardControl);        // Reparent this under the card ship control rather than the game board which it was initially added to

            player.CurrentShipsPlaced++;
        }

        /// <summary>
        /// Cannot add ships to other ships
        /// </summary>
        /// <param name="cardShipPair"></param>
        public override void AddToCardShipPair(CardShipPair cardShipPair)
        {
            Debug.Fail("Cannot add ships to other ships");
        }

        /// <summary>
        /// Calls this function iteratively on all CardObjectPairs parented under this ship card.
        /// </summary>
        public override void MakeReadyForCardPlacement()
        {
            base.MakeReadyForCardPlacement();

            foreach (CardObjectPair pair in Children.GetChildrenOfType<CardObjectPair>())
            {
                pair.MakeReadyForCardPlacement();
            }
        }

        /// <summary>
        /// Calls this function iteratively on all CardObjectPairs parented under this ship card.
        /// </summary>
        public override void MakeReadyForBattle()
        {
            base.MakeReadyForBattle();

            foreach (CardObjectPair pair in Children.GetChildrenOfType<CardObjectPair>())
            {
                pair.MakeReadyForBattle();
            }
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// A utility function for setting up the default weapon on this ship
        /// </summary>
        private void AddDefaultWeapon()
        {
            WeaponCardData defaultWeaponCardData = AssetManager.GetData<WeaponCardData>(WeaponCardData.defaultWeaponCardDataAsset);
            CardWeaponPair defaultWeapon = AddChild(new CardWeaponPair(defaultWeaponCardData));

            defaultWeapon.AddToCardShipPair(this);
        }

        #endregion
    }
}
