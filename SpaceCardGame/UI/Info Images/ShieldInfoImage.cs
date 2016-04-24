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
        /// The image label for the weapon's current damage.
        /// </summary>
        private ImageAndLabel CurrentShieldStrength { get; set; }

        /// <summary>
        /// The image label for the weapon's shots left.
        /// </summary>
        private ImageAndLabel CurrentRechargeRate { get; set; }

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
            Vector2 textSize = new Vector2(20, 40);

            CheckShouldLoad();

            CurrentShieldStrength = AddChild(new ImageAndLabel("", new Vector2(-Size.X * 0.5f, Size.Y * 0.5f), "UI\\CardStatThumnails\\DefenceThumbnail"));
            CurrentShieldStrength.Colour.Value = Color.LightGreen;
            CurrentShieldStrength.Label.Size = textSize;

            CurrentRechargeRate = AddChild(new ImageAndLabel("", Size * 0.5f, "UI\\CardStatThumnails\\ShieldRechargeThumbnail"));
            CurrentRechargeRate.Colour.Value = Color.Red;
            CurrentRechargeRate.Label.Size = textSize;

            base.LoadContent();
        }

        /// <summary>
        /// Do some UI fixup
        /// </summary>
        public override void Initialise()
        {
            CheckShouldInitialise();

            base.Initialise();

            CurrentShieldStrength.LocalPosition -= new Vector2(-CurrentShieldStrength.Anchor.X * 0.5f, CurrentShieldStrength.Size.Y * 0.5f);
            CurrentRechargeRate.LocalPosition -= new Vector2(CurrentRechargeRate.Anchor.X + CurrentRechargeRate.Size.X * 0.5f, CurrentRechargeRate.Size.Y * 0.5f);
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
            CurrentShieldStrength.Label.Text = CardShieldPair.Shield.DamageModule.Health.ToString();
            CurrentRechargeRate.Label.Text = CardShieldPair.Shield.ShieldData.ShieldRechargePerTurn.ToString();
        }

        #endregion
    }
}
