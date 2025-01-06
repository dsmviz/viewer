using Dsmviz.Interfaces.Data.Entities;

namespace Dsmviz.Interfaces.Data.Model.Relations
{
    public interface IRelationModelPersistency
    {
        /// <summary>
        /// Clear the model.
        /// </summary>
        void Clear();
        /// <summary>
        /// Import a relation in the model.
        /// </summary>
        IRelation? ImportRelation(int relationId, IElement consumer, IElement provider, string type, int weight, IDictionary<string, string>? properties);
        /// <summary>
        /// Get all relations to be persisted.
        /// </summary>
        IEnumerable<IRelation> GetPersistedRelations();
        /// <summary>
        /// Get relation count of all relations to be persisted.
        /// </summary>
        int GetPersistedRelationCount();
    }
}
