using Dsmviz.Interfaces.Data.Entities;

namespace Dsmviz.Interfaces.Application.Query
{
    public interface IRelationQuery
    {
        /// <summary>
        /// Get relation types existing in the model.
        /// </summary>
        IEnumerable<string> GetRelationTypes();
        /// <summary>
        /// Get interface of an element.
        /// </summary>
        IEnumerable<IElement> GetElementInterface(IElement element);
        /// <summary>
        /// Get elements used by an element.
        /// </summary>
        IEnumerable<IElement> GetElementProviders(IElement element);
        /// <summary>
        /// Get elements using an element.
        /// </summary>
        IEnumerable<IElement> GetElementConsumers(IElement element);
        /// <summary>
        /// Get provider in relations between specific consumer and provider and their children.
        /// </summary>
        IEnumerable<IElement> GetRelationProviders(IElement consumer, IElement provider);
        /// <summary>
        /// Get consumers in relations between specific consumer and provider and their children.
        /// </summary>
        IEnumerable<IElement> GetRelationConsumers(IElement consumer, IElement provider);
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
        /// Get all relations of between a consumer and a provider including their children.
        /// </summary>
        IEnumerable<IRelation> GetAllRelations(IElement consumer, IElement provider);
    }
}
