
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Viewer.Data.Model.Elements;
using Dsmviz.Viewer.Data.Model.Relations;

namespace Test.Viewer.Data.Model.Relations
{
    [TestClass]
    public class RelationModelTest
    {
        private RelationModel _relationModel;
        private ElementModel _elementsDataModel;

        private IElement _a;
        private IElement _b;
        private IElement _c;
        private int _relationAddedEventCount;
        private int _relationRemovedEventCount;

        [TestInitialize]
        public void TestInitialize()
        {
            _relationAddedEventCount = 0;
            _relationRemovedEventCount = 0;

            _relationModel = new RelationModel();
            _relationModel.RelationAdded += OnRelationAdded;
            _relationModel.RelationRemoved += OnRelationRemoved;

            _elementsDataModel = new ElementModel(_relationModel);

            _relationModel.Clear();

            CreateElementHierarchy();
        }

        private void OnRelationAdded(object sender, IRelation e)
        {
            _relationAddedEventCount++;
        }

        private void OnRelationRemoved(object sender, IRelation e)
        {
            _relationRemovedEventCount++;
        }

        [TestMethod]
        public void WhenModelIsConstructed_ThenItIsEmpty()
        {
            Assert.AreEqual(0, _relationModel.GetRelationCount());
        }

        [TestMethod]
        public void GivenNoRelationsExist_WhenThreeRelationsAreImported_ThenRelationCountIsThree()
        {
            // Given
            Assert.AreEqual(0, _relationModel.GetRelationCount());

            // When
            IRelation relation1 = _relationModel.ImportRelation(1, _a, _b, "typeab", 11, null);
            Assert.IsNotNull(relation1);

            IRelation relation2 = _relationModel.ImportRelation(2, _b, _c, "typebc", 22, null);
            Assert.IsNotNull(relation2);

            IRelation relation3 = _relationModel.ImportRelation(3, _c, _a, "typeca", 33, null);
            Assert.IsNotNull(relation3);

            // Then
            Assert.AreEqual(3, _relationModel.GetRelationCount());
        }

        [TestMethod]
        public void GivenNoRelationsExist_WhenThreeRelationsAreImported_ThenThreeRelationAddedEventsAreTriggered()
        {
            // Given
            Assert.AreEqual(0, _relationAddedEventCount);

            // When
            IRelation relation1 = _relationModel.ImportRelation(1, _a, _b, "typeab", 11, null);
            Assert.IsNotNull(relation1);

            IRelation relation2 = _relationModel.ImportRelation(2, _b, _c, "typebc", 22, null);
            Assert.IsNotNull(relation2);

            IRelation relation3 = _relationModel.ImportRelation(3, _c, _a, "typeca", 33, null);
            Assert.IsNotNull(relation3);

            // Then
            Assert.AreEqual(3, _relationAddedEventCount);
        }


        [TestMethod]
        public void GivenNoRelationsExist_WhenThreeRelationsAreImported_ThenRelationsCanBeFoundById()
        {
            // Given
            Assert.IsNull(_relationModel.GetRelationById(1));
            Assert.IsNull(_relationModel.GetRelationById(2));
            Assert.IsNull(_relationModel.GetRelationById(3));

            // When
            IRelation relation1 = _relationModel.ImportRelation(1, _a, _b, "typeab", 11, null);
            Assert.IsNotNull(relation1);

            IRelation relation2 = _relationModel.ImportRelation(2, _b, _c, "typebc", 22, null);
            Assert.IsNotNull(relation2);

            IRelation relation3 = _relationModel.ImportRelation(3, _c, _a, "typeca", 33, null);
            Assert.IsNotNull(relation3);

            // Then
            Assert.AreEqual(relation1, _relationModel.GetRelationById(1));
            Assert.AreEqual(relation2, _relationModel.GetRelationById(2));
            Assert.AreEqual(relation3, _relationModel.GetRelationById(3));
        }


        [TestMethod]
        public void GivenNoRelationsExist_WhenThreeRelationsAreImported_ThenRelationsCanBeFoundByConsumerProvider()
        {
            // Given
            Assert.IsNull(_relationModel.FindRelation(_a, _b, "typeab", 11));
            Assert.IsNull(_relationModel.FindRelation(_b, _c, "typebc", 22));
            Assert.IsNull(_relationModel.FindRelation(_c, _a, "typeca", 33));

            // When
            IRelation relation1 = _relationModel.ImportRelation(1, _a, _b, "typeab", 11, null);
            Assert.IsNotNull(relation1);

            IRelation relation2 = _relationModel.ImportRelation(2, _b, _c, "typebc", 22, null);
            Assert.IsNotNull(relation2);

            IRelation relation3 = _relationModel.ImportRelation(3, _c, _a, "typeca", 33, null);
            Assert.IsNotNull(relation3);

            // Then
            Assert.AreEqual(relation1, _relationModel.FindRelation(_a, _b, "typeab", 11));
            Assert.AreEqual(relation2, _relationModel.FindRelation(_b, _c, "typebc", 22));
            Assert.AreEqual(relation3, _relationModel.FindRelation(_c, _a, "typeca", 33));
        }


        [TestMethod]
        public void GivenNoRelationsExist_WhenThreeRelationsAreImported_ThenRelationsCanBeRetrieved()
        {
            // Given
            List<IRelation> relationsBefore = _relationModel.GetRelations().OrderBy(x => x.Id).ToList();
            Assert.AreEqual(0, relationsBefore.Count);

            // When
            IRelation relation1 = _relationModel.ImportRelation(1, _a, _b, "typeab", 11, null);
            Assert.IsNotNull(relation1);

            IRelation relation2 = _relationModel.ImportRelation(2, _b, _c, "typebc", 22, null);
            Assert.IsNotNull(relation2);

            IRelation relation3 = _relationModel.ImportRelation(3, _c, _a, "typeca", 33, null);
            Assert.IsNotNull(relation3);

            // Then
            List<IRelation> relationsAfter = _relationModel.GetRelations().OrderBy(x => x.Id).ToList();
            Assert.AreEqual(3, relationsAfter.Count);

            Assert.AreEqual(1, relationsAfter[0].Id);
            Assert.AreEqual(_a, relationsAfter[0].Consumer);
            Assert.AreEqual(_b, relationsAfter[0].Provider);
            Assert.AreEqual(11, relationsAfter[0].Weight);
            Assert.AreEqual("typeab", relationsAfter[0].Type);

            Assert.AreEqual(2, relationsAfter[1].Id);
            Assert.AreEqual(_b, relationsAfter[1].Consumer);
            Assert.AreEqual(_c, relationsAfter[1].Provider);
            Assert.AreEqual(22, relationsAfter[1].Weight);
            Assert.AreEqual("typebc", relationsAfter[1].Type);

            Assert.AreEqual(3, relationsAfter[2].Id);
            Assert.AreEqual(_c, relationsAfter[2].Consumer);
            Assert.AreEqual(_a, relationsAfter[2].Provider);
            Assert.AreEqual(33, relationsAfter[2].Weight);
            Assert.AreEqual("typeca", relationsAfter[2].Type);
        }

        [TestMethod]
        public void GivenNoRelationsExist_WhenThreeRelationsAreAdded_ThenRelationCountIsThree()
        {
            // Given
            Assert.AreEqual(0, _relationModel.GetRelationCount());

            // When
            IRelation relation1 = _relationModel.AddRelation(_a, _b, "typeab", 11, null);
            Assert.IsNotNull(relation1);

            IRelation relation2 = _relationModel.AddRelation(_b, _c, "typebc", 22, null);
            Assert.IsNotNull(relation2);

            IRelation relation3 = _relationModel.AddRelation(_c, _a, "typeca", 33, null);
            Assert.IsNotNull(relation3);

            // Then
            Assert.AreEqual(3, _relationModel.GetRelationCount());
        }

        [TestMethod]
        public void GivenNoRelationsExist_WhenThreeRelationsAreAdded_ThenThreeRelationAddedEventsAreTriggered()
        {
            // Given
            Assert.AreEqual(0, _relationAddedEventCount);

            // When
            IRelation relation1 = _relationModel.AddRelation(_a, _b, "typeab", 11, null);
            Assert.IsNotNull(relation1);

            IRelation relation2 = _relationModel.AddRelation(_b, _c, "typebc", 22, null);
            Assert.IsNotNull(relation2);

            IRelation relation3 = _relationModel.AddRelation(_c, _a, "typeca", 33, null);
            Assert.IsNotNull(relation3);

            // Then
            Assert.AreEqual(3, _relationAddedEventCount);
        }

        [TestMethod]
        public void GivenNoRelationsExist_WhenThreeRelationsAreAdded_ThenRelationsCanBeFoundById()
        {
            // Given
            Assert.IsNull(_relationModel.GetRelationById(1));
            Assert.IsNull(_relationModel.GetRelationById(2));
            Assert.IsNull(_relationModel.GetRelationById(3));

            // When
            IRelation relation1 = _relationModel.AddRelation(_a, _b, "typeab", 11, null);
            Assert.IsNotNull(relation1);

            IRelation relation2 = _relationModel.AddRelation(_b, _c, "typebc", 22, null);
            Assert.IsNotNull(relation2);

            IRelation relation3 = _relationModel.AddRelation(_c, _a, "typeca", 33, null);
            Assert.IsNotNull(relation3);

            // Then
            Assert.AreEqual(relation1, _relationModel.GetRelationById(1));
            Assert.AreEqual(relation2, _relationModel.GetRelationById(2));
            Assert.AreEqual(relation3, _relationModel.GetRelationById(3));
        }

        [TestMethod]
        public void GivenNoRelationsExist_WhenThreeRelationsAreAdded_ThenRelationsCanBeFoundByConsumerProvider()
        {
            // Given
            Assert.IsNull(_relationModel.FindRelation(_a, _b, "typeab", 11));
            Assert.IsNull(_relationModel.FindRelation(_b, _c, "typebc", 22));
            Assert.IsNull(_relationModel.FindRelation(_c, _a, "typeca", 33));

            // When
            IRelation relation1 = _relationModel.AddRelation(_a, _b, "typeab", 11, null);
            Assert.IsNotNull(relation1);

            IRelation relation2 = _relationModel.AddRelation(_b, _c, "typebc", 22, null);
            Assert.IsNotNull(relation2);

            IRelation relation3 = _relationModel.AddRelation(_c, _a, "typeca", 33, null);
            Assert.IsNotNull(relation3);

            // Then
            Assert.AreEqual(relation1, _relationModel.FindRelation(_a, _b, "typeab", 11));
            Assert.AreEqual(relation2, _relationModel.FindRelation(_b, _c, "typebc", 22));
            Assert.AreEqual(relation3, _relationModel.FindRelation(_c, _a, "typeca", 33));
        }


        [TestMethod]
        public void GivenNoRelationsExist_WhenThreeRelationsAreAdded_ThenRelationsCanBeRetrieved()
        {
            // Given
            List<IRelation> relationsBefore = _relationModel.GetRelations().OrderBy(x => x.Id).ToList();
            Assert.AreEqual(0, relationsBefore.Count);

            // When
            IRelation relation1 = _relationModel.AddRelation(_a, _b, "typeab", 11, null);
            Assert.IsNotNull(relation1);

            IRelation relation2 = _relationModel.AddRelation(_b, _c, "typebc", 22, null);
            Assert.IsNotNull(relation2);

            IRelation relation3 = _relationModel.AddRelation(_c, _a, "typeca", 33, null);
            Assert.IsNotNull(relation3);

            // Then
            List<IRelation> relationsAfter = _relationModel.GetRelations().OrderBy(x => x.Id).ToList();
            Assert.AreEqual(3, relationsAfter.Count);

            Assert.AreEqual(1, relationsAfter[0].Id);
            Assert.AreEqual(_a, relationsAfter[0].Consumer);
            Assert.AreEqual(_b, relationsAfter[0].Provider);
            Assert.AreEqual(11, relationsAfter[0].Weight);
            Assert.AreEqual("typeab", relationsAfter[0].Type);

            Assert.AreEqual(2, relationsAfter[1].Id);
            Assert.AreEqual(_b, relationsAfter[1].Consumer);
            Assert.AreEqual(_c, relationsAfter[1].Provider);
            Assert.AreEqual(22, relationsAfter[1].Weight);
            Assert.AreEqual("typebc", relationsAfter[1].Type);

            Assert.AreEqual(3, relationsAfter[2].Id);
            Assert.AreEqual(_c, relationsAfter[2].Consumer);
            Assert.AreEqual(_a, relationsAfter[2].Provider);
            Assert.AreEqual(33, relationsAfter[2].Weight);
            Assert.AreEqual("typeca", relationsAfter[2].Type);
        }

        [TestMethod]
        public void GivenRelationExist_WhenRelationBetweenSameConsumerAndProviderIsAdded_ThenNewRelationIsAdded()
        {
            Assert.AreEqual(0, _relationModel.GetRelationCount());

            IRelation relation1 = _relationModel.AddRelation(_a, _b, "typeab1", 11, null);
            Assert.IsNotNull(relation1);
            Assert.AreEqual(1, _relationAddedEventCount);

            Assert.AreEqual(1, _relationModel.GetRelationCount());

            Assert.AreEqual(relation1, _relationModel.GetRelationById(1));

            List<IRelation> relationsBefore = _relationModel.GetRelations().OrderBy(x => x.Id).ToList();
            Assert.AreEqual(1, relationsBefore.Count);

            Assert.AreEqual(1, relationsBefore[0].Id);
            Assert.AreEqual(_a, relationsBefore[0].Consumer);
            Assert.AreEqual(_b, relationsBefore[0].Provider);

            IRelation relation2 = _relationModel.AddRelation(_a, _b, "typeab2", 22, null);
            Assert.IsNotNull(relation2);

            Assert.AreEqual(2, _relationModel.GetRelationCount());

            Assert.AreEqual(relation1, _relationModel.GetRelationById(1));
            Assert.AreEqual(relation2, _relationModel.GetRelationById(2));

            List<IRelation> relationsAfter = _relationModel.GetRelations().OrderBy(x => x.Id).ToList();
            Assert.AreEqual(2, relationsAfter.Count);

            Assert.AreEqual(1, relationsBefore[0].Id);
            Assert.AreEqual(_a, relationsBefore[0].Consumer);
            Assert.AreEqual(_b, relationsBefore[0].Provider);

            Assert.AreEqual(2, relationsAfter[1].Id);
            Assert.AreEqual(_a, relationsAfter[1].Consumer);
            Assert.AreEqual(_b, relationsAfter[1].Provider);
        }

        [TestMethod]
        public void GivenTwoRelationsExist_WhenModelIsCleared_ThenNoRelationsExists()
        {
            // Given
            IRelation relation1 = _relationModel.AddRelation(_a, _b, "typeab", 11, null);
            Assert.IsNotNull(relation1);

            IRelation relation2 = _relationModel.AddRelation(_b, _c, "typebc", 22, null);
            Assert.IsNotNull(relation2);

            Assert.AreEqual(2, _relationModel.GetRelationCount());

            // When
            _relationModel.Clear();

            // Then
            Assert.AreEqual(0, _relationModel.GetRelationCount());
        }

        [TestMethod]
        public void GivenTwoRelationsExist_WhenOneRelationIsRemoved_ThenOneRelationRemovedEventIsTriggered()
        {
            // Given
            Assert.AreEqual(0, _relationRemovedEventCount);

            IRelation relation1 = _relationModel.AddRelation(_a, _b, "typeab", 11, null);
            Assert.IsNotNull(relation1);

            IRelation relation2 = _relationModel.AddRelation(_b, _c, "typebc", 22, null);
            Assert.IsNotNull(relation2);

            Assert.AreEqual(2, _relationModel.GetRelationCount());

            // When
            _relationModel.RemoveRelation(relation1.Id);

            // Then
            Assert.AreEqual(1, _relationRemovedEventCount);
        }

        [TestMethod]
        public void GivenTwoRelationsExist_WhenOneRelationIsRemoved_ThenOneRelationIsExists()
        {
            // Given
            IRelation relation1 = _relationModel.AddRelation(_a, _b, "typeab", 11, null);
            Assert.IsNotNull(relation1);

            IRelation relation2 = _relationModel.AddRelation(_b, _c, "typebc", 22, null);
            Assert.IsNotNull(relation2);

            Assert.AreEqual(2, _relationModel.GetRelationCount());

            Assert.AreEqual(relation1, _relationModel.FindRelation(_a, _b, "typeab", 11));
            Assert.AreEqual(relation2, _relationModel.FindRelation(_b, _c, "typebc", 22));

            List<IRelation> relationsBefore = _relationModel.GetRelations().OrderBy(x => x.Id).ToList();
            Assert.AreEqual(2, relationsBefore.Count);

            Assert.AreEqual(1, relationsBefore[0].Id);
            Assert.AreEqual(_a, relationsBefore[0].Consumer);
            Assert.AreEqual(_b, relationsBefore[0].Provider);
            Assert.AreEqual(11, relationsBefore[0].Weight);
            Assert.AreEqual("typeab", relationsBefore[0].Type);
            Assert.IsFalse(relationsBefore[0].IsDeleted);

            Assert.AreEqual(2, relationsBefore[1].Id);
            Assert.AreEqual(_b, relationsBefore[1].Consumer);
            Assert.AreEqual(_c, relationsBefore[1].Provider);
            Assert.AreEqual(22, relationsBefore[1].Weight);
            Assert.AreEqual("typebc", relationsBefore[1].Type);
            Assert.IsFalse(relationsBefore[1].IsDeleted);

            // When
            _relationModel.RemoveRelation(relation1.Id);
            Assert.AreEqual(1, _relationRemovedEventCount);

            // Then
            Assert.AreEqual(1, _relationModel.GetRelationCount());

            Assert.AreEqual(null, _relationModel.FindRelation(_a, _b, "typeab", 1));
            Assert.AreEqual(relation2, _relationModel.FindRelation(_b, _c, "typebc", 22));

            List<IRelation> relationsAfter = _relationModel.GetRelations().OrderBy(x => x.Id).ToList();
            Assert.AreEqual(1, relationsAfter.Count);

            Assert.AreEqual(2, relationsAfter[0].Id);
            Assert.AreEqual(_b, relationsAfter[0].Consumer);
            Assert.AreEqual(_c, relationsAfter[0].Provider);
            Assert.AreEqual(22, relationsAfter[0].Weight);
            Assert.AreEqual("typebc", relationsAfter[0].Type);
            Assert.IsFalse(relationsAfter[0].IsDeleted);
        }

        [TestMethod]
        public void GivenTwoRelationsExist_WhenOneRelationIsRemoved_ThenStillTwoRelationsIncludingDeletedOnesExist()
        {
            // Given
            IRelation relation1 = _relationModel.AddRelation(_a, _b, "typeab", 11, null);
            Assert.IsNotNull(relation1);

            IRelation relation2 = _relationModel.AddRelation(_b, _c, "typebc", 22, null);
            Assert.IsNotNull(relation2);

            Assert.AreEqual(2, _relationModel.GetRelationCountIncludingDeletedOnes());

            List<IRelation> relationsIncludingDeletedOnesBefore = _relationModel.GetRelationsIncludingDeletedOnes().OrderBy(x => x.Id).ToList();
            Assert.AreEqual(2, relationsIncludingDeletedOnesBefore.Count);

            Assert.AreEqual(1, relationsIncludingDeletedOnesBefore[0].Id);
            Assert.AreEqual(_a, relationsIncludingDeletedOnesBefore[0].Consumer);
            Assert.AreEqual(_b, relationsIncludingDeletedOnesBefore[0].Provider);
            Assert.AreEqual(11, relationsIncludingDeletedOnesBefore[0].Weight);
            Assert.AreEqual("typeab", relationsIncludingDeletedOnesBefore[0].Type);
            Assert.IsFalse(relationsIncludingDeletedOnesBefore[0].IsDeleted);

            Assert.AreEqual(2, relationsIncludingDeletedOnesBefore[1].Id);
            Assert.AreEqual(_b, relationsIncludingDeletedOnesBefore[1].Consumer);
            Assert.AreEqual(_c, relationsIncludingDeletedOnesBefore[1].Provider);
            Assert.AreEqual(22, relationsIncludingDeletedOnesBefore[1].Weight);
            Assert.AreEqual("typebc", relationsIncludingDeletedOnesBefore[1].Type);
            Assert.IsFalse(relationsIncludingDeletedOnesBefore[1].IsDeleted);

            // When
            _relationModel.RemoveRelation(relation1.Id);
            Assert.AreEqual(1, _relationRemovedEventCount);

            // Then
            Assert.AreEqual(2, _relationModel.GetRelationCountIncludingDeletedOnes());

            List<IRelation> relationsIncludingDeletedOnesAfter = _relationModel.GetRelationsIncludingDeletedOnes().OrderBy(x => x.Id).ToList();
            Assert.AreEqual(2, relationsIncludingDeletedOnesBefore.Count);

            Assert.AreEqual(1, relationsIncludingDeletedOnesAfter[0].Id);
            Assert.AreEqual(_a, relationsIncludingDeletedOnesAfter[0].Consumer);
            Assert.AreEqual(_b, relationsIncludingDeletedOnesAfter[0].Provider);
            Assert.AreEqual(11, relationsIncludingDeletedOnesAfter[0].Weight);
            Assert.AreEqual("typeab", relationsIncludingDeletedOnesAfter[0].Type);
            Assert.IsTrue(relationsIncludingDeletedOnesAfter[0].IsDeleted);

            Assert.AreEqual(2, relationsIncludingDeletedOnesAfter[1].Id);
            Assert.AreEqual(_b, relationsIncludingDeletedOnesAfter[1].Consumer);
            Assert.AreEqual(_c, relationsIncludingDeletedOnesAfter[1].Provider);
            Assert.AreEqual(22, relationsIncludingDeletedOnesAfter[1].Weight);
            Assert.AreEqual("typebc", relationsIncludingDeletedOnesAfter[1].Type);
            Assert.IsFalse(relationsIncludingDeletedOnesAfter[1].IsDeleted);
        }

        [TestMethod]
        public void GivenOneRelationExist_WhenOneRelationIsUnremoved_ThenOneRelationAddedEventIsTriggered()
        {
            // Given
            IRelation relation1 = _relationModel.AddRelation(_a, _b, "typeab", 11, null);
            Assert.IsNotNull(relation1);

            IRelation relation2 = _relationModel.AddRelation(_b, _c, "typebc", 22, null);
            Assert.IsNotNull(relation2);

            _relationModel.RemoveRelation(relation1.Id);

            Assert.AreEqual(1, _relationModel.GetRelationCount());

            Assert.AreEqual(2, _relationAddedEventCount);

            // When
            _relationModel.RestoreRelation(relation1.Id);

            // Then
            Assert.AreEqual(3, _relationAddedEventCount);
        }

        [TestMethod]
        public void GivenOneRelationExist_WhenOneRelationIsUnremoved_ThenTwoRelationsIsExist()
        {

            //Given
            IRelation relation1 = _relationModel.AddRelation(_a, _b, "typeab", 11, null);
            Assert.IsNotNull(relation1);

            IRelation relation2 = _relationModel.AddRelation(_b, _c, "typebc", 22, null);
            Assert.IsNotNull(relation2);

            _relationModel.RemoveRelation(relation1.Id);

            Assert.AreEqual(1, _relationModel.GetRelationCount());

            List<IRelation> relationsBefore = _relationModel.GetRelations().OrderBy(x => x.Id).ToList();
            Assert.AreEqual(1, relationsBefore.Count);

            Assert.AreEqual(2, relationsBefore[0].Id);
            Assert.AreEqual(_b, relationsBefore[0].Consumer);
            Assert.AreEqual(_c, relationsBefore[0].Provider);
            Assert.AreEqual(22, relationsBefore[0].Weight);
            Assert.AreEqual("typebc", relationsBefore[0].Type);
            Assert.IsFalse(relationsBefore[0].IsDeleted);

            Assert.AreEqual(null, _relationModel.GetRelationById(1));
            Assert.AreEqual(relation2, _relationModel.GetRelationById(2));

            Assert.AreEqual(null, _relationModel.FindRelation(_a, _b, "typeab", 11));
            Assert.AreEqual(relation2, _relationModel.FindRelation(_b, _c, "typebc", 22));

            // When
            _relationModel.RestoreRelation(relation1.Id);

            // Then
            Assert.AreEqual(2, _relationModel.GetRelationCount());

            List<IRelation> relationsAfter = _relationModel.GetRelations().OrderBy(x => x.Id).ToList();
            Assert.AreEqual(2, relationsAfter.Count);

            Assert.AreEqual(1, relationsAfter[0].Id);
            Assert.AreEqual(_a, relationsAfter[0].Consumer);
            Assert.AreEqual(_b, relationsAfter[0].Provider);
            Assert.AreEqual(11, relationsAfter[0].Weight);
            Assert.AreEqual("typeab", relationsAfter[0].Type);
            Assert.IsFalse(relationsAfter[1].IsDeleted);

            Assert.AreEqual(2, relationsAfter[1].Id);
            Assert.AreEqual(_b, relationsAfter[1].Consumer);
            Assert.AreEqual(_c, relationsAfter[1].Provider);
            Assert.AreEqual(22, relationsAfter[1].Weight);
            Assert.AreEqual("typebc", relationsAfter[1].Type);
            Assert.IsFalse(relationsAfter[1].IsDeleted);

            Assert.AreEqual(relation1, _relationModel.GetRelationById(1));
            Assert.AreEqual(relation2, _relationModel.GetRelationById(2));

            Assert.AreEqual(relation1, _relationModel.FindRelation(_a, _b, "typeab", 11));
            Assert.AreEqual(relation2, _relationModel.FindRelation(_b, _c, "typebc", 22));
        }

        [TestMethod]
        public void GivenOneRelationExist_WhenOneRelationIsUnremoved_ThenTwoRelationsIncludingDeletedOnesExist()
        {
            // Given
            IRelation relation1 = _relationModel.AddRelation(_a, _b, "typeab", 11, null);
            Assert.IsNotNull(relation1);

            IRelation relation2 = _relationModel.AddRelation(_b, _c, "typebc", 22, null);
            Assert.IsNotNull(relation2);

            _relationModel.RemoveRelation(relation1.Id);

            Assert.AreEqual(2, _relationModel.GetRelationCountIncludingDeletedOnes());

            List<IRelation> relationsIncludingDeletedOnesBefore = _relationModel.GetRelationsIncludingDeletedOnes().OrderBy(x => x.Id).ToList();
            Assert.AreEqual(2, relationsIncludingDeletedOnesBefore.Count);

            Assert.AreEqual(1, relationsIncludingDeletedOnesBefore[0].Id);
            Assert.AreEqual(_a, relationsIncludingDeletedOnesBefore[0].Consumer);
            Assert.AreEqual(_b, relationsIncludingDeletedOnesBefore[0].Provider);
            Assert.AreEqual(11, relationsIncludingDeletedOnesBefore[0].Weight);
            Assert.AreEqual("typeab", relationsIncludingDeletedOnesBefore[0].Type);
            Assert.IsTrue(relationsIncludingDeletedOnesBefore[0].IsDeleted);

            Assert.AreEqual(2, relationsIncludingDeletedOnesBefore[1].Id);
            Assert.AreEqual(_b, relationsIncludingDeletedOnesBefore[1].Consumer);
            Assert.AreEqual(_c, relationsIncludingDeletedOnesBefore[1].Provider);
            Assert.AreEqual(22, relationsIncludingDeletedOnesBefore[1].Weight);
            Assert.AreEqual("typebc", relationsIncludingDeletedOnesBefore[1].Type);
            Assert.IsFalse(relationsIncludingDeletedOnesBefore[1].IsDeleted);

            // When
            _relationModel.RestoreRelation(relation1.Id);
            Assert.AreEqual(3, _relationAddedEventCount);

            // Then
            Assert.AreEqual(2, _relationModel.GetRelationCountIncludingDeletedOnes());

            List<IRelation> relationsIncludingDeletedOnesAfter = _relationModel.GetRelationsIncludingDeletedOnes().OrderBy(x => x.Id).ToList();
            Assert.AreEqual(2, relationsIncludingDeletedOnesAfter.Count);

            Assert.AreEqual(1, relationsIncludingDeletedOnesAfter[0].Id);
            Assert.AreEqual(_a, relationsIncludingDeletedOnesAfter[0].Consumer);
            Assert.AreEqual(_b, relationsIncludingDeletedOnesAfter[0].Provider);
            Assert.AreEqual(11, relationsIncludingDeletedOnesAfter[0].Weight);
            Assert.AreEqual("typeab", relationsIncludingDeletedOnesAfter[0].Type);
            Assert.IsFalse(relationsIncludingDeletedOnesBefore[1].IsDeleted);

            Assert.AreEqual(2, relationsIncludingDeletedOnesAfter[1].Id);
            Assert.AreEqual(_b, relationsIncludingDeletedOnesAfter[1].Consumer);
            Assert.AreEqual(_c, relationsIncludingDeletedOnesAfter[1].Provider);
            Assert.AreEqual(22, relationsIncludingDeletedOnesAfter[1].Weight);
            Assert.AreEqual("typebc", relationsIncludingDeletedOnesAfter[1].Type);
            Assert.IsFalse(relationsIncludingDeletedOnesAfter[1].IsDeleted);
        }

        [TestMethod]
        public void GivenNoRelationsExist_WhenRelationsAreImported_ThenModelHasOutgoingRelations()
        {
            // Given
            List<IRelation> aConsumerRelationsBefore = _relationModel.GetAllOutgoingRelations(_a).OrderBy(x => x.Id).ToList();
            Assert.AreEqual(0, aConsumerRelationsBefore.Count);

            List<IRelation> cConsumerRelationsBefore = _relationModel.GetAllOutgoingRelations(_c).OrderBy(x => x.Id).ToList();
            Assert.AreEqual(0, cConsumerRelationsBefore.Count);

            // When
            IRelation relation1 = _relationModel.AddRelation(_a, _b, "typeab", 11, null);
            Assert.IsNotNull(relation1);

            IRelation relation2 = _relationModel.AddRelation(_a, _c, "typeac", 22, null);
            Assert.IsNotNull(relation2);

            IRelation relation3 = _relationModel.AddRelation(_b, _c, "typebc", 33, null);
            Assert.IsNotNull(relation3);

            // Then
            List<IRelation> aConsumerRelationsAfter = _relationModel.GetAllOutgoingRelations(_a).OrderBy(x => x.Id).ToList();
            Assert.AreEqual(2, aConsumerRelationsAfter.Count);
            Assert.AreEqual(relation1, aConsumerRelationsAfter[0]);
            Assert.AreEqual(relation2, aConsumerRelationsAfter[1]);

            List<IRelation> cConsumerRelationsAfter = _relationModel.GetAllOutgoingRelations(_c).OrderBy(x => x.Id).ToList();
            Assert.AreEqual(0, cConsumerRelationsAfter.Count);
        }

        [TestMethod]
        public void GivenNoRelationsExist_WhenRelationsAreImported_ThenModelHasIngoingRelations()
        {
            // Given
            List<IRelation> cProviderRelationsBefore = _relationModel.GetAllIngoingRelations(_c).OrderBy(x => x.Id).ToList();
            Assert.AreEqual(0, cProviderRelationsBefore.Count);

            List<IRelation> aProviderRelationBefore = _relationModel.GetAllIngoingRelations(_a).OrderBy(x => x.Id).ToList();
            Assert.AreEqual(0, aProviderRelationBefore.Count);

            // When
            IRelation relation1 = _relationModel.AddRelation(_a, _b, "typeab", 11, null);
            Assert.IsNotNull(relation1);

            IRelation relation2 = _relationModel.AddRelation(_a, _c, "typeac", 22, null);
            Assert.IsNotNull(relation2);

            IRelation relation3 = _relationModel.AddRelation(_b, _c, "typebc", 33, null);
            Assert.IsNotNull(relation3);

            // Then
            List<IRelation> cProviderRelationsAfter = _relationModel.GetAllIngoingRelations(_c).OrderBy(x => x.Id).ToList();
            Assert.AreEqual(2, cProviderRelationsAfter.Count);
            Assert.AreEqual(relation2, cProviderRelationsAfter[0]);
            Assert.AreEqual(relation3, cProviderRelationsAfter[1]);

            List<IRelation> aProviderRelationsAfter = _relationModel.GetAllIngoingRelations(_a).OrderBy(x => x.Id).ToList();
            Assert.AreEqual(0, aProviderRelationsAfter.Count);
        }

        [TestMethod]
        public void GivenRelationsExist_WhenRelationTypeIsChanged_ThenRelationTypeIsUpdated()
        {
            // Given
            Assert.AreEqual(0, _relationModel.GetRelationCount());

            IRelation relation1 = _relationModel.AddRelation(_a, _b, "typeab", 11, null);
            Assert.IsNotNull(relation1);

            List<IRelation> relationsBefore = _relationModel.GetRelations().OrderBy(x => x.Id).ToList();
            Assert.AreEqual(1, relationsBefore.Count);
            Assert.AreEqual("typeab", relationsBefore[0].Type);

            // When
            _relationModel.ChangeRelationType(relation1, "typeba");

            // Then
            List<IRelation> relationsAfter = _relationModel.GetRelations().OrderBy(x => x.Id).ToList();
            Assert.AreEqual(1, relationsAfter.Count);
            Assert.AreEqual("typeba", relationsAfter[0].Type);
        }

        [TestMethod]
        public void GivenRelationsExist_WhenRelationWeightIsChanged_ThenRelationWeightIsUpdatedAndRelationRemovedAndAddedEventAreTriggered()
        {
            // Given
            Assert.AreEqual(0, _relationModel.GetRelationCount());

            IRelation relation1 = _relationModel.AddRelation(_a, _b, "typeab", 11, null);
            Assert.IsNotNull(relation1);

            List<IRelation> relationsBefore = _relationModel.GetRelations().OrderBy(x => x.Id).ToList();
            Assert.AreEqual(1, relationsBefore.Count);
            Assert.AreEqual(11, relationsBefore[0].Weight);

            Assert.AreEqual(1, _relationAddedEventCount);
            Assert.AreEqual(0, _relationRemovedEventCount);

            // When
            _relationModel.ChangeRelationWeight(relation1, 22);

            Assert.AreEqual(2, _relationAddedEventCount);
            Assert.AreEqual(1, _relationRemovedEventCount);

            // Then
            List<IRelation> relationsAfter = _relationModel.GetRelations().OrderBy(x => x.Id).ToList();
            Assert.AreEqual(1, relationsAfter.Count);
            Assert.AreEqual(22, relationsAfter[0].Weight);
        }


        [TestMethod]
        public void GivenNoRelationsExist_WhenRelationIsAdded_ThenItCanBeFoundByInputData()
        {
            // Given
            Assert.AreEqual(0, _relationModel.GetRelationCount());
            Assert.IsNull(_relationModel.FindRelation(_a, _b, "typeab", 11));

            // When
            IRelation relation1 = _relationModel.AddRelation(_a, _b, "typeab", 11, null);
            Assert.IsNotNull(relation1);

            // Then
            Assert.IsNotNull(_relationModel.FindRelation(_a, _b, "typeab", 11));
        }

        [TestMethod]
        public void GivenNoRelationsExist_WhenThreeRelationsAreImportedWithDifferentType_ThenThreeRelationTypesAndEmptyTypeExist()
        {
            Assert.AreEqual(0, _relationModel.GetRelationCount());

            IRelation relation1 = _relationModel.ImportRelation(1, _a, _b, "typeab", 11, null);
            Assert.IsNotNull(relation1);
            Assert.AreEqual(1, _relationAddedEventCount);

            IRelation relation2 = _relationModel.ImportRelation(2, _b, _c, "typebc", 22, null);
            Assert.IsNotNull(relation2);
            Assert.AreEqual(2, _relationAddedEventCount);

            IRelation relation3 = _relationModel.ImportRelation(3, _c, _a, "typeca", 33, null);
            Assert.IsNotNull(relation3);
            Assert.AreEqual(3, _relationAddedEventCount);

            Assert.AreEqual(3, _relationModel.GetRelationCount());

            List<string> relationTypes = _relationModel.GetRelationTypes().ToList();
            Assert.AreEqual(4, relationTypes.Count);
            Assert.AreEqual("", relationTypes[0]);
            Assert.AreEqual("typeab", relationTypes[1]);
            Assert.AreEqual("typebc", relationTypes[2]);
            Assert.AreEqual("typeca", relationTypes[3]);
        }

        [TestMethod]
        public void GivenNoRelationsExist_WhenThreeRelationsAreImportedWithSameType_ThenOneRelationTypeAndEmptyTypeExists()
        {
            Assert.AreEqual(0, _relationModel.GetRelationCount());

            IRelation relation1 = _relationModel.ImportRelation(1, _a, _b, "typeab", 11, null);
            Assert.IsNotNull(relation1);
            Assert.AreEqual(1, _relationAddedEventCount);

            IRelation relation2 = _relationModel.ImportRelation(2, _b, _c, "typeab", 22, null);
            Assert.IsNotNull(relation2);
            Assert.AreEqual(2, _relationAddedEventCount);

            IRelation relation3 = _relationModel.ImportRelation(3, _c, _a, "typeab", 33, null);
            Assert.IsNotNull(relation3);
            Assert.AreEqual(3, _relationAddedEventCount);

            Assert.AreEqual(3, _relationModel.GetRelationCount());

            List<string> relationTypes = _relationModel.GetRelationTypes().ToList();
            Assert.AreEqual(2, relationTypes.Count);
            Assert.AreEqual("", relationTypes[0]);
            Assert.AreEqual("typeab", relationTypes[1]);
        }

        [TestMethod]
        public void GivenNoRelationsExist_WhenThreeRelationsAreAddedWithDifferentType_ThenThreeRelationTypesAndEmptyTypeExist()
        {
            Assert.AreEqual(0, _relationModel.GetRelationCount());

            IRelation relation1 = _relationModel.AddRelation(_a, _b, "typeab", 11, null);
            Assert.IsNotNull(relation1);
            Assert.AreEqual(1, _relationAddedEventCount);

            IRelation relation2 = _relationModel.AddRelation(_b, _c, "typebc", 22, null);
            Assert.IsNotNull(relation2);
            Assert.AreEqual(2, _relationAddedEventCount);

            IRelation relation3 = _relationModel.AddRelation(_c, _a, "typeca", 33, null);
            Assert.IsNotNull(relation3);
            Assert.AreEqual(3, _relationAddedEventCount);

            Assert.AreEqual(3, _relationModel.GetRelationCount());

            List<string> relationTypes = _relationModel.GetRelationTypes().ToList();
            Assert.AreEqual(4, relationTypes.Count);
            Assert.AreEqual("", relationTypes[0]);
            Assert.AreEqual("typeab", relationTypes[1]);
            Assert.AreEqual("typebc", relationTypes[2]);
            Assert.AreEqual("typeca", relationTypes[3]);
        }

        [TestMethod]
        public void GivenNoRelationsExist_WhenThreeRelationsAreAddedWithSameType_ThenOneRelationTypeAndEmptyTypeExist()
        {
            Assert.AreEqual(0, _relationModel.GetRelationCount());

            IRelation relation1 = _relationModel.AddRelation(_a, _b, "typeab", 11, null);
            Assert.IsNotNull(relation1);
            Assert.AreEqual(1, _relationAddedEventCount);

            IRelation relation2 = _relationModel.AddRelation(_b, _c, "typeab", 22, null);
            Assert.IsNotNull(relation2);
            Assert.AreEqual(2, _relationAddedEventCount);

            IRelation relation3 = _relationModel.AddRelation(_c, _a, "typeab", 33, null);
            Assert.IsNotNull(relation3);
            Assert.AreEqual(3, _relationAddedEventCount);

            Assert.AreEqual(3, _relationModel.GetRelationCount());

            List<string> relationTypes = _relationModel.GetRelationTypes().ToList();
            Assert.AreEqual(2, relationTypes.Count);
            Assert.AreEqual("", relationTypes[0]);
            Assert.AreEqual("typeab", relationTypes[1]);
        }

        [TestMethod]
        public void WhenInvalidRelationIsAdded_ThenNoRelationIsAdded()
        {
            // Given
            Assert.AreEqual(0, _relationModel.GetRelationCount());

            // When
            Assert.IsNull(_relationModel.AddRelation(_a, _a, "", 1, null));  // To self
            Assert.IsNull(_relationModel.AddRelation(_a, _b, "", 0, null));    // No weight
            Assert.IsNull(_relationModel.AddRelation(_a, _b, "", -1, null));    // No valid weight

            // Then
            Assert.AreEqual(0, _relationModel.GetRelationCount());
        }

        [TestMethod]
        public void WhenInvalidRelationIsImported_ThenNoRelationIsAdded()
        {
            // Given
            Assert.AreEqual(0, _relationModel.GetRelationCount());

            // When
            Assert.IsNull(_relationModel.ImportRelation(1, _a, _a, "", 1, null));  // To self
            Assert.IsNull(_relationModel.ImportRelation(1, _a, _b, "", 0, null));    // No weight
            Assert.IsNull(_relationModel.ImportRelation(1, _a, _b, "", -1, null));    // No valid weight

            // Then
            Assert.AreEqual(0, _relationModel.GetRelationCount());
        }

        private void CreateElementHierarchy()
        {
            _a = _elementsDataModel.ImportElement(11, "a", "", null, 1, false, false, null);
            _b = _elementsDataModel.ImportElement(14, "b", "", null, 4, false, false, null);
            _c = _elementsDataModel.ImportElement(17, "c", "", null, 7, false, false, null);
        }
    }
}