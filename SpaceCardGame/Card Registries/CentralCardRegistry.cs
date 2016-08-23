using CelesteEngine;
using Microsoft.Xna.Framework.Content;
using SpaceCardGameData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SpaceCardGame
{
    /// <summary>
    /// This class is not designed to replace the AssetManager for querying data, but rather to provide extra utility functions such as finding cards of specific types
    /// or reverse look up to find data asset strings.
    /// Using AssetManager.GetData to get CardData is still the best thing to do - this is just meant to provide custom functionality for our game.
    /// </summary>
    public static class CentralCardRegistry
    {
        /// <summary>
        /// A lookup of all the card types in our game
        /// </summary>
        public static List<string> CardTypes { get; set; }

        /// <summary>
        /// A lookup of card data type to a dictionary of card data asset to card data of that type
        /// </summary>
        public static Dictionary<string, Dictionary<string, CardData>> CardData { get; set; }

        /// <summary>
        /// A reference to our loaded card registry data.
        /// </summary>
        public static CardRegistryData CardRegistryData { get; private set; }

        /// <summary>
        /// A flag to indicate whether we have loaded the data for this class.
        /// We will have serious problems if we start to do stuff without this being loaded.
        /// </summary>
        public static bool IsLoaded { get; private set; }

        private static string cardRegistryDataPath = Path.Combine("Cards", "CardRegistryData");
        public const string CardFolderPath = "Cards";

        public const int PackSize = 5;

        #region Asset Management Functions

        /// <summary>
        /// Load all the data for the cards.
        /// </summary>
        public static void LoadAssets(ContentManager content)
        {
            CardRegistryData = AssetManager.GetData<CardRegistryData>(cardRegistryDataPath);
            DebugUtils.AssertNotNull(CardRegistryData);

            CardTypes = new List<string>();
            CardData = new Dictionary<string, Dictionary<string, CardData>>();

            // Adds all of the loaded card data to our registry
            List<KeyValuePair<string, CardData>> allCardData = AssetManager.GetAllDataPairsOfType<CardData>();
            allCardData.RemoveAll(x => x.Key == WeaponCard.DefaultWeaponCardDataAsset);

            foreach (KeyValuePair<string, CardData> dataPair in allCardData)
            {
                // Make sure we register new types
                if (!CardTypes.Exists(x => x == dataPair.Value.Type))
                {
                    // If this is a new type, we also need to initialise the list in the Dictionary look up for it
                    CardTypes.Add(dataPair.Value.Type);
                    CardData.Add(dataPair.Value.Type, new Dictionary<string, CardData>());
                }

                // Remove "Cards\\" from the front of the data key - if they are stored here we know they are Cards!
                string key = dataPair.Key.Remove(0, 6);
                CardData[dataPair.Value.Type].Add(key, dataPair.Value);
            }

            // Load our universal card back texture
            Card.CardBackTexture = AssetManager.GetSprite(Card.CardBackTextureAsset);

            IsLoaded = true;
        }
        
        #endregion

        #region Utility Functions

        /// <summary>
        /// Uses the card data's type to search through the appropriate sub dictionary in the CardData dictionary and matches on reference.
        /// This is not as costly as you may fear so do not be afraid about using this rather than some other roundabout function (although obviously don't be stupid!).
        /// </summary>
        /// <param name="cardData">The card data we wish to find the string of</param>
        /// <returns>The semi-string of the card data e.g. 'Resources\\Crew\\CrewResource.xml'.  Empty string if it couldn't be found.</returns>
        public static string FindCardDataAsset(CardData cardData)
        {
            if (cardData == null)
            {
                Debug.Fail("Inputted Card Data cannot be null");
                return "";
            }

            // If we have not loaded we are going to run into trouble here
            Debug.Assert(IsLoaded);
            Debug.Assert(CardData.ContainsKey(cardData.Type));

            List<string> cardDataAssets = CardData[cardData.Type].Keys.ToList();
            string result = cardDataAssets.Find(x => CardData[cardData.Type][x] == cardData);   // This will return null if the string is not found
            
            Debug.Assert(!string.IsNullOrEmpty(result), "Couldn't find string data asset for inputted card data.");
            return result != null ? result : "";    // If our result is null we return "" rather than null
        }

        /// <summary>
        /// Takes an inputted list of card data (could be a deck) and returns a list of their data assets - used in saving.
        /// </summary>
        /// <param name="cardDataList">The list of card data we wish to convert</param>
        /// <returns>The list of card data assets we have obtained.  In the form e.g. 'Resources\Crew\CrewResourceCard.xml'</returns>
        public static List<string> ConvertToAssetList(List<CardData> cardDataList)
        {
            // If we have not loaded we are going to run into trouble here
            Debug.Assert(IsLoaded);

            List<string> cardDataAssetList = new List<string>();
            foreach (CardData cardData in cardDataList)
            {
                cardDataAssetList.Add(FindCardDataAsset(cardData));
            }

            return cardDataAssetList;
        }

        /// <summary>
        /// Takes an inputted list of card data assets and returns a list of the corresponding data assets - used in loading.
        /// </summary>
        /// <param name="cardDataAssetList">The list of card data assets we wish to convert</param>
        /// <returns></returns>
        public static List<CardData> ConvertToDataList(List<string> cardDataAssetList)
        {
            // If we have not loaded we are going to run into trouble here
            Debug.Assert(IsLoaded);

            List<CardData> cardDataList = new List<CardData>();
            foreach (string cardDataAsset in cardDataAssetList)
            {
                // Load from AssetManager here - iteration lookup through the dictionaries is too costly
                CardData cardData = AssetManager.GetData<CardData>(Path.Combine("Cards", cardDataAsset));
                DebugUtils.AssertNotNull(cardData);

                cardDataList.Add(cardData);
            }

            return cardDataList;
        }

        /// <summary>
        /// Picks #PackSize cards from all the loaded cards for when our player opens a pack and returns the CardData for those cards.
        /// </summary>
        /// <returns></returns>
        public static List<CardData> PickCardsForPackOpening()
        {
            List<CardData> cards = new List<CardData>(PackSize);
            List<CardData> allLoadedCardData = AssetManager.GetAllDataOfType<CardData>();

            for (int i = 0; i < PackSize; i++)
            {
                int randomIndex = MathUtils.GenerateInt(0, allLoadedCardData.Count - 1);
                cards.Add(allLoadedCardData[randomIndex]);
            }

            return cards;
        }

        #endregion
    }
}
