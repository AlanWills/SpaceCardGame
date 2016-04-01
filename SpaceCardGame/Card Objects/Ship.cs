using System;
using _2DEngine;
using _2DEngineData;
using Microsoft.Xna.Framework;
using SpaceCardGameData;
using System.Diagnostics;

namespace SpaceCardGame
{
    public class Ship : GameObjectContainer, IDamageable
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the data for the ship this object represents
        /// </summary>
        private ShipData ShipData { get; set; }

        /// <summary>
        /// A float to represent the health of this ship
        /// </summary>
        private float health;
        public float Health
        {
            get { return health; }
            private set
            {
                health = value;
                HandleCurrentHealth();
            }
        }

        /// <summary>
        /// A reference to the turret for our ship.
        /// We will create a default turret and then can override it by adding a turret card to our ship
        /// </summary>
        private Turret Turret { get; set; }

        #endregion

        // The ship is tied to the card, so it's position will be amended when the card is added to the screen
        public Ship(ShipCardData cardData) :
            base(Vector2.Zero, cardData.ObjectDataAsset)
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

            Health = ShipData.Defence;

            Turret = AddObject(new Turret(ShipData.Attack, Vector2.Zero));

            base.LoadContent();
        }

        /// <summary>
        /// Handles attacking
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            DebugUtils.AssertNotNull(Collider);
            if (GameMouse.Instance.IsClicked(MouseButton.kLeftButton))
            {
                Debug.Assert(ScreenManager.Instance.CurrentScreen is BattleScreen);
                BattleScreen battleScreen = ScreenManager.Instance.CurrentScreen as BattleScreen;

                foreach (CardObjectPair pair in battleScreen.Board.NonActivePlayerBoardSection.PlayerGameBoardSection.PlayerShipCardControl)
                {
                    // We check intersection with mouse position here, because the object may not have actually had it's HandleInput called yet
                    // Could do this stuff in the Update loop, but this is really what this function is for so do this CheckIntersects instead for clarity 
                    if (pair.CardObject != this && pair.CardObject.Collider.CheckIntersects(mousePosition))
                    {
                        Turret.Fire(pair.CardObject);
                    }
                }
            }
        }

        #endregion

        #region IDamagable Interface Functions

        /// <summary>
        /// Damages the ship by reducing it's health by the inputted amount
        /// </summary>
        /// <param name="damage"></param>
        public void Damage(float damage)
        {
            health -= damage;
        }

        /// <summary>
        /// Analyses the ship's current health and kills it if it has no health left
        /// </summary>
        public void HandleCurrentHealth()
        {
            if (Health <= 0)
            {
                DebugUtils.AssertNotNull(GetParent());
                GetParent().Die();          // Kill our parent, which will clean up ourselves and our card
            }
        }

        #endregion
    }
}
