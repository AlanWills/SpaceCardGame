using CardGameEngine;
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

    /// <summary>
    /// A class used to represent a resource in our game.
    /// </summary>
    public class ResourceCard : GameCard
    {
        #region Properties and Fields

        /// <summary>
        /// An indicator of the resource type of this
        /// </summary>
        public ResourceType ResourceType { get; protected set; }

        /// <summary>
        /// A flag to indicate whether we have used the resource this card represents.
        /// Setting this will drive the card's flip state
        /// </summary>
        private bool used = false;
        public bool Used
        {
            get { return used; }
            set
            {
                // If our current use state is the same as the one we are setting we do nothing
                if (used == value)
                {
                    return;
                }
                else
                {
                    used = value;
                    Flip(used ? CardFlipState.kFaceDown : CardFlipState.kFaceUp);
                }
            }
        }

        #endregion

        public ResourceCard(ResourceCardData cardData) :
            base(cardData)
        {
            
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