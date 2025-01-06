namespace Dsmviz.Interfaces.Application.Query
{
    public interface IQuery
    {
        IElementQuery ElementQuery { get; }
        IRelationQuery RelationQuery { get; }
    }
}
