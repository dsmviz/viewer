using System.ComponentModel;
using System.Windows.Input;
using Dsmviz.Viewer.Utils;

namespace Dsmviz.ViewModel.Interfaces.Settings
{
    public interface ISettingsViewModel
    {
        ICommand AcceptChangeCommand { get; }
        LogLevel LogLevel { get; set; }
        List<string> SupportedThemeNames { get; }
        string SelectedThemeName { get; set; }
        event PropertyChangedEventHandler? PropertyChanged;
    }
}
