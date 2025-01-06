using Dsmviz.Interfaces.Data.Entities;

namespace Dsmviz.Interfaces.Data.Model.MetaData
{
    public interface IMetaDataModelEditing
    {
        /// <summary>
        /// Clear the model.
        /// </summary>
        void Clear();
        /// <summary>
        /// Add a new metadata item with specified name and value and selected group.
        /// </summary>
        IMetaDataItem AddMetaDataItem(string group, string name, string value);
    }
}
