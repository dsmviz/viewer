using Dsmviz.Interfaces.Data.Entities;

namespace Dsmviz.Interfaces.Data.Model.Relations
{
    public interface IRelationModelQuery
    {
        /// <summary>
        /// Event triggered when new relation is added to the model using Dsmviz.Interfaces.Data.Entities;
        /// </summary>
        event EventHandler<IRelation>? RelationAdded;
        /// <summary>
        /// Event triggered when existing relation is removed from  the model.
        /// </summary>
        event EventHandler<IRelation>? RelationRemoved;

        /// <summary>
        /// Get all relations available in the model.
        /// </summary>
        IEnumerable<IRelation> GetRelations();
        /// <summary>
        /// Get total number of relations in the model.
        /// </summary>
        int GetRelationCount();
        /// <summary>
        /// Find specific relation with specified attributes.
        /// </summary>
        IRelation? FindRelation(IElement consumer, IElement provider, string type, int weight);
        /// <summary>
        /// Get all ingoing relations of an element including its children.
        /// </summary>
        IEnumerable<IRelation> GetAllIngoingRelations(IElement element);
        /// <summary>
        /// Get all outgoing relations of an element including its children.
        /// </summary>
        IEnumerable<IRelation> GetAllOutgoingRelations(IElement element);
        /// <summary>
        /// Get all internal relations of an element between its children.
        /// </summary>
        IEnumerable<IRelation> GetAllInternalRelations(IElement element);
        /// <summary>
        /// Get all ingoing and outgoing relations of an element including its children.
        /// </summary>
        IEnumerable<IRelation> FindAllExternalRelations(IElement element);
        /// <summary>
        /// Get all relations of between a consumer and a provider including their children.
        /// </summary>
        IEnumerable<IRelation> GetAllRelations(IElement consumer, IElement provider);
        /// <summary>
        /// Get relation types existing in the model.
        /// </summary>
        IEnumerable<string> GetRelationTypes();
    }
}
