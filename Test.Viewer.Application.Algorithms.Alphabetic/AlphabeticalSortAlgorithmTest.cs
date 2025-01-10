using Dsmviz.Interfaces.Application.Algorithms;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Viewer.Application.Algorithms.Alphabetic;
using Moq;

namespace Dsmviz.Test.Application.Algorithms.Alphabetic
{
    [TestClass]
    public class AlphabeticalSortAlgorithmTest
    {
        private const string NameA = "a";
        private const string NameB = "b";
        private const string NameC = "c";
        private const string NameD = "d";

        [TestMethod]
        public void GivenNonSortedElementLIstWhenSortedAlphabeticallyAscendingThenElementsAreInAlphabeticalOrder()
        {
            Mock<IElement> a = new Mock<IElement>();
            a.Setup(x => x.Id).Returns(34);
            a.Setup(x => x.Name).Returns(NameA);
            Mock<IElement> b = new Mock<IElement>();
            b.Setup(x => x.Id).Returns(45);
            b.Setup(x => x.Name).Returns(NameB);
            Mock<IElement> c = new Mock<IElement>();
            c.Setup(x => x.Id).Returns(12);
            c.Setup(x => x.Name).Returns(NameC);
            Mock<IElement> d = new Mock<IElement>();
            d.Setup(x => x.Id).Returns(23);
            d.Setup(x => x.Name).Returns(NameD);
            List<IElement> children = [c.Object, d.Object, a.Object, b.Object];

            Mock<IElement> parent = new Mock<IElement>();
            parent.Setup(x => x.Children).Returns(children);

            Mock<IElementWeightMatrix> weightsMatrix = new Mock<IElementWeightMatrix>();

            AlphabeticalAscendingSortAlgorithm algorithm = new AlphabeticalAscendingSortAlgorithm();
            ISortResult result = algorithm.Sort(parent.Object, weightsMatrix.Object);
            Assert.AreEqual("2,3,0,1", result.ToString());
            Assert.AreEqual(4, result.SortedIndexValues.Count);
            Assert.AreEqual(a.Object, children[result.SortedIndexValues[0]]);
            Assert.AreEqual(b.Object, children[result.SortedIndexValues[1]]);
            Assert.AreEqual(c.Object, children[result.SortedIndexValues[2]]);
            Assert.AreEqual(d.Object, children[result.SortedIndexValues[3]]);
        }

        [TestMethod]
        public void GivenNonSortedElementLIstWhenSortedAlphabeticallyDescendingThenElementsAreInReversedAlphabeticalOrder()
        {
            Mock<IElement> a = new Mock<IElement>();
            a.Setup(x => x.Id).Returns(34);
            a.Setup(x => x.Name).Returns(NameA);
            Mock<IElement> b = new Mock<IElement>();
            b.Setup(x => x.Id).Returns(45);
            b.Setup(x => x.Name).Returns(NameB);
            Mock<IElement> c = new Mock<IElement>();
            c.Setup(x => x.Id).Returns(12);
            c.Setup(x => x.Name).Returns(NameC);
            Mock<IElement> d = new Mock<IElement>();
            d.Setup(x => x.Id).Returns(23);
            d.Setup(x => x.Name).Returns(NameD);
            List<IElement> children = [c.Object, d.Object, a.Object, b.Object];

            Mock<IElement> parent = new Mock<IElement>();
            parent.Setup(x => x.Children).Returns(children);

            Mock<IElementWeightMatrix> weightsMatrix = new Mock<IElementWeightMatrix>();

            AlphabeticalDescendingSortAlgorithm algorithm = new AlphabeticalDescendingSortAlgorithm();
            ISortResult result = algorithm.Sort(parent.Object, weightsMatrix.Object);
            Assert.AreEqual("1,0,3,2", result.ToString());
            Assert.AreEqual(4, result.SortedIndexValues.Count);
            Assert.AreEqual(d.Object, children[result.SortedIndexValues[0]]);
            Assert.AreEqual(c.Object, children[result.SortedIndexValues[1]]);
            Assert.AreEqual(b.Object, children[result.SortedIndexValues[2]]);
            Assert.AreEqual(a.Object, children[result.SortedIndexValues[3]]);
        }
    }
}
