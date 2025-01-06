using Dsmviz.Interfaces.Application.Metrics;

namespace Dsmviz.Viewer.Application.Metrics
{
    public class FloatMetric(float value, string unit) : IMetric
    {
        public string Unit { get; set; } = unit;
        public string FormattedValue { get; set; } = $"{value:0.000}";
        public bool IsZero { get; } = value == 0.00f;
    }
}
