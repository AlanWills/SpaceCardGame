using _2DEngine;
using Microsoft.Xna.Framework;
using SpaceCardGameData;

namespace SpaceCardGame
{
    /// <summary>
    /// A class which represents an ability (spell in the conventional sense).
    /// </summary>
    public class Ability : CardObject
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the ability data for this ability
        /// </summary>
        private AbilityData AbilityData { get; set; }

        #endregion

        // A constructor used for creating a custom engine from a card
        public Ability(string abilityDataAsset) :
            base(Vector2.Zero, abilityDataAsset)
        {
            UsesCollider = false;
        }
    }
}
