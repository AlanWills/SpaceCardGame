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
        public ShipCardControl ShipCardControl { get; private set; }

        /// <summary>
        /// A reference to the human player
        /// </summary>
        private Player Player { get; set; }

        /// <summary>
        /// A reference to the battle screen for convenience
        /// </summary>
        private BattleScreen BattleScreen { get; set; }

        /// <summary>
        /// An array of references to our card outlines we use in the card placement phase to show where we can place ships.
        /// </summary>
        private CardOutline[] CardOutlines { get; set; }

        #endregion

        public GameBoardSection(Player player, Vector2 localPosition, string dataAsset = AssetManager.EmptyGameObjectDataAsset) :
            base(localPosition, dataAsset)
        {
            Player = player;
            UsesCollider = false;
            Size = new Vector2(ScreenManager.Instance.ScreenDimensions.X, ScreenManager.Instance.ScreenDimensions.Y * 0.5f);

            Vector2 shipCardControlPosition = new Vector2(0, -Size.Y * 0.25f);
            Vector2 shipCardControlSize = new Vector2(Size.X * 0.8f, Size.Y * 0.5f);
            
            ShipCardControl = AddChild(new ShipCardControl(shipCardControlSize, shipCardControlPosition));
        }

        #region Virtual Functions

        /// <summary>
        /// Set up the reference to the battle screen and add the card outlines for the ships.
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            Debug.Assert(ScreenManager.Instance.CurrentScreen is BattleScreen);
            BattleScreen = ScreenManager.Instance.CurrentScreen as BattleScreen;

            ShipCard stationCard = Player.GetStationData();
            CardObjectPair stationPair = AddCard(stationCard, WorldPosition, false, false);

            CardOutlines = new CardOutline[ShipCardControl.LocalXPositions.Length];
            for (int i = 0; i < ShipCardControl.LocalXPositions.Length; ++i)
            {
                CardOutlines[i] = AddChild(new CardOutline(stationPair.Card.Size, ShipCardControl.LocalPosition + new Vector2(ShipCardControl.LocalXPositions[i], 0)), true, true);
                CardOutlines[i].Valid = false;
            }

            BattleScreen.OnCardPlacementStateStarted += ToggleCardOutlines;
            BattleScreen.OnBattleStateStarted += ToggleCardOutlines;
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
        public CardObjectPair AddCard(Card card, Vector2 desiredWorldPosition, bool load = true, bool initialise = true)
        {
            // The size parameter comes from the card thumbnail
            // We pass it in to keep the sizes of things consistent
            // Could possibly remove this later, but for now it does the trick
            CardObjectPair pair = AddChild(card.CreateCardObjectPair(), load, initialise);

            pair.LocalPosition = desiredWorldPosition - WorldPosition;

            // Deduct the resources
            Player.AlterResources(card, ChargeType.kCharge);

            pair.WhenAddedToGameBoard(this);
            pair.Card.OnLay();

            Player.CurrentHand.Remove(card);
            BattleScreen.Board.ActivePlayerBoardSection.UIBoardSection.HandUI.NeedsRebuild = true;
            
            return pair;
        }

        #endregion

        #region Callbacks

        /// <summary>
        /// Hides or shows our card outlines based on the current phase of the turn we are in
        /// </summary>
        private void ToggleCardOutlines()
        {
            bool show = BattleScreen.TurnState == TurnState.kPlaceCards;
            foreach (CardOutline cardOutline in CardOutlines)
            {
                if (show)
                {
                    cardOutline.Show();
                }
                else
                {
                    cardOutline.Hide();
                }
            }
        }

        #endregion
    }
}