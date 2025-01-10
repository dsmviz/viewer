using System.ComponentModel;
using System.Windows.Input;
using Dsmviz.Interfaces.Data.Entities;

namespace Dsmviz.Interfaces.ViewModel.Editing.Relation
{
    public interface IRelationEditViewModel
    {
        event EventHandler<IRelation>? RelationUpdated;
        string Title { get; }
        string ConsumerName { get; }
        string ProviderName { get; }
        List<string> RelationTypes { get; }
        string SelectedRelationType { get; set; }
        int Weight { get; set; }
        string Help { get; }
        ICommand? AcceptChangeCommand { get; }
        event PropertyChangedEventHandler? PropertyChanged;
    }
}
