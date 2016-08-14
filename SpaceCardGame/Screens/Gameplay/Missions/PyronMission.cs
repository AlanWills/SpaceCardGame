using _2DEngine;
using System.Collections.Generic;

namespace SpaceCardGame
{
    public class PyronMission : MissionScreen
    {
        public PyronMission(Deck playerChosenDeck, Deck opponentChosenDeck) :
            base(playerChosenDeck, opponentChosenDeck, "Screens\\Missions\\Mission8 - Pyron.xml")
        {
        }

        #region Virtual Functions

        protected override void AddDialogStrings()
        {
            base.AddDialogStrings();

            // Pre fight dialogue

            Dialog.Enqueue(new List<string>()
            {
                "K'Than fleet, this is the Commander of a unified Terran and El'Ek fleet",
                "We are here to stop your reign of terror throughout this system"
            });

            Dialog.Enqueue(new List<string>()
            {
                "Did I not destroy you and your pitiful fleet at Freiza?",
                "No matter, it will take just as little effort to wipe you out again"
            });

            Dialog.Enqueue(new List<string>()
            {
                "This is it fleet",
                "No other objectives",
                "Take out their command ship and this threat will be over",
                "Lets finish this"
            });
        }

        /// <summary>
        /// Set up our dialog box commands
        /// </summary>
        protected override void AddInitialCommands()
        {
            base.AddInitialCommands();

            string terranCommanderPortrait = "Portraits\\TerranCommander";
            string kthanCommanderPortrait = "Portraits\\K'ThanCommander";

            AddCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(kthanCommanderPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, Dialog.Dequeue())).
                NextCommand(new NewGameCommand());
        }

        #endregion
    }
}
