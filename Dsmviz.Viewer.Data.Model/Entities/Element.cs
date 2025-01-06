
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Viewer.Data.Model.Common;

namespace Dsmviz.Viewer.Data.Model.Entities
{
    /// <summary>
    /// Represent an element in the dsm hierarchy.
    /// </summary>
    public class Element : IElement
    {
        private char _typeId;
        private readonly List<IElement> _children = [];
        private Element? _parent;
        private readonly TypeNameRegistration _elementTypeNameRegistration;

        public Element(TypeNameRegistration elementTypeNameRegistration, PropertyNameRegistration elementPropertyNameRegistration, int id, string name, string type, IDictionary<string, string>? properties, int order = 0, bool isExpanded = false)
        {
            _elementTypeNameRegistration = elementTypeNameRegistration;

            Id = id;
            Name = name;
            _typeId = _elementTypeNameRegistration.RegisterName(type);
            Properties = properties;
            Order = order;
            IsExpanded = isExpanded;
            IsIncludedInTree = true;

            if (properties != null)
            {
                foreach (var key in properties.Keys)
                {
                    elementPropertyNameRegistration.RegisterName(key);
                }
            }
        }

        public int Id { get; }

        public int Order { get; set; }

        public string Type
        {
            get => _elementTypeNameRegistration.GetRegisteredName(_typeId);
            set => _typeId = _elementTypeNameRegistration.RegisterName(value);
        }

        public string Name { get; set; }

        public IDictionary<string, string>? Properties { get; }

        public bool IsDeleted { get; set; }

        public bool IsRoot => Parent == null;

        public string Fullname
        {
            get
            {
                var fullname = Name;
                var parent = Parent;
                while (parent != null)
                {
                    if (parent.Name.Length > 0)
                    {
                        fullname = parent.Name + "." + fullname;
                    }
                    parent = parent.Parent;
                }
                return fullname;
            }
        }

        public string GetRelativeName(IElement element)
        {
            var fullname = Name;
            var parent = Parent;
            while (parent != element && parent != null)
            {
                if (parent.Name.Length > 0)
                {
                    fullname = parent.Name + "." + fullname;
                }
                parent = parent.Parent;
            }
            return fullname;
        }

        public bool IsExpanded { get; set; }
        public bool IsBookmarked { get; set; }

        public bool IsMatch { get; set; }

        public bool IsIncludedInTree { get; set; }

        public IElement? Parent => _parent;

        public int TotalElementCount
        {
            get
            {
                var count = 0;
                CountChildren(this, ref count);
                return count;
            }
        }

        public int ChildCount => Children.Count;

        public bool IsRecursiveChildOf(IElement element)
        {
            var isRecursiveChildOf = false;

            var parent = Parent;
            while (parent != null && !isRecursiveChildOf)
            {
                if (parent == element)
                {
                    isRecursiveChildOf = true;
                }

                parent = parent.Parent;
            }
            return isRecursiveChildOf;
        }

        public IList<IElement> Children => _children.Where(child => child is { IsDeleted: false, IsIncludedInTree: true }).ToList();

        public IList<IElement> PersistedChildren => _children.Where(child => child is { IsDeleted: false, IsIncludedInTree: true }).ToList();

        public IList<IElement> ChildrenIncludingDeletedOnes => _children;

        public int IndexOfChild(IElement child)
        {
            return _children.IndexOf(child);
        }

        public bool ContainsChildWithName(string name)
        {
            var containsChildWithName = false;
            foreach (var child in Children)
            {
                if (child.Name == name)
                {
                    containsChildWithName = true;
                }
            }

            return containsChildWithName;
        }

        public bool HasChildren => Children.Count > 0;

        public void InsertChildAtEnd(IElement child)
        {
            _children.Add(child);
            if (child is Element c)
            {
                c._parent = this;
            }
        }

        public void InsertChildAtIndex(IElement child, int index)
        {
            var rangeLimitedIndex = index;
            rangeLimitedIndex = Math.Min(rangeLimitedIndex, _children.Count);
            rangeLimitedIndex = Math.Max(rangeLimitedIndex, 0);
            _children.Insert(rangeLimitedIndex, child);
            if (child is Element c)
            {
                c._parent = this;
            }
        }

        public void RemoveChild(IElement child)
        {
            _children.Remove(child);
            if (child is Element c)
            {
                c._parent = null;
            }
        }

        public void RemoveAllChildren()
        {
            foreach (var child in Children)
            {
                RemoveChild(child);
            }
        }

        public IList<IElement> GetSelfAndChildrenRecursive()
        {
            List<IElement> elements = [];
            GetElementAndItsChildren(this, elements);
            return elements;
        }

        public IList<IElement> GetChildrenRecursive()
        {
            List<IElement> elements = [];
            foreach (var child in Children)
            {
                GetElementAndItsChildren(child, elements);
            }
            return elements;
        }

        private void GetElementAndItsChildren(IElement element, List<IElement> elements)
        {
            if (!element.IsDeleted)
            {
                if (element is Element toBeAddedElement)
                {
                    elements.Add(toBeAddedElement);
                }
            }

            foreach (var child in element.Children)
            {
                GetElementAndItsChildren(child, elements);
            }
        }

        public bool Swap(IElement element1, IElement element2)
        {
            var swapped = false;

            if (_children.Contains(element1) && _children.Contains(element2))
            {
                var index1 = _children.IndexOf(element1);
                var index2 = _children.IndexOf(element2);

                _children[index2] = element1;
                _children[index1] = element2;

                swapped = true;
            }

            return swapped;
        }

        public int CompareTo(object? obj)
        {
            var element = obj as Element;
            return Id.CompareTo(element?.Id);
        }

        private void CountChildren(IElement element, ref int count)
        {
            count++;

            foreach (var child in element.Children)
            {
                CountChildren(child, ref count);
            }
        }
    }
}
