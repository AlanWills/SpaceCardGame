using _2DEngine;
using Microsoft.Xna.Framework.Content;
using SpaceCardGameData;

namespace SpaceCardGame
{
    /// <summary>
    /// Holds information on the current session including player game data.
    /// </summary>
    public static class SessionManager
    {
        #region Properties and Fields

        public static PlayerGameData PlayerGameData { get; set; }
        
        #endregion

        public static void LoadAssets(ContentManager content)
        {
            DebugUtils.AssertNotNull(AssetManager.OptionsPath);

            bool createIfDoesNotExist = true;
            PlayerGameData = AssetManager.GetData<PlayerGameData>(AssetManager.PlayerGameDataPath, createIfDoesNotExist);
            DebugUtils.AssertNotNull(PlayerGameData);
        }

        public static void SaveAssets()
        {
            DebugUtils.AssertNotNull(AssetManager.OptionsPath);

            if (PlayerGameData != null)
            {
                AssetManager.SaveData(PlayerGameData, AssetManager.PlayerGameDataPath);
            }
        }
    }
}
