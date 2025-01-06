
using Dsmviz.Interfaces.Util;
using Dsmviz.Viewer.ViewModel.Common;

namespace Dsmviz.Viewer.ViewModel.Main
{
    public class ProgressViewModel : ViewModelBase
    {
        public event EventHandler<ProgressState>? StateChanged;

        private ProgressState _state = ProgressState.Ready;
        private string _title = string.Empty;
        private string _errorText = string.Empty;
        private string _progressText = string.Empty;
        private int _progressValue;

        public void Update(IFileProgressInfo progress)
        {
            switch (progress.State)
            {
                case ProgressState.Busy:
                    {
                        Title = progress.ActionText;
                        ProgressText = $"{progress.CurrentItemCount}/{progress.TotalItemCount} {progress.ItemType}";
                        ProgressValue = progress.Percentage;
                    }
                    break;
                case ProgressState.Error:
                    {
                        ErrorText = progress.ErrorText;
                    }
                    break;
            }

            State = progress.State;
        }

        public string Title
        {
            get => _title;
            private set { _title = value; OnPropertyChanged(); }
        }

        public ProgressState State
        {
            get => _state;
            private set
            {
                if (_state != value)
                {
                    _state = value;
                    OnPropertyChanged();
                    StateChanged?.Invoke(this, _state);
                }
            }
        }

        public string ErrorText
        {
            get => _errorText;
            private set { _errorText = value; OnPropertyChanged(); }
        }

        public int ProgressValue
        {
            get => _progressValue;
            private set { _progressValue = value; OnPropertyChanged(); }
        }

        public string ProgressText
        {
            get => _progressText;
            private set { _progressText = value; OnPropertyChanged(); }
        }
    }
}
