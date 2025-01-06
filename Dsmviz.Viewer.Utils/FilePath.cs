namespace Dsmviz.Viewer.Utils
{
    public static class FilePath
    {
        public static string ResolveFile(string? path, string filename)
        {
            string resolvedFile = filename;
            if (path != null)
            {
                resolvedFile = Resolve(path, filename);
            }

            return resolvedFile;
        }

        public static List<string> ResolveFiles(string path, IEnumerable<string> filenames)
        {
            List<string> resolvedFiles = [];
            foreach (string filename in filenames)
            {
                resolvedFiles.Add(Resolve(path, filename));
            }

            return resolvedFiles;
        }

        private static string Resolve(string path, string filename)
        {
            string absoluteFilename = Path.GetFullPath(filename);
            if (!File.Exists(absoluteFilename))
            {
                absoluteFilename = Path.GetFullPath(Path.Combine(path, filename));
            }

            Logger.LogInfo("Resolve file: path=" + path + " file=" + filename + " as file=" + absoluteFilename);
            return absoluteFilename;
        }
    }
}
