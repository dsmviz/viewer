
using Dsmviz.Viewer.Utils;
using System.Xml.Serialization;
using System.Xml;
using Dsmviz.Interfaces.Util;

namespace Dsmviz.Viewer.ViewModel.Settings
{
    public static class ViewerSettingStore
    {
        private static readonly string ApplicationSettingsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Dsmviz");
        private static readonly string SettingsFilePath = Path.Combine(ApplicationSettingsFolder, "ViewerSettings.xml");

        private static ViewerSettingsData _viewerSettings = ViewerSettingsData.CreateDefault();

        public static void Read()
        {
            if (!Directory.Exists(ApplicationSettingsFolder))
            {
                Directory.CreateDirectory(ApplicationSettingsFolder);
            }

            FileInfo settingsFileInfo = new FileInfo(SettingsFilePath);
            if (!settingsFileInfo.Exists)
            {
                WriteToFile(SettingsFilePath, _viewerSettings);
            }
            else
            {
                _viewerSettings = ReadFromFile(settingsFileInfo.FullName);
            }
        }

        public static LogLevel LogLevel
        {
            set => _viewerSettings.LogLevel = value;
            get => _viewerSettings.LogLevel;
        }

        public static Theme Theme
        {
            set => _viewerSettings.Theme = value;
            get => _viewerSettings.Theme;
        }

        public static void Write()
        {
            WriteToFile(SettingsFilePath, _viewerSettings);
        }

        private static void WriteToFile(string filename, ViewerSettingsData viewerSettings)
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings() { Indent = true };
            XmlSerializer serializer = new XmlSerializer(typeof(ViewerSettingsData));

            using XmlWriter xmlWriter = XmlWriter.Create(filename, xmlWriterSettings);
            serializer.Serialize(xmlWriter, viewerSettings);
        }

        private static ViewerSettingsData? ReadFromFile(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ViewerSettingsData));

            using XmlReader reader = XmlReader.Create(filename);
            return (ViewerSettingsData?)serializer.Deserialize(reader);
        }
    }
}
