using _2DEngine;
using System.Collections.Generic;

namespace SpaceCardGame
{
    public class AntheaMission : MissionScreen
    {
        // Need to change the starter pack
        // For this mission, have the shipyard as a card that is there in addition to our station
        // We have to keep it alive
        // When done, we get the shipyard card

        public AntheaMission(Deck playerChosenDeck, Deck opponentChosenDeck) :
            base(playerChosenDeck, opponentChosenDeck, "Screens\\Missions\\Mission2 - Anthea.xml")
        {
        }

        #region Virtual Functions

        protected override void AddDialogStrings()
        {
            base.AddDialogStrings();

            // Pre fight dialogue

            Dialog.Enqueue(new List<string>()
            {
                "We have arrived at the Anthea colony",
                "Computer, locate the shipyard"
            });

            Dialog.Enqueue(new List<string>()
            {
                "I have located the shipyard",
                "However, we are not alone",
                "Sensors indicate many small scavenger craft moving towards the shipyard",
                "I am plotting a course which should get us to the shipyard before them"
            });

            Dialog.Enqueue(new List<string>()
            {
                "Unidentified craft, this is the commander of the 1st Terran Fleet",
                "You are trespassing in our space",
                "Power down and allow us to escort you from this system"
            });

            Dialog.Enqueue(new List<string>()
            {
                "Fat chance Terran",
                "This plunder will make us rich as kings, then we won't have to take orders from the likes of you",
                "Come on boys, lets turn 'em into scrap"
            });

            Dialog.Enqueue(new List<string>()
            {
                "Commander, it appears their actions are hostile"
            });

            Dialog.Enqueue(new List<string>()
            {
                "Of course their actions are hostile, they're firing at us!"
            });

            Dialog.Enqueue(new List<string>()
            {
                "What are you orders sir?"
            });

            Dialog.Enqueue(new List<string>()
            {
                "FIRE BACK!"
            });

            // Post fight dialogue

            Dialog.Enqueue(new List<string>()
            {
                "Our fleet has broken the scavenger force sir and repair crews are on their way to the shipyard",
                "There is only minor damage so construction can begin shortly"
            });
        }

        /// <summary>
        /// Set up our dialog box commands
        /// </summary>
        protected override void AddInitialCommands()
        {
            base.AddInitialCommands();

            string computerAIPortrait = "Portraits\\ComputerAI";
            string terranCommanderPortrait = "Portraits\\TerranCommander";
            string scavengerCommanderPortrait = "Portraits\\ScavengerCommander";

            AddCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, Dialog.Dequeue())).
                NextCommand(new WaitCommand(1)).
                NextCommand(new CharacterDialogBoxCommand(computerAIPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(scavengerCommanderPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(computerAIPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(computerAIPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, Dialog.Dequeue())).
                NextCommand(new NewGameCommand());
        }

        protected override void OnOpponentDefeated()
        {
            string computerAIPortrait = "Portraits\\ComputerAI";

            AddCommand(new CharacterDialogBoxCommand(computerAIPortrait, Dialog.Dequeue()));

            base.OnOpponentDefeated();
        }

        #endregion
    }
}
