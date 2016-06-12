namespace SpaceCardGame
{
    /// <summary>
    /// Class for the fuel resource card
    /// </summary>
    public class FuelResourceCard : ResourceCard
    {
        public FuelResourceCard(CardData resourceCardData) :
            base(resourceCardData)
        {
            ResourceType = ResourceType.Fuel;
        }
    }
}
