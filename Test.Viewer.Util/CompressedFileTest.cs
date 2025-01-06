using Dsmviz.Interfaces.Util;
using Dsmviz.Viewer.Utils;
using Moq;

namespace Test.Viewer.Util
{
    [TestClass]
    public class CompressedFileTest
    {
        private readonly List<string> _lines = [];

        private readonly Mock<IFileProgress> _fileProgress = new();

        [TestInitialize]
        public void Initialize()
        {
            _lines.Clear();
        }

        [TestMethod]
        public void GiveFileDoesNotExist_WhenFileExistsIsCalled_ThenFalseIsReturned()
        {
            // Given
            CompressedFile file = new CompressedFile(NotExistingFilePath);

            // When
            bool exists = file.FileExists;

            // Then
            Assert.IsFalse(exists);
        }

        [TestMethod]
        public void GiveFileDoesNotExist_WhenIsCompressedIsCalled_ThenFalseIsReturned()
        {
            // When
            CompressedFile file = new CompressedFile(NotExistingFilePath);

            // When
            bool isCompressed = file.IsCompressed;

            // Then
            Assert.IsFalse(isCompressed);
        }

        [TestMethod]
        public void GiveFileIsUncompressed_WhenIsCompressedIsCalled_ThenFalseIsReturned()
        {
            // When
            CompressedFile file = new CompressedFile(UncompressedFilePath);
            Assert.IsTrue(file.FileExists);

            // When
            bool isCompressed = file.IsCompressed;

            // Then
            Assert.IsFalse(isCompressed);
        }

        [TestMethod]
        public void GiveFileIsCompressed_WhenIsCompressedIsCalled_ThenTrueIsReturned()
        {
            // When
            CompressedFile file = new CompressedFile(CompressedFilePath);
            Assert.IsTrue(file.FileExists);

            // When
            bool isCompressed = file.IsCompressed;

            // Then
            Assert.IsTrue(isCompressed);
        }

        [TestMethod]
        public void GiveFileIsUncompressed_WhenReadFileIsCalled_ThenTheContentIsReadCorrectly()
        {
            // Given
            CompressedFile file = new CompressedFile(UncompressedFilePath);
            Assert.IsTrue(file.FileExists);

            // When
            file.ReadFile(ReadContent, _fileProgress.Object);

            // Then
            Assert.AreEqual(4, _lines.Count);
            Assert.AreEqual("line0", _lines[0]);
            Assert.AreEqual("line1", _lines[1]);
            Assert.AreEqual("line2", _lines[2]);
            Assert.AreEqual("line3", _lines[3]);
        }

        [TestMethod]
        public void GiveFileIsCompressed_WhenReadFileIsCalled_ThenTheContentIsReadCorrectly()
        {
            // Given
            CompressedFile file = new CompressedFile(CompressedFilePath);
            Assert.IsTrue(file.FileExists);

            //When
            file.ReadFile(ReadContent, _fileProgress.Object);

            // Then
            Assert.AreEqual(4, _lines.Count);
            Assert.AreEqual("line0", _lines[0]);
            Assert.AreEqual("line1", _lines[1]);
            Assert.AreEqual("line2", _lines[2]);
            Assert.AreEqual("line3", _lines[3]);
        }

        [TestMethod]
        public void WhenContentIsWrittenUncompressedToAFile_ThenTheReadBackContentIsIdentical()
        {
            // When
            string newPath = NewFilePath;
            CompressedFile writtenFile = new CompressedFile(newPath);
            Assert.IsFalse(writtenFile.FileExists);
            writtenFile.WriteFile(WriteContent, _fileProgress.Object, false);
            Assert.IsTrue(writtenFile.FileExists);

            // Then
            CompressedFile readFile = new CompressedFile(newPath);
            Assert.IsTrue(readFile.FileExists);
            readFile.ReadFile(ReadContent, _fileProgress.Object);
            Assert.AreEqual(4, _lines.Count);
            Assert.AreEqual("line0", _lines[0]);
            Assert.AreEqual("line1", _lines[1]);
            Assert.AreEqual("line2", _lines[2]);
            Assert.AreEqual("line3", _lines[3]);
        }

        [TestMethod]
        public void WhenContentIsWrittenCompressedToAFile_ThenTheReadBackContentIsIdentical()
        {
            // When
            string newPath = NewFilePath;
            CompressedFile writtenFile = new CompressedFile(newPath);
            Assert.IsFalse(writtenFile.FileExists);
            writtenFile.WriteFile(WriteContent, _fileProgress.Object, true);
            Assert.IsTrue(writtenFile.FileExists);

            // Then
            CompressedFile readFile = new CompressedFile(newPath);
            Assert.IsTrue(readFile.FileExists);
            readFile.ReadFile(ReadContent, _fileProgress.Object);
            Assert.AreEqual(4, _lines.Count);
            Assert.AreEqual("line0", _lines[0]);
            Assert.AreEqual("line1", _lines[1]);
            Assert.AreEqual("line2", _lines[2]);
            Assert.AreEqual("line3", _lines[3]);
        }

        private void ReadContent(Stream stream, IFileProgress progress)
        {
            Assert.AreEqual(progress, _fileProgress.Object);

            using StreamReader reader = new StreamReader(stream);
            string line;
            do
            {
                line = reader.ReadLine();
                if (line != null)
                {
                    _lines.Add(line);
                }
            } while (line != null);
        }

        private void WriteContent(Stream stream, IFileProgress progress)
        {
            Assert.AreEqual(progress, _fileProgress.Object);

            using StreamWriter writer = new StreamWriter(stream);
            for (int lineCount = 0; lineCount < 4; lineCount++)
            {
                writer.WriteLine($"line{lineCount}");
            }
        }

        private static string NotExistingFilePath => @"C:\Temp\TestFile.txt";

        private static string UncompressedFilePath => @"C:\Temp\TestFileCopy.txt";

        private static string CompressedFilePath => @"C:\Temp\TestFileCopy.zip";

        private static string NewFilePath => $@"C:\Temp\File{Guid.NewGuid().ToString()}.txt";
    }
}
