using _2DEngine;
using Microsoft.Xna.Framework;

namespace SpaceCardGame
{
    /// <summary>
    /// A class which represents an explosion in our game.
    /// Nothing more than an animated image, but as it will be useful it makes sense to add it to as a separate class.
    /// </summary>
    public class Explosion : AnimatedUIObject
    {
        public Explosion(Vector2 localPosition) :
            this(Vector2.Zero, localPosition)
        {

        }

        public Explosion(Vector2 size, Vector2 localPosition) :
            base(size, localPosition, "Content\\Data\\GameObjects\\Explosion.xml")
        {

        }
    }
}