namespace Dsmviz.Interfaces.Application.Editing
{
    public interface IAction
    {
        /// <summary>
        /// Type of action.
        /// </summary>
        ActionType Type { get; }
        /// <summary>
        /// Title of the action e.g. 'Create element'.
        /// </summary>
        string Title { get; }
        /// <summary>
        /// Detailed description of the açtion e.g. 'name=MyClass type=class parent=MyNamespace'.
        /// </summary>
        string Description { get; }
        /// <summary>
        /// Perform the action. In cases an element is created it is returned as object.
        /// </summary>
        object? Do();
        /// <summary>
        /// Undo the performed action.
        /// </summary>
        void Undo();
        /// <summary>
        /// Are the input parameter provided in the constructor valid.
        /// </summary>
        bool IsValid();
    }
}
