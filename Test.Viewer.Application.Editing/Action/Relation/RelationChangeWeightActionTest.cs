using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.Data.Model.Relations;
using Dsmviz.Viewer.Application.Editing.Action.Relation;
using Moq;


namespace Test.Viewer.Application.Editing.Action.Relation
{
    [TestClass]
    public class RelationChangeWeightActionTest
    {
        private Mock<IRelationModelEditing> _relationModelEditingMock;
        private Mock<IRelation> _relationMock;

        private const int RelationId = 1;
        private const int ConsumerId = 2;
        private const int ProviderId = 3;
        private const int OldWeight = 123;
        private const int NewWeight = 456;

        [TestInitialize()]
        public void Setup()
        {
            _relationModelEditingMock = new Mock<IRelationModelEditing>();
            _relationMock = new Mock<IRelation>();

            _relationMock.Setup(x => x.Id).Returns(RelationId);
            _relationMock.Setup(x => x.Consumer.Id).Returns(ConsumerId);
            _relationMock.Setup(x => x.Provider.Id).Returns(ProviderId);
            _relationMock.Setup(x => x.Weight).Returns(OldWeight);
        }

        [TestMethod]
        public void WhenDoActionThenRelationWeightIsChangedDataModel()
        {
            RelationChangeWeightAction action = new RelationChangeWeightAction(_relationModelEditingMock.Object, _relationMock.Object, NewWeight);
            Assert.IsTrue(action.IsValid());

            Assert.IsNull(action.Do());

            _relationModelEditingMock.Verify(x => x.ChangeRelationWeight(_relationMock.Object, NewWeight), Times.Once());
        }

        [TestMethod]
        public void WhenUndoActionThenRelationWeightIsRevertedDataModel()
        {
            RelationChangeWeightAction action = new RelationChangeWeightAction(_relationModelEditingMock.Object, _relationMock.Object, NewWeight);
            Assert.IsTrue(action.IsValid());

            action.Undo();

            _relationModelEditingMock.Verify(x => x.ChangeRelationWeight(_relationMock.Object, OldWeight), Times.Once());
        }
    }
}
