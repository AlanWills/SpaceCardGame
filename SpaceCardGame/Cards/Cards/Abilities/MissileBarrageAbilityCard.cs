using _2DEngine;
using SpaceCardGameData;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// The missile barrage ability card.
    /// Deals one damage to all enemies with a combined defence and speed of 5 or less when laid.
    /// </summary>
    public class MissileBarrageAbilityCard : AbilityCard
    {
        public MissileBarrageAbilityCard(AbilityCardData abilityCardData) :
            base(abilityCardData)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// When we lay this card, use the ability immediately
        /// </summary>
        public override void OnLay()
        {
            // No target
            UseAbility(null);
        }

        /// <summary>
        /// When we lay this card, we do damage all the opponent's ships with defence + speed <= 5
        /// </summary>
        public override void UseAbility(CardObjectPair target)
        {
            Debug.Assert(ScreenManager.Instance.CurrentScreen is BattleScreen);
            BattleScreen battleScreen = ScreenManager.Instance.CurrentScreen as BattleScreen;

            foreach (CardShipPair pair in battleScreen.Board.NonActivePlayerBoardSection.GameBoardSection.ShipCardControl)
            {
                // Add up the ship's speed and defence and if it is less than or equal to 5, we do one damage to it
                if (pair.Ship.ShipData.Defence + pair.Ship.ShipData.Speed <= 5)
                {
                    pair.Ship.DamageModule.Damage(1, CardPair);
                    SpawnBulletAtTarget(pair.Ship);
                }
            }

            // Kill our parent which will kill us and the object we are paired to too
            Parent.Die();
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Spawns a bullet (not parented under a turret, but just as effect) which fires at the inputted target ship
        /// </summary>
        /// <param name="targetShip"></param>
        private void SpawnBulletAtTarget(Ship targetShip)
        {
            ProjectileData bulletData = AssetManager.GetData<ProjectileData>(Bullet.defaultBulletDataAsset);
            DebugUtils.AssertNotNull(bulletData);

            ScreenManager.Instance.CurrentScreen.AddInGameUIObject(new Bullet(targetShip, WorldPosition, bulletData), true, true);
        }

        #endregion
    }
}