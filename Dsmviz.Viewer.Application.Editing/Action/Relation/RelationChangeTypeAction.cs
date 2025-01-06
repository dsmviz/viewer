using Dsmviz.Interfaces.Application.Editing;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.Data.Model.Relations;

namespace Dsmviz.Viewer.Application.Editing.Action.Relation
{
    public class RelationChangeTypeAction : IAction
    {
        private readonly IRelationModelEditing _relationModelEditing;
        private readonly IRelation _relation;
        private readonly IElement _consumer;
        private readonly IElement _provider;
        private readonly string _old;
        private readonly string _new;

        public const ActionType RegisteredType = ActionType.RelationChangeType;

        public RelationChangeTypeAction(IRelationModelEditing relationModelEditing, IRelation relation, string type)
        {
            _relationModelEditing = relationModelEditing;
            _relation = relation;
            _consumer = _relation.Consumer;
            _provider = _relation.Provider;
            _old = relation.Type;
            _new = type;
        }

        public ActionType Type => RegisteredType;
        public string Title => "Change relation type";
        public string Description => $"consumer={_consumer.Fullname} provider={_provider.Fullname} type={_old}->{_new}";

        public object? Do()
        {
            _relationModelEditing.ChangeRelationType(_relation, _new);
            return null;
        }

        public void Undo()
        {
            _relationModelEditing.ChangeRelationType(_relation, _old);
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
