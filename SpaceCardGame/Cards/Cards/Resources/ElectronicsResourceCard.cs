namespace SpaceCardGame
{
    /// <summary>
    /// Class for the electronics resource card
    /// </summary>
    public class ElectronicsResourceCard : ResourceCard
    {
        public ElectronicsResourceCard(CardData resourceCardData) :
            base(resourceCardData)
        {
            ResourceType = ResourceType.Electronics;
        }
    }
}
