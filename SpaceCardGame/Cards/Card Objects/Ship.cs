using _2DEngine;
using _2DEngineData;
using Microsoft.Xna.Framework;
using SpaceCardGameData;
using System.Diagnostics;

namespace SpaceCardGame
{
    public class Ship : GameObject
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the data for the ship this object represents
        /// </summary>
        public ShipData ShipData { get; private set; }

        /// <summary>
        /// A local reference to the parent CardShipPair (for convenience)
        /// </summary>
        private CardShipPair CardShipPair { get; set; }
        
        /// <summary>
        /// A reference to our damageable object module - useful for reference to it's health etc.
        /// </summary>
        public DamageableObjectModule DamageModule { get; private set; }

        /// <summary>
        /// A reference to our highlight module - used as fancy animation
        /// </summary>
        private HighlightOnHoverModule HighlightModule { get; set; }

        /// <summary>
        /// A reference to the turret for our ship.
        /// We will create a default turret from the ship data and then can override it by adding a turret card to the ship
        /// </summary>
        public Turret Turret { get; set; }

        /// <summary>
        /// A reference to the shield for our ship.
        /// By default this will not be set to anything, but can be set by adding a shield card to the ship
        /// </summary>
        public Shield Shield { get; set; }

        /// <summary>
        /// A reference to the shield for our ship.
        /// By default we will create a default engine from the ship data and then can override by adding an engine card to the ship
        /// </summary>
        public Engine Engine { get; private set; }

        #endregion

        // The ship is tied to the card, so it's position will be amended when the card is added to the screen
        public Ship(string shipDataAsset) :
            base(Vector2.Zero, shipDataAsset)
        {
            
        }

        #region Virtual Functions

        /// <summary>
        /// Loads the ship object data
        /// </summary>
        /// <returns></returns>
        protected override GameObjectData LoadGameObjectData()
        {
            return AssetManager.LoadData<ShipData>(DataAsset);
        }

        /// <summary>
        /// Loads the ship data and sets up it's stats
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            ShipData = Data as ShipData;
            DebugUtils.AssertNotNull(ShipData);

            DamageModule = AddModule(new DamageableObjectModule(ShipData.Defence));
            HighlightModule = AddModule(new HighlightOnHoverModule(Color.White, Color.LightGray, BlendMode.kBinary));

            Engine = AddChild(new Engine(ShipData.Speed, Vector2.Zero));

            base.LoadContent();
        }

        /// <summary>
        /// Sets up the reference to our CardShipPair
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            DebugUtils.AssertNotNull(Parent);
            CardShipPair = Parent as CardShipPair;
        }

        /// <summary>
        /// If the ship is selected and ready, we trigger a script to handle the attacking of other ships
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            DebugUtils.AssertNotNull(Collider);
            if (Collider.IsClicked && Turret.CanFire && CardShipPair.IsReady)
            {
                DebugUtils.AssertNotNull(Parent);
                Debug.Assert(Parent is CardShipPair);
                ScriptManager.Instance.AddChild(new AttackOpponentShipScript(Parent as CardShipPair), true, true);
            }
        }

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
