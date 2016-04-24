using _2DEngine;
using Microsoft.Xna.Framework;

namespace SpaceCardGame
{
    /// <summary>
    /// The info image used in our battle screen to show information about a shield.
    /// Shows current shield strength and recharge rate.
    /// </summary>
    public class ShieldInfoImage : Image
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the card shield pair we are storing info for.
        /// </summary>
        private CardShieldPair CardShieldPair { get; set; }

        /// <summary>
        /// The label for the weapon's current damage.
        /// </summary>
        private Label CurrentShieldStrengthLabel { get; set; }

        /// <summary>
        /// The label for the weapon's shots left.
        /// </summary>
        private Label CurrentRechargeRateLabel { get; set; }

        #endregion

        public ShieldInfoImage(CardShieldPair cardShieldPair, Vector2 size, Vector2 localPosition) :
            base(size, localPosition, cardShieldPair.Card.TextureAsset)
        {
            CardShieldPair = cardShieldPair;
        }

        #region Virtual Functions

        /// <summary>
        /// Adds UI to represent the shield's current strength and shots left.
        /// </summary>
        public override void LoadContent()
        {
            Vector2 textSize = new Vector2(30, 60);

            CheckShouldLoad();

            CurrentShieldStrengthLabel = AddChild(new Label("", new Vector2(-Size.X * 0.5f, Size.Y * 0.5f)));
            CurrentShieldStrengthLabel.Colour.Value = Color.LightGreen;
            CurrentShieldStrengthLabel.Size = textSize;

            CurrentRechargeRateLabel = AddChild(new Label("", Size * 0.5f));
            CurrentRechargeRateLabel.Colour.Value = Color.Red;
            CurrentRechargeRateLabel.Size = textSize;

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
            CurrentShieldStrengthLabel.Text = CardShieldPair.Shield.DamageModule.Health.ToString();
            CurrentRechargeRateLabel.Text = CardShieldPair.Shield.ShieldData.ShieldRechargePerTurn.ToString();
        }

        #endregion
    }
}
