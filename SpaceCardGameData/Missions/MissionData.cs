using _2DEngineData;
using CardGameEngineData;

namespace SpaceCardGameData
{
    public class MissionData : BaseData
    {
        public string MissionName { get; set; }

        public string MissionThumbnailTextureAsset { get; set; }

        public string MissionDescription { get; set; }

        public DeckData OpponentDeckData { get; set; }
    }
}