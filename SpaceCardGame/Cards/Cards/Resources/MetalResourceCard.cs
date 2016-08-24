using SpaceCardGameData;

namespace SpaceCardGame
{
    /// <summary>
    /// Class for the metal resource card
    /// </summary>
    public class MetalResourceCard : ResourceCard
    {
        public MetalResourceCard(Player player, CardData resourceCardData) :
            base(player, resourceCardData)
        {
            ResourceType = ResourceType.Metal;
        }
    }
}
