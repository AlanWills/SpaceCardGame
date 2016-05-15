using _2DEngine;
using _2DEngineData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SpaceCardGameData;

namespace SpaceCardGame
{
    /// <summary>
    /// Represents an Engine on our ship
    /// </summary>
    public class Engine : CardObject
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

        /// <summary>
        /// A reference to the engine blaze for this Engine
        /// </summary>
        public EngineBlaze EngineBlaze { get; private set; }

        /// <summary>
        /// A reference to the engine SFX for this Engine
        /// </summary>
        public CustomSoundEffect EngineSFX { get; set; }

        // A string which represents the default engine all ships have 
        private const string defaultEngineDataAsset = "Cards\\Engines\\DefaultEngine.xml";

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
            return AssetManager.GetData<EngineData>(DataAsset);
        }

        /// <summary>
        /// Loads the engine data and sets up it's stats
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            EngineData = Data as EngineData;
            DebugUtils.AssertNotNull(EngineData);

            EngineSFX = new CustomSoundEffect("Engines\\Engine");
            EngineBlaze = AddChild(new EngineBlaze(Vector2.Zero));

            base.LoadContent();
        }

        /// <summary>
        /// Fixes up the Engine Blaze animation
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            EngineBlaze.LocalPosition += new Vector2(0, EngineBlaze.Size.Y * 0.5f);
        }

        /// <summary>
        /// Plays the engine sound effect
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            //if (EngineSFX.State == SoundState.Stopped)
            //{
            //    EngineSFX.Play();
            //}
        }

        #endregion
    }
}
