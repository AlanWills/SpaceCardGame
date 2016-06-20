using _2DEngine;
using System.Collections.Generic;

namespace SpaceCardGame
{
    public class PyronMission : BattleScreen
    {
        public PyronMission(Deck playerChosenDeck, Deck opponentChosenDeck) :
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
            string kthanCommanderPortrait = "Portraits\\K'ThanCommander";

            List<string> dialog = new List<string>()
            {
                "K'Than fleet, this is the Commander of a unified Terran and El'Ek fleet",
                "We are here to stop your reign of terror throughout this system"
            };

            Command previousCommand = AddCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, dialog));

            dialog = new List<string>()
            {
                "Did I not destroy you and your pitiful fleet at Freiza?",
                "No matter, it will take just as little effort to wipe you out again"
            };

            previousCommand = AddCommand(new CharacterDialogBoxCommand(kthanCommanderPortrait, dialog));

            dialog = new List<string>()
            {
                "This is it fleet",
                "No other objectives",
                "Take out their command ship and this threat will be over",
                "Lets finish this"
            };

            previousCommand = AddCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, dialog));
        }

        #endregion
    }
}
