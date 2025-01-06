using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Viewer.Application.Editing.Action.Base;

namespace Dsmviz.Viewer.Application.Editing.Action.Management
{
    public class ActionContext : IActionContext
    {
        private IElement? _element;

        public void AddElementToClipboard(IElement element)
        {
            _element = element;
        }

        public void RemoveElementFromClipboard(IElement element)
        {
            _element = null;
        }

        public IElement? GetElementOnClipboard()
        {
            return _element;
        }

        public bool IsElementOnClipboard()
        {
            return _element != null;
        }
    }
}
