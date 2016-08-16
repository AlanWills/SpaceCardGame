using _2DEngine;
using Microsoft.Xna.Framework;
using SpaceCardGameData;
using System;
using System.Collections.Generic;

namespace SpaceCardGame
{
    /// <summary>
    /// A class representing the screen after our MainMenuScreen, where players can edit their decks and play games
    /// </summary>
    public class LobbyMenuScreen : MenuScreen
    {
        #region Properties and Fields

        private const float shipSpawnTimer = 22;
        private float currentShipSpawnTimer = shipSpawnTimer;

        private List<string> ShipsAssets { get; set; }
        private List<GameObject> Ships { get; set; }

        #endregion

        public LobbyMenuScreen(string screenDataAsset = "Screens\\LobbyMenuScreen.xml") :
            base(screenDataAsset)
        {
            AddModule(new SpaceBackgroundModule(this));

            // Create instances of game objects
            ShipsAssets = new List<string>()
            {
                "Cards\\Stations\\AzmodaeusSupercruiser\\AzmodaeusSupercruiserObject.xml",
                "Cards\\Stations\\BastionShipyard\\BastionShipyardObject.xml",
                "Cards\\Stations\\HulkDreadnought\\HulkDreadnoughtObject.xml",
                "Cards\\Stations\\OmegaCruiser\\OmegaCruiserObject.xml",
                "Cards\\Stations\\RaiuT'Ek\\RaiuT'EkObject.xml",
            };

            Ships = new List<GameObject>();
        }

        #region Virtual Functions

        /// <summary>
        /// Adds our buttons for playing or managing decks
        /// </summary>
        protected override void AddInitialUI()
        {
            base.AddInitialUI();

            ListControl buttonGridControl = AddScreenUIObject(new ListControl(new Vector2(ScreenDimensions.X * 0.75f, ScreenDimensions.Y * 0.4f)));
            buttonGridControl.UseScissorRectangle = false;

            Button playGameButton = buttonGridControl.AddChild(new Button("Play", Vector2.Zero));
            playGameButton.ClickableModule.OnLeftClicked += OnPlayGameButtonLeftClicked;

            Button tutorialButton = buttonGridControl.AddChild(new Button("Tutorial", Vector2.Zero));
            tutorialButton.ClickableModule.OnLeftClicked += OnTutorialButtonLeftClicked;

            // Disable the play button if we have no decks to choose from
            if (PlayerDataRegistry.Instance.AvailableDecks == 0)
            {
                playGameButton.Disable();
            }

            Button deckManagerButton = buttonGridControl.AddChild(new Button("Decks", Vector2.Zero));
            deckManagerButton.ClickableModule.OnLeftClicked += OnDeckManagerButtonClicked;

            Button shopButton = buttonGridControl.AddChild(new Button("Shop", Vector2.Zero));
            shopButton.ClickableModule.OnLeftClicked += delegate
            {
                Transition(new ShopScreen());
            };

            Button openPacksButton = buttonGridControl.AddChild(new Button("Open Packs", Vector2.Zero));
            openPacksButton.ClickableModule.OnLeftClicked += OnOpenPacksButtonLeftClicked;

            if (PlayerDataRegistry.Instance.PlayerData.AvailablePacksToOpen <= 0)
            {
                openPacksButton.Disable();
            }
        }

        /// <summary>
        /// Load the ship assets
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            foreach (string shipAsset in ShipsAssets)
            {
                GameObject ship = AddGameObject(new GameObject(Vector2.Zero, shipAsset));
                ship.Hide();

                Ships.Add(ship);
            }

            base.LoadContent();
        }

        /// <summary>
        /// Transition back to our main menu screen
        /// </summary>
        protected override void GoToPreviousScreen()
        {
            Transition(new MainMenuScreen());
        }

        /// <summary>
        /// Add ships every so often from a certain list of ship assets
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            currentShipSpawnTimer += elapsedGameTime;
            if (currentShipSpawnTimer >= shipSpawnTimer)
            {
                AddShip();
                currentShipSpawnTimer = 0;
            }
        }

        #endregion

        #region Animated Ship UI Sugar

        /// <summary>
        /// Adds a ship using a randomly selected asset from the list we made in the constructir
        /// </summary>
        private void AddShip()
        {
            int index = MathUtils.GenerateInt(0, Ships.Count - 1);
            GameObject ship = Ships[index];
            Ships.RemoveAt(index);

            ship.LocalPosition = new Vector2(ScreenDimensions.X * 0.35f, ScreenDimensions.Y + ship.Size.Y * 0.5f);
            ship.AddModule(new MoveToDestinationModule(new Vector2(ScreenDimensions.X * 0.35f, -ship.Size.Y * 0.5f), 100), true, true);
            ship.AddModule(new LifeTimeModule(shipSpawnTimer), true, true);
            ship.Show();
        }

        #endregion

        #region Click Callbacks

        /// <summary>
        /// The callback to execute when we press the 'Play' button
        /// </summary>
        /// <param name="baseObject"></param>
        private void OnPlayGameButtonLeftClicked(BaseObject baseObject)
        {
            Transition(new CampaignMapScreen());
        }

        /// <summary>
        /// The callback to execute when we press the 'Tutorial' button
        /// </summary>
        /// <param name="baseObject"></param>
        private void OnTutorialButtonLeftClicked(BaseObject baseObject)
        {
            PlayerDataRegistryData startingRegistryData = AssetManager.GetData<PlayerDataRegistryData>(PlayerDataRegistry.startingDataRegistryDataAsset);
            Deck tutorialDeck = new Deck();
            tutorialDeck.Create(startingRegistryData.Decks[0].CardDataAssets);

            Transition(new TutorialScreen(tutorialDeck));
        }

        /// <summary>
        /// The callback to execute when we press the 'Decks' button
        /// </summary>
        /// <param name="image"></param>
        private void OnDeckManagerButtonClicked(BaseObject baseObject)
        {
            Transition(new DeckManagerScreen());
        }

        /// <summary>
        /// The callback to execute when we press the 'Open Packs' button
        /// </summary>
        /// <param name=""></param>
        private void OnOpenPacksButtonLeftClicked(BaseObject baseObject)
        {
            Transition(new OpenCardPacksScreen());
        }

        #endregion
    }
}