using Dsmviz.Interfaces.Application.Editing;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.Data.Model.Relations;

namespace Dsmviz.Viewer.Application.Editing.Action.Relation
{
    public class RelationChangeWeightAction : IAction
    {
        private readonly IRelationModelEditing _relationModelEditing;
        private readonly IRelation _relation;
        private readonly IElement _consumer;
        private readonly IElement _provider;
        private readonly int _old;
        private readonly int _new;

        public const ActionType RegisteredType = ActionType.RelationChangeWeight;

        public RelationChangeWeightAction(IRelationModelEditing relationModelEditing, IRelation relation, int weight)
        {
            _relationModelEditing = relationModelEditing;
            _relation = relation;
            _consumer = _relation.Consumer;
            _provider = _relation.Provider;
            _old = relation.Weight;
            _new = weight;
        }

        public ActionType Type => RegisteredType;
        public string Title => "Change relation weight";
        public string Description => $"consumer={_consumer.Fullname} provider={_provider.Fullname} type={_relation.Type} weight={_old}->{_new}";

        public object? Do()
        {
            _relationModelEditing.ChangeRelationWeight(_relation, _new);
            return null;
        }

        public void Undo()
        {
            _relationModelEditing.ChangeRelationWeight(_relation, _old);
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
