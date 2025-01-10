using System.ComponentModel;
using System.Windows.Input;

namespace Dsmviz.Interfaces.ViewModel.Editing.Element
{
    public interface IElementEditViewModel
    {
        ICommand? AcceptChangeCommand { get; }
        string Title { get; }
        string Help { get; }
        string Name { get; set; }
        List<string> ElementTypes { get; }
        string SelectedElementType { get; set; }
        event PropertyChangedEventHandler? PropertyChanged;
    }
}
