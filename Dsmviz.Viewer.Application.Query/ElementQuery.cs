

using Dsmviz.Interfaces.Application.Query;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.Data.Model.Elements;

namespace Dsmviz.Viewer.Application.Query
{
    public class ElementQuery(IElementModelQuery elementModelQuery) : IElementQuery
    {
        public IElement RootElement => elementModelQuery.GetRootElement();

        public IEnumerable<string> GetElementTypes()
        {
            return elementModelQuery.GetElementTypes();
        }

        public IList<IElement> SearchElements(string searchText, IElement? searchInElement, bool caseSensitive,
            string elementTypeFilter, bool markMatchingElements)
        {
            return elementModelQuery.SearchElements(searchText, searchInElement, caseSensitive, elementTypeFilter, markMatchingElements);
        }
    }
}
