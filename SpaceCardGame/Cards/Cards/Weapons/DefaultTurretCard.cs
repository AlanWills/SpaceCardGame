using SpaceCardGameData;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// The weapon card for the default turret every ship has when first created.
    /// Creates a kinetic turret.
    /// </summary>
    public class DefaultTurretCard : WeaponCard
    {
        public DefaultTurretCard(Player player, CardData weaponCardData) :
            base(player, weaponCardData)
        {
            // Never show an outline for the default turret card
            CardOutline.Hide();
        }

        #region Virtual Functions

        public override void Begin()
        {
            base.Begin();

            CardOutline.Die();
        }

        public override CardObjectPair CreateCardObjectPair()
        {
            Debug.Fail("This should never be called for the Default Turret Card");
            return null;
        }

        /// <summary>
        /// Creates a kinetic turret
        /// </summary>
        /// <param name="weaponObjectDataAsset"></param>
        /// <returns></returns>
        public override Turret CreateTurret(string weaponObjectDataAsset)
        {
            return new KineticTurret(weaponObjectDataAsset);
        }

        /// <summary>
        /// The default turret card is never played conventionally like a normal card - it is to fit in with our system only.
        /// Therefore it doesn't matter what we return here - it is never going to be called.
        /// </summary>
        /// <returns></returns>
        public override AICardWorthMetric CalculateAIMetric(GameBoardSection aiGameBoardSection, GameBoardSection otherGameBoardSection)
        {
            Debug.Fail("This should never be called");
            return AICardWorthMetric.kShouldNotPlayAtAll;
        }

        #endregion
    }
}
