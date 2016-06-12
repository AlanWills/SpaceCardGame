namespace SpaceCardGame
{
    /// <summary>
    /// Class for the phase energy shield card
    /// </summary>
    public class PhaseEnergyShieldCard : ShieldCard
    {
        public PhaseEnergyShieldCard(CardData shieldCardData) :
            base(shieldCardData)
        {

        }

        #region Virtual Functions

        public override AICardWorthMetric CalculateAIMetric()
        {
            return AICardWorthMetric.kShouldNotPlayAtAll;
        }

        #endregion
    }
}
