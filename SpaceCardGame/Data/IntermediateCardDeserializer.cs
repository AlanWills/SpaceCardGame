using _2DEngine;
using CardGameEngine;
using CardGameEngineData;
using Microsoft.Xna.Framework.Content;
using SpaceCardGameData;
using System.Diagnostics;

namespace SpaceCardGame
{
    public static class IntermediateCardDeserializer
    {
        public static void LoadAssets(ContentManager content)
        {
            Debug.Assert(CentralCardRegistry.IsLoaded);
            DebugUtils.AssertNotNull(CentralCardRegistry.CardData);
            CardRegistryData cardRegistryData = CentralCardRegistry.CardRegistryData;

            CentralCardRegistry.LoadCardType<CardData>(content, cardRegistryData.AbilityCardDataAssets, "Ability");
            CentralCardRegistry.LoadCardType<CardData>(content, cardRegistryData.DefenceCardDataAssets, "Defence");
            CentralCardRegistry.LoadCardType<ResourceCardData>(content, cardRegistryData.ResourceCardDataAssets, "Resource");
            CentralCardRegistry.LoadCardType<CardData>(content, cardRegistryData.ShipCardDataAssets, "Ship");
            CentralCardRegistry.LoadCardType<CardData>(content, cardRegistryData.WeaponCardDataAssets, "Weapon");
        }
    }
}
