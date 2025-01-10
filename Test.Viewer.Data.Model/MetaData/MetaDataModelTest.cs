

using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Viewer.Data.Model.MetaData;

namespace Dsmviz.Test.Data.Model.MetaData
{
    [TestClass]
    public class MetaDataModelTest
    {
        [TestMethod]
        public void GivenItemNameNotUsedBefore_WhenAddMetaDataItemIsCalled_ThenItemIsAdded()
        {
            string groupName = "GroupName";

            MetaDataModel model = new MetaDataModel();
            List<IMetaDataItem> itemsBefore = model.GetMetaDataGroupItems(groupName).ToList();
            Assert.AreEqual(0, itemsBefore.Count);

            string itemName = "SomeItemName";
            string itemValue = "SomeItemValue";
            model.AddMetaDataItem(groupName, itemName, itemValue);

            List<IMetaDataItem> itemsAfter = model.GetMetaDataGroupItems(groupName).ToList();
            Assert.AreEqual(1, itemsAfter.Count);
            Assert.AreEqual(itemName, itemsAfter[0].Name);
            Assert.AreEqual(itemValue, itemsAfter[0].Value);
        }

        [TestMethod]
        public void GivenItemNameUsedBefore_WhenAddMetaDataItemToDefaultGroupIsCalled_ThenItemIsUpdated()
        {
            string groupName = "GroupName";

            MetaDataModel model = new MetaDataModel();
            List<IMetaDataItem> itemsBefore = model.GetMetaDataGroupItems(groupName).ToList();
            Assert.AreEqual(0, itemsBefore.Count);

            string itemName = "SomeItemName";
            string itemValue1 = "SomeItemValue1";
            model.AddMetaDataItem(groupName, itemName, itemValue1);

            string itemValue2 = "SomeItemValue2";
            model.AddMetaDataItem(groupName, itemName, itemValue2);

            List<IMetaDataItem> itemsAfter = model.GetMetaDataGroupItems(groupName).ToList();
            Assert.AreEqual(1, itemsAfter.Count);
            Assert.AreEqual(itemName, itemsAfter[0].Name);
            Assert.AreEqual(itemValue2, itemsAfter[0].Value);
        }

        [TestMethod]
        public void WhenAddMetaDataItemIsCalled_ThenGroupIsAdded()
        {
            MetaDataModel model = new MetaDataModel();
            List<string> groupsBefore = model.GetMetaDataGroups().ToList();
            Assert.AreEqual(0, groupsBefore.Count);

            string groupName = "GroupName";
            string itemName = "SomeItemName";
            string itemValue = "SomeItemValue1";
            model.AddMetaDataItem(groupName, itemName, itemValue);

            List<string> groupsAfter = model.GetMetaDataGroups().ToList();
            Assert.AreEqual(1, groupsAfter.Count);
            Assert.AreEqual(groupName, groupsAfter[0]);
        }

        [TestMethod]
        public void WhenAddMetaDataItemIsCalled_ThenItemIsAdded()
        {
            MetaDataModel model = new MetaDataModel();

            string groupName = "ImportedGroupName";
            string name = "SomeItemName";
            string value = "SomeItemValue1";
            model.AddMetaDataItem(groupName, name, value);

            List<IMetaDataItem> itemsAfter = model.GetMetaDataGroupItems(groupName).ToList();
            Assert.AreEqual(1, itemsAfter.Count);
            Assert.AreEqual(name, itemsAfter[0].Name);
            Assert.AreEqual(value, itemsAfter[0].Value);
        }


        [TestMethod]
        public void WhenAddMetaDataItemIsCalledTwice_ThenTwoItemIAreAddedAndOrderIsMaintained()
        {
            MetaDataModel model = new MetaDataModel();

            string groupName = "ImportedGroupName";
            string itemName1 = "SomeItemName1";
            string itemValue1 = "SomeItemValue2";
            model.AddMetaDataItem(groupName, itemName1, itemValue1);
            string itemName2 = "SomeItemName";
            string itemValue2 = "SomeItemValue1";
            model.AddMetaDataItem(groupName, itemName2, itemValue2);

            List<IMetaDataItem> itemsAfter = model.GetMetaDataGroupItems(groupName).ToList();
            Assert.AreEqual(2, itemsAfter.Count);
            Assert.AreEqual(itemName1, itemsAfter[0].Name);
            Assert.AreEqual(itemValue1, itemsAfter[0].Value);
            Assert.AreEqual(itemName2, itemsAfter[1].Name);
            Assert.AreEqual(itemValue2, itemsAfter[1].Value);
        }
    }
}

