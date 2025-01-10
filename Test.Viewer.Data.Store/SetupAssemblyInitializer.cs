
using Dsmviz.Viewer.Utils;
using System.Reflection;

namespace Dsmviz.Test.Data.Store
{
    [TestClass]
    public class SetupAssemblyInitializer
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            Logger.Init(Assembly.GetExecutingAssembly(), true);
        }
    }
}
