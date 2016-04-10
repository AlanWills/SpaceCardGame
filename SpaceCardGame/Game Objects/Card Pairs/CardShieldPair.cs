using Microsoft.Xna.Framework;

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

        public CardShieldPair(ShieldCardData defenceCardData) :
            base(defenceCardData)
        {
            ShieldCard = AddChild(new ShieldCard(defenceCardData));
            Shield = AddChild(new Shield(defenceCardData));

            Card = ShieldCard;
            CardObject = Shield;
        }

        #region Virtual Functions

        /// <summary>
        /// Adds a defence object to a ship
        /// </summary>
        /// <param name="cardShipPair"></param>
        public override void AddToCardShipPair(CardShipPair cardShipPair)
        {
            // Reparent under the ship card
            Reparent(cardShipPair);

            // Change the size and position of the card so it appears to the top right of the ship card
            ShieldCard.Size = cardShipPair.Card.Size / 3;
            ShieldCard.EnlargeOnHover = false;
            LocalPosition = new Vector2((cardShipPair.Card.Size.X + ShieldCard.Size.X) * 0.5f, (ShieldCard.Size.Y - cardShipPair.Card.Size.Y) * 0.5f);

            // Set our Shield's position so that it will be centred at the centre of the ship
            Shield.LocalPosition = cardShipPair.WorldPosition - WorldPosition;
        }

        #endregion
    }
}