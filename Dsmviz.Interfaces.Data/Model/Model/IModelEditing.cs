using Dsmviz.Interfaces.Data.Model.Elements;
using Dsmviz.Interfaces.Data.Model.MetaData;
using Dsmviz.Interfaces.Data.Model.Relations;

namespace Dsmviz.Interfaces.Data.Model.Model
{
    public interface IModelEditing
    {
        IMetaDataModelEditing MetaDataModelEditing { get; }
        IElementModelEditing ElementModelEditing { get; }
        IRelationModelEditing RelationModelEditing { get; }
    }
}
