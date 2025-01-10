using System.ComponentModel;

namespace Dsmviz.Interfaces.ViewModel.Sidebar
{
    public interface IModelSideBarViewModel
    {
        void Select();
        void Unselect();
        bool Selected { get; }
        string ModelName { get; }
        string ModelCreatedDate { get; }
        string ModelModifiedDate { get; }
        int ModelVersion { get; }
        string ModelLoadingTimeInMilliseconds { get; }
        int NumberOfElements { get; }
        int NumberOfRelations { get; }
        event PropertyChangedEventHandler? PropertyChanged;
    }
}
