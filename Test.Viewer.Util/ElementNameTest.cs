using Dsmviz.Viewer.Utils;

namespace Dsmviz.Test.Util
{
    [TestClass]
    public class ElementNameTest
    {
        [TestMethod]
        public void WhenElementNameIsConstructedWithNoArgument_ThenItsHasOneNamePartWhichIsAnEmptyString()
        {
            // When
            ElementName elementName = new ElementName();

            // Then
            Assert.AreEqual("", elementName.FullName);
            Assert.AreEqual("", elementName.ParentName);
            Assert.AreEqual("", elementName.LastNamePart);
            Assert.AreEqual(1, elementName.NamePartCount);
        }

        [TestMethod]
        public void WhenElementNameIsConstructedWithSingleMultiPartArgument_ThenItsHasMultipleNameParts()
        {
            // Given
            ElementName elementName = new ElementName("a.b.c");

            // Then
            Assert.AreEqual("a.b.c", elementName.FullName);
            Assert.AreEqual("a.b", elementName.ParentName);
            Assert.AreEqual("c", elementName.LastNamePart);
            Assert.AreEqual(3, elementName.NamePartCount);
            Assert.AreEqual("a", elementName.NameParts[0]);
            Assert.AreEqual("b", elementName.NameParts[1]);
            Assert.AreEqual("c", elementName.NameParts[2]);
        }

        [TestMethod]
        public void WhenElementNameIsConstructedWithTwoArguments_ThenItsHasTheJoinedMultipleNameParts()
        {
            // Given
            ElementName elementName = new ElementName("a.b", "c");

            // Then
            Assert.AreEqual("a.b.c", elementName.FullName);
            Assert.AreEqual("a.b", elementName.ParentName);
            Assert.AreEqual("c", elementName.LastNamePart);
            Assert.AreEqual(3, elementName.NamePartCount);
            Assert.AreEqual("a", elementName.NameParts[0]);
            Assert.AreEqual("b", elementName.NameParts[1]);
            Assert.AreEqual("c", elementName.NameParts[2]);
        }

        [TestMethod]
        public void GivenAnEmptyElementName_WhenAddPartIsCalled_ThenItsHasOneNamePart()
        {
            // Given
            ElementName elementName = new ElementName();

            // When
            elementName.AddNamePart("a");

            // Then
            Assert.AreEqual("a", elementName.FullName);
            Assert.AreEqual("", elementName.ParentName);
            Assert.AreEqual("a", elementName.LastNamePart);
            Assert.AreEqual(1, elementName.NamePartCount);
            Assert.AreEqual("a", elementName.NameParts[0]);
        }

        [TestMethod]
        public void GivenFilledElementName_WhenAddPartIsCalled_ThenItsHasOneAdditionalNamePart()
        {
            // Give
            ElementName elementName = new ElementName("a.b");

            // When
            elementName.AddNamePart("c");

            // Then
            Assert.AreEqual("a.b.c", elementName.FullName);
            Assert.AreEqual("a.b", elementName.ParentName);
            Assert.AreEqual("c", elementName.LastNamePart);
            Assert.AreEqual(3, elementName.NamePartCount);
            Assert.AreEqual("a", elementName.NameParts[0]);
            Assert.AreEqual("b", elementName.NameParts[1]);
            Assert.AreEqual("c", elementName.NameParts[2]);
        }
    }
}
