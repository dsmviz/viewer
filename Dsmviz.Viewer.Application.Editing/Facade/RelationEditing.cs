using Dsmviz.Interfaces.Application.Editing;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.Data.Model.Model;
using Dsmviz.Interfaces.Data.Model.Relations;
using Dsmviz.Viewer.Application.Editing.Action.Management;
using Dsmviz.Viewer.Application.Editing.Action.Relation;

namespace Dsmviz.Viewer.Application.Editing.Facade
{
    public class RelationEditing(IActionManager actionManager, IDataModel dataModel) : IRelationEditing
    {
        private readonly IRelationModelQuery _relationModelQuery = dataModel.RelationModelQuery;
        private readonly IRelationModelEditing _relationModelEditing = dataModel.RelationModelEditing;

        public IRelation? CreateRelation(IElement consumer, IElement provider, string type, int weight)
        {
            RelationCreateAction action = new RelationCreateAction(_relationModelEditing, consumer, provider, type, weight);
            return actionManager.Execute(action) as IRelation;
        }

        public void DeleteRelation(IRelation relation)
        {
            RelationDeleteAction action = new RelationDeleteAction(_relationModelEditing, relation);
            actionManager.Execute(action);
        }

        public void ChangeRelationType(IRelation relation, string type)
        {
            RelationChangeTypeAction action = new RelationChangeTypeAction(_relationModelEditing, relation, type);
            actionManager.Execute(action);
        }

        public void ChangeRelationWeight(IRelation relation, int weight)
        {
            RelationChangeWeightAction action = new RelationChangeWeightAction(_relationModelEditing, relation, weight);
            actionManager.Execute(action);
        }

        public IEnumerable<string> GetRelationTypes()
        {
            return _relationModelQuery.GetRelationTypes();
        }
    }
}
