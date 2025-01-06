using Dsmviz.Interfaces.Data.Entities;

namespace Dsmviz.Interfaces.Data.Model.Relations
{
    public interface IRelationModelEditing
    {
        /// <summary>
        /// Clear the model.
        /// </summary>
        void Clear();
        /// <summary>
        /// Add a new relation to the model.
        /// </summary>
        IRelation? AddRelation(IElement consumer, IElement provider, string type, int weight, IDictionary<string, string>? properties);
        /// <summary>
        /// Remove a relation from the model.
        /// </summary>
        void RemoveRelation(int relationId);
        /// <summary>
        /// Restore a relation from the model.
        /// </summary>
        void RestoreRelation(int relationId);
        /// <summary>
        /// Change the type of relation.
        /// </summary>
        void ChangeRelationType(IRelation relation, string type);
        /// <summary>
        /// Change the weight of a relation.
        /// </summary>
        void ChangeRelationWeight(IRelation relation, int weight);

    }
}
