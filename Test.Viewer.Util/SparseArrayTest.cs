using Dsmviz.Viewer.Utils;

namespace Dsmviz.Test.Util
{
    [TestClass]
    public class SparseArrayTest
    {
        private int _a;
        private int _b;

        private int _x;

        [TestInitialize]
        public void TestInitialize()
        {
            _a = 1;
            _b = 2;

            _x = 3;
        }


        [TestMethod]
        public void WhenEmptySparseArray_ThenRetrievedValueIsDefault()
        {
            // Given
            SparseArray<int> sparseArray = new SparseArray<int>(123);

            // When
            Assert.AreEqual(0, sparseArray.FilledCount);
            Assert.AreEqual(123, sparseArray.GetValue(_a));
        }

        [TestMethod]
        public void GivenEmptySparseArray_WhenTheValueIsSet_ThenSetValueIsReturnedWhenRetrieved()
        {
            // Given
            SparseArray<int> sparseArray = new SparseArray<int>(123);

            // When
            bool changed1 = sparseArray.SetValue(_a, 456);
            Assert.IsTrue(changed1);
            bool changed2 = sparseArray.SetValue(_a, 456);
            Assert.IsFalse(changed2);

            // Then
            Assert.AreEqual(1, sparseArray.FilledCount);
            Assert.AreEqual(456, sparseArray.GetValue(_a));
        }

        [TestMethod]
        public void GivenEmptySparseArray_WhenTwoValuesAreDefinedWithUniqueProviders_ThenBothValuesCanBeFound()
        {
            // Given
            SparseArray<int> sparseArray = new SparseArray<int>(123);

            // When
            bool changed1 = sparseArray.SetValue(_a, 12);
            Assert.IsTrue(changed1);
            bool changed2 = sparseArray.SetValue(_b, 34);
            Assert.IsTrue(changed2);

            // Then
            Assert.AreEqual(2, sparseArray.FilledCount);
            Assert.AreEqual(12, sparseArray.GetValue(_a));
            Assert.AreEqual(34, sparseArray.GetValue(_b));
        }

        [TestMethod]
        public void GivenValueIsSet_WhenTheValueIsCleared_ThenRetrievedValueIsDefault()
        {
            // Given
            SparseArray<int> sparseArray = new SparseArray<int>(123);
            sparseArray.SetValue(_a, 456);
            Assert.AreEqual(1, sparseArray.FilledCount);
            Assert.AreEqual(456, sparseArray.GetValue(_a));

            // When
            bool changed1 = sparseArray.ClearValue(_a);
            Assert.IsTrue(changed1);
            bool changed2 = sparseArray.ClearValue(_a);
            Assert.IsFalse(changed2);

            // Then
            Assert.AreEqual(0, sparseArray.FilledCount);
            Assert.AreEqual(123, sparseArray.GetValue(_a));
        }

        [TestMethod]
        public void GivenValueIsSet_WhenTheValueSetToDefault_ThenRetrievedValueIsDefault()
        {
            // Given
            SparseArray<int> sparseArray = new SparseArray<int>(123);
            sparseArray.SetValue(_a, 456);
            Assert.AreEqual(1, sparseArray.FilledCount);
            Assert.AreEqual(456, sparseArray.GetValue(_a));

            // When
            bool changed = sparseArray.SetValue(_a, 123);
            Assert.IsTrue(changed);

            // Then
            Assert.AreEqual(0, sparseArray.FilledCount);
            Assert.AreEqual(123, sparseArray.GetValue(_a));
        }

        [TestMethod]
        public void GivenNonEmptySparseArray_WhenUnknownProviderIsSpecified_ThenRetrievedValueIsDefault()
        {
            // Given
            SparseArray<int> sparseArray = new SparseArray<int>(123);
            sparseArray.SetValue(_a, 456);
            Assert.AreEqual(1, sparseArray.FilledCount);

            // When
            int value = sparseArray.GetValue(_x);

            // Then
            Assert.AreEqual(123, value);
        }
    }
}
