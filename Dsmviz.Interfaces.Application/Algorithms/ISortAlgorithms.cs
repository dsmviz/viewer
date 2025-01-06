
namespace Dsmviz.Interfaces.Application.Algorithms
{
    public interface ISortAlgorithms
    {
        /// <summary>
        /// Create sort algorithm for selected element with weight matrix and sport algorithm name as input.
        /// </summary>
        ISortAlgorithm? CreateAlgorithm(string algorithmName);
        /// <summary>
        /// Get list of supported sort algorithm names.
        /// </summary>
        IEnumerable<string> GetSupportedAlgorithms();
    }
}
