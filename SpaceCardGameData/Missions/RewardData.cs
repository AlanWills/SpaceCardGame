using _2DEngineData;
using System.Collections.Generic;

namespace SpaceCardGameData
{
    public class RewardData : BaseData
    {
        public int Money { get; set; }
        public List<string> CardDataAssets { get; set; }
        public int CardPacks { get; set; }
    }
}
