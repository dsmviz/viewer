using System.Windows;

namespace Dsmviz.Viewer.View.Resources.Themes
{
    public class ThemeResourceDictionary : ResourceDictionary
    {
        private Uri _lightThemeSource;
        private Uri _darkThemeSource;

        public Uri LightThemeSource
        {
            get => _lightThemeSource;
            set
            {
                _lightThemeSource = value;
                UpdateSource();
            }
        }

        public Uri DarkThemeSource
        {
            get => _darkThemeSource;
            set
            {
                _darkThemeSource = value;
                UpdateSource();
            }
        }

        private void UpdateSource()
        {
            // TODO FIX
            //Uri uri;
            //switch (App.Skin)
            //{
            //    case Theme.Dark:
            //        uri = DarkThemeSource;
            //        break;
            //    case Theme.Light:
            //        uri = LightThemeSource;
            //        break;
            //    default:
            //        uri = LightThemeSource;
            //        break;
            //}

            //if ((uri != null) && (Source != uri))
            //{
            //    Source = uri;
            //}
        }
    }
}
