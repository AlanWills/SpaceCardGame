using _2DEngine;
using CardGameEngine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
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

            ShipCardControl = AddChild(new GameCardControl(typeof(ShipCardData), new Vector2(Size.X * 0.8f, Size.Y * 0.5f), GamePlayer.MaxShipNumber, 1, new Vector2(0, - Size.Y * 0.25f), "Backgrounds\\TileableNebula"));
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
        }

        #endregion

        #region Specific Function for adding cards

        /// <summary>
        /// Adds our card to the section, but calls a particular function based on it's type to perform extra stuff like adding a reference to a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObjectToAdd"></param>
        /// <param name="load"></param>
        /// <param name="initialise"></param>
        /// <returns></returns>
        public void AddCard(GameCardData cardData, Vector2 size)
        {
            // The size parameter comes from the card thumbnail
            // We pass it in to keep the sizes of things consistent
            // Could possibly remove this later, but for now it does the trick
            CardObjectPair pair = AddChild(cardData.CreateCardObjectPair(), true, true);
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