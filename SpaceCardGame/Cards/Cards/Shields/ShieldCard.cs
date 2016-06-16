using _2DEngine;

namespace SpaceCardGame
{
    /// <summary>
    /// A class used to represent a shield in our game.
    /// </summary>
    public abstract class ShieldCard : Card
    {
        public ShieldCard(Player player, CardData shieldCardData) :
            base(player, shieldCardData)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// Shields create a CardShieldPair
        /// </summary>
        /// <returns></returns>
        public override CardObjectPair CreateCardObjectPair()
        {
            return new CardShieldPair(this);
        }

        /// <summary>
        /// Defence cards can only be targetted on ships, so we just check that we have a CardShipPair which is not dead
        /// </summary>
        /// <param name="pairToValidate"></param>
        /// <returns></returns>
        public override bool CanUseOn(CardObjectPair pairToValidate)
        {
            if (pairToValidate is CardShipPair)
            {
                // If we are targeting a ship, it is valid if it is not dead
                return !(pairToValidate as CardShipPair).Ship.DamageModule.Dead;
            }

            // Otherwise the target is invalid
            return false;
        }

        /// <summary>
        /// When we lay a shield we need to run a script to choose a ship to add it to.
        /// /// Alternatively, if the AI is laying a card it should add it without running the script.
        /// </summary>
        public override void OnLay()
        {
            base.OnLay();

            CommandManager.Instance.AddChild(new ChooseFriendlyShipCommand(this), true, true);
        }

        #endregion
    }
}