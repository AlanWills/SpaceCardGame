using _2DEngine;
using System.Collections.Generic;

namespace SpaceCardGame
{
    public class FreizaMission : MissionScreen
    {
        public FreizaMission(Deck playerChosenDeck, Deck opponentChosenDeck) :
            base(playerChosenDeck, opponentChosenDeck, "Screens\\Missions\\Mission5 - Freiza.xml")
        {
        }

        #region Virtual Functions

        protected override void AddDialogStrings()
        {
            base.AddDialogStrings();

            Dialog.Enqueue(new List<string>()
            {
                "We have intercepted K'Than transmissions using our sensor relays",
                "Their attack on Freiza has begun",
                "It cannot be allowed to continue",
            });

            Dialog.Enqueue(new List<string>()
            {
                "Using all known data I estimate our probability of victory at 8.13%",
                "We are walking to defeat"
            });

            Dialog.Enqueue(new List<string>()
            {
                "We have no choice - the fewer free planets exist, the fewer allies we have",
                "We must try"
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

            AddCommand(new CharacterDialogBoxCommand(elekCommanderPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(computerAIPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, Dialog.Dequeue()));
        }

        #endregion
    }
}
