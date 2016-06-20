using _2DEngine;
using System.Collections.Generic;

namespace SpaceCardGame
{
    public class ElektrinMission : BattleScreen
    {
        public ElektrinMission(Deck playerChosenDeck, Deck opponentChosenDeck) :
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

            List<string> dialog = new List<string>()
            {
                "Commander, we have arrived at the supposed coordinates of the El'Ek homeworld",
                "There is however, nothing here"
            };

            Command previousCommand = AddCommand(new CharacterDialogBoxCommand(computerAIPortrait, dialog));

            dialog = new List<string>()
            {
                "Blast!",
                "What are we supposed to do know?"
            };

            previousCommand = AddCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, dialog));

            dialog = new List<string>()
            {
                "May I suggest that we leave immediately",
                "Our enemies move on us - we cannot afford to waste time"
            };

            previousCommand = AddCommand(new CharacterDialogBoxCommand(computerAIPortrait, dialog));

            previousCommand = AddCommand(new WaitCommand(1));

            dialog = new List<string>()
            {
                "Wait",
                "Commander, multiple enemy ships de-cloaking",
            };

            previousCommand = AddCommand(new CharacterDialogBoxCommand(computerAIPortrait, dialog));

            dialog = new List<string>()
            {
                "Alien",
                "Why do you tread in a place that does not belong to you"
            };

            previousCommand = AddCommand(new CharacterDialogBoxCommand(elekCommanderPortrait, dialog));

            dialog = new List<string>()
            {
                "We seek your assistance",
                "A mighty race threatens us all - we ask you help us defeat them"
            };

            previousCommand = AddCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, dialog));

            dialog = new List<string>()
            {
                "Are you worth saving?",
                "We shall test you to measure your worth to the universe",
                "If you survive, you have your help"
            };

            previousCommand = AddCommand(new CharacterDialogBoxCommand(elekCommanderPortrait, dialog));
        }

        #endregion
    }
}
