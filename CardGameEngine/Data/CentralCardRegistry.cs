using _2DEngine;
using CardGameEngineData;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace CardGameEngine
{
    public static class CentralCardRegistry
    {
        /// <summary>
        /// A lookup of card data assets to their loaded data.
        /// </summary>
        public static Dictionary<string, CardData> CardData { get; set; }

        /// <summary>
        /// The list of all the card data assets we should load
        /// </summary>
        public static List<string> CardDataAssets { get; set; }

        private const string cardRegistryDataPath = "\\Data\\Cards\\CardRegistryData.xml";

        /// <summary>
        /// Load all the data for the cards.
        /// </summary>
        public static void LoadAssets(ContentManager content)
        {
            CardRegistryData cardRegistryData = AssetManager.GetData<CardRegistryData>(content.RootDirectory + cardRegistryDataPath);
            DebugUtils.AssertNotNull(cardRegistryData);

            CardData = new Dictionary<string, CardData>();

            foreach (string cardDataAsset in cardRegistryData.CardDataAssets)
            {
                CardData data = AssetManager.GetData<CardData>("Content\\Data\\Cards\\" + cardDataAsset);
                DebugUtils.AssertNotNull(data);

                CardData.Add(cardDataAsset, data);
            }
        }
    }
}
