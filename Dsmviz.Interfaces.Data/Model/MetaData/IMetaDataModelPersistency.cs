using Dsmviz.Interfaces.Data.Entities;

namespace Dsmviz.Interfaces.Data.Model.MetaData
{
    public interface IMetaDataModelPersistency
    {
        /// <summary>
        /// Import a metadata item in the model.
        /// </summary>
        IMetaDataItem ImportMetaDataItem(string group, string name, string value);
        /// <summary>
        /// Get all metadata groups in the model.
        /// </summary>
        IEnumerable<string> GetMetaDataGroups();
        /// <summary>
        /// Get all metadata items in specified group.
        /// </summary>
        IEnumerable<IMetaDataItem> GetMetaDataGroupItems(string group);
        /// <summary>
        /// Get all metadata items with group as key in the collection.
        /// </summary>
        IDictionary<string, List<IMetaDataItem>> GetMetaDataItems();
    }
}
