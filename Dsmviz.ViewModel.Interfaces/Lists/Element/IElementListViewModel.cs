using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Dsmviz.ViewModel.Interfaces.Lists.Element
{
    public interface IElementListViewModel
    {
        string Title { get; }
        string SubTitle { get; }
        ObservableCollection<IElementListItemViewModel> Elements { get; }
        ICommand CopyToClipboardCommand { get; }
        event PropertyChangedEventHandler? PropertyChanged;
    }
}
