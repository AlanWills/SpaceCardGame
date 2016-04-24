using _2DEngine;
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
        /// The label for the ship's current attack.
        /// </summary>
        private Label CurrentAttackLabel { get; set; }

        /// <summary>
        /// The label for the ship's current defence.
        /// </summary>
        private Label CurrentDefenceLabel { get; set; }

        /// <summary>
        /// The label for the ship's current speed.
        /// </summary>
        private Label CurrentSpeedLabel { get; set; }

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
            Vector2 textSize = new Vector2(30, 60);

            CheckShouldLoad();

            CurrentAttackLabel = AddChild(new Label("", new Vector2(-Size.X * 0.5f, Size.Y * 0.5f)));
            CurrentAttackLabel.Colour.Value = Color.Red;
            CurrentAttackLabel.Size = textSize;

            CurrentDefenceLabel = AddChild(new Label("", new Vector2(0, Size.Y * 0.5f)));
            CurrentDefenceLabel.Colour.Value = Color.LightGreen;
            CurrentDefenceLabel.Size = textSize;

            CurrentSpeedLabel = AddChild(new Label("", Size * 0.5f));
            CurrentSpeedLabel.Colour.Value = Color.Yellow;
            CurrentSpeedLabel.Size = textSize;

            base.LoadContent();
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
            CurrentAttackLabel.Text = CardShipPair.ShipCard.CalculateAttack(null).ToString();
            CurrentDefenceLabel.Text = CardShipPair.Ship.DamageModule.Health.ToString();
            CurrentSpeedLabel.Text = CardShipPair.Ship.ShipData.Speed.ToString();
        }

        #endregion
    }
}
