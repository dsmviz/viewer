using Dsmviz.Viewer.ViewModel.Common;
using System.ComponentModel;
using System.Windows.Input;

namespace Dsmviz.Viewer.ViewModel.Main
{
    public interface IMainViewModel : INotifyPropertyChanged
    {
        ICommand ChangeElementParentCommand { get; }

        ViewPerspective SelectedViewPerspective { get; }

        bool IsSearchActive { get; }

        void UpdateCommandStates();
    }
}
