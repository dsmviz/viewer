

using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Viewer.Data.Model.Entities;

namespace Dsmviz.Test.Data.Model.Entities
{
    [TestClass]
    public class MetaDataItemTest
    {
        [TestMethod]
        public void WhenItemIsConstructed_ThenPropertiesAreSetAccordingArguments()
        {
            IMetaDataItem item = new MetaDataItem("name", "value");
            Assert.AreEqual("name", item.Name);
            Assert.AreEqual("value", item.Value);
        }
    }
}
