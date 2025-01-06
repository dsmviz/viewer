using Dsmviz.Interfaces.Data.Model.Model;
using Dsmviz.Interfaces.Data.Store;
using Dsmviz.Interfaces.Util;
using Dsmviz.Viewer.Utils;

namespace Dsmviz.Viewer.Data.Store
{
    public class CoreDataStore(IModelPersistency modelPersistency) : IDataStore
    {
        public int TotalElementCount => modelPersistency.ElementModelPersistency.GetPersistedElementCount();
        public int TotalRelationCount => modelPersistency.RelationModelPersistency.GetPersistedRelationCount();

        public bool LoadModel(string filename, IFileProgress progress)
        {
            Logger.LogDataModelMessage($"Load data model file={filename}");

            ModelFilename = filename;

            modelPersistency.Clear();
            CoreDsmFile dsmModelFile = new CoreDsmFile(filename, modelPersistency);
            bool result = dsmModelFile.Load(progress);
            IsCompressed = dsmModelFile.IsCompressedFile;
            ModelVersion = modelPersistency.ModelVersion;
            return result;
        }

        public bool SaveModel(string filename, bool compressFile, IFileProgress progress)
        {
            Logger.LogDataModelMessage($"Save data model file={filename} compress={compressFile}");

            ModelFilename = filename;

            CoreDsmFile dsmModelFile = new CoreDsmFile(filename, modelPersistency);
            bool result = dsmModelFile.Save(compressFile, progress);
            return result;
        }

        public string ModelFilename { get; private set; } = string.Empty;
        public int ModelVersion { get; private set; }

        public bool IsCompressed { get; private set; }

        public string FileExtension => ".dsm";
        public string FileDescription => "DSM model";
    }
}
