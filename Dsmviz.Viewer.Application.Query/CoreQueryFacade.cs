
using Dsmviz.Interfaces.Application.Query;
using Dsmviz.Interfaces.Data.Model.Model;

namespace Dsmviz.Viewer.Application.Query
{
    public class CoreQueryFacade(IModelQuery modelQuery) : IQuery
    {
        public IElementQuery ElementQuery { get; } = new ElementQuery(modelQuery.ElementModelQuery);
        public IRelationQuery RelationQuery { get; } = new RelationQuery(modelQuery.RelationModelQuery);
    }
}
