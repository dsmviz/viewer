using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.Data.Model.Elements;
using Dsmviz.Viewer.Application.Editing.Action.Element;
using Moq;

namespace Dsmviz.Test.Application.Editing.Action.Element
{
    [TestClass]
    public class ElementMoveDownActionTest
    {
        private Mock<IElementModelEditing> _elementModelEditingMock;
        private Mock<IElement> _elementMock;
        private Mock<IElement> _nextElementMock;

        [TestInitialize()]
        public void Setup()
        {
            _elementModelEditingMock = new Mock<IElementModelEditing>();
            _elementMock = new Mock<IElement>();
            _nextElementMock = new Mock<IElement>();
            _elementMock.Setup(x => x.Id).Returns(1);
        }

        [TestMethod]
        public void WhenDoActionThenElementIsRemovedFromDataModel()
        {
            _elementModelEditingMock.Setup(x => x.NextSibling(_elementMock.Object)).Returns(_nextElementMock.Object);

            ElementMoveDownAction action = new ElementMoveDownAction(_elementModelEditingMock.Object, _elementMock.Object);
            Assert.IsTrue(action.IsValid());

            Assert.IsNull(action.Do());

            _elementModelEditingMock.Verify(x => x.Swap(_elementMock.Object, _nextElementMock.Object), Times.Once());
        }

        [TestMethod]
        public void WhenUndoActionThenElementIsRestoredInDataModel()
        {
            _elementModelEditingMock.Setup(x => x.PreviousSibling(_elementMock.Object)).Returns(_nextElementMock.Object);

            ElementMoveDownAction action = new ElementMoveDownAction(_elementModelEditingMock.Object, _elementMock.Object);
            Assert.IsTrue(action.IsValid());

            action.Undo();

            _elementModelEditingMock.Verify(x => x.Swap(_nextElementMock.Object, _elementMock.Object), Times.Once());
        }
    }
}
