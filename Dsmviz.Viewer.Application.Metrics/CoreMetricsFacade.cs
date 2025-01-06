

using Dsmviz.Interfaces.Application.Matrix;
using Dsmviz.Interfaces.Application.Metrics;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.Data.Model.Elements;
using Dsmviz.Interfaces.Data.Model.Model;
using Dsmviz.Interfaces.Data.Model.Relations;
using Dsmviz.Viewer.Utils;

namespace Dsmviz.Viewer.Application.Metrics
{
    public class CoreMetricsFacade : IMetrics
    {
        private readonly IntSparseArray _systemCycleCount = new();
        private readonly IntSparseArray _hierarchicalCycleCount = new();

        private readonly IElementModelQuery _elementModelQuery;
        private readonly IRelationModelQuery _relationModelQuery;
        private readonly IDependencyCycleMatrix _dependencyCycleMatrix;

        public CoreMetricsFacade(IModelQuery modelQuery,
                             IDependencyCycleMatrix dependencyCycleMatrix)
        {
            _elementModelQuery = modelQuery.ElementModelQuery;
            _relationModelQuery = modelQuery.RelationModelQuery;

            _dependencyCycleMatrix = dependencyCycleMatrix;
            _dependencyCycleMatrix.CycleChanged += OnCycleChanged;
        }

        public IMetric? GetMetric(MetricType metricType, IElement element)
        {
            switch (metricType)
            {
                case MetricType.NumberOfElements:
                    {
                        int metricCount = element.TotalElementCount;
                        return new IntMetric(metricCount, "");
                    }
                case MetricType.RelativeSizePercentage:
                    {
                        int childElementCount = element.TotalElementCount;
                        int totalElementCount = _elementModelQuery.GetElementCount();
                        float metricCount = totalElementCount > 0 ? childElementCount * 100.0f / totalElementCount : 0;
                        return new FloatMetric(metricCount, "%");
                    }
                case MetricType.IngoingRelations:
                    {
                        int metricCount = _relationModelQuery.GetAllIngoingRelations(element).ToList().Count;
                        return new IntMetric(metricCount, "");
                    }
                case MetricType.OutgoingRelations:
                    {
                        int metricCount = _relationModelQuery.GetAllOutgoingRelations(element).ToList().Count;
                        return new IntMetric(metricCount, "");
                    }
                case MetricType.InternalRelations:
                    {
                        int metricCount = _relationModelQuery.GetAllInternalRelations(element).ToList().Count;
                        return new IntMetric(metricCount, "");
                    }
                case MetricType.ExternalRelations:
                    {
                        int metricCount = _relationModelQuery.FindAllExternalRelations(element).ToList().Count;
                        return new IntMetric(metricCount, "");
                    }
                case MetricType.HierarchicalCycles:
                    {
                        int metricCount = GetHierarchicalCycleCount(element);
                        return new IntMetric(metricCount, "");
                    }
                case MetricType.SystemCycles:
                    {
                        int metricCount = GetSystemCycleCount(element);
                        return new IntMetric(metricCount, "");
                    }
                case MetricType.Cycles:
                    {
                        int metricCount = GetHierarchicalCycleCount(element) +
                                          GetSystemCycleCount(element);
                        return new IntMetric(metricCount, "");
                    }
                case MetricType.CycalityPercentage:
                    {
                        int cycleCount = GetHierarchicalCycleCount(element) +
                                         GetSystemCycleCount(element);
                        int relationCount = _relationModelQuery.GetAllInternalRelations(element).ToList().Count;
                        float metricCount = relationCount > 0 ? cycleCount * 100.0f / relationCount : 0;
                        return new FloatMetric(metricCount, "%");
                    }
                default:
                    return null;
            }
        }

        private int GetSystemCycleCount(IElement element)
        {
            return _systemCycleCount.GetValue(element.Id);
        }

        private int GetHierarchicalCycleCount(IElement element)
        {
            return _hierarchicalCycleCount.GetValue(element.Id);
        }

        private void OnCycleChanged(object sender, CycleEventArgs e)
        {
            switch (e.NewValue)
            {
                case Cycle.System:
                    switch (e.OldValue)
                    {
                        case Cycle.System:
                            break;
                        case Cycle.Hierarchical:
                            IncrementCount(_systemCycleCount, e.Provider);
                            break;
                        case Cycle.None:
                            IncrementCount(_systemCycleCount, e.Provider);
                            break;
                    }
                    break;
                case Cycle.Hierarchical:
                    switch (e.OldValue)
                    {
                        case Cycle.System:
                            IncrementCount(_hierarchicalCycleCount, e.Provider);
                            break;
                        case Cycle.Hierarchical:
                            break;
                        case Cycle.None:
                            IncrementCount(_hierarchicalCycleCount, e.Provider);
                            break;
                    }
                    break;
                case Cycle.None:
                    switch (e.OldValue)
                    {
                        case Cycle.System:
                            DecrementCount(_systemCycleCount, e.Provider);
                            break;
                        case Cycle.Hierarchical:
                            DecrementCount(_hierarchicalCycleCount, e.Provider);
                            break;
                        case Cycle.None:
                            break;
                    }
                    break;
            }
        }

        private void IncrementCount(IntSparseArray providerValues, IElement provider)
        {
            IElement? currentProvider = provider;
            while (currentProvider != null)
            {
                if (!currentProvider.IsRoot)
                {
                    providerValues.IncrementValue(currentProvider.Id);
                }

                currentProvider = currentProvider.Parent;
            }
        }

        private void DecrementCount(IntSparseArray providerValues, IElement provider)
        {
            IElement? currentProvider = provider;
            while (currentProvider != null)
            {
                if (!currentProvider.IsRoot)
                {
                    providerValues.DecrementValue(currentProvider.Id);
                }

                currentProvider = currentProvider.Parent;
            }
        }
    }
}
