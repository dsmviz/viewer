using Dsmviz.Interfaces.Data.Model.Elements;
using Dsmviz.Interfaces.Data.Model.MetaData;
using Dsmviz.Interfaces.Data.Model.Relations;

namespace Dsmviz.Interfaces.Data.Model.Model
{
    public interface IModelQuery
    {
        IMetaDataModelQuery MetaDataModelQuery { get; }

        IElementModelQuery ElementModelQuery { get; }

        IRelationModelQuery RelationModelQuery { get; }
    }
}
