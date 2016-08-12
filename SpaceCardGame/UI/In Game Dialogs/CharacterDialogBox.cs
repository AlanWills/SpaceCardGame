using _2DEngine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace SpaceCardGame
{
    /// <summary>
    /// A text box which also displays a character portrait too
    /// </summary>
    public class CharacterDialogBox : TextDialogBox
    {
        public CharacterDialogBox(string characterPortraitTextureAsset, string text, string title, Vector2 localPosition, string textureAsset = AssetManager.DefaultMenuTextureAsset) :
            this(characterPortraitTextureAsset, new string[1] { text }, title, localPosition, textureAsset)
        {

        }

        public CharacterDialogBox(string characterPortraitTextureAsset, List<string> strings, string title, Vector2 localPosition, string textureAsset = AssetManager.DefaultMenuTextureAsset) :
            this(characterPortraitTextureAsset, strings.ToArray(), title, localPosition, textureAsset)
        {

        }

        public CharacterDialogBox(string characterPortraitTextureAsset, string[] strings, string title, Vector2 localPosition, string textureAsset = AssetManager.DefaultMenuTextureAsset) :
            base(strings, title, localPosition, textureAsset)
        {
            AddChild(new Image(Anchor.kCentreLeft, 1, characterPortraitTextureAsset));
        }
    }
}
