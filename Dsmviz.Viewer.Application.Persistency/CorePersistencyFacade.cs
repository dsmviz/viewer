

using Dsmviz.Interfaces.Application.Editing;
using Dsmviz.Interfaces.Application.Persistency;
using Dsmviz.Interfaces.Data.Model.Model;
using Dsmviz.Interfaces.Data.Store;
using Dsmviz.Interfaces.Util;
using Dsmviz.Viewer.Data.Import.Dsi;
using System.Diagnostics;

namespace Dsmviz.Viewer.Application.Persistency
{
    public class CorePersistencyFacade : IPersistency
    {
        private readonly IDataStore _dataStore;
        private readonly IDataModel _dataModel;
        private readonly IActionManagement _actionManagement;

        public event EventHandler<bool>? Modified;
        public event EventHandler? Loaded;

        public CorePersistencyFacade(IDataStore dataStore, IDataModel dataModel, IActionManagement actionManagement)
        {
            _dataStore = dataStore;
            _dataModel = dataModel;
            _actionManagement = actionManagement;
            _actionManagement.ActionPerformed += OnActionPerformed;
        }

        public bool IsModified { get; private set; }

        public string ModelName { get; private set; }
        public DateTime ModelCreatedDate { get; private set; }
        public DateTime ModelModifiedDate { get; private set; }

        public int ModelVersion { get; private set; }

        public long ModelLoadingTimeInMilliseconds { get; private set; }

        public int TotalElementCount => _dataStore.TotalElementCount;
        public int TotalRelationCount => _dataStore.TotalRelationCount;

        public async Task AsyncImportModel(FileInfo fileInfo, IFileProgress progress)
        {
            await Task.Run(() => ImportModel(fileInfo, progress));
        }

        public async Task AsyncOpenDsmModel(FileInfo fileInfo, IFileProgress progress)
        {
            await Task.Run(() => OpenDsmModel(fileInfo, progress));
        }

        public async Task AsyncSaveDsmModel(FileInfo fileInfo, IFileProgress progress)
        {
            await Task.Run(() => SaveDsmModel(fileInfo, true, progress));
        }

        public virtual void ImportModel(FileInfo fileInfo, IFileProgress progress)
        {
            if (fileInfo.Extension == ".dsi")
            {
                ImportDsiModel(fileInfo, progress);
            }
        }

        protected void ImportDsiModel(FileInfo fileInfo, IFileProgress progress)
        {
            string dsmFilename = fileInfo.FullName.Replace(".dsi", ".dsm");
            DsiModel dsiModel = new DsiModel(_dataModel, _dataModel);
            bool success = dsiModel.ImportModel(fileInfo.FullName, true, progress);
            if (success)
            {
                success = _dataStore.SaveModel(dsmFilename, true, progress);
            }

            if (success)
            {
                Loaded?.Invoke(this, EventArgs.Empty);
            }
        }

        public void OpenDsmModel(FileInfo fileInfo, IFileProgress progress)
        {
            Stopwatch watch = Stopwatch.StartNew();
            bool success = _dataStore.LoadModel(fileInfo.FullName, progress);
            watch.Stop();

            if (success)
            {
                ModelName = fileInfo.Name;
                ModelCreatedDate = fileInfo.CreationTime;
                ModelModifiedDate = fileInfo.LastWriteTime;
                ModelVersion = _dataStore.ModelVersion;
                ModelLoadingTimeInMilliseconds = watch.ElapsedMilliseconds;

                TriggerLoadedEvent();
                TriggerModifiedEvent(false);
            }
        }

        public void SaveDsmModel(FileInfo fileInfo, bool compressFile, IFileProgress progress)
        {
            bool success = _dataStore.SaveModel(fileInfo.FullName, compressFile, progress);
            TriggerModifiedEvent(success);
        }

        private void OnActionPerformed(object sender, EventArgs e)
        {
            TriggerModifiedEvent(true);
        }

        private void TriggerLoadedEvent()
        {
            Loaded?.Invoke(this, EventArgs.Empty);
        }

        private void TriggerModifiedEvent(bool modified)
        {
            IsModified = modified;
            Modified?.Invoke(this, IsModified);
        }
    }
}
