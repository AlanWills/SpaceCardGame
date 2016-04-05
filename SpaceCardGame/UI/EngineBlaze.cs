using _2DEngine;
using Microsoft.Xna.Framework;

namespace SpaceCardGame
{
    /// <summary>
    /// A class which represents an engine blaze in our game.
    /// Nothing more than an animated image, but as it will be useful it makes to have a separate class for it.
    /// </summary>
    public class EngineBlaze : AnimatedGameObject
    {
        public EngineBlaze(Vector2 localPosition) :
            this(Vector2.Zero, localPosition)
        {

        }

        public EngineBlaze(Vector2 size, Vector2 localPosition) :
            base(size, localPosition, "Content\\Data\\GameObjects\\EngineBlaze.xml")
        {
            UsesCollider = false;
        }
    }
}
