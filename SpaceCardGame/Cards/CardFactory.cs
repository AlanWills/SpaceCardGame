using CardGameEngine;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A class which creates the cards from CardData
    /// </summary>
    public static class CardFactory
    {
        /// <summary>
        /// Create a card from the inputted card data
        /// </summary>
        /// <param name="cardData"></param>
        /// <returns></returns>
        public static GameCard CreateCard(CardData cardData)
        {
            switch (cardData.Type)
            {
                case "Ability":
                    return CreateAbilityCard(cardData);

                case "Resource":
                    return CreateResourceCard(cardData);

                case "Shield":
                    return CreateShieldCard(cardData);

                case "Ship":
                    return CreateShipCard(cardData);

                case "Weapon":
                    return CreateWeaponCard(cardData);

                default:
                    Debug.Fail("Unregistered CardData in card factory create");
                    return null;
            }
        }

        #region Create Functions for Individual Card Types

        /// <summary>
        /// A function for specifically creating ability cards
        /// </summary>
        /// <param name="cardData"></param>
        /// <returns></returns>
        private static GameCard CreateAbilityCard(CardData cardData)
        {
            return new AbilityCard(cardData as AbilityCardData);
        }

        /// <summary>
        /// A function for specifically creating resource cards
        /// </summary>
        /// <param name="cardData"></param>
        /// <returns></returns>
        private static GameCard CreateResourceCard(CardData cardData)
        {
            return new ResourceCard(cardData as ResourceCardData);
        }

        /// <summary>
        /// A function for specifically creating shield cards
        /// </summary>
        /// <param name="cardData"></param>
        /// <returns></returns>
        private static GameCard CreateShieldCard(CardData cardData)
        {
            return new ShieldCard(cardData as ShieldCardData);
        }

        /// <summary>
        /// A function for specifically creating ship cards
        /// </summary>
        /// <param name="cardData"></param>
        /// <returns></returns>
        private static GameCard CreateShipCard(CardData cardData)
        {
            return new ShipCard(cardData as ShipCardData);
        }

        /// <summary>
        /// A function for specifically creating weapon cards
        /// </summary>
        /// <param name="cardData"></param>
        /// <returns></returns>
        private static GameCard CreateWeaponCard(CardData cardData)
        {
            return new WeaponCard(cardData as WeaponCardData);
        }

        #endregion
    }
}
