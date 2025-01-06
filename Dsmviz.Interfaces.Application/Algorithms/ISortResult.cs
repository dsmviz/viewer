
namespace Dsmviz.Interfaces.Application.Algorithms
{
    public interface ISortResult
    {
        /// <summary>
        /// Get sorted index values.
        /// </summary>
        IReadOnlyList<int> SortedIndexValues { get; }
        /// <summary>
        /// Is the sort result valid.
        /// </summary>
        bool IsValid { get; }
        /// <summary>
        /// Restore the original order before sorting.
        /// </summary>
        void InvertOrder();
        /// <summary>
        /// A textual description of the sorted order useful for logging and display.
        /// </summary>
        string ToString();
    }
}
