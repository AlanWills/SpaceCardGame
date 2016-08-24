using SpaceCardGameData;

namespace SpaceCardGame
{
    /// <summary>
    /// Class for the crew resource card
    /// </summary>
    public class CrewResourceCard : ResourceCard
    {
        public CrewResourceCard(Player player, CardData resourceCardData) :
            base(player, resourceCardData)
        {
            ResourceType = ResourceType.Crew;
        }
    }
}
