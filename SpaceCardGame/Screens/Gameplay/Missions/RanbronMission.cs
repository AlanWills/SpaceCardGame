using _2DEngine;
using System.Collections.Generic;

namespace SpaceCardGame
{
    public class RanbronMission : MissionScreen
    {
        // Get the sentinel card after this mission

        public RanbronMission(Deck playerChosenDeck, Deck opponentChosenDeck) :
            base(playerChosenDeck, opponentChosenDeck, "Screens\\Missions\\Mission6 - Ranbron.xml")
        {
        }

        #region Virtual Functions

        protected override void AddDialogStrings()
        {
            base.AddDialogStrings();

            // Pre fight dialogue

            Dialog.Enqueue(new List<string>()
            {
                "Our coordinates match no known Terran space charts",
                "Cross referencing the El'Ek database"
            });

            Dialog.Enqueue(new List<string>()
            {
                "This is ancient space",
                "No ship has passed through here for millenia",
            });

            Dialog.Enqueue(new List<string>()
            {
                "No matches found"
            });

            Dialog.Enqueue(new List<string>()
            {
                "We have a bigger problem",
                "There is a ship on long range scanners",
                "At least Dreadnought class"
            });

            Dialog.Enqueue(new List<string>()
            {
                "Unidentified ship, we have been stranded here by mistake",
                "Allow us to recharge our engines and we will leave immediately"
            });

            Dialog.Enqueue(new List<string>()
            {
                "...",
            });

            Dialog.Enqueue(new List<string>()
            {
                "I repeat, unidentified ship, we have been stranded here by mistake",
                "Please respond"
            });

            Dialog.Enqueue(new List<string>()
            {
                "...",
            });

            Dialog.Enqueue(new List<string>()
            {
                "Weapons locked"
            });

            Dialog.Enqueue(new List<string>()
            {
                "I was afraid of that happening..."
            });

            // Post fight dialogue

            Dialog.Enqueue(new List<string>()
            {
                "This ship is more ancient than our civilization"
            });

            Dialog.Enqueue(new List<string>()
            {
                "Commander, I have been able to translate some of the ship's data core",
                "It refers to itself as 'The Sentinel'",
                "It could take me years to process all the information it has stored, but already I am seeing the same coordinates mentioned time and again",
                "The Sentinel calls it 'Akkar'",
                "Home"
            });

            Dialog.Enqueue(new List<string>()
            {
                "Plot a course"
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
            string elekCommanderPortrait = "Portraits\\El'EkCommander";
            string sentinelPortrait = "Portraits\\Sentinel";

            AddCommand(new CharacterDialogBoxCommand(computerAIPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(elekCommanderPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(computerAIPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, Dialog.Dequeue())).
                NextCommand(new WaitCommand(0.5f)).
                NextCommand(new CharacterDialogBoxCommand(sentinelPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, Dialog.Dequeue())).
                NextCommand(new WaitCommand(0.5f)).
                NextCommand(new CharacterDialogBoxCommand(sentinelPortrait, Dialog.Dequeue())).
                NextCommand(new WaitCommand(0.5f)).
                NextCommand(new CharacterDialogBoxCommand(sentinelPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, Dialog.Dequeue()));
        }

        protected override void OnOpponentDefeated()
        {
            string computerAIPortrait = "Portraits\\ComputerAI";
            string terranCommanderPortrait = "Portraits\\TerranCommander";
            string elekCommanderPortrait = "Portraits\\El'EkCommander";

            AddCommand(new CharacterDialogBoxCommand(elekCommanderPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(computerAIPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, Dialog.Dequeue()));

            base.OnOpponentDefeated();
        }

        #endregion
    }
}
