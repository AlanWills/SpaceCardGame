using CardGameEngine;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// Yeah this is gonna suck
    /// </summary>
    public static class CardFactory
    {
        /// <summary>
        /// Creates a card based on the inputted card data - really not very nice.
        /// Card position will be set up when we add it to our game board, so don't need to worry about doing that.
        /// </summary>
        /// <param name="cardData"></param>
        /// <returns></returns>
        public static GameCard CreateCard(CardData cardData, bool load = true, bool initialise = true)
        {
            GameCard card = null;

            if (cardData is AbilityCardData)
            {
                card = new AbilityCard(cardData as AbilityCardData);
            }
            else if (cardData is ShieldCardData)
            {
                card = new ShieldCard(cardData as ShieldCardData);
            }
            else if (cardData is ResourceCardData)
            {
                card = new ResourceCard(cardData as ResourceCardData);
            }
            else if (cardData is ShipCardData)
            {
                card = new ShipCard(cardData as ShipCardData);
            }
            else if (cardData is WeaponCardData)
            {
                card = new WeaponCard(cardData as WeaponCardData);
            }
            else
            {
                Debug.Fail("Card factory mismatch");
            }

            // Load if indicated
            if (load)
            {
                card.LoadContent();
            }

            // Initialise if indicated
            if (initialise)
            {
                card.Initialise();
            }

            return card;
        }
    }
}
