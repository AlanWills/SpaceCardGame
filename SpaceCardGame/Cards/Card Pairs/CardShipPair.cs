using _2DEngine;
using Microsoft.Xna.Framework;
using System.Diagnostics;

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

        public CardShipPair(ShipCard shipCard) :
            base(shipCard)
        {
            Ship = AddChild(new Ship(shipCard.CardData.ObjectDataAsset));
            CardObject = Ship;

            Debug.Assert(Card is ShipCard);
            ShipCard = Card as ShipCard;
        }

        #region Virtual Functions

        /// <summary>
        /// Add our ShipHoverCardInfo module to the card ship pair.
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            if (AddHoverInfoModule)
            {
                HoverInfoModule = AddModule(new ShipCardHoverInfoModule(this));
            }

            base.LoadContent();
        }

        /// <summary>
        /// Fixup the size of the ship so that we don't create one larger than our card.
        /// Do this in begin, because we usually do extra size fixup before we start updating so we want to capture those changes.
        /// Also, do it before so that the colliders for our card and card object get the latest size too.
        /// </summary>
        public override void Begin()
        {
            Ship.Scale(MathHelper.Min(Card.Size.Y / CardObject.Size.Y, 1));

            AddDefaultWeapon();

            base.Begin();
        }

        /// <summary>
        /// When we add a ship to the game board.
        /// Want to update the player's ships placed and set up event callbacks for when this ship dies
        /// </summary>
        /// <param name="gameBoard"></param>
        /// <param name="player"></param>
        public override void WhenAddedToGameBoard(GameBoardSection gameBoard)
        {
            base.WhenAddedToGameBoard(gameBoard);

            ReparentTo(gameBoard.ShipCardControl);   // Reparent this under the card ship control rather than the game board which it was initially added to
        }

        /// <summary>
        /// Cannot add ships to other ships
        /// </summary>
        /// <param name="cardShipPair"></param>
        public override void AddToCardShipPair(CardShipPair cardShipPair)
        {
            Debug.Fail("Cannot add ships to other ships");
        }
        
        #endregion

        #region Utility Functions

        /// <summary>
        /// A utility function for setting up the default weapon on this ship
        /// </summary>
        private void AddDefaultWeapon()
        {
            CardData defaultWeaponCardData = AssetManager.GetData<CardData>(WeaponCard.DefaultWeaponCardDataAsset);
            CardWeaponPair defaultWeapon = AddChild(new CardWeaponPair(defaultWeaponCardData.CreateCard(Card.Player) as WeaponCard), true, true);

            defaultWeapon.AddToCardShipPair(this);
        }

        #endregion
    }
}
