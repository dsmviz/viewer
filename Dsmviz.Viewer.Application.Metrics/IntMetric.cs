using Dsmviz.Interfaces.Application.Metrics;

namespace Dsmviz.Viewer.Application.Metrics
{
    public class IntMetric(int value, string unit) : IMetric
    {
        public string Unit { get; set; } = unit;
        public string FormattedValue { get; set; } = $"{value}";
        public bool IsZero { get; } = value == 0;
    }
}
