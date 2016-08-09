using _2DEngine;
using System.Collections.Generic;

namespace SpaceCardGame
{
    public class TelegraMission : MissionScreen
    {
        public TelegraMission(Deck playerChosenDeck, Deck opponentChosenDeck) :
            base(playerChosenDeck, opponentChosenDeck, "Screens\\Missions\\Mission3 - Telegra.xml")
        {
        }

        #region Virtual Functions

        protected override void AddDialogStrings()
        {
            base.AddDialogStrings();

            Dialog.Enqueue(new List<string>()
            {
                "Fleet, this is the Commander",
                "We shall shortly be completing our trans-space jump and will appear on the edge of the pirate's sensors",
                "We will have the element of surprise",
                "Let us use it to eradicate the scourge that threatened our home"
            });
        }

        /// <summary>
        /// Set up our dialog box commands
        /// </summary>
        protected override void AddInitialCommands()
        {
            base.AddInitialCommands();

            string terranCommanderPortrait = "Portraits\\TerranCommander";

            AddCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, Dialog.Dequeue()));
        }

        #endregion
    }
}
