using System.ComponentModel;
using System.Windows.Input;

namespace Dsmviz.ViewModel.Interfaces.Lists.Action
{
    public interface IActionListViewModel
    {
        string Title { get; }
        IEnumerable<IActionListItemViewModel> Actions { get; set; }
        ICommand CopyToClipboardCommand { get; }
        ICommand ClearCommand { get; }
        event PropertyChangedEventHandler? PropertyChanged;
    }
}
