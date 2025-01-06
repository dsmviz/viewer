namespace Dsmviz.Interfaces.Util
{
    public interface IFileProgress
    {
        void ReportStart(string actionText, string itemsType);
        void ReportProgress(int totalItemCount, int progressedItemCount);
        void ReportDone();
        void ReportError(string errorMessage);
    }
}
