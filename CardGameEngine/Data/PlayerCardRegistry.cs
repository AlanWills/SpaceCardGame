using _2DEngine;
using System.Collections.Generic;
using CardGameEngineData;
using System.Diagnostics;
using System;

namespace CardGameEngine
{
    /// <summary>
    /// A class for the player's cards and decks.
    /// Singleton.
    /// </summary>
    public class PlayerCardRegistry
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

        /// <summary>
        /// A property just to return the number of created decks the player has 
        /// </summary>
        public int AvailableDecks
        {
            get
            {
                return Array.FindAll(Decks, x => x.IsCreated).Length;
            }
        }

        /// <summary>
        /// A flag to indicate the number of packs our player can open
        /// </summary>
        public int AvailablePacksToOpen { get; set; }

        public const string playerCardRegistryDataAsset = "\\Data\\Player\\PlayerCardRegistryData.xml";
        public const string startingCardRegistryDataAsset = "\\Data\\Player\\StartingPlayerCardRegistryData.xml";
        public const int maxDeckNumber = 8;

        #endregion

        public PlayerCardRegistry()
        {
            ScreenManager.Instance.SaveAssets += SaveAssets;
        }

        /// <summary>
        /// Load our player's card and deck data.
        /// Don't copy from the Central Card registry because we want to create multiples instances of the same card.
        /// If we used the Central Card Registry, they would all be the same?
        /// </summary>
        /// <param name="content"></param>
        public void LoadAssets(string path)
        {
            AvailableCards = new List<CardData>();
            Decks = new Deck[maxDeckNumber];

            for (int i = 0; i < maxDeckNumber; i++)
            {
                Decks[i] = new Deck();
            }

            PlayerCardRegistryData playerData = AssetManager.LoadData<PlayerCardRegistryData>("Content" + path);
            DebugUtils.AssertNotNull(playerData);

            LoadCardType(playerData.AbilityCardDataAssets);
            LoadCardType(playerData.DefenceCardDataAssets);
            LoadCardType(playerData.ResourceCardDataAssets);
            LoadCardType(playerData.ShipCardDataAssets);
            LoadCardType(playerData.WeaponCardDataAssets);

            // Load decks too
            Debug.Assert(playerData.Decks.Count <= maxDeckNumber);

            int deckIndex = 0;
            foreach (DeckData deckData in playerData.Decks)
            {
                Debug.Assert(deckIndex < maxDeckNumber);
                DebugUtils.AssertNotNull(Decks[deckIndex]);

                Decks[deckIndex].Create(CentralCardRegistry.ConvertToDataList(deckData.CardDataAssets));
                Decks[deckIndex].Name = deckData.Name;

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

                dataMap[cardData.Type].Add(CentralCardRegistry.FindCardDataAsset(cardData));
            }

            // This is fragile at the moment - need to have a way of mapping card type to Asset list
            PlayerCardRegistryData playerData = new PlayerCardRegistryData();
            playerData.AbilityCardDataAssets = dataMap["Ability"];
            playerData.DefenceCardDataAssets = dataMap["Defence"];
            playerData.ResourceCardDataAssets = dataMap["Resource"];
            playerData.ShipCardDataAssets = dataMap["Ship"];
            playerData.WeaponCardDataAssets = dataMap["Weapon"];
            playerData.Decks = new List<DeckData>();

            // Now add our decks to our data
            for (int i = 0; i < maxDeckNumber; i++)
            {
                if (Decks[i].IsCreated)
                {
                    // If we have created this deck then create deck data and add to our PlayerCardRegistryData
                    DeckData deckData = new DeckData();
                    deckData.Name = Decks[i].Name;
                    deckData.CardDataAssets = CentralCardRegistry.ConvertToAssetList(Decks[i].Cards);

                    playerData.Decks.Add(deckData);
                }
            }

            // Save our player card registry data
            AssetManager.SaveData(playerData, "Content" + playerCardRegistryDataAsset);
        }

        #region Utility Functions for saving and loading

        /// <summary>
        /// Load our resource cards not being used in decks
        /// </summary>
        /// <param name="content"></param>
        private void LoadCardType(List<string> assetsToLoad)
        {
            // Load the resource cards from the central registry
            List<CardData> data = CentralCardRegistry.ConvertToDataList(assetsToLoad);

            AvailableCards.AddRange(data);
        }

        #endregion
    }
}
