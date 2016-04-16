﻿using _2DEngine;
using Microsoft.Xna.Framework;

namespace SpaceCardGame
{
    /// <summary>
    /// A class which represents an explosion in our game.
    /// Nothing more than an animated image, but as it will be useful it makes sense to add it to as a separate class.
    /// </summary>
    public class Explosion : AnimatedGameObject
    {
        public Explosion(Vector2 localPosition) :
            base(localPosition, "Content\\Data\\GameObjects\\Explosion.xml")
        {
            UsesCollider = false;
        }
    }
}