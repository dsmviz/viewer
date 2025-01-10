using Dsmviz.Interfaces.Application.Editing;
using Dsmviz.Interfaces.Application.Matrix;
using Dsmviz.Interfaces.Application.Metrics;
using Dsmviz.Interfaces.Application.Persistency;
using Dsmviz.Interfaces.Application.Query;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Viewer.Utils;
using Dsmviz.Viewer.ViewModel.Common;
using Dsmviz.Viewer.ViewModel.Editing.Element;
using Dsmviz.Viewer.ViewModel.Lists.Action;
using Dsmviz.Viewer.ViewModel.Matrix;
using Dsmviz.Viewer.ViewModel.Search;
using Dsmviz.Viewer.ViewModel.Settings;
using System.Windows.Input;
using Dsmviz.Interfaces.ViewModel.Common;
using Dsmviz.Interfaces.ViewModel.Editing.Element;
using Dsmviz.Interfaces.ViewModel.Lists.Action;
using Dsmviz.Interfaces.ViewModel.Main;
using Dsmviz.Interfaces.ViewModel.Matrix;
using Dsmviz.Interfaces.ViewModel.Settings;

namespace Dsmviz.Viewer.ViewModel.Main
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        public event EventHandler<IElementEditViewModel>? ElementEditStarted;

        public event EventHandler<IActionListViewModel>? ActionsVisible;

        public event EventHandler<ISettingsViewModel>? SettingsVisible;

        public event EventHandler? ScreenshotRequested;

        private readonly string _appName;

        private readonly IActionManagement _actionManagement;
        private readonly IElementEditing _elementEditing;
        private readonly IRelationEditing _relationEditing;
        private readonly IElementQuery _elementQuery;
        private readonly IRelationQuery _relationQuery;
        private readonly IPersistency _persistency;

        private string _modelFilename;
        private string _title;

        private bool _isModified;
        private bool _isLoaded;
        private readonly double _minZoom = 0.50;
        private readonly double _maxZoom = 2.00;
        private readonly double _zoomFactor = 1.25;

        private readonly MatrixViewModel _matrixViewModel;
        private readonly ElementSearchViewModel _elementSearchViewModel;
        private readonly ProgressViewModel _progressViewModel;

        // Toolbar
        private string? _redoText;
        private string? _undoText;
        private string _selectedSortAlgorithm;
        private ViewPerspective _selectedViewPerspective;
        // End toolbar

        public MainViewModel(string appName, IEditing editing, IQuery query, IMatrix matrix, IMetrics metrics, IPersistency persistency)
        {
            _appName = appName;

            _actionManagement = editing.ActionManagement;
            _actionManagement.ActionPerformed += OnActionPerformed;

            _elementEditing = editing.ElementEditing;
            _relationEditing = editing.RelationEditing;

            _elementQuery = query.ElementQuery;
            _relationQuery = query.RelationQuery;

            _persistency = persistency;
            _persistency.Modified += OnModelModified;

            _progressViewModel = new ProgressViewModel();

            _matrixViewModel = new MatrixViewModel(this, matrix, metrics, _persistency, editing, query);
            _elementSearchViewModel = new ElementSearchViewModel(_elementQuery, null, null, null, true);
            _elementSearchViewModel.SearchUpdated += OnSearchUpdated;

            // Toolbar commands
            OpenFileCommand = RegisterCommand(OpenFileExecute, OpenFileCanExecute);
            SaveFileCommand = RegisterCommand(SaveFileExecute, SaveFileCanExecute);
            ImportFileCommand = RegisterCommand(ImportFileExecute, ImportFileCanExecute);
            ExportFileCommand = RegisterCommand(ExportFileExecute, ExportFileCanExecute);

            MoveUpElementCommand = RegisterCommand(MoveUpElementExecute, MoveUpElementCanExecute);
            MoveDownElementCommand = RegisterCommand(MoveDownElementExecute, MoveDownElementCanExecute);
            SortAlphabeticalAscendingCommand = RegisterCommand(SortAlphabeticalAscendingExecute, SortAlphabeticalAscendingCanExecute);
            SortAlphabeticalDescendingCommand = RegisterCommand(SortAlphabeticalDescendingExecute, SortAlphabeticalDescendingCanExecute);
            MatrixSortElementCommand = RegisterCommand(MatrixSortElementExecute, MatrixSortElementCanExecute);

            EditCommand = RegisterCommand(EditExecute, EditCanExecute);
            AddCommand = RegisterCommand(AddExecute, AddCanExecute);
            AddChildCommand = RegisterCommand(AddChildExecute, AddChildCanExecute);
            RemoveCommand = RegisterCommand(RemoveExecute, RemoveCanExecute);

            CopyElementCommand = RegisterCommand(CopyElementExecute, CopyElementCanExecute);
            CutElementCommand = RegisterCommand(CutElementExecute, CutElementCanExecute);
            PasteElementCommand = RegisterCommand(PasteElementExecute, PasteElementCanExecute);

            ToggleElementBookmarkCommand = RegisterCommand(ToggleElementBookmarkExecute, ToggleElementBookmarkCanExecute);

            UndoCommand = RegisterCommand(UndoExecute, UndoCanExecute);
            RedoCommand = RegisterCommand(RedoExecute, RedoCanExecute);

            ShowFullMatrixCommand = RegisterCommand(ShowFullMatrixExecute, ShowFullMatrixCanExecute);
            ShowContextMatrixCommand = RegisterCommand(ShowContextMatrixExecute, ShowContextMatrixCanExecute);
            ShowDetailMatrixCommand = RegisterCommand(ShowDetailMatrixExecute, ShowDetailMatrixCanExecute);

            ZoomInCommand = RegisterCommand(ZoomInExecute, ZoomInCanExecute);
            ZoomOutCommand = RegisterCommand(ZoomOutExecute, ZoomOutCanExecute);
            TakeScreenshotCommand = RegisterCommand(TakeScreenshotExecute);

            ShowHistoryCommand = RegisterCommand(ShowHistoryExecute, ShowHistoryCanExecute);
            ShowSettingsCommand = RegisterCommand(ShowSettingsExecute, ShowSettingsCanExecute);

            // Drag and drop commands
            ChangeElementParentCommand = RegisterCommand(ChangeElementParentExecute, ChangeElementParentCanExecute);

            _modelFilename = "";
            _title = _appName;

            _isModified = false;
            _isLoaded = false;

            _selectedSortAlgorithm = SupportedSortAlgorithms[0];

            _selectedViewPerspective = ViewPerspective.Explore;
        }

        private void OnSearchUpdated(object? sender, EventArgs e)
        {
            IsSearchActive = !string.IsNullOrEmpty(ElementSearchViewModel.SearchText);

            MatrixViewModel.ContentChanged();
        }

        private void OnModelModified(object? sender, bool e)
        {
            IsModified = e;
        }

        public IElement? SelectedConsumer => MatrixViewModel.SelectedConsumer;

        public IElement? SelectedProvider => MatrixViewModel.SelectedProvider;

        public IMatrixViewModel MatrixViewModel => _matrixViewModel;

        public ElementSearchViewModel ElementSearchViewModel => _elementSearchViewModel;

        private bool _isMetricsViewExpanded;

        public bool IsMetricsViewExpanded
        {
            get => _isMetricsViewExpanded;
            set { _isMetricsViewExpanded = value; OnPropertyChanged(); }
        }

        private bool _isSearchActive;

        public bool IsSearchActive
        {
            get => _isSearchActive;
            set { _isSearchActive = value; OnPropertyChanged(); }
        }

        public List<string> SupportedSortAlgorithms => _elementEditing.GetSupportedSortAlgorithms().ToList();

        public string SelectedSortAlgorithm
        {
            get => _selectedSortAlgorithm;
            set { _selectedSortAlgorithm = value; OnPropertyChanged(); }
        }

        public List<ViewPerspective> SupportedViewPerspectives => Enum.GetValues(typeof(ViewPerspective)).Cast<ViewPerspective>().ToList();

        public ViewPerspective SelectedViewPerspective
        {
            get => _selectedViewPerspective;
            set
            {
                _selectedViewPerspective = value;
                OnPropertyChanged();
                MatrixViewModel.ContentChanged();
            }
        }

        // Toolbar commands
        public ICommand OpenFileCommand { get; }
        public ICommand SaveFileCommand { get; }
        public ICommand ImportFileCommand { get; }
        public ICommand ExportFileCommand { get; }

        public ICommand MoveUpElementCommand { get; }
        public ICommand MoveDownElementCommand { get; }
        public ICommand SortAlphabeticalAscendingCommand { get; }
        public ICommand SortAlphabeticalDescendingCommand { get; }
        public ICommand MatrixSortElementCommand { get; }

        public ICommand EditCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand AddChildCommand { get; }
        public ICommand RemoveCommand { get; }

        public ICommand CopyElementCommand { get; }
        public ICommand CutElementCommand { get; }
        public ICommand PasteElementCommand { get; }

        public ICommand ToggleElementBookmarkCommand { get; }

        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }
        public ICommand ShowHistoryCommand { get; }

        public ICommand ShowFullMatrixCommand { get; }
        public ICommand ShowContextMatrixCommand { get; }
        public ICommand ShowDetailMatrixCommand { get; }

        public ICommand ZoomInCommand { get; }
        public ICommand ZoomOutCommand { get; }
        public ICommand TakeScreenshotCommand { get; }

        public ICommand ShowSettingsCommand { get; }

        // Drag and drop commands
        public ICommand ChangeElementParentCommand { get; }

        public string ModelFilename
        {
            get => _modelFilename;
            set { _modelFilename = value; OnPropertyChanged(); }
        }

        public bool IsModified
        {
            get => _isModified;
            set { _isModified = value; OnPropertyChanged(); }
        }

        public bool IsLoaded
        {
            get => _isLoaded;
            set { _isLoaded = value; OnPropertyChanged(); }
        }

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        public string UndoText
        {
            get => _undoText ?? string.Empty;
            set { _undoText = value; OnPropertyChanged(); }
        }

        public string RedoText
        {
            get => _redoText ?? string.Empty;
            set { _redoText = value; OnPropertyChanged(); }
        }

        public ProgressViewModel ProgressViewModel => _progressViewModel;

        // Toolbar commands
        private async void OpenFileExecute(object? parameter)
        {
            if (parameter is string filename)
            {
                FileInfo fileInfo = new FileInfo(filename);

                switch (fileInfo.Extension)
                {
                    case ".dsm":
                        {
                            FileProgress progress = new FileProgress(p =>
                            {
                                _progressViewModel.Update(p);
                            });
                            await _persistency.AsyncOpenDsmModel(fileInfo, progress);
                            ModelFilename = fileInfo.FullName;
                            string displayName = Path.GetFileNameWithoutExtension(fileInfo.Name);
                            Title = $"{_appName} - {displayName}";
                            IsLoaded = true;
                        }
                        break;
                    case ".dsi":
                        {
                            FileProgress progress = new FileProgress(p =>
                            {
                                _progressViewModel.Update(p);
                            });
                            await _persistency.AsyncImportModel(fileInfo, progress);

                            FileInfo dsmFileInfo = new FileInfo(fileInfo.FullName.Replace(".dsi", ".dsm"));
                            ModelFilename = dsmFileInfo.FullName;
                            string displayName = Path.GetFileNameWithoutExtension(dsmFileInfo.Name);
                            Title = $"{_appName} - {displayName}";
                            IsLoaded = true;
                        }
                        break;
                }
                _matrixViewModel.ContentChanged();
            }
        }

        private bool OpenFileCanExecute(object? parameter)
        {
            if (parameter is string filename)
            {
                return File.Exists(filename);
            }
            else
            {
                return false;
            }
        }

        private async void SaveFileExecute(object? parameter)
        {
            FileProgress progress = new FileProgress(p =>
            {
                _progressViewModel.Update(p);
            });
            FileInfo fileInfo = new FileInfo(ModelFilename);
            await _persistency.AsyncSaveDsmModel(fileInfo, progress);
        }

        private bool SaveFileCanExecute(object? parameter)
        {
            return _persistency.IsModified;
        }

        private void ImportFileExecute(object? parameter)
        {
            // TODO: Implement
        }

        private bool ImportFileCanExecute(object? parameter)
        {
            // TODO: Implement
            return false;
        }

        private void ExportFileExecute(object? parameter)
        {
            // TODO: Implement
        }

        private bool ExportFileCanExecute(object? parameter)
        {
            // TODO: Implement
            return false;
        }

        private void MoveUpElementExecute(object? parameter)
        {
            if (SelectedProvider != null && SelectedConsumer == null)
            {
                _elementEditing.MoveUp(SelectedProvider);
            }
        }

        private bool MoveUpElementCanExecute(object? parameter)
        {
            if (SelectedProvider != null && SelectedConsumer == null)
            {
                IElement? current = SelectedProvider;
                IElement? previous = _elementEditing.PreviousSibling(current);
                return (current != null) && (previous != null);
            }
            else
            {
                return false;
            }
        }

        private void MoveDownElementExecute(object? parameter)
        {
            if (SelectedProvider != null && SelectedConsumer == null)
            {
                _elementEditing.MoveDown(SelectedProvider);
            }
        }

        private bool MoveDownElementCanExecute(object? parameter)
        {
            if (SelectedProvider != null && SelectedConsumer == null)
            {
                IElement? current = SelectedProvider;
                IElement? next = _elementEditing.NextSibling(current);
                return (current != null) && (next != null);
            }
            else
            {
                return false;
            }
        }

        private void SortAlphabeticalAscendingExecute(object? parameter)
        {
            if (SelectedProvider != null && SelectedConsumer == null)
            {
                _elementEditing.Sort(SelectedProvider, "AlphabeticalAscending");
            }
        }

        private bool SortAlphabeticalAscendingCanExecute(object? parameter)
        {
            if (SelectedProvider != null && SelectedConsumer == null)
            {
                return SelectedProvider.HasChildren;
            }
            else
            {
                return false;
            }
        }

        private void SortAlphabeticalDescendingExecute(object? parameter)
        {
            if (SelectedProvider != null && SelectedConsumer == null)
            {
                _elementEditing.Sort(SelectedProvider, "AlphabeticalDescending");
            }
        }

        private bool SortAlphabeticalDescendingCanExecute(object? parameter)
        {
            if (SelectedProvider != null && SelectedConsumer == null)
            {
                return SelectedProvider.HasChildren;
            }
            else
            {
                return false;
            }
        }

        private void MatrixSortElementExecute(object? parameter)
        {
            if (SelectedProvider != null && SelectedConsumer == null)
            {
                _elementEditing.Sort(SelectedProvider, "Partition");
            }
        }

        private bool MatrixSortElementCanExecute(object? parameter)
        {
            if (SelectedProvider != null && SelectedConsumer == null)
            {
                return SelectedProvider.HasChildren;
            }
            else
            {
                return false;
            }
        }

        private void EditExecute(object? parameter)
        {
            if (SelectedProvider != null && SelectedConsumer == null)
            {
                ElementEditViewModel elementEditViewModel = new ElementEditViewModel(ElementEditViewModelType.Modify,
                    _elementEditing, SelectedProvider);
                ElementEditStarted?.Invoke(this, elementEditViewModel);
            }
        }

        private bool EditCanExecute(object? parameter)
        {
            bool canExecute = false;
            if (SelectedProvider != null && SelectedConsumer == null)
            {
                canExecute = !SelectedProvider.IsRoot;
            }
            return canExecute;
        }

        private void AddExecute(object? parameter)
        {
            if (SelectedProvider != null)
            {
                if (SelectedConsumer == null)
                {
                    // Add sibling of selected element 
                    IElement? parent = SelectedProvider.Parent;
                    if (parent != null)
                    {
                        string temporarilyName = $"$new_{parent.Children.Count}";
                        int indexAfterSelectedElement = parent.IndexOfChild(SelectedProvider) + 1;
                        _elementEditing.CreateElement(temporarilyName, "", parent, indexAfterSelectedElement);
                    }
                }
                else
                {
                    // Add relation between consumer and provider with weight 1
                    _relationEditing.CreateRelation(SelectedConsumer, SelectedProvider, "", 1);
                }
            }
        }

        private bool AddCanExecute(object? parameter)
        {
            return SelectedProvider != null;
        }

        private void AddChildExecute(object? parameter)
        {
            if (SelectedProvider != null && SelectedConsumer == null)
            {
                SelectedProvider.IsExpanded = true;

                // Add child to selected element 
                string temporarilyName = $"$new_{SelectedProvider.Children.Count}";
                _elementEditing.CreateElement(temporarilyName, "", SelectedProvider, 0);
            }
        }

        private bool AddChildCanExecute(object? parameter)
        {
            return SelectedProvider != null && (SelectedProvider.IsExpanded || !SelectedProvider.HasChildren);
        }

        private void RemoveExecute(object? parameter)
        {
            if (SelectedProvider != null)
            {
                if (SelectedConsumer == null)
                {
                    // Remove selected element
                    _elementEditing.DeleteElement(SelectedProvider);
                }
                else
                {
                    // Remove all relations of selected cell
                    IEnumerable<IRelation> relationsOfCell =
                        _relationQuery.GetAllRelations(SelectedConsumer, SelectedConsumer);

                    // TODO FIX: remove relations(s)
                    //if (relationsOfCell.Count() == 1)
                    //{
                    //    IRelation? relation = relationsOfCell.First();
                    //    if (relation != null)
                    //    {
                    //        _relationEditing.DeleteRelation(relation);
                    //    }
                    //}
                }
            }
        }

        private bool RemoveCanExecute(object? parameter)
        {
            bool canExecute = false;
            if (SelectedProvider != null)
            {
                canExecute = !SelectedProvider.IsRoot;
            }
            return canExecute;
        }

        private void CopyElementExecute(object? parameter)
        {
            if (SelectedProvider != null && SelectedConsumer == null)
            {
                _elementEditing.CopyElement(SelectedProvider);
            }
        }

        private bool CopyElementCanExecute(object? parameter)
        {
            bool canExecute = false;
            if (SelectedProvider != null && SelectedConsumer == null)
            {
                // TODO: Currently only copying leaf elements is allowed
                canExecute = !SelectedProvider.IsRoot && !SelectedProvider.HasChildren;
            }
            return canExecute;
        }

        private void CutElementExecute(object? parameter)
        {
            if (SelectedProvider != null && SelectedConsumer == null)
            {
                _elementEditing.CutElement(SelectedProvider);
            }
        }

        private bool CutElementCanExecute(object? parameter)
        {
            bool canExecute = false;
            if (SelectedProvider != null && SelectedConsumer == null)
            {
                // TODO: Currently only cutting leaf elements is allowed
                canExecute = !SelectedProvider.IsRoot && !SelectedProvider.HasChildren;
            }
            return canExecute;
        }

        private void PasteElementExecute(object? parameter)
        {
            if (SelectedProvider != null && SelectedConsumer == null)
            {
                _elementEditing.PasteElement(SelectedProvider, 0);
            }
        }

        private bool PasteElementCanExecute(object? parameter)
        {
            bool canExecute = false;
            if (SelectedProvider != null && SelectedConsumer == null)
            {
                canExecute = !SelectedProvider.IsRoot && _elementEditing.IsElementOnClipboard();
            }
            return canExecute;
        }

        private void ToggleElementBookmarkExecute(object? parameter)
        {
            if (_selectedViewPerspective == ViewPerspective.Bookmarks && SelectedProvider != null && SelectedConsumer == null)
            {
                SelectedProvider.IsBookmarked = !SelectedProvider.IsBookmarked;
            }
        }

        private bool ToggleElementBookmarkCanExecute(object? parameter)
        {
            return _selectedViewPerspective == ViewPerspective.Bookmarks && SelectedProvider != null && SelectedConsumer == null;
        }

        private void UndoExecute(object? parameter)
        {
            _actionManagement.Undo();
        }

        private bool UndoCanExecute(object? parameter)
        {
            return _actionManagement.CanUndo();
        }

        private void RedoExecute(object? parameter)
        {
            _actionManagement.Redo();
        }

        private bool RedoCanExecute(object? parameter)
        {
            return _actionManagement.CanRedo();
        }

        private void OnActionPerformed(object? sender, EventArgs e)
        {
            UndoText = $"Undo latest action: {_actionManagement.GetUndoActionDescription()}";
            RedoText = $"Redo latest undo: {_actionManagement.GetRedoActionDescription()}";
            MatrixViewModel.ContentChanged();
            NotifyCommandsCanExecuteChanged();
        }

        public void UpdateCommandStates()
        {
            NotifyCommandsCanExecuteChanged();
        }

        private void ShowFullMatrixExecute(object? parameter)
        {
            IncludeAllInTree();
            _matrixViewModel.ContentChanged();
        }

        private bool ShowFullMatrixCanExecute(object? parameter)
        {
            return IsLoaded;
        }

        private void ShowContextMatrixExecute(object? parameter)
        {
            if (SelectedProvider != null)
            {
                if (SelectedConsumer != null)
                {
                    ExcludeAllFromTree();
                    IncludeInTree(SelectedProvider);
                    IncludeInTree(SelectedConsumer);
                }
                else
                {
                    ExcludeAllFromTree();
                    IncludeInTree(SelectedProvider);

                    foreach (IElement consumer in _relationQuery.GetElementConsumers(SelectedProvider))
                    {
                        IncludeInTree(consumer);
                    }

                    foreach (IElement provider in _relationQuery.GetElementProviders(SelectedProvider))
                    {
                        IncludeInTree(provider);
                    }
                }
                _matrixViewModel.ContentChanged();
            }
        }

        private bool ShowContextMatrixCanExecute(object? parameter)
        {
            return SelectedProvider != null;
        }

        private void ShowDetailMatrixExecute(object? parameter)
        {
            if (SelectedProvider != null && SelectedConsumer == null)
            {
                ExcludeAllFromTree();
                IncludeInTree(SelectedProvider);
                _matrixViewModel.ContentChanged();
            }
        }

        private bool ShowDetailMatrixCanExecute(object? parameter)
        {
            return SelectedProvider != null && SelectedConsumer == null;
        }

        private void ZoomInExecute(object? parameter)
        {
            MatrixViewModel.ZoomLevel *= _zoomFactor;
        }

        private bool ZoomInCanExecute(object? parameter)
        {
            return MatrixViewModel.ZoomLevel < _maxZoom;
        }

        private void ZoomOutExecute(object? parameter)
        {
            MatrixViewModel.ZoomLevel /= _zoomFactor;
        }

        private bool ZoomOutCanExecute(object? parameter)
        {
            return MatrixViewModel.ZoomLevel > _minZoom;
        }

        private void TakeScreenshotExecute(object? parameter)
        {
            ScreenshotRequested?.Invoke(this, EventArgs.Empty);
        }

        private void ShowHistoryExecute(object? parameter)
        {
            ActionListViewModel viewModelType = new ActionListViewModel(_actionManagement);
            ActionsVisible?.Invoke(this, viewModelType);
        }

        private bool ShowHistoryCanExecute(object? parameter)
        {
            return true;
        }

        private void ShowSettingsExecute(object? parameter)
        {
            SettingsViewModel viewModel = new SettingsViewModel();
            SettingsVisible?.Invoke(this, viewModel);
        }

        private bool ShowSettingsCanExecute(object? parameter)
        {
            return true;
        }

        // Drag and drop command
        private void ChangeElementParentExecute(object? parameter)
        {
            Tuple<IElement, IElement, int> moveParameter = parameter as Tuple<IElement, IElement, int>;
            if (moveParameter != null)
            {
                _elementEditing.ChangeElementParent(moveParameter.Item1, moveParameter.Item2, moveParameter.Item3);
            }
        }

        private bool ChangeElementParentCanExecute(object? parameter)
        {
            bool canExecute = false;
            if (SelectedProvider != null)
            {
                canExecute = !SelectedProvider.IsRoot;
            }
            return canExecute;
        }

        private void ExcludeAllFromTree()
        {
            UpdateChildrenIncludeInTree(_elementQuery.RootElement, false);
        }

        private void IncludeAllInTree()
        {
            UpdateChildrenIncludeInTree(_elementQuery.RootElement, true);
        }

        private void IncludeInTree(IElement element)
        {
            UpdateChildrenIncludeInTree(element, true);
            UpdateParentsIncludeInTree(element, true);
        }

        private void UpdateChildrenIncludeInTree(IElement element, bool included)
        {
            element.IsIncludedInTree = included;

            foreach (IElement child in element.ChildrenIncludingDeletedOnes)
            {
                UpdateChildrenIncludeInTree(child, included);
            }
        }

        private void UpdateParentsIncludeInTree(IElement element, bool included)
        {
            IElement? current = element;
            do
            {
                current.IsIncludedInTree = included;
                current = current.Parent;
            } while (current != null);
        }
    }
}
