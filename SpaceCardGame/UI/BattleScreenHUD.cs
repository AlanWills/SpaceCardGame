using _2DEngine;
using CardGameEngine;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace SpaceCardGame
{
    public class BattleScreenHUD : UIObjectContainer
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to our player
        /// </summary>
        private Player Player { get; set; }

        /// <summary>
        /// A reference to the player's deck UI
        /// </summary>
        private PlayerDeckUI PlayerDeckUI { get; set; }

        /// <summary>
        /// A reference to the player's hand UI
        /// </summary>
        private PlayerHandUI PlayerHandUI { get; set; }

        /// <summary>
        /// A button which will end the current player's turn
        /// </summary>
        private Button EndTurnButton { get; set; }

        #endregion

        public BattleScreenHUD(string hudTextureAsset) :
            base(ScreenManager.Instance.ScreenDimensions, ScreenManager.Instance.ScreenCentre, hudTextureAsset)
        {
            Player = BattleScreen.Player;
        }

        #region Virtual Functions

        /// <summary>
        /// Sets up callbacks for when we draw a card
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            PlayerDeckUI = AddObject(new PlayerDeckUI(Player, new Vector2(ScreenManager.Instance.ScreenDimensions.X * 0.45f, ScreenManager.Instance.ScreenDimensions.Y * 0.4f)));
            PlayerHandUI = AddObject(new PlayerHandUI(Player, new Vector2(0, Size.Y * 0.4f)));

            EndTurnButton = AddObject(new Button("End Turn", Vector2.Zero));
            EndTurnButton.OnLeftClicked += OnEndTurnButtonClicked;

            base.LoadContent();
        }

        /// <summary>
        /// Fixup some UI
        /// </summary>
        public override void Initialise()
        {
            CheckShouldInitialise();

            base.Initialise();

            EndTurnButton.LocalPosition += new Vector2((Size.X - EndTurnButton.Size.X) * 0.5f - 10, 0);
        }

        /// <summary>
        /// Do some size fixup - when we resize cards this may not be necessary
        /// </summary>
        public override void Begin()
        {
            // Do this before we call base.Begin, because we need the new size before we call the begin function in there
            PlayerDeckUI.Size *= 0.5f;

            base.Begin();
        }

        #endregion

        #region Click Callbacks

        /// <summary>
        /// Nothing more than a callback wrapper around the NewPlayerTurn - but this is specifically for callbacks.
        /// Want to keep the original function too, so that we can end the turn after a certain amount of time too.
        /// </summary>
        /// <param name="clickable"></param>
        private void OnEndTurnButtonClicked(IClickable clickable)
        {
            Debug.Assert(ScreenManager.Instance.CurrentScreen is BattleScreen);

            (ScreenManager.Instance.CurrentScreen as BattleScreen).NewPlayerTurn();
        }

        #endregion
    }
}