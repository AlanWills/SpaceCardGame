namespace SpaceCardGame
{
    /// <summary>
    /// An interface for elements associated with cards.
    /// Should be attached to anything that extends off of a CardObejctPair, Card or CardObject.
    /// </summary>
    public interface ICardObjectElement
    {
        /// <summary>
        /// A function which is called at the start of the card placement phase.
        /// </summary>
        void OnTurnBegin();

        /// <summary>
        /// A function which is called at the start of the battle phase
        /// </summary>
        void MakeReadyForBattle();

        /// <summary>
        /// A function which is called at the start of the battle phase
        /// </summary>
        void OnTurnEnd();
    }
}
