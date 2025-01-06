using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Viewer.Data.Model.Common;

namespace Dsmviz.Viewer.Data.Model.Entities
{
    /// <summary>
    /// Relation between two elements.
    /// </summary>
    public class Relation : IRelation
    {
        private char _typeId;
        private readonly TypeNameRegistration _typeNameRegistration;

        public Relation(TypeNameRegistration typeNameRegistration, PropertyNameRegistration propertyNameRegistration, int id, IElement consumer, IElement provider, string type, int weight, IDictionary<string, string>? properties)
        {
            Id = id;
            Consumer = consumer;
            Provider = provider;
            _typeNameRegistration = typeNameRegistration;
            _typeId = _typeNameRegistration.RegisterName(type);
            Weight = weight;
            Properties = properties;

            if (properties != null)
            {
                foreach (var key in properties.Keys)
                {
                    propertyNameRegistration.RegisterName(key);
                }
            }
        }

        public int Id { get; }

        public IElement Consumer { get; }

        public IElement Provider { get; }

        public string Type
        {
            get => _typeNameRegistration.GetRegisteredName(_typeId);
            set => _typeId = _typeNameRegistration.RegisterName(value);
        }

        public int Weight { get; set; }

        public IDictionary<string, string>? Properties { get; }

        public bool IsDeleted { get; set; }
    }
}
