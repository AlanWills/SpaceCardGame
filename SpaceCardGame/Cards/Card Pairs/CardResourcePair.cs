using _2DEngine;
using System.Diagnostics;
using Microsoft.Xna.Framework;

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

        public CardResourcePair(ResourceCardData resourceCardData) :
            base(resourceCardData)
        {
            Resource = AddChild(new Resource(Vector2.Zero, AssetManager.EmptyGameObjectDataAsset));
            CardObject = Resource;

            Debug.Assert(Card is ResourceCard);
            ResourceCard = Card as ResourceCard;

            // Make ready as soon as we lay it
            MakeReadyForCardPlacement();
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
