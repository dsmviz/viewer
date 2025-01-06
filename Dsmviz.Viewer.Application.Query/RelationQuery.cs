

using Dsmviz.Interfaces.Application.Query;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.Data.Model.Relations;

namespace Dsmviz.Viewer.Application.Query
{
    public class RelationQuery(IRelationModelQuery relationModelQuery) : IRelationQuery
    {
        public IEnumerable<string> GetRelationTypes()
        {
            return relationModelQuery.GetRelationTypes();
        }

        public IEnumerable<IElement> GetElementInterface(IElement element)
        {
            var relations = relationModelQuery.GetAllIngoingRelations(element)
                .OrderBy(x => x.Provider.Fullname)
                .GroupBy(x => x.Provider.Fullname)
                .Select(x => x.FirstOrDefault())
                .ToList();

            var elements = relations.Select(x => x.Provider)
                .ToList();
            return elements;
        }

        public IEnumerable<IElement> GetElementProviders(IElement element)
        {
            var relations = relationModelQuery.GetAllOutgoingRelations(element)
                .OrderBy(x => x.Provider.Fullname)
                .GroupBy(x => x.Provider.Fullname)
                .Select(x => x.FirstOrDefault())
                .ToList();

            var elements = relations.Select(x => x.Provider)
                .ToList();
            return elements;
        }

        public IEnumerable<IElement> GetElementConsumers(IElement element)
        {
            var relations = relationModelQuery.GetAllIngoingRelations(element)
                .OrderBy(x => x.Consumer.Fullname)
                .GroupBy(x => x.Consumer.Fullname)
                .Select(x => x.FirstOrDefault())
                .ToList();

            var elements = relations.Select(x => x.Consumer)
                .ToList();
            return elements;
        }

        public IEnumerable<IElement> GetRelationProviders(IElement consumer, IElement provider)
        {
            var relations = relationModelQuery.GetAllRelations(consumer, provider)
                .OrderBy(x => x.Provider.Fullname)
                .GroupBy(x => x.Provider.Fullname)
                .Select(x => x.FirstOrDefault())
                .ToList();

            var elements = relations.Select(x => x.Provider)
                .ToList();
            return elements;
        }


        public IEnumerable<IElement> GetRelationConsumers(IElement consumer, IElement provider)
        {
            var relations = relationModelQuery.GetAllRelations(consumer, provider)
                .OrderBy(x => x.Consumer.Fullname)
                .GroupBy(x => x.Consumer.Fullname)
                .Select(x => x.FirstOrDefault())
                .ToList();

            var elements = relations.Select(x => x.Consumer)
                .ToList();
            return elements;
        }

        public IEnumerable<IRelation> GetAllIngoingRelations(IElement element)
        {
            return relationModelQuery.GetAllIngoingRelations(element);
        }

        public IEnumerable<IRelation> GetAllOutgoingRelations(IElement element)
        {
            return relationModelQuery.GetAllOutgoingRelations(element);
        }

        public IEnumerable<IRelation> GetAllInternalRelations(IElement element)
        {
            return relationModelQuery.GetAllInternalRelations(element);
        }

        public IEnumerable<IRelation> GetAllRelations(IElement consumer, IElement provider)
        {
            return relationModelQuery.GetAllRelations(consumer, provider);
        }
    }
}
