using Dsmviz.Interfaces.Util;

namespace Dsmviz.Interfaces.Data.Import
{
    public interface IDataImport
    {
        IEnumerable<IImportType> ImportType { get; }

        void Import(string inputFilename, bool clearModel, IFileProgress progress);
    }
}
