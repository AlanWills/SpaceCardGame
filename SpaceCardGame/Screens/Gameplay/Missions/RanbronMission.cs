using _2DEngine;
using System.Collections.Generic;

namespace SpaceCardGame.Screens.Gameplay.Missions
{
    public class RanbronMission : BattleScreen
    {
        // Get the sentinel card after this mission

        public RanbronMission(Deck playerChosenDeck, Deck opponentChosenDeck) :
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

            string computerAIPortrait = "Portraits\\ComputerAI";
            string terranCommanderPortrait = "Portraits\\TerranCommander";
            string elekCommanderPortrait = "Portraits\\El'EkCommander";
            string sentinelPortrait = "Portraits\\Sentinel";

            List<string> dialog = new List<string>()
            {
                "Our coordinates match no known Terran space charts",
                "Cross referencing the El'Ek database"
            };

            Command previousCommand = AddCommand(new CharacterDialogBoxCommand(computerAIPortrait, dialog));

            dialog = new List<string>()
            {
                "This is ancient space",
                "No ship has passed through here for millenia",
            };

            previousCommand = AddCommand(new CharacterDialogBoxCommand(elekCommanderPortrait, dialog));

            dialog = new List<string>()
            {
                "No matches found"
            };

            previousCommand = AddCommand(new CharacterDialogBoxCommand(computerAIPortrait, dialog));

            dialog = new List<string>()
            {
                "We have a bigger problem",
                "There is a ship on long range scanners",
                "At least Dreadnought class"
            };

            previousCommand = AddCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, dialog));

            dialog = new List<string>()
            {
                "Unidentified ship, we have been stranded here by mistake",
                "Allow us to recharge our engines and we will leave immediately"
            };

            previousCommand = AddCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, dialog));

            previousCommand = AddCommand(new WaitCommand(0.5f));

            dialog = new List<string>()
            {
                "...",
            };

            previousCommand = AddCommand(new CharacterDialogBoxCommand(sentinelPortrait, dialog));

            dialog = new List<string>()
            {
                "I repeat, unidentified ship, we have been stranded here by mistake",
                "Please respond"
            };

            previousCommand = AddCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, dialog));

            previousCommand = AddCommand(new WaitCommand(0.5f));

            dialog = new List<string>()
            {
                "...",
            };

            previousCommand = AddCommand(new CharacterDialogBoxCommand(sentinelPortrait, dialog));

            previousCommand = AddCommand(new WaitCommand(0.5f));

            dialog = new List<string>()
            {
                "Weapons locked"
            };

            previousCommand = AddCommand(new CharacterDialogBoxCommand(sentinelPortrait, dialog));

            dialog = new List<string>()
            {
                "I was afraid of that happening..."
            };

            previousCommand = AddCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, dialog));
        }

        #endregion
    }
}
