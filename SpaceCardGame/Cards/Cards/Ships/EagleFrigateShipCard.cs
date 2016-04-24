using _2DEngine;

namespace SpaceCardGame
{
    /// <summary>
    /// The eagle frigate takes one less damage from slower ships and deals one more damage to slower ships.
    /// We add a custom function to our damage module to deal with taking less damage.
    /// </summary>
    public class EagleFrigateShipCard : ShipCard
    {
        public EagleFrigateShipCard(ShipCardData shipCardData) :
            base(shipCardData)
        {

        }

        #region Event Callbacks

        /// <summary>
        /// The callback we call when this ship is damaged.
        /// The object doing the damaging should be a CardShipPair and we analyse the speed of it against our own to see whether we can nullify some damage.
        /// </summary>
        /// <param name="shipDamageModule"></param>
        /// <param name="objectDoingTheDamage"></param>
        protected override float CalculateDamageDoneToThis(BaseObject objectDoingTheDamage, float inputDamage)
        {
            // Some sources of damage may not come from a ship (and indeed could be healing) so we wish to ignore those
            if (objectDoingTheDamage is CardShipPair)
            {
                CardShipPair attackingShipPair = objectDoingTheDamage as CardShipPair;
                if (attackingShipPair.Ship.ShipData.Speed < CardShipPair.Ship.ShipData.Speed)
                {
                    // We are being damaged by a ship which is slower than us so we can remove one damage
                    inputDamage--;
                }
            }

            return inputDamage;
        }

        /// <summary>
        /// Callback for calculating how much damage this ship does.
        /// We analyse the inputted target and if this ship has higher speed, we deal one extra damage
        /// </summary>
        /// <returns></returns>
        public override float CalculateAttack(BaseObject targetBaseObject)
        {
            float baseAttack = base.CalculateAttack(targetBaseObject);

            if (targetBaseObject != null && targetBaseObject is CardShipPair)
            {
                CardShipPair targetCardShipPair = targetBaseObject as CardShipPair;
                if (CardShipPair.Ship.ShipData.Speed > targetCardShipPair.Ship.ShipData.Speed)
                {
                    // If we have a higher speed, we do more damage
                    baseAttack++;
                }
            }

            return baseAttack;
        }

        #endregion
    }
}