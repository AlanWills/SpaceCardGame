using CelesteEngine;
using Microsoft.Xna.Framework;
using SpaceCardGameData;
using System;

namespace SpaceCardGame
{
    public class MainMenuScreen : MenuScreen
    {
        private const float shipSpawnTime = 2;
        private float currentShipSpawnTimer = shipSpawnTime;

        private const float bulletSpawnTime = 0.2f;
        private float currentBulletSpawnTime;

        private const int missileSpawnCounter = 5;
        private float currentMissileSpawnCounter = missileSpawnCounter;

        public MainMenuScreen() :
            base("MainMenuScreen")
        {
            AddModule(new SpaceBackgroundModule(this));
        }

        #region Virtual Functions

        /// <summary>
        /// Add Buttons to our MainMenuScreen
        /// </summary>
        protected override void AddInitialUI()
        {
            base.AddInitialUI();

            ListControl buttonControl = AddScreenUIObject(new ListControl(new Vector2(ScreenDimensions.X * 0.75f, ScreenDimensions.Y * 0.4f)));
            buttonControl.UseScissorRectangle = false;

            Button newGameButton = buttonControl.AddChild(new Button("New Campaign", Vector2.Zero));
            newGameButton.AddModule(new ToolTipModule("Begin a new campaign"));
            newGameButton.ClickableModule.OnLeftClicked += OnNewGameButtonLeftClicked;

            Button continueGameButton = buttonControl.AddChild(new Button("Continue Campaign", Vector2.Zero));
            continueGameButton.ClickableModule.OnLeftClicked += OnContinueButtonLeftClicked;

            Button optionsButton = buttonControl.AddChild(new Button("Options", Vector2.Zero));
            optionsButton.ClickableModule.OnLeftClicked += OnOptionsButtonClicked;

#if DEBUG
            // If in debug add the hardpoint screen option
            Button hardpointButton = buttonControl.AddChild(new Button("Hardpoint Calculator", Vector2.Zero));
            hardpointButton.ClickableModule.OnLeftClicked += OnHardpointButtonClicked;
#endif

            Button exitGameButton = buttonControl.AddChild(new Button("Exit", Vector2.Zero));
            exitGameButton.ClickableModule.OnLeftClicked += OnExitGameButtonClicked;

            // Add static string on each card for their data asset?
        }

        /// <summary>
        /// Add our ships here so that we can load and initialise them immediately to fix up sizes etc.
        /// </summary>
        public override void Initialise()
        {
            CheckShouldInitialise();

            base.Initialise();

            AddShips();
        }

        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            currentShipSpawnTimer += elapsedGameTime;
            if (currentShipSpawnTimer > shipSpawnTime)
            {
                currentShipSpawnTimer = 0;
                SpawnSmallShip();
            }

            currentBulletSpawnTime += elapsedGameTime;
            // Firing missiles is done in FireAtSmallShips so we don't have to loop over objects multiple times
            if (currentBulletSpawnTime > bulletSpawnTime)
            {
                currentBulletSpawnTime = 0;
                Fire();
            }
        }

        #endregion

        #region Animated Ship UI Sugar

        private void AddShips()
        {
            GameObject eagleFrigate = AddGameObject(new GameObject(new Vector2(ScreenDimensions.X * 0.35f, ScreenDimensions.Y * 0.35f), "Cards\\Ships\\EagleFrigate\\EagleFrigateObject.xml"), true, true);
            eagleFrigate.Name = "Eagle Frigate";
            eagleFrigate.LocalRotation = MathHelper.PiOver2;
            float maxDimension = Math.Max(eagleFrigate.Size.X, eagleFrigate.Size.Y);

            Image eagleShield = eagleFrigate.AddChild(new Image(new Vector2(1.75f * maxDimension), Vector2.Zero, "Cards\\Shields\\PhaseEnergyShield\\PhaseEnergyShield"), true, true);

            GameObject pirateRaider = AddGameObject(new GameObject(new Vector2(ScreenDimensions.X * 0.25f, ScreenDimensions.Y * 0.85f), "Cards\\Ships\\PirateRaider\\PirateRaiderObject.xml"), true, true);
            pirateRaider.Name = "Pirate Raider";
        }

        private void SpawnSmallShip()
        { 
            GameObject smallShip = AddGameObject(new GameObject(new Vector2(-100, ScreenDimensions.Y * 0.1f), "Cards\\Ships\\BlazeInterceptor\\BlazeInterceptorObject.xml"), true, true);
            smallShip.LocalRotation = MathHelper.PiOver2;
            smallShip.Name = "Target";
            smallShip.AddModule(new MoveToDestinationModule(new Vector2(ScreenDimensions.X + 100, ScreenDimensions.Y * 0.1f), 600), true, true);
            smallShip.AddModule(new LifeTimeModule(3.5f), true, true);
        }

        /// <summary>
        /// Called when we should fire bullets.
        /// We also fire missiles after we have called this missileSpawnCounter number of times.
        /// This function takes care of maintaining the missile counter.
        /// </summary>
        private void Fire()
        {
            GameObject eagleFrigate = FindGameObject<GameObject>(x => x.Name == "Eagle Frigate");
            DebugUtils.AssertNotNull(eagleFrigate);

            // Fire at the latest small ship we have added - one should always exist so don't bother checking the result of LastChild
            Projectile projectile = AddGameObject(new Bullet(GameObjects.LastChild<GameObject>(x => x.Name == "Target"), eagleFrigate.WorldPosition, AssetManager.GetData<ProjectileData>("Cards\\Weapons\\Kinetic\\GatlingLaserTurret\\GatlingLaserTurretBullet.xml")), true, true);
            projectile.AddModule(new LifeTimeModule(1), true, true);

            GameObject pirateRaider = FindGameObject<GameObject>(x => x.Name == "Pirate Raider");
            DebugUtils.AssertNotNull(pirateRaider);

            currentMissileSpawnCounter++;

            // See if we have reached the number of spawn calls to fire missiles
            if (currentMissileSpawnCounter >= missileSpawnCounter)
            {
                Projectile missile = AddGameObject(new Missile(pirateRaider, eagleFrigate.WorldPosition, AssetManager.GetData<ProjectileData>("Cards\\Weapons\\Missile\\VulcanMissileTurret\\VulcanMissileTurretBullet.xml")), true, true);
                missile.AddModule(new LifeTimeModule(3), true, true);

                Beam beam = AddGameObject(new Beam(eagleFrigate, pirateRaider.WorldPosition, AssetManager.GetData<ProjectileData>("Cards\\Weapons\\Beam\\LaserBeamTurret\\LaserBeamTurretBullet.xml")), true, true);
                beam.AddModule(new LifeTimeModule(3), true, true);

                currentMissileSpawnCounter = 0;
            }
        }

        #endregion

        #region Event callbacks for main menu screen buttons

        /// <summary>
        /// The callback to execute when we press the 'Play' button
        /// </summary>
        /// <param name="baseObject">The baseObject that was clicked</param>
        private void OnNewGameButtonLeftClicked(BaseObject baseObject)
        {
            // Need to load assets before we transition to the next screen
            PlayerDataRegistry.Instance.LoadAssets(PlayerDataRegistry.startingDataRegistryDataAsset);

            // Reset the player's current level to 1
            PlayerDataRegistry.Instance.PlayerData.CurrentLevel = 1;
            Transition(new LobbyMenuScreen());
        }

        /// <summary>
        /// The callback to execute when we press the 'Continue' button
        /// </summary>
        /// <param name="baseObject"></param>
        private void OnContinueButtonLeftClicked(BaseObject baseObject)
        {
            // Need to load assets before we transition to the next screen
            PlayerDataRegistry.Instance.LoadAssets(PlayerDataRegistry.playerDataRegistryDataAsset);
            Transition(new LobbyMenuScreen());
        }

        /// <summary>
        /// The callback to execute when we press the 'Options' button
        /// </summary>
        /// <param name="baseObject">The image that was clicked</param>
        private void OnOptionsButtonClicked(BaseObject baseObject)
        {
            Transition(new GameOptionsScreen());
        }

#if DEBUG

        /// <summary>
        /// The callback to execute where we transition to the hardpoint screen
        /// </summary>
        /// <param name="baseObject"></param>
        private void OnHardpointButtonClicked(BaseObject baseObject)
        {
            Transition(new HardpointScreen());
        }

#endif

        /// <summary>
        /// The callback to execute when we press the 'Exit' button
        /// </summary>
        /// <param name="baseObject">Unused</param>
        private void OnExitGameButtonClicked(BaseObject baseObject)
        {
            ScreenManager.Instance.EndGame();
        }

        #endregion
    }
}