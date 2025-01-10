

using System.ComponentModel;
using Dsmviz.Interfaces.Application.Persistency;
using Dsmviz.Interfaces.ViewModel.Sidebar;
using Dsmviz.Viewer.ViewModel.Common;

namespace Dsmviz.Viewer.ViewModel.SideBar
{
    public class ModelSideBarViewModel(IPersistency persistency) : ViewModelBase, IModelSideBarViewModel
    {
        // Interfaces

        // Selection
        private bool _selected;

        // Properties
        private string? _modelName;
        private string? _modelCreatedDate;
        private string? _modelModifiedDate;
        private int? _modelVersion;
        private string? _modelLoadingTimeInMilliseconds;

        private int? _numberOfElements;
        private int? _numberOfRelations;

        public void Select()
        {
            Selected = true;
        }

        public void Unselect()
        {
            Selected = false;
        }

        public bool Selected
        {
            get => _selected;
            private set { _selected = value; OnPropertyChanged(); Update(); }
        }

        private void Update()
        {
            if (_selected)
            {
                ModelName = persistency.ModelName;
                ModelCreatedDate = persistency.ModelCreatedDate.ToString("yyyy-MM-dd HH:mm:ss");
                ModelModifiedDate = persistency.ModelModifiedDate.ToString("yyyy-MM-dd HH:mm:ss");
                ModelVersion = persistency.ModelVersion;
                ModelLoadingTimeInMilliseconds = persistency.ModelLoadingTimeInMilliseconds.ToString() + "ms";

                NumberOfElements = persistency.TotalElementCount;
                NumberOfRelations = persistency.TotalRelationCount;
            }
        }

        public string ModelName
        {
            get => _modelName ?? string.Empty;
            private set { _modelName = value; OnPropertyChanged(); }
        }

        public string ModelCreatedDate
        {
            get => _modelCreatedDate ?? string.Empty;
            private set { _modelCreatedDate = value; OnPropertyChanged(); }
        }

        public string ModelModifiedDate
        {
            get => _modelModifiedDate ?? string.Empty;
            private set { _modelModifiedDate = value; OnPropertyChanged(); }
        }

        public int ModelVersion
        {
            get => _modelVersion ?? 0;
            private set { _modelVersion = value; OnPropertyChanged(); }
        }

        public string ModelLoadingTimeInMilliseconds
        {
            get => _modelLoadingTimeInMilliseconds ?? string.Empty;
            private set { _modelLoadingTimeInMilliseconds = value; OnPropertyChanged(); }
        }

        public int NumberOfElements
        {
            get => _numberOfElements ?? 0;
            private set { _numberOfElements = value; OnPropertyChanged(); }
        }

        public int NumberOfRelations
        {
            get => _numberOfRelations ?? 0;
            private set { _numberOfRelations = value; OnPropertyChanged(); }
        }
    }
}
