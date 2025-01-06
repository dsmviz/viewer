using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Viewer.ViewModel.Common;

namespace Dsmviz.Viewer.ViewModel.Lists.Element
{
    public class ElementListItemViewModel(IElement element) : ViewModelBase, IComparable
    {
        public int Index { get; set; }
        public string ElementPath { get; } = element.Parent?.Fullname ?? string.Empty;
        public string ElementName { get; } = element.Name;
        public string ElementType { get; } = element.Type;

        public int CompareTo(object? obj)
        {
            ElementListItemViewModel? other = obj as ElementListItemViewModel;
            return string.Compare(ElementName, other?.ElementName, StringComparison.Ordinal);
        }
    }
}
