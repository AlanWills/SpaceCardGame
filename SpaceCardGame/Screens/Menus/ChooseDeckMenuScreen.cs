using _2DEngine;
using CardGameEngine;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// The screen we have just before our gameplay screen where we choose the deck to use.
    /// </summary>
    public class ChooseDeckMenuScreen : MenuScreen
    {
        public ChooseDeckMenuScreen(string screenDataAsset = "Screens\\ChooseDeckMenuScreen.xml") :
            base(screenDataAsset)
        {

        }

        #region Virtual Functions

        protected override void AddInitialUI()
        {
            base.AddInitialUI();

            // Have to do this separately so we get the callbacks added to our objects during load
            ChooseDeckGridControl chooseDeckBox = AddScreenUIObject(new ChooseDeckGridControl(1, 4, ScreenCentre));
            chooseDeckBox.OnLeftClicked += ChooseDeckBoxClicked;

            AddScreenUIObject(new Label("Choose Deck", new Vector2(ScreenCentre.X, ScreenCentre.Y * 0.25f)));
        }

        /// <summary>
        /// Go to our lobby menu screen
        /// </summary>
        protected override void GoToPreviousScreen()
        {
            Transition(new LobbyMenuScreen());
        }

        #endregion

        #region Callbacks

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

            Transition(new BattleScreen(clickableImage.StoredObject as Deck));
        }

        #endregion
    }
}