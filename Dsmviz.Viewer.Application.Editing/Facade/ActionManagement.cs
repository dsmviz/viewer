using Dsmviz.Interfaces.Application.Editing;
using Dsmviz.Viewer.Application.Editing.Action.Management;

namespace Dsmviz.Viewer.Application.Editing.Facade
{
    public class ActionManagement : IActionManagement
    {
        private readonly IActionManager _actionManager;

        public event EventHandler? ActionPerformed;

        public ActionManagement(IActionManager actionManager)
        {
            _actionManager = actionManager;
            _actionManager.ActionPerformed += OnActionPerformed;
        }

        private void OnActionPerformed(object sender, EventArgs e)
        {
            ActionPerformed?.Invoke(this, e);
        }

        public bool CanUndo()
        {
            return _actionManager.CanUndo();
        }

        public string GetUndoActionDescription()
        {
            return _actionManager.GetCurrentUndoAction()?.Title ?? string.Empty;
        }

        public void Undo()
        {
            _actionManager.Undo();
        }

        public bool CanRedo()
        {
            return _actionManager.CanRedo();
        }

        public string GetRedoActionDescription()
        {
            return _actionManager.GetCurrentRedoAction()?.Title ?? string.Empty;
        }

        public void Redo()
        {
            _actionManager.Redo();
        }

        public IEnumerable<IAction> GetActions()
        {
            return _actionManager.GetActionsInReverseChronologicalOrder();
        }

        public void ClearActions()
        {
            _actionManager.Clear();
        }
    }
}
