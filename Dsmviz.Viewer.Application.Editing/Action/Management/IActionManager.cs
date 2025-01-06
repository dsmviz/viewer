using Dsmviz.Interfaces.Application.Editing;
using Dsmviz.Viewer.Application.Editing.Action.Base;

namespace Dsmviz.Viewer.Application.Editing.Action.Management
{
    public interface IActionManager
    {
        event EventHandler? ActionPerformed;

        bool CanUndo();
        IAction? GetCurrentUndoAction();
        void Undo();

        bool CanRedo();
        IAction? GetCurrentRedoAction();
        void Redo();

        void Clear();
        void Add(IAction action);
        object? Execute(IAction action);
        IEnumerable<IAction> GetActionsInReverseChronologicalOrder();
        IActionContext GetContext();
    }
}
