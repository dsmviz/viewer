namespace Dsmviz.Interfaces.Data.Entities
{
    public interface IEdge
    {
        /// <summary>
        /// Unique and non-modifiable Number identifying the edge.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// The consumer element.
        /// </summary>
        IElement Consumer { get; }

        /// <summary>
        /// The provider element.
        /// </summary>
        IElement Provider { get; }
    }
}
