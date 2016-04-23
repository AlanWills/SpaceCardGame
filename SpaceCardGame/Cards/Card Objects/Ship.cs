using _2DEngine;
using _2DEngineData;
using Microsoft.Xna.Framework;
using SpaceCardGameData;
using System.Collections.Generic;
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
        /// A reference to the engine for our ship.
        /// Used mainly for fancy animation.
        /// </summary>
        public Engine Engine { get; set; }

        /// <summary>
        /// A list of the ui we use to signify damage
        /// </summary>
        public List<DamageUI> DamageUI { get; private set; }

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
            return AssetManager.GetData<ShipData>(DataAsset);
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
            DamageModule.OnDamage += ShowDamage;

            // Add engine UI from our data
            Debug.Assert(ShipData.EngineHardpoints.Count > 0);
            foreach (Vector2 engineHardpoint in ShipData.EngineHardpoints)
            {
                Engine = AddChild(new Engine(ShipData.Speed, engineHardpoint));
            }

            Debug.Assert(ShipData.Defence > 0);
            Debug.Assert(ShipData.DamageHardpoints.Count == ShipData.Defence - 1);
            DamageUI = new List<DamageUI>(ShipData.Defence - 1);            

            for (int i = 0; i < ShipData.Defence - 1; i++)
            {
                DamageUI damageUI = AddChild(new DamageUI(ShipData.DamageHardpoints[i]));
                damageUI.Hide();            // Initially hide all the damage UI
                DamageUI.Add(damageUI);
            }

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
                CommandManager.Instance.AddChild(new AttackOpponentShipCommand(Parent as CardShipPair), true, true);
            }

            // Updates the colour of the ship if our mouse is over it
            Parent.Colour.Value = Collider.IsMouseOver ? Color.Green : Color.White;
        }

        /// <summary>
        /// Make sure that we still hide any damage that should not be visible yet
        /// </summary>
        /// <param name="showChildren"></param>
        public override void Show(bool showChildren = true)
        {
            base.Show(showChildren);

            ShowDamage(DamageModule);
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

        #region Event Callbacks

        /// <summary>
        /// Shows damage on the ship corresponding to how much we have taken
        /// </summary>
        private void ShowDamage(DamageableObjectModule damageModule)
        {
            if (DamageModule.Health <= 0)
            {
                // Don't add damage if we have no health - in the end we will implement the damage happening when our bullet hits so it won't matter that we skip this
                return;
            }

            // Hide any damage we no longer need to show
            for (int i = 0; i < (int)DamageModule.Health - 1; i++)
            {
                Debug.Assert(i < DamageUI.Count);
                DamageUI[i].Hide();
            }

            // Turn on damage equal to the damage we have taken
            for (int i = (int)DamageModule.Health - 1; i < ShipData.Defence - 1; ++i)
            {
                Debug.Assert(i < DamageUI.Count);
                DamageUI[i].Show();
            }
        }

        #endregion
    }
}