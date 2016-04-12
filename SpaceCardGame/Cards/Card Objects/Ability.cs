using _2DEngine;
using _2DEngineData;
using Microsoft.Xna.Framework;
using SpaceCardGameData;

namespace SpaceCardGame
{
    /// <summary>
    /// A class which represents an ability (spell card in the conventional sense).
    /// </summary>
    public class Ability : GameObject
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

        #region Properties and Fields

        /// <summary>
        /// Loads our ability data
        /// </summary>
        /// <returns></returns>
        protected override GameObjectData LoadGameObjectData()
        {
            return AssetManager.LoadData<AbilityData>(DataAsset);
        }

        /// <summary>
        /// Loads up the ability data
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            AbilityData = Data as AbilityData;

            base.LoadContent();
        }

        #endregion
    }
}
