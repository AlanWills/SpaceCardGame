using _2DEngine;
using Microsoft.Xna.Framework;
using System;

namespace SpaceCardGame
{
    /// <summary>
    /// A menu with various options after the player has been defeated, including replaying the current mission and quitting to the lobby screen.
    /// </summary>
    public class PlayerDefeatedUI : ListControl
    {
        public PlayerDefeatedUI() :
            base(new Vector2(300, 280), ScreenManager.Instance.ScreenCentre)
        {
            UsesCollider = false;
        }

        #region Virtual Functions

        /// <summary>
        /// Populate our menu with the various options that the player is able to do now that they have lost
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            Label title = AddChild(new Label("Defeat", Anchor.kTopCentre, -1));
            title.Colour.Value = Color.White;

            Button replayMissionButton = AddChild(new Button("Replay Mission", Vector2.Zero, AssetManager.DefaultNarrowButtonTextureAsset, AssetManager.DefaultNarrowButtonHighlightedTextureAsset));
            replayMissionButton.ClickableModule.OnLeftClicked += ReplayMission;

            Button backToLobbyScreenButton = AddChild(new Button("Back to Lobby", Vector2.Zero, AssetManager.DefaultNarrowButtonTextureAsset, AssetManager.DefaultNarrowButtonHighlightedTextureAsset));
            backToLobbyScreenButton.ClickableModule.OnLeftClicked += TransitionToLobbyScreen;

            base.LoadContent();
        }

        #endregion

        #region Callbacks

        /// <summary>
        /// For now, just transitions to a new instance of the current screen we are on to simulate restarting.
        /// Might be more efficient to restart the current screen somehow, but this is much easier.
        /// </summary>
        /// <param name="clickedObject"></param>
        private void ReplayMission(BaseObject clickedObject)
        {
            BattleScreen battleScreen = ScreenManager.Instance.GetCurrentScreenAs<BattleScreen>();
            DebugUtils.AssertNotNull(battleScreen);

            BattleScreen newScreen = (BattleScreen)Activator.CreateInstance(battleScreen.GetType(), BattleScreen.Player.DeckInstance.Deck, BattleScreen.Opponent.DeckInstance.Deck);
            DebugUtils.AssertNotNull(newScreen);

            battleScreen.Transition(newScreen);
        }

        private void TransitionToLobbyScreen(BaseObject clickedObject)
        {
            ScreenManager.Instance.CurrentScreen.Transition(new LobbyMenuScreen());
        }

        #endregion
    }
}
