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
            IsReady.Value = true;
            
            // No hover info for resources - there is no need
            AddHoverInfoModule = false;
        }

        #region Virtual Functions

        /// <summary>
        /// Alters the Player's resource count and changes the position of the resource card to fit with the current placed ones.
        /// </summary>
        /// <param name="gameBoard"></param>
        /// <param name="player"></param>
        public override void WhenAddedToGameBoard(GameBoardSection gameBoard, GamePlayer player)
        {
            Debug.Assert(player.ResourceCardsPlacedThisTurn < GamePlayer.ResourceCardsCanLay);

            float padding = 10;

            int typeIndex = (int)ResourceCard.ResourceType;
            int cardCount = player.Resources[typeIndex].Count;

            ResourceCard.Size *= 0.7f;

            if (cardCount == 0)
            {
                // We are adding the first resource card of this type
                LocalPosition = new Vector2((-gameBoard.Size.X + ResourceCard.Size.X) * 0.5f + padding, -gameBoard.Size.Y * 0.5f + ResourceCard.Size.Y * 0.5f + padding + typeIndex * (ResourceCard.Size.Y + padding));
            }
            else
            {
                // We are adding another resource card, so overlay it on top and slightly to the side of the previous one
                LocalPosition = player.Resources[typeIndex][cardCount - 1].Parent.LocalPosition + new Vector2(ResourceCard.Size.X * 0.15f, 0);
            }

            player.Resources[typeIndex].Add(ResourceCard);
            player.ResourceCardsPlacedThisTurn++;
        }

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
