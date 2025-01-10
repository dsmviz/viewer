namespace Dsmviz.Interfaces.ViewModel.Lists.Action
{
    public interface IActionListItemViewModel
    {
        int Index { get; }
        string Action { get; }
        string Details { get; }
    }
}
