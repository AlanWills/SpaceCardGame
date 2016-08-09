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

            // Pre fight dialogue

            Dialog.Enqueue(new List<string>()
            {
                "Fleet, this is the Commander",
                "We shall shortly be completing our trans-space jump and will appear on the edge of the pirate's sensors",
                "We will have the element of surprise",
                "Let us use it to eradicate the scourge that threatened our home"
            });

            // Post fight dialogue

            Dialog.Enqueue(new List<string>()
            {
                "The ships we fought...",
                "How could these mere pirates obtain such formidable technology?"
            });

            Dialog.Enqueue(new List<string>()
            {
                "Sir, these ships match no known database designs and are powered by Octon reactors - a fuel that is unobtainable in this quadrant",
                "I would suspect that they were bought or exchanged them"
            });

            Dialog.Enqueue(new List<string>()
            {
                "That is a disturbing thought"
            });

            Dialog.Enqueue(new List<string>()
            {
                "I would like to run a full scan of any records that can be retrieved from the scuttled ships",
            });

            Dialog.Enqueue(new List<string>()
            {
                "Agreed, but make it fast"
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

        protected override void OnOpponentDefeated()
        {
            string terranCommanderPortrait = "Portraits\\TerranCommander";
            string computerAIPortrait = "Portraits\\ComputerAI";

            AddCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(computerAIPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(computerAIPortrait, Dialog.Dequeue())).
                NextCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, Dialog.Dequeue()));

            base.OnOpponentDefeated();
        }

        #endregion
    }
}
