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
        /// The image label for the weapon's current damage.
        /// </summary>
        private ImageAndLabel CurrentDamage { get; set; }

        /// <summary>
        /// The image label for the weapon's shots left.
        /// </summary>
        private ImageAndLabel CurrentShotsLeft { get; set; }

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
            Vector2 textSize = new Vector2(20, 40);

            CheckShouldLoad();

            CurrentDamage = AddChild(new ImageAndLabel("", new Vector2(-Size.X * 0.5f, Size.Y * 0.5f), "UI\\CardStatThumnails\\DamageThumbnail"));
            CurrentDamage.Colour.Value = Color.Red;
            CurrentDamage.Label.Size = textSize;

            CurrentShotsLeft = AddChild(new ImageAndLabel("", Size * 0.5f, "UI\\CardStatThumnails\\ShotsLeftThumbnail"));
            CurrentShotsLeft.Colour.Value = Color.LightGray;
            CurrentShotsLeft.Label.Size = textSize;

            base.LoadContent();
        }

        /// <summary>
        /// Do some UI fixup
        /// </summary>
        public override void Initialise()
        {
            CheckShouldInitialise();

            base.Initialise();

            CurrentDamage.LocalPosition -= new Vector2(-CurrentDamage.Anchor.X * 0.5f, CurrentDamage.Size.Y * 0.5f);
            CurrentShotsLeft.LocalPosition -= new Vector2(CurrentShotsLeft.Anchor.X + CurrentShotsLeft.Size.X * 0.5f, CurrentShotsLeft.Size.Y * 0.5f);
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
            CurrentDamage.Label.Text = CardWeaponPair.Turret.BulletData.BulletDamage.ToString();
            CurrentShotsLeft.Label.Text = CardWeaponPair.Turret.ShotsLeft.ToString();
        }

        #endregion
    }
}
