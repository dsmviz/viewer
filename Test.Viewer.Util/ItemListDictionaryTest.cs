using Dsmviz.Viewer.Utils;

namespace Dsmviz.Test.Util
{
    [TestClass]
    public class ItemListDictionaryTest
    {
        [TestMethod]
        public void WhenCollectionIsConstructed_ThenItsIsEmpty()
        {
            // When
            ItemListDictionary<int, string> collection = new ItemListDictionary<int, string>();

            // Then
            Assert.AreEqual(0, collection.GetTotalItemCount());
        }

        [TestMethod]
        public void GivenCollectionIsEmpty_WhenTwoItemAreAddedToOneList_ThenTwoItemsAreFound()
        {
            // Given
            ItemListDictionary<int, string> collection = new ItemListDictionary<int, string>();

            // When
            collection.AddItem(1, "1a");
            collection.AddItem(1, "1b");

            // Then
            Assert.AreEqual(2, collection.GetTotalItemCount());
            Assert.AreEqual(2, collection.GetItemCount(1));

            List<string> items1 = collection.GetItems(1).OrderBy(x => x).ToList();
            Assert.AreEqual(2, items1.Count);
            Assert.AreEqual("1a", items1[0]);
            Assert.AreEqual("1b", items1[1]);
        }

        [TestMethod]
        public void GivenCollectionIsEmpty_WhenFiveItemAreAddedToTwoLists_ThenFiveItemsAreFound()
        {
            // Given
            ItemListDictionary<int, string> collection = new ItemListDictionary<int, string>();

            // When
            collection.AddItem(1, "1a");
            collection.AddItem(1, "1b");
            collection.AddItem(2, "2a");
            collection.AddItem(2, "2b");
            collection.AddItem(2, "2c");

            // Then
            Assert.AreEqual(5, collection.GetTotalItemCount());
            Assert.AreEqual(2, collection.GetItemCount(1));
            Assert.AreEqual(3, collection.GetItemCount(2));

            List<string> items1 = collection.GetItems(1).OrderBy(x => x).ToList();
            Assert.AreEqual(2, items1.Count);
            Assert.AreEqual("1a", items1[0]);
            Assert.AreEqual("1b", items1[1]);

            List<string> items2 = collection.GetItems(2).OrderBy(x => x).ToList();
            Assert.AreEqual(3, items2.Count);
            Assert.AreEqual("2a", items2[0]);
            Assert.AreEqual("2b", items2[1]);
            Assert.AreEqual("2c", items2[2]);
        }

        [TestMethod]
        public void GivenCollectionIsNotEmpty_WhenFirstItemOfListIsRemoved_ThenItsIsNotFoundAnymore()
        {
            // Given
            ItemListDictionary<int, string> collection = new ItemListDictionary<int, string>();
            collection.AddItem(1, "1a");
            collection.AddItem(1, "1b");
            Assert.AreEqual(2, collection.GetItemCount(1));
            Assert.AreEqual(2, collection.GetTotalItemCount());

            // When
            collection.RemoveItem(1, "1a");

            // Then
            Assert.AreEqual(1, collection.GetTotalItemCount());
            Assert.AreEqual(1, collection.GetTotalItemCount());

            List<string> items1 = collection.GetItems(1).OrderBy(x => x).ToList();
            Assert.AreEqual(1, items1.Count);
            Assert.AreEqual("1b", items1[0]);
        }

        [TestMethod]
        public void GivenCollectionIsNotEmpty_WhenLastItemOfListIsRemoved_ThenItsIsNotFoundAnymore()
        {
            // Given
            ItemListDictionary<int, string> collection = new ItemListDictionary<int, string>();
            collection.AddItem(1, "1a");
            collection.AddItem(1, "1b");
            Assert.AreEqual(2, collection.GetItemCount(1));
            Assert.AreEqual(2, collection.GetTotalItemCount());

            // When
            collection.RemoveItem(1, "1a");
            collection.RemoveItem(1, "1b");

            // Then
            Assert.AreEqual(0, collection.GetTotalItemCount());
            Assert.AreEqual(0, collection.GetTotalItemCount());

            List<string> items1 = collection.GetItems(1).OrderBy(x => x).ToList();
            Assert.AreEqual(0, items1.Count);
        }

        [TestMethod]
        public void GivenCollectionIsNotEmpty_WhenItemWithUnknownKeyIsRemoved_ThenNoChangeIsObserved()
        {
            // Given
            ItemListDictionary<int, string> collection = new ItemListDictionary<int, string>();
            collection.AddItem(1, "1a");
            collection.AddItem(1, "1b");
            Assert.AreEqual(2, collection.GetItemCount(1));
            Assert.AreEqual(2, collection.GetTotalItemCount());

            // When
            collection.RemoveItem(3, "1a");

            // Then
            Assert.AreEqual(2, collection.GetItemCount(1));
            Assert.AreEqual(2, collection.GetTotalItemCount());

            List<string> items1 = collection.GetItems(1).OrderBy(x => x).ToList();
            Assert.AreEqual(2, items1.Count);
            Assert.AreEqual("1a", items1[0]);
            Assert.AreEqual("1b", items1[1]);
        }

        [TestMethod]
        public void GivenCollectionIsNotEmpty_WhenAllItemUnderKeyAreRemoved_ThenNoItemsAreFoundForThatKey()
        {
            // Given
            ItemListDictionary<int, string> collection = new ItemListDictionary<int, string>();
            collection.AddItem(1, "1a");
            collection.AddItem(1, "1b");
            collection.AddItem(2, "2a");
            collection.AddItem(2, "2b");
            collection.AddItem(2, "2c");
            Assert.AreEqual(5, collection.GetTotalItemCount());
            Assert.AreEqual(2, collection.GetItemCount(1));
            Assert.AreEqual(3, collection.GetItemCount(2));

            // When
            collection.RemoveItems(2);

            // Then
            Assert.AreEqual(2, collection.GetTotalItemCount());
            Assert.AreEqual(2, collection.GetItemCount(1));
            Assert.AreEqual(0, collection.GetItemCount(2));

            List<string> items1 = collection.GetItems(1).OrderBy(x => x).ToList();
            Assert.AreEqual(2, items1.Count);
            Assert.AreEqual("1a", items1[0]);
            Assert.AreEqual("1b", items1[1]);

            List<string> items2 = collection.GetItems(2).OrderBy(x => x).ToList();
            Assert.AreEqual(0, items2.Count);
        }

        [TestMethod]
        public void GivenCollectionIsNotEmpty_WhenItIsClear_ThenItsIsEmpty()
        {
            // Given
            ItemListDictionary<int, string> collection = new ItemListDictionary<int, string>();
            collection.AddItem(1, "1a");
            Assert.AreEqual(1, collection.GetTotalItemCount());

            // When
            collection.Clear();

            // Then
            Assert.AreEqual(0, collection.GetTotalItemCount());
        }
    }
}
