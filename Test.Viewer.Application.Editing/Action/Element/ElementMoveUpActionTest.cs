using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.Data.Model.Elements;
using Dsmviz.Viewer.Application.Editing.Action.Element;
using Moq;

namespace Dsmviz.Test.Application.Editing.Action.Element
{
    [TestClass]
    public class ElementMoveUpActionTest
    {
        private Mock<IElementModelEditing> _elementModelEditingMock;
        private Mock<IElement> _elementMock;
        private Mock<IElement> _previousElementMock;

        [TestInitialize()]
        public void Setup()
        {
            _elementModelEditingMock = new Mock<IElementModelEditing>();
            _elementMock = new Mock<IElement>();
            _previousElementMock = new Mock<IElement>();
            _elementMock.Setup(x => x.Id).Returns(1);
        }

        [TestMethod]
        public void WhenDoActionThenElementIsRemovedFromDataModel()
        {
            _elementModelEditingMock.Setup(x => x.PreviousSibling(_elementMock.Object)).Returns(_previousElementMock.Object);

            ElementMoveUpAction action = new ElementMoveUpAction(_elementModelEditingMock.Object, _elementMock.Object);
            Assert.IsTrue(action.IsValid());

            Assert.IsNull(action.Do());

            _elementModelEditingMock.Verify(x => x.Swap(_elementMock.Object, _previousElementMock.Object), Times.Once());
        }

        [TestMethod]
        public void WhenUndoActionThenElementIsRestoredInDataModel()
        {
            _elementModelEditingMock.Setup(x => x.NextSibling(_elementMock.Object)).Returns(_previousElementMock.Object);

            ElementMoveUpAction action = new ElementMoveUpAction(_elementModelEditingMock.Object, _elementMock.Object);
            Assert.IsTrue(action.IsValid());

            action.Undo();

            _elementModelEditingMock.Verify(x => x.Swap(_previousElementMock.Object, _elementMock.Object), Times.Once());
        }
    }
}
