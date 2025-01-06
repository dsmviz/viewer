namespace Dsmviz.Interfaces.Data.Entities
{
    public interface IRelation : IEdge
    {
        /// <summary>
        /// Type of relation.
        /// </summary>
        string Type { get; }

        /// <summary>
        /// Strength or weight of the relation
        /// </summary>
        int Weight { get; }

        bool IsDeleted { get; }

        // Named properties found for this relation
        IDictionary<string, string>? Properties { get; }
    }
}
