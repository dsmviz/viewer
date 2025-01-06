namespace Dsmviz.Interfaces.Application.Editing
{
    public interface IEditing
    {
        IActionManagement ActionManagement { get; }
        IElementEditing ElementEditing { get; }
        IRelationEditing RelationEditing { get; }
    }
}
