namespace Dsmviz.Interfaces.Application.Editing
{
    public interface IActionManagement
    {
        /// <summary>
        /// Event triggered when an action has been completed.
        /// </summary>
        event EventHandler? ActionPerformed;

        /// <summary>
        /// Can most recent action be undone.
        /// </summary>
        bool CanUndo();
        /// <summary>
        /// Description of most recent undoable action.
        /// </summary>
        string GetUndoActionDescription();
        /// <summary>
        /// Undo most recent  action.
        /// </summary>
        void Undo();
        /// <summary>
        /// Can most recent undone action be redone.
        /// </summary>
        bool CanRedo();
        /// <summary>
        /// Description of most recent redoable action.
        /// </summary>
        string GetRedoActionDescription();
        /// <summary>
        /// Redo most recent undone action.
        /// </summary>
        void Redo();
        /// <summary>
        /// Get list of all actions.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IAction> GetActions();
        /// <summary>
        /// Clear list of actions.
        /// </summary>
        void ClearActions();
    }
}
