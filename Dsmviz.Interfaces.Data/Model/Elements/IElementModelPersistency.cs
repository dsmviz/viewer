

using Dsmviz.Interfaces.Data.Entities;

namespace Dsmviz.Interfaces.Data.Model.Elements
{
    public interface IElementModelPersistency
    {
        /// <summary>
        /// Import an element in the model.
        /// </summary>
        IElement? ImportElement(int id, string name, string type, IDictionary<string, string>? properties, int order, bool expanded, bool bookmarked, int? parentId);
        /// <summary>
        /// Get element by its id.
        /// </summary>
        IElement? GetElementById(int elementId);
        /// <summary>
        /// Get root element of element hierarchy.
        /// </summary>
        IElement GetRootElement();
        /// <summary>
        /// Get element count to be persisted
        /// </summary>
        int GetPersistedElementCount();
    }
}
