using _2DEngine;
using CardGameEngineData;
using Microsoft.Xna.Framework.Content;
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
        /// A flag to indicate whether we have loaded the data for this class.
        /// We will have serious problems if we start to do stuff without this being loaded.
        /// </summary>
        private static bool IsLoaded { get; set; }

        private const string cardRegistryDataPath = "\\Data\\Cards\\CardRegistryData.xml";
        public const string CardFolderPath = "Content\\Data\\Cards\\";

        #region Asset Management Functions

        /// <summary>
        /// Load all the data for the cards.
        /// </summary>
        public static void LoadAssets(ContentManager content)
        {
            CardRegistryData cardRegistryData = AssetManager.LoadData<CardRegistryData>(content.RootDirectory + cardRegistryDataPath);
            DebugUtils.AssertNotNull(cardRegistryData);

            CardTypes = new List<string>();
            CardData = new Dictionary<string, CardData>();

            LoadCardType<CardData>(content, cardRegistryData.AbilityCardDataAssets, "Ability");
            LoadCardType<CardData>(content, cardRegistryData.DefenceCardDataAssets, "Defence");
            LoadCardType<CardData>(content, cardRegistryData.ResourceCardDataAssets, "Resource");
            LoadCardType<CardData>(content, cardRegistryData.ShipCardDataAssets, "Ship");
            LoadCardType<CardData>(content, cardRegistryData.WeaponCardDataAssets, "Weapon");

            IsLoaded = true;
        }

        /// <summary>
        /// Load our resource cards
        /// </summary>
        /// <param name="content"></param>
        private static void LoadCardType<T>(ContentManager content, List<string> assetsToLoad, string typeName) where T : CardData
        {
            // Check we actually have cards to load
            Debug.Assert(assetsToLoad.Count > 0);

            // Load the resource cards
            foreach (string cardDataAsset in assetsToLoad)
            {
                // Add resource data when we implement it
                CardData data = AssetManager.LoadData<T>(CardFolderPath + cardDataAsset);
                DebugUtils.AssertNotNull(data);

                // Check the type of the data matches the type we expect
                Debug.Assert(data.Type == typeName);

                CardData.Add(cardDataAsset, data);
            }

            // Check we have not added this type already and then add it to our registered types
            Debug.Assert(!CardTypes.Exists(x => x == typeName));
            CardTypes.Add(typeName);
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
                if (CardData[dataAsset] != null)
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

        #endregion
    }
}
