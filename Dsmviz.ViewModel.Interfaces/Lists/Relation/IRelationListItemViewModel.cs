using System.ComponentModel;
using Dsmviz.Interfaces.Data.Entities;

namespace Dsmviz.Interfaces.ViewModel.Lists.Relation
{
    public interface IRelationListItemViewModel
    {
        IRelation Relation { get; set; }
        int Index { get; set; }
        string ConsumerPath { get; }
        string ConsumerName { get; }
        string ProviderPath { get; }
        string ProviderName { get; }
        string RelationType { get; }
        int RelationWeight { get; }
        int CompareTo(object? obj);
        event PropertyChangedEventHandler? PropertyChanged;
    }
}
