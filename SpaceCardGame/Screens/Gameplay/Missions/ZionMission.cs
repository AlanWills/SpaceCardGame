using _2DEngine;
using System.Collections.Generic;

namespace SpaceCardGame
{
    public class ZionMission : MissionScreen
    {
        // Get the cards for all enemy ships left over at the end of this mission

        public ZionMission(Deck playerChosenDeck, Deck opponentChosenDeck) :
            base(playerChosenDeck, opponentChosenDeck, "Screens\\Missions\\Mission7 - Zion.xml")
        {
        }

        #region Virtual Functions

        protected override void AddDialogStrings()
        {
            base.AddDialogStrings();

            Dialog.Enqueue(new List<string>()
            {
                "The fleet has arrived at the coordinates we obtained from the Sentinel",
                "It appears there is a fleet of Sanak'Ah ships similar in power to the Sentinel"
            });

            Dialog.Enqueue(new List<string>()
            {
                "It also appears that they are not inactive!",
            });

            Dialog.Enqueue(new List<string>()
            {
                "The ships have responded to the carrier wave of the Sentinel",
                "I can override their systems, but it will take time",
                "You will have to weather their attack whilst I integrate with them",
                "Commander,",
                "Destroy as few ships as you can - we will need all we can get against the K'Than"
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

            AddCommand(new CharacterDialogBoxCommand(computerAIPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(computerAIPortrait, Dialog.Dequeue()));
        }

        #endregion
    }
}
