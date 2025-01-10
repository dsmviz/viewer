

using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Viewer.Data.Model.Common;
using Dsmviz.Viewer.Data.Model.Entities;

namespace Dsmviz.Test.Data.Model.Entities
{
    [TestClass]
    public class ElementTest
    {
        private readonly TypeNameRegistration _elementTypeNameRegistration = new();
        private readonly PropertyNameRegistration _elementPropertyNameRegistration = new();

        [TestMethod]
        public void WhenElementIsConstructedWithoutProperties_ThenElementAccordingInputArguments()
        {
            // When
            int elementId = 1;
            string elementName = "name1";
            string elementType = "file";
            Element element = CreateElement(elementId, elementName, elementType, null);

            // Then
            Assert.AreEqual(elementId, element.Id);
            Assert.AreEqual(elementName, element.Name);
            Assert.AreEqual(elementType, element.Type);
            Assert.AreEqual(0, element.Order);
            Assert.AreEqual(elementName, element.Fullname);
            Assert.IsNull(element.Properties);
        }

        [TestMethod]
        public void WhenElementIsConstructedWithProperties_ThenElementAccordingInputArguments()
        {
            // When
            Dictionary<string, string> elementProperties = new Dictionary<string, string>
            {
                ["annotation"] = "some text",
                ["version"] = "1.0"
            };
            int elementId = 1;
            string elementName = "name1";
            string elementType = "file";
            Element element = CreateElement(elementId, elementName, elementType, elementProperties);

            // Then
            Assert.AreEqual(elementId, element.Id);
            Assert.AreEqual(elementName, element.Name);
            Assert.AreEqual(elementType, element.Type);
            Assert.AreEqual(0, element.Order);
            Assert.AreEqual(elementName, element.Fullname);
            Assert.IsNotNull(element.Properties);
            Assert.AreEqual(2, element.Properties.Count);
            Assert.AreEqual("some text", element.Properties["annotation"]);
            Assert.AreEqual("1.0", element.Properties["version"]);
        }

        [TestMethod]
        public void WhenElementHasNoParent_ThenItIsRoot()
        {
            // When
            int elementId = 1;
            string elementName = "elementName";
            Element element = CreateElement(elementId, elementName, "", null);

            // Then
            Assert.AreEqual(null, element.Parent);
            Assert.IsTrue(element.IsRoot);
        }

        [TestMethod]
        public void WhenElementHasParent_ThenItIsNotRoot()
        {
            // When
            int parentId = 1;
            string parentName = "parent";
            Element parent = CreateElement(parentId, parentName, "", null);
            Assert.AreEqual(null, parent.Parent);
            Assert.AreEqual(0, parent.Children.Count);

            int child1Id = 10;
            string childName = "child";
            Element child = CreateElement(child1Id, childName, "", null);
            Assert.AreEqual(null, child.Parent);

            parent.InsertChildAtEnd(child);

            // Then
            Assert.AreEqual(parent, child.Parent);
            Assert.IsFalse(child.IsRoot);
        }

        [TestMethod]
        public void GivenElementHierarchy_WhenFullnameIsRetrieved_ThenCombinedNameIsReturned()
        {
            // When
            int parentId = 1;
            string parentName = "parent";
            Element parent = CreateElement(parentId, parentName, "", null);
            Assert.AreEqual(null, parent.Parent);
            Assert.AreEqual(0, parent.Children.Count);

            int childId = 10;
            string childName = "child";
            Element child = CreateElement(childId, childName, "", null);
            Assert.AreEqual(null, child.Parent);

            int subChild1Id = 10;
            string subChildName = "subchild";
            Element subChild = CreateElement(subChild1Id, subChildName, "", null);
            Assert.AreEqual(null, subChild.Parent);

            parent.InsertChildAtEnd(child);
            child.InsertChildAtEnd(subChild);

            // When
            string fullname = subChild.Fullname;

            // Then
            Assert.AreEqual("parent.child.subchild", fullname);
        }

        [TestMethod]
        public void GivenElementHierarchy_WhenCheckedIfContainsChildWithValidName_ThenReturnsTrueForDirectChildOnly()
        {
            // When
            int parentId = 1;
            string parentName = "parent";
            Element parent = CreateElement(parentId, parentName, "", null);
            Assert.AreEqual(null, parent.Parent);
            Assert.AreEqual(0, parent.Children.Count);

            int childId = 10;
            string childName = "child";
            Element child = CreateElement(childId, childName, "", null);
            Assert.AreEqual(null, child.Parent);

            int subChild1Id = 10;
            string subChildName = "subchild";
            Element subChild = CreateElement(subChild1Id, subChildName, "", null);
            Assert.AreEqual(null, subChild.Parent);

            parent.InsertChildAtEnd(child);
            child.InsertChildAtEnd(subChild);

            // When
            bool containsChildName = parent.ContainsChildWithName(childName);
            bool containsSubChildName = parent.ContainsChildWithName(subChildName);

            // Then
            Assert.IsTrue(containsChildName);
            Assert.IsFalse(containsSubChildName);
        }

        [TestMethod]
        public void GivenElementHierarchy_WhenCheckedIfContainsChildWithInvalidName_ThenReturnsFalse()
        {
            // When
            int parentId = 1;
            string parentName = "parent";
            Element parent = CreateElement(parentId, parentName, "", null);
            Assert.AreEqual(null, parent.Parent);
            Assert.AreEqual(0, parent.Children.Count);

            int childId = 10;
            string childName = "child";
            Element child = CreateElement(childId, childName, "", null);
            Assert.AreEqual(null, child.Parent);

            int subChild1Id = 10;
            string subChildName = "subchild";
            Element subChild = CreateElement(subChild1Id, subChildName, "", null);
            Assert.AreEqual(null, subChild.Parent);


            parent.InsertChildAtEnd(child);
            child.InsertChildAtEnd(subChild);

            // When
            string invalidChildName = "somename";
            bool containsChildWithName = parent.ContainsChildWithName(invalidChildName);

            // Then
            Assert.IsFalse(containsChildWithName);
        }

        [TestMethod]
        public void GivenElementHierarchy_WhenRelativeNameIsRetrieved_ThenLastPartOfCombinedNameIsReturned()
        {
            // When
            int parentId = 1;
            string parentName = "parent";
            Element parent = CreateElement(parentId, parentName, "", null);
            Assert.AreEqual(null, parent.Parent);
            Assert.AreEqual(0, parent.Children.Count);

            int childId = 10;
            string childName = "child";
            Element child = CreateElement(childId, childName, "", null);
            Assert.AreEqual(null, child.Parent);

            int subChild1Id = 10;
            string subChildName = "subchild";
            Element subChild = CreateElement(subChild1Id, subChildName, "", null);
            Assert.AreEqual(null, subChild.Parent);

            parent.InsertChildAtEnd(child);
            child.InsertChildAtEnd(subChild);

            // When
            string relativeName = subChild.GetRelativeName(parent);

            // Then
            Assert.AreEqual("child.subchild", relativeName);
        }

        [TestMethod]
        public void GivenElementHierarchy_WhenGetElementAndItsChildren_ThenDictionaryWithAllElementWithIdAsKeyIsReturned()
        {
            // When
            int parentId = 1;
            string parentName = "parent";
            Element parent = CreateElement(parentId, parentName, "", null);

            int child1Id = 2;
            string child1Name = "child1";
            Element child1 = CreateElement(child1Id, child1Name, "", null);

            int child2Id = 3;
            string child2Name = "child2";
            Element child2 = CreateElement(child2Id, child2Name, "", null);

            int subChild11Id = 4;
            string subChild11Name = "subchild";
            Element subChild11 = CreateElement(subChild11Id, subChild11Name, "", null);

            int subChild12Id = 5;
            string subChild12Name = "subchild";
            Element subChild12 = CreateElement(subChild12Id, subChild12Name, "", null);

            parent.InsertChildAtEnd(child1);
            parent.InsertChildAtEnd(child2);
            child1.InsertChildAtEnd(subChild11);
            child1.InsertChildAtEnd(subChild12);

            // When
            IList<IElement> children = parent.GetSelfAndChildrenRecursive();

            // Then
            Assert.AreEqual(5, children.Count);
            Assert.AreEqual(parent, children[0]);
            Assert.AreEqual(child1, children[1]);
            Assert.AreEqual(subChild11, children[2]);
            Assert.AreEqual(subChild12, children[3]);
            Assert.AreEqual(child2, children[4]);
        }

        [TestMethod]
        public void GivenElementHierarchy_WhenChildCountIsRetrieved_ThenOnlyDirectChildrenAreCounted()
        {
            // When
            int parentId = 1;
            string parentName = "parent";
            Element parent = CreateElement(parentId, parentName, "", null);

            int child1Id = 2;
            string child1Name = "child1";
            Element child1 = CreateElement(child1Id, child1Name, "", null);

            int child2Id = 3;
            string child2Name = "child2";
            Element child2 = CreateElement(child2Id, child2Name, "", null);

            int subChild11Id = 4;
            string subChild11Name = "subchild";
            Element subChild11 = CreateElement(subChild11Id, subChild11Name, "", null);

            int subChild12Id = 5;
            string subChild12Name = "subchild";
            Element subChild12 = CreateElement(subChild12Id, subChild12Name, "", null);

            parent.InsertChildAtEnd(child1);
            parent.InsertChildAtEnd(child2);
            child1.InsertChildAtEnd(subChild11);
            child1.InsertChildAtEnd(subChild12);

            // When
            int directChildCount = parent.ChildCount;

            // Then
            Assert.AreEqual(2, directChildCount);
        }

        [TestMethod]
        public void GivenElementHierarchy_WhenTotalElementCountIsRetrieved_ThenTheParentAndAllItsChildrenAreCounted()
        {
            // When
            int parentId = 1;
            string parentName = "parent";
            Element parent = CreateElement(parentId, parentName, "", null);

            int child1Id = 2;
            string child1Name = "child1";
            Element child1 = CreateElement(child1Id, child1Name, "", null);

            int child2Id = 3;
            string child2Name = "child2";
            Element child2 = CreateElement(child2Id, child2Name, "", null);

            int subChild11Id = 4;
            string subChild11Name = "subchild";
            Element subChild11 = CreateElement(subChild11Id, subChild11Name, "", null);

            int subChild12Id = 5;
            string subChild12Name = "subchild";
            Element subChild12 = CreateElement(subChild12Id, subChild12Name, "", null);

            parent.InsertChildAtEnd(child1);
            parent.InsertChildAtEnd(child2);
            child1.InsertChildAtEnd(subChild11);
            child1.InsertChildAtEnd(subChild12);

            // When
            int recursiveChildCount = parent.TotalElementCount;

            // Then
            Assert.AreEqual(5, recursiveChildCount);
        }

        [TestMethod]
        public void GivenFourElements_WhenThreeChildrenAreAddedToParent_ThenParentHasThreeChildren()
        {
            // Given
            int parentId = 1;
            string parentName = "parent";
            Element parent = CreateElement(parentId, parentName, "", null);

            int child1Id = 10;
            string child1Name = "child1";
            Element child1 = CreateElement(child1Id, child1Name, "", null);

            int child2Id = 100;
            string child2Name = "child2";
            Element child2 = CreateElement(child2Id, child2Name, "", null);

            int child3Id = 1000;
            string child3Name = "child3";
            Element child3 = CreateElement(child3Id, child3Name, "", null);

            // When
            parent.InsertChildAtEnd(child1);
            parent.InsertChildAtEnd(child2);
            parent.InsertChildAtEnd(child3);

            // Then
            Assert.IsTrue(parent.HasChildren);
            Assert.AreEqual(3, parent.Children.Count);
            Assert.AreEqual(child1, parent.Children[0]);
            Assert.AreEqual(child2, parent.Children[1]);
            Assert.AreEqual(child3, parent.Children[2]);
            Assert.AreEqual(parent, child3.Parent);
            Assert.IsTrue(child3.IsRecursiveChildOf(parent));
        }

        [TestMethod]
        public void GivenElementHierarchyWithThreeChildren_WhenOneChildIsRemoved_ThenParentHasTwoChildren()
        {
            // Given
            int parentId = 1;
            string parentName = "parent";
            Element parent = CreateElement(parentId, parentName, "", null);

            int child1Id = 10;
            string child1Name = "child1";
            Element child1 = CreateElement(child1Id, child1Name, "", null);

            int child2Id = 100;
            string child2Name = "child2";
            Element child2 = CreateElement(child2Id, child2Name, "", null);

            int child3Id = 1000;
            string child3Name = "child3";
            Element child3 = CreateElement(child3Id, child3Name, "", null);

            parent.InsertChildAtEnd(child1);
            parent.InsertChildAtEnd(child2);
            parent.InsertChildAtEnd(child3);

            Assert.AreEqual(3, parent.Children.Count);

            // When
            parent.RemoveChild(child2);

            //Then
            Assert.AreEqual(parent, child1.Parent);
            Assert.AreEqual(null, child2.Parent);
            Assert.AreEqual(parent, child3.Parent);

            Assert.IsTrue(child1.IsRecursiveChildOf(parent));
            Assert.IsFalse(child2.IsRecursiveChildOf(parent));
            Assert.IsTrue(child3.IsRecursiveChildOf(parent));

            Assert.IsTrue(parent.HasChildren);
            Assert.AreEqual(2, parent.Children.Count);
            Assert.AreEqual(child1, parent.Children[0]);
            Assert.AreEqual(child3, parent.Children[1]);
        }

        [TestMethod]
        public void GivenElementHierarchyWithThreeChildren_WhenAllChildrenAreRemoved_ThenParentHasNoChildren()
        {
            // Given
            int parentId = 1;
            string parentName = "parent";
            Element parent = CreateElement(parentId, parentName, "", null);

            int child1Id = 10;
            string child1Name = "child1";
            Element child1 = CreateElement(child1Id, child1Name, "", null);

            int child2Id = 100;
            string child2Name = "child2";
            Element child2 = CreateElement(child2Id, child2Name, "", null);

            int child3Id = 1000;
            string child3Name = "child3";
            Element child3 = CreateElement(child3Id, child3Name, "", null);

            parent.InsertChildAtEnd(child1);
            parent.InsertChildAtEnd(child2);
            parent.InsertChildAtEnd(child3);

            Assert.AreEqual(3, parent.Children.Count);

            // When
            parent.RemoveAllChildren();

            // Then
            Assert.AreEqual(null, child1.Parent);
            Assert.AreEqual(null, child2.Parent);
            Assert.AreEqual(null, child2.Parent);

            Assert.IsFalse(child1.IsRecursiveChildOf(parent));
            Assert.IsFalse(child2.IsRecursiveChildOf(parent));
            Assert.IsFalse(child3.IsRecursiveChildOf(parent));

            Assert.IsFalse(parent.HasChildren);
            Assert.AreEqual(0, parent.Children.Count);
        }

        [TestMethod]
        public void GivenElementHierarchyWithThreeChildren_WhenOneChildIsMarkedAsDeleted_ThenItsHasTwoChildren()
        {
            // Given
            int parentId = 1;
            string parentName = "parent";
            Element parent = CreateElement(parentId, parentName, "", null);

            int child1Id = 10;
            string child1Name = "child1";
            Element child1 = CreateElement(child1Id, child1Name, "", null);

            int child2Id = 100;
            string child2Name = "child2";
            Element child2 = CreateElement(child2Id, child2Name, "", null);

            int child3Id = 1000;
            string child3Name = "child3";
            Element child3 = CreateElement(child3Id, child3Name, "", null);

            parent.InsertChildAtEnd(child1);
            parent.InsertChildAtEnd(child2);
            parent.InsertChildAtEnd(child3);

            Assert.AreEqual(3, parent.Children.Count);
            Assert.AreEqual(3, parent.ChildrenIncludingDeletedOnes.Count);

            // When
            child2.IsDeleted = true;

            // Then
            Assert.AreEqual(2, parent.Children.Count);
            Assert.AreEqual(3, parent.ChildrenIncludingDeletedOnes.Count);
        }

        [TestMethod]
        public void GivenElementHierarchyWithTwoChildren_WhenChildInsertAtIndex_ThenParentHasThreeChildren()
        {
            // Given
            int parentId = 1;
            string parentName = "parent";
            Element parent = CreateElement(parentId, parentName, "", null);

            int child1Id = 10;
            string child1Name = "child1";
            Element child1 = CreateElement(child1Id, child1Name, "", null);

            int child2Id = 100;
            string child2Name = "child2";
            Element child2 = CreateElement(child2Id, child2Name, "", null);

            int child3Id = 1000;
            string child3Name = "child3";
            Element child3 = CreateElement(child3Id, child3Name, "", null);

            parent.InsertChildAtEnd(child1);
            parent.InsertChildAtEnd(child3);

            Assert.AreEqual(2, parent.Children.Count);

            // When
            parent.InsertChildAtIndex(child2, 1);

            // Then
            Assert.AreEqual(3, parent.Children.Count);
            Assert.AreEqual(child1, parent.Children[0]);
            Assert.AreEqual(child2, parent.Children[1]);
            Assert.AreEqual(child3, parent.Children[2]);

            Assert.AreEqual(0, parent.IndexOfChild(child1));
            Assert.AreEqual(1, parent.IndexOfChild(child2));
            Assert.AreEqual(2, parent.IndexOfChild(child3));
        }

        [TestMethod]
        public void GivenElementHierarchyWithThreeElements_WhenSwapCalledUsingFirstAndSecondChild_ThenThePositionAreChangedAndTrueIsReturned()
        {
            // Given
            int parentId = 1;
            string parentName = "parent";
            Element parent = CreateElement(parentId, parentName, "", null);

            int child1Id = 2;
            string child1Name = "child1";
            Element child1 = CreateElement(child1Id, child1Name, "", null);

            int child2Id = 3;
            string child2Name = "child2";
            Element child2 = CreateElement(child2Id, child2Name, "", null);

            int child3Id = 4;
            string child3Name = "child3";
            Element child3 = CreateElement(child3Id, child3Name, "", null);

            parent.InsertChildAtEnd(child1);
            parent.InsertChildAtEnd(child2);
            parent.InsertChildAtEnd(child3);

            Assert.AreEqual(3, parent.Children.Count);
            Assert.AreEqual(child1, parent.Children[0]);
            Assert.AreEqual(child2, parent.Children[1]);
            Assert.AreEqual(child3, parent.Children[2]);

            // When
            bool result = parent.Swap(child1, child2);

            // Then
            Assert.IsTrue(result);
            Assert.AreEqual(3, parent.Children.Count);
            Assert.AreEqual(child2, parent.Children[0]);
            Assert.AreEqual(child1, parent.Children[1]);
            Assert.AreEqual(child3, parent.Children[2]);
        }

        [TestMethod]
        public void GivenElementHierarchyWithTwoElements_WhenSwapCalledUsingThirdElementAsFirstArgument_ThenThePositionsAreUnchangedAndFalseIsReturned()
        {
            // Given
            int parentId = 1;
            string parentName = "parent";
            Element parent = CreateElement(parentId, parentName, "", null);

            int child1Id = 2;
            string child1Name = "child1";
            Element child1 = CreateElement(child1Id, child1Name, "", null);

            int child2Id = 3;
            string child2Name = "child2";
            Element child2 = CreateElement(child2Id, child2Name, "", null);

            int child3Id = 4;
            string child3Name = "child3";
            Element child3 = CreateElement(child3Id, child3Name, "", null);

            parent.InsertChildAtEnd(child1);
            parent.InsertChildAtEnd(child2);

            Assert.AreEqual(2, parent.Children.Count);
            Assert.AreEqual(child1, parent.Children[0]);
            Assert.AreEqual(child2, parent.Children[1]);

            // When
            bool result = parent.Swap(child3, child2);

            // Then
            Assert.IsFalse(result);
            Assert.AreEqual(2, parent.Children.Count);
            Assert.AreEqual(child1, parent.Children[0]);
            Assert.AreEqual(child2, parent.Children[1]);
        }

        [TestMethod]
        public void GivenElementHierarchyWithTwoElements_WhenSwapCalledUsingThirdElementAsSecondArgument_ThenThePositionsAreUnchangedAndFalseIsReturned()
        {
            // Given
            int parentId = 1;
            string parentName = "parent";
            Element parent = CreateElement(parentId, parentName, "", null);

            int child1Id = 2;
            string child1Name = "child1";
            Element child1 = CreateElement(child1Id, child1Name, "", null);

            int child2Id = 3;
            string child2Name = "child2";
            Element child2 = CreateElement(child2Id, child2Name, "", null);

            int child3Id = 4;
            string child3Name = "child3";
            Element child3 = CreateElement(child3Id, child3Name, "", null);

            parent.InsertChildAtEnd(child1);
            parent.InsertChildAtEnd(child2);

            Assert.AreEqual(2, parent.Children.Count);
            Assert.AreEqual(child1, parent.Children[0]);
            Assert.AreEqual(child2, parent.Children[1]);

            // When
            bool result = parent.Swap(child1, child3);

            // Then
            Assert.IsFalse(result);
            Assert.AreEqual(2, parent.Children.Count);
            Assert.AreEqual(child1, parent.Children[0]);
            Assert.AreEqual(child2, parent.Children[1]);
        }

        [TestMethod]
        public void GivenElement_WhenIsDeletedIsSet_TheIsDeletedIsReturned()
        {
            // Given
            int elementId = 1;
            string elementName = "elementName";
            Element element = CreateElement(elementId, elementName, "", null);
            Assert.IsFalse(element.IsDeleted);

            //When
            element.IsDeleted = true;

            Assert.IsTrue(element.IsDeleted);
        }

        [TestMethod]
        public void GivenAnElement_WhenIsExpandedIsSet_TheIsExpandedIsReturned()
        {
            // Given
            int elementId = 1;
            string elementName = "elementName";
            Element element = CreateElement(elementId, elementName, "", null);
            Assert.IsFalse(element.IsExpanded);

            //When
            element.IsExpanded = true;

            Assert.IsTrue(element.IsExpanded);
        }

        [TestMethod]
        public void GivenAnElement_WhenIsBookmarkedIsSet_TheIsBookmarkedIsReturned()
        {
            // Given
            int elementId = 1;
            string elementName = "elementName";
            Element element = CreateElement(elementId, elementName, "", null);
            Assert.IsFalse(element.IsBookmarked);

            //When
            element.IsBookmarked = true;

            Assert.IsTrue(element.IsBookmarked);
        }

        [TestMethod]
        public void GivenAnElement_WhenIsMatchIsSet_TheIsMatchIsReturned()
        {
            // Given
            int elementId = 1;
            string elementName = "elementName";
            Element element = CreateElement(elementId, elementName, "", null);
            Assert.IsFalse(element.IsMatch);

            //When
            element.IsMatch = true;

            Assert.IsTrue(element.IsMatch);
        }

        [TestMethod]
        public void GivenAnElement_WhenTypeIsSet_TheTypeIsReturned()
        {
            // Given
            int elementId = 1;
            string elementName = "elementName";
            string oldType = "oldtype";
            Element element = CreateElement(elementId, elementName, oldType, null);
            Assert.AreEqual(oldType, element.Type);

            //When
            string newType = "newtype";
            element.Type = newType;

            Assert.AreEqual(newType, element.Type);
        }

        private Element CreateElement(int id, string name, string type, IDictionary<string, string> properties, int order = 0, bool isExpanded = false)
        {
            return new Element(_elementTypeNameRegistration, _elementPropertyNameRegistration, id, name, type, properties, order, isExpanded);
        }
    }
}
