using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.Data.Model.Elements;
using Dsmviz.Viewer.Data.Model.Common;
using Dsmviz.Viewer.Data.Model.Entities;
using Dsmviz.Viewer.Data.Model.Relations;
using Dsmviz.Viewer.Utils;

namespace Dsmviz.Viewer.Data.Model.Elements
{
    public class ElementModel : IElementModelPersistency, IElementModelEditing, IElementModelQuery
    {
        private readonly IRelationModelUpdate _relationModel;

        private readonly Dictionary<int /*id*/, IElement> _elementsById;
        private readonly Dictionary<int /*id*/, IElement> _deletedElementsById;
        private readonly Dictionary<string /*fullname*/, IElement> _elementsByName;

        private readonly TypeNameRegistration _typeNameRegistration = new();
        private readonly PropertyNameRegistration _propertyNameRegistration = new();

        private int _lastElementId;
        private readonly Element _root;


        public event EventHandler<IElement>? ElementAdded;
        public event EventHandler<IElement>? ElementRemoved;

        public ElementModel(IRelationModelUpdate relationModel)
        {
            _relationModel = relationModel;

            _elementsById = new Dictionary<int, IElement>();
            _deletedElementsById = new Dictionary<int, IElement>();

            _elementsByName = new Dictionary<string, IElement>();

            _root = new Element(_typeNameRegistration, _propertyNameRegistration, 0, "", "", null);

            Clear();
        }

        public void Clear()
        {
            _typeNameRegistration.Clear();
            _propertyNameRegistration.Clear();
            _elementsById.Clear();
            _deletedElementsById.Clear();
            _root.RemoveAllChildren();

            _lastElementId = 0;
            RegisterElement(_root);
        }

        public IElement ImportElement(int id, string name, string type, IDictionary<string, string>? properties, int order, bool expanded, bool bookmarked, int? parentId)
        {
            Logger.LogDataModelMessage($"Import element id={id} name={name} type={type} order={order} expanded={expanded} bookmarked={bookmarked} parentId={parentId}");

            if (id > _lastElementId)
            {
                _lastElementId = id;
            }
            return AddElementById(id, name, type, null, properties, order, expanded, bookmarked, parentId);
        }

        public IElement? AddElement(string name, string type, int? parentId, int? index, IDictionary<string, string>? properties)
        {
            Logger.LogDataModelMessage($"Add element name={name} type={type} parentId={parentId}");

            var fullname = name;
            if (parentId.HasValue)
            {
                var parent = GetElementById(parentId.Value);
                if (parent != null)
                {
                    var elementName = new ElementName(parent.Fullname);
                    elementName.AddNamePart(name);
                    fullname = elementName.FullName;
                }
            }

            var element = GetElementByFullname(fullname);
            if (element == null)
            {
                _lastElementId++;
                element = AddElementById(_lastElementId, name, type, index, properties, 0, false, false, parentId);
            }

            return element;
        }

        public void ChangeElementName(IElement element, string name)
        {
            UnregisterElementNameHierarchy(element);
            UpdateName(element, name);
            RegisterElementNameHierarchy(element);
        }

        public void ChangeElementType(IElement element, string type)
        {
            UpdateType(element, type);
        }

        public bool IsChangeElementParentAllowed(IElement element, IElement parent)
        {
            return element.Parent != null &&
                   element.Parent != parent && // Do not allow new parent same as current parent
                   !parent.IsRecursiveChildOf(element); // Do not allow new parent being a child of the changed element
        }

        public void ChangeElementParent(IElement element, IElement parent, int index)
        {
            Logger.LogDataModelMessage($"Change element parent name={element.Name} from {element.Parent?.Fullname} to {parent.Fullname}");

            if (IsChangeElementParentAllowed(element, parent))
            {
                Element? currentParent = (Element?)element.Parent;
                Element? newParent = (Element?)parent;

                if (currentParent != null & newParent != null)
                {
                    RemoveAllElementEdges(element);

                    UnregisterElementNameHierarchy(element);
                    currentParent?.RemoveChild(element);
                    if (currentParent != null) CollapseIfNoChildrenLeft(currentParent);

                    newParent?.InsertChildAtIndex(element, index);
                    RegisterElementNameHierarchy(element);

                    RestoreAllElementEdges(element);
                }
            }
        }

        public void RemoveElement(int elementId)
        {
            Logger.LogDataModelMessage($"Remove element id={elementId}");

            var element = GetElementById(elementId);
            if (element != null)
            {
                UnregisterElement(element);
                if (element.Parent != null) CollapseIfNoChildrenLeft(element.Parent);
            }
        }

        public void RestoreElement(int elementId)
        {
            Logger.LogDataModelMessage($"Restore element id={elementId}");

            if (_deletedElementsById.TryGetValue(elementId, out var element))
            {
                UndoUnregisterElement(element);
            }
        }

        public IEnumerable<string> GetElementTypes()
        {
            return _typeNameRegistration.GetRegisteredNames();
        }

        public IEnumerable<IElement> GetElements()
        {
            return _elementsById.Values;
        }

        public int GetElementCount()
        {
            return _elementsById.Count;
        }

        public IElement GetRootElement()
        {
            return _root;
        }

        public int GetPersistedElementCount()
        {
            return GetElementCount();
        }


        public void AssignElementOrder()
        {
            Logger.LogDataModelMessage("AssignElementOrder");

            var order = 1;
            foreach (var rootElement in _root.Children)
            {
                AssignElementOrder(rootElement, ref order);
            }
        }

        public IElement? GetElementById(int elementId)
        {
            return _elementsById.GetValueOrDefault(elementId);
        }

        public IElement? GetElementByFullname(string fullname)
        {
            return _elementsByName.GetValueOrDefault(fullname);
        }

        public IList<IElement> SearchElements(string searchText, IElement? searchInElement, bool caseSensitive, string elementTypeFilter, bool markMatchingElements)
        {
            List<IElement> matchingElements = [];

            var fullname = "";
            var text = caseSensitive ? searchText : searchText.ToLower();

            if (text.Length > 0)
            {
                RecursiveSearchElements(searchInElement, text, caseSensitive, elementTypeFilter, markMatchingElements, fullname, matchingElements);
            }
            else
            {
                ClearMarkElements(_root);
            }

            return matchingElements;
        }

        public void ReorderChildren(IElement element, IReadOnlyList<int> order)
        {
            if (element is Element parent)
            {
                List<IElement> clonedChildren = [.. parent.Children];

                foreach (var child in clonedChildren)
                {
                    parent.RemoveChild(child);
                }

                foreach (var index in order)
                {
                    parent.InsertChildAtEnd(clonedChildren[index]);
                }
            }
        }

        public bool Swap(IElement element1, IElement element2)
        {
            var swapped = false;

            if (element1.Parent == element2.Parent)
            {
                if (element1.Parent is Element parent)
                {
                    swapped = parent.Swap(element1, element2);
                }
            }

            return swapped;
        }

        public IElement? NextSibling(IElement? element)
        {
            IElement? nextSibling = null;
            var parent = element?.Parent;
            if (parent != null && element != null)
            {
                var index = parent.IndexOfChild(element);

                if (index < parent.Children.Count - 1)
                {
                    nextSibling = parent.Children[index + 1];
                }
            }
            return nextSibling;
        }

        public IElement? PreviousSibling(IElement? element)
        {
            IElement? previousSibling = null;
            var parent = element?.Parent;
            if (parent != null && element != null)
            {
                var index = parent.IndexOfChild(element);

                if (index > 0)
                {
                    previousSibling = parent.Children[index - 1];
                }
            }
            return previousSibling;
        }

        private bool RecursiveSearchElements(IElement? searchInElement, string searchText, bool caseSensitive, string elementTypeFilter, bool markMatchingElements, string fullname, IList<IElement> matchingElements)
        {
            var isMatch = false;

            if (fullname.Length > 0)
            {
                fullname += ".";
            }

            if (caseSensitive)
            {
                fullname += searchInElement?.Name ?? string.Empty;
            }
            else
            {
                fullname += searchInElement?.Name.ToLower() ?? string.Empty;
            }

            if (fullname.Contains(searchText) && IsElementFilterMatch(searchInElement, elementTypeFilter) && !IsElementDeleted(searchInElement))
            {
                // Add to list and mark searchInElement as search match
                isMatch = true;
                matchingElements.Add(searchInElement);
            }

            if (searchInElement != null)
            {
                foreach (var child in searchInElement.Children)
                {
                    if (RecursiveSearchElements(child, searchText, caseSensitive, elementTypeFilter,
                            markMatchingElements, fullname, matchingElements))
                    {
                        // Add parent searchInElement as match when it contains a matching child
                        isMatch = true;
                    }
                }

                if (markMatchingElements)
                {
                    searchInElement.IsMatch = isMatch;
                }
            }

            return isMatch;
        }

        private bool IsElementFilterMatch(IElement? searchInElement, string elementTypeFilter)
        {
            return string.IsNullOrEmpty(elementTypeFilter) || elementTypeFilter == searchInElement?.Type;
        }

        private bool IsElementDeleted(IElement? searchInElement)
        {
            if (searchInElement != null)
            {
                return searchInElement.IsDeleted;
            }
            else
            {
                return false;
            }
        }
        private void ClearMarkElements(IElement element)
        {
            element.IsMatch = false;

            foreach (var child in element.Children)
            {
                ClearMarkElements(child);
            }
        }

        private IElement AddElementById(int id, string name, string type, int? index, IDictionary<string, string>? properties, int order, bool expanded, bool bookmarked, int? parentId)
        {
            var element = new Element(_typeNameRegistration, _propertyNameRegistration, id, name, type, properties) { Order = order, IsExpanded = expanded, IsBookmarked = bookmarked, IsDeleted = false };

            if (parentId.HasValue)
            {
                var parent = GetElementById(parentId.Value);

                if (_deletedElementsById.TryGetValue(parentId.Value, out var value))
                {
                    parent = value;
                }

                if (parent is Element p)
                {
                    if (index.HasValue)
                    {
                        p.InsertChildAtIndex(element, index.Value);
                    }
                    else
                    {
                        p.InsertChildAtEnd(element);
                    }
                }
                else
                {
                    Logger.LogError($"Parent not found id={id}");
                }
            }
            else
            {
                _root.InsertChildAtEnd(element);
                _root.IsExpanded = true;
            }

            RegisterElement(element);

            return element;
        }

        private void AssignElementOrder(IElement element, ref int order)
        {
            UpdateOrder(element, order);

            order++;

            foreach (var child in element.Children)
            {
                AssignElementOrder(child, ref order);
            }
        }

        private void CollapseIfNoChildrenLeft(IElement element)
        {
            if (element is { Children.Count: 0 })
            {
                element.IsExpanded = false;
            }
        }

        private void RegisterElement(IElement element)
        {
            _elementsById[element.Id] = element;
            _elementsByName[element.Fullname] = element;

            ElementAdded?.Invoke(this, element);
        }

        private void UnregisterElement(IElement element)
        {
            RemoveAllElementEdges(element);

            UpdateDeleted(element, true);

            _deletedElementsById[element.Id] = element;
            _elementsById.Remove(element.Id);
            _elementsByName.Remove(element.Fullname);

            foreach (var child in element.ChildrenIncludingDeletedOnes)
            {
                UnregisterElement(child);
            }

            ElementRemoved?.Invoke(this, element);
        }

        private void UndoUnregisterElement(IElement element)
        {
            foreach (var child in element.ChildrenIncludingDeletedOnes)
            {
                UndoUnregisterElement(child);
            }

            _elementsById[element.Id] = element;
            _elementsByName[element.Fullname] = element;
            _deletedElementsById.Remove(element.Id);

            UpdateDeleted(element, false);

            RestoreAllElementEdges(element);

            ElementAdded?.Invoke(this, element);
        }

        protected virtual void RemoveAllElementEdges(IElement element)
        {
            _relationModel.RemoveAllElementRelations(element);
        }

        protected virtual void RestoreAllElementEdges(IElement element)
        {
            _relationModel.RestoreAllElementRelations(element);
        }

        private void UnregisterElementNameHierarchy(IElement element)
        {
            _elementsByName.Remove(element.Fullname);

            foreach (var child in element.ChildrenIncludingDeletedOnes)
            {
                UnregisterElementNameHierarchy(child);
            }
        }

        private void RegisterElementNameHierarchy(IElement element)
        {
            _elementsByName[element.Fullname] = element;

            foreach (var child in element.ChildrenIncludingDeletedOnes)
            {
                RegisterElementNameHierarchy(child);
            }
        }

        private void UpdateDeleted(IElement element, bool isDeleted)
        {
            if (element is Element e)
            {
                e.IsDeleted = isDeleted;
            }
        }

        private void UpdateOrder(IElement element, int order)
        {
            if (element is Element e)
            {
                e.Order = order;
            }
        }

        private void UpdateName(IElement element, string name)
        {
            if (element is Element e)
            {
                e.Name = name;
            }
        }

        private void UpdateType(IElement element, string type)
        {
            if (element is Element e)
            {
                e.Type = type;
            }
        }
    }
}
