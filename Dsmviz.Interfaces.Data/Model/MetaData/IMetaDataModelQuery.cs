using Dsmviz.Interfaces.Data.Entities;

namespace Dsmviz.Interfaces.Data.Model.MetaData
{
    public interface IMetaDataModelQuery
    {
        /// <summary>
        /// Get all metadata groups in the model.
        /// </summary>
        IEnumerable<string> GetMetaDataGroups();
        /// <summary>
        /// Get all metadata items in specified group.
        /// </summary>
        IEnumerable<IMetaDataItem> GetMetaDataGroupItems(string groupName);
    }
}
