using _2DEngine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace SpaceCardGame
{
    /// <summary>
    /// A class which takes in a list of card data assets, or various other data structures and creates a tab control and CardGridControl for each type.
    /// Useful for shop screens, or editing screens where you wish to automatically create all the UI for the various card types.
    /// </summary>
    public class CardTypesTabControl : TabControl
    {
        #region Properties and Fields

        /// <summary>
        /// A dictionary lookup of card type string to list of card data so we can easily get all the card data of one type.
        /// </summary>
        private Dictionary<string, Dictionary<string, CardData>> CardData { get; set; }

        /// <summary>
        /// The size of each card grid control we will make for each card type
        /// </summary>
        private Vector2 CardGridControlSize { get; set; }

        /// <summary>
        /// The OnLeftClicked methods of all the cards in this tab control.
        /// </summary>
        private OnClicked CardOnLeftClicked { get; set; }

        #endregion

        /// <summary>
        /// If you use this constructor, this tab control will possibly remove/add cards from that dictionary as it is only passing in a reference.
        /// This will happen if you have set up click events to move cards to places like in an editing screen.
        /// Just be careful that this dictionary is being passed as a REFERENCE.
        /// </summary>
        /// <param name="cardData"></param>
        /// <param name="size"></param>
        /// <param name="localPosition"></param>
        /// <param name="textureAsset"></param>
        public CardTypesTabControl(Dictionary<string, Dictionary<string, CardData>> cardData, Vector2 tabsTotalSize, Vector2 cardPagesSize, Vector2 localPosition, OnClicked cardOnLeftClicked = null, string textureAsset = AssetManager.DefaultEmptyPanelTextureAsset) :
            base(tabsTotalSize, localPosition, textureAsset)
        {
            CardData = cardData;
            CardGridControlSize = cardPagesSize;
            CardOnLeftClicked = cardOnLeftClicked;
        }

        /// <summary>
        /// Use this constructor if you wish this tab control to create an internal dictionary of CardData.
        /// Edits to this dictionary will be contained within this UI without changing other containers of CardData.
        /// You can sync these changes to affect other containers, but this will have to be done explicitly using events
        /// </summary>
        /// <param name="cardDataAssets"></param>
        /// <param name="tabsTotalSize"></param>
        /// <param name="cardPagesSize"></param>
        /// <param name="localPosition"></param>
        /// <param name="textureAsset"></param>
        public CardTypesTabControl(List<string> cardDataAssets, Vector2 tabsTotalSize, Vector2 cardPagesSize, Vector2 localPosition, OnClicked cardOnLeftClicked = null, string textureAsset = AssetManager.DefaultEmptyPanelTextureAsset) :
            base(tabsTotalSize, localPosition, textureAsset)
        {
            CardData = new Dictionary<string, Dictionary<string, CardData>>();
            CardGridControlSize = cardPagesSize;
            CardOnLeftClicked = cardOnLeftClicked;

            // Set up our data structure before we load the card data
            foreach (string type in CentralCardRegistry.CardTypes)
            {
                CardData.Add(type, new Dictionary<string, CardData>());
            }

            // Now add all the card data to the data structure
            foreach (string cardDataAsset in cardDataAssets)
            {
                CardData data = AssetManager.GetData<CardData>(cardDataAsset);
                CardData[data.Type].Add(cardDataAsset, data);
            }
        }

        #region Virtual Functions

        public override void LoadContent()
        {
            CheckShouldLoad();

            foreach (KeyValuePair<string, Dictionary<string, CardData>> cardData in CardData)
            {
                CardGridControl cardGridControl = new CardGridControl(cardData.Value.Values.ToList(), 4, CardGridControlSize, new Vector2(0, CardGridControlSize.Y * 0.5f));
                cardGridControl.Name = cardData.Key;

                if (CardOnLeftClicked != null)
                {
                    cardGridControl.OnLeftClicked += CardOnLeftClicked;
                }
                else
                {
                    // Empty delegate if no click method specified
                    cardGridControl.OnLeftClicked += delegate { };
                }

                AddChild(cardGridControl);
            }

            base.LoadContent();
        }

        #endregion
    }
}
