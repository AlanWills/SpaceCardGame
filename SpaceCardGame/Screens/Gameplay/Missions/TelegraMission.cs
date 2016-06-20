using _2DEngine;
using System.Collections.Generic;

namespace SpaceCardGame
{
    public class TelegraMission : BattleScreen
    {
        public TelegraMission(Deck playerChosenDeck, Deck opponentChosenDeck) :
            base(playerChosenDeck, opponentChosenDeck, "Screens\\BattleScreen.xml")
        {
        }

        #region Virtual Functions

        /// <summary>
        /// Set up our dialog box commands
        /// </summary>
        protected override void AddInitialCommands()
        {
            base.AddInitialCommands();

            string terranCommanderPortrait = "Portraits\\TerranCommander";

            List<string> dialog = new List<string>()
            {
                "Fleet, this is the Commander",
                "We shall shortly be completing our trans-space jump and will appear on the edge of the pirate's sensors",
                "We will have the element of surprise",
                "Let us use it to eradicate the scourge that threatened our home"
            };

            Command previousCommand = AddCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, dialog));
        }

        #endregion
    }
}
