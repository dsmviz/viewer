using Dsmviz.Interfaces.Data.Entities;

namespace Dsmviz.Interfaces.Application.Editing
{
    public interface IRelationEditing
    {
        /// <summary>
        /// Get relation types existing in the model to be used for newly created relation.
        /// </summary>
        IEnumerable<string> GetRelationTypes();
        /// <summary>
        /// Create a new relation.
        /// </summary>
        IRelation? CreateRelation(IElement consumer, IElement provider, string type, int weight);
        /// <summary>
        /// Delete specified relation.
        /// </summary>
        void DeleteRelation(IRelation relation);
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
