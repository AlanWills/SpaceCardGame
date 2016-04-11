using SpaceCardGameData;
using System;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// An enum for each resource card type in our game currently
    /// </summary>
    public enum ResourceType
    {
        Crew,
        Electronics,
        Fuel,
        Metal,
        kNumResourceTypes
    }

    public class ResourceCard : GameCard
    {
        /// <summary>
        /// An indicator of the resource type of this
        /// </summary>
        public ResourceType ResourceType { get; private set; }

        public ResourceCard(ResourceCardData cardData) :
            base(cardData)
        {
            ResourceType = (ResourceType)Enum.Parse(typeof(ResourceType), cardData.ResourceType);
        }

        #region Virtual Functions

        /// <summary>
        /// Do a check to make sure we definitely have set our resource type
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            Debug.Assert(ResourceType != ResourceType.kNumResourceTypes);
        }

        #endregion
    }
}