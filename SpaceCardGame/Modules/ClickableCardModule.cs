using _2DEngine;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A wrapper around the implementation of the ClickableObjectModule which will only respond to clicks when the current active player is the owner of the card
    /// </summary>
    public class ClickableCardModule : ClickableObjectModule
    {
        #region Properties and Fields

        private Card card;
        private Card Card
        {
            get
            {
                if (card == null)
                {
                    Debug.Assert(AttachedBaseObject is Card);
                    card = AttachedBaseObject as Card;
                }

                return card;
            }
        }

        #endregion

        #region Virtual Functions

        /// <summary>
        /// Only update if it is the owner of this Card's go
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            if (Card.Parent is CardObjectPair)
            {
                ShouldHandleInput.Value = Card.Player == (ScreenManager.Instance.CurrentScreen as BattleScreen).ActivePlayer;
            }
        }

        #endregion
    }
}
