using Dsmviz.Interfaces.Data.Model.Elements;
using Dsmviz.Interfaces.Data.Model.MetaData;
using Dsmviz.Interfaces.Data.Model.Model;
using Dsmviz.Interfaces.Data.Model.Relations;
using Dsmviz.Viewer.Data.Model.Elements;
using Dsmviz.Viewer.Data.Model.MetaData;
using Dsmviz.Viewer.Data.Model.Relations;

namespace Dsmviz.Viewer.Data.Model.Facade
{
    public class CoreDataModel : IDataModel
    {
        private readonly MetaDataModel _metaDataModel;
        private readonly ElementModel _elementsDataModel;
        private readonly RelationModel _relationsDataModel;

        public CoreDataModel()
        {
            _metaDataModel = new MetaDataModel();
            _relationsDataModel = new RelationModel();
            _elementsDataModel = new ElementModel(_relationsDataModel);
        }

        public int ModelVersion { get; set; }

        public void Clear()
        {
            _metaDataModel.Clear();
            _elementsDataModel.Clear();
            _relationsDataModel.Clear();
        }

        public IMetaDataModelEditing MetaDataModelEditing => _metaDataModel;

        public IMetaDataModelQuery MetaDataModelQuery => _metaDataModel;

        public IMetaDataModelPersistency MetaDataModelPersistency => _metaDataModel;

        public IElementModelEditing ElementModelEditing => _elementsDataModel;

        public IElementModelQuery ElementModelQuery => _elementsDataModel;

        public IElementModelPersistency ElementModelPersistency => _elementsDataModel;

        public IRelationModelEditing RelationModelEditing => _relationsDataModel;

        public IRelationModelQuery RelationModelQuery => _relationsDataModel;

        public IRelationModelPersistency RelationModelPersistency => _relationsDataModel;
    }
}
