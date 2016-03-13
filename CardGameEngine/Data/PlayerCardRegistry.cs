﻿using _2DEngine;
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

        public const string playerCardRegistryDataAsset = "\\Data\\Player\\PlayerCardRegistryData.xml";
        public const string startingCardRegistryDataAsset = "\\Data\\Player\\StartingPlayerCardRegistryData.xml";
        public const int maxDeckNumber = 8;

        #endregion

        public PlayerCardRegistry()
        {
            
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

            LoadCardType<CardData>(playerData.AbilityCardDataAssets);
            LoadCardType<CardData>(playerData.DefenceCardDataAssets);
            LoadCardType<CardData>(playerData.ResourceCardDataAssets);
            LoadCardType<CardData>(playerData.ShipCardDataAssets);
            LoadCardType<CardData>(playerData.WeaponCardDataAssets);

            // Load decks too
            Debug.Assert(playerData.Decks.Count <= maxDeckNumber);

            int deckIndex = 0;
            foreach (DeckData deckData in playerData.Decks)
            {
                Debug.Assert(deckIndex < maxDeckNumber);
                DebugUtils.AssertNotNull(Decks[deckIndex]);

                //Decks[deckIndex].Create(Player.CardData);
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
                Debug.Assert(CentralCardRegistry.CardData.ContainsValue(cardData));

                dataMap[cardData.Type].Add(CentralCardRegistry.FindCardDataAsset(cardData));
            }

            // This is fragile at the moment - need to have a way of mapping card type to Asset list
            PlayerCardRegistryData playerData = new PlayerCardRegistryData();
            playerData.ResourceCardDataAssets = dataMap["Ability"];
            playerData.ResourceCardDataAssets = dataMap["Defence"];
            playerData.ResourceCardDataAssets = dataMap["Resource"];
            playerData.ResourceCardDataAssets = dataMap["Ship"];
            playerData.ResourceCardDataAssets = dataMap["Weapon"];

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
            AssetManager.SaveData(playerData, playerCardRegistryDataAsset);
        }

        #region Utility Functions for saving and loading

        /// <summary>
        /// Load our resource cards
        /// </summary>
        /// <param name="content"></param>
        private void LoadCardType<T>(List<string> assetsToLoad) where T : CardData
        {
            // Check we actually have cards to load
            Debug.Assert(assetsToLoad.Count > 0);

            // Load the resource cards
            foreach (string cardDataAsset in assetsToLoad)
            {
                // Add resource data when we implement it
                CardData data = AssetManager.LoadData<T>(CentralCardRegistry.CardFolderPath + cardDataAsset);
                DebugUtils.AssertNotNull(data);

                AvailableCards.Add(data);
            }
        }

        #endregion
    }
}
