using Dsmviz.Viewer.Data.Model.Common;

namespace Test.Viewer.Data.Model.Common
{
    [TestClass]
    public class PropertyNameRegistrationTest
    {
        [TestMethod]
        public void WhenNameRegistrationIsConstructed_ThenNoNamesCanBeFound()
        {
            // When
            PropertyNameRegistration registration = new PropertyNameRegistration();

            // Then
            List<string> registeredNames = registration.GetRegisteredNames().ToList();
            Assert.AreEqual(0, registeredNames.Count);
        }

        [TestMethod]
        public void GivenNoRegisteredNames_WhenThreeNamesAreRegistered_ThenAllThreeNamesCanBeFound()
        {
            // Given
            PropertyNameRegistration registration = new PropertyNameRegistration();

            // When
            string propertyA = "propertyA";
            string propertyB = "propertyB";
            string propertyC = "propertyC";
            registration.RegisterName(propertyA);
            registration.RegisterName(propertyB);
            registration.RegisterName(propertyC);

            //Then
            List<string> registeredNames = registration.GetRegisteredNames().ToList();
            Assert.AreEqual(3, registeredNames.Count);
            Assert.AreEqual(propertyA, registeredNames[0]);
            Assert.AreEqual(propertyB, registeredNames[1]);
            Assert.AreEqual(propertyC, registeredNames[2]);
        }

        [TestMethod]
        public void GivenNoRegisteredNames_WhenThreeNamesAreRegistered_ThenAllThreeCanBeFound()
        {
            // Given
            PropertyNameRegistration registration = new PropertyNameRegistration();

            // When
            string propertyA = "propertyA";
            string propertyB = "propertyB";
            string propertyC = "propertyC";
            char ida = registration.RegisterName(propertyA);
            char idb = registration.RegisterName(propertyB);
            char idc = registration.RegisterName(propertyC);

            //Then
            Assert.AreEqual(propertyA, registration.GetRegisteredName(ida));
            Assert.AreEqual(propertyB, registration.GetRegisteredName(idb));
            Assert.AreEqual(propertyC, registration.GetRegisteredName(idc));
        }

        [TestMethod]
        public void GivenNoRegisteredNames_WhenNameIsRegisteredTwice_ThenItsIsOnlyOneStoredTheFirstTime()
        {
            // Given
            PropertyNameRegistration registration = new PropertyNameRegistration();

            // When
            string propertyA = "propertyA";
            char ida1 = registration.RegisterName(propertyA);
            char ida2 = registration.RegisterName(propertyA);

            //Then
            Assert.AreEqual(ida1, ida2);
            Assert.AreEqual(propertyA, registration.GetRegisteredName(ida1));
        }

        [TestMethod]
        public void GivenNoRegisteredNames_WhenRegisteredNameIsRetrievedWithInvalidId_ThenEmptyValueIsReturned()
        {
            // Given
            PropertyNameRegistration registration = new PropertyNameRegistration();

            // When
            char id = char.MaxValue;
            string type = registration.GetRegisteredName(id);

            //Then
            Assert.AreEqual("", type);
        }

        [TestMethod]
        public void GivenThreeRegisteredNames_WhenClearingTheRegistry_ThenNoNamesCanBeFound()
        {
            // Given
            string propertyA = "propertyA";
            string propertyB = "propertyB";
            string propertyC = "propertyC";
            PropertyNameRegistration registration = new PropertyNameRegistration();
            registration.RegisterName(propertyA);
            registration.RegisterName(propertyB);
            registration.RegisterName(propertyC);
            Assert.AreEqual(3, registration.GetRegisteredNames().ToList().Count);

            // When
            registration.Clear();

            // Then
            List<string> registeredNames = registration.GetRegisteredNames().ToList();
            Assert.AreEqual(0, registeredNames.Count);
        }
    }
}
