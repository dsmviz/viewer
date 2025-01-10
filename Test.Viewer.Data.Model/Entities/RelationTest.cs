

using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Viewer.Data.Model.Common;
using Dsmviz.Viewer.Data.Model.Entities;

namespace Dsmviz.Test.Data.Model.Entities
{
    [TestClass]
    public class RelationTest
    {
        private readonly TypeNameRegistration _elementTypeNameRegistration = new();
        private readonly PropertyNameRegistration _elementPropertyNameRegistration = new();
        private readonly TypeNameRegistration _relationTypeNameRegistration = new();
        private readonly PropertyNameRegistration _relationPropertyNameRegistration = new();

        [TestMethod]
        public void
            GivenTwoElements_WhenRelationWithoutPropertiesIsConstructedBetweenThem_ThenRelationAccordingInputArguments()
        {
            // Given
            int consumerId = 2;
            int providerId = 3;
            IElement consumer = CreateElement(consumerId, "element1", "type1", null);
            IElement provider = CreateElement(providerId, "element2", "type2", null);

            // When
            int relationId = 1;
            string relationType = "include";
            int weight = 4;
            Relation relation = CreateRelation(relationId, consumer, provider, relationType, weight, null);

            //Then
            Assert.AreEqual(relationId, relation.Id);
            Assert.AreEqual(consumerId, relation.Consumer.Id);
            Assert.AreEqual(providerId, relation.Provider.Id);
            Assert.AreEqual(relationType, relation.Type);
            Assert.AreEqual(weight, relation.Weight);
            Assert.IsNull(relation.Properties);
        }

        [TestMethod]
        public void
            GivenTwoElements_WhenRelationWithPropertiesIsConstructedBetweenThem_ThenRelationAccordingInputArguments()
        {
            // Given
            int consumerId = 2;
            int providerId = 3;
            IElement consumer = CreateElement(consumerId, "element1", "type1", null);
            IElement provider = CreateElement(providerId, "element2", "type2", null);

            // When
            int relationId = 1;
            string relationType = "include";
            int weight = 4;
            Dictionary<string, string> relationProperties = new Dictionary<string, string>
            {
                ["annotation"] = "some text",
                ["version"] = "1.0"
            };
            Relation relation = CreateRelation(relationId, consumer, provider, relationType, weight, relationProperties);

            // Then
            Assert.AreEqual(relationId, relation.Id);
            Assert.AreEqual(consumerId, relation.Consumer.Id);
            Assert.AreEqual(providerId, relation.Provider.Id);
            Assert.AreEqual(relationType, relation.Type);
            Assert.AreEqual(weight, relation.Weight);
            Assert.IsNotNull(relation.Properties);
            Assert.AreEqual(2, relation.Properties.Count);
            Assert.AreEqual("some text", relation.Properties["annotation"]);
            Assert.AreEqual("1.0", relation.Properties["version"]);
        }

        private Element CreateElement(int id, string name, string type, IDictionary<string, string> properties,
            int order = 0, bool isExpanded = false)
        {
            return new Element(_elementTypeNameRegistration, _elementPropertyNameRegistration, id, name, type,
                properties, order, isExpanded);
        }

        private Relation CreateRelation(int id, IElement consumer, IElement provider, string type, int weight,
            IDictionary<string, string> properties)
        {
            return new Relation(_relationTypeNameRegistration, _relationPropertyNameRegistration, id, consumer,
                provider, type, weight, properties);
        }
    }
}
