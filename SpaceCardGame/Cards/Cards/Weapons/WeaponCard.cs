﻿using CelesteEngine;
using SpaceCardGameData;

namespace SpaceCardGame
{
    /// <summary>
    /// A class used to represent a weapon in our game.
    /// </summary>
    public abstract class WeaponCard : Card
    {
        #region Properties and Fields

        // The path to a default weapon we will use to create a weapon for each ship initially
        public const string DefaultWeaponCardDataAsset = "Cards\\Weapons\\DefaultTurretCard";

        #endregion

        public WeaponCard(Player player, CardData weaponCardData) :
            base(player, weaponCardData)
        {
            
        }

        #region Virtual Functions

        /// <summary>
        /// Weapons create a CardWeaponPair
        /// </summary>
        /// <returns></returns>
        public override CardObjectPair CreateCardObjectPair()
        {
            return new CardWeaponPair(this);
        }

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
        /// Alternatively, if the AI is laying a card it should add it without running the script.
        /// </summary>
        public override void OnLay()
        {
            base.OnLay();

            CommandManager.Instance.AddChild(new ChooseFriendlyShipCommand(this), true, true);
        }

        /// <summary>
        /// Any weapon is good to lay
        /// </summary>
        /// <param name="aiGameBoardSection"></param>
        /// <param name="otherGameBoardSection"></param>
        /// <returns></returns>
        public override AICardWorthMetric CalculateAIMetric(GameBoardSection aiGameBoardSection, GameBoardSection otherGameBoardSection)
        {
            return AICardWorthMetric.kGoodCardToPlay;
        }

        #endregion
    }
}
