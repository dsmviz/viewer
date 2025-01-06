using Dsmviz.Interfaces.Data.Entities;

namespace Dsmviz.Interfaces.Application.Query
{
    public interface IElementQuery
    {
        /// <summary>
        /// Get root element of element hierarchy.
        /// </summary>
        IElement RootElement { get; }
        /// <summary>
        /// Get element types existing in the model.
        /// </summary>
        IEnumerable<string> GetElementTypes();
        /// <summary>
        /// Search for element. It returns a list of matching element. Optionally the matching elements can be marked as matching for visualization reasons.
        /// </summary>
        IList<IElement> SearchElements(string searchText, IElement? searchInElement, bool caseSensitive, string elementTypeFilter, bool markMatchingElements);
    }
}
