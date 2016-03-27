using _2DEngine;
using Microsoft.Xna.Framework;

namespace SpaceCardGame
{
    /// <summary>
    /// A class to represent a card pack which we will open in the OpenCardPacksScreen.
    /// Will generate a set of 5 random cards from the card registry using their rarity.
    /// </summary>
    public class CardPack : UIObjectContainer
    {
        public CardPack(Vector2 localPosition, string textureAsset = GameCard.CardBackTextureAsset) :
            base(localPosition, textureAsset)
        {

        }
    }
}
