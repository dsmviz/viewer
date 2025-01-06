using Dsmviz.Interfaces.Data.Entities;

namespace Dsmviz.Viewer.Data.Model.Entities
{
    public class MetaDataItem(string name, string value) : IMetaDataItem
    {
        public string Name { get; } = name;
        public string Value { get; set; } = value;
    }
}
