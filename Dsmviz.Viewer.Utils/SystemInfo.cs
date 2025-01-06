using System.Reflection;

namespace Dsmviz.Viewer.Utils
{
    public class SystemInfo
    {
        public static string GetExecutableInfo(Assembly assembly)
        {
            string name = assembly.GetName().Name ?? string.Empty;
            string version = assembly.GetName().Version?.ToString() ?? string.Empty;
            DateTime buildDate = new FileInfo(assembly.Location).LastWriteTime;
            return $"{name} version={version} build={buildDate}";
        }
    }
}
