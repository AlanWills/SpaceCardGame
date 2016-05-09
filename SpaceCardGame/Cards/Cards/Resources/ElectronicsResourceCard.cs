namespace SpaceCardGame
{
    /// <summary>
    /// Class for the electronics resource card
    /// </summary>
    public class ElectronicsResourceCard : ResourceCard
    {
        public ElectronicsResourceCard(ResourceCardData resourceCardData) :
            base(resourceCardData)
        {
            ResourceType = ResourceType.Electronics;
        }
    }
}
