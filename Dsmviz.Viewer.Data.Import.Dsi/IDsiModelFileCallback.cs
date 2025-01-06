

using Dsmviz.Interfaces.Data.Entities;

namespace Dsmviz.Viewer.Data.Import.Dsi
{
    public interface IDsiModelFileCallback
    {
        IMetaDataItem ImportMetaDataItem(string group, string name, string value);

        IElement? ImportElement(int id, string name, string type, IDictionary<string, string>? properties);

        IRelation? ImportRelation(int consumerId, int providerId, string type, int weight, IDictionary<string, string>? properties);
    }
}
