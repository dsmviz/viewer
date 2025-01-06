

using Dsmviz.Interfaces.Application.Matrix;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.Data.Model.Relations;
using Dsmviz.Viewer.Utils;

namespace Dsmviz.Viewer.Application.Matrix
{
    public class DependencyWeightMatrix : IDependencyWeightMatrix
    {
        private readonly IRelationModelQuery _relationModelQuery;

        private readonly IntSparseMatrix _cellDirectWeights = new();
        private readonly IntSparseMatrix _cellDerivedWeights = new();

        public event EventHandler<WeightEventArgs>? DerivedWeightChanged;

        public DependencyWeightMatrix(IRelationModelQuery relationModelQuery)
        {
            _relationModelQuery = relationModelQuery;

            _relationModelQuery.RelationAdded += OnRelationAdded;
            _relationModelQuery.RelationRemoved += OnRelationRemoved;
        }

        public int GetDirectDependencyWeight(IElement consumer, IElement provider)
        {
            return _cellDirectWeights.GetValue(consumer.Id, provider.Id);
        }

        public int GetDerivedDependencyWeight(IElement consumer, IElement provider)
        {
            return _cellDerivedWeights.GetValue(consumer.Id, provider.Id);
        }

        private void AddDirectWeight(IRelation relation)
        {
            if (relation.Consumer.Id != relation.Provider.Id)
            {
                _cellDirectWeights.AddValue(relation.Consumer.Id, relation.Provider.Id, relation.Weight);
            }
        }

        private void RemoveDirectWeight(IRelation relation)
        {
            if (relation.Consumer.Id != relation.Provider.Id)
            {
                _cellDirectWeights.SubtractValue(relation.Consumer.Id, relation.Provider.Id, relation.Weight);
            }
        }

        private void RecursiveAddDerivedWeight(IRelation relation)
        {
            IElement currentConsumer = relation.Consumer;
            while (currentConsumer != null)
            {
                IElement currentProvider = relation.Provider;
                while (currentProvider != null)
                {
                    if (currentConsumer.Id != currentProvider.Id &&
                        !currentConsumer.IsRoot &&
                        !currentProvider.IsRoot &&
                        !IsRecursiveChildOf(currentConsumer, currentProvider) &&
                        !IsRecursiveChildOf(currentProvider, currentConsumer))
                    {
                        AddDerivedWeight(currentConsumer, currentProvider, relation.Weight);
                    }

                    currentProvider = currentProvider.Parent;
                }

                currentConsumer = currentConsumer.Parent;
            }
        }

        private void AddDerivedWeight(IElement consumer, IElement provider, int weight)
        {
            int updatedDerivedWeight = _cellDerivedWeights.AddValue(consumer.Id, provider.Id, weight);
            WeightEventArgs value = new WeightEventArgs(consumer, provider, updatedDerivedWeight);
            DerivedWeightChanged?.Invoke(this, value);
        }

        private void RecursiveRemoveDerivedWeight(IRelation relation)
        {
            IElement currentConsumer = relation.Consumer;
            while (currentConsumer != null)
            {
                IElement currentProvider = relation.Provider;
                while (currentProvider != null)
                {
                    if (currentConsumer.Id != currentProvider.Id &&
                        !currentConsumer.IsRoot &&
                        !currentProvider.IsRoot &&
                        !IsRecursiveChildOf(currentConsumer, currentProvider) &&
                        !IsRecursiveChildOf(currentProvider, currentConsumer))
                    {
                        RemoveDerivedWeight(currentConsumer, currentProvider, relation.Weight);
                    }

                    currentProvider = currentProvider.Parent;
                }

                currentConsumer = currentConsumer.Parent;
            }
        }

        private bool IsRecursiveChildOf(IElement element1, IElement element2)
        {
            bool isRecursiveChildOf = false;

            IElement parent = element2.Parent;
            while (parent != null && !isRecursiveChildOf)
            {
                if (parent == element1)
                {
                    isRecursiveChildOf = true;
                }

                parent = parent.Parent;
            }

            return isRecursiveChildOf;
        }

        private void RemoveDerivedWeight(IElement consumer, IElement provider, int weight)
        {
            int updatedDerivedWeight = _cellDerivedWeights.SubtractValue(consumer.Id, provider.Id, weight);
            WeightEventArgs value = new WeightEventArgs(consumer, provider, updatedDerivedWeight);
            DerivedWeightChanged?.Invoke(this, value);
        }

        private void OnRelationAdded(object sender, IRelation relation)
        {
            AddDirectWeight(relation);
            RecursiveAddDerivedWeight(relation);
        }

        private void OnRelationRemoved(object sender, IRelation relation)
        {
            RemoveDirectWeight(relation);
            RecursiveRemoveDerivedWeight(relation);
        }
    }
}
