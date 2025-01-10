using Dsmviz.Viewer.Utils;
using System.Reflection;

namespace Dsmviz.Test.Util
{
    [TestClass]
    public class SystemInfoTest
    {
        [TestMethod]
        public void WhenGetExecutableInfoIsCalled_ThenInfoContainingTheAssemblyNameIsReturned()
        {
            // When
            string executableInfo = SystemInfo.GetExecutableInfo(Assembly.GetExecutingAssembly());

            // Then
            char[] itemSeparators = ['=', ' '];
            string[] elements = executableInfo.Split(itemSeparators);
            Assert.IsTrue(elements.Length >= 6); // Depending on locale PM/AM might be post fixed

            Assert.AreEqual("Dsmviz.Test.Util", elements[0]);
        }

        [TestMethod]
        public void WhenGetExecutableInfoIsCalled_ThenInfoIdentifyingTheAssemblyVersionIsReturned()
        {

            // When
            string executableInfo = SystemInfo.GetExecutableInfo(Assembly.GetExecutingAssembly());

            // Then
            char[] itemSeparators = ['=', ' '];
            string[] elements = executableInfo.Split(itemSeparators);
            Assert.IsTrue(elements.Length >= 6); // Depending on locale PM/AM might be post fixed

            Assert.AreEqual("version", elements[1]);

            char[] versionNumberSeparators = ['.'];
            string[] versionNumberItems = elements[2].Split(versionNumberSeparators);
            Assert.AreEqual(4, versionNumberItems.Length);

            Assert.IsTrue(int.TryParse(versionNumberItems[0], out var versionNumberPart0));
            Assert.AreEqual(1, versionNumberPart0);

            Assert.IsTrue(int.TryParse(versionNumberItems[1], out var versionNumberPart1));
            Assert.AreEqual(0, versionNumberPart1);

            Assert.IsTrue(int.TryParse(versionNumberItems[2], out var versionNumberPart2));

            Assert.IsTrue(int.TryParse(versionNumberItems[3], out var versionNumberPart3));
        }

        [TestMethod]
        public void WhenGetExecutableInfoIsCalled_ThenInfoIdentifyingTheAssemblyBuildTimeStampIsReturned()
        {
            // When
            string executableInfo = SystemInfo.GetExecutableInfo(Assembly.GetExecutingAssembly());

            // Then
            char[] itemSeparators = ['=', ' '];
            string[] elements = executableInfo.Split(itemSeparators);
            Assert.IsTrue(elements.Length >= 6); // Depending on locale PM/AM might be post fixed

            Assert.AreEqual("build", elements[3]);
            Assert.IsTrue(DateTime.TryParse(elements[4], out var date));
            Assert.IsTrue(DateTime.TryParse(elements[5], out var time));
        }
    }
}
