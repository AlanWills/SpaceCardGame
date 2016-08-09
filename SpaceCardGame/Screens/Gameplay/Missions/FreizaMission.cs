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

            // Pre fight dialogue

            Dialog.Enqueue(new List<string>()
            {
                "We have intercepted K'Than transmissions using long range sensor relays",
                "Their attack on Freiza has begun",
                "It cannot be allowed to continue",
            });

            Dialog.Enqueue(new List<string>()
            {
                "Using all known telemitry data I estimate our probability of victory to be 8.13%, even with the El'Ek flagships",
                "We are walking to defeat"
            });

            Dialog.Enqueue(new List<string>()
            {
                "We have no choice - the fewer free planets exist, the fewer allies we have",
                "We must try"
            });

            // Post fight dialogue

            Dialog.Enqueue(new List<string>()
            {
                "Victory against the K'Than force is imminent sir"
            });

            Dialog.Enqueue(new List<string>()
            {
                "This doesn't seem right",
                "The sensor reports indicated a much larger force than the one we fought"
            });

            Dialog.Enqueue(new List<string>()
            {
                "Commander, long range sensors are showing movement on the dark side of the planet"
            });

            Dialog.Enqueue(new List<string>()
            {
                "It's the K'Than",
                "We were being delayed and nothing more"
            });

            Dialog.Enqueue(new List<string>()
            {
                "A firestorm is engulfing Freiza and a sizeable K'Than force is on an intercept course"
            });

            Dialog.Enqueue(new List<string>()
            {
                "We MUST retreat",
                "Our fleet has suffered heavy losses and we cannot fight that armada"
            });

            Dialog.Enqueue(new List<string>()
            {
                "Jump trajectory will take time to coordinate"
            });

            Dialog.Enqueue(new List<string>()
            {
                "There is no time!",
                "Just jump"
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

        protected override void OnOpponentDefeated()
        {
            string computerAIPortrait = "Portraits\\ComputerAI";
            string terranCommanderPortrait = "Portraits\\TerranCommander";
            string elekCommanderPortrait = "Portraits\\El'EkCommander";

            AddCommand(new CharacterDialogBoxCommand(computerAIPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(computerAIPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(computerAIPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(elekCommanderPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(computerAIPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, Dialog.Dequeue()));

            base.OnOpponentDefeated();
        }

        #endregion
    }
}
