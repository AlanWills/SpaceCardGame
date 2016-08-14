using _2DEngine;
using Microsoft.Xna.Framework;

namespace SpaceCardGame
{
    /// <summary>
    /// A class which functions exactly the same except for some UI fixups so our battle screen is more user-friendly
    /// </summary>
    public class OpponentBoardSection : PlayerBoardSection
    {
        public OpponentBoardSection(Player player, Vector2 localPosition, string dataAsset = AssetManager.EmptyGameObjectDataAsset) :
            base(player, localPosition, dataAsset)
        {
            UsesCollider = false;

            // We should not be able to interact with the opponent's Hand
            // Instead we will use an AI script to add things
            UIBoardSection.HandUI.ShouldHandleInput = false;
        }

        #region Virtual Functions

        /// <summary>
        /// Fixes up our UI for the battle screen
        /// </summary>
        public override void Begin()
        {
            // This is used in base.Begin() so must be fixed up before we call that function
            GameBoardSection.ShipCardControl.StationPosition *= new Vector2(1, -1);

            base.Begin();

            LocalRotation = MathHelper.Pi;
            GameBoardSection.LocalRotation = MathHelper.Pi;
            GameBoardSection.ShipCardControl.LocalPosition *= new Vector2(1, -1);

            bool includeChildrenToAdd = true;
            foreach (CardOutline cardOutline in GameBoardSection.GetChildrenOfType<CardOutline>(includeChildrenToAdd))
            {
                cardOutline.LocalPosition *= new Vector2(1, -1);
            }

            UIBoardSection.LocalRotation = MathHelper.Pi;
            UIBoardSection.HandUI.LocalRotation = MathHelper.Pi;
            UIBoardSection.DeckUI.LocalPosition *= new Vector2(1, -1);
            UIBoardSection.DeckUI.DeckCountLabel.LocalPosition *= new Vector2(1, -1);
        }

        #endregion
    }
}
