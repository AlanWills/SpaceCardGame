using _2DEngine;
using System.Collections.Generic;

namespace SpaceCardGame
{
    public class TerraMission : BattleScreen
    {
        public TerraMission(Deck playerChosenDeck, Deck opponentChosenDeck) :
            base(playerChosenDeck, opponentChosenDeck, "Screens\\BattleScreen.xml")
        {
        }

        #region Virtual Functions

        /// <summary>
        /// Set up our dialog box commands
        /// </summary>
        protected override void AddInitialCommands()
        {
            base.AddInitialCommands();

            string terranCommanderPortrait = "Portraits\\TerranCommander";
            string pirateCommanderPortrait = "Portraits\\PirateCommander";

            List<string> dialog = new List<string>()
            {
                "This is the commander of the Terran fleet",
                "If you continue to not respond to our transmissions you will be engaged"
            };

            Command previousCommand = AddCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, dialog));

            dialog = new List<string>()
            {
                "I am J'Taahn",
                "Surrender and your lives will be spared",
                "Resist us and you will be destroyed"
            };

            previousCommand = AddCommand(new CharacterDialogBoxCommand(pirateCommanderPortrait, dialog), previousCommand);

            dialog = new List<string>()
            {
                "Pirates..."
            };

            previousCommand = AddCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, dialog), previousCommand);

            dialog = new List<string>()
            {
                "We are not pirates, we are now warlords of this sector of space",
                "Give up your planet and we will show mercy"
            };

            previousCommand = AddCommand(new CharacterDialogBoxCommand(pirateCommanderPortrait, dialog), previousCommand);

            dialog = new List<string>()
            {
                "We will not be bullied by pirates scum",
                "Ready the fleet, we will show these invaders our full strength"
            };

            previousCommand = AddCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, dialog), previousCommand);
        }

        #endregion
    }
}
