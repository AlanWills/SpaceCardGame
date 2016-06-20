using _2DEngine;
using System.Collections.Generic;

namespace SpaceCardGame.Screens.Gameplay.Missions
{
    public class FreizaMission : BattleScreen
    {
        public FreizaMission(Deck playerChosenDeck, Deck opponentChosenDeck) :
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

            string computerAIPortrait = "Portraits\\ComputerAI";
            string terranCommanderPortrait = "Portraits\\TerranCommander";
            string elekCommanderPortrait = "Portraits\\El'EkCommander";

            List<string> dialog = new List<string>()
            {
                "We have intercepted K'Than transmissions using our sensor relays",
                "Their attack on Freiza has begun",
                "It cannot be allowed to continue",
            };

            Command previousCommand = AddCommand(new CharacterDialogBoxCommand(elekCommanderPortrait, dialog));

            dialog = new List<string>()
            {
                "Using all known data I estimate our probability of victory at 8.13%",
                "We are walking to defeat"
            };

            previousCommand = AddCommand(new CharacterDialogBoxCommand(computerAIPortrait, dialog));

            dialog = new List<string>()
            {
                "We have no choice - the fewer free planets exist, the fewer allies we have",
                "We must try"
            };

            previousCommand = AddCommand(new CharacterDialogBoxCommand(terranCommanderPortrait, dialog));
        }

        #endregion
    }
}
