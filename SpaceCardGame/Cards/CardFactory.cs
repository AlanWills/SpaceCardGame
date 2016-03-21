using CardGameEngine;
using CardGameEngineData;
using SpaceCardGameData;
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
        public static Card CreateCard(CardData cardData)
        {
            if (cardData is AbilityCardData)
            {
                return new AbilityCard(cardData as AbilityCardData);
            }
            else if (cardData is DefenceCardData)
            {
                return new DefenceCard(cardData as DefenceCardData);
            }
            else if (cardData is ResourceCardData)
            {
                return new ResourceCard(cardData as ResourceCardData);
            }
            else if (cardData is ShipCardData)
            {
                return new ShipCard(cardData as ShipCardData);
            }
            else if (cardData is WeaponCardData)
            {
                return new WeaponCard(cardData as WeaponCardData);
            }
            else
            {
                Debug.Fail("Card factory mismatch");
                return null;
            }
        }
    }
}
