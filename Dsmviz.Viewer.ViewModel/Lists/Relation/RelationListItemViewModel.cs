using System.ComponentModel;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.ViewModel.Lists.Relation;
using Dsmviz.Viewer.ViewModel.Common;

namespace Dsmviz.Viewer.ViewModel.Lists.Relation
{
    public class RelationListItemViewModel(IRelation relation) : ViewModelBase, IComparable, IRelationListItemViewModel
    {
        public IRelation Relation { get; set; } = relation;

        public int Index { get; set; }

        public string ConsumerPath { get; } = relation.Consumer.Parent?.Fullname ?? string.Empty;
        public string ConsumerName { get; } = relation.Consumer.Name;
        public string ProviderPath { get; } = relation.Provider.Parent?.Fullname ?? string.Empty;
        public string ProviderName { get; } = relation.Provider.Name;
        public string RelationType { get; } = relation.Type;
        public int RelationWeight { get; } = relation.Weight;

        public int CompareTo(object? obj)
        {
            RelationListItemViewModel? other = obj as RelationListItemViewModel;

            int compareConsumer = string.Compare(ConsumerName, other?.ConsumerName, StringComparison.Ordinal);
            int compareProvider = string.Compare(ProviderName, other?.ProviderName, StringComparison.Ordinal);

            if (compareConsumer != 0)
            {
                return compareConsumer;
            }
            else
            {
                return compareProvider;
            }
        }
    }
}
