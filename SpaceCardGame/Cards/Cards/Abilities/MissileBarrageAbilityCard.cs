using CelesteEngine;
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
        public MissileBarrageAbilityCard(Player player, CardData abilityCardData) :
            base(player, abilityCardData)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// When we lay this card, use the ability immediately
        /// </summary>
        public override void OnLay()
        {
            base.OnLay();

            // No target
            UseAbility(null);
        }

        /// <summary>
        /// When we lay this card, we do damage all the opponent's ships with defence + speed <= 5
        /// </summary>
        public override void UseAbility(CardObjectPair target)
        {
            BattleScreen battleScreen = ScreenManager.Instance.GetCurrentScreenAs<BattleScreen>();

            foreach (CardShipPair pair in battleScreen.Board.NonActivePlayerBoardSection.GameBoardSection.ShipCardControl)
            {
                // Add up the ship's speed and defence and if it is less than or equal to 5, we do one damage to it
                if (pair.Ship.ShipData.Defence + pair.Ship.ShipData.Speed <= 5)
                {
                    pair.Ship.DamageModule.Damage(1, this);
                    SpawnMissileAtTarget(pair.Ship);
                }
            }

            // Kill our parent which will kill us and the object we are paired to too
            Parent.Die();
        }

        /// <summary>
        /// To decide if this ability is worth playing, we look at the number of ships it will actually affect.
        /// </summary>
        /// <param name="aiGameBoardSection"></param>
        /// <param name="otherGameBoardSection"></param>
        /// <returns></returns>
        public override AICardWorthMetric CalculateAIMetric(GameBoardSection aiGameBoardSection, GameBoardSection otherGameBoardSection)
        {
            int count = 0;
            foreach (CardShipPair ship in otherGameBoardSection.ShipCardControl)
            {
                if (ship.Ship.ShipData.Defence + ship.Ship.ShipData.Speed <= 5)
                {
                    count++;
                }
            }

            if (count >= 3)
            {
                return AICardWorthMetric.kGoodCardToPlay;
            }
            else if (count >= 2)
            {
                return AICardWorthMetric.kAverageCardToPlay;
            }
            else if (count >= 1)
            {
                return AICardWorthMetric.kBadCardToPlay;
            }
            else
            {
                return AICardWorthMetric.kShouldNotPlayAtAll;
            }
        }


        #endregion

        #region Utility Functions

        /// <summary>
        /// Spawns a missile (not parented under a turret, but just as effect) which fires at the inputted target ship
        /// </summary>
        /// <param name="targetShip"></param>
        private void SpawnMissileAtTarget(Ship targetShip)
        {
            ProjectileData missileData = AssetManager.GetData<ProjectileData>(Missile.defaultMissileDataAsset);
            DebugUtils.AssertNotNull(missileData);

            ScreenManager.Instance.CurrentScreen.AddInGameUIObject(new Missile(targetShip, WorldPosition, missileData), true, true);
        }

        #endregion
    }
}