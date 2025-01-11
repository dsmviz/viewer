using System.ComponentModel;
using System.Windows.Input;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.ViewModel.Common;
using Dsmviz.Interfaces.ViewModel.Editing.Element;
using Dsmviz.Interfaces.ViewModel.Lists.Action;
using Dsmviz.Interfaces.ViewModel.Matrix;
using Dsmviz.Interfaces.ViewModel.Search;
using Dsmviz.Interfaces.ViewModel.Settings;

namespace Dsmviz.Interfaces.ViewModel.Main
{
    public interface IMainViewModel
    {
        event EventHandler<IElementEditViewModel>? ElementEditStarted;
        event EventHandler<IActionListViewModel>? ActionsVisible;
        event EventHandler<ISettingsViewModel>? SettingsVisible;
        event EventHandler? ScreenshotRequested;
        IElement? SelectedConsumer { get; }
        IElement? SelectedProvider { get; }
        IMatrixViewModel MatrixViewModel { get; }
        IElementSearchViewModel ElementSearchViewModel { get; }
        bool IsMetricsViewExpanded { get; set; }
        bool IsSearchActive { get; set; }
        List<string> SupportedSortAlgorithms { get; }
        string SelectedSortAlgorithm { get; set; }
        List<ViewPerspective> SupportedViewPerspectives { get; }
        ViewPerspective SelectedViewPerspective { get; set; }
        ICommand OpenFileCommand { get; }
        ICommand SaveFileCommand { get; }
        ICommand ImportFileCommand { get; }
        ICommand ExportFileCommand { get; }
        ICommand MoveUpElementCommand { get; }
        ICommand MoveDownElementCommand { get; }
        ICommand SortAlphabeticalAscendingCommand { get; }
        ICommand SortAlphabeticalDescendingCommand { get; }
        ICommand MatrixSortElementCommand { get; }
        ICommand EditCommand { get; }
        ICommand AddCommand { get; }
        ICommand AddChildCommand { get; }
        ICommand RemoveCommand { get; }
        ICommand CopyElementCommand { get; }
        ICommand CutElementCommand { get; }
        ICommand PasteElementCommand { get; }
        ICommand ToggleElementBookmarkCommand { get; }
        ICommand UndoCommand { get; }
        ICommand RedoCommand { get; }
        ICommand ShowHistoryCommand { get; }
        ICommand ShowFullMatrixCommand { get; }
        ICommand ShowContextMatrixCommand { get; }
        ICommand ShowDetailMatrixCommand { get; }
        ICommand ZoomInCommand { get; }
        ICommand ZoomOutCommand { get; }
        ICommand TakeScreenshotCommand { get; }
        ICommand ShowSettingsCommand { get; }
        ICommand ChangeElementParentCommand { get; }
        string ModelFilename { get; set; }
        bool IsModified { get; set; }
        bool IsLoaded { get; set; }
        string Title { get; set; }
        string UndoText { get; set; }
        string RedoText { get; set; }
        IProgressViewModel ProgressViewModel { get; }
        void UpdateCommandStates();
        event PropertyChangedEventHandler? PropertyChanged;
    }
}
