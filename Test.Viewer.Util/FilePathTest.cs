using Dsmviz.Viewer.Utils;

namespace Test.Viewer.Util
{
    [TestClass]
    public class FilePathTest
    {
        [TestMethod]
        public void WhenResolveFileIsCalledWithAnAbsoluteFilename_ThenTheAbsoluteFileNameIsReturned()
        {
            // When
            string input = @"C:\temp\TestFileCopy.txt";
            string output = FilePath.ResolveFile(null, input);

            //Then
            Assert.AreEqual(@"C:\temp\TestFileCopy.txt", output);
        }

        [TestMethod]
        public void WhenResolveFileIsCalledWithAnAbsolutePathAndFilename_ThenTheResolvedAbsoluteFileNameIsReturned()
        {
            // When
            string path = @"C:\temp";
            string input = "TestFileCopy.txt";
            string output = FilePath.ResolveFile(path, input);

            // Then
            Assert.AreEqual(@"C:\temp\TestFileCopy.txt", output);
        }

        [TestMethod]
        public void WhenResolveFileIsCalledWithAnAbsolutePathAndMultipleFilenames_ThenTheResolvedAbsoluteFileNamesAreReturned()
        {
            // When
            string path = @"C:\temp";
            string[] input = ["TestFileCopy.txt", "TestFileCopyAgain.txt"];
            List<string> output = FilePath.ResolveFiles(path, input);

            // Then
            Assert.AreEqual(@"C:\temp\TestFileCopy.txt", output[0]);
            Assert.AreEqual(@"C:\temp\TestFileCopyAgain.txt", output[1]);
        }

        [TestMethod]
        public void WhenResolveFileIsCalledWithAnAbsolutePathAndRelativeFilename_ThenTheResolvedAbsoluteFileNameIsReturned()
        {
            // When
            string path = @"C:\temp\TestDir";
            string input = @"..\TestFileCopy.txt";
            string output = FilePath.ResolveFile(path, input);

            // Then
            Assert.AreEqual(@"C:\temp\TestFileCopy.txt", output);
        }
    }
}
