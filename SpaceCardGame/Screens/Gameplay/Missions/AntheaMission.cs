using _2DEngine;
using System.Collections.Generic;

namespace SpaceCardGame
{
    public class AntheaMission : BattleScreen
    {
        // Need to change the starter pack
        // For this mission, have the shipyard as a card that is there in addition to our station
        // We have to keep it alive
        // When done, we get the shipyard card

        public AntheaMission(Deck playerChosenDeck, Deck opponentChosenDeck) :
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
            string scavengerCommanderPortrait = "Portraits\\ScavengerCommander";

            List<string> dialog = new List<string>()
            {
                "We have arrived at the Anthea colony",
                "Computer, locate the shipyard"
            };

            Command previousCommand = AddCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, dialog));

            previousCommand = AddCommand(new WaitCommand(1), previousCommand);

            dialog = new List<string>()
            {
                "I have located the shipyard",
                "However, we are not alone",
                "Sensors indicate many small scavenger craft moving towards the shipyard",
                "I am plotting a course which should get us to the shipyard before them"
            };

            previousCommand = AddCommand(new CharacterDialogBoxCommand(computerAIPortrait, dialog), previousCommand);

            dialog = new List<string>()
            {
                "Unidentified craft, this is the commander of the 1st Terran Fleet",
                "You are trespassing in our space",
                "Power down and allow us to escort you from this system"
            };

            previousCommand = AddCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, dialog), previousCommand);

            dialog = new List<string>()
            {
                "Fat chance Terran",
                "This plunder will make us rich as kings, then we won't have to take orders from the likes of you",
                "Come on boys, lets turn 'em into scrap"
            };

            previousCommand = AddCommand(new CharacterDialogBoxCommand(scavengerCommanderPortrait, dialog), previousCommand);

            dialog = new List<string>()
            {
                "Commander, it appears their actions are hostile"
            };

            previousCommand = AddCommand(new CharacterDialogBoxCommand(computerAIPortrait, dialog), previousCommand);

            dialog = new List<string>()
            {
                "Of course their actions are hostile, they're firing at us!"
            };

            previousCommand = AddCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, dialog), previousCommand);

            dialog = new List<string>()
            {
                "What are you orders sir?"
            };

            previousCommand = AddCommand(new CharacterDialogBoxCommand(computerAIPortrait, dialog), previousCommand);

            dialog = new List<string>()
            {
                "FIRE BACK!"
            };

            previousCommand = AddCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, dialog), previousCommand);
        }

        #endregion
    }
}
