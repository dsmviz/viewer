

using Dsmviz.Viewer.Data.Model.Common;

namespace Dsmviz.Test.Data.Model.Common
{
    [TestClass]
    public class TypeNameRegistrationTest
    {
        [TestMethod]
        public void WhenNameRegistrationIsConstructed_ThenOnlyEmptyTypeCanBeFound()
        {
            // When
            TypeNameRegistration registration = new TypeNameRegistration();

            // Then
            List<string> registeredNames = registration.GetRegisteredNames().ToList();
            Assert.AreEqual(1, registeredNames.Count);
            Assert.AreEqual("", registeredNames[0]);
        }

        [TestMethod]
        public void GivenNoRegisteredNames_WhenThreeNamesAreRegistered_ThenAllThreeNamesAndEmptyTypeCanBeFound()
        {
            // Given
            TypeNameRegistration registration = new TypeNameRegistration();

            // When
            string typeA = "typeA";
            string typeB = "typeB";
            string typeC = "typeC";
            registration.RegisterName(typeA);
            registration.RegisterName(typeB);
            registration.RegisterName(typeC);

            //Then
            List<string> registeredNames = registration.GetRegisteredNames().ToList();
            Assert.AreEqual(4, registeredNames.Count);
            Assert.AreEqual("", registeredNames[0]);
            Assert.AreEqual(typeA, registeredNames[1]);
            Assert.AreEqual(typeB, registeredNames[2]);
            Assert.AreEqual(typeC, registeredNames[3]);
        }

        [TestMethod]
        public void GivenNoRegisteredNames_WhenThreeNamesAreRegistered_ThenAllThreeAndEmptyTypeCanBeFound()
        {
            // Given
            TypeNameRegistration registration = new TypeNameRegistration();

            // When
            string typeA = "typeA";
            string typeB = "typeB";
            string typeC = "typeC";
            char ida = registration.RegisterName(typeA);
            char idb = registration.RegisterName(typeB);
            char idc = registration.RegisterName(typeC);

            //Then
            Assert.AreEqual("", registration.GetRegisteredName((char)0));
            Assert.AreEqual(typeA, registration.GetRegisteredName(ida));
            Assert.AreEqual(typeB, registration.GetRegisteredName(idb));
            Assert.AreEqual(typeC, registration.GetRegisteredName(idc));
        }

        [TestMethod]
        public void GivenNoRegisteredNames_WhenNameIsRegisteredTwice_ThenItsIsOnlyOneStoredTheFirstTime()
        {
            // Given
            TypeNameRegistration registration = new TypeNameRegistration();

            // When
            string typeA = "typeA";
            char ida1 = registration.RegisterName(typeA);
            char ida2 = registration.RegisterName(typeA);

            //Then
            Assert.AreEqual(ida1, ida2);
            Assert.AreEqual(typeA, registration.GetRegisteredName(ida1));
        }

        [TestMethod]
        public void GivenNoRegisteredNames_WhenRegisteredNameIsRetrievedWithInvalidId_ThenEmptyValueIsReturned()
        {
            // Given
            TypeNameRegistration registration = new TypeNameRegistration();

            // When
            char id = Char.MaxValue;
            string type = registration.GetRegisteredName(id);

            //Then
            Assert.AreEqual("", type);
        }

        [TestMethod]
        public void GivenThreeRegisteredNames_WhenClearingTheRegistry_ThenOnlyEmptyTypeCanBeFound()
        {
            // Given
            string typeA = "typeA";
            string typeB = "typeB";
            string typeC = "typeC";
            TypeNameRegistration registration = new TypeNameRegistration();
            registration.RegisterName(typeA);
            registration.RegisterName(typeB);
            registration.RegisterName(typeC);
            Assert.AreEqual(4, registration.GetRegisteredNames().ToList().Count);

            // When
            registration.Clear();

            // Then
            List<string> registeredNames = registration.GetRegisteredNames().ToList();
            Assert.AreEqual(1, registeredNames.Count);
            Assert.AreEqual("", registeredNames[0]);
        }
    }
}
