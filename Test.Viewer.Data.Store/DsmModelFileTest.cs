using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.Data.Model.Model;
using Dsmviz.Interfaces.Util;
using Dsmviz.Viewer.Data.Model.Entities;
using Dsmviz.Viewer.Data.Model.Facade;
using Dsmviz.Viewer.Data.Store;
using Moq;

namespace Dsmviz.Test.Data.Store
{
    /// <summary>
    /// Dependency matrix used for tests:
    /// -System cycle between a and b
    /// -Hierarchical cycle between a and c
    /// 
    ///        | a           | b           | c           |
    ///        +------+------+------+------+------+------+
    ///        | a1   | a2   | b1   | b2   | c1   | c2   |
    /// --+----+------+------+------+------+------+------+
    ///   | a1 |      |      |      | 1    |      |      |
    /// a +----+------+------+------+------+------+------+
    ///   | a2 |      |      |      | 2    |  4   |      |
    /// -------+------+------+------+------+------+------+
    ///   | b1 | 1000 | 200  |      |      |      |      |
    /// b +----+------+------+------+------+------+------+
    ///   | b2 |  30  | 4    |      |      |      |      |
    /// --+----+------+------+------+------+------+------+
    ///   | c1 |      |      |      |      |      |      |
    /// c +----+------+------+------+------+------+------+
    ///   | c2 |  5   |      |      |      |      |      |
    /// --+----+------+------+------+------+------+------+
    /// </summary>
    [TestClass]
    public class DsmModelFileTest
    {
        private readonly Mock<IFileProgress> _fileProgress = new();

        [TestInitialize()]
        public void MyTestInitialize()
        {
        }

        [TestMethod]
        public void TestLoadModel()
        {
            IDataModel dataModel = new CoreDataModel();
            string inputFile = "Dsmviz.Test.Data.Store.Validation.Input.dsm";
            CoreDsmFile readModelFile = new CoreDsmFile(inputFile,
                dataModel);
            readModelFile.Load(_fileProgress.Object);
            Assert.IsFalse(readModelFile.IsCompressedFile);

            IDictionary<string, List<IMetaDataItem>> metaData = dataModel.MetaDataModelPersistency.GetMetaDataItems();
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

            Element rootElement = dataModel.ElementModelPersistency.GetRootElement() as Element;
            Assert.IsNotNull(rootElement);
            Assert.AreEqual(3, rootElement.ChildrenIncludingDeletedOnes.Count);

            IElement a = rootElement.ChildrenIncludingDeletedOnes[0];
            Assert.AreEqual(11, a.Id);
            Assert.AreEqual(1, a.Order);
            Assert.AreEqual("", a.Type);
            Assert.AreEqual("a", a.Name);
            Assert.AreEqual("a", a.Fullname);
            Assert.IsTrue(a.IsExpanded);
            Assert.IsTrue(a.IsBookmarked);
            Assert.IsNotNull(a.Properties);
            Assert.AreEqual("some element text", a.Properties["annotation"]);
            Assert.AreEqual("1.0", a.Properties["version"]);
            Assert.IsFalse(a.IsDeleted);

            Assert.AreEqual(2, a.ChildrenIncludingDeletedOnes.Count);
            IElement a1 = a.ChildrenIncludingDeletedOnes[0];
            Assert.AreEqual(12, a1.Id);
            Assert.AreEqual(2, a1.Order);
            Assert.AreEqual("eta", a1.Type);
            Assert.AreEqual("a1", a1.Name);
            Assert.AreEqual("a.a1", a1.Fullname);
            Assert.IsFalse(a1.IsExpanded);
            Assert.IsFalse(a1.IsBookmarked);
            Assert.IsNull(a1.Properties);
            Assert.IsFalse(a1.IsDeleted);

            IElement a2 = a.ChildrenIncludingDeletedOnes[1];
            Assert.AreEqual(13, a2.Id);
            Assert.AreEqual(3, a2.Order);
            Assert.AreEqual("eta", a2.Type);
            Assert.AreEqual("a2", a2.Name);
            Assert.AreEqual("a.a2", a2.Fullname);
            Assert.IsFalse(a2.IsExpanded);
            Assert.IsFalse(a2.IsBookmarked);
            Assert.IsNull(a2.Properties);
            Assert.IsFalse(a2.IsDeleted);

            IElement b = rootElement.ChildrenIncludingDeletedOnes[1];
            Assert.AreEqual(14, b.Id);
            Assert.AreEqual(4, b.Order);
            Assert.AreEqual("", b.Type);
            Assert.AreEqual("b", b.Name);
            Assert.AreEqual("b", b.Fullname);
            Assert.IsFalse(b.IsExpanded);
            Assert.IsFalse(b.IsBookmarked);
            Assert.IsNull(b.Properties);
            Assert.IsFalse(b.IsDeleted);

            Assert.AreEqual(2, b.ChildrenIncludingDeletedOnes.Count);
            IElement b1 = b.ChildrenIncludingDeletedOnes[0];
            Assert.AreEqual(15, b1.Id);
            Assert.AreEqual(5, b1.Order);
            Assert.AreEqual("etb", b1.Type);
            Assert.AreEqual("b1", b1.Name);
            Assert.AreEqual("b.b1", b1.Fullname);
            Assert.IsFalse(b1.IsExpanded);
            Assert.IsFalse(b1.IsBookmarked);
            Assert.IsNull(b1.Properties);
            Assert.IsFalse(b1.IsDeleted);

            IElement b2 = b.ChildrenIncludingDeletedOnes[1];
            Assert.AreEqual(16, b2.Id);
            Assert.AreEqual(6, b2.Order);
            Assert.AreEqual("etb", b2.Type);
            Assert.AreEqual("b2", b2.Name);
            Assert.AreEqual("b.b2", b2.Fullname);
            Assert.IsFalse(b2.IsExpanded);
            Assert.IsFalse(b2.IsBookmarked);
            Assert.IsNull(b2.Properties);
            Assert.IsFalse(b2.IsDeleted);

            IElement c = rootElement.ChildrenIncludingDeletedOnes[2];
            Assert.AreEqual(17, c.Id);
            Assert.AreEqual(7, c.Order);
            Assert.AreEqual("", c.Type);
            Assert.AreEqual("c", c.Name);
            Assert.AreEqual("c", c.Fullname);
            Assert.IsFalse(c.IsExpanded);
            Assert.IsFalse(c.IsBookmarked);
            Assert.IsNull(c.Properties);
            Assert.IsFalse(c.IsDeleted);

            Assert.AreEqual(2, c.ChildrenIncludingDeletedOnes.Count);
            IElement c1 = c.ChildrenIncludingDeletedOnes[0];
            Assert.AreEqual(18, c1.Id);
            Assert.AreEqual(8, c1.Order);
            Assert.AreEqual("etc", c1.Type);
            Assert.AreEqual("c1", c1.Name);
            Assert.AreEqual("c.c1", c1.Fullname);
            Assert.IsFalse(c1.IsExpanded);
            Assert.IsFalse(c1.IsBookmarked);
            Assert.IsNull(c1.Properties);
            Assert.IsFalse(c1.IsDeleted);

            IElement c2 = c.ChildrenIncludingDeletedOnes[1];
            Assert.AreEqual(19, c2.Id);
            Assert.AreEqual(9, c2.Order);
            Assert.AreEqual("etc", c2.Type);
            Assert.AreEqual("c2", c2.Name);
            Assert.AreEqual("c.c2", c2.Fullname);
            Assert.IsFalse(c2.IsExpanded);
            Assert.IsTrue(c2.IsBookmarked);
            Assert.IsNull(c2.Properties);
            Assert.IsFalse(c2.IsDeleted);

            List<IRelation> relations = dataModel.RelationModelPersistency.GetPersistedRelations().OrderBy(r => r.Id).ToList();
            Assert.IsNotNull(relations);

            Assert.AreEqual(91, relations[0].Id);
            Assert.AreEqual(a1.Id, relations[0].Consumer.Id);
            Assert.AreEqual(b1.Id, relations[0].Provider.Id);
            Assert.AreEqual("ra", relations[0].Type);
            Assert.AreEqual(1000, relations[0].Weight);
            Assert.IsNotNull(relations[0].Properties);
            Assert.AreEqual("some relation text", relations[0].Properties["annotation"]);
            Assert.AreEqual("1.1", relations[0].Properties["version"]);
            Assert.IsFalse(relations[0].IsDeleted);

            Assert.AreEqual(92, relations[1].Id);
            Assert.AreEqual(a2.Id, relations[1].Consumer.Id);
            Assert.AreEqual(b1.Id, relations[1].Provider.Id);
            Assert.AreEqual("ra", relations[1].Type);
            Assert.AreEqual(200, relations[1].Weight);
            Assert.IsNull(relations[1].Properties);
            Assert.IsFalse(relations[1].IsDeleted);

            Assert.AreEqual(93, relations[2].Id);
            Assert.AreEqual(a1.Id, relations[2].Consumer.Id);
            Assert.AreEqual(b2.Id, relations[2].Provider.Id);
            Assert.AreEqual("ra", relations[2].Type);
            Assert.AreEqual(30, relations[2].Weight);
            Assert.IsNull(relations[2].Properties);
            Assert.IsFalse(relations[2].IsDeleted);

            Assert.AreEqual(94, relations[3].Id);
            Assert.AreEqual(a2.Id, relations[3].Consumer.Id);
            Assert.AreEqual(b2.Id, relations[3].Provider.Id);
            Assert.AreEqual("ra", relations[3].Type);
            Assert.AreEqual(4, relations[3].Weight);
            Assert.IsNull(relations[3].Properties);
            Assert.IsFalse(relations[3].IsDeleted);

            Assert.AreEqual(95, relations[4].Id);
            Assert.AreEqual(a1.Id, relations[4].Consumer.Id);
            Assert.AreEqual(c2.Id, relations[4].Provider.Id);
            Assert.AreEqual("ra", relations[4].Type);
            Assert.AreEqual(5, relations[4].Weight);
            Assert.IsNull(relations[4].Properties);
            Assert.IsFalse(relations[4].IsDeleted);

            Assert.AreEqual(96, relations[5].Id);
            Assert.AreEqual(b2.Id, relations[5].Consumer.Id);
            Assert.AreEqual(a1.Id, relations[5].Provider.Id);
            Assert.AreEqual("rb", relations[5].Type);
            Assert.AreEqual(1, relations[5].Weight);
            Assert.IsNull(relations[5].Properties);
            Assert.IsFalse(relations[5].IsDeleted);

            Assert.AreEqual(97, relations[6].Id);
            Assert.AreEqual(b2.Id, relations[6].Consumer.Id);
            Assert.AreEqual(a2.Id, relations[6].Provider.Id);
            Assert.AreEqual("rb", relations[6].Type);
            Assert.AreEqual(2, relations[6].Weight);
            Assert.IsNull(relations[6].Properties);
            Assert.IsFalse(relations[6].IsDeleted);

            Assert.AreEqual(98, relations[7].Id);
            Assert.AreEqual(c1.Id, relations[7].Consumer.Id);
            Assert.AreEqual(a2.Id, relations[7].Provider.Id);
            Assert.AreEqual("rc", relations[7].Type);
            Assert.AreEqual(4, relations[7].Weight);
            Assert.IsNull(relations[7].Properties);
            Assert.IsFalse(relations[7].IsDeleted);
        }

        [TestMethod]
        public void TestSaveModel()
        {
            IDataModel dataModel = new CoreDataModel();

            string inputFile = "Dsmviz.Test.Data.Store.Validation.Input.dsm";
            string outputFile = "Dsmviz.Test.Data.Store.Validation.Output.dsm";

            FillModelData(dataModel);

            CoreDsmFile writtenModelFile = new CoreDsmFile(outputFile,
                dataModel);

            writtenModelFile.Save(false, _fileProgress.Object);
            Assert.IsFalse(writtenModelFile.IsCompressedFile);

            Assert.IsTrue(File.ReadAllBytes(outputFile).SequenceEqual(File.ReadAllBytes(inputFile)));
        }

        private void FillModelData(IDataModel dataModel)
        {
            dataModel.MetaDataModelPersistency.ImportMetaDataItem("group1", "item1", "value1");
            dataModel.MetaDataModelPersistency.ImportMetaDataItem("group1", "item2", "value2");
            dataModel.MetaDataModelPersistency.ImportMetaDataItem("group2", "item3", "value3");
            dataModel.MetaDataModelPersistency.ImportMetaDataItem("group2", "item4", "value4");

            Dictionary<string, string> elementProperties = new Dictionary<string, string>
            {
                ["annotation"] = "some element text",
                ["version"] = "1.0"
            };

            IElement a = dataModel.ElementModelPersistency.ImportElement(11, "a", "", elementProperties, 1, true, true, null);
            IElement a1 = dataModel.ElementModelPersistency.ImportElement(12, "a1", "eta", null, 2, false, false, 11);
            IElement a2 = dataModel.ElementModelPersistency.ImportElement(13, "a2", "eta", null, 3, false, false, 11);
            IElement b = dataModel.ElementModelPersistency.ImportElement(14, "b", "", null, 4, false, false, null);
            IElement b1 = dataModel.ElementModelPersistency.ImportElement(15, "b1", "etb", null, 5, false, false, 14);
            IElement b2 = dataModel.ElementModelPersistency.ImportElement(16, "b2", "etb", null, 6, false, false, 14);
            IElement c = dataModel.ElementModelPersistency.ImportElement(17, "c", "", null, 7, false, false, null);
            IElement c1 = dataModel.ElementModelPersistency.ImportElement(18, "c1", "etc", null, 8, false, false, 17);
            IElement c2 = dataModel.ElementModelPersistency.ImportElement(19, "c2", "etc", null, 9, false, true, 17);

            Dictionary<string, string> relationProperties = new Dictionary<string, string>
            {
                ["annotation"] = "some relation text",
                ["version"] = "1.1"
            };

            dataModel.RelationModelPersistency.ImportRelation(91, a1, b1, "ra", 1000, relationProperties);
            dataModel.RelationModelPersistency.ImportRelation(92, a2, b1, "ra", 200, null);
            dataModel.RelationModelPersistency.ImportRelation(93, a1, b2, "ra", 30, null);
            dataModel.RelationModelPersistency.ImportRelation(94, a2, b2, "ra", 4, null);
            dataModel.RelationModelPersistency.ImportRelation(95, a1, c2, "ra", 5, null);
            dataModel.RelationModelPersistency.ImportRelation(96, b2, a1, "rb", 1, null);
            dataModel.RelationModelPersistency.ImportRelation(97, b2, a2, "rb", 2, null);
            dataModel.RelationModelPersistency.ImportRelation(98, c1, a2, "rc", 4, null);
        }
    }
}
