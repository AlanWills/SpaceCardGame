namespace SpaceCardGame
{
    /// <summary>
    /// Class for the metal resource card
    /// </summary>
    public class MetalResourceCard : ResourceCard
    {
        public MetalResourceCard(ResourceCardData resourceCardData) :
            base(resourceCardData)
        {
            ResourceType = ResourceType.Metal;
        }
    }
}
