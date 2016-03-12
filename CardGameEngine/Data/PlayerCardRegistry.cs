using _2DEngine;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using CardGameEngineData;
using System.Diagnostics;

namespace CardGameEngine
{
    /// <summary>
    /// A class for the player's cards and decks.
    /// Singleton.
    /// </summary>
    public class PlayerCardRegistry : IAsset
    {
        #region Properties and Fields

        /// <summary>
        /// A singleton for the PlayerCardRegistry
        /// </summary>
        private static PlayerCardRegistry instance = new PlayerCardRegistry();
        public static PlayerCardRegistry Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// A list of all the available cards that the player has unlocked
        /// </summary>
        public List<CardData> AvailableCards { get; private set; }

        /// <summary>
        /// A data structure for all of the player's decks the player has created
        /// </summary>
        public Deck[] Decks { get; private set; }

        public const int maxDeckNumber = 8;
        private const string PlayerCardRegistryDataAsset = "Content\\Data\\Player\\PlayerCardRegistryData.xml";

        #endregion

        public PlayerCardRegistry()
        {
            AvailableCards = new List<CardData>();
            Decks = new Deck[maxDeckNumber];

            for (int i = 0; i < maxDeckNumber; i++)
            {
                Decks[i] = new Deck();
            }
        }

        /// <summary>
        /// Load our player's card and deck data.
        /// Don't copy from the Central Card registry because we want to create multiples instances of the same card.
        /// If we used the Central Card Registry, they would all be the same?
        /// </summary>
        /// <param name="content"></param>
        public void LoadAssets(ContentManager content)
        {
            PlayerCardRegistryData playerData = AssetManager.LoadData<PlayerCardRegistryData>(PlayerCardRegistryDataAsset);
            DebugUtils.AssertNotNull(playerData);

            // Load resource cards
            foreach (string cardAsset in playerData.ResourceCardDataAssets)
            {
                // Change this to resource data when we create it
                CardData cardData = AssetManager.LoadData<CardData>(CentralCardRegistry.CardFolderPath + cardAsset);
                DebugUtils.AssertNotNull(cardData);

                AvailableCards.Add(cardData);
            }

            // Load decks too
            Debug.Assert(playerData.Decks.Count <= maxDeckNumber);

            int deckIndex = 0;
            foreach (DeckData deckData in playerData.Decks)
            {
                Debug.Assert(deckIndex < maxDeckNumber);
                DebugUtils.AssertNotNull(Decks[deckIndex]);

                Decks[deckIndex].Create(CentralCardRegistry.ConvertToDataList(playerData.ResourceCardDataAssets));

                deckIndex++;
            }
        }

        /// <summary>
        /// Save our player's configuration to XML.
        /// </summary>
        public void SaveAssets()
        {
            // Create a map of card types and a list of card assets we will serialize
            Dictionary<string, List<string>> dataMap = new Dictionary<string, List<string>>();

            // Set up all of our lists of card data for each resource type
            foreach (string cardType in CentralCardRegistry.CardTypes)
            {
                dataMap.Add(cardType, new List<string>());
            }

            // Loop through our available cards and add them into the appropriate list in our map
            foreach (CardData cardData in AvailableCards)
            {
                Debug.Assert(dataMap.ContainsKey(cardData.Type));
                Debug.Assert(CentralCardRegistry.CardData.ContainsValue(cardData));

                dataMap[cardData.Type].Add(CentralCardRegistry.FindCardDataAsset(cardData));
            }

            // This is fragile at the moment - need to have a way of mapping card type to Asset list
            PlayerCardRegistryData playerData = new PlayerCardRegistryData();
            playerData.ResourceCardDataAssets = dataMap["Resource"];

            // Now add our decks to our data
            for (int i = 0; i < maxDeckNumber; i++)
            {
                if (Decks[i].IsCreated)
                {
                    // If we have created this deck then create deck data and add to our PlayerCardRegistryData
                    DeckData deckData = new DeckData();
                    deckData.CardDataAssets = CentralCardRegistry.ConvertToAssetList(Decks[i]);

                    playerData.Decks.Add(deckData);
                }
            }

            // Save our player card registry data
            AssetManager.SaveData(playerData, PlayerCardRegistryDataAsset);
        }
    }
}
