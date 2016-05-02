using _2DEngine;
using CardGameEngine;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A class to handle the game objects in the game board for one player
    /// </summary>
    public class GameBoardSection : GameObject
    {
        #region Properties and Fields

        /// <summary>
        /// A container to group our ships together and automatically space them.
        /// </summary>
        public ShipCardControl ShipCardControl { get; private set; }

        /// <summary>
        /// A reference to the human player
        /// </summary>
        private GamePlayer Player { get; set; }

        /// <summary>
        /// A reference to the battle screen for convenience
        /// </summary>
        private BattleScreen BattleScreen { get; set; }

        #endregion

        public GameBoardSection(GamePlayer player, Vector2 localPosition, string dataAsset = AssetManager.EmptyGameObjectDataAsset) :
            base(localPosition, dataAsset)
        {
            Player = player;
            UsesCollider = false;
            Size = new Vector2(ScreenManager.Instance.ScreenDimensions.X, ScreenManager.Instance.ScreenDimensions.Y * 0.5f);

            Vector2 shipCardControlPosition = new Vector2(0, -Size.Y * 0.25f);
            Vector2 shipCardControlSize = new Vector2(Size.X * 0.8f, Size.Y * 0.5f);

            GridControl cardOutlineGridControl = AddChild(new GridControl(1, GamePlayer.MaxShipNumber, shipCardControlSize, shipCardControlPosition));
            for (int i = 0; i < GamePlayer.MaxShipNumber; ++i)
            {
                cardOutlineGridControl.AddChild(new CardOutline(Vector2.Zero));
            }

            ShipCardControl = AddChild(new ShipCardControl(shipCardControlSize, shipCardControlPosition));
        }

        #region Virtual Functions

        /// <summary>
        /// Set up the reference to the battle screen
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            Debug.Assert(ScreenManager.Instance.CurrentScreen is BattleScreen);
            BattleScreen = ScreenManager.Instance.CurrentScreen as BattleScreen;

            ShipCardData stationData = Player.GetStationData();
            AddCard(stationData, Vector2.Zero, Vector2.Zero, false, false);
        }

        #endregion

        #region Specific Function for adding cards

        /// <summary>
        /// Adds our card to the section, but calls functions on the CardObjectPair to perform extra setup.
        /// Also charges the resources to the player.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObjectToAdd"></param>
        /// <param name="load"></param>
        /// <param name="initialise"></param>
        /// <returns></returns>
        public void AddCard(GameCardData cardData, Vector2 size, Vector2 desiredWorldPosition, bool load = true, bool initialise = true)
        {
            // The size parameter comes from the card thumbnail
            // We pass it in to keep the sizes of things consistent
            // Could possibly remove this later, but for now it does the trick
            CardObjectPair pair = AddChild(cardData.CreateCardObjectPair(), load, initialise);
            pair.LocalPosition = desiredWorldPosition - WorldPosition;
            pair.Card.Size = size;

            // Deduct the resources
            bool charge = true;
            Player.AlterResources(cardData, charge);

            pair.WhenAddedToGameBoard(this);
            pair.Card.OnLay();
        }

        #endregion
    }
}