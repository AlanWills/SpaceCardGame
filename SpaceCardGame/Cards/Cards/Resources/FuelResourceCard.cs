using SpaceCardGameData;

namespace SpaceCardGame
{
    /// <summary>
    /// Class for the fuel resource card
    /// </summary>
    public class FuelResourceCard : ResourceCard
    {
        public FuelResourceCard(Player player, CardData resourceCardData) :
            base(player, resourceCardData)
        {
            ResourceType = ResourceType.Fuel;
        }
    }
}
