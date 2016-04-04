using _2DEngine;
using _2DEngineData;
using Microsoft.Xna.Framework;
using SpaceCardGameData;

namespace SpaceCardGame
{
    /// <summary>
    /// Represents an Engine on our ship
    /// </summary>
    public class Engine : GameObject
    {
        #region Properties and Fields

        /// <summary>
        /// The amount of speed our engine has
        /// </summary>
        private float EngineSpeed { get; set; }

        /// <summary>
        /// A reference to the engine data for this Engine
        /// </summary>
        private EngineData EngineData { get; set; }

        // A string which represents the default engine all ships have 
        private const string defaultEngineDataAsset = "Content\\Data\\Cards\\Defence\\Engines\\DefaultEngine.xml";

        #endregion

        // A constructor used for creating a custom engine from a card
        public Engine(Vector2 localPosition, string dataAsset) :
            base(localPosition, dataAsset)
        {
            UsesCollider = false;
        }

        // Constructor used for creating a default engine for each ship
        public Engine(float engineSpeed, Vector2 localPosition) :
            this(localPosition, defaultEngineDataAsset)
        {
            EngineSpeed = engineSpeed;
        }

        #region Virtual Functions

        /// <summary>
        /// Loads the engine object data
        /// </summary>
        /// <returns></returns>
        protected override GameObjectData LoadGameObjectData()
        {
            return AssetManager.LoadData<EngineData>(DataAsset);
        }

        /// <summary>
        /// Loads the engine data and sets up it's stats
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            EngineData = Data as EngineData;
            DebugUtils.AssertNotNull(EngineData);

            base.LoadContent();
        }

        #endregion

        // TODO Engine contrail
    }
}
