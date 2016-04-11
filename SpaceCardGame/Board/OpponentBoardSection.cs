using _2DEngine;
using Microsoft.Xna.Framework;

namespace SpaceCardGame
{
    /// <summary>
    /// A class which functions exactly the same except for some UI fixups so our battle screen is more user-friendly
    /// </summary>
    public class OpponentBoardSection : PlayerBoardSection
    {
        public OpponentBoardSection(GamePlayer player, Vector2 localPosition, string dataAsset = AssetManager.EmptyGameObjectDataAsset) :
            base(player, localPosition, dataAsset)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// Fixes up our UI for the batle screen
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            LocalRotation = MathHelper.Pi;
            GameBoardSection.LocalRotation = MathHelper.Pi;
            GameBoardSection.ShipCardControl.LocalPosition *= new Vector2(1, -1);

            UIBoardSection.LocalRotation = MathHelper.Pi;
            UIBoardSection.HandUI.LocalRotation = MathHelper.Pi;
            UIBoardSection.DeckUI.LocalPosition *= new Vector2(1, -1);
            UIBoardSection.DeckUI.DeckCountLabel.LocalPosition *= new Vector2(1, -1);

            // We should not be able to interact with the opponent's board section
            // Instead we will use an AI script to add things
            // We do this here because the board sections are added to the screen rather than the PlayerBoardSection, because they are of different types and it will not work
            UIBoardSection.ShouldHandleInput.Value = false;
            GameBoardSection.ShouldHandleInput.Value = false;
        }

        #endregion
    }
}
