using System.ComponentModel;
using System.Windows.Input;
using Dsmviz.Interfaces.Util;

namespace Dsmviz.Interfaces.ViewModel.Settings
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
