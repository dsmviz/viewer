
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.Data.Model.Model;
using Dsmviz.Interfaces.Util;
using Dsmviz.Viewer.Data.Import.Dsi;
using Dsmviz.Viewer.Data.Model.Entities;
using Dsmviz.Viewer.Data.Model.Facade;
using Moq;

namespace Test.Viewer.Data.Import.Dsi
{
    [TestClass]
    public class DsiModelFileTest
    {
        private readonly Mock<IFileProgress> _fileProgress = new();
        private readonly IDataModel _dataModel = new CoreDataModel();

        [TestInitialize]
        public void TestInitialize()
        {
        }

        [TestMethod]
        public void GivenDataModelIsClean_WhenModelIsImported_ThenModelIsFilled()
        {
            string inputFilename = "Test.Viewer.Data.Import.Dsi.Validation.Input.dsi";

            DsiModel dsiModel = new DsiModel(_dataModel, _dataModel);
            dsiModel.ImportModel(inputFilename, true, _fileProgress.Object);

            IDictionary<string, List<IMetaDataItem>> metaData = _dataModel.MetaDataModelPersistency.GetMetaDataItems();
            Assert.IsNotNull(metaData);

            Assert.AreEqual(2, metaData.Count);

            Assert.AreEqual(2, metaData["group1"].Count);
            Assert.AreEqual("item1", metaData["group1"][0].Name);
            Assert.AreEqual("value1", metaData["group1"][0].Value);
            Assert.AreEqual("item2", metaData["group1"][1].Name);
            Assert.AreEqual("value2", metaData["group1"][1].Value);

            Assert.AreEqual(2, metaData["group2"].Count);
            Assert.AreEqual("item3", metaData["group2"][0].Name);
            Assert.AreEqual("value3", metaData["group2"][0].Value);
            Assert.AreEqual("item4", metaData["group2"][1].Name);
            Assert.AreEqual("value4", metaData["group2"][1].Value);

            Element rootElement = _dataModel.ElementModelPersistency.GetRootElement() as Element;
            Assert.IsNotNull(rootElement);
            Assert.AreEqual(2, rootElement.ChildrenIncludingDeletedOnes.Count);

            List<IElement> rootElementChildren = rootElement.Children.OrderBy(r => r.Id).ToList();
            Assert.AreEqual(2, rootElementChildren.Count);

            int aAssignedId = 1;
            int a1AssignedId = 2;
            int a2AssignedId = 3;
            int bAssignedId = 4;
            int b1AssignedId = 5;

            IElement a = rootElementChildren[0];
            Assert.AreEqual(aAssignedId, a.Id);
            Assert.AreEqual(1, a.Order);
            Assert.AreEqual("", a.Type);
            Assert.AreEqual("a", a.Name);
            Assert.AreEqual("a", a.Fullname);
            Assert.IsFalse(a.IsExpanded);
            Assert.IsFalse(a.IsBookmarked);
            Assert.IsNull(a.Properties);
            Assert.IsFalse(a.IsDeleted);

            List<IElement> aElementChildren = a.Children.OrderBy(r => r.Id).ToList();
            Assert.AreEqual(2, aElementChildren.Count);

            IElement a1 = aElementChildren[0];
            Assert.AreEqual(a1AssignedId, a1.Id);
            Assert.AreEqual(3, a1.Order);
            Assert.AreEqual("eta1", a1.Type);
            Assert.AreEqual("a1", a1.Name);
            Assert.AreEqual("a.a1", a1.Fullname);
            Assert.IsFalse(a1.IsExpanded);
            Assert.IsFalse(a1.IsBookmarked);
            Assert.IsNotNull(a1.Properties);
            Assert.AreEqual(2, a1.Properties.Count);
            Assert.AreEqual("some element text", a1.Properties["annotation"]);
            Assert.AreEqual("1.0", a1.Properties["version"]);

            Assert.IsFalse(a1.IsDeleted);

            IElement a2 = aElementChildren[1];
            Assert.AreEqual(a2AssignedId, a2.Id);
            Assert.AreEqual(2, a2.Order);
            Assert.AreEqual("eta2", a2.Type);
            Assert.AreEqual("a2", a2.Name);
            Assert.AreEqual("a.a2", a2.Fullname);
            Assert.IsFalse(a2.IsExpanded);
            Assert.IsFalse(a2.IsBookmarked);
            Assert.IsNull(a2.Properties);
            Assert.IsFalse(a2.IsDeleted);

            IElement b = rootElementChildren[1];
            Assert.AreEqual(bAssignedId, b.Id);
            Assert.AreEqual(4, b.Order);
            Assert.AreEqual("", b.Type);
            Assert.AreEqual("b", b.Name);
            Assert.AreEqual("b", b.Fullname);
            Assert.IsFalse(b.IsExpanded);
            Assert.IsFalse(b.IsBookmarked);
            Assert.IsNull(b.Properties);
            Assert.IsFalse(b.IsDeleted);

            List<IElement> bElementChildren = b.Children.OrderBy(r => r.Id).ToList();
            Assert.AreEqual(1, bElementChildren.Count);

            IElement b1 = bElementChildren[0];
            Assert.AreEqual(b1AssignedId, b1.Id);
            Assert.AreEqual(5, b1.Order);
            Assert.AreEqual("etb1", b1.Type);
            Assert.AreEqual("b1", b1.Name);
            Assert.AreEqual("b.b1", b1.Fullname);
            Assert.IsFalse(b1.IsExpanded);
            Assert.IsFalse(b1.IsBookmarked);
            Assert.IsNull(b1.Properties);
            Assert.IsFalse(b1.IsDeleted);

            List<IRelation> relations =
                _dataModel.RelationModelPersistency.GetPersistedRelations().OrderBy(r => r.Id).ToList();
            Assert.IsNotNull(relations);
            Assert.AreEqual(2, relations.Count);

            Assert.AreEqual(1, relations[0].Id);
            Assert.AreEqual(a1AssignedId, relations[0].Consumer.Id);
            Assert.AreEqual(a2AssignedId, relations[0].Provider.Id);
            Assert.AreEqual("ra", relations[0].Type);
            Assert.AreEqual(100, relations[0].Weight);
            Assert.IsNotNull(relations[0].Properties);
            Assert.AreEqual("some relation text", relations[0].Properties["annotation"]);
            Assert.AreEqual("1.1", relations[0].Properties["version"]);
            Assert.IsFalse(relations[0].IsDeleted);

            Assert.AreEqual(2, relations[1].Id);
            Assert.AreEqual(a2AssignedId, relations[1].Consumer.Id);
            Assert.AreEqual(b1AssignedId, relations[1].Provider.Id);
            Assert.AreEqual("rb", relations[1].Type);
            Assert.AreEqual(200, relations[1].Weight);
            Assert.IsNull(relations[1].Properties);
            Assert.IsFalse(relations[1].IsDeleted);
        }

        //[TestMethod]
        //public void GivenDataModelIsFilled_WhenModelIsImported_ThenModelIsUpdated()
        //{
        //    Assert.Inconclusive();
        //}
    }
}
