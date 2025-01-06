namespace Dsmviz.Interfaces.Application.Metrics
{
    public interface IMetric
    {
        /// <summary>
        ///  The formatted string value of the metric value.
        /// </summary>
        string FormattedValue { get; }
        /// <summary>
        /// The unit of the metric type. Empty string if not applicable.
        /// </summary>
        string Unit { get; set; }
        /// <summary>
        /// Has value zero
        /// </summary>
        bool IsZero { get; }
    }
}
