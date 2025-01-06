using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.Data.Model.Relations;
using Dsmviz.Viewer.Application.Matrix;
using Dsmviz.Viewer.Utils;
using Moq;

namespace Test.Viewer.Application.Matrix
{

    //    /// <summary>
    //    /// Dependency matrix used for tests:
    //    /// -System cycle between a and b
    //    /// -Hierarchical cycle between a and c
    //    /// 
    //    ///        | a           | b           | c           |
    //    ///        +------+------+------+------+------+------+
    //    ///        | a1   | a2   | b1   | b2   | c1   | c2   |
    //    /// --+----+------+------+------+------+------+------+
    //    ///   | a1 |      |      |      | 2    |      |      |
    //    /// a +----+------+------+------+------+------+------+
    //    ///   | a2 |  1   |      |      | 3    |  4   |      |
    //    /// -------+------+------+------+------+------+------+
    //    ///   | b1 | 1000 | 200  |      |      |      |      |
    //    /// b +----+------+------+------+------+------+------+
    //    ///   | b2 |  30  | 4    |      |      |      |      |
    //    /// --+----+------+------+------+------+------+------+
    //    ///   | c1 |      |      |      |      |      | 1    |
    //    /// c +----+------+------+------+------+------+------+
    //    ///   | c2 |  5   |      |      |      | 1    |      |
    //    /// --+----+------+------+------+------+------+------+
    //    /// </summary>
    //    /// 


    [TestClass]
    public class DependencyWeightDesignMatrixTest
    {
        private DependencyWeightMatrix _matrix;
        private Mock<IRelationModelQuery> _relationModelQuery;

        private Mock<IElement> _root;

        private Mock<IElement> _a;
        private Mock<IElement> _a1;
        private Mock<IElement> _a2;

        private Mock<IElement> _b;
        private Mock<IElement> _b1;
        private Mock<IElement> _b2;

        private Mock<IElement> _c;
        private Mock<IElement> _c1;
        private Mock<IElement> _c2;

        private Mock<IRelation> _a1_to_a2_w0001;
        private Mock<IRelation> _a1_to_b1_w1000;
        private Mock<IRelation> _a1_to_b2_w0030;
        private Mock<IRelation> _a1_to_c2_w0005;

        private Mock<IRelation> _a2_to_b1_w0200;
        private Mock<IRelation> _a2_to_b2_w0004;

        private Mock<IRelation> _b2_to_a1_w0002;
        private Mock<IRelation> _b2_to_a2_w0003;

        private Mock<IRelation> _c1_to_a2_w0004;
        private Mock<IRelation> _c1_to_c2_w0001;
        private Mock<IRelation> _c2_to_c1_w0001;

        private Mock<IRelation> _a1_to_a1_w0001; // To self

        private SparseMatrix<int> _expectedDirectWeights;
        private SparseMatrix<int> _expectedDerivedWeights;

        private readonly List<Mock<IElement>> _elements = [];

        [TestInitialize]
        public void TestInitialize()
        {
            _elements.Clear();

            _relationModelQuery = new Mock<IRelationModelQuery>();

            _expectedDirectWeights = new SparseMatrix<int>(0);
            _expectedDerivedWeights = new SparseMatrix<int>(0);

            CreateElementHierarchy();
            CreateElementRelations();
        }

        [TestMethod]
        public void GivenEmptyMatrix_WhenNoRelationsAreAdded_ThenAllDirectWeightsAreAllZero()
        {
            // Given
            _matrix = new DependencyWeightMatrix(_relationModelQuery.Object);

            // When

            // Then
            CheckDirectDependencyWeights();
        }

        [TestMethod]
        public void GivenEmptyMatrix_WhenNoRelationsAreAdded_ThenAllDerivedWeightsAreAllZero()
        {
            // Given
            _matrix = new DependencyWeightMatrix(_relationModelQuery.Object);

            // When

            // Then
            CheckDerivedDependencyWeights();
        }

        [TestMethod]
        public void GivenEmptyMatrix_WhenRelationsAreAdded_ThenAllDirectWeightsCorrect()
        {
            // Given
            _matrix = new DependencyWeightMatrix(_relationModelQuery.Object);

            // When
            AddRelation(_a1_to_a2_w0001.Object);
            AddRelation(_a1_to_b1_w1000.Object);
            AddRelation(_a1_to_b2_w0030.Object);
            AddRelation(_a1_to_c2_w0005.Object);

            AddRelation(_a2_to_b1_w0200.Object);
            AddRelation(_a2_to_b2_w0004.Object);

            AddRelation(_b2_to_a1_w0002.Object);
            AddRelation(_b2_to_a2_w0003.Object);

            AddRelation(_c1_to_a2_w0004.Object);
            AddRelation(_c1_to_c2_w0001.Object);
            AddRelation(_c2_to_c1_w0001.Object);

            // Then
            _expectedDirectWeights.SetValue(_a1.Object.Id, _a2.Object.Id, 1);
            _expectedDirectWeights.SetValue(_a1.Object.Id, _b1.Object.Id, 1000);
            _expectedDirectWeights.SetValue(_a2.Object.Id, _b1.Object.Id, 200);
            _expectedDirectWeights.SetValue(_a1.Object.Id, _b2.Object.Id, 30);
            _expectedDirectWeights.SetValue(_a2.Object.Id, _b2.Object.Id, 4);
            _expectedDirectWeights.SetValue(_a1.Object.Id, _c2.Object.Id, 5);

            _expectedDirectWeights.SetValue(_b2.Object.Id, _a1.Object.Id, 2);
            _expectedDirectWeights.SetValue(_b2.Object.Id, _a2.Object.Id, 3);

            _expectedDirectWeights.SetValue(_c1.Object.Id, _a2.Object.Id, 4);
            _expectedDirectWeights.SetValue(_c1.Object.Id, _c2.Object.Id, 1);
            _expectedDirectWeights.SetValue(_c2.Object.Id, _c1.Object.Id, 1);

            CheckDirectDependencyWeights();
        }

        [TestMethod]
        public void GivenEmptyMatrix_WhenRelationsAreAdded_ThenAllDerivedWeightsCorrect()
        {
            // Given
            _matrix = new DependencyWeightMatrix(_relationModelQuery.Object);

            // When
            AddRelation(_a1_to_a2_w0001.Object);
            AddRelation(_a1_to_b1_w1000.Object);
            AddRelation(_a1_to_b2_w0030.Object);
            AddRelation(_a1_to_c2_w0005.Object);

            AddRelation(_a2_to_b1_w0200.Object);
            AddRelation(_a2_to_b2_w0004.Object);

            AddRelation(_b2_to_a1_w0002.Object);
            AddRelation(_b2_to_a2_w0003.Object);

            AddRelation(_c1_to_a2_w0004.Object);
            AddRelation(_c1_to_c2_w0001.Object);
            AddRelation(_c2_to_c1_w0001.Object);

            // Then
            _expectedDerivedWeights.SetValue(_a1.Object.Id, _a2.Object.Id, 1);

            _expectedDerivedWeights.SetValue(_a1.Object.Id, _b1.Object.Id, 1000);
            _expectedDerivedWeights.SetValue(_a2.Object.Id, _b1.Object.Id, 200);
            _expectedDerivedWeights.SetValue(_a1.Object.Id, _b2.Object.Id, 30);
            _expectedDerivedWeights.SetValue(_a2.Object.Id, _b2.Object.Id, 4);

            _expectedDerivedWeights.SetValue(_a1.Object.Id, _c2.Object.Id, 5);

            _expectedDerivedWeights.SetValue(_b2.Object.Id, _a1.Object.Id, 2);
            _expectedDerivedWeights.SetValue(_b2.Object.Id, _a2.Object.Id, 3);

            _expectedDerivedWeights.SetValue(_c1.Object.Id, _a2.Object.Id, 4);

            _expectedDerivedWeights.SetValue(_c1.Object.Id, _c2.Object.Id, 1);
            _expectedDerivedWeights.SetValue(_c2.Object.Id, _c1.Object.Id, 1);

            _expectedDerivedWeights.SetValue(_a1.Object.Id, _b.Object.Id, 1030);
            _expectedDerivedWeights.SetValue(_a2.Object.Id, _b.Object.Id, 204);
            _expectedDerivedWeights.SetValue(_a.Object.Id, _b1.Object.Id, 1200);
            _expectedDerivedWeights.SetValue(_a.Object.Id, _b2.Object.Id, 34);

            _expectedDerivedWeights.SetValue(_a.Object.Id, _b.Object.Id, 1234);

            _expectedDerivedWeights.SetValue(_a.Object.Id, _c2.Object.Id, 5);
            _expectedDerivedWeights.SetValue(_a1.Object.Id, _c.Object.Id, 5);
            _expectedDerivedWeights.SetValue(_a.Object.Id, _c.Object.Id, 5);

            _expectedDerivedWeights.SetValue(_b.Object.Id, _a1.Object.Id, 2);
            _expectedDerivedWeights.SetValue(_b.Object.Id, _a2.Object.Id, 3);
            _expectedDerivedWeights.SetValue(_b2.Object.Id, _a.Object.Id, 5);
            _expectedDerivedWeights.SetValue(_b.Object.Id, _a.Object.Id, 5);

            _expectedDerivedWeights.SetValue(_c1.Object.Id, _a.Object.Id, 4);
            _expectedDerivedWeights.SetValue(_c.Object.Id, _a2.Object.Id, 4);
            _expectedDerivedWeights.SetValue(_c.Object.Id, _a.Object.Id, 4);

            CheckDerivedDependencyWeights();
        }

        [TestMethod]
        public void GivenEmptyMatrix_WhenRelationToSelfIsAdded_ThenAllDirectWeightsAreAllZero()
        {
            // Given
            _matrix = new DependencyWeightMatrix(_relationModelQuery.Object);

            // When
            AddRelation(_a1_to_a1_w0001.Object);

            // Then
            CheckDirectDependencyWeights();
        }

        [TestMethod]
        public void GivenEmptyMatrix_WhenRelationToSelfIsAdded_ThenAllDerivedWeightsAreAllZero()
        {
            // Given
            _matrix = new DependencyWeightMatrix(_relationModelQuery.Object);

            // When
            AddRelation(_a1_to_a1_w0001.Object);

            // Then
            CheckDerivedDependencyWeights();
        }

        [TestMethod]
        public void GivenNonEmptyMatrix_WhenRelationsAreRemoved_ThenDirectWeightsAreReduced()
        {
            // Given
            _matrix = new DependencyWeightMatrix(_relationModelQuery.Object);
            AddRelation(_a1_to_b1_w1000.Object);
            AddRelation(_a1_to_b2_w0030.Object);
            _expectedDirectWeights.SetValue(_a1.Object.Id, _b1.Object.Id, 1000);
            _expectedDirectWeights.SetValue(_a1.Object.Id, _b2.Object.Id, 0030);
            CheckDirectDependencyWeights();

            // When
            RemoveRelation(_a1_to_b2_w0030.Object);

            // Then
            _expectedDirectWeights.Clear();
            _expectedDirectWeights.SetValue(_a1.Object.Id, _b1.Object.Id, 1000);
            CheckDirectDependencyWeights();
        }

        [TestMethod]
        public void GivenNonEmptyMatrix_WhenRelationsAreRemoved_ThenDerivedWeightsAreReduced()
        {
            // Given
            _matrix = new DependencyWeightMatrix(_relationModelQuery.Object);
            AddRelation(_a1_to_b1_w1000.Object);
            AddRelation(_a1_to_b2_w0030.Object);
            _expectedDerivedWeights.SetValue(_a1.Object.Id, _b1.Object.Id, 1000);
            _expectedDerivedWeights.SetValue(_a1.Object.Id, _b2.Object.Id, 0030);
            _expectedDerivedWeights.SetValue(_a.Object.Id, _b1.Object.Id, 1000);
            _expectedDerivedWeights.SetValue(_a.Object.Id, _b2.Object.Id, 0030);
            _expectedDerivedWeights.SetValue(_a1.Object.Id, _b.Object.Id, 1030);
            _expectedDerivedWeights.SetValue(_a1.Object.Id, _b.Object.Id, 1030);
            _expectedDerivedWeights.SetValue(_a.Object.Id, _b.Object.Id, 1030);
            CheckDerivedDependencyWeights();

            // When
            RemoveRelation(_a1_to_b2_w0030.Object);

            // Then
            _expectedDerivedWeights.Clear();
            _expectedDerivedWeights.SetValue(_a1.Object.Id, _b1.Object.Id, 1000);
            _expectedDerivedWeights.SetValue(_a.Object.Id, _b1.Object.Id, 1000);
            _expectedDerivedWeights.SetValue(_a1.Object.Id, _b.Object.Id, 1000);
            _expectedDerivedWeights.SetValue(_a1.Object.Id, _b.Object.Id, 1000);
            _expectedDerivedWeights.SetValue(_a.Object.Id, _b.Object.Id, 1000);
            CheckDerivedDependencyWeights();
        }

        private void CreateElementHierarchy()
        {
            _root = CreateRootElement(10);

            _a = CreateChildElement(11, "a", _root);
            _a1 = CreateChildElement(12, "a1", _a);
            _a2 = CreateChildElement(13, "a2", _a);

            _b = CreateChildElement(14, "b", _root);
            _b1 = CreateChildElement(15, "b1", _b);
            _b2 = CreateChildElement(16, "b2", _b);

            _c = CreateChildElement(17, "c", _root);
            _c1 = CreateChildElement(18, "c1", _c);
            _c2 = CreateChildElement(19, "c2", _c);
        }

        private Mock<IElement> CreateRootElement(int id)
        {
            IElement? parent = null;
            Mock<IElement> mock = new Mock<IElement>();
            mock.Setup(x => x.Id).Returns(id);
            mock.Setup(x => x.Parent).Returns(parent);
            mock.Setup(x => x.IsRoot).Returns(true);
            return mock;
        }

        private Mock<IElement> CreateChildElement(int id, string fullname, Mock<IElement> parent)
        {
            Mock<IElement> mock = new Mock<IElement>();
            mock.Setup(x => x.Id).Returns(id);
            mock.Setup(x => x.Parent).Returns(parent.Object);
            mock.Setup(x => x.Fullname).Returns(fullname);
            mock.Setup(x => x.IsRoot).Returns(false);

            _elements.Add(mock);
            return mock;
        }


        private void CreateElementRelations()
        {
            _a1_to_a2_w0001 = CreateRelation(1, _a1, _a2, 1);
            _a1_to_b1_w1000 = CreateRelation(2, _a1, _b1, 1000);
            _a1_to_b2_w0030 = CreateRelation(3, _a1, _b2, 30);
            _a1_to_c2_w0005 = CreateRelation(4, _a1, _c2, 5);

            _a2_to_b1_w0200 = CreateRelation(5, _a2, _b1, 200);
            _a2_to_b2_w0004 = CreateRelation(6, _a2, _b2, 4);

            _b2_to_a1_w0002 = CreateRelation(7, _b2, _a1, 2);
            _b2_to_a2_w0003 = CreateRelation(8, _b2, _a2, 3);

            _c1_to_a2_w0004 = CreateRelation(9, _c1, _a2, 4);
            _c1_to_c2_w0001 = CreateRelation(10, _c1, _c2, 1);
            _c2_to_c1_w0001 = CreateRelation(11, _c2, _c1, 1);

            _a1_to_a1_w0001 = CreateRelation(12, _a1, _a1, 1);
        }

        private Mock<IRelation> CreateRelation(int id, Mock<IElement> consumer, Mock<IElement> provider, int weight)
        {
            Mock<IRelation> mock = new Mock<IRelation>();
            mock.Setup(x => x.Id).Returns(id);
            mock.Setup(x => x.Consumer).Returns(consumer.Object);
            mock.Setup(x => x.Provider).Returns(provider.Object);
            mock.Setup(x => x.Weight).Returns(weight);
            return mock;
        }

        private void CheckDirectDependencyWeights()
        {
            foreach (Mock<IElement> consumer in _elements)
            {
                foreach (Mock<IElement> provider in _elements)
                {
                    int expected = _expectedDirectWeights.GetValue(consumer.Object.Id, provider.Object.Id);
                    int actual = _matrix.GetDirectDependencyWeight(consumer.Object, provider.Object);
                    Assert.AreEqual(expected, actual, $"Direct weight not equal consumer={consumer.Object.Fullname} provider={provider.Object.Fullname} expected={expected}");
                }
            }
        }

        private void CheckDerivedDependencyWeights()
        {
            foreach (Mock<IElement> consumer in _elements)
            {
                foreach (Mock<IElement> provider in _elements)
                {
                    int expected = _expectedDerivedWeights.GetValue(consumer.Object.Id, provider.Object.Id);
                    int actual = _matrix.GetDerivedDependencyWeight(consumer.Object, provider.Object);
                    Assert.AreEqual(expected, actual, $"Derived weight not equal consumer={consumer.Object.Fullname} provider={provider.Object.Fullname} expected={expected}");
                }
            }
        }

        private void AddRelation(IRelation relation)
        {
            _relationModelQuery.Raise(i => i.RelationAdded += null, this, relation);
        }

        private void RemoveRelation(IRelation relation)
        {
            _relationModelQuery.Raise(i => i.RelationRemoved += null, this, relation);
        }
    }
}
