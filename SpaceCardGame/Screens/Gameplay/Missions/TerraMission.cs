using SpaceCardGameData;
using System.Collections.Generic;

namespace SpaceCardGame
{
    public class TerraMission : MissionScreen
    {
        public TerraMission(Deck playerChosenDeck, Deck opponentChosenDeck) :
            base(playerChosenDeck, opponentChosenDeck, "Screens\\Missions\\Mission1 - Terra.xml")
        {
        }

        #region Virtual Functions

        protected override void AddDialogStrings()
        {
            base.AddDialogStrings();

            Dialog.Enqueue(new List<string>()
            {
                "This is the commander of the Terran fleet",
                "If you continue to not respond to our transmissions you will be engaged"
            });

            Dialog.Enqueue(new List<string>()
            {
                "I am J'Taahn",
                "Surrender and your lives will be spared",
                "Resist us and you will be destroyed"
            });

            Dialog.Enqueue(new List<string>()
            {
                "Pirates..."
            });

            Dialog.Enqueue(new List<string>()
            {
                "We are not pirates, we are now warlords of this sector of space",
                "Give up your planet and we will show mercy"
            });

            Dialog.Enqueue(new List<string>()
            {
                "We will not be bullied by pirate scum",
                "Ready the fleet, we will show these invaders our full strength"
            });
        }

        /// <summary>
        /// Set up our dialog box commands
        /// </summary>
        protected override void AddInitialCommands()
        {
            base.AddInitialCommands();

            string terranCommanderPortrait = "Portraits\\TerranCommander";
            string pirateCommanderPortrait = "Portraits\\PirateCommander";

            AddCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(pirateCommanderPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(pirateCommanderPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, Dialog.Dequeue()));
        }

        #endregion
    }
}
