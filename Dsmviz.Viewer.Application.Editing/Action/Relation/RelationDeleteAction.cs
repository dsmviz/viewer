using Dsmviz.Interfaces.Application.Editing;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.Data.Model.Relations;

namespace Dsmviz.Viewer.Application.Editing.Action.Relation
{
    public class RelationDeleteAction : IAction
    {
        private readonly IRelationModelEditing _relationModelEditing;
        private readonly IRelation _relation;
        private readonly IElement _consumer;
        private readonly IElement _provider;

        public const ActionType RegisteredType = ActionType.RelationDelete;

        public RelationDeleteAction(IRelationModelEditing relationModelEditing, IRelation relation)
        {
            _relationModelEditing = relationModelEditing;
            _relation = relation;
            _consumer = _relation.Consumer;
            _provider = _relation.Provider;
        }

        public ActionType Type => RegisteredType;
        public string Title => "Delete relation";
        public string Description => $"consumer={_consumer.Fullname} provider={_provider.Fullname} type={_relation.Type}";

        public object? Do()
        {
            _relationModelEditing.RemoveRelation(_relation.Id);
            return null;
        }

        public void Undo()
        {
            _relationModelEditing.RestoreRelation(_relation.Id);
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
