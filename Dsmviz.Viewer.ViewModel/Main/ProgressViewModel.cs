using System.ComponentModel;
using Dsmviz.Interfaces.Util;
using Dsmviz.Interfaces.ViewModel.Main;
using Dsmviz.Viewer.ViewModel.Common;

namespace Dsmviz.Viewer.ViewModel.Main
{
    /// <summary>  
    /// ViewModel for managing the progress of operations in the application.  
    /// This class provides properties and methods to update and notify about   
    /// the current progress state, title, error messages, and progress percentage.  
    /// </summary>  
    public class ProgressViewModel : ViewModelBase, IProgressViewModel
    {
        /// <summary>  
        /// Event triggered when the progress state changes.  
        /// </summary>  
        public event EventHandler<ProgressState>? StateChanged;

        private ProgressState _state = ProgressState.Ready; // Initial state set to Ready  
        private string _title = string.Empty; // Title of the current operation  
        private string _errorText = string.Empty; // Error message, if any  
        private string _progressText = string.Empty; // Text showing current progress  
        private int _progressValue; // Percentage value of the current progress  

        /// <summary>  
        /// Updates the ViewModel with the current progress information.  
        /// This method updates the title, progress text, progress value,  
        /// and error text based on the provided progress information.  
        /// </summary>  
        /// <param name="progress">An object containing progress information.</param>  
        public void Update(IFileProgressInfo progress)
        {
            switch (progress.State)
            {
                case ProgressState.Busy:
                    {
                        Title = progress.ActionText; // Set the title based on the current action  
                        ProgressText = $"{progress.CurrentItemCount}/{progress.TotalItemCount} {progress.ItemType}"; // Update progress text  
                        ProgressValue = progress.Percentage; // Update progress value  
                    }
                    break;

                case ProgressState.Error:
                    {
                        ErrorText = progress.ErrorText; // Set the error text  
                    }
                    break;
            }

            State = progress.State; // Update the overall state  
        }

        /// <summary>  
        /// Gets or sets the title of the current operation.  
        /// </summary>  
        public string Title
        {
            get => _title;
            private set { _title = value; OnPropertyChanged(); } // Notify property change  
        }

        /// <summary>  
        /// Gets or sets the current progress state.  
        /// Raises the StateChanged event when the state changes.  
        /// </summary>  
        public ProgressState State
        {
            get => _state;
            private set
            {
                if (_state != value)
                {
                    _state = value; // Update state  
                    OnPropertyChanged(); // Notify property change  
                    StateChanged?.Invoke(this, _state); // Trigger state changed event  
                }
            }
        }

        /// <summary>  
        /// Gets or sets the error message, if any.  
        /// </summary>  
        public string ErrorText
        {
            get => _errorText;
            private set { _errorText = value; OnPropertyChanged(); } // Notify property change  
        }

        /// <summary>  
        /// Gets or sets the current progress value as a percentage.  
        /// </summary>  
        public int ProgressValue
        {
            get => _progressValue;
            private set { _progressValue = value; OnPropertyChanged(); } // Notify property change  
        }

        /// <summary>  
        /// Gets or sets the text displaying the current progress.  
        /// </summary>  
        public string ProgressText
        {
            get => _progressText;
            private set { _progressText = value; OnPropertyChanged(); } // Notify property change  
        }
    }
}