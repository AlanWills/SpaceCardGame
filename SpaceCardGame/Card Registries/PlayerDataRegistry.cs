using _2DEngine;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using SpaceCardGameData;

namespace SpaceCardGame
{
    /// <summary>
    /// A class for the player's cards and decks.
    /// Singleton.
    /// </summary>
    public class PlayerDataRegistry
    {
        #region Properties and Fields

        /// <summary>
        /// A singleton for the PlayerCardRegistry
        /// </summary>
        private static PlayerDataRegistry instance = new PlayerDataRegistry();
        public static PlayerDataRegistry Instance
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
        /// A flag to indicate whether we have loaded our data from disc - only need to do this once per session.
        /// </summary>
        private bool Loaded { get; set; }

        /// <summary>
        /// The data file for the PlayerDataRegistry
        /// </summary>
        public PlayerDataRegistryData PlayerData { get; set; }
        
        public const string playerCardRegistryDataAsset = "Player\\PlayerDataRegistryData.xml";
        public const string startingCardRegistryDataAsset = "Player\\StartingPlayerDataRegistryData.xml";
        public const int maxDeckNumber = 8;

        #endregion

        public PlayerDataRegistry()
        {
            ScreenManager.Instance.SaveAssets += SaveAssets;
        }

        #region Saving and Loading

        /// <summary>
        /// Load our player's card and deck data if we have not done so already.
        /// Don't copy from the Central Card registry because we want to create multiples instances of the same card.
        /// If we used the Central Card Registry, they would all be the same?
        /// </summary>
        /// <param name="content"></param>
        public void LoadAssets(string path)
        {
            // If we have already loaded there is no need to load again - just raises the possibility of overwriting data
            if (Loaded) { return; }

            AvailableCards = new List<CardData>();
            Decks = new Deck[maxDeckNumber];

            for (int i = 0; i < maxDeckNumber; i++)
            {
                Decks[i] = new Deck();
            }

            PlayerData = AssetManager.GetData<PlayerDataRegistryData>(path);
            DebugUtils.AssertNotNull(PlayerData);

            LoadCardType(PlayerData.AbilityCardDataAssets);
            LoadCardType(PlayerData.ShieldCardDataAssets);
            LoadCardType(PlayerData.ResourceCardDataAssets);
            LoadCardType(PlayerData.ShipCardDataAssets);
            LoadCardType(PlayerData.WeaponCardDataAssets);

            // Load decks too
            Debug.Assert(PlayerData.Decks.Count <= maxDeckNumber);

            int deckIndex = 0;
            foreach (DeckData deckData in PlayerData.Decks)
            {
                Debug.Assert(deckIndex < maxDeckNumber);
                DebugUtils.AssertNotNull(Decks[deckIndex]);

                Decks[deckIndex].Create(deckData.CardDataAssets);
                Decks[deckIndex].Name = deckData.Name;

                deckIndex++;
            }

            Loaded = true;
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

            Debug.Fail("Refactor this");
            // Maybe somehow just use the PlayerData's lists of string assets and keep loading them throughout the program

            // This is fragile at the moment - need to have a way of mapping card type to Asset list
            PlayerData.AbilityCardDataAssets = dataMap["Ability"];
            PlayerData.ShieldCardDataAssets = dataMap["Shield"];
            PlayerData.ResourceCardDataAssets = dataMap["Resource"];
            PlayerData.ShipCardDataAssets = dataMap["Ship"];
            PlayerData.WeaponCardDataAssets = dataMap["Weapon"];
            PlayerData.Decks = new List<DeckData>();

            // Now add our decks to our data
            for (int i = 0; i < maxDeckNumber; i++)
            {
                if (Decks[i].IsCreated)
                {
                    // If we have created this deck then create deck data and add to our PlayerCardRegistryData
                    DeckData deckData = new DeckData();
                    deckData.Name = Decks[i].Name;
                    deckData.CardDataAssets = CentralCardRegistry.ConvertToAssetList(Decks[i].Cards);

                    PlayerData.Decks.Add(deckData);
                }
            }

            // Save our player card registry data
            AssetManager.SaveData(PlayerData, playerCardRegistryDataAsset);
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

        #endregion

        #region Utility Functions

        /// <summary>
        /// A function to determine whether the player already has the inputted card data available or in a deck.
        /// </summary>
        /// <param name="cardData"></param>
        /// <returns></returns>
        public bool PlayerOwnsCard(CardData cardData)
        {
            // Check our available cards
            if (AvailableCards.Contains(cardData))
            {
                return true;
            }

            // Then check each deck
            for (int i = 0; i < Decks.Length; i++)
            {
                if (Decks[i] != null)
                {
                    if (Decks[i].Cards.Exists(x => x == cardData))
                    {
                        return true;
                    }
                }
            }

            // If we have got here then the player cannot own it
            return false;
        }

        #endregion
    }
}
