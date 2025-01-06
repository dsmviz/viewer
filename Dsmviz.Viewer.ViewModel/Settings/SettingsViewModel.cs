
using Dsmviz.Viewer.Utils;
using Dsmviz.Viewer.ViewModel.Common;
using System.Windows.Input;

namespace Dsmviz.Viewer.ViewModel.Settings
{
    public class SettingsViewModel : ViewModelBase
    {
        private const string DarkThemeName = "Dark";
        private const string LightThemeName = "Light";

        private LogLevel _logLevel;
        private string? _selectedThemeName;

        private readonly Dictionary<Theme, string> _supportedThemes;

        public SettingsViewModel()
        {
            _supportedThemes = new Dictionary<Theme, string>
            {
                [Theme.Dark] = DarkThemeName,
                [Theme.Light] = LightThemeName
            };

            LogLevel = ViewerSetting.LogLevel;
            SelectedThemeName = _supportedThemes[ViewerSetting.Theme];

            AcceptChangeCommand = RegisterCommand(AcceptChangeExecute, AcceptChangeCanExecute);
        }

        public ICommand AcceptChangeCommand { get; }

        public LogLevel LogLevel
        {
            get => _logLevel;
            set
            {
                _logLevel = value;
                OnPropertyChanged();
            }
        }

        public List<string> SupportedThemeNames => _supportedThemes.Values.ToList();

        public string SelectedThemeName
        {
            get => _selectedThemeName ?? _supportedThemes[0];
            set
            {
                _selectedThemeName = value;
                OnPropertyChanged();
            }
        }

        private void AcceptChangeExecute(object? parameter)
        {
            ViewerSetting.LogLevel = LogLevel;
            ViewerSetting.Theme = _supportedThemes.FirstOrDefault(x => x.Value == SelectedThemeName).Key;
        }

        private bool AcceptChangeCanExecute(object? parameter)
        {
            return true;
        }
    }
}
