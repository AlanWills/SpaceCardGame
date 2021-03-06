﻿using CelesteEngine;
using Microsoft.Xna.Framework;

namespace SpaceCardGame
{
    /// <summary>
    /// The info image used in our battle screen to show the status of the ship.
    /// Shows current attack, current defence, shield status, weapon status and speed.
    /// </summary>
    public class ShipInfoImage : Image
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the card ship pair we are storing info for.
        /// </summary>
        private CardShipPair CardShipPair { get; set; }

        /// <summary>
        /// The image and label pair for the ship's current attack.
        /// </summary>
        private ImageAndLabel CurrentAttack { get; set; }

        /// <summary>
        /// The image and label for the ship's current defence.
        /// </summary>
        private ImageAndLabel CurrentDefence { get; set; }

        /// <summary>
        /// The image and label for the ship's current speed.
        /// </summary>
        private ImageAndLabel CurrentSpeed { get; set; }

        #endregion

        public ShipInfoImage(CardShipPair cardShipPair, Vector2 size, Vector2 localPosition) :
            base(size, localPosition, cardShipPair.Card.TextureAsset)
        {
            CardShipPair = cardShipPair;
        }

        #region Virtual Functions

        /// <summary>
        /// Adds UI to represent the ship's current attack, defence, speed, shield and turret status.
        /// </summary>
        public override void LoadContent()
        {
            Vector2 textSize = new Vector2(20, 40);

            CheckShouldLoad();

            CurrentAttack = AddChild(new ImageAndLabel("", new Vector2(-Size.X * 0.5f, Size.Y * 0.5f), "UI\\CardStatThumbnails\\DamageThumbnail"));
            CurrentAttack.Colour = Color.Red;
            CurrentAttack.Label.Size = textSize;

            CurrentDefence = AddChild(new ImageAndLabel("", new Vector2(0, Size.Y * 0.5f), "UI\\CardStatThumbnails\\DefenceThumbnail"));
            CurrentDefence.Colour = Color.LightGreen;
            CurrentDefence.Label.Size = textSize;

            CurrentSpeed = AddChild(new ImageAndLabel("", Size * 0.5f, "UI\\CardStatThumbnails\\SpeedThumbnail"));
            CurrentSpeed.Colour = Color.White;
            CurrentSpeed.Label.Size = textSize;

            base.LoadContent();
        }

        /// <summary>
        /// Do some UI fixup
        /// </summary>
        public override void Initialise()
        {
            CheckShouldInitialise();

            base.Initialise();

            CurrentAttack.LocalPosition -= new Vector2(-CurrentAttack.CentreAnchor.X * 0.5f, CurrentAttack.Size.Y * 0.5f);
            CurrentDefence.LocalPosition -= new Vector2(CurrentDefence.CentreAnchor.X - CurrentDefence.Size.X * 0.5f, CurrentDefence.Size.Y * 0.5f);
            CurrentSpeed.LocalPosition -= new Vector2(CurrentSpeed.CentreAnchor.X + CurrentSpeed.Size.X * 0.5f, CurrentSpeed.Size.Y * 0.5f);
        }

        /// <summary>
        /// Update the labels to sync with any changes made in game to our ship's stats.
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            UpdateLabels();
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Updates our attack, defence and speed label text.
        /// </summary>
        private void UpdateLabels()
        {
            CurrentAttack.Label.Text = CardShipPair.ShipCard.CalculateAttack(null).ToString();
            CurrentDefence.Label.Text = CardShipPair.Ship.DamageModule.Health.ToString();
            CurrentSpeed.Label.Text = CardShipPair.Ship.ShipData.Speed.ToString();
        }

        #endregion
    }
}
