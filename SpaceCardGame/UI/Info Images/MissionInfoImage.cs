using CelesteEngine;
using Microsoft.Xna.Framework;
using SpaceCardGameData;
using System;
using System.Diagnostics;
using System.Reflection;

namespace SpaceCardGame
{
    /// <summary>
    /// An info UI for displaying details about a mission.
    /// Contains the UI for playing the mission
    /// </summary>
    public class MissionInfoImage : UIObject
    {
        #region Properties and Fields

        private MissionData MissionData { get; set; }

        #endregion

        public MissionInfoImage(MissionData missionData) :
            base(new Vector2(ScreenManager.Instance.ScreenDimensions.X * 0.5f, ScreenManager.Instance.ScreenDimensions.Y),
                 new Vector2(ScreenManager.Instance.ScreenDimensions.X * 0.75f, ScreenManager.Instance.ScreenDimensions.Y * 0.5f),
                 AssetManager.DefaultMenuTextureAsset)
        {
            MissionData = missionData;
            UsesCollider = false;
        }

        #region Virtual Functions

        /// <summary>
        /// Add all of the information for the mission
        /// </summary>
        public override void Initialise()
        {
            CheckShouldInitialise();

            base.Initialise();

            // Add the mission thumbnail image
            Image thumbnail = AddChild(new Image(new Vector2(0, -Size.Y * 0.25f), MissionData.MissionThumbnailTextureAsset), true, true);

            // Add the mission name label
            Label missionNameLabel = thumbnail.AddChild(new Label(MissionData.MissionName, Anchor.kTopCentre, 2), true, true);
            missionNameLabel.Colour = Color.White;

            // Add the mission description
            Label missionDescriptionLabel = thumbnail.AddChild(new Label(MissionData.MissionDescription, Anchor.kBottomCentre, 2), true, true);
            missionDescriptionLabel.Colour = Color.White;

            // Add the button to play the mission
            Button playMissionButton = missionDescriptionLabel.AddChild(new Button("Play Mission", Anchor.kBottomCentre, 2, AssetManager.DefaultNarrowButtonTextureAsset, AssetManager.DefaultNarrowButtonHighlightedTextureAsset), true, true);
            playMissionButton.LocalPosition = new Vector2(0, missionDescriptionLabel.Size.Y + playMissionButton.Size.Y);
            playMissionButton.ClickableModule.OnLeftClicked += PlayMissionCallback;
        }

        #endregion

        #region Callbacks

        private void PlayMissionCallback(BaseObject clickedObject)
        {
            // Have to do this separately so we get the callbacks added to our objects during load
            ChooseDeckGridControl chooseDeckBox = ScreenManager.Instance.CurrentScreen.AddScreenUIObject(new ChooseDeckGridControl(4, ScreenManager.Instance.ScreenCentre), true, true);
            chooseDeckBox.OnLeftClicked += ChooseDeckBoxClicked;
        }

        /// <summary>
        /// The callback to execute when we choose our deck
        /// </summary>
        /// <param name="image"></param>
        private void ChooseDeckBoxClicked(BaseObject baseObject)
        {
            Debug.Assert(baseObject is UIObject);
            UIObject clickableImage = baseObject as UIObject;

            DebugUtils.AssertNotNull(clickableImage.StoredObject);
            Debug.Assert(clickableImage.StoredObject is Deck);

            // Create our opponent's deck using the cards in our mission data
            Deck opponentDeck = new Deck();
            opponentDeck.Create(MissionData.OpponentDeckData.CardDataAssets);

            // Find the appropriate type of screen we should transition to based on the mission
            Type missionScreenType = Assembly.GetExecutingAssembly().GetType("SpaceCardGame." + MissionData.MissionName + "Mission");
            DebugUtils.AssertNotNull(missionScreenType);

            // Transition to the new screen
            BattleScreen missionScreen = (BattleScreen)Activator.CreateInstance(missionScreenType, clickableImage.StoredObject as Deck, opponentDeck);
            ScreenManager.Instance.CurrentScreen.Transition(missionScreen);
        }

        #endregion
    }
}