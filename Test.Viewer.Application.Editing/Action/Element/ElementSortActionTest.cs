using Dsmviz.Interfaces.Application.Algorithms;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.Data.Model.Elements;
using Dsmviz.Viewer.Application.Editing.Action.Element;
using Moq;

namespace Test.Viewer.Application.Editing.Action.Element
{
    [TestClass]
    public class ElementSortActionTest
    {
        private Mock<IElementModelEditing> _elementModelEditingMock;
        private Mock<IElement> _elementMock;
        private Mock<IElementWeightMatrix> _weightsMatrixMock;
        private Mock<ISortAlgorithm> _sortAlgorithmMock;
        private Mock<ISortResult> _sortResultMock;

        private const int ElementId = 1;
        private const string SortResult = "2,0,1";
        private const string InverseSortResult = "1,2,0";

        [TestInitialize()]
        public void Setup()
        {
            _elementModelEditingMock = new Mock<IElementModelEditing>();
            _elementMock = new Mock<IElement>();
            _weightsMatrixMock = new Mock<IElementWeightMatrix>();
            _sortAlgorithmMock = new Mock<ISortAlgorithm>();
            _sortResultMock = new Mock<ISortResult>();

            _sortAlgorithmMock.Setup(x => x.Sort(_elementMock.Object, _weightsMatrixMock.Object)).Returns(_sortResultMock.Object);
            _elementMock.Setup(x => x.Id).Returns(ElementId);
        }

        [TestMethod]
        public void WhenDoActionThenElementsChildrenAreSorted()
        {
            ElementSortAction action = new ElementSortAction(_elementModelEditingMock.Object, _elementMock.Object, _weightsMatrixMock.Object, _sortAlgorithmMock.Object);
            Assert.IsTrue(action.IsValid());

            Assert.IsNull(action.Do());

            _elementModelEditingMock.Verify(x => x.ReorderChildren(_elementMock.Object, _sortResultMock.Object.SortedIndexValues), Times.Once());
        }

        [TestMethod]
        public void WhenUndoActionThenElementsChildrenAreSortIsReverted()
        {
            ElementSortAction action = new ElementSortAction(_elementModelEditingMock.Object, _elementMock.Object, _weightsMatrixMock.Object, _sortAlgorithmMock.Object);
            Assert.IsTrue(action.IsValid());

            action.Do();

            _elementModelEditingMock.Verify(x => x.ReorderChildren(_elementMock.Object, _sortResultMock.Object.SortedIndexValues), Times.Once());

            action.Undo();

            _sortResultMock.Verify(x => x.InvertOrder(), Times.Once());
            _elementModelEditingMock.Verify(x => x.ReorderChildren(_elementMock.Object, _sortResultMock.Object.SortedIndexValues), Times.Exactly(2));
        }
    }
}
