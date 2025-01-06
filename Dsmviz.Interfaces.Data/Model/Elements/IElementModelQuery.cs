

using Dsmviz.Interfaces.Data.Entities;

namespace Dsmviz.Interfaces.Data.Model.Elements
{
    public interface IElementModelQuery
    {
        /// <summary>
        /// Get element by its id.
        /// </summary>
        IElement? GetElementById(int id);
        /// <summary>
        /// Get all elements available in the model.
        /// </summary>
        IEnumerable<IElement> GetElements();
        /// <summary>
        /// Get total number of elements in the model.
        /// </summary>
        int GetElementCount();
        /// <summary>
        /// Get root element of element hierarchy.
        /// </summary>
        IElement GetRootElement();
        /// <summary>
        /// Get element by its full name.
        /// </summary>
        IElement? GetElementByFullname(string fullname);
        /// <summary>
        /// Search for element. It returns a list of matching element. Optionally the matching elements can be marked as matching for visualization reasons.
        /// </summary>
        IList<IElement> SearchElements(string searchText, IElement? searchInElement, bool caseSensitive, string elementTypeFilter, bool markMatchingElements);
        /// <summary>
        /// Get element types existing in the model.
        /// </summary>
        IEnumerable<string> GetElementTypes();
    }
}
