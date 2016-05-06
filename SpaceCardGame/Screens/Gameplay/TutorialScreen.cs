using _2DEngine;
using CardGameEngine;

namespace SpaceCardGame
{
    /// <summary>
    /// A battle screen which is specifically designed as a tutorial.
    /// Implements commands and UI to teach the player about the game.
    /// </summary>
    public class TutorialScreen : BattleScreen
    {
        public TutorialScreen(Deck playerChosenDeck, string screenDataAsset = "Screens\\BattleScreen.xml") :
            base(playerChosenDeck, screenDataAsset)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// Add our dialog boxes for this tutorial
        /// </summary>
        protected override void AddInitialCommands()
        {
            base.AddInitialCommands();

            NewGameCommand newGameCommand = CommandManager.Instance.FindChild<NewGameCommand>();
            DebugUtils.AssertNotNull(newGameCommand);

            newGameCommand.PreviousCommand = AddCommand(new TextDialogBoxCommand("", true));
        }

        #endregion
    }
}
