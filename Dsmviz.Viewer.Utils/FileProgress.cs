using Dsmviz.Interfaces.Util;

namespace Dsmviz.Viewer.Utils
{
    public class FileProgress(Action<IFileProgressInfo> handler) : Progress<IFileProgressInfo>, IFileProgress
    {
        private FileProgressInfo? _progressInfo;
        private readonly IProgress<IFileProgressInfo> _progress = new Progress<IFileProgressInfo>(handler);
        private int _currentPercentage;

        public void ReportStart(string actionText, string itemsType)
        {
            _progressInfo = new FileProgressInfo(actionText, itemsType);
        }

        public void ReportProgress(int totalItemCount, int progressedItemCount)
        {
            if (_progressInfo != null)
            {
                _progressInfo.UpdateOnProgress(totalItemCount, progressedItemCount);
                if (_currentPercentage != _progressInfo.Percentage)
                {
                    // Only report to UI when percentage changed to avoid overloading progress bar with update so progress is not shown anymore
                    // and white empty window is shown
                    _currentPercentage = _progressInfo.Percentage;
                    _progress.Report(_progressInfo);
                }
            }
        }

        public void ReportDone()
        {
            if (_progressInfo != null)
            {
                _progressInfo.UpdateOnDone();
                _progress.Report(_progressInfo);
            }
        }

        public void ReportError(string errorMessage)
        {
            if (_progressInfo != null)
            {
                _progressInfo.UpdateOnError(errorMessage);
                _progress.Report(_progressInfo);
            }
        }
    }
}
