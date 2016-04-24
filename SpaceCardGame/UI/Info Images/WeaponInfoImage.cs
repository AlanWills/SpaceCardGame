using _2DEngine;
using Microsoft.Xna.Framework;

namespace SpaceCardGame
{
    /// <summary>
    /// The info image used in our battle screen to show information about a weapon.
    /// Shows damage and shots left.
    /// </summary>
    public class WeaponInfoImage : Image
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the card weapon pair we are storing info for.
        /// </summary>
        private CardWeaponPair CardWeaponPair { get; set; }

        /// <summary>
        /// The label for the weapon's current damage.
        /// </summary>
        private Label CurrentDamageLabel { get; set; }

        /// <summary>
        /// The label for the weapon's shots left.
        /// </summary>
        private Label CurrentShotsLeftLabel { get; set; }

        #endregion

        public WeaponInfoImage(CardWeaponPair cardWeaponPair, Vector2 size, Vector2 localPosition) :
            base(size, localPosition, cardWeaponPair.Card.TextureAsset)
        {
            CardWeaponPair = cardWeaponPair;
        }

        #region Virtual Functions

        /// <summary>
        /// Adds UI to represent the weapon's damage and shots left
        /// </summary>
        public override void LoadContent()
        {
            Vector2 textSize = new Vector2(30, 60);

            CheckShouldLoad();

            CurrentDamageLabel = AddChild(new Label("", new Vector2(-Size.X * 0.5f, Size.Y * 0.5f)));
            CurrentDamageLabel.Colour.Value = Color.Red;
            CurrentDamageLabel.Size = textSize;

            CurrentShotsLeftLabel = AddChild(new Label("", Size * 0.5f));
            CurrentShotsLeftLabel.Colour.Value = Color.LightGray;
            CurrentShotsLeftLabel.Size = textSize;

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
            CurrentDamageLabel.Text = CardWeaponPair.Turret.BulletData.BulletDamage.ToString();
            CurrentShotsLeftLabel.Text = CardWeaponPair.Turret.ShotsLeft.ToString();
        }

        #endregion
    }
}
