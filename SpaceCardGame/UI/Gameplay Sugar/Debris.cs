using CelesteEngine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace SpaceCardGame
{
    /// <summary>
    /// A class for debris that is created when ships are destroyed 
    /// </summary>
    public class Debris : Asteroid
    {
        #region Properties and Fields

        /// <summary>
        /// A cached list of all the available debris assets - used for choosing a random one
        /// </summary>
        private static List<string> DebrisAssets = new List<string>()
        {
            "Objects\\Debris\\debris_sml0",
            "Objects\\Debris\\debris_sml1",
            "Objects\\Debris\\debris_sml2",
            "Objects\\Debris\\debris_sml3",
            "Objects\\Debris\\debris_med0",
            "Objects\\Debris\\debris_med1",
            "Objects\\Debris\\debris_med2",
            "Objects\\Debris\\debris_lrg0",
            "Objects\\Debris\\debris_lrg1",
        };

        #endregion

        public Debris(Vector2 localPosition) :
            base(MathUtils.GenerateFloat(5, 40), localPosition, DebrisAssets[MathUtils.GenerateInt(0, DebrisAssets.Count - 1)])
        {

        }

        #region Virtual Functions

        /// <summary>
        /// When our debris moves out of the window screen we should kill it
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            if (WorldPosition.X > ScreenManager.Instance.ScreenDimensions.X + Size.X)
            {
                Die();
            }
        }

        #endregion
    }
}