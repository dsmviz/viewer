using System.ComponentModel;
using Dsmviz.Interfaces.Util;

namespace Dsmviz.ViewModel.Interfaces.Main
{
    public interface IProgressViewModel
    {
        /// <summary>  
        /// Event triggered when the progress state changes.  
        /// </summary>  
        event EventHandler<ProgressState>? StateChanged;

        /// <summary>  
        /// Gets or sets the title of the current operation.  
        /// </summary>  
        string Title
        {
            get;
            // Notify property change  
        }

        /// <summary>  
        /// Gets or sets the current progress state.  
        /// Raises the StateChanged event when the state changes.  
        /// </summary>  
        ProgressState State { get; }

        /// <summary>  
        /// Gets or sets the error message, if any.  
        /// </summary>  
        string ErrorText
        {
            get;
            // Notify property change  
        }

        /// <summary>  
        /// Gets or sets the current progress value as a percentage.  
        /// </summary>  
        int ProgressValue
        {
            get;
            // Notify property change  
        }

        /// <summary>  
        /// Gets or sets the text displaying the current progress.  
        /// </summary>  
        string ProgressText
        {
            get;
            // Notify property change  
        }

        /// <summary>  
        /// Updates the ViewModel with the current progress information.  
        /// This method updates the title, progress text, progress value,  
        /// and error text based on the provided progress information.  
        /// </summary>  
        /// <param name="progress">An object containing progress information.</param>  
        void Update(IFileProgressInfo progress);

        event PropertyChangedEventHandler? PropertyChanged;
    }
}
