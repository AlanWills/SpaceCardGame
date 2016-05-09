namespace SpaceCardGame
{
    /// <summary>
    /// A command which we run at the start of the tutorial.
    /// Rather than adding random cards to the player's starting hands, we add specific types so that we can explain their functionality.
    /// </summary>
    public class NewTutorialCommand : NewGameCommand
    {
        #region Virtual Functions

        /// <summary>
        /// We want to add certain cards for our player and opponent rather than random ones.
        /// </summary>
        protected override void AddInitialCards(float elapsedGameTime)
        {
            BattleScreen.Player.DrawCard("FuelResourceCard");
            BattleScreen.Player.DrawCard("PhaseEnergyShieldCard");
            BattleScreen.Player.DrawCard("WaspFighterShipCard");
            BattleScreen.Player.DrawCard("VulcanMissileTurretCard");
            BattleScreen.Player.DrawCard("MissileBarrageAbilityCard");

            BattleScreen.Opponent.DrawCard(5);

            Die();
        }

        #endregion
    }
}
