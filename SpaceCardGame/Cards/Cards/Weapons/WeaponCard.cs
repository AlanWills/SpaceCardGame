using _2DEngine;

namespace SpaceCardGame
{
    /// <summary>
    /// A class used to represent a weapon in our game.
    /// </summary>
    public abstract class WeaponCard : GameCard
    {
        public WeaponCard(WeaponCardData weaponCardData) :
            base(weaponCardData)
        {
            
        }

        #region Virtual Functions

        /// <summary>
        /// An abstract function each weapon card will need to implement for specifying the type of turret we create
        /// </summary>
        /// <returns></returns>
        public abstract Turret CreateTurret(string weaponObjectDataAsset);

        /// <summary>
        /// Defence cards can only be targetted on ships, so we just check that we have a CardShipPair which is not dead
        /// </summary>
        /// <param name="pairToValidate"></param>
        /// <returns></returns>
        public override bool CanUseOn(CardObjectPair pairToValidate)
        {
            if (pairToValidate is CardShipPair)
            {
                // If we are targeting a ship, it is valid if it not dead
                return !(pairToValidate as CardShipPair).Ship.DamageModule.Dead;
            }

            // Otherwise the target is invalid
            return false;
        }

        /// <summary>
        /// When we lay a WeaponCard we need to run a script to find a ship to add it to.
        /// </summary>
        public override void OnLay()
        {
            CommandManager.Instance.AddChild(new ChooseFriendlyShipCommand(this), true, true);
        }

        #endregion
    }
}
