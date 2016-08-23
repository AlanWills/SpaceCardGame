using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using CelesteEngine;

namespace SpaceCardGame
{
    /// <summary>
    /// Adds a text dialog box to the screen.
    /// </summary>
    public class CharacterDialogBoxCommand : Command
    {
        #region Properties and Fields

        /// <summary>
        /// The dialog box we create for this command
        /// </summary>
        private CharacterDialogBox DialogBox { get; set; }

        #endregion

        public CharacterDialogBoxCommand(string characterPortraitTextureAsset, string text, bool shouldPauseGame = true, float lifeTime = float.MaxValue) :
            this(characterPortraitTextureAsset, new string[1] { text }, shouldPauseGame, lifeTime)
        {

        }

        public CharacterDialogBoxCommand(string characterPortraitTextureAsset, List<string> strings, bool shouldPauseGame = true, float lifeTime = float.MaxValue) :
            this(characterPortraitTextureAsset, strings.ToArray(), shouldPauseGame, lifeTime)
        {

        }

        public CharacterDialogBoxCommand(string characterPortraitTextureAsset, string[] strings, bool shouldPauseGame = true, float lifeTime = float.MaxValue) :
            base(shouldPauseGame, lifeTime)
        {
            Debug.Assert(strings.Length > 0);

            DialogBox = ParentScreen.AddScreenUIObject(new CharacterDialogBox(characterPortraitTextureAsset, strings, "", ScreenManager.Instance.ScreenCentre), true, true);
            DialogBox.Hide();
        }

        #region Virtual Functions

        /// <summary>
        /// When we begin running this command, we show our text box
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            DialogBox.Show();
        }

        /// <summary>
        /// If our game is paused, manually handle input for our text box
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            if (!GameHandleInput)
            {
                DialogBox.HandleInput(elapsedGameTime, mousePosition);
            }
        }

        /// <summary>
        /// If our game is paused, manually update our text box
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            if (!GameUpdate)
            {
                DialogBox.Update(elapsedGameTime);
            }

            // When the text box dies, kill this command
            if (!DialogBox.IsAlive)
            {
                Die();
            }
        }

        #endregion
    }
}
