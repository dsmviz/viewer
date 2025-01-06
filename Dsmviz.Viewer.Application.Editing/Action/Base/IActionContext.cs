
using Dsmviz.Interfaces.Data.Entities;

namespace Dsmviz.Viewer.Application.Editing.Action.Base
{
    public interface IActionContext
    {
        void AddElementToClipboard(IElement element);
        void RemoveElementFromClipboard(IElement element);
        IElement? GetElementOnClipboard();
        bool IsElementOnClipboard();
    }
}
