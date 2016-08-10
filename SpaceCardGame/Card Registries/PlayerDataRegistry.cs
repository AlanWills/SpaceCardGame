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
        
        public const string playerDataRegistryDataAsset = "Player\\PlayerDataRegistryData.xml";
        public const string startingDataRegistryDataAsset = "Player\\StartingPlayerDataRegistryData.xml";
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

            Decks = new Deck[maxDeckNumber];

            for (int i = 0; i < maxDeckNumber; i++)
            {
                Decks[i] = new Deck();
            }

            PlayerData = AssetManager.GetData<PlayerDataRegistryData>(path);
            DebugUtils.AssertNotNull(PlayerData);

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
            PlayerData.Decks.Clear();

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
            AssetManager.SaveData(PlayerData, playerDataRegistryDataAsset);
        }

        #endregion
    }
}
