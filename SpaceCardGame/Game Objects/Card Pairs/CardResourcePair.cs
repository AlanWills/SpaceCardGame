using System;
using _2DEngine;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A class which uses the CardObjectPair class but is specifically used with Resources.
    /// </summary>
    public class CardResourcePair : CardObjectPair
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the stored card as a ResourceCard (saves casting elsewhere)
        /// </summary>
        public ResourceCard ResourceCard { get; private set; }

        /// <summary>
        /// A reference to the stored game object as a Turret (saves casting elsewhere)
        /// </summary>
        public Resource Resource { get; private set; }

        #endregion

        public CardResourcePair(ResourceCard resourceCard, Resource resource) :
            base(resourceCard, resource)
        {
            DebugUtils.AssertNotNull(resourceCard);
            DebugUtils.AssertNotNull(resource);

            ResourceCard = resourceCard;
            Resource = resource;
        }

        #region Virtual Functions

        /// <summary>
        /// Cannot add a resource to a ship
        /// </summary>
        /// <param name="cardShipPair"></param>
        public override void AddToCardShipPair(CardShipPair cardShipPair)
        {
            Debug.Fail("Cannot add a resource to a ship");
        }

        #endregion
    }
}
