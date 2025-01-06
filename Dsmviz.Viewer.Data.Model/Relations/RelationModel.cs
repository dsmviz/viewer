using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.Data.Model.Relations;
using Dsmviz.Viewer.Data.Model.Common;
using Dsmviz.Viewer.Data.Model.Entities;
using Dsmviz.Viewer.Utils;

namespace Dsmviz.Viewer.Data.Model.Relations
{
    public class RelationModel : IRelationModelPersistency, IRelationModelEditing, IRelationModelQuery, IRelationModelUpdate
    {
        private readonly Dictionary<int /*relationId*/, IRelation> _relationsById = new();
        private readonly Dictionary<int /*relationId*/, IRelation> _deletedRelationsById = new();

        private readonly ItemListDictionary<ValueTuple<int/*consumerId*/, int/*providerId*/>, IRelation> _consumerToProviderRelations = new();

        private readonly ItemListDictionary<int/*consumerId*/, IRelation> _consumerRelations = new();
        private readonly ItemListDictionary<int/*providerId*/, IRelation> _providerRelations = new();

        private readonly ItemListDictionary<int/*consumerId*/, IRelation> _deletedConsumerRelations = new();
        private readonly ItemListDictionary<int/*providerId*/, IRelation> _deletedProviderRelations = new();

        private int _lastRelationId = 0;

        private readonly TypeNameRegistration _typeNameRegistration = new();
        private readonly PropertyNameRegistration _propertyNameRegistration = new();

        public event EventHandler<IRelation>? RelationAdded;
        public event EventHandler<IRelation>? RelationRemoved;

        public void Clear()
        {
            _typeNameRegistration.Clear();
            _propertyNameRegistration.Clear();
            _relationsById.Clear();
            _deletedRelationsById.Clear();
            _consumerToProviderRelations.Clear();
            _consumerRelations.Clear();
            _providerRelations.Clear();
            _deletedConsumerRelations.Clear();
            _deletedProviderRelations.Clear();

            _lastRelationId = 0;
        }

        public IRelation? ImportRelation(int relationId, IElement consumer, IElement provider, string type, int weight, IDictionary<string, string>? properties)
        {
            Relation? relation = null;

            if (weight > 0)
            {
                Logger.LogDataModelMessage(
                    $"Import relation relationId={relationId} consumerId={consumer.Id} providerId={provider.Id} type={type} weight={weight}");

                if (relationId > _lastRelationId)
                {
                    _lastRelationId = relationId;
                }

                if (consumer.Id != provider.Id)
                {
                    relation = new Relation(_typeNameRegistration, _propertyNameRegistration, relationId, consumer, provider, type, weight, properties) { IsDeleted = false };
                    RegisterRelation(relation);
                }
            }

            return relation;
        }

        public IRelation? AddRelation(IElement consumer, IElement provider, string type, int weight, IDictionary<string, string>? properties)
        {
            Relation? relation = null;

            if (weight > 0)
            {
                Logger.LogDataModelMessage(
                    $"Add relation consumerId={consumer.Id} providerId={provider.Id} type={type} weight={weight}");

                if (consumer.Id != provider.Id)
                {
                    _lastRelationId++;
                    relation = new Relation(_typeNameRegistration, _propertyNameRegistration, _lastRelationId, consumer, provider, type, weight, properties) { IsDeleted = false };
                    RegisterRelation(relation);
                }
            }

            return relation;
        }

        public void ChangeRelationType(IRelation relation, string type)
        {
            if (relation is Relation changedRelation)
            {
                UnregisterRelation(changedRelation);

                changedRelation.Type = type;

                RegisterRelation(changedRelation);
            }
        }

        public IEnumerable<string> GetRelationTypes()
        {
            return _typeNameRegistration.GetRegisteredNames();
        }

        public void ChangeRelationWeight(IRelation relation, int weight)
        {
            if (relation is Relation changedRelation)
            {
                UnregisterRelation(changedRelation);

                changedRelation.Weight = weight;

                RegisterRelation(changedRelation);
            }
        }

        public void RemoveRelation(int relationId)
        {
            if (_relationsById.TryGetValue(relationId, out var relation))
            {
                UnregisterRelation(relation);
            }
        }

        public void RestoreRelation(int relationId)
        {
            if (_deletedRelationsById.TryGetValue(relationId, out var relation))
            {
                RegisterRelation(relation);
            }
        }

        public void RemoveAllElementRelations(IElement element)
        {
            List<IRelation> relations = [];
            relations.AddRange(_consumerRelations.GetItems(element.Id));
            relations.AddRange(_providerRelations.GetItems(element.Id));

            foreach (var relation in relations)
            {
                UnregisterRelation(relation);
            }

            foreach (var child in element.Children)
            {
                RemoveAllElementRelations(child);
            }
        }

        public void RestoreAllElementRelations(IElement element)
        {
            List<IRelation> relations = [];
            relations.AddRange(_deletedConsumerRelations.GetItems(element.Id));
            relations.AddRange(_deletedProviderRelations.GetItems(element.Id));

            foreach (var relation in relations)
            {
                RegisterRelation(relation);
            }

            foreach (var child in element.Children)
            {
                RestoreAllElementRelations(child);
            }
        }

        public IRelation? GetRelationById(int id)
        {
            return _relationsById.GetValueOrDefault(id);
        }

        public IRelation? FindRelation(IElement consumer, IElement provider, string type, int weight)
        {
            IRelation? foundRelation = null;

            foreach (var relation in FindRelations(consumer, provider))
            {
                if (relation.Type == type && relation.Weight == weight)
                {
                    foundRelation = relation;
                }
            }
            return foundRelation;

        }

        public IEnumerable<IRelation> GetRelations()
        {
            return _relationsById.Values;
        }

        public IEnumerable<IRelation> GetAllIngoingRelations(IElement element)
        {
            return FindRelationsInScope(element, RelationDirection.Ingoing, RelationScope.External);
        }

        public IEnumerable<IRelation> GetAllOutgoingRelations(IElement element)
        {
            return FindRelationsInScope(element, RelationDirection.Outgoing, RelationScope.External);
        }

        public IEnumerable<IRelation> GetAllInternalRelations(IElement element)
        {
            return FindRelationsInScope(element, RelationDirection.Ingoing, RelationScope.Internal);
        }

        public IEnumerable<IRelation> FindAllExternalRelations(IElement element)
        {
            return FindRelationsInScope(element, RelationDirection.Both, RelationScope.External);
        }

        public IEnumerable<IRelation> GetAllRelations(IElement consumer, IElement provider)
        {
            var relations = FindRelationsBetweenElementsAndItsChildren(consumer, provider)
                .OrderBy(x => x.Provider.Fullname)
                .ThenBy(x => x.Consumer.Fullname)
                .ToList();
            return relations;
        }

        public int GetRelationCount()
        {
            return _relationsById.Values.Count;
        }

        public IEnumerable<IRelation> GetRelationsIncludingDeletedOnes()
        {
            List<IRelation> relations = [];
            relations.AddRange(_relationsById.Values);
            relations.AddRange(_deletedRelationsById.Values);
            return relations.OrderBy(x => x.Id);
        }

        public int GetRelationCountIncludingDeletedOnes()
        {
            return _relationsById.Values.Count + _deletedRelationsById.Values.Count;
        }

        public int GetPersistedRelationCount()
        {
            return GetRelationCount();
        }

        public IEnumerable<IRelation> GetPersistedRelations()
        {
            return GetRelations();
        }

        private IEnumerable<IRelation> FindRelations(IElement consumer, IElement provider)
        {
            var key = new ValueTuple<int, int>(consumer.Id, provider.Id);
            return _consumerToProviderRelations.GetItems(key);
        }

        private IEnumerable<IRelation> FindRelationsInScope(IElement element, RelationDirection direction, RelationScope scope)
        {
            List<IRelation> relations = [];

            IDictionary<int, IElement> elementInScope = element.GetSelfAndChildrenRecursive().ToDictionary(x => x.Id);

            foreach (var e in elementInScope.Values)
            {
                if (direction != RelationDirection.Ingoing)
                {
                    foreach (var relation in GetConsumerRelations(e))
                    {
                        if (HasSelectedScope(scope, relation.Provider, elementInScope))
                        {
                            relations.Add(relation);
                        }
                    }
                }

                if (direction != RelationDirection.Outgoing)
                {
                    foreach (var relation in GetProviderRelations(e))
                    {
                        if (HasSelectedScope(scope, relation.Consumer, elementInScope))
                        {
                            relations.Add(relation);
                        }
                    }
                }
            }

            return relations;
        }

        private IEnumerable<IRelation> GetConsumerRelations(IElement consumer)
        {
            return _consumerRelations.GetItems(consumer.Id);
        }

        private IEnumerable<IRelation> GetProviderRelations(IElement provider)
        {
            return _providerRelations.GetItems(provider.Id);
        }

        private bool HasSelectedScope(RelationScope scope, IElement element, IDictionary<int, IElement> elements)
        {
            switch (scope)
            {
                case RelationScope.Internal:
                    return elements.ContainsKey(element.Id);
                case RelationScope.External:
                    return !elements.ContainsKey(element.Id);
                case RelationScope.Both:
                    return true;
                default:
                    return true;
            }
        }


        private IEnumerable<IRelation> FindRelationsBetweenElementsAndItsChildren(IElement consumer, IElement provider)
        {
            IList<IRelation> relations = new List<IRelation>();

            foreach (var c in consumer.GetSelfAndChildrenRecursive())
            {
                foreach (var p in provider.GetSelfAndChildrenRecursive())
                {
                    foreach (var relation in FindRelations(c, p))
                    {
                        if (!relation.IsDeleted)
                        {
                            relations.Add(relation);
                        }
                    }
                }
            }

            return relations;
        }

        private void RegisterRelation(IRelation relation)
        {
            ((Relation)relation).IsDeleted = false;
            _relationsById[relation.Id] = relation;

            if (_deletedRelationsById.ContainsKey(relation.Id))
            {
                _deletedRelationsById.Remove(relation.Id);
            }

            _consumerRelations.AddItem(relation.Consumer.Id, relation);
            _providerRelations.AddItem(relation.Provider.Id, relation);

            _deletedConsumerRelations.RemoveItem(relation.Consumer.Id, relation);
            _deletedProviderRelations.RemoveItem(relation.Provider.Id, relation);

            var key = new ValueTuple<int, int>(relation.Consumer.Id, relation.Provider.Id);
            _consumerToProviderRelations.AddItem(key, relation);

            RelationAdded?.Invoke(this, relation);
        }

        private void UnregisterRelation(IRelation relation)
        {
            ((Relation)relation).IsDeleted = true;
            _relationsById.Remove(relation.Id);

            _deletedRelationsById[relation.Id] = relation;

            _consumerRelations.RemoveItem(relation.Consumer.Id, relation);
            _providerRelations.RemoveItem(relation.Provider.Id, relation);

            _deletedConsumerRelations.AddItem(relation.Consumer.Id, relation);
            _deletedProviderRelations.AddItem(relation.Provider.Id, relation);

            var key = new ValueTuple<int, int>(relation.Consumer.Id, relation.Provider.Id);
            _consumerToProviderRelations.RemoveItem(key, relation);


            RelationRemoved?.Invoke(this, relation);
        }
    }
}
