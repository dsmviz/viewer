using Dsmviz.Interfaces.Util;

namespace Dsmviz.Interfaces.Data.Store
{
    public interface IDataStore
    {
        bool LoadModel(string filename, IFileProgress progress);
        bool SaveModel(string filename, bool compressFile, IFileProgress progress);

        int ModelVersion { get; }

        int TotalElementCount { get; }
        int TotalRelationCount { get; }
    }
}
