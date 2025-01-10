using System.ComponentModel;
using System.Windows.Input;
using Dsmviz.ViewModel.Interfaces.Common;

namespace Dsmviz.ViewModel.Interfaces.Main
{
    public interface IMainViewModel : INotifyPropertyChanged
    {
        ICommand ChangeElementParentCommand { get; }

        ViewPerspective SelectedViewPerspective { get; }

        bool IsSearchActive { get; }

        void UpdateCommandStates();
    }
}
