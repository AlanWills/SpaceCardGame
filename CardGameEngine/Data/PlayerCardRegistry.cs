using _2DEngine;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using CardGameEngineData;

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

        }

        /// <summary>
        /// Load our player's card and deck data.
        /// Don't copy from the Central Card registry because we want to create multiples instances of the same card.
        /// If we used the Central Card Registry, they would all be the same?
        /// </summary>
        /// <param name="content"></param>
        public void LoadAssets(ContentManager content)
        {
            PlayerCardRegistryData playerData = AssetManager.GetData<PlayerCardRegistryData>(PlayerCardRegistryDataAsset);
            DebugUtils.AssertNotNull(playerData);

            foreach (string cardAsset in playerData.CardDataAssets)
            {
                CardData cardData = AssetManager.GetData<CardData>(CentralCardRegistry.CardFolderPath + cardAsset);
                DebugUtils.AssertNotNull(cardData);

                AvailableCards.Add(cardData);
            }

            // Load decks too
        }
    }
}
