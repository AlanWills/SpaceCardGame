using _2DEngine;
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
        public GameCardControl ShipCardControl { get; private set; }

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
            Size = new Vector2(ScreenManager.Instance.ScreenDimensions.X, ScreenManager.Instance.ScreenDimensions.Y * 0.5f);

            ShipCardControl = AddChild(new GameCardControl(typeof(ShipCardData), new Vector2(Size.X * 0.8f, Size.Y * 0.5f), GamePlayer.MaxShipNumber, 1, new Vector2(0, - Size.Y * 0.25f)));
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
            AddCard(stationData, new Vector2(300, 100), false, false);
        }

        #endregion

        #region Specific Function for adding cards

        /// <summary>
        /// Adds our card to the section, but calls functions on the CardObjectPair and Card to perform extra type functionality
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObjectToAdd"></param>
        /// <param name="load"></param>
        /// <param name="initialise"></param>
        /// <returns></returns>
        public void AddCard(GameCardData cardData, Vector2 size, bool load = true, bool initialise = true)
        {
            // The size parameter comes from the card thumbnail
            // We pass it in to keep the sizes of things consistent
            // Could possibly remove this later, but for now it does the trick
            CardObjectPair pair = AddChild(cardData.CreateCardObjectPair(), load, initialise);
            pair.LocalPosition = GameMouse.Instance.InGamePosition - WorldPosition;
            pair.Card.Size = size;

            pair.WhenAddedToGameBoard(this, Player);

            DebugUtils.AssertNotNull(pair);
            DebugUtils.AssertNotNull(pair.Card);
            DebugUtils.AssertNotNull(pair.CardObject);

            pair.Card.OnLay(BattleScreen.Board, Player);
        }

        #endregion
    }
}