using _2DEngine;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// The base class for any object associated with a card in a card object pair.
    /// Objects become visible during the battle phase.
    /// </summary>
    public class CardObject : GameObject, ICardObjectElement
    {
        public CardObject(Vector2 localPosition, string dataAsset) :
            base(localPosition, dataAsset)
        {
            Hide();
        }

        #region Virtual Functions

        /// <summary>
        /// Override this function to perform custom behaviour when our turn begins.
        /// </summary>
        public virtual void OnTurnBegin()
        {
            Hide();
        }

        /// <summary>
        /// Override this function to perform custom behaviour when we begin the battle phase.
        /// </summary>
        public virtual void MakeReadyForBattle()
        {
            Show();
        }

        /// <summary>
        /// Override this function to perform custom behaviour when our turn ends.
        /// </summary>
        public virtual void OnTurnEnd() { }

        /// <summary>
        /// Kills our parent which will kill us and the card we are attached too
        /// </summary>
        public override void Die()
        {
            // Make sure we call Die so that when our parent calls Die on us again, we will already be dead and not have this function called again
            base.Die();

            DebugUtils.AssertNotNull(Parent);
            Parent.Die();
        }

        /// <summary>
        /// A templated function for accessing our parent as a specialized version of CardObjectPair
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetCardObjectPair<T>() where T : CardObjectPair
        {
            Debug.Assert(Parent is T);
            return Parent as T;
        }

        #endregion
    }
}
