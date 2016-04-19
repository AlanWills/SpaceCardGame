using _2DEngine;
using CardGameEngineData;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CardGameEngine
{
    public static class CentralCardRegistry
    {
        /// <summary>
        /// A lookup of all the card types in our game
        /// </summary>
        public static List<string> CardTypes { get; set; }

        /// <summary>
        /// A lookup of card data assets to their loaded data.
        /// </summary>
        public static Dictionary<string, CardData> CardData { get; set; }

        /// <summary>
        /// A reference to our loaded card registry data.
        /// </summary>
        public static CardRegistryData CardRegistryData { get; private set; }

        /// <summary>
        /// A flag to indicate whether we have loaded the data for this class.
        /// We will have serious problems if we start to do stuff without this being loaded.
        /// </summary>
        public static bool IsLoaded { get; private set; }

        private const string cardRegistryDataPath = "Cards\\CardRegistryData.xml";
        public const string CardFolderPath = "Cards\\";

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
            CardData = new Dictionary<string, CardData>();

            // Adds all of the loaded card data to our registry
            List<KeyValuePair<string, CardData>> allCardData = AssetManager.GetAllDataPairsOfType<CardData>();
            foreach (KeyValuePair<string, CardData> dataPair in allCardData)
            {
                // Remove "Cards\\" from the front of the data key - if they are stored here we know they are Cards!
                string key = dataPair.Key.Remove(0, 6);
                CardData.Add(key, dataPair.Value);

                if (!CardTypes.Exists(x => x == dataPair.Value.Type))
                {
                    CardTypes.Add(dataPair.Value.Type);
                }
            }

            // Load our universal card back texture
            BaseUICard.CardBackTexture = AssetManager.GetSprite(BaseUICard.CardBackTextureAsset);

            IsLoaded = true;
        }
        
        #endregion

        #region Utility Functions

        /// <summary>
        /// REALLY BAD AND HORRIBLE.
        /// Looks through each key and sees if it corresponds to the inputted cardData
        /// </summary>
        /// <param name="cardData">The card data we wish to find the string of</param>
        /// <returns>The semi-string of the card data e.g. 'Resources\\Crew\\CrewResource.xml'.  Empty string if it couldn't be found.</returns>
        public static string FindCardDataAsset(CardData cardData)
        {
            // If we have not loaded we are going to run into trouble here
            Debug.Assert(IsLoaded);

            foreach (string dataAsset in CardData.Keys)
            {
                // Match on display names - GULP!
                if (CardData[dataAsset].DisplayName == cardData.DisplayName)
                {
                    return dataAsset;
                }
            }

            Debug.Fail("Couldn't find string data asset for inputted card data.");
            return "";
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
                Debug.Assert(CardData.ContainsKey(cardDataAsset));
                DebugUtils.AssertNotNull(CardData[cardDataAsset]);

                cardDataList.Add(CardData[cardDataAsset]);
            }

            return cardDataList;
        }

        /// <summary>
        /// Picks cards from all the registered cards for when our player opens a pack.
        /// </summary>
        /// <returns></returns>
        public static List<CardData> PickCardsForPackOpening()
        {
            List<CardData> cards = new List<CardData>(PackSize);
            CardData[] registeredCards = new CardData[CardData.Count];
            CardData.Values.CopyTo(registeredCards, 0);

            for (int i = 0; i < PackSize; i++)
            {
                int randomIndex = MathUtils.GenerateInt(0, registeredCards.Length - 1);
                cards.Add(registeredCards[randomIndex]);
            }

            return cards;
        }

        #endregion
    }
}
