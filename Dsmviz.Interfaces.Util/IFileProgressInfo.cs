namespace Dsmviz.Interfaces.Util
{
    public interface IFileProgressInfo
    {
        string ActionText { get; }
        string ItemType { get; }
        string ErrorText { get; }
        int TotalItemCount { get; }
        int CurrentItemCount { get; }
        int Percentage { get; }
        ProgressState State { get; }
    }
}
