using System.ComponentModel;

namespace Dsmviz.Interfaces.ViewModel.Tooltips
{
    public interface IElementToolTipViewModel
    {
        string Title { get; }
        int Id { get; }
        string Name { get; }
        string Type { get; }
        event PropertyChangedEventHandler? PropertyChanged;
    }
}
