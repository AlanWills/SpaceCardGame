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
            if (cardData is ResourceCardData)
            {
                return new ResourceCard(cardData as ResourceCardData);
            }
            else
            {
                Debug.Fail("Card factory mismatch");
                return null;
            }
        }
    }
}
