using SpaceCardGameData;

namespace SpaceCardGame
{
    /// <summary>
    /// Class for the electronics resource card
    /// </summary>
    public class ElectronicsResourceCard : ResourceCard
    {
        public ElectronicsResourceCard(Player player, CardData resourceCardData) :
            base(player, resourceCardData)
        {
            ResourceType = ResourceType.Electronics;
        }
    }
}
