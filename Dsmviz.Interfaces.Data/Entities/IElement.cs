﻿namespace Dsmviz.Interfaces.Data.Entities
{
    public interface IElement : IComparable
    {
        /// <summary>
        /// Unique and non-modifiable Number identifying the element.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Number identifying sequential order of the element in element tree.
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Type of element.
        /// </summary>
        string Type { get; }

        /// <summary>
        /// Name of the element.
        /// </summary>
        string Name { get; }

        // Named properties found for this element
        IDictionary<string, string>? Properties { get; }

        /// <summary>
        /// Full name of the element based on its position in the element hierarchy
        /// </summary>
        string Fullname { get; }

        string GetRelativeName(IElement element);

        bool IsDeleted { get; }
        bool IsBookmarked { get; set; }
        bool IsRoot { get; }

        /// <summary>
        /// Has the element any children.
        /// </summary>
        bool HasChildren { get; }

        /// <summary>
        /// Children of the element.
        /// </summary>
        IList<IElement> Children { get; }
        IList<IElement> ChildrenIncludingDeletedOnes { get; }
        IList<IElement> PersistedChildren { get; }
        IList<IElement> GetSelfAndChildrenRecursive();
        IList<IElement> GetChildrenRecursive();
        int IndexOfChild(IElement child);

        bool ContainsChildWithName(string name);

        /// <summary>
        /// Parent of the element.
        /// </summary>
        IElement? Parent { get; }

        /// <summary>
        /// Total number of recursive children>
        /// </summary>
        int TotalElementCount { get; }

        /// <summary>
        /// Is the selected element a recursive child of this element.
        /// </summary>
        bool IsRecursiveChildOf(IElement element);

        /// <summary>
        /// Is the element expanded in the viewer.
        /// </summary>
        bool IsExpanded { get; set; }

        /// <summary>
        /// Is the element match in search.
        /// </summary>
        bool IsMatch { get; set; }

        /// <summary>
        /// Is the element included in the tree
        /// </summary>
        bool IsIncludedInTree { get; set; }
    }
}
