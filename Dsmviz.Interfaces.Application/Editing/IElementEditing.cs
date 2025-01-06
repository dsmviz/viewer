using Dsmviz.Interfaces.Data.Entities;

namespace Dsmviz.Interfaces.Application.Editing
{
    public interface IElementEditing
    {
        /// <summary>
        /// Get element types existing in the model to be used for newly created element.
        /// </summary>
        IEnumerable<string> GetElementTypes();
        /// <summary>
        /// Get element by its full name to check if created element already exists.
        /// </summary>
        IElement? GetElementByFullname(string fullname);
        /// <summary>
        /// Create a new element. Index refers to the position in the list of children of the parent where the element is created.
        /// </summary>
        IElement? CreateElement(string name, string type, IElement parent, int index);
        /// <summary>
        /// Delete specified element.
        /// </summary>
        void DeleteElement(IElement element);
        /// <summary>
        /// Change the name of an element.
        /// </summary>
        void ChangeElementName(IElement element, string name);
        /// <summary>
        /// Change the type of element.
        /// </summary>
        void ChangeElementType(IElement element, string type);
        /// <summary>
        /// Change the parent of an element. Index refers to the position in the list of children of the parent where the element is moved to.
        /// </summary>
        void ChangeElementParent(IElement element, IElement newParent, int index);
        /// <summary>
        /// Cut element on clipboard.
        /// </summary>
        void CutElement(IElement element);
        /// <summary>
        /// Copy element to clipboard.
        /// </summary>
        void CopyElement(IElement element);
        /// <summary>
        /// Is any element on clipboard which can be pasted
        /// </summary>
        /// <returns></returns>
        bool IsElementOnClipboard();
        /// <summary>
        /// Past element from clipboard. Index refers to the position in the list of children of the parent where the element is moved to.
        /// </summary>
        void PasteElement(IElement newParent, int index);
        /// <summary>
        /// Sort children of an element using specified algorithm.
        /// </summary>
        void Sort(IElement element, string algorithm);
        /// <summary>
        /// Get list of supported sort algorithm names.
        /// </summary>
        IEnumerable<string> GetSupportedSortAlgorithms();
        /// <summary>
        /// Get previous sibling. Returns null when it is the first element.
        /// </summary>
        IElement? PreviousSibling(IElement element);
        /// <summary>
        /// Get next sibling. Returns null when it is the last element.
        /// </summary>
        IElement? NextSibling(IElement element);
        /// <summary>
        /// Move element up in the of children of its parent.
        /// </summary>
        void MoveUp(IElement element);
        /// <summary>
        /// Move element down in the of children of its parent.
        /// </summary>
        void MoveDown(IElement element);
    }
}
