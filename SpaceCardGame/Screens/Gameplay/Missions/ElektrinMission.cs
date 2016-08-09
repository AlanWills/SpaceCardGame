using _2DEngine;
using System.Collections.Generic;

namespace SpaceCardGame
{
    public class ElektrinMission : MissionScreen
    {
        public ElektrinMission(Deck playerChosenDeck, Deck opponentChosenDeck) :
            base(playerChosenDeck, opponentChosenDeck, "Screens\\Missions\\Mission4 - Elektrin.xml")
        {
        }

        #region Virtual Functions

        protected override void AddDialogStrings()
        {
            base.AddDialogStrings();

            // Pre fight dialogue

            Dialog.Enqueue(new List<string>()
            {
                "Commander, we have arrived at the supposed coordinates of the El'Ek homeworld",
                "There is however, nothing here"
            });

            Dialog.Enqueue(new List<string>()
            {
                "Blast!",
                "What are we supposed to do know?"
            });

            Dialog.Enqueue(new List<string>()
            {
                "May I suggest that we leave immediately",
                "Our enemies move on us - we cannot afford to waste time"
            });

            Dialog.Enqueue(new List<string>()
            {
                "Wait",
                "Commander, multiple enemy ships de-cloaking",
            });

            Dialog.Enqueue(new List<string>()
            {
                "Alien",
                "Why do you tread in a place that does not belong to you"
            });

            Dialog.Enqueue(new List<string>()
            {
                "We seek your assistance",
                "A mighty race threatens us all - we ask you help us defeat them"
            });

            Dialog.Enqueue(new List<string>()
            {
                "Are you worth saving?",
                "We shall test you to measure your worth to the universe",
                "If you survive, you have your help"
            });

            // Post fight dialogue

            Dialog.Enqueue(new List<string>()
            {
                "Your technology is basic, but your race shows great promise Terran",
                "We will add our strength to yours to fight that which threatens us all"
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

            AddCommand(new CharacterDialogBoxCommand(computerAIPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(computerAIPortrait, Dialog.Dequeue())).
                NextCommand(new WaitCommand(1)).
                NextCommand(new CharacterDialogBoxCommand(computerAIPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(elekCommanderPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(elekCommanderPortrait, Dialog.Dequeue()));
        }

        protected override void OnOpponentDefeated()
        {
            string elekCommanderPortrait = "Portraits\\El'EkCommander";

            AddCommand(new CharacterDialogBoxCommand(elekCommanderPortrait, Dialog.Dequeue()));

            base.OnOpponentDefeated();
        }

        #endregion
    }
}
