using _2DEngine;
using _2DEngineData;
using Microsoft.Xna.Framework;

namespace CardGameEngine
{
    /// <summary>
    /// A base class for our card object
    /// </summary>
    public class Card : GameObject
    {
        #region Properties and Fields

        #endregion

        public const string CardBackTextureAsset = "Sprites\\Cards\\Back";

        public Card(Vector2 localPosition, string dataAsset) :
            base(localPosition, dataAsset)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// Look up our data from the Card Registry rather than loading it from disc
        /// </summary>
        /// <returns></returns>
        protected override GameObjectData LoadGameObjectData()
        {
            return CentralCardRegistry.CardData[DataAsset];
        }

        /// <summary>
        /// Override this function to do on laid abilities
        /// </summary>
        public override void Begin()
        {
            base.Begin();
        }

        // Do normal during fight ability, plus a function to determine whether we can do it?

        /// <summary>
        /// Override this function to do on death abilities
        /// </summary>
        public override void Die()
        {
            base.Die();
        }

        #endregion
    }
}
