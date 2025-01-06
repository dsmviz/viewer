using Dsmviz.Interfaces.Util;
using Dsmviz.Viewer.Data.Model.Facade;
using Dsmviz.Viewer.Data.Store;
using Microsoft.VisualStudio.TestPlatform.MSTest.TestAdapter.ObjectModel;
using Moq;
using System.Diagnostics;

namespace Test.Viewer.Application.Persistency.Performance
{
    /// <summary>
    /// Summary description for PerformanceTest
    /// </summary>
    [TestClass]
    public class PerformanceTest
    {
        private readonly Mock<IFileProgress> _fileProgress = new();

        [TestMethod]
        public void TestModelLoadPerformance()
        {
            Process currentProcess = Process.GetCurrentProcess();
            const long million = 1000000;

            long peakPagedMemMbBefore = currentProcess.PeakPagedMemorySize64 / million;
            long peakVirtualMemMbBefore = currentProcess.PeakVirtualMemorySize64 / million;
            long peakWorkingSetMbBefore = currentProcess.PeakWorkingSet64 / million;
            Console.WriteLine($"Peak physical memory usage {peakWorkingSetMbBefore:0.000}MB");
            Console.WriteLine($"Peak paged memory usage    {peakPagedMemMbBefore:0.000}MB");
            Console.WriteLine($"Peak virtual memory usage  {peakVirtualMemMbBefore:0.000}MB");

            string inputFilename = "PerformanceTest.Input.dsm";

            CoreDataModel model = new CoreDataModel();
            CoreDataStore persistency = new CoreDataStore(model);

            Assert.AreEqual(1, model.ElementModelQuery.GetElementCount());
            Assert.AreEqual(0, model.RelationModelQuery.GetRelationCount());

            Stopwatch watch = new Stopwatch();
            watch.Start();

            persistency.LoadModel(inputFilename, _fileProgress.Object);

            watch.Stop();

            long peakPagedMemMbAfter = currentProcess.PeakPagedMemorySize64 / million;
            long peakVirtualMemMbAfter = currentProcess.PeakVirtualMemorySize64 / million;
            long peakWorkingSetMbAfter = currentProcess.PeakWorkingSet64 / million;
            Console.WriteLine($"Peak physical memory usage {peakWorkingSetMbAfter:0.000}MB");
            Console.WriteLine($"Peak paged memory usage    {peakPagedMemMbAfter:0.000}MB");
            Console.WriteLine($"Peak virtual memory usage  {peakVirtualMemMbAfter:0.000}MB");

            Console.WriteLine($"Load time                  {watch.ElapsedMilliseconds:0}ms");

            Assert.AreEqual(1458, model.ElementModelQuery.GetElementCount());
            Assert.AreEqual(9769, model.RelationModelQuery.GetRelationCount());
        }

        [TestMethod]
        public void TestModelSavePerformance()
        {
            Process currentProcess = Process.GetCurrentProcess();
            const long million = 1000000;

            long peakPagedMemMbBefore = currentProcess.PeakPagedMemorySize64 / million;
            long peakVirtualMemMbBefore = currentProcess.PeakVirtualMemorySize64 / million;
            long peakWorkingSetMbBefore = currentProcess.PeakWorkingSet64 / million;
            Console.WriteLine($"Peak physical memory usage {peakWorkingSetMbBefore:0.000}MB");
            Console.WriteLine($"Peak paged memory usage    {peakPagedMemMbBefore:0.000}MB");
            Console.WriteLine($"Peak virtual memory usage  {peakVirtualMemMbBefore:0.000}MB");

            string inputFilename = "PerformanceTest.Input.dsm";
            string outputFilename = "PerformanceTest.Output.dsm";

            CoreDataModel model = new CoreDataModel();
            CoreDataStore persistency = new CoreDataStore(model);

            Assert.AreEqual(1, model.ElementModelQuery.GetElementCount());
            Assert.AreEqual(0, model.RelationModelQuery.GetRelationCount());

            persistency.LoadModel(inputFilename, _fileProgress.Object);

            Stopwatch watch = new Stopwatch();
            watch.Start();

            persistency.SaveModel(outputFilename, false, _fileProgress.Object);

            watch.Stop();

            long peakPagedMemMbAfter = currentProcess.PeakPagedMemorySize64 / million;
            long peakVirtualMemMbAfter = currentProcess.PeakVirtualMemorySize64 / million;
            long peakWorkingSetMbAfter = currentProcess.PeakWorkingSet64 / million;
            Console.WriteLine($"Peak physical memory usage {peakWorkingSetMbAfter:0.000}MB");
            Console.WriteLine($"Peak paged memory usage    {peakPagedMemMbAfter:0.000}MB");
            Console.WriteLine($"Peak virtual memory usage  {peakVirtualMemMbAfter:0.000}MB");

            Console.WriteLine($"Save time                  {watch.ElapsedMilliseconds:0}ms");

            Assert.AreEqual(1458, model.ElementModelQuery.GetElementCount());
            Assert.AreEqual(9769, model.RelationModelQuery.GetRelationCount());
        }
    }
}
