using Dsmviz.Interfaces.Data.Entities;

namespace Dsmviz.Interfaces.Application.Metrics
{
    public interface IMetrics
    {
        /// <summary>
        /// Get the specified metric for the selected element.
        /// </summary>
        IMetric? GetMetric(MetricType type, IElement element);
    }
}
