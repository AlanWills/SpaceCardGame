using _2DEngine;
using Microsoft.Xna.Framework;

namespace SpaceCardGame
{
    /// <summary>
    /// A class used for visual sugar, but has no impact on our game.
    /// Will implement some basic physics like angular and linear rotation so that it moves across our board.
    /// </summary>
    public class Asteroid : Image
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the rigid body module of the asteroid - this handles the physics of moving and rotating it.
        /// </summary>
        public RigidBodyModule RigidBody { get; set; }

        #endregion

        public Asteroid(float xLinearVelocity, float angularVelocity, Vector2 localPosition, string textureAsset) :
            base(localPosition, textureAsset)
        {
            RigidBody = AddModule(new RigidBodyModule(new Vector2(xLinearVelocity, 0), angularVelocity));
        }
    }
}
