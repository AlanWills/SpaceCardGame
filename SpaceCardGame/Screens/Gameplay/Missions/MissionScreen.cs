using _2DEngine;
using SpaceCardGameData;
using System.Collections.Generic;

namespace SpaceCardGame
{
    /// <summary>
    /// A wrapper class for the battle screen which holds useful data which all of our missions will use.
    /// Mainly focused on stuff for setting up opening dialog and victory/defeat UI.
    /// </summary>
    public abstract class MissionScreen : BattleScreen
    {
        #region Properties and Fields

        /// <summary>
        /// A container for any dialog that will used at some point in the screen
        /// </summary>
        protected Queue<List<string>> Dialog { get; set; }

        #endregion

        public MissionScreen(Deck playerChosenDeck, Deck opponentChosenDeck, string screenDataAsset) :
            base(playerChosenDeck, opponentChosenDeck, screenDataAsset)
        {
            Dialog = new Queue<List<string>>();
        }

        #region Virtual Functions

        /// <summary>
        /// A utility function for enqueue the dialog we will use in the dialog commands.
        /// Called before LoadContent.
        /// </summary>
        protected virtual void AddDialogStrings() { }

        /// <summary>
        /// Adds the dialog strings to the Dialog queue for use in AddInitialCommands.
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            AddDialogStrings();

            base.LoadContent();
        }

        /// <summary>
        /// Update the player's data so that we record they have completed this mission
        /// </summary>
        protected override void OnOpponentDefeated()
        {
            base.OnOpponentDefeated();

            MissionData missionData = AssetManager.GetData<MissionData>((ScreenData as MissionScreenData).MissionDataAsset);
            PlayerDataRegistry.Instance.PlayerData.CurrentLevel = missionData.MissionNumber + 1;

            AddScreenUIObject(new PlayerVictoryUI(missionData.RewardData), true, true);
        }

        #endregion
    }
}
