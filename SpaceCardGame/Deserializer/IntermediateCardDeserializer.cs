using _2DEngine;
using CardGameEngine;
using CardGameEngineData;
using Microsoft.Xna.Framework.Content;
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

            CentralCardRegistry.LoadCardType<AbilityCardData>(content, cardRegistryData.AbilityCardDataAssets, "Ability");
            CentralCardRegistry.LoadCardType<ShieldCardData>(content, cardRegistryData.ShieldCardDataAssets, "Shield");
            CentralCardRegistry.LoadCardType<ResourceCardData>(content, cardRegistryData.ResourceCardDataAssets, "Resource");
            CentralCardRegistry.LoadCardType<ShipCardData>(content, cardRegistryData.ShipCardDataAssets, "Ship");
            CentralCardRegistry.LoadCardType<WeaponCardData>(content, cardRegistryData.WeaponCardDataAssets, "Weapon");
        }
    }
}
