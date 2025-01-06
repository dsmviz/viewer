using Dsmviz.Interfaces.Util;

namespace Dsmviz.Viewer.Utils
{
    public class FileProgressInfo(string actionText, string itemsType) : IFileProgressInfo
    {
        public void UpdateOnProgress(int totalItemCount, int progressedItemCount)
        {
            TotalItemCount = totalItemCount;
            CurrentItemCount = progressedItemCount;
            if (TotalItemCount == 0)
            {
                Percentage = 100;
                State = ProgressState.Ready;
            }
            else
            {
                Percentage = progressedItemCount * 100 / TotalItemCount;
                State = TotalItemCount == progressedItemCount ? ProgressState.Ready : ProgressState.Busy;
            }
        }

        public void UpdateOnDone()
        {
            Percentage = 100;
            State = ProgressState.Ready;
        }

        public void UpdateOnError(string errorText)
        {
            ErrorText = errorText;
        }

        public string ActionText { get; } = actionText;
        public string ItemType { get; } = itemsType;
        public string ErrorText { get; private set; } = "";
        public int TotalItemCount { get; private set; }
        public int CurrentItemCount { get; private set; }
        public int Percentage { get; private set; }
        public ProgressState State { get; private set; } = ProgressState.Busy;
    }
}
