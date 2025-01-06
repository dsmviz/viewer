using Dsmviz.Interfaces.Data.Model.Elements;
using Dsmviz.Interfaces.Data.Model.MetaData;
using Dsmviz.Interfaces.Data.Model.Relations;

namespace Dsmviz.Interfaces.Data.Model.Model
{
    public interface IModelPersistency
    {
        void Clear();

        int ModelVersion { get; set; }

        IMetaDataModelPersistency MetaDataModelPersistency { get; }
        IElementModelPersistency ElementModelPersistency { get; }
        IRelationModelPersistency RelationModelPersistency { get; }
    }
}
