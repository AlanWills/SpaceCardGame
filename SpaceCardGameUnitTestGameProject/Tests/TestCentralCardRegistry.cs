using _2DEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceCardGame;
using UnitTestFramework;

namespace SpaceCardGameUnitTestGameProject
{
    [TestClass]
    public class TestCentralCardRegistry : UnitTest
    {
        public TestCentralCardRegistry()
        {
            CentralCardRegistry.LoadAssets(ScreenManager.Instance.Content);
        }

        [TestMethod]
        public void TestFindCardDataAsset_Success()
        {
            string expectedCardDataAsset = "TestCardData.xml";
            CardData cardData = AssetManager.GetData<CardData>("Cards\\" + expectedCardDataAsset);

            string actualCardDataAsset = CentralCardRegistry.FindCardDataAsset(cardData);

            Assert.AreEqual(expectedCardDataAsset, actualCardDataAsset);
        }

        [TestMethod]
        public void TestFindCardDataAsset_PassNull_Failure()
        {
            string expectedCardDataAsset = "";
            string actualCardDataAsset = CentralCardRegistry.FindCardDataAsset(null);

            Assert.AreEqual(expectedCardDataAsset, actualCardDataAsset);
        }

        [TestMethod]
        public void TestFindCardDataAsset_PassUnregisteredCardData_Failure()
        {
            // Clear all the ship card data for this test (we will load them again at the end of this test
            CentralCardRegistry.CardData["Ship"].Clear();

            string expectedCardDataAsset = "";
            CardData cardData = AssetManager.GetData<CardData>("Cards\\TestCardData.xml");

            string actualCardDataAsset = CentralCardRegistry.FindCardDataAsset(cardData);

            Assert.AreEqual(expectedCardDataAsset, actualCardDataAsset);
        }
    }
}
