
using Dsmviz.Interfaces.Util;

namespace Dsmviz.Interfaces.Application.Persistency
{
    public interface IPersistency
    {
        /// <summary>
        /// Info of DSM file
        /// </summary>
        string ModelName { get; }
        DateTime ModelCreatedDate { get; }
        DateTime ModelModifiedDate { get; }

        /// <summary>
        /// Version of DSM file
        /// </summary>
        int ModelVersion { get; }

        /// <summary>
        /// Time required to load DSM file
        /// </summary>
        long ModelLoadingTimeInMilliseconds { get; }
        int TotalElementCount { get; }
        int TotalRelationCount { get; }

        /// <summary>
        /// Event triggered when the model has been modified.
        /// </summary>
        event EventHandler<bool>? Modified;

        /// <summary>
        /// Event triggered when the model has been loaded.
        /// </summary>
        event EventHandler? Loaded;

        /// <summary>
        /// Indicates that the model has been modified.
        /// </summary>
        bool IsModified { get; }
        /// <summary>
        /// Import model file
        /// </summary>
        Task AsyncImportModel(FileInfo fileInfo, IFileProgress progress);
        /// <summary>
        /// Open a dsm model.
        /// </summary>
        Task AsyncOpenDsmModel(FileInfo fileInfo, IFileProgress progress);
        /// <summary>
        /// Save the modified dsm model.
        /// </summary>
        Task AsyncSaveDsmModel(FileInfo fileInfo, IFileProgress progress);
    }
}
