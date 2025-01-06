using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.Data.Model.Elements;
using Dsmviz.Viewer.Application.Editing.Action.Element;
using Moq;

namespace Test.Viewer.Application.Editing.Action.Element
{
    [TestClass]
    public class ElementChangeTypeActionTest
    {
        private Mock<IElementModelEditing> _elementModelEditingMock;
        private Mock<IElement> _elementMock;

        private const int ElementId = 1;
        private const string OldType = "oldtype";
        private const string NewType = "newtype";

        [TestInitialize()]
        public void Setup()
        {
            _elementModelEditingMock = new Mock<IElementModelEditing>();
            _elementMock = new Mock<IElement>();

            _elementMock.Setup(x => x.Id).Returns(ElementId);
            _elementMock.Setup(x => x.Type).Returns(OldType);
        }

        [TestMethod]
        public void WhenDoActionThenElementTypeIsChangedDataModel()
        {
            ElementChangeTypeAction action = new ElementChangeTypeAction(_elementModelEditingMock.Object, _elementMock.Object, NewType);
            Assert.IsTrue(action.IsValid());

            Assert.IsNull(action.Do());

            _elementModelEditingMock.Verify(x => x.ChangeElementType(_elementMock.Object, NewType), Times.Once());
        }

        [TestMethod]
        public void WhenUndoActionThenElementTypeIsRevertedDataModel()
        {
            ElementChangeTypeAction action = new ElementChangeTypeAction(_elementModelEditingMock.Object, _elementMock.Object, NewType);
            Assert.IsTrue(action.IsValid());

            action.Undo();

            _elementModelEditingMock.Verify(x => x.ChangeElementType(_elementMock.Object, OldType), Times.Once());
        }
    }
}
