

using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.Data.Model.MetaData;
using Dsmviz.Viewer.Data.Model.Entities;
using Dsmviz.Viewer.Utils;

namespace Dsmviz.Viewer.Data.Model.MetaData
{
    public class MetaDataModel : IMetaDataModelPersistency, IMetaDataModelEditing, IMetaDataModelQuery
    {
        private readonly List<string> _metaDataGroupNames = [];
        private readonly Dictionary<string, List<IMetaDataItem>> _metaDataGroups = new();

        public void Clear()
        {
            _metaDataGroupNames.Clear();
            _metaDataGroups.Clear();
        }

        public IMetaDataItem ImportMetaDataItem(string groupName, string name, string value)
        {
            return AddMetaDataItem(groupName, name, value);
        }

        public IMetaDataItem AddMetaDataItem(string groupName, string name, string value)
        {
            Logger.LogDataModelMessage($"Add metadata group={groupName} name={name} value={value}");

            if (FindItem(groupName, name) is MetaDataItem item)
            {
                item.Value = value;
            }
            else
            {
                item = new MetaDataItem(name, value);
                GetMetaDataGroupItemList(groupName).Add(item);
            }
            return item;
        }

        public IEnumerable<string> GetMetaDataGroups()
        {
            return _metaDataGroupNames;
        }

        public IEnumerable<IMetaDataItem> GetMetaDataGroupItems(string groupName)
        {
            return GetMetaDataGroupItemList(groupName);
        }

        public IDictionary<string, List<IMetaDataItem>> GetMetaDataItems()
        {
            return _metaDataGroups;
        }

        private IList<IMetaDataItem> GetMetaDataGroupItemList(string groupName)
        {
            if (!_metaDataGroups.ContainsKey(groupName))
            {
                _metaDataGroupNames.Add(groupName);
                _metaDataGroups[groupName] = [];
            }

            return _metaDataGroups[groupName];
        }

        private IMetaDataItem? FindItem(string groupName, string name)
        {
            return (from item in GetMetaDataGroupItemList(groupName)
                    where item.Name == name
                    select item).FirstOrDefault();
        }
    }
}
