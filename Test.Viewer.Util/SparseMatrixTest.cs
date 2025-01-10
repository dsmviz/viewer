using Dsmviz.Viewer.Utils;

namespace Dsmviz.Test.Util
{
    [TestClass]
    public class SparseMatrixTest
    {
        private int _a;
        private int _b;
        private int _c;
        private int _d;

        private int _x;

        [TestInitialize]
        public void TestInitialize()
        {
            _a = 1;
            _b = 2;
            _c = 3;
            _d = 4;

            _x = 5;
        }


        [TestMethod]
        public void WhenEmptySparseMatrix_ThenRetrievedValueIsDefault()
        {
            // Given
            SparseMatrix<int> sparseMatrix = new SparseMatrix<int>(123);

            // When
            Assert.AreEqual(0, sparseMatrix.FilledCount);
            Assert.AreEqual(123, sparseMatrix.GetValue(_a, _b));
        }

        [TestMethod]
        public void GivenEmptySparseMatrix_WhenTheValueIsSet_ThenSetValueIsReturnedWhenRetrieved()
        {
            // Given
            SparseMatrix<int> sparseMatrix = new SparseMatrix<int>(123);

            // When
            bool changed1 = sparseMatrix.SetValue(_a, _b, 456);
            Assert.IsTrue(changed1);
            bool changed2 = sparseMatrix.SetValue(_a, _b, 456);
            Assert.IsFalse(changed2);

            // Then
            Assert.AreEqual(1, sparseMatrix.FilledCount);
            Assert.AreEqual(456, sparseMatrix.GetValue(_a, _b));
        }

        [TestMethod]
        public void GivenEmptySparseMatrix_WhenFourValuesAreDefinedWithUniqueConsumersAndProviders_ThenAllFourValuesCanBeFound()
        {
            // Given
            SparseMatrix<int> sparseMatrix = new SparseMatrix<int>(123);

            // When
            bool changed1 = sparseMatrix.SetValue(_a, _b, 12);
            Assert.IsTrue(changed1);
            bool changed2 = sparseMatrix.SetValue(_b, _c, 34);
            Assert.IsTrue(changed2);
            bool changed3 = sparseMatrix.SetValue(_c, _d, 56);
            Assert.IsTrue(changed3);
            bool changed4 = sparseMatrix.SetValue(_d, _a, 78);
            Assert.IsTrue(changed4);

            // Then
            Assert.AreEqual(4, sparseMatrix.FilledCount);
            Assert.AreEqual(12, sparseMatrix.GetValue(_a, _b));
            Assert.AreEqual(34, sparseMatrix.GetValue(_b, _c));
            Assert.AreEqual(56, sparseMatrix.GetValue(_c, _d));
            Assert.AreEqual(78, sparseMatrix.GetValue(_d, _a));
        }

        [TestMethod]
        public void GivenValueIsSet_WhenTheValueIsCleared_ThenRetrievedValueIsDefault()
        {
            // Given
            SparseMatrix<int> sparseMatrix = new SparseMatrix<int>(123);
            sparseMatrix.SetValue(_a, _b, 456);
            Assert.AreEqual(1, sparseMatrix.FilledCount);
            Assert.AreEqual(456, sparseMatrix.GetValue(_a, _b));

            // When
            bool changed1 = sparseMatrix.ClearValue(_a, _b);
            Assert.IsTrue(changed1);
            bool changed2 = sparseMatrix.ClearValue(_a, _b);
            Assert.IsFalse(changed2);

            // Then
            Assert.AreEqual(0, sparseMatrix.FilledCount);
            Assert.AreEqual(123, sparseMatrix.GetValue(_a, _b));
        }

        [TestMethod]
        public void GivenValueIsSet_WhenTheValueIsSetToDefault_ThenRetrievedValueIsDefault()
        {
            // Given
            SparseMatrix<int> sparseMatrix = new SparseMatrix<int>(123);
            sparseMatrix.SetValue(_a, _b, 456);
            Assert.AreEqual(1, sparseMatrix.FilledCount);
            Assert.AreEqual(456, sparseMatrix.GetValue(_a, _b));

            // When
            bool changed = sparseMatrix.SetValue(_a, _b, 123);
            Assert.IsTrue(changed);

            // Then
            Assert.AreEqual(0, sparseMatrix.FilledCount);
            Assert.AreEqual(123, sparseMatrix.GetValue(_a, _b));
        }

        [TestMethod]
        public void GivenNonEmptySparseMatrix_WhenGetWithUnknownConsumerIsCalled_ThenRetrievedValueIsDefault()
        {
            // Given
            SparseMatrix<int> sparseMatrix = new SparseMatrix<int>(123);
            sparseMatrix.SetValue(_a, _b, 456);
            Assert.AreEqual(1, sparseMatrix.FilledCount);

            // When
            int value = sparseMatrix.GetValue(_x, _b);

            // Then
            Assert.AreEqual(123, value);
        }

        [TestMethod]
        public void GivenNonEmptySparseMatrix_WhenGetWithUnknownProviderIsCalled_ThenRetrievedValueIsDefault()
        {
            // Given
            SparseMatrix<int> sparseMatrix = new SparseMatrix<int>(123);
            sparseMatrix.SetValue(_a, _b, 456);
            Assert.AreEqual(1, sparseMatrix.FilledCount);

            // When
            int value = sparseMatrix.GetValue(_a, _x);

            // Then
            Assert.AreEqual(123, value);
        }
    }
}
