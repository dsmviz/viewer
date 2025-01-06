namespace Dsmviz.Interfaces.Data.Import
{
    public interface IImportType
    {
        string FileExtension { get; }
        string FileDescription { get; }
    }
}
