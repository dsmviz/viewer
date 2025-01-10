

using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Viewer.Data.Model.Elements;
using Dsmviz.Viewer.Data.Model.Relations;
using Moq;

namespace Dsmviz.Test.Data.Model.Elements
{
    [TestClass]
    public class ElementModelTest
    {
        Mock<IRelationModelUpdate>? _relationModel;
        private ElementModel? _elementModel;

        private int _elementAddedEventCount;
        private int _elementRemovedEventCount;

        [TestInitialize]
        public void TestInitialize()
        {
            _elementAddedEventCount = 0;
            _elementRemovedEventCount = 0;

            _relationModel = new Mock<IRelationModelUpdate>();
            _elementModel = new ElementModel(_relationModel.Object);

            _elementModel.ElementAdded += OnElementAdded;
            _elementModel.ElementRemoved += OnElementRemoved;
        }

        private void OnElementAdded(object sender, IElement e)
        {
            _elementAddedEventCount++;
        }

        private void OnElementRemoved(object sender, IElement e)
        {
            _elementRemovedEventCount++;
        }

        [TestMethod]
        public void WhenModelIsConstructed_ThenOnlyRootElementExists()
        {
            Assert.AreEqual(1, _elementModel.GetElementCount());
        }

        [TestMethod]
        public void GivenOneElementExists_WhenClearIsCalled_ThenOnlyRootElementExists()
        {
            // Given
            Assert.AreEqual(1, _elementModel.GetElementCount());

            _elementModel.ImportElement(1, "name", "type", null, 0, false, false, null);
            Assert.AreEqual(2, _elementModel.GetElementCount());

            // When
            _elementModel.Clear();

            // Then
            Assert.AreEqual(1, _elementModel.GetElementCount());
        }

        [TestMethod]
        public void GivenOnlyRootElementExists_WhenAddElementIsCalled_ThenTwoElementsExist()
        {
            // Given
            Assert.AreEqual(1, _elementModel.GetElementCount());

            // When
            IElement a = _elementModel.AddElement("a", "type", null, 0, null);
            Assert.IsNotNull(a);

            // Then
            Assert.AreEqual(2, _elementModel.GetElementCount());
        }

        [TestMethod]
        public void GivenOnlyRootElementExists_WhenAddElementIsCalledTwice_ThenThreeElementsExist()
        {
            // Given
            Assert.AreEqual(1, _elementModel.GetElementCount());

            // When
            IElement a = _elementModel.AddElement("a", "type", null, 0, null);
            Assert.IsNotNull(a);

            IElement b = _elementModel.AddElement("b", "type", a.Id, 0, null);
            Assert.IsNotNull(b);

            // Then
            Assert.AreEqual(3, _elementModel.GetElementCount());
        }

        [TestMethod]
        public void GivenOnlyRootElementExists_WhenAddElementIsCalledTwice_ThenTwoElementAddedEventsAreTriggered()
        {
            // Given
            Assert.AreEqual(0, _elementAddedEventCount);

            // When
            IElement a = _elementModel.AddElement("a", "type", null, 0, null);
            Assert.IsNotNull(a);

            IElement b = _elementModel.AddElement("b", "type", a.Id, 0, null);
            Assert.IsNotNull(b);

            // Then
            Assert.AreEqual(2, _elementAddedEventCount);
        }

        [TestMethod]
        public void GivenOnlyRootElementExists_WhenImportElementIsCalled_ThenTwoElementsExists()
        {
            // Given
            Assert.AreEqual(1, _elementModel.GetElementCount());

            // When
            IElement a = _elementModel.ImportElement(1, "name", "type", null, 0, false, false, null);
            Assert.IsNotNull(a);

            // Then
            Assert.AreEqual(2, _elementModel.GetElementCount());
        }

        [TestMethod]
        public void GivenOnlyRootElementExists_WhenImportElementIsCalledTwice_ThenThreeElementsExist()
        {
            // Given
            Assert.AreEqual(1, _elementModel.GetElementCount());

            // When
            IElement a = _elementModel.ImportElement(1, "a", "type", null, 0, false, false, null);
            Assert.IsNotNull(a);

            IElement b = _elementModel.ImportElement(2, "b", "type", null, 0, false, false, null);
            Assert.IsNotNull(b);

            // Then
            Assert.AreEqual(3, _elementModel.GetElementCount());
        }

        [TestMethod]
        public void GivenOnlyRootElementExists_WhenImportElementIsCalledTwice_ThenTwoElementAddedEventsAreTriggered()
        {
            // Given
            Assert.AreEqual(0, _elementAddedEventCount);

            // When
            IElement a = _elementModel.ImportElement(1, "a", "type", null, 0, false, false, null);
            Assert.IsNotNull(a);

            IElement b = _elementModel.ImportElement(2, "b", "type", null, 0, false, false, null);
            Assert.IsNotNull(b);

            // Then
            Assert.AreEqual(2, _elementAddedEventCount);
        }

        [TestMethod]
        public void GivenTheElementExists_WhenFindByIdIsCalledUsingValidId_ThenElementIsFound()
        {
            // Given
            Assert.AreEqual(1, _elementModel.GetElementCount());

            _elementModel.ImportElement(1, "name", "type", null, 10, true, false, null);

            // When
            IElement foundElement = _elementModel.GetElementById(1);

            // Then
            Assert.IsNotNull(foundElement);
            Assert.AreEqual(1, foundElement.Id);
            Assert.AreEqual("name", foundElement.Name);
            Assert.AreEqual("type", foundElement.Type);
            Assert.AreEqual(10, foundElement.Order);
            Assert.AreEqual(true, foundElement.IsExpanded);
            Assert.IsNotNull(foundElement.Parent); // root element
            Assert.IsNull(foundElement.Parent.Parent);
        }

        [TestMethod]
        public void GivenTheElementExists_WhenFindByIdIsCalledUsingInvalidId_ThenElementIsNotFound()
        {
            // Given
            Assert.AreEqual(1, _elementModel.GetElementCount());

            _elementModel.ImportElement(1, "name", "type", null, 10, true, false, null);

            // When
            IElement foundElement = _elementModel.GetElementById(2);

            // Then
            Assert.IsNull(foundElement);
        }

        [TestMethod]
        public void GivenHierarchyOfTwoElementsExists_WhenFindByNameIsCalledUsingValidFullName_ThenElementIsFound()
        {
            // Given
            Assert.AreEqual(1, _elementModel.GetElementCount());

            IElement a = _elementModel.ImportElement(1, "a", "type", null, 10, true, false, null);
            Assert.IsNotNull(a);

            IElement b = _elementModel.ImportElement(2, "b", "type", null, 11, true, false, a.Id);
            Assert.IsNotNull(b);

            // When
            IElement foundElement = _elementModel.GetElementByFullname("a.b");

            // Then
            Assert.IsNotNull(foundElement);
            Assert.AreEqual(2, foundElement.Id);
            Assert.AreEqual("b", foundElement.Name);
            Assert.AreEqual("a.b", foundElement.Fullname);
            Assert.AreEqual("type", foundElement.Type);
            Assert.AreEqual(11, foundElement.Order);
            Assert.AreEqual(true, foundElement.IsExpanded);
            Assert.AreEqual(a, foundElement.Parent);
        }

        [TestMethod]
        public void GivenHierarchyOfTwoElementsExists_WhenFindByNameIsCalledUsingInvalidFullName_ThenElementIsNotFound()
        {
            // Given
            Assert.AreEqual(1, _elementModel.GetElementCount());

            IElement a = _elementModel.ImportElement(1, "a", "type", null, 10, true, false, null);
            Assert.IsNotNull(a);

            IElement b = _elementModel.ImportElement(2, "b", "type", null, 11, true, false, a.Id);
            Assert.IsNotNull(b);

            //When
            IElement foundElement = _elementModel.GetElementByFullname("a.c");

            // Then
            Assert.IsNull(foundElement);
        }

        [TestMethod]
        public void GivenHierarchyOfTwoElementsExists_WhenChangeElementIsCalledToChangeName_ThenItCanBeFoundUnderThatName()
        {
            // Given
            Assert.AreEqual(1, _elementModel.GetElementCount());

            IElement a = _elementModel.ImportElement(1, "a", "type", null, 10, true, false, null);
            IElement b = _elementModel.ImportElement(2, "b", "type", null, 11, true, false, a.Id);

            IElement foundElementBefore = _elementModel.GetElementByFullname("a.c");
            Assert.IsNull(foundElementBefore);

            // When
            _elementModel.ChangeElementName(b, "c");

            // Then
            IElement foundElementAfter = _elementModel.GetElementByFullname("a.c");
            Assert.IsNotNull(foundElementAfter);
        }

        [TestMethod]
        public void GivenOneElementsExists_WhenChangeElementIsCalledToChangeType_ThenTypeIsChanged()
        {
            // Given
            Assert.AreEqual(1, _elementModel.GetElementCount());

            IElement a = _elementModel.ImportElement(1, "a", "typebefore", null, 10, true, false, null);
            Assert.AreEqual("typebefore", a.Type);

            // When
            _elementModel.ChangeElementType(a, "typeafter");

            // Then
            Assert.AreEqual("typeafter", a.Type);
        }

        [TestMethod]
        public void GivenHierarchyOfElementsExists_WhenChangeElementParentIsCalled_ThenItCanBeFoundAtTheNewLocation()
        {
            // Given
            Assert.AreEqual(1, _elementModel.GetElementCount());

            IElement a = _elementModel.ImportElement(1, "a", "type", null, 10, true, false, null);
            IElement b = _elementModel.ImportElement(2, "b", "type", null, 11, true, false, a.Id);

            IElement c = _elementModel.ImportElement(3, "c", "type", null, 12, true, false, null);
            IElement d = _elementModel.ImportElement(4, "d", "type", null, 13, true, false, c.Id);
            IElement e = _elementModel.ImportElement(5, "e", "type", null, 14, true, false, c.Id);

            IElement ab = _elementModel.GetElementByFullname("a.b");
            Assert.AreEqual(b, ab);
            Assert.IsNull(_elementModel.GetElementByFullname("c.b"));

            Assert.AreEqual(1, a.Children.Count);
            Assert.AreEqual(b, a.Children[0]);

            Assert.AreEqual(2, c.Children.Count);
            Assert.AreEqual(d, c.Children[0]);
            Assert.AreEqual(e, c.Children[1]);

            // When
            _elementModel.ChangeElementParent(b, c, 1);

            // Then
            Assert.IsNull(_elementModel.GetElementByFullname("a.b"));

            IElement cb = _elementModel.GetElementByFullname("c.b");
            Assert.AreEqual(b, ab);

            Assert.AreEqual(0, a.Children.Count);

            Assert.AreEqual(3, c.Children.Count);
            Assert.AreEqual(d, c.Children[0]);
            Assert.AreEqual(b, c.Children[1]);
            Assert.AreEqual(e, c.Children[2]);
        }

        [TestMethod]
        public void GivenHierarchyOfElementsExists_WhenChangeElementParentIsCalled_ThenRelationsAndRulesAerRemovedAndUnremovedAgainToAllowDerivedWeightToBeUpdatedCorrectly()
        {
            // Given
            Assert.AreEqual(1, _elementModel.GetElementCount());

            IElement a = _elementModel.ImportElement(1, "a", "type", null, 10, true, false, null);
            IElement b = _elementModel.ImportElement(2, "b", "type", null, 11, true, false, a.Id);

            IElement c = _elementModel.ImportElement(3, "c", "type", null, 12, true, false, null);
            IElement d = _elementModel.ImportElement(4, "d", "type", null, 13, true, false, c.Id);
            IElement e = _elementModel.ImportElement(5, "e", "type", null, 14, true, false, c.Id);

            // When
            _elementModel.ChangeElementParent(b, c, 1);

            // Then
            _relationModel.Verify(x => x.RemoveAllElementRelations(b), Times.Once());
            _relationModel.Verify(x => x.RestoreAllElementRelations(b), Times.Once());
        }

        [TestMethod]
        public void GivenHierarchyOfElementsExists_WhenSearchIsCalledWithTextPartOfItsNameAndMarkingIsOn_ThenElementIsFoundAndTheElementAndItsParentAreMarkedAsMatching()
        {
            // Given
            Assert.AreEqual(1, _elementModel.GetElementCount());

            IElement a = _elementModel.ImportElement(1, "a", "atype", null, 10, true, false, null);
            Assert.IsNotNull(a);

            IElement b = _elementModel.ImportElement(2, "b", "btype", null, 11, true, false, a.Id);
            Assert.IsNotNull(b);

            IElement c = _elementModel.ImportElement(3, "c", "ctype", null, 12, true, false, a.Id);
            Assert.IsNotNull(c);

            // When
            IList<IElement> matches = _elementModel.SearchElements("a.b", _elementModel.GetRootElement(), true, null, true);

            // Then
            Assert.AreEqual(1, matches.Count);
            Assert.AreEqual(b, matches[0]);

            Assert.AreEqual(true, _elementModel.GetRootElement().IsMatch);

            Assert.AreEqual(true, a.IsMatch);
            Assert.AreEqual(true, b.IsMatch);
            Assert.AreEqual(false, c.IsMatch);
        }

        [TestMethod]
        public void GivenAnElementHasBeenMarkedAfterSearch_WhenSearchTextIsCleared_ThenNoElementAreFoundAndTheElementAndItsParentAreNotMarkedAsMatching()
        {
            // Given
            Assert.AreEqual(1, _elementModel.GetElementCount());

            IElement a = _elementModel.ImportElement(1, "a", "atype", null, 10, true, false, null);
            Assert.IsNotNull(a);

            IElement b = _elementModel.ImportElement(2, "b", "btype", null, 11, true, false, a.Id);
            Assert.IsNotNull(b);

            IElement c = _elementModel.ImportElement(3, "c", "ctype", null, 12, true, false, a.Id);
            Assert.IsNotNull(c);

            IList<IElement> matchesBefore = _elementModel.SearchElements("a.b", _elementModel.GetRootElement(), true, null, true);

            // Then
            Assert.AreEqual(1, matchesBefore.Count);
            Assert.AreEqual(b, matchesBefore[0]);

            Assert.AreEqual(true, _elementModel.GetRootElement().IsMatch);

            Assert.AreEqual(true, a.IsMatch);
            Assert.AreEqual(true, b.IsMatch);
            Assert.AreEqual(false, c.IsMatch);

            // When
            IList<IElement> matchesAfter = _elementModel.SearchElements("", _elementModel.GetRootElement(), true, null, true);
            Assert.AreEqual(0, matchesAfter.Count);

            // Then
            Assert.AreEqual(false, a.IsMatch);
            Assert.AreEqual(false, b.IsMatch);
            Assert.AreEqual(false, c.IsMatch);
        }

        [TestMethod]
        public void GivenHierarchyOfElementsExists_WhenSearchIsCalledWithTextPartOfItsNameAndMarkingIsOff_ThenElementIsFoundAndNoElementsAreMarkedAsMatching()
        {
            // Given
            Assert.AreEqual(1, _elementModel.GetElementCount());

            IElement a = _elementModel.ImportElement(1, "a", "atype", null, 10, true, false, null);
            Assert.IsNotNull(a);

            IElement b = _elementModel.ImportElement(2, "b", "btype", null, 11, true, false, a.Id);
            Assert.IsNotNull(b);

            IElement c = _elementModel.ImportElement(3, "c", "ctype", null, 12, true, false, a.Id);
            Assert.IsNotNull(c);

            // When
            IList<IElement> matches = _elementModel.SearchElements("a.b", _elementModel.GetRootElement(), true, null, false);

            // Then
            Assert.AreEqual(1, matches.Count);
            Assert.AreEqual(b, matches[0]);

            Assert.AreEqual(false, _elementModel.GetRootElement().IsMatch);

            Assert.AreEqual(false, a.IsMatch);
            Assert.AreEqual(false, b.IsMatch);
            Assert.AreEqual(false, c.IsMatch);
        }

        [TestMethod]
        public void GivenHierarchyOfElementsExists_WhenSearchAndCaseSensitiveIsOffIsCalledWithTextPartOfItsNameInWrongCase_ThenElementIsFound()
        {
            // Given
            Assert.AreEqual(1, _elementModel.GetElementCount());

            IElement a = _elementModel.ImportElement(1, "a", "atype", null, 10, true, false, null);
            Assert.IsNotNull(a);

            IElement b = _elementModel.ImportElement(2, "b", "btype", null, 11, true, false, a.Id);
            Assert.IsNotNull(b);

            IElement c = _elementModel.ImportElement(3, "c", "ctype", null, 12, true, false, a.Id);
            Assert.IsNotNull(c);

            // When
            IList<IElement> matches = _elementModel.SearchElements("A.B", _elementModel.GetRootElement(), false, null, true);

            // Then
            Assert.AreEqual(1, matches.Count);
            Assert.AreEqual(b, matches[0]);
        }

        [TestMethod]
        public void GivenHierarchyOfElementsExists_WhenSearchWithElementFilterIsActiveIsCalledWithTextPartOfItsNameAndValidType_ThenElementIsFound()
        {
            // Given
            Assert.AreEqual(1, _elementModel.GetElementCount());

            IElement a = _elementModel.ImportElement(1, "a", "atype", null, 10, true, false, null);
            Assert.IsNotNull(a);

            IElement b = _elementModel.ImportElement(2, "b", "btype", null, 11, true, false, a.Id);
            Assert.IsNotNull(b);

            IElement c = _elementModel.ImportElement(3, "c", "ctype", null, 12, true, false, a.Id);
            Assert.IsNotNull(c);

            IList<IElement> matchesWithoutFilter = _elementModel.SearchElements("a", _elementModel.GetRootElement(), true, null, true);
            Assert.AreEqual(3, matchesWithoutFilter.Count);
            Assert.AreEqual(a, matchesWithoutFilter[0]);
            Assert.AreEqual(b, matchesWithoutFilter[1]);
            Assert.AreEqual(c, matchesWithoutFilter[2]);

            // When
            IList<IElement> matchesWithFilter = _elementModel.SearchElements("a", _elementModel.GetRootElement(), true, "atype", true);

            //Then
            Assert.AreEqual(1, matchesWithFilter.Count);
            Assert.AreEqual(a, matchesWithoutFilter[0]);
        }

        [TestMethod]
        public void GivenHierarchyOfElementsExists_WhenSearchIsCalledWithTextNotPartOfItsName_ThenElementIsNotFound()
        {

            // Given
            Assert.AreEqual(1, _elementModel.GetElementCount());

            IElement a = _elementModel.ImportElement(1, "a", "atype", null, 10, true, false, null);
            Assert.IsNotNull(a);

            IElement b = _elementModel.ImportElement(2, "b", "btype", null, 11, true, false, a.Id);
            Assert.IsNotNull(b);

            IElement c = _elementModel.ImportElement(3, "c", "ctype", null, 12, true, false, a.Id);
            Assert.IsNotNull(c);

            // When
            IList<IElement> matches = _elementModel.SearchElements(".d", _elementModel.GetRootElement(), true, null, true);

            // Then
            Assert.AreEqual(0, matches.Count);
        }

        [TestMethod]
        public void GivenHierarchyOfElementsExists_WhenRemoveElementIsCalled_ThenElementAndItsChildrenAreRemoved()
        {
            // Given
            Assert.AreEqual(1, _elementModel.GetElementCount());

            IElement a = _elementModel.AddElement("a", "", null, null, null);
            Assert.AreEqual(1, a.Id);
            IElement a1 = _elementModel.AddElement("a1", "eta", a.Id, null, null);
            Assert.AreEqual(2, a1.Id);
            IElement a2 = _elementModel.AddElement("a2", "eta", a.Id, null, null);
            Assert.AreEqual(3, a2.Id);

            IElement b = _elementModel.AddElement("b", "", null, null, null);
            Assert.AreEqual(4, b.Id);
            IElement b1 = _elementModel.AddElement("b1", "etb", b.Id, null, null);
            Assert.AreEqual(5, b1.Id);

            Assert.AreEqual(6, _elementModel.GetElementCount());

            List<IElement> rootElementsBefore = _elementModel.GetRootElement().Children.OrderBy(x => x.Id).ToList();
            Assert.AreEqual(2, rootElementsBefore.Count);

            Assert.AreEqual(a, rootElementsBefore[0]);
            Assert.AreEqual(2, rootElementsBefore[0].Children.Count);
            Assert.AreEqual(a1, rootElementsBefore[0].Children[0]);
            Assert.AreEqual(a2, rootElementsBefore[0].Children[1]);

            Assert.AreEqual(b, rootElementsBefore[1]);
            Assert.AreEqual(1, rootElementsBefore[1].Children.Count);
            Assert.AreEqual(b1, rootElementsBefore[1].Children[0]);

            // When
            _elementModel.RemoveElement(a.Id);

            // Then
            Assert.AreEqual(3, _elementModel.GetElementCount());

            List<IElement> rootElementsAfter = _elementModel.GetRootElement().Children.OrderBy(x => x.Id).ToList();
            Assert.AreEqual(1, rootElementsAfter.Count);

            Assert.AreEqual(b, rootElementsAfter[0]);
            Assert.AreEqual(1, rootElementsAfter[0].Children.Count);
            Assert.AreEqual(b1, rootElementsAfter[0].Children[0]);
        }

        [TestMethod]
        public void GivenHierarchyOfElementsExists_WhenRemoveElementIsCalled_ThenAnElementRemovedEventIsTriggeredForTheRemovedElementAndItsChilderen()
        {
            // Given
            Assert.AreEqual(0, _elementRemovedEventCount);

            IElement a = _elementModel.AddElement("a", "", null, null, null);
            Assert.AreEqual(1, a.Id);
            IElement a1 = _elementModel.AddElement("a1", "eta", a.Id, null, null);
            Assert.AreEqual(2, a1.Id);
            IElement a2 = _elementModel.AddElement("a2", "eta", a.Id, null, null);
            Assert.AreEqual(3, a2.Id);

            IElement b = _elementModel.AddElement("b", "", null, null, null);
            Assert.AreEqual(4, b.Id);
            IElement b1 = _elementModel.AddElement("b1", "etb", b.Id, null, null);
            Assert.AreEqual(5, b1.Id);

            // When
            _elementModel.RemoveElement(a.Id);

            // Then
            // Given
            Assert.AreEqual(3, _elementRemovedEventCount);
        }

        [TestMethod]
        public void GivenHierarchyOfElementsExists_WhenUnremoveElementIsCalled_ThenElementAndItsChildrenAreRestored()
        {
            // Given
            Assert.AreEqual(1, _elementModel.GetElementCount());

            IElement a = _elementModel.AddElement("a", "", null, null, null);
            Assert.AreEqual(1, a.Id);
            IElement a1 = _elementModel.AddElement("a1", "eta", a.Id, null, null);
            Assert.AreEqual(2, a1.Id);
            IElement a2 = _elementModel.AddElement("a2", "eta", a.Id, null, null);
            Assert.AreEqual(3, a2.Id);

            IElement b = _elementModel.AddElement("b", "", null, null, null);
            Assert.AreEqual(4, b.Id);
            IElement b1 = _elementModel.AddElement("b1", "etb", b.Id, null, null);
            Assert.AreEqual(5, b1.Id);

            Assert.AreEqual(6, _elementModel.GetElementCount());

            _elementModel.RemoveElement(a.Id);

            Assert.AreEqual(3, _elementModel.GetElementCount());

            List<IElement> rootElementsBefore = _elementModel.GetRootElement().Children.OrderBy(x => x.Id).ToList();
            Assert.AreEqual(1, rootElementsBefore.Count);

            Assert.AreEqual(b, rootElementsBefore[0]);
            Assert.AreEqual(1, rootElementsBefore[0].Children.Count);
            Assert.AreEqual(b1, rootElementsBefore[0].Children[0]);

            // When
            _elementModel.RestoreElement(a.Id);

            // Then
            Assert.AreEqual(6, _elementModel.GetElementCount());

            List<IElement> rootElementsAfter = _elementModel.GetRootElement().Children.OrderBy(x => x.Id).ToList();
            Assert.AreEqual(2, rootElementsAfter.Count);

            Assert.AreEqual(a, rootElementsAfter[0]);
            Assert.AreEqual(2, rootElementsAfter[0].Children.Count);
            Assert.AreEqual(a1, rootElementsAfter[0].Children[0]);
            Assert.AreEqual(a2, rootElementsAfter[0].Children[1]);

            Assert.AreEqual(b, rootElementsAfter[1]);
            Assert.AreEqual(1, rootElementsAfter[1].Children.Count);
            Assert.AreEqual(b1, rootElementsAfter[1].Children[0]);
        }

        [TestMethod]
        public void GivenHierarchyOfElementsExists_WhenUnremoveElementIsCalled_ThenAnElementAddedEventIsTriggeredForTheUnremovedElementAndItsChildren()
        {
            // Given
            IElement a = _elementModel.AddElement("a", "", null, null, null);
            Assert.AreEqual(1, a.Id);
            IElement a1 = _elementModel.AddElement("a1", "eta", a.Id, null, null);
            Assert.AreEqual(2, a1.Id);
            IElement a2 = _elementModel.AddElement("a2", "eta", a.Id, null, null);
            Assert.AreEqual(3, a2.Id);

            IElement b = _elementModel.AddElement("b", "", null, null, null);
            Assert.AreEqual(4, b.Id);
            IElement b1 = _elementModel.AddElement("b1", "etb", b.Id, null, null);
            Assert.AreEqual(5, b1.Id);

            Assert.AreEqual(5, _elementAddedEventCount);

            _elementModel.RemoveElement(a.Id);

            Assert.AreEqual(3, _elementRemovedEventCount);

            // When
            _elementModel.RestoreElement(a.Id);

            // Then
            Assert.AreEqual(8, _elementAddedEventCount);
        }

        [TestMethod]
        public void GivenHierarchyOfElementsExists__WhenRemoveElementIsCalledOnLastChild_ThenParentIsCollapsed()
        {
            // Given
            Assert.AreEqual(1, _elementModel.GetElementCount());

            IElement a = _elementModel.AddElement("a", "", null, null, null);
            Assert.AreEqual(1, a.Id);
            IElement a1 = _elementModel.AddElement("a1", "eta", a.Id, null, null);
            Assert.AreEqual(2, a1.Id);
            Assert.AreEqual(3, _elementModel.GetElementCount());
            Assert.AreEqual(1, a.Children.Count);

            a.IsExpanded = true;

            // When
            _elementModel.RemoveElement(a1.Id);

            // Then
            Assert.AreEqual(2, _elementModel.GetElementCount());
            Assert.AreEqual(0, a.Children.Count);

            Assert.IsFalse(a.IsExpanded);
        }

        [TestMethod]
        public void GivenHierarchyOfElementsExists_WhenGetElementsIsCalled_ThenAllAreReturned()
        {
            // Given
            Assert.AreEqual(1, _elementModel.GetElementCount());

            IElement a = _elementModel.AddElement("a", "", null, null, null);
            Assert.AreEqual(1, a.Id);
            IElement a1 = _elementModel.AddElement("a1", "eta", a.Id, null, null);
            Assert.AreEqual(2, a1.Id);

            IElement b = _elementModel.AddElement("b", "", null, null, null);
            Assert.AreEqual(3, b.Id);
            IElement b1 = _elementModel.AddElement("b1", "etb", b.Id, null, null);
            Assert.AreEqual(4, b1.Id);
            IElement b2 = _elementModel.AddElement("b2", "etb", b.Id, null, null);
            Assert.AreEqual(5, b2.Id);

            IElement c = _elementModel.AddElement("c", "", null, null, null);
            Assert.AreEqual(6, c.Id);
            IElement c1 = _elementModel.AddElement("c1", "etc", c.Id, null, null);
            Assert.AreEqual(7, c1.Id);
            IElement c2 = _elementModel.AddElement("c2", "etc", c.Id, null, null);
            Assert.AreEqual(8, c2.Id);
            IElement c3 = _elementModel.AddElement("c3", "etc", c.Id, null, null);
            Assert.AreEqual(9, c3.Id);

            // When
            List<IElement> rootElements = _elementModel.GetRootElement().Children.OrderBy(x => x.Id).ToList();

            // Then
            Assert.AreEqual(3, rootElements.Count);

            Assert.AreEqual(a, rootElements[0]);
            Assert.AreEqual(1, rootElements[0].Children.Count);
            Assert.AreEqual(a1, rootElements[0].Children[0]);

            Assert.AreEqual(b, rootElements[1]);
            Assert.AreEqual(2, rootElements[1].Children.Count);
            Assert.AreEqual(b1, rootElements[1].Children[0]);
            Assert.AreEqual(b2, rootElements[1].Children[1]);

            Assert.AreEqual(c, rootElements[2]);
            Assert.AreEqual(3, rootElements[2].Children.Count);
            Assert.AreEqual(c1, rootElements[2].Children[0]);
            Assert.AreEqual(c2, rootElements[2].Children[1]);
            Assert.AreEqual(c3, rootElements[2].Children[2]);
        }

        [TestMethod]
        public void GivenHierarchyOfElementsExists_WhenSwapElementIsCalled_ThenOrderIsChanged()
        {
            // Given
            Assert.AreEqual(1, _elementModel.GetElementCount());

            IElement a = _elementModel.AddElement("a", "", null, null, null);
            Assert.AreEqual(1, a.Id);
            IElement a1 = _elementModel.AddElement("a1", "eta", a.Id, null, null);
            Assert.AreEqual(2, a1.Id);
            IElement a2 = _elementModel.AddElement("a2", "eta", a.Id, null, null);
            Assert.AreEqual(3, a2.Id);
            IElement a3 = _elementModel.AddElement("a3", "eta", a.Id, null, null);
            Assert.AreEqual(4, a3.Id);

            List<IElement> rootElementsBefore = _elementModel.GetRootElement().Children.ToList();
            Assert.AreEqual(1, rootElementsBefore.Count);

            Assert.AreEqual(a, rootElementsBefore[0]);
            Assert.AreEqual(3, rootElementsBefore[0].Children.Count);
            Assert.AreEqual(a1, rootElementsBefore[0].Children[0]);
            Assert.AreEqual(a2, rootElementsBefore[0].Children[1]);
            Assert.AreEqual(a3, rootElementsBefore[0].Children[2]);

            // When
            _elementModel.Swap(a1, a2);

            // Then
            List<IElement> rootElementsAfter = _elementModel.GetRootElement().Children.ToList();
            Assert.AreEqual(1, rootElementsAfter.Count);

            Assert.AreEqual(a, rootElementsAfter[0]);
            Assert.AreEqual(3, rootElementsAfter[0].Children.Count);
            Assert.AreEqual(a2, rootElementsAfter[0].Children[0]);
            Assert.AreEqual(a1, rootElementsAfter[0].Children[1]);
            Assert.AreEqual(a3, rootElementsAfter[0].Children[2]);
        }

        [TestMethod]
        public void GivenHierarchyOfElementsExists_WhenReorderElementIsCalled_ThenOrderIsChanged()
        {
            // Given
            Assert.AreEqual(1, _elementModel.GetElementCount());

            IElement a = _elementModel.AddElement("a", "", null, null, null);
            Assert.AreEqual(1, a.Id);
            IElement a1 = _elementModel.AddElement("a1", "eta", a.Id, null, null);
            Assert.AreEqual(2, a1.Id);
            IElement a2 = _elementModel.AddElement("a2", "eta", a.Id, null, null);
            Assert.AreEqual(3, a2.Id);
            IElement a3 = _elementModel.AddElement("a3", "eta", a.Id, null, null);
            Assert.AreEqual(4, a3.Id);
            IElement a4 = _elementModel.AddElement("a4", "eta", a.Id, null, null);
            Assert.AreEqual(5, a4.Id);

            List<IElement> rootElementsBefore = _elementModel.GetRootElement().Children.ToList();
            Assert.AreEqual(1, rootElementsBefore.Count);

            Assert.AreEqual(a, rootElementsBefore[0]);
            Assert.AreEqual(4, rootElementsBefore[0].Children.Count);
            Assert.AreEqual(a1, rootElementsBefore[0].Children[0]);
            Assert.AreEqual(a2, rootElementsBefore[0].Children[1]);
            Assert.AreEqual(a3, rootElementsBefore[0].Children[2]);
            Assert.AreEqual(a4, rootElementsBefore[0].Children[3]);

            // When
            List<int> order =
            [
                1,
                2,
                3,
                0
            ];
            _elementModel.ReorderChildren(a, order);

            // Then
            List<IElement> rootElementsAfter = _elementModel.GetRootElement().Children.ToList();
            Assert.AreEqual(1, rootElementsAfter.Count);

            Assert.AreEqual(a, rootElementsAfter[0]);
            Assert.AreEqual(4, rootElementsAfter[0].Children.Count);
            Assert.AreEqual(a2, rootElementsAfter[0].Children[0]);
            Assert.AreEqual(a3, rootElementsAfter[0].Children[1]);
            Assert.AreEqual(a4, rootElementsAfter[0].Children[2]);
            Assert.AreEqual(a1, rootElementsAfter[0].Children[3]);
        }

        [TestMethod]
        public void GivenHierarchyOfElementsExists_WhenAssignElementOrderIsCalled_ThenElementsHaveOrderValueSetInProperSequence()
        {
            // Given
            Assert.AreEqual(1, _elementModel.GetElementCount());

            IElement a = _elementModel.ImportElement(1, "a", "", null, 0, false, false, null);
            Assert.AreEqual(1, a.Id);
            IElement a1 = _elementModel.ImportElement(2, "a1", "eta", null, 0, false, false, a.Id);
            Assert.AreEqual(2, a1.Id);

            IElement b = _elementModel.ImportElement(3, "b", "", null, 0, false, false, null);
            Assert.AreEqual(3, b.Id);
            IElement b1 = _elementModel.ImportElement(4, "b1", "etb", null, 0, false, false, b.Id);
            Assert.AreEqual(4, b1.Id);
            IElement b2 = _elementModel.ImportElement(5, "b2", "etb", null, 0, false, false, b.Id);
            Assert.AreEqual(5, b2.Id);

            IElement c = _elementModel.ImportElement(6, "c", "", null, 0, false, false, null);
            Assert.AreEqual(6, c.Id);
            IElement c1 = _elementModel.ImportElement(7, "c1", "etc", null, 0, false, false, c.Id);
            Assert.AreEqual(7, c1.Id);
            IElement c2 = _elementModel.ImportElement(8, "c2", "etc", null, 0, false, false, c.Id);
            Assert.AreEqual(8, c2.Id);
            IElement c3 = _elementModel.ImportElement(9, "c3", "etc", null, 0, false, false, c.Id);
            Assert.AreEqual(9, c3.Id);

            Assert.AreEqual(0, a.Order);
            Assert.AreEqual(0, a1.Order);
            Assert.AreEqual(0, b.Order);
            Assert.AreEqual(0, b1.Order);
            Assert.AreEqual(0, b2.Order);
            Assert.AreEqual(0, c.Order);
            Assert.AreEqual(0, c1.Order);
            Assert.AreEqual(0, c2.Order);
            Assert.AreEqual(0, c3.Order);

            // When
            _elementModel.AssignElementOrder();

            // Then
            Assert.AreEqual(1, a.Order);
            Assert.AreEqual(2, a1.Order);
            Assert.AreEqual(3, b.Order);
            Assert.AreEqual(4, b1.Order);
            Assert.AreEqual(5, b2.Order);
            Assert.AreEqual(6, c.Order);
            Assert.AreEqual(7, c1.Order);
            Assert.AreEqual(8, c2.Order);
            Assert.AreEqual(9, c3.Order);
        }

        [TestMethod]
        public void GivenHierarchyOfElementsExists_WhenNextSiblingIsCalledOnOtherElementThanTheLast_ThenNextElementIsReturned()
        {
            // Given
            Assert.AreEqual(1, _elementModel.GetElementCount());

            IElement a = _elementModel.AddElement("a", "", null, null, null);
            Assert.AreEqual(1, a.Id);
            IElement a1 = _elementModel.AddElement("a1", "eta", a.Id, null, null);
            Assert.AreEqual(2, a1.Id);
            IElement a2 = _elementModel.AddElement("a2", "eta", a.Id, null, null);
            Assert.AreEqual(3, a2.Id);
            IElement a3 = _elementModel.AddElement("a3", "eta", a.Id, null, null);
            Assert.AreEqual(4, a3.Id);

            // When/Then
            Assert.AreEqual(a2, _elementModel.NextSibling(a1));
            Assert.AreEqual(a3, _elementModel.NextSibling(a2));
            Assert.AreEqual(null, _elementModel.NextSibling(a3));
        }

        [TestMethod]
        public void GivenHierarchyOfElementsExists_WhenPreviousSiblingIsCalledOnOtherElementThanTheFirst_ThenPreviousElementIsReturned()
        {
            // Given
            Assert.AreEqual(1, _elementModel.GetElementCount());

            IElement a = _elementModel.AddElement("a", "", null, null, null);
            Assert.AreEqual(1, a.Id);
            IElement a1 = _elementModel.AddElement("a1", "eta", a.Id, null, null);
            Assert.AreEqual(2, a1.Id);
            IElement a2 = _elementModel.AddElement("a2", "eta", a.Id, null, null);
            Assert.AreEqual(3, a2.Id);
            IElement a3 = _elementModel.AddElement("a3", "eta", a.Id, null, null);
            Assert.AreEqual(4, a3.Id);

            // When/Then
            Assert.AreEqual(null, _elementModel.PreviousSibling(a1));
            Assert.AreEqual(a1, _elementModel.PreviousSibling(a2));
            Assert.AreEqual(a2, _elementModel.PreviousSibling(a3));
        }

        [TestMethod]
        public void GivenNoElementsExist_WhenThreeElementsAreImportedWithDifferentType_ThenThreeElementTypesAndEmptyTypeExist()
        {
            // Given
            Assert.AreEqual(1, _elementModel.GetElementCount());

            // When
            IElement a = _elementModel.ImportElement(1, "a", "atype", null, 10, true, false, null);
            Assert.IsNotNull(a);

            IElement b = _elementModel.ImportElement(2, "b", "btype", null, 11, true, false, a.Id);
            Assert.IsNotNull(b);

            IElement c = _elementModel.ImportElement(3, "c", "ctype", null, 12, true, false, a.Id);
            Assert.IsNotNull(c);

            // Then
            Assert.AreEqual(4, _elementModel.GetElementCount());

            List<string> elementTypes = _elementModel.GetElementTypes().ToList();
            Assert.AreEqual(4, elementTypes.Count);
            Assert.AreEqual("", elementTypes[0]);
            Assert.AreEqual("atype", elementTypes[1]);
            Assert.AreEqual("btype", elementTypes[2]);
            Assert.AreEqual("ctype", elementTypes[3]);
        }

        [TestMethod]
        public void GivenOnlyRootElementExists_WhenThreeElementsAreImportedWithSameType_ThenOneElementTypeAndEmptyTypeExists()
        {
            // Given
            Assert.AreEqual(1, _elementModel.GetElementCount());

            // When
            IElement a = _elementModel.ImportElement(1, "a", "atype", null, 10, true, false, null);
            Assert.IsNotNull(a);

            IElement b = _elementModel.ImportElement(2, "b", "atype", null, 11, true, false, a.Id);
            Assert.IsNotNull(b);

            IElement c = _elementModel.ImportElement(3, "c", "atype", null, 12, true, false, a.Id);
            Assert.IsNotNull(c);

            // Then
            Assert.AreEqual(4, _elementModel.GetElementCount());

            List<string> elementTypes = _elementModel.GetElementTypes().ToList();
            Assert.AreEqual(2, elementTypes.Count);
            Assert.AreEqual("", elementTypes[0]);
            Assert.AreEqual("atype", elementTypes[1]);
        }

        [TestMethod]
        public void GivenOnlyRootElementExists_WhenThreeElementsAreAddedWithDifferentType_ThenThreeElementTypesAndEmptyTypeExist()
        {
            // Given
            Assert.AreEqual(1, _elementModel.GetElementCount());

            // When
            IElement a = _elementModel.AddElement("a", "atype", null, 0, null);
            Assert.IsNotNull(a);

            IElement b = _elementModel.AddElement("b", "btype", null, 0, null);
            Assert.IsNotNull(b);

            IElement c = _elementModel.AddElement("c", "ctype", null, 0, null);
            Assert.IsNotNull(c);

            //Then
            Assert.AreEqual(4, _elementModel.GetElementCount());

            List<string> elementTypes = _elementModel.GetElementTypes().ToList();
            Assert.AreEqual(4, elementTypes.Count);
            Assert.AreEqual("", elementTypes[0]);
            Assert.AreEqual("atype", elementTypes[1]);
            Assert.AreEqual("btype", elementTypes[2]);
            Assert.AreEqual("ctype", elementTypes[3]);
        }

        [TestMethod]
        public void GivenOnlyRootElementExists_WhenThreeElementsAreAddWithSameType_ThenOneElementTypeAndEmptyTypeExist()
        {
            // Given
            Assert.AreEqual(1, _elementModel.GetElementCount());

            // When
            IElement a = _elementModel.AddElement("a", "atype", null, 0, null);
            Assert.IsNotNull(a);

            IElement b = _elementModel.AddElement("b", "atype", null, 0, null);
            Assert.IsNotNull(b);

            IElement c = _elementModel.AddElement("c", "atype", null, 0, null);
            Assert.IsNotNull(c);

            // Then
            Assert.AreEqual(4, _elementModel.GetElementCount());

            List<string> elementTypes = _elementModel.GetElementTypes().ToList();
            Assert.AreEqual(2, elementTypes.Count);
            Assert.AreEqual("", elementTypes[0]);
            Assert.AreEqual("atype", elementTypes[1]);
        }

        [TestMethod]
        public void GivenOnlyRootElementExists_WhenThreeElementsAreAdded_ThenThreeElementsExistsWithNoOrderAssignedYet()
        {
            // Given
            Assert.AreEqual(1, _elementModel.GetElements().Count());

            // When
            IElement a = _elementModel.AddElement("a", "atype", null, 0, null);
            Assert.IsNotNull(a);

            IElement b = _elementModel.AddElement("b", "btype", null, 0, null);
            Assert.IsNotNull(b);

            IElement c = _elementModel.AddElement("c", "ctype", null, 0, null);
            Assert.IsNotNull(c);

            // Then
            List<IElement> elements = _elementModel.GetElements().OrderBy(x => x.Id).ToList();
            Assert.AreEqual(4, elements.Count);

            Assert.AreEqual(0, elements[0].Id);
            Assert.AreEqual("", elements[0].Name);
            Assert.AreEqual("", elements[0].Type);
            Assert.AreEqual(0, elements[0].Order);
            Assert.IsNull(elements[0].Parent); // root element

            Assert.AreEqual(1, elements[1].Id);
            Assert.AreEqual("a", elements[1].Name);
            Assert.AreEqual("atype", elements[1].Type);
            Assert.AreEqual(0, elements[1].Order);
            Assert.AreEqual(_elementModel.GetRootElement(), elements[1].Parent);

            Assert.AreEqual(2, elements[2].Id);
            Assert.AreEqual("b", elements[2].Name);
            Assert.AreEqual("btype", elements[2].Type);
            Assert.AreEqual(0, elements[2].Order);
            Assert.AreEqual(_elementModel.GetRootElement(), elements[2].Parent);

            Assert.AreEqual(3, elements[3].Id);
            Assert.AreEqual("c", elements[3].Name);
            Assert.AreEqual("ctype", elements[3].Type);
            Assert.AreEqual(0, elements[3].Order);
            Assert.AreEqual(_elementModel.GetRootElement(), elements[3].Parent);
        }
    }
}
