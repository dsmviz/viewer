using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.Data.Model.Elements;
using Dsmviz.Viewer.Application.Editing.Action.Element;
using Moq;

namespace Dsmviz.Test.Application.Editing.Action.Element
{
    [TestClass]
    public class ElementChangeParentActionTest
    {
        private Mock<IElementModelEditing> _elementModelEditingMock;
        private Mock<IElement> _elementMock;
        private Mock<IElement> _oldParentMock;
        private Mock<IElement> _newParentMock;

        private const int ElementId = 1;
        private const int OldParentId = 2;
        private const int NewParentId = 3;

        private const int OldIndex = 4;
        private const int NewIndex = 5;

        private const string ElementName = "ElementName";

        [TestInitialize()]
        public void Setup()
        {
            _elementModelEditingMock = new Mock<IElementModelEditing>();
            _elementMock = new Mock<IElement>();
            _oldParentMock = new Mock<IElement>();
            _newParentMock = new Mock<IElement>();

            _elementMock.Setup(x => x.Id).Returns(ElementId);
            _elementMock.Setup(x => x.Parent).Returns(_oldParentMock.Object);
            _elementMock.Setup(x => x.Name).Returns(ElementName);
            _oldParentMock.Setup(x => x.IndexOfChild(_elementMock.Object)).Returns(OldIndex);
            _oldParentMock.Setup(x => x.Id).Returns(OldParentId);
            _newParentMock.Setup(x => x.Id).Returns(NewParentId);
        }

        [TestMethod]
        public void WhenDoActionThenElementParentIsChangedDataModel()
        {
            _newParentMock.Setup(x => x.ContainsChildWithName(ElementName)).Returns(false);

            ElementChangeParentAction action =
                new ElementChangeParentAction(_elementModelEditingMock.Object, _elementMock.Object, _newParentMock.Object, NewIndex);
            Assert.IsTrue(action.IsValid());

            Assert.IsNull(action.Do());

            _elementModelEditingMock.Verify(x => x.ChangeElementParent(_elementMock.Object, _newParentMock.Object, NewIndex), Times.Once());
        }

        [TestMethod]
        public void WhenUndoActionThenElementParentIsRevertedDataModelNoNameChange()
        {
            _newParentMock.Setup(x => x.ContainsChildWithName(ElementName)).Returns(false);

            ElementChangeParentAction action =
                new ElementChangeParentAction(_elementModelEditingMock.Object, _elementMock.Object, _newParentMock.Object, NewIndex);
            Assert.IsTrue(action.IsValid());

            action.Undo();

            _elementModelEditingMock.Verify(x => x.ChangeElementParent(_elementMock.Object, _oldParentMock.Object, OldIndex), Times.Once());
        }
    }
}
