
using Dsmviz.Interfaces.Application.Matrix;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Viewer.Application.Editing.Data;
using Moq;

namespace Dsmviz.Test.Application.Editing.Data
{
    [TestClass]
    public class ElementWeightsMatrixTest
    {
        [TestMethod]
        public void GivenFilledDesignMatrix_WhenWeightMatrixIsConstructed_ThenWeightMatrixIsFilledWithDesignMatrixValuesExceptForDependencyWithSelf()
        {
            // Given
            Mock<IDependencyWeightMatrix> matrix = new Mock<IDependencyWeightMatrix>();
            Mock<IElement> element = new Mock<IElement>();
            Mock<IElement> child1 = new Mock<IElement>();
            Mock<IElement> child2 = new Mock<IElement>();
            Mock<IElement> child3 = new Mock<IElement>();

            List<IElement> children =
            [
                child1.Object,
                child2.Object,
                child3.Object
            ];

            element.Setup(x => x.Children).Returns(children);
            matrix.Setup(x => x.GetDerivedDependencyWeight(child1.Object, child1.Object)).Returns(1);
            matrix.Setup(x => x.GetDerivedDependencyWeight(child1.Object, child2.Object)).Returns(2);
            matrix.Setup(x => x.GetDerivedDependencyWeight(child1.Object, child3.Object)).Returns(3);

            matrix.Setup(x => x.GetDerivedDependencyWeight(child2.Object, child1.Object)).Returns(4);
            matrix.Setup(x => x.GetDerivedDependencyWeight(child2.Object, child2.Object)).Returns(5);
            matrix.Setup(x => x.GetDerivedDependencyWeight(child2.Object, child3.Object)).Returns(6);

            matrix.Setup(x => x.GetDerivedDependencyWeight(child3.Object, child1.Object)).Returns(7);
            matrix.Setup(x => x.GetDerivedDependencyWeight(child3.Object, child2.Object)).Returns(8);
            matrix.Setup(x => x.GetDerivedDependencyWeight(child3.Object, child3.Object)).Returns(9);

            // When
            ElementWeightsMatrix weightsMatrix = new ElementWeightsMatrix(matrix.Object, element.Object);

            // Then (row=provider column=consumer)
            Assert.AreEqual(0, weightsMatrix.GetWeight(0, 0));
            Assert.AreEqual(2, weightsMatrix.GetWeight(1, 0));
            Assert.AreEqual(3, weightsMatrix.GetWeight(2, 0));

            Assert.AreEqual(4, weightsMatrix.GetWeight(0, 1));
            Assert.AreEqual(0, weightsMatrix.GetWeight(1, 1));
            Assert.AreEqual(6, weightsMatrix.GetWeight(2, 1));

            Assert.AreEqual(7, weightsMatrix.GetWeight(0, 2));
            Assert.AreEqual(8, weightsMatrix.GetWeight(1, 2));
            Assert.AreEqual(0, weightsMatrix.GetWeight(2, 2));
        }

        [TestMethod]
        public void GivenWeightsMatrixConstructedWithSizeThree_WhenSizeRetrieved_ThenThreeIsReturned()
        {
            // Given
            ElementWeightsMatrix weightMatrix = new ElementWeightsMatrix(3);

            // When
            int size = weightMatrix.Size;

            // Then
            Assert.AreEqual(3, size);
        }

        [TestMethod]
        public void GivenWeightsMatrixConstructedWithSizeThree_WhenAllNineValuesAreRetrieved_ThenAllReturnZero()
        {
            // Given
            ElementWeightsMatrix weightMatrix = new ElementWeightsMatrix(3);

            // When/Then
            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    Assert.AreEqual(0, weightMatrix.GetWeight(row, column));
                }
            }
        }

        [TestMethod]
        public void GivenWeightsMatrixConstructedWithSizeThreeAndAllNineValuesSet_WhenAllNineValuesAreRetrieved_ThenAllReturnCorrectValue()
        {
            // Given
            ElementWeightsMatrix weightMatrix = new ElementWeightsMatrix(3);
            int weightIn = 1;
            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    weightMatrix.SetWeight(row, column, weightIn);
                    weightIn++;
                }
            }

            // When/Then
            int weightOut = 1;
            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    Assert.AreEqual(weightOut, weightMatrix.GetWeight(row, column));
                    weightOut++;
                }
            }
        }

        [TestMethod]
        public void GivenWeightsMatrixConstructedWithSizeThreeAndAllNineValuesSet_WhenMatrixIsCloned_ThenCloneWillHaveSameValues()
        {
            // Given
            ElementWeightsMatrix weightMatrix1 = new ElementWeightsMatrix(3);
            int weightIn = 1;
            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    weightMatrix1.SetWeight(row, column, weightIn);
                    weightIn++;
                }
            }

            // When
            ElementWeightsMatrix weightMatrix2 = (ElementWeightsMatrix)weightMatrix1.Clone();
            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    weightMatrix1.SetWeight(row, column, 0);
                    weightIn++;
                }
            }

            // When/Then
            int weightOut = 1;
            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    Assert.AreEqual(weightOut, weightMatrix2.GetWeight(row, column));
                    weightOut++;
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Row index out of range")]
        public void GivenWeightsMatrixConstructedWithSizeThree_WhenGetValueIsCalledForFourthRow_ThenExceptionIsThrow()
        {
            ElementWeightsMatrix weightMatrix = new ElementWeightsMatrix(3);
            weightMatrix.GetWeight(3, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Column index out of range")]
        public void GivenWeightsMatrixConstructedWithSizeThree_WhenGetValueIsCalledForFourthColumn_ThenExceptionIsThrow()
        {
            ElementWeightsMatrix weightMatrix = new ElementWeightsMatrix(3);
            weightMatrix.GetWeight(0, 3);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Row index out of range")]
        public void GivenWeightsMatrixConstructedWithSizeThree_WhenSetValueIsCalledForFourthRow_ThenExceptionIsThrow()
        {
            ElementWeightsMatrix weightMatrix = new ElementWeightsMatrix(3);
            weightMatrix.SetWeight(3, 0, 123);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Column index out of range")]
        public void GivenWeightsMatrixConstructedWithSizeThree_WhenSetValueIsCalledForFourthColumn_ThenExceptionIsThrow()
        {
            ElementWeightsMatrix weightMatrix = new ElementWeightsMatrix(3);
            weightMatrix.SetWeight(0, 3, 123);
        }
    }
}
