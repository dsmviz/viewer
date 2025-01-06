using Dsmviz.Interfaces.Application.Editing;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.Data.Model.Relations;

namespace Dsmviz.Viewer.Application.Editing.Action.Relation
{
    public class RelationCreateAction(
        IRelationModelEditing relationModelEditing,
        IElement consumer,
        IElement provider,
        string type,
        int weight)
        : IAction
    {
        private IRelation? _relation;

        public const ActionType RegisteredType = ActionType.RelationCreate;

        public ActionType Type => RegisteredType;
        public string Title => "Create relation";
        public string Description => $"consumer={consumer.Fullname} provider={provider.Fullname} type={type} weight={weight}";

        public object? Do()
        {
            _relation = relationModelEditing.AddRelation(consumer, provider, type, weight, null);
            return _relation;
        }

        public void Undo()
        {
            if (_relation != null)
            {
                relationModelEditing.RemoveRelation(_relation.Id);
            }
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
