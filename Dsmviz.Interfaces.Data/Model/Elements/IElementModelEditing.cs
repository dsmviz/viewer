
using Dsmviz.Interfaces.Data.Entities;

namespace Dsmviz.Interfaces.Data.Model.Elements
{
    public interface IElementModelEditing
    {
        /// <summary>
        /// Clear the model.
        /// </summary>
        void Clear();
        /// <summary>
        /// Add a new element to the model.
        /// </summary>
        IElement? AddElement(string name, string type, int? parentId, int? index, IDictionary<string, string>? properties);
        /// <summary>
        /// Remove an element from the model.
        /// </summary>
        void RemoveElement(int elementId);
        /// <summary>
        /// Restore of an element from the model.
        /// </summary>
        void RestoreElement(int elementId);
        /// <summary>
        /// Change the name of an element.
        /// </summary>
        void ChangeElementName(IElement element, string name);
        /// <summary>
        /// Change the type of element.
        /// </summary>
        void ChangeElementType(IElement element, string type);
        /// <summary>
        /// Check if changing element parent to specified value is allowed. 
        /// </summary>
        bool IsChangeElementParentAllowed(IElement element, IElement parent);
        /// <summary>
        /// Change the parent of an element.
        /// </summary>
        void ChangeElementParent(IElement element, IElement parent, int index);
        /// <summary>
        /// Assign order number to all elements in the model.
        /// </summary>
        void AssignElementOrder();
        /// <summary>
        /// Reorder children of an element using order of specified sort result.
        /// </summary>
        void ReorderChildren(IElement element, IReadOnlyList<int> order);
        /// <summary>
        /// Get previous sibling. Returns null when it is the first element.
        /// </summary>
        IElement? PreviousSibling(IElement element);
        /// <summary>
        /// Get next sibling. Returns null when it is the last element.
        /// </summary>
        IElement? NextSibling(IElement element);
        /// <summary>
        /// Swap the order of two elements.
        /// </summary>
        bool Swap(IElement first, IElement second);
    }
}
