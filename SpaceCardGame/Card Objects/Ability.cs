using _2DEngine;
using Microsoft.Xna.Framework;

namespace SpaceCardGame
{
    public class Ability : GameObject
    {
        // A constructor used for creating a custom engine from a card
        public Ability(Vector2 localPosition, string dataAsset) :
            base(localPosition, dataAsset)
        {
            UsesCollider = false;
        }
    }
}
