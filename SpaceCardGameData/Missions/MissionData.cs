using CelesteEngineData;

namespace SpaceCardGameData
{
    public class MissionData : BaseData
    {
        public string MissionName { get; set; }

        public string MissionThumbnailTextureAsset { get; set; }

        /// <summary>
        /// Zero based index
        /// </summary>
        public int MissionNumber { get; set; }

        public string MissionDescription { get; set; }

        public DeckData OpponentDeckData { get; set; }

        public RewardData RewardData { get; set; }
    }
}