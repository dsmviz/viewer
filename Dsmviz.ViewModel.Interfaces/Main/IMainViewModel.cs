using System.ComponentModel;
using System.Windows.Input;
using Dsmviz.Interfaces.ViewModel.Common;

namespace Dsmviz.Interfaces.ViewModel.Main
{
    public interface IMainViewModel : INotifyPropertyChanged
    {
        ICommand ChangeElementParentCommand { get; }

        ViewPerspective SelectedViewPerspective { get; }

        bool IsSearchActive { get; }

        void UpdateCommandStates();
    }
}
