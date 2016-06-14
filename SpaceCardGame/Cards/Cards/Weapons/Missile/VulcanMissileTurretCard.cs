using _2DEngine;
using Microsoft.Xna.Framework;

namespace SpaceCardGame
{
    /// <summary>
    /// The Vulcan missile turret fires missiles.
    /// Also, if the player pays one fuel it can fire one more time.
    /// </summary>
    public class VulcanMissileTurretCard : WeaponCard
    {
        #region Properties and Fields

        /// <summary>
        /// A flag to indicate whether we have performed our ability this turn.
        /// </summary>
        private bool AbilityPerformed { get; set; }

        #endregion

        public VulcanMissileTurretCard(CardData weaponCardData) :
            base(weaponCardData)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// Creates a missile turret.
        /// </summary>
        /// <param name="weaponObjectDataAsset"></param>
        /// <returns></returns>
        public override Turret CreateTurret(string weaponObjectDataAsset)
        {
            return new MissileTurret(weaponObjectDataAsset);
        }

        /// <summary>
        /// Add extra UI for our active ability
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            ClickableModule.OnLeftClicked += PerformAbility;

            base.LoadContent();
        }
        
        /// <summary>
        /// Disables the UI for activating our ability if we do not have enough fuel to use it
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            if (IsPlaced)
            {
                // If our ability is not performed and we have unused fuel we can activate the UI for this card's ability
                ValidateUI(!AbilityPerformed && CardObjectPair.Player.Resources[(int)ResourceType.Fuel].Exists(x => !x.Used));
            }
        }

        /// <summary>
        /// Resets our ability performed flag so that we can perform it once more
        /// </summary>
        public override void OnTurnBegin()
        {
            base.OnTurnBegin();

            AbilityPerformed = false;
        }

        #endregion

        #region Callbacks

        /// <summary>
        /// The callback for our ability - this can only happen once per turn
        /// </summary>
        private void PerformAbility(BaseObject clickedObject)
        {
            if (IsPlaced)
            {
                CardWeaponPair.Turret.ShotsLeft++;
                (ScreenManager.Instance.CurrentScreen as BattleScreen).ActivePlayer.AlterResources(ResourceType.Fuel, 1, ChargeType.kCharge);

                AbilityPerformed = true;
            }
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Changes the colour of the card and enables/disables click input based on the input flag
        /// </summary>
        /// <param name="canUseAbility"></param>
        private void ValidateUI(bool canUseAbility)
        {
            if (canUseAbility)
            {
                ClickableModule.ShouldHandleInput.Value = true;
                CardObjectPair.Colour.Value = Color.Green;
            }
            else
            {
                ClickableModule.ShouldHandleInput.Value = false;
                CardObjectPair.Colour.Value = Color.White;
            }
        }

        #endregion
    }
}