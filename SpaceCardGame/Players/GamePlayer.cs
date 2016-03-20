using CardGameEngine;

namespace SpaceCardGame
{
    public class GamePlayer : Player
    {
        #region Properties and Fields

        public int[] AvailableResources { get; private set; }

        public int ResourceCardsPlacedThisTurn { get; set; }

        #endregion

        public GamePlayer(Deck chosenDeck) :
            base(chosenDeck)
        {
            AvailableResources = new int[(int)ResourceType.kNumResourceTypes];
            for (ResourceType resourceType = ResourceType.Crew; resourceType < ResourceType.kNumResourceTypes; resourceType++)
            {
                AvailableResources[(int)resourceType] = 0;
            }
        }
    }
}
