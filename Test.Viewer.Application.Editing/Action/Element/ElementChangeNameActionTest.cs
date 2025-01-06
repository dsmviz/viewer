using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.Data.Model.Elements;
using Dsmviz.Viewer.Application.Editing.Action.Element;
using Moq;

namespace Test.Viewer.Application.Editing.Action.Element
{
    [TestClass]
    public class ElementChangeNameActionTest
    {
        private Mock<IElementModelEditing> _elementModelEditingMock;
        private Mock<IElement> _elementMock;

        private const int ElementId = 1;
        private const string OldName = "oldname";
        private const string NewName = "newname";

        [TestInitialize()]
        public void Setup()
        {
            _elementModelEditingMock = new Mock<IElementModelEditing>();
            _elementMock = new Mock<IElement>();

            _elementMock.Setup(x => x.Id).Returns(ElementId);
            _elementMock.Setup(x => x.Name).Returns(OldName);
        }

        [TestMethod]
        public void WhenDoActionThenElementNameIsChangedDataModel()
        {
            ElementChangeNameAction action = new ElementChangeNameAction(_elementModelEditingMock.Object, _elementMock.Object, NewName);
            Assert.IsTrue(action.IsValid());

            Assert.IsNull(action.Do());

            _elementModelEditingMock.Verify(x => x.ChangeElementName(_elementMock.Object, NewName), Times.Once());
        }

        [TestMethod]
        public void WhenUndoActionThenElementNameIsRevertedDataModel()
        {
            ElementChangeNameAction action = new ElementChangeNameAction(_elementModelEditingMock.Object, _elementMock.Object, NewName);
            Assert.IsTrue(action.IsValid());

            action.Undo();

            _elementModelEditingMock.Verify(x => x.ChangeElementName(_elementMock.Object, OldName), Times.Once());
        }
    }
}
