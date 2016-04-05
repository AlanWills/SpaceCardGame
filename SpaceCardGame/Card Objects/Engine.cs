using System;
using _2DEngine;
using _2DEngineData;
using Microsoft.Xna.Framework;
using SpaceCardGameData;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceCardGame
{
    /// <summary>
    /// Represents an Engine on our ship
    /// </summary>
    public class Engine : ShipAddOn
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
        private EngineBlaze EngineBlaze { get; set; }

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

            EngineBlaze = new EngineBlaze(Vector2.Zero);
            EngineBlaze.SetParent(this);
            EngineBlaze.LoadContent();

            base.LoadContent();
        }

        /// <summary>
        /// Initialises the Engine and the Engine Blaze
        /// </summary>
        public override void Initialise()
        {
            CheckShouldInitialise();

            EngineBlaze.Initialise();

            base.Initialise();
        }

        /// <summary>
        /// Fixes up the Engine Blaze animation
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            EngineBlaze.Begin();
            EngineBlaze.LocalPosition += new Vector2(0, EngineBlaze.Size.Y * 0.5f);
        }

        /// <summary>
        /// Handles input for this engine and the engine blaze
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            EngineBlaze.HandleInput(elapsedGameTime, mousePosition);
        }

        /// <summary>
        /// Updates the engine and the engine blaze
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            EngineBlaze.Update(elapsedGameTime);
        }

        /// <summary>
        /// Draws the engine and the engine blaze
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            EngineBlaze.Draw(spriteBatch);
        }

        /// <summary>
        /// Adds this engine to the inputted ship
        /// </summary>
        /// <param name="ship"></param>
        public override void AddToShip(Ship ship)
        {
            Debug.Fail("TODO");
        }

        #endregion
    }
}
