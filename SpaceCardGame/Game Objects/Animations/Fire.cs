﻿using CelesteEngine;
using Microsoft.Xna.Framework;

namespace SpaceCardGame
{
    /// <summary>
    /// A class which represents a fire in our game.
    /// Nothing more than an animated image.
    /// </summary>
    public class Fire : AnimatedGameObject
    {
        public Fire(Vector2 localPosition) :
            base(localPosition, "GameObjects\\Animations\\RedFire")
        {
            UsesCollider = false;
        }

        #region Virtual Functions

        public override void Initialise()
        {
            CheckShouldInitialise();

            base.Initialise();

            Size *= 0.5f;
        }

        #endregion
    }
}
