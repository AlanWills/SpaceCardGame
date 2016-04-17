using _2DEngine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace SpaceCardGame
{
    /// <summary>
    /// A class used as UI sugar to improve the effects on our battle screen.
    /// </summary>
    public class DamageUI : Image
    {
        #region Properties and Fields

        /// <summary>
        /// A cached list of all the available damage assets - used for choosing a random one
        /// </summary>
        private static List<string> DamageAssets = new List<string>()
        {
            "Effects\\Damage\\DamageBurns0",
            "Effects\\Damage\\DamageBurns1",
            "Effects\\Damage\\DamageCracks0",
            "Effects\\Damage\\DamageCracks1",
            "Effects\\Damage\\DamageHoles0",
            "Effects\\Damage\\DamageHoles1",
        };

        #endregion

        public DamageUI(Vector2 localPosition, string textureAsset = AssetManager.DefaultEmptyPanelTextureAsset) :
            base(localPosition, DamageAssets[MathUtils.GenerateInt(0, DamageAssets.Count - 1)])
        {

        }
    }
}