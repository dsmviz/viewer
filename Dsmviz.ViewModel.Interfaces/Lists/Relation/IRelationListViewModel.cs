using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Dsmviz.ViewModel.Interfaces.Editing.Relation;

namespace Dsmviz.ViewModel.Interfaces.Lists.Relation
{
    public interface IRelationListViewModel
    {
        event EventHandler<IRelationEditViewModel>? RelationAddStarted;
        event EventHandler<IRelationEditViewModel>? RelationEditStarted;
        string Title { get; }
        string SubTitle { get; }
        ObservableCollection<IRelationListItemViewModel> Relations { get; }
        IRelationListItemViewModel? SelectedRelation { get; set; }
        ICommand CopyToClipboardCommand { get; }
        ICommand DeleteRelationCommand { get; }
        ICommand EditRelationCommand { get; }
        ICommand AddRelationCommand { get; }
        event PropertyChangedEventHandler? PropertyChanged;
    }
}
