using Dsmviz.Viewer.Utils;

namespace Dsmviz.Test.Util
{
    [TestClass]
    public class LongTest
    {

        [TestMethod]
        public void TestCreate()
        {
            int first = 0x1234;
            int second = 0x5678;
            long key = LongKey.Create(first, second);
            Assert.AreEqual(0x12345678, key);
        }
    }
}
