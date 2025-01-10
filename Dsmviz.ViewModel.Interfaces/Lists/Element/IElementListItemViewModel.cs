using System.ComponentModel;

namespace Dsmviz.Interfaces.ViewModel.Lists.Element
{
    public interface IElementListItemViewModel
    {
        int Index { get; set; }
        string ElementPath { get; }
        string ElementName { get; }
        string ElementType { get; }
        int CompareTo(object? obj);
        event PropertyChangedEventHandler? PropertyChanged;
    }
}
