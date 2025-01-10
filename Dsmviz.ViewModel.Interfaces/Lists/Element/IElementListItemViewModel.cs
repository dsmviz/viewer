using System.ComponentModel;

namespace Dsmviz.ViewModel.Interfaces.Lists.Element
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
