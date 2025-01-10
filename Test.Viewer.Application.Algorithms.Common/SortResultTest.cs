

using Dsmviz.Viewer.Algorithms.Common;

namespace Dsmviz.Test.Application.Algorithms.Common
{
    [TestClass]
    public class SortResultTest
    {
        [TestMethod]
        public void WhenSortResultConstructedWithZeroSizeThenItIsInvalid()
        {
            SortResult result = new SortResult(0);
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void WhenSortResultConstructedWithNonZeroSizeThenItIsValid()
        {
            SortResult result = new SortResult(4);
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(0, result.GetIndex(0));
            Assert.AreEqual(1, result.GetIndex(1));
            Assert.AreEqual(2, result.GetIndex(2));
            Assert.AreEqual(3, result.GetIndex(3));

            Assert.AreEqual(4, result.SortedIndexValues.Count);
            Assert.AreEqual(0, result.SortedIndexValues[0]);
            Assert.AreEqual(1, result.SortedIndexValues[1]);
            Assert.AreEqual(2, result.SortedIndexValues[2]);
            Assert.AreEqual(3, result.SortedIndexValues[3]);
        }

        [TestMethod]
        public void WhenSwapWithValidArgumentThenTheOrderIsChanged()
        {
            SortResult result = new SortResult(3);
            result.SetIndex(0, 2);
            result.SetIndex(1, 1);
            result.SetIndex(2, 0);
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(2, result.GetIndex(0));
            Assert.AreEqual(1, result.GetIndex(1));
            Assert.AreEqual(0, result.GetIndex(2));

            result.Swap(0, 1);

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(1, result.GetIndex(0));
            Assert.AreEqual(2, result.GetIndex(1));
            Assert.AreEqual(0, result.GetIndex(2));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WhenSwapWithOutOfBoundArgumentThenExceptionIsThrown()
        {
            SortResult result = new SortResult(3);
            result.SetIndex(0, 2);
            result.SetIndex(1, 1);
            result.SetIndex(2, 0);
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(2, result.GetIndex(0));
            Assert.AreEqual(1, result.GetIndex(1));
            Assert.AreEqual(0, result.GetIndex(2));

            result.Swap(0, 3);
        }

        [TestMethod]
        public void WhenInvertOrderThenTheOrderIsChanged()
        {
            SortResult result = new SortResult(3);
            result.SetIndex(0, 2);
            result.SetIndex(1, 0);
            result.SetIndex(2, 1);
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(2, result.GetIndex(0));
            Assert.AreEqual(0, result.GetIndex(1));
            Assert.AreEqual(1, result.GetIndex(2));

            result.InvertOrder();

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(1, result.GetIndex(0));
            Assert.AreEqual(2, result.GetIndex(1));
            Assert.AreEqual(0, result.GetIndex(2));
        }

        [TestMethod]
        public void WhenInvertOrderIsCalledTwiceThenTheOrderIsUnchanged()
        {
            SortResult result = new SortResult(3);
            result.SetIndex(0, 2);
            result.SetIndex(1, 0);
            result.SetIndex(2, 1);
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(2, result.GetIndex(0));
            Assert.AreEqual(0, result.GetIndex(1));
            Assert.AreEqual(1, result.GetIndex(2));

            result.InvertOrder();
            result.InvertOrder();

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(2, result.GetIndex(0));
            Assert.AreEqual(0, result.GetIndex(1));
            Assert.AreEqual(1, result.GetIndex(2));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WhenGetIndexWithOutOfBoundArgumentThenExceptionIsThrown()
        {
            SortResult result = new SortResult(3);
            result.SetIndex(0, 2);
            result.SetIndex(1, 1);
            result.SetIndex(2, 0);
            Assert.IsTrue(result.IsValid);
            result.GetIndex(3);
        }
    }
}
