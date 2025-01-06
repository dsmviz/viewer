using Dsmviz.Interfaces.Data.Entities;

namespace Dsmviz.Viewer.Data.Model.Relations
{
    public interface IRelationModelUpdate
    {
        /// <summary>
        /// Remove all relations in which the removed element is involved as consumer or provider
        /// </summary>
        void RemoveAllElementRelations(IElement element);
        /// <summary>
        /// Restore all relations in which the restored element is involved as consumer or provider
        /// </summary>
        void RestoreAllElementRelations(IElement element);
    }
}
