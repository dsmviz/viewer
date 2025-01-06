

using Dsmviz.Interfaces.Application.Matrix;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Viewer.Utils;

namespace Dsmviz.Viewer.Application.Matrix
{
    public class DependencyCycleMatrix : IDependencyCycleMatrix
    {
        private readonly IDependencyWeightMatrix _dependencyWeightMatrix;

        private readonly SparseMatrix<Cycle> _cycles = new(Cycle.None);

        public event EventHandler<CycleEventArgs>? CycleChanged;

        public DependencyCycleMatrix(IDependencyWeightMatrix dependencyWeightMatrix)
        {
            _dependencyWeightMatrix = dependencyWeightMatrix;
            _dependencyWeightMatrix.DerivedWeightChanged += OnDerivedWeightChanged;
        }

        public Cycle GetCycle(IElement consumer, IElement provider)
        {
            return _cycles.GetValue(consumer.Id, provider.Id);
        }

        private void OnDerivedWeightChanged(object sender, WeightEventArgs eventData)
        {
            Evaluate(eventData.Consumer, eventData.Provider, eventData.Value);
        }

        private void Evaluate(IElement consumer, IElement provider, int derivedWeightConsumerToProvider)
        {
            if (derivedWeightConsumerToProvider > 0)
            {
                int derivedWeightProviderToConsumer = _dependencyWeightMatrix.GetDerivedDependencyWeight(provider, consumer);
                if (derivedWeightProviderToConsumer > 0)
                {
                    int directWeightConsumerToProvider = _dependencyWeightMatrix.GetDirectDependencyWeight(consumer, provider);
                    int directWeightProviderToConsumer = _dependencyWeightMatrix.GetDirectDependencyWeight(provider, consumer);

                    if ((directWeightConsumerToProvider > 0) &&
                        (directWeightProviderToConsumer > 0))
                    {
                        UpdateCycle(consumer, provider, Cycle.System);
                    }
                    else if ((derivedWeightConsumerToProvider > 0) &&
                             (derivedWeightProviderToConsumer > 0))
                    {
                        UpdateCycle(consumer, provider, Cycle.Hierarchical);
                    }
                    else
                    {
                        UpdateCycle(consumer, provider, Cycle.None);
                    }
                }
                else
                {
                    UpdateCycle(consumer, provider, Cycle.None);
                }
            }
            else
            {
                UpdateCycle(consumer, provider, Cycle.None);
            }
        }

        private void UpdateCycle(IElement consumer, IElement provider, Cycle updatedValue)
        {
            UpdateCycleForOneDirection(consumer, provider, updatedValue);
            UpdateCycleForOneDirection(provider, consumer, updatedValue);
        }

        private void UpdateCycleForOneDirection(IElement consumer, IElement provider, Cycle updatedValue)
        {
            Cycle oldValue = _cycles.GetValue(consumer.Id, provider.Id);
            if (_cycles.SetValue(consumer.Id, provider.Id, updatedValue))
            {
                CycleEventArgs eventData = new CycleEventArgs(consumer, provider, oldValue, updatedValue);
                CycleChanged?.Invoke(this, eventData);
            }
        }
    }
}

