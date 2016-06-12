namespace SpaceCardGame
{
    /// <summary>
    /// Class for the crew resource card
    /// </summary>
    public class CrewResourceCard : ResourceCard
    {
        public CrewResourceCard(CardData resourceCardData) :
            base(resourceCardData)
        {
            ResourceType = ResourceType.Crew;
        }
    }
}
