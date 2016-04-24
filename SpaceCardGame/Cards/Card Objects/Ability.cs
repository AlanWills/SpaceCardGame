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

        #region Virtual Functions

        /// <summary>
        /// Loads our ability data
        /// </summary>
        /// <returns></returns>
        /*protected override GameObjectData LoadGameObjectData()
        {
            return AssetManager.GetData<AbilityData>(DataAsset);
        }

        /// <summary>
        /// Loads up the ability data
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            AbilityData = Data as AbilityData;

            base.LoadContent();
        }*/


        /// <summary>
        /// Kills our parent which will kill us and the card we are attached too
        /// </summary>
        public override void Die()
        {
            // Make sure we call Die so that when our parent calls Die on us again, we will already be dead and not have this function called again
            base.Die();

            DebugUtils.AssertNotNull(Parent);
            Parent.Die();
        }

        #endregion
    }
}
